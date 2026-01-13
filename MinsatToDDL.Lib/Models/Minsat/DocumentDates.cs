using System;
using System.Xml.Serialization;

namespace MinsatToDDL.Lib.Models.Minsat
{
    public class DocumentDates
    {
        [XmlElement("DocumentDate")]
        public DateTime DocumentDate { get; set; }

        [XmlElement("DueDate")]
        public DateTime? DueDate { get; set; }
    }
}
