using System.Collections.Generic;
using System.Xml.Serialization;

namespace MinsatToDDL.Lib.Models.Minsat.Order
{
    public class OrderDetail
    {
        [XmlElement("OrderItemDetail")]
        public List<OrderItemDetail> Items { get; set; }
    }
}