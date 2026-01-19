using System.Xml.Serialization;

namespace MinsatToDDL.Lib.Models.Minsat.Order
{
    public class BuyerInformation
    {
        [XmlElement("EANCode")]
        public string EANCode { get; set; }

        [XmlElement("Department")]
        public string Department { get; set; }

        [XmlElement("InternalCode")]
        public string InternalCode { get; set; }
    }
}