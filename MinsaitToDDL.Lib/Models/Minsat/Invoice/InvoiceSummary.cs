using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MinsaitToDDL.Lib.Models.Minsait.Invoice
{
    public class InvoiceSummary
    {
        [XmlElement("NumberOfLines")]
        public int NumberOfLines { get; set; }

        [XmlElement("InvoiceTotals")]
        public InvoiceTotals InvoiceTotals { get; set; }

        [XmlArray("SummaryTaxes")]
        [XmlArrayItem("SummaryTax")]
        public List<SummaryTax> SummaryTaxes { get; set; }
    }
}