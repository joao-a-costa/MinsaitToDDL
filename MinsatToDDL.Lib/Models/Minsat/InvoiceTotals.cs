using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MinsatToDDL.Lib.Models.Minsat
{
    public class InvoiceTotals
    {
        [XmlElement("NetValue")]
        public decimal NetValue { get; set; }

        [XmlElement("TotalTaxAmount")]
        public decimal TotalTaxAmount { get; set; }

        [XmlElement("TotalAmountPayable")]
        public decimal TotalAmountPayable { get; set; }
    }
}