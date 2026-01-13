using System.Xml.Serialization;

namespace MinsaitToDDL.Lib.Models.Minsait
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
