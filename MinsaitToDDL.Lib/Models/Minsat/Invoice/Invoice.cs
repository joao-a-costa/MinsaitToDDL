using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MinsaitToDDL.Lib.Models.Minsait.Invoice
{
    [XmlRoot("Invoice")]
    public class Invoice
    {
        [XmlElement("InvoiceHeader")]
        public InvoiceHeader InvoiceHeader { get; set; }

        [XmlElement("InvoiceDetail")]
        public InvoiceDetail InvoiceDetail { get; set; }

        [XmlElement("InvoiceSummary")]
        public InvoiceSummary InvoiceSummary { get; set; }
    }
}