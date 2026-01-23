using MinsaitToDDL.Lib.Models.Minsait.Common;
using System.Xml.Serialization;

namespace MinsaitToDDL.Lib.Models.Minsait.Invoice
{
    public class LineItem
    {
        [XmlElement("Number")]
        public int Number { get; set; }

        [XmlElement("TradeItemIdentification")]
        public string TradeItemIdentification { get; set; }

        [XmlElement("ItemDescription")]
        public string ItemDescription { get; set; }

        [XmlElement("Quantity")]
        public Quantity Quantity { get; set; }

        [XmlElement("NetPrice")]
        public decimal NetPrice { get; set; }

        [XmlElement("NetLineAmount")]
        public decimal NetLineAmount { get; set; }

        [XmlElement("LineVat")]
        public LineVat LineVat { get; set; }
    }
}