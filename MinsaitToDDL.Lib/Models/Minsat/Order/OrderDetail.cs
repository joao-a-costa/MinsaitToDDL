using System.Collections.Generic;
using System.Xml.Serialization;

namespace MinsatToDDL.Lib.Models.Minsat.Order
{
    public class OrderDetail
    {
        [XmlElement("ItemDetail")]
        public List<ItemDetail> ItemDetails { get; set; } // <-- Change type and name
    }
}