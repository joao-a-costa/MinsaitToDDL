using System.Xml.Serialization;

namespace MinsatToDDL.Lib.Models.Minsat.Order
{
    public class Package
    {
        [XmlElement("PackageIdentifier")]
        public string PackageIdentifier { get; set; }
    }
}