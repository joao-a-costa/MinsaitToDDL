using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MinsatToDDL.Lib.Interfaces;
using MinsatToDDL.Lib.Models;
using MinsatToDDL.Lib.Parsers;

namespace MinsatToDDL.Lib
{
    public class MinsatParser
    {
        private readonly List<IMinsatDocumentParser> _parsers;

        public MinsatParser()
        {
            _parsers = new List<IMinsatDocumentParser>
            {
                new MinsatInvoiceParser(),
                // new MinsatOrderParser(),
                // new MinsatDesadvParser()
            };
        }

        public ItemTransaction Parse(string xml)
        {
            var doc = XDocument.Parse(xml);
            var root = doc.Root;

            var parser = _parsers.FirstOrDefault(p => p.CanParse(root));

            if (parser == null)
                throw new InvalidOperationException(
                    "Unsupported Minsat document type: " + root.Name.LocalName);

            return parser.Parse(xml);
        }
    }
}