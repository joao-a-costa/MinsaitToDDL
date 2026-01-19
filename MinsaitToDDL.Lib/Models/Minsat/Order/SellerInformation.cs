using System.Xml.Serialization;

namespace MinsatToDDL.Lib.Models.Minsat.Order
{
    public class SellerInformation
    {
        [XmlElement("EANCode")]
        public string EANCode { get; set; }
    }
}