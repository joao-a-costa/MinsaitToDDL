using System.Xml.Serialization;

namespace MinsaitToDDL.Lib.Models.Minsat.Common
{
    public class PackSize
    {

        [XmlElement(ElementName = "Quantity")]
        public int Quantity { get; set; }
    }
}