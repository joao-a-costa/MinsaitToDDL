using System.Xml.Serialization;

namespace MinsaitToDDL.Lib.Models.Minsat.Invoice
{
    public class VatSummary
    {
        [XmlElement("TaxPercentage")]
        public decimal TaxPercentage { get; set; }

        [XmlElement("TaxTotalValue")]
        public decimal TaxTotalValue { get; set; }

        [XmlElement("TaxableAmount")]
        public decimal TaxableAmount { get; set; }
    }
}