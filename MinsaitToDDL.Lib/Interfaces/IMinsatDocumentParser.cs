using System.Xml.Linq;
using MinsaitToDDL.Lib.Models;

namespace MinsaitToDDL.Lib.Interfaces
{
    public interface IMinsaitDocumentParser
    {
        bool CanParse(XElement root);
        ItemTransaction Parse(string xml);
        string ParseFromDdl(ItemTransaction transaction);
    }
}