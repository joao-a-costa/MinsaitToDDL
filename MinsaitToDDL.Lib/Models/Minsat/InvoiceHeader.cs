using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MinsaitToDDL.Lib.Models.Minsait
{
    public class InvoiceHeader
    {
        [XmlElement("InvoiceNumber")]
        public string InvoiceNumber { get; set; }

        [XmlElement("InvoiceDate")]
        public DateTime InvoiceDate { get; set; }

        [XmlElement("InvoiceCurrency")]
        public string InvoiceCurrency { get; set; }

        [XmlElement("OtherInvoiceDates")]
        public OtherInvoiceDates OtherInvoiceDates { get; set; }

        [XmlElement("BuyerInformation")]
        public Party BuyerInformation { get; set; }

        [XmlElement("SellerInformation")]
        public Party SellerInformation { get; set; }
    }
}