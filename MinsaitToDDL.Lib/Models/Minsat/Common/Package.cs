using System.Xml.Serialization;

namespace MinsaitToDDL.Lib.Models.Minsat.Common
{
    public class Package
    {
        [XmlElement("PackageIdentifier")]
        public string PackageIdentifier { get; set; }
    }
}