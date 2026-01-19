using System.Xml.Serialization;

namespace MinsatToDDL.Lib.Models.Minsat.Order
{
    public class OrderTotals
    {
        [XmlElement("NetValue")]
        public decimal NetValue { get; set; }

        [XmlElement("GrossValue")]
        public decimal GrossValue { get; set; }
    }
}