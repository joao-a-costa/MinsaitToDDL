using System.Xml.Serialization;

namespace MinsatToDDL.Lib.Models.Minsait.Order
{
    public class PaymentInstructions
    {
        [XmlElement("PaymentTerm")]
        public string PaymentTerm { get; set; }
    }
}