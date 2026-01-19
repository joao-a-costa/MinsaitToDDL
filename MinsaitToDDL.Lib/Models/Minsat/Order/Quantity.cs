using System.Xml.Serialization;

namespace MinsatToDDL.Lib.Models.Minsat.Order
{
    public class Quantity
    {
        [XmlElement("QuantityValue")]
        public decimal QuantityValue { get; set; }
    }
}