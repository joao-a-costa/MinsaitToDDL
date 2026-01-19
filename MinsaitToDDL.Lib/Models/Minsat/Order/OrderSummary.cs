using System.Xml.Serialization;

namespace MinsatToDDL.Lib.Models.Minsat.Order
{
    public class OrderSummary
    {
        [XmlElement("OrderTotals")]
        public OrderTotals OrderTotals { get; set; }
    }
}