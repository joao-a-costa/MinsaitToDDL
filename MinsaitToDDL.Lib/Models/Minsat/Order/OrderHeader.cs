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

        [XmlElement("OrderCurrency")]
        public string OrderCurrency { get; set; } // <-- Add this

        [XmlElement("PaymentInstructions")]
        public PaymentInstructions PaymentInstructions { get; set; } // <-- Add this

        [XmlElement("BuyerInformation")]
        public BuyerInformation BuyerInformation { get; set; } // <-- Change type

        [XmlElement("SellerInformation")]
        public SellerInformation SellerInformation { get; set; } // <-- Change type
    }
}