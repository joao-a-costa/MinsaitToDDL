using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MinsaitToDDL.Lib.Models.Minsat.Invoice
{
    public class OtherInvoiceDates
    {
        [XmlElement("DeliveryDate")]
        public DateTime? DeliveryDate { get; set; }

        [XmlElement("InvoiceDueDate")]
        public DateTime? InvoiceDueDate { get; set; }

        [XmlElement("InvoiceDueDays")]
        public int? InvoiceDueDays { get; set; }
    }
}