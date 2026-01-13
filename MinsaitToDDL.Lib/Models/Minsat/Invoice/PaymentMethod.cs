using System.Xml.Serialization;

namespace MinsaitToDDL.Lib.Models.Minsat.Invoice
{
    public class PaymentMethod
    {
        [XmlElement("ibanCode")]
        public string IbanCode { get; set; } = string.Empty;

        [XmlElement("swiftCode")]
        public string SwiftCode { get; set; } = string.Empty;

        [XmlElement("bankName")]
        public string BankName { get; set; } = string.Empty;
    }
}
