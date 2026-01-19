using System.Xml.Serialization;

namespace MinsatToDDL.Lib.Models.Minsat.Order
{
    public class ItemDetail
    {
        [XmlElement("StandardPartNumber")]
        public string StandardPartNumber { get; set; }

        [XmlElement("BuyerPartNumber")]
        public string BuyerPartNumber { get; set; }

        [XmlElement("SellerPartNumber")]
        public string SellerPartNumber { get; set; }

        [XmlElement("ItemDescriptions")]
        public ItemDescriptions ItemDescriptions { get; set; }

        [XmlElement("Quantity")]
        public Quantity Quantity { get; set; }

        [XmlElement("Price")]
        public Price Price { get; set; }

        [XmlElement("MonetaryAmount")]
        public decimal? MonetaryAmount { get; set; }

        [XmlElement("Package")]
        public Package Package { get; set; }
    }
}