using System.Xml.Serialization;

namespace MinsatToDDL.Lib.Models.Minsat
{
    public class PaymentTerms
    {
        [XmlElement("description")]
        public string Description { get; set; }

        [XmlElement("value")]
        public string Value { get; set; } = "PP";
    }
}
