using AutoMapper;
using MinsaitToDDL.Lib.Interfaces;
using MinsaitToDDL.Lib.Models;
using MinsaitToDDL.Lib.Models.Minsat.Invoice;
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

        // =====================================================
        // AutoMapper configuration
        // =====================================================
        private static IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Order, ItemTransaction>()
                    // ============================
                    // HEADER
                    // ============================

                    // OrderNumber → Transaction ID
                    .ForMember(d => d.ISignableTransactionTransactionID,
                        o => o.MapFrom(s => s.OrderHeader.OrderNumber))

                    // OrderDate
                    .ForMember(d => d.CreateDate,
                        o => o.MapFrom(s => s.OrderHeader.OrderDate))

                    //// Currency (se existir no DDL)
                    //.ForMember(d => d.CurrencyCode,
                    //    o => o.MapFrom(s => s.OrderHeader.OrderCurrency))

                    // Payment terms (ex: 30)
                    .ForPath(d => d.Payment.Description,
                        o => o.MapFrom(s =>
                            s.OrderHeader.PaymentInstructions != null
                                ? s.OrderHeader.PaymentInstructions.PaymentTerm
                                : null))

                    // ============================
                    // PARTIES
                    // ============================

                    // Buyer
                    .ForMember(d => d.Party,
                        o => o.MapFrom(s => MapOrderBuyer(s.OrderHeader.BuyerInformation)))

                    // Seller
                    .ForMember(d => d.SupplierParty,
                        o => o.MapFrom(s => MapOrderSeller(s.OrderHeader.SellerInformation)))

                    // ============================
                    // DETAILS (LINES)
                    // ============================

                    .ForMember(d => d.Details,
                        o => o.MapFrom(s => MapOrderLines(
                            s.OrderDetail != null
                                ? s.OrderDetail.ItemDetails
                                : null)))

                    // ============================
                    // TOTALS (OrderSummary)
                    // ============================

                    .ForMember(d => d.TotalAmount,
                        o => o.MapFrom(s =>
                            s.OrderSummary != null
                                ? s.OrderSummary.OrderTotals.NetValue
                                : (decimal?)null))

                    .ForMember(d => d.TotalTransactionAmount,
                        o => o.MapFrom(s =>
                            s.OrderSummary != null
                                ? s.OrderSummary.OrderTotals.GrossValue
                                : (decimal?)null))

                    // Orders normalmente NÃO incluem IVA
                    .ForMember(d => d.TransactionTaxIncluded,
                        o => o.MapFrom(_ => false))

                    .ForAllOtherMembers(o => o.Ignore());


                // ============================
                // DDL -> Minsait (mínimo viável)
                // ============================
                cfg.CreateMap<ItemTransaction, Order>()
                    .ForPath(d => d.OrderHeader.OrderNumber,
                        o => o.MapFrom(s => s.ISignableTransactionTransactionID))
                    .ForPath(d => d.OrderHeader.OrderDate,
                        o => o.MapFrom(s => s.CreateDate))
                    //.ForPath(d => d.OrderHeader.BuyerInformation,
                    //    o => o.MapFrom(s => MapPartyReverse(s.Party)))
                    //.ForPath(d => d.OrderHeader.SellerInformation,
                    //    o => o.MapFrom(s => MapPartyReverse(
                    //        s.SupplierParty ?? s.Party)))
                    .ForAllOtherMembers(o => o.Ignore());
            });

            return config.CreateMapper();
        }

        // =====================================================
        // Helpers
        // =====================================================

        private static Party MapOrderBuyer(BuyerInformation buyer)
        {
            if (buyer == null) return null;

            return new Party
            {
                GLN = buyer.EANCode,
                //ExternalID = buyer.EANCode,
                //Department = buyer.Department,
                //InternalCode = buyer.InternalCode
            };
        }

        private static Party MapOrderSeller(SellerInformation seller)
        {
            if (seller == null) return null;

            return new Party
            {
                GLN = seller.EANCode
            };
        }


        private static List<Detail> MapOrderLines(
            IEnumerable<ItemDetail> items)
        {
            var list = new List<Detail>();
            if (items == null) return list;

            foreach (var i in items)
            {
                var detail = new Detail
                {
                    ItemID = i.StandardPartNumber,
                    //BuyerItemID = i.BuyerPartNumber,
                    SupplierItemID = i.SellerPartNumber,
                    Description = i.ItemDescriptions?.Description,
                    Quantity = (double?)i.Quantity?.QuantityValue,
                    UnitPrice = (double?)i.Price?.NetPrice,
                    TotalNetAmount = (double?)i.MonetaryAmount
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

    }
}