using AutoMapper;
using MinsaitToDDL.Lib.Models;
using MinsaitToDDL.Lib.Models.Minsat.Common;
using MinsaitToDDL.Lib.Models.Minsat.Invoice;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace MinsaitToDDL.Lib
{
    /// <summary>
    /// Class responsible for parsing Minsait documents and mapping them to ItemTransaction objects.
    /// </summary>
    public class MinsaitInvoiceMapper
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
                    // ============================
                    // HEADER
                    // ============================
                    .ForPath(d => d.InvoiceHeader.InvoiceNumber,
                        o => o.MapFrom(s => s.ISignableTransactionTransactionID))

                    .ForPath(d => d.InvoiceHeader.InvoiceDate,
                        o => o.MapFrom(s => s.CreateDate))

                    .ForPath(d => d.InvoiceHeader.BuyerInformation,
                        o => o.MapFrom(s => MapPartyReverse(s.Party)))

                    .ForPath(d => d.InvoiceHeader.SellerInformation,
                        o => o.MapFrom(s => MapPartyReverse(
                            s.SupplierParty ?? s.Party)))

                    // ============================
                    // DETAIL (LINES)  ❗ REQUIRED
                    // ============================
                    .ForPath(d => d.InvoiceDetail.Items,
                        o => o.MapFrom(s => MapInvoiceLinesReverse(s.Details)))

                    // ============================
                    // SUMMARY (TOTALS) ❗ REQUIRED
                    // ============================
                    .ForPath(d => d.InvoiceSummary.InvoiceTotals,
                        o => o.MapFrom(s => MapInvoiceTotalsReverse(s)))

                    .ForAllOtherMembers(o => o.Ignore());
            });
        }


        #endregion

        #region Helpers

        private static Models.Party MapParty(Models.Minsat.Common.Party party)
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

        private static MinsaitToDDL.Lib.Models.Minsat.Common.Party MapPartyReverse(Models.Party party)
        {
            if (party == null) return null;

            return new MinsaitToDDL.Lib.Models.Minsat.Common.Party
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

        private static List<InvoiceItemDetail> MapInvoiceLinesReverse(
            IEnumerable<Detail> details)
        {
            var lines = new List<InvoiceItemDetail>();
            if (details == null)
                return lines;

            int lineNo = 1;

            foreach (var d in details)
            {
                var line = new InvoiceItemDetail
                {
                    LineItemNum = lineNo++,

                    // Article
                    StandardPartNumber = d.ItemID,
                    ItemDescription = d.Description,

                    // Quantity
                    Quantity = new Quantity
                    {
                        QuantityValue = (decimal)(d.Quantity ?? 0),
                        UnitOfMeasurement = "UN"
                    },

                    // Net line amount
                    MonetaryAmount = (decimal)(d.TotalNetAmount ?? 0)
                };

                lines.Add(line);
            }

            return lines;
        }

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

        private static InvoiceTotals MapInvoiceTotalsReverse(ItemTransaction t)
        {
            return new InvoiceTotals
            {
                //NumberOfLines = t.Details?.Count ?? 0,
                NetValue = (decimal)(t.TotalAmount ?? 0),
                //GrossValue = (decimal)(t.TotalTransactionAmount ?? 0),
                TotalAmountPayable = (decimal)(t.TotalTransactionAmount ?? 0),
                TotalTaxAmount = (decimal)(t.TotalTaxAmount ?? 0)
            };
        }



        #endregion
    }
}