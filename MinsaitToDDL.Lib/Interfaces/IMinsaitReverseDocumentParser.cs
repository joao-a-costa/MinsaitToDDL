using MinsaitToDDL.Lib.Models;
using static MinsaitToDDL.Lib.Enums.Enums;

namespace MinsaitToDDL.Lib.Interfaces
{
    public interface IMinsaitReverseDocumentParser
    {
        bool CanHandle(DocumentType documentType);
        string MapToXml(ItemTransaction transaction);
    }
}
