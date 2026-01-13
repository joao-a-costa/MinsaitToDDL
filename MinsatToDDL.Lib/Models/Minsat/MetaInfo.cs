using System.Xml.Serialization;

namespace MinsatToDDL.Lib.Models.Minsat
{
    public class MetaInfo
    {
        [XmlElement("key")]
        public string Key { get; set; }

        [XmlElement("value")]
        public string Value { get; set; }
    }
}
