using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using AutoMapper;
using Newtonsoft.Json;
using MinsaitToDDL.Lib.Models;
using MinsaitToDDL.Lib.Models.Minsait;

namespace MinsaitToDDL.Lib
{
    /// <summary>
    /// Class responsible for parsing Minsait documents and mapping them to ItemTransaction objects.
    /// </summary>
    public class MinsaitToDDL
    {
        #region Enums

        public enum DocumentType
        {
            Invoice = 1,
            Order = 2,
            CreditNote = 4
        }

        #endregion

        #region Properties

        /// <summary>
        /// Parsed Minsait invoice
        /// </summary>
        public Invoice ItemTransactionMinsait { get; set; }

        /// <summary>
        /// Parsed DDL transaction
        /// </summary>
        public ItemTransaction ItemTransaction { get; set; }

        public DocumentType ItemTransactionMinsaitDocumentType { get; set; } = DocumentType.Invoice;

        #endregion

        #region Public API

        public ItemTransaction ParseFromFile(string file)
        {
            return Parse(File.ReadAllText(file));
        }

        public ItemTransaction Parse(string fileContent)
        {
            XDocument doc = XDocument.Parse(fileContent);
            XElement root = doc.Root;

            string rootName = root.Name.LocalName;

            var serializer = new XmlSerializer(typeof(Invoice), new XmlRootAttribute(rootName));

            Invoice document;
            using (var reader = new StringReader(fileContent))
            {
                document = (Invoice)serializer.Deserialize(reader);
            }

            ItemTransactionMinsait = document;

            var config = MapConfig();
            ItemTransaction = config.CreateMapper().Map<ItemTransaction>(document);

            return ItemTransaction;
        }

        public static Invoice MapFromDdl(ItemTransaction itemTransaction)
        {
            var config = MapConfig();
            return config.CreateMapper().Map<Invoice>(itemTransaction);
        }

        public static Invoice MapFromDdlString(string itemTransactionJson)
        {
            return MapFromDdl(JsonConvert.DeserializeObject<ItemTransaction>(itemTransactionJson));
        }

        public static string SerializeInvoiceToXml(Invoice invoice)
        {
            var serializer = new XmlSerializer(typeof(Invoice));

            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, invoice);
                return writer.ToString();
            }
        }

        #endregion

        #region Mapping Configuration

        private static MapperConfiguration MapConfig()
        {
            return new MapperConfiguration(cfg =>
            {
                // ============================
                // Minsait ➜ DDL
                // ============================
                cfg.CreateMap<Invoice, ItemTransaction>()
                    .ForMember(d => d.CreateDate,
                        o => o.MapFrom(s => s.InvoiceHeader.InvoiceDate))
                    .ForMember(d => d.DeferredPaymentDate,
                        o => o.MapFrom(s =>
                            s.InvoiceHeader.OtherInvoiceDates != null
                                ? s.InvoiceHeader.OtherInvoiceDates.InvoiceDueDate
                                : (DateTime?)null))
                    .ForMember(d => d.ISignableTransactionTransactionID,
                        o => o.MapFrom(s => s.InvoiceHeader.InvoiceNumber))
                    .ForMember(d => d.TotalAmount,
                        o => o.MapFrom(s => s.InvoiceSummary.InvoiceTotals.NetValue))
                    .ForMember(d => d.TotalTaxAmount,
                        o => o.MapFrom(s => s.InvoiceSummary.InvoiceTotals.TotalTaxAmount))
                    .ForMember(d => d.TotalTransactionAmount,
                        o => o.MapFrom(s => s.InvoiceSummary.InvoiceTotals.TotalAmountPayable))
                    .ForPath(d => d.Party,
                        o => o.MapFrom(s => MapParty(s.InvoiceHeader.BuyerInformation)))
                    .ForPath(d => d.SupplierParty,
                        o => o.MapFrom(s => MapParty(s.InvoiceHeader.SellerInformation)))
                    .ForPath(d => d.Details,
                        o => o.MapFrom(s => MapInvoiceLines(
                            s.InvoiceDetail != null
                                ? s.InvoiceDetail.Items
                                : null)))
                    .ForPath(d => d.Taxes,
                        o => o.MapFrom(s => MapSummaryTaxes(
                            s.InvoiceSummary.SummaryTaxes)))
                    .ForAllOtherMembers(o => o.Ignore());

                // ============================
                // DDL ➜ Minsait (mínimo viável)
                // ============================
                cfg.CreateMap<ItemTransaction, Invoice>()
                    .ForPath(d => d.InvoiceHeader.InvoiceNumber,
                        o => o.MapFrom(s => s.ISignableTransactionTransactionID))
                    .ForPath(d => d.InvoiceHeader.InvoiceDate,
                        o => o.MapFrom(s => s.CreateDate))
                    .ForPath(d => d.InvoiceHeader.BuyerInformation,
                        o => o.MapFrom(s => MapPartyReverse(s.Party)))
                    .ForPath(d => d.InvoiceHeader.SellerInformation,
                        o => o.MapFrom(s => MapPartyReverse(
                            s.SupplierParty ?? s.Party)))
                    .ForAllOtherMembers(o => o.Ignore());
            });
        }


        #endregion

        #region Helpers

        private static Models.Party MapParty(Models.Minsait.Party party)
        {
            if (party == null) return null;

            return new Models.Party
            {
                FederalTaxID = party.NIF,
                OrganizationName = party.Name,
                AddressLine1 = party.Street,
                PostalCode = party.PostalCode,
                CountryID = party.Country
            };
        }

        private static Models.Minsait.Party MapPartyReverse(Models.Party party)
        {
            if (party == null) return null;

            return new Models.Minsait.Party
            {
                NIF = party.FederalTaxID,
                Name = party.OrganizationName,
                Street = party.AddressLine1,
                PostalCode = party.PostalCode,
                Country = party.CountryID
            };
        }

        //private static UnloadPlaceAddress MapUnloadPlaceAddress(Models.Minsait.Party party)
        //{
        //    if (party == null) return null;

        //    return new UnloadPlaceAddress
        //    {
        //        AddressLine1 = party.Street,
        //        PostalCode = party.PostalCode,
        //        CountryID = party.Country
        //    };
        //}

        //private static Models.Minsait.Party MapUnloadPlaceAddressReverse(UnloadPlaceAddress address)
        //{
        //    if (address == null) return null;

        //    return new Models.Minsait.Party
        //    {
        //        Street = address.AddressLine1,
        //        PostalCode = address.PostalCode,
        //        Country = address.CountryID
        //    };
        //}

        private static List<Detail> MapInvoiceLines(
            IEnumerable<InvoiceItemDetail> lines)
        {
            var details = new List<Detail>();
            if (lines == null) return details;

            foreach (var line in lines)
            {
                details.Add(new Detail
                {
                    ItemID = line.StandardPartNumber,
                    Description = line.ItemDescription,
                    Quantity = (double)line.Quantity.QuantityValue,
                    UnitPrice = line.Quantity.QuantityValue != 0
                        ? (double)(line.MonetaryAmount / line.Quantity.QuantityValue)
                        : 0,
                    TotalNetAmount = (double)line.MonetaryAmount
                });
            }

            return details;
        }

        //private static List<LineItem> MapInvoiceLinesReverse(IEnumerable<Detail> details)
        //{
        //    var lines = new List<LineItem>();
        //    int lineNo = 1;

        //    if (details == null) return lines;

        //    foreach (var d in details)
        //    {
        //        var quantity = (decimal)(d.Quantity ?? 0);

        //        var line = new LineItem
        //        {
        //            Number = lineNo++,
        //            TradeItemIdentification = d.ItemID,
        //            ItemDescription = d.Description,

        //            Quantity = new Quantity
        //            {
        //                QuantityValue = quantity,
        //                UnitOfMeasurement = "UN" // ou deixa null se não for obrigatório
        //            },

        //            NetLineAmount = (decimal)(d.TotalNetAmount ?? 0),

        //            // cálculo seguro do preço unitário
        //            NetPrice = quantity != 0
        //                ? (decimal)((d.TotalNetAmount ?? 0) / (double)quantity)
        //                : 0
        //        };

        //        var taxRate = d.TaxList != null
        //            ? d.TaxList.FirstOrDefault()?.TaxRate
        //            : null;

        //        if (taxRate.HasValue)
        //        {
        //            line.LineVat = new LineVat
        //            {
        //                TaxPercentage = (decimal)taxRate.Value,
        //                TaxableAmount = (decimal)(d.TotalNetAmount ?? 0),
        //                TaxTotalValue = (decimal)(d.TotalTaxAmount ?? 0)
        //            };
        //        }

        //        lines.Add(line);
        //    }

        //    return lines;
        //}

        //private static List<TaxValue> MapTaxValues(IEnumerable<VatSummary> vats)
        //{
        //    var list = new List<TaxValue>();
        //    if (vats == null) return list;

        //    foreach (var v in vats)
        //    {
        //        list.Add(new TaxValue
        //        {
        //            TaxRate = (double)v.TaxPercentage,
        //            TotalTaxAmount = (double)v.TaxTotalValue,
        //            TotalNetChargeableAmount = (double)v.TaxableAmount
        //        });
        //    }

        //    return list;
        //}

        //private static List<VatSummary> MapTaxValuesReverse(IEnumerable<TaxValue> taxes)
        //{
        //    var list = new List<VatSummary>();
        //    if (taxes == null) return list;

        //    foreach (var t in taxes)
        //    {
        //        list.Add(new VatSummary
        //        {
        //            TaxPercentage = (decimal)t.TaxRate,
        //            TaxTotalValue = (decimal)t.TotalTaxAmount,
        //            TaxableAmount = (decimal)t.TotalNetChargeableAmount
        //        });
        //    }

        //    return list;
        //}

        private static List<TaxValue> MapSummaryTaxes(
            IEnumerable<SummaryTax> taxes)
        {
            var list = new List<TaxValue>();
            if (taxes == null) return list;

            foreach (var t in taxes)
            {
                list.Add(new TaxValue
                {
                    TaxRate = (double)t.TaxPercent,
                    TotalTaxAmount = (double)t.TaxAmount,
                    TotalNetChargeableAmount = (double)t.TaxableAmount
                });
            }

            return list;
        }


        #endregion
    }
}