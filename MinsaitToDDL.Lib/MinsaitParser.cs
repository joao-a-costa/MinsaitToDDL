using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MinsaitToDDL.Lib.Interfaces;
using MinsaitToDDL.Lib.Models;
using MinsaitToDDL.Lib.Parsers;

namespace MinsaitToDDL.Lib
{
    public class MinsaitParser
    {
        private readonly List<IMinsaitDocumentParser> _parsers;

        public MinsaitParser()
        {
            _parsers = new List<IMinsaitDocumentParser>
            {
                new MinsaitInvoiceParser(),
                new MinsaitOrderParser(),
                // new MinsaitDesadvParser()
            };
        }

        public ItemTransaction Parse(string xml)
        {
            var doc = XDocument.Parse(xml);
            var root = doc.Root;

            var parser = _parsers.FirstOrDefault(p => p.CanParse(root));

            if (parser == null)
                throw new InvalidOperationException(
                    "Unsupported Minsait document type: " + root.Name.LocalName);

            return parser.Parse(xml);
        }

        public string MapToXml(ItemTransaction transaction, Enums.Enums.DocumentType documentType)
        {
            switch (documentType)
            {
                case Enums.Enums.DocumentType.INVOICE:
                    var invoiceParser = _parsers.OfType<MinsaitInvoiceParser>().FirstOrDefault();
                    if (invoiceParser != null)
                    {
                        return invoiceParser.ParseFromDdl(transaction);
                    }
                    break;
                case Enums.Enums.DocumentType.ORDER:
                    var orderParser = _parsers.OfType<MinsaitOrderParser>().FirstOrDefault();
                    if (orderParser != null)
                    {
                        return orderParser.ParseFromDdl(transaction);
                    }
                    break;
                // Adicione outros casos conforme necessário
                default:
                    throw new InvalidOperationException("Unsupported document type: " + documentType);
            }

            return null;
        }
    }
}