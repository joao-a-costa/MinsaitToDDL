using System.Xml.Serialization;

namespace MinsaitToDDL.Lib.Models.Minsait
{
    public class Quantity
    {
        [XmlElement("QuantityValue")]
        public decimal QuantityValue { get; set; }

        [XmlElement("UnitOfMeasurement")]
        public string UnitOfMeasurement { get; set; }
    }
}