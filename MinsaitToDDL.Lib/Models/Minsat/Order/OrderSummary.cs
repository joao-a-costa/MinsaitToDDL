using System.Xml.Serialization;

namespace MinsatToDDL.Lib.Models.Minsait.Order
{
    public class OrderSummary
    {
        [XmlElement("NumberOfLines")]
        public int NumberOfLines { get; set; }

        [XmlElement("OrderTotals")]
        public OrderTotals OrderTotals { get; set; }
    }
}