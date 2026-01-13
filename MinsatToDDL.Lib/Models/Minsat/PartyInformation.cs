using System.Xml.Serialization;

namespace MinsatToDDL.Lib.Models.Minsat
{
    public class PartyInformation
    {
        [XmlElement("Buyer")]
        public Party Buyer { get; set; }

        [XmlElement("Seller")]
        public Party Seller { get; set; }

        [XmlElement("ShipTo")]
        public Party ShipTo { get; set; }
    }
}