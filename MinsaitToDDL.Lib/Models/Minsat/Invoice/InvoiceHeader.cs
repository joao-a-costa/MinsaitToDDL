using System;
using System.Xml.Serialization;

namespace MinsaitToDDL.Lib.Models.Minsat.Invoice
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
        public Common.Party BuyerInformation { get; set; }

        [XmlElement("SellerInformation")]
        public Common.Party SellerInformation { get; set; }
    }
}