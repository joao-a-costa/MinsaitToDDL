using System.Xml.Serialization;

namespace MinsaitToDDL.Lib.Models.Minsat.Common
{
    public class Quantity
    {
        [XmlElement("QuantityValue")]
        public decimal QuantityValue { get; set; }

        [XmlElement("UnitOfMeasurement")]
        public string UnitOfMeasurement { get; set; }
    }
}