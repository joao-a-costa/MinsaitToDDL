using System.Xml.Serialization;

namespace MinsaitToDDL.Lib.Models.Minsait.Invoice
{
    public class PaymentTerms
    {
        [XmlElement("description")]
        public string Description { get; set; }

        [XmlElement("value")]
        public string Value { get; set; } = "PP";
    }
}
