using System.Xml.Serialization;

namespace MinsaitToDDL.Lib.Models.Minsait.Common
{
    public class Package
    {
        [XmlElement("PackageIdentifier")]
        public string PackageIdentifier { get; set; }
    }
}