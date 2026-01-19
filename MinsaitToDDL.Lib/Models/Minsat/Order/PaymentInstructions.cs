using System.Xml.Serialization;

namespace MinsatToDDL.Lib.Models.Minsat.Order
{
    public class PaymentInstructions
    {
        [XmlElement("PaymentTerm")]
        public string PaymentTerm { get; set; }
    }
}