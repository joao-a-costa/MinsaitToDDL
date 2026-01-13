using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MinsaitToDDL.Lib.Models.Minsait
{
    public class InvoiceDetail
    {
        [XmlElement("InvoiceItemDetail")]
        public List<InvoiceItemDetail> Items { get; set; }
    }
}