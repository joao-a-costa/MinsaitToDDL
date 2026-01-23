using System.Collections.Generic;
using System.Xml.Serialization;

namespace MinsaitToDDL.Lib.Models.Minsait.Common
{
    public class HeaderTaxes
    {

        [XmlElement(ElementName = "HeaderTaxesHeader")]
        public List<HeaderTaxesHeader> HeaderTaxesHeader { get; set; }
    }
}