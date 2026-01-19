using MinsaitToDDL.Lib.Models.Minsat.Common;
using System.Xml.Serialization;

namespace MinsatToDDL.Lib.Models.Minsat.Order
{
    public class OrderItemDetail
    {
        [XmlElement("LineItemNum")]
        public int LineItemNum { get; set; }

        [XmlElement("StandardPartNumber")]
        public string StandardPartNumber { get; set; }

        [XmlElement("ItemDescription")]
        public string ItemDescription { get; set; }

        [XmlElement("Quantity")]
        public Quantity Quantity { get; set; }

        // OPCIONAL — só se existir no Order
        [XmlElement("NetPrice")]
        public decimal? NetPrice { get; set; }

        // OPCIONAL — só se existir no Order
        [XmlElement("NetAmount")]
        public decimal? NetAmount { get; set; }
    }
}