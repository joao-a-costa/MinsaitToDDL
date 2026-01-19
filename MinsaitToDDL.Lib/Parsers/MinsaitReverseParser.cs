using MinsaitToDDL.Lib.Interfaces;
using MinsaitToDDL.Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static MinsaitToDDL.Lib.Enums.Enums;

namespace MinsaitToDDL.Lib
{
    public class MinsaitReverseParser
    {
        private readonly IEnumerable<IMinsaitReverseDocumentParser> _parsers;

        public MinsaitReverseParser(IEnumerable<IMinsaitReverseDocumentParser> parsers)
        {
            _parsers = parsers;
        }

        public string MapToXml(ItemTransaction transaction, DocumentType documentType)
        {
            var parser = _parsers.FirstOrDefault(p => p.CanHandle(documentType));

            if (parser == null)
                throw new NotSupportedException(
                    $"DocumentType {documentType} not supported for XML export");

            return parser.MapToXml(transaction);
        }
    }
}