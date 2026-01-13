using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MinsaitToDDL.Lib.Models.Minsat.Invoice
{
    public class InvoiceDetail
    {
        [XmlElement("InvoiceItemDetail")]
        public List<InvoiceItemDetail> Items { get; set; }
    }
}