using System.Xml.Serialization;

namespace MinsatToDDL.Lib.Models.Minsat.Order
{
    [XmlRoot("Order")]
    public class Order
    {
        [XmlElement("OrderHeader")]
        public OrderHeader OrderHeader { get; set; }

        [XmlElement("OrderDetail")]
        public OrderDetail OrderDetail { get; set; }

        [XmlElement("OrderSummary")]
        public OrderSummary OrderSummary { get; set; } // <-- Add this
    }
}