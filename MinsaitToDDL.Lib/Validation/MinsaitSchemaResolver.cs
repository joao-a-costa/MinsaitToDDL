using System;
using System.Xml.Linq;

namespace MinsaitToDDL.Lib.Validation
{
    public static class MinsaitSchemaResolver
    {
        public enum DocumentKind
        {
            Invoice,
            Order
        }

        public static DocumentKind Detect(string xmlContent)
        {
            var doc = XDocument.Parse(xmlContent);
            var root = doc.Root.Name.LocalName;

            if (root.Equals("Invoice", StringComparison.OrdinalIgnoreCase))
                return DocumentKind.Invoice;

            if (root.Equals("Order", StringComparison.OrdinalIgnoreCase))
                return DocumentKind.Order;

            throw new InvalidOperationException(
                "Unknown Minsait document type: " + root);
        }
    }
}