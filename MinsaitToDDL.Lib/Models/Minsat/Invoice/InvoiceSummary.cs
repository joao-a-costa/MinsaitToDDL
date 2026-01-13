using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MinsaitToDDL.Lib.Models.Minsat.Invoice
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