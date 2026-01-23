using System.Xml.Serialization;

namespace MinsaitToDDL.Lib.Models.Minsat.Common
{
    public class ItemDescriptions
    {
        [XmlElement("Description")]
        public string Description { get; set; }
    }
}