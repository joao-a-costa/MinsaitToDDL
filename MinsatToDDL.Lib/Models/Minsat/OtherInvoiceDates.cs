using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MinsatToDDL.Lib.Models.Minsat
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