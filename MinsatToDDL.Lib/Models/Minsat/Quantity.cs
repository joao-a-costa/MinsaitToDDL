using System.Xml.Serialization;

namespace MinsatToDDL.Lib.Models.Minsat
{
    public class Quantity
    {
        [XmlElement("QuantityValue")]
        public decimal QuantityValue { get; set; }

        [XmlElement("UnitOfMeasurement")]
        public string UnitOfMeasurement { get; set; }
    }
}