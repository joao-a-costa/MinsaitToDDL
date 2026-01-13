using System;
using System.Xml.Serialization;

namespace MinsaitToDDL.Lib.Models.Minsat.Invoice
{
    public class DocumentDates
    {
        [XmlElement("DocumentDate")]
        public DateTime DocumentDate { get; set; }

        [XmlElement("DueDate")]
        public DateTime? DueDate { get; set; }
    }
}
