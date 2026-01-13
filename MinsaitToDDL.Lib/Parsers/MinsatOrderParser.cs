using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using AutoMapper;
using MinsaitToDDL.Lib.Interfaces;
using MinsaitToDDL.Lib.Models;
using MinsatToDDL.Lib.Models.Minsat.Order;

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

        // =====================================================
        // AutoMapper configuration
        // =====================================================
        private static IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Order, ItemTransaction>()
                    .ForMember(d => d.ISignableTransactionTransactionID,
                        o => o.MapFrom(s => s.OrderHeader.OrderNumber))
                    .ForMember(d => d.CreateDate,
                        o => o.MapFrom(s => s.OrderHeader.OrderDate))
                    .ForPath(d => d.Party,
                        o => o.MapFrom(s => MapParty(
                            s.OrderHeader.BuyerInformation)))
                    .ForPath(d => d.SupplierParty,
                        o => o.MapFrom(s => MapParty(
                            s.OrderHeader.SellerInformation)))
                    .ForPath(d => d.Details,
                        o => o.MapFrom(s => MapOrderLines(
                            s.OrderDetail != null
                                ? s.OrderDetail.Items
                                : null)))
                    // ORDER não tem impostos nem totais fechados
                    .ForAllOtherMembers(o => o.Ignore());
            });

            return config.CreateMapper();
        }

        // =====================================================
        // Helpers
        // =====================================================

        private static Models.Party MapParty(MinsaitToDDL.Lib.Models.Minsat.Common.Party party)
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

        private static List<Detail> MapOrderLines(
            IEnumerable<OrderItemDetail> lines)
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

                    // Em ORDER pode não existir preço
                    UnitPrice = line.NetPrice.HasValue
                        ? (double)line.NetPrice.Value
                        : (double?)null,

                    TotalNetAmount = line.NetAmount.HasValue
                        ? (double)line.NetAmount.Value
                        : (double?)null
                });
            }

            return details;
        }
    }
}