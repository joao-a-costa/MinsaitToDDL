using System.Collections.Generic;
using System.Xml.Serialization;

namespace MinsatToDDL.Lib.Models.Minsait.Order
{
    public class OrderDetail
    {
        [XmlElement("ItemDetail")]
        public List<ItemDetail> ItemDetails { get; set; } // <-- Change type and name
    }
}