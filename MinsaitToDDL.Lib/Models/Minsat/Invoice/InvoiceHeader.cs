using MinsaitToDDL.Lib.Models.Minsait.Common;
using MinsaitToDDL.Lib.Models.Minsait.Invoice;
using System;
using System.Xml.Serialization;

namespace MinsaitToDDL.Lib.Models.Minsait.Invoice
{
    public class InvoiceHeader
    {
        [XmlElement("InvoiceNumber")]
        public string InvoiceNumber { get; set; }

        [XmlElement("InvoiceDate")]
        public DateTime InvoiceDate { get; set; }

        //[XmlElement("DocType")]
        //public string DocType { get; set; } = "221";

        [XmlElement("InvoiceType")]
        public string InvoiceType { get; set; } = "9";

        [XmlElement("InvoiceCurrency")]
        public string InvoiceCurrency { get; set; } = "EUR";

        [XmlElement("OtherInvoiceDates")]
        public OtherInvoiceDates OtherInvoiceDates { get; set; }

        //[XmlElement("PaymentInstructions")]
        //public PaymentInstructions PaymentInstructions { get; set; }

        //[XmlElement(ElementName = "HeaderTaxes")]
        //public HeaderTaxes HeaderTaxes { get; set; }

        [XmlElement("BuyerInformation")]
        public Common.Party BuyerInformation { get; set; }

        [XmlElement("SellerInformation")]
        public Common.Party SellerInformation { get; set; }

        //[XmlElement("DeliveryPlaceInformation")]
        //public Party DeliveryPlaceInformation { get; set; }

        [XmlElement("BillToPartyInformation")]
        public Party BillToPartyInformation { get; set; }
    }
}