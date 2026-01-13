using System.Collections.Generic;
using System.Xml.Serialization;

namespace MinsatToDDL.Lib.Models.Minsat
{
    public class DocumentTotals
    {
        [XmlElement("TotalNetAmount")]
        public decimal TotalNetAmount { get; set; }

        [XmlElement("TotalVatAmount")]
        public decimal TotalVatAmount { get; set; }

        [XmlElement("TotalAmountPayable")]
        public decimal TotalAmountPayable { get; set; }

        [XmlElement("VatSummary")]
        public List<VatSummary> VatSummary { get; set; }
    }
}