using MinsaitToDDL.Lib.Interfaces;
using MinsaitToDDL.Lib.Models;
using static MinsaitToDDL.Lib.Enums.Enums;

namespace MinsaitToDDL.Lib.Parsers
{
    public class MinsaitOrderReverseParser : IMinsaitReverseDocumentParser
    {
        public bool CanHandle(DocumentType documentType)
            => documentType == DocumentType.ORDER;

        public string MapToXml(ItemTransaction transaction)
        {
            var order = OrderReverseMapper.MapFromDdl(transaction);
            return OrderXmlSerializer.Serialize(order);
        }
    }
}
