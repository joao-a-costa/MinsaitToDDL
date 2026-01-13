using System.Xml.Serialization;

namespace MinsatToDDL.Lib.Models.Minsat
{
    public class LineVat
    {
        [XmlElement("TaxPercentage")]
        public decimal TaxPercentage { get; set; }

        [XmlElement("TaxTotalValue")]
        public decimal TaxTotalValue { get; set; }

        [XmlElement("TaxableAmount")]
        public decimal TaxableAmount { get; set; }
    }
}