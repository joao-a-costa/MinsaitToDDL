using System;
using System.Xml.Serialization;
using MinsaitToDDL.Lib.Models.Minsat.Common;

namespace MinsatToDDL.Lib.Models.Minsat.Order
{
    public class OrderHeader
    {
        [XmlElement("OrderNumber")]
        public string OrderNumber { get; set; }

        [XmlElement("OrderDate")]
        public DateTime OrderDate { get; set; }

        [XmlElement("BuyerInformation")]
        public Party BuyerInformation { get; set; }

        [XmlElement("SellerInformation")]
        public Party SellerInformation { get; set; }
    }
}