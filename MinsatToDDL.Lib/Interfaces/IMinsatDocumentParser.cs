using System.Xml.Linq;
using MinsatToDDL.Lib.Models;

namespace MinsatToDDL.Lib.Interfaces
{
    public interface IMinsatDocumentParser
    {
        bool CanParse(XElement root);
        ItemTransaction Parse(string xml);
    }
}