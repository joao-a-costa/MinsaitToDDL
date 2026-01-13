using System.Xml.Serialization;

namespace MinsaitToDDL.Lib.Models.Minsat.Common
{
    public class MetaInfo
    {
        [XmlElement("key")]
        public string Key { get; set; }

        [XmlElement("value")]
        public string Value { get; set; }
    }
}
