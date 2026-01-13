using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MinsatToDDL.Lib.Models.Minsat
{
    public class InvoiceItemDetail
    {
        [XmlElement("LineItemNum")]
        public int LineItemNum { get; set; }

        [XmlElement("StandardPartNumber")]
        public string StandardPartNumber { get; set; }

        [XmlElement("ItemDescription")]
        public string ItemDescription { get; set; }

        [XmlElement("Quantity")]
        public Quantity Quantity { get; set; }

        [XmlElement("MonetaryAmount")]
        public decimal MonetaryAmount { get; set; }
    }
}