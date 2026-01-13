using System.Xml.Serialization;

namespace MinsatToDDL.Lib.Models.Minsat
{
    public class BinaryDocumentFormat
    {
        [XmlElement("contentType")]
        public string ContentType { get; set; } = "application/pdf";

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("contentData")]
        public string ContentData { get; set; }
    }
}
