using System;
using System.Xml.Serialization;

namespace MinsaitToDDL.Lib.Models.Minsait
{
    public class SummaryTax
    {
        [XmlElement("TaxType")]
        public string TaxType { get; set; }

        [XmlElement("TaxPercent")]
        public decimal TaxPercent { get; set; }

        [XmlElement("TaxableAmount")]
        public decimal TaxableAmount { get; set; }

        [XmlElement("TaxAmount")]
        public decimal TaxAmount { get; set; }

        [XmlElement("TaxFreeText")]
        public string TaxFreeText { get; set; }
    }
}