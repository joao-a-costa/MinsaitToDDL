using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MinsatToDDL.Lib.Models.Minsat
{
    public class InvoiceSummary
    {
        [XmlElement("InvoiceTotals")]
        public InvoiceTotals InvoiceTotals { get; set; }

        [XmlArray("SummaryTaxes")]
        [XmlArrayItem("SummaryTax")]
        public List<SummaryTax> SummaryTaxes { get; set; }
    }
}