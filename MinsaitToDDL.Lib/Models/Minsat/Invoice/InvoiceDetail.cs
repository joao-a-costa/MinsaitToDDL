using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MinsaitToDDL.Lib.Models.Minsait.Invoice
{
    public class InvoiceDetail
    {
        [XmlElement("InvoiceItemDetail")]
        public List<ItemDetail> ItemDetails { get; set; }
    }
}