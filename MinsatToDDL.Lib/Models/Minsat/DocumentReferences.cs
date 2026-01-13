using System.Xml.Serialization;

namespace MinsatToDDL.Lib.Models.Minsat
{
    public class DocumentReferences
    {
        [XmlElement("invoiceReference")]
        public string InvoiceReference { get; set; }

        [XmlElement("orderReference")]
        public string OrderReference { get; set; }

        [XmlElement("commitmentReference")]
        public string CommitmentReference { get; set; }

        [XmlElement("contractId")]
        public string ContractId { get; set; }
    }
}
