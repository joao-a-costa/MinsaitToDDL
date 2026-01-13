using System.Xml.Serialization;

namespace MinsaitToDDL.Lib.Models.Minsait
{
    public class MetaInfo
    {
        [XmlElement("key")]
        public string Key { get; set; }

        [XmlElement("value")]
        public string Value { get; set; }
    }
}
