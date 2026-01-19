using System.Xml.Serialization;

namespace MinsatToDDL.Lib.Models.Minsat.Order
{
    public class Price
    {
        [XmlElement("NetPrice")]
        public decimal NetPrice { get; set; }

        [XmlElement("PVP")]
        public decimal? PVP { get; set; }
    }
}