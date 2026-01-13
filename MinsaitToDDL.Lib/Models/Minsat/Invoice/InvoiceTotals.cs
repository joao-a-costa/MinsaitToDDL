namespace MinsaitToDDL.Lib.Models.Minsat.Invoice
{
    using System.Xml.Serialization;

    public class InvoiceTotals
    {
        [XmlElement("TotalTaxableAmount")]
        public decimal TotalTaxableAmount { get; set; }

        [XmlElement("NetValue")]
        public decimal NetValue { get; set; }

        [XmlElement("GrossValue")]
        public decimal GrossValue { get; set; }

        [XmlElement("TotalAmountPayable")]
        public decimal TotalAmountPayable { get; set; }

        [XmlElement("PackageAmount")]
        public decimal PackageAmount { get; set; }

        [XmlElement("ECOValue")]
        public decimal ECOValue { get; set; }

        [XmlElement("TaxDiscountPP")]
        public decimal TaxDiscountPP { get; set; }

        [XmlElement("PrepaidAmount")]
        public decimal PrepaidAmount { get; set; }

        [XmlElement("TotalDiscountPP")]
        public decimal TotalDiscountPP { get; set; }

        [XmlElement("TotalTaxAmount")]
        public decimal TotalTaxAmount { get; set; }

        [XmlElement("ContraValor")]
        public decimal ContraValor { get; set; }
    }
}