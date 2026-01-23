using AutoMapper;
using MinsaitToDDL.Lib.Interfaces;
using MinsaitToDDL.Lib.Models;
using MinsatToDDL.Lib.Models.Minsat.Order;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace MinsaitToDDL.Lib.Parsers
{
    public class MinsaitOrderParser : IMinsaitDocumentParser
    {
        public bool CanParse(XElement root)
        {
            // Ajusta se o root tiver outro nome
            return root.Name.LocalName == "Order";
        }

        public ItemTransaction Parse(string xml)
        {
            var serializer = new XmlSerializer(typeof(Order));
            Order document;

            using (var reader = new StringReader(xml))
            {
                document = (Order)serializer.Deserialize(reader);
            }

            var mapper = CreateMapper();
            return mapper.Map<ItemTransaction>(document);
        }

        public string ParseFromDdl(ItemTransaction transaction)
        {
            var mapper = CreateMapper();
            var document = mapper.Map<Order>(transaction);

            var serializer = new XmlSerializer(typeof(Order));
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, document);
                return writer.ToString();
            }
        }

        private static IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Order, ItemTransaction>()
                    .ForMember(d => d.CreateDate,
                        o => o.MapFrom(s => s.OrderHeader.OrderDate))
                    .ForMember(d => d.ISignableTransactionTransactionID,
                        o => o.MapFrom(s => s.OrderHeader.OrderNumber))
                    .ForMember(d => d.TotalAmount,
                        o => o.MapFrom(s => s.OrderSummary.OrderTotals.NetValue))
                    .ForPath(d => d.Party,
                        o => o.MapFrom(s => MapParty(s.OrderHeader.BuyerInformation)))
                    .ForPath(d => d.SupplierParty,
                        o => o.MapFrom(s => MapParty(s.OrderHeader.SellerInformation)))
                    .ForPath(d => d.Details,
                        o => o.MapFrom(s => MapOrderLines(
                            s.OrderDetail != null ? s.OrderDetail.ItemDetails : null)))
                    .ForAllOtherMembers(o => o.Ignore());
                cfg.CreateMap<ItemTransaction, Order>()
                    .ForPath(d => d.OrderHeader.OrderDate,
                        o => o.MapFrom(s => s.CreateDate))
                    .ForPath(d => d.OrderHeader.OtherOrderDates.DeliveryDate,
                        o => o.MapFrom(s => s.ActualDeliveryDate))
                    .ForPath(d => d.OrderHeader.OtherOrderDates.LastAcceptableDeliveryDate,
                        o => o.MapFrom(s => s.ActualDeliveryDate))
                    .ForPath(d => d.OrderHeader.DocType,
                        o => o.MapFrom(_ => "221"))
                    .ForPath(d => d.OrderHeader.OrderType,
                        o => o.MapFrom(_ => "9"))
                    .ForPath(d => d.OrderHeader.OrderCurrency,
                        o => o.MapFrom(_ => "EUR"))
                    .ForPath(d => d.OrderHeader.PaymentInstructions.PaymentTerm,
                        o => o.MapFrom(s => ((int)s.Payment.PaymentDays).ToString()))
                    .ForPath(d => d.OrderHeader.OrderNumber,
                        o => o.MapFrom(s => s.ISignableTransactionTransactionID))
                    .ForPath(d => d.OrderSummary.NumberOfLines,
                        o => o.MapFrom(s => s.Details.Count))
                    .ForPath(d => d.OrderSummary.OrderTotals.NetValue,
                        o => o.MapFrom(s => s.TotalGrossAmount))
                    .ForPath(d => d.OrderSummary.OrderTotals.GrossValue,
                        o => o.MapFrom(s => s.TotalAmount))
                    .ForPath(d => d.OrderHeader.BuyerInformation,
                        o => o.MapFrom(s => MapPartyReverse(s.Party, s.PartyGLN)))
                    .ForPath(d => d.OrderHeader.SellerInformation,
                        o => o.MapFrom(s => MapPartyReverse(s.SupplierParty, s.LoadPlaceAddress.GLN)))
                    .ForPath(d => d.OrderHeader.DeliveryPlaceInformation,
                        o => o.MapFrom(s => MapPartyReverse(s.SupplierParty, s.PartyGLN)))
                    .ForPath(d => d.OrderHeader.BillToPartyInformation,
                        o => o.MapFrom(s => MapPartyReverse(s.SupplierParty, s.PartyGLN)))
                    .ForPath(d => d.OrderHeader.HeaderTaxes,
                        o => o.MapFrom(s => MapOrderHeaderTaxesReverse(s.Taxes)))
                    .ForPath(d => d.OrderDetail.ItemDetails,
                        o => o.MapFrom(s => MapOrderLinesReverse(s.Details)));

            });

            return config.CreateMapper();
        }

        private static Party MapParty(Models.Minsat.Common.PartyOrder party)
        {
            if (party == null) return null;

            return new Models.Party
            {
                //PartyID = party.InternalCode,
                GLN = party.EANCode,
                //Department = party.Department
            };
        }

        private static List<Detail> MapOrderLines(IEnumerable<ItemDetail> items)
        {
            var list = new List<Detail>();
            if (items == null) return list;

            foreach (var i in items)
            {
                var detail = new Detail
                {
                    LineItemID = i.LineItemNum,
                    ItemID = i.StandardPartNumber,
                    //BuyerItemID = i.BuyerPartNumber,
                    SupplierItemID = i.SellerPartNumber,
                    Description = i.ItemDescriptions?.Description,
                    Quantity = (double?)i.Quantity?.QuantityValue,
                    UnitPrice = i.Price?.NetPrice,
                    TotalNetAmount = i.MonetaryAmount
                };

                //// Guardar info extra sem perder dados
                //detail.ExtraData = new Dictionary<string, string>();

                //if (i.Price?.PVP != null)
                //    detail.ExtraData["PVP"] = i.Price.PVP.ToString();

                //if (i.Package?.PackageIdentifier != null)
                //    detail.ExtraData["PackageIdentifier"] = i.Package.PackageIdentifier;

                list.Add(detail);
            }

            return list;
        }

        #region "Reverse"

        private static Models.Minsat.Common.PartyOrder MapPartyReverse(Party party, string partyGLN)
        {
            //if (party == null) return null;

            return new Models.Minsat.Common.PartyOrder
            {
                EANCode = partyGLN,
                // InternalCode = party.PartyID,
                // Department = party.Department
            };
        }

        private static List<ItemDetail> MapOrderLinesReverse(IEnumerable<Detail> details)
        {
            var list = new List<ItemDetail>();
            if (details == null) return list;

            foreach (var d in details)
            {
                list.Add(new ItemDetail
                {
                    LineItemNum = (int)(d.LineItemID != null ? d.LineItemID.Value : 0),
                    StandardPartNumber = d.ItemID,
                    BuyerPartNumber = d.ItemID,
                    SellerPartNumber = d.Description,

                    ItemDescriptions = d.Description != null
                        ? new ItemDescriptions
                        {
                            Description = d.Description
                        }
                        : null,
                    Quantity = d.Quantity != null
                        ? new Quantity
                        {
                            QuantityValue = (decimal)d.Quantity
                        }
                        : null,
                    Price = d.UnitPrice != null
                        ? new Price
                        {
                            NetPrice = (d.UnitPrice != null ? d.UnitPrice.Value : 0),
                            GrossPrice = (d.TaxIncludedPrice != null ? d.TaxIncludedPrice.Value : 0),
                            PVP = (d.TaxIncludedPrice != null ? d.TaxIncludedPrice.Value : 0),
                            PriceBasisQuantity = (d.Quantity != null ? d.Quantity.Value : 0),
                        }
                        : null,
                    MonetaryAmount = (d.TotalGrossAmount != null ? d.TotalGrossAmount.Value : 0),
                });
            }

            return list;
        }

        private static Models.Minsat.Common.HeaderTaxes MapOrderHeaderTaxesReverse(IEnumerable<TaxValue> taxes)
        {
            var headerTaxes = new Models.Minsat.Common.HeaderTaxes
            {
                HeaderTaxesHeader = new List<Models.Minsat.Common.HeaderTaxesHeader>()
            };

            if (taxes == null) return headerTaxes;

            foreach (var t in taxes)
            {
                headerTaxes.HeaderTaxesHeader.Add(new Models.Minsat.Common.HeaderTaxesHeader
                {
                    TaxPercent = t.TaxRate
                });
            }
            return headerTaxes;

        }

        #endregion
    }
}