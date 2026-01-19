using MinsaitToDDL.Lib.Interfaces;
using MinsaitToDDL.Lib.Models;
using static MinsaitToDDL.Lib.Enums.Enums;

namespace MinsaitToDDL.Lib.Parsers
{
    public class MinsaitInvoiceReverseParser : IMinsaitReverseDocumentParser
    {
        public bool CanHandle(DocumentType documentType)
            => documentType == DocumentType.INVOICE;

        public string MapToXml(ItemTransaction transaction)
        {
            var invoice = InvoiceReverseMapper.MapFromDdl(transaction);
            return InvoiceXmlSerializer.Serialize(invoice);
        }
    }
}
