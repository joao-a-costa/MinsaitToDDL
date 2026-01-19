using System.Xml.Serialization;

namespace MinsatToDDL.Lib.Models.Minsat.Order
{
    public class ItemDescriptions
    {
        [XmlElement("Description")]
        public string Description { get; set; }
    }
}