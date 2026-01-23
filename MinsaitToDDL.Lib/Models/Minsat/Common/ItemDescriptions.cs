using System.Xml.Serialization;

namespace MinsaitToDDL.Lib.Models.Minsait.Common
{
    public class ItemDescriptions
    {
        [XmlElement("Description")]
        public string Description { get; set; }
    }
}