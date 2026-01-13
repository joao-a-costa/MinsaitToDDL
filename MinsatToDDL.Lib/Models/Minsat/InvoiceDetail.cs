using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MinsatToDDL.Lib.Models.Minsat
{
    public class InvoiceDetail
    {
        [XmlElement("InvoiceItemDetail")]
        public List<InvoiceItemDetail> Items { get; set; }
    }
}