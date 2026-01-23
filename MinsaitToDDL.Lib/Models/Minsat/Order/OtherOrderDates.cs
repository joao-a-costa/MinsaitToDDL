using System;
using System.Xml.Serialization;

namespace MinsaitToDDL.Lib.Models.Minsait.Order
{
    public class OtherOrderDates
    {
        [XmlElement("DeliveryDate")]
        public DateTime? DeliveryDate { get; set; }

        [XmlElement("LastAcceptableDeliveryDate")]
        public DateTime? LastAcceptableDeliveryDate { get; set; }
    }
}