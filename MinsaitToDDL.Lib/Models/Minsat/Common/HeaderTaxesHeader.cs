using System.Xml.Serialization;

namespace MinsaitToDDL.Lib.Models.Minsait.Common
{
    public class HeaderTaxesHeader
    {

        [XmlElement(ElementName = "TaxType")]
        public string TaxType { get; set; } = "VAT";

        [XmlElement(ElementName = "TaxPercent")]
        public double TaxPercent { get; set; }
    }
}