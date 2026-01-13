using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace MinsaitToDDL.Lib.Validation
{
    public static class XmlSchemaValidator
    {
        // =====================================================
        // VALIDATE using XSD as byte[]
        // =====================================================
        public static IList<string> Validate(
            string xmlContent,
            byte[] xsdBytes)
        {
            var errors = new List<string>();

            var schemas = new XmlSchemaSet();
            using (var xsdStream = new MemoryStream(xsdBytes))
            {
                schemas.Add(null, XmlReader.Create(xsdStream));
            }

            var settings = new XmlReaderSettings
            {
                ValidationType = ValidationType.Schema,
                Schemas = schemas
            };

            settings.ValidationEventHandler += (sender, args) =>
            {
                errors.Add(args.Severity + ": " + args.Message);
            };

            using (var reader = XmlReader.Create(
                new StringReader(xmlContent),
                settings))
            {
                try
                {
                    while (reader.Read()) { }
                }
                catch (XmlException ex)
                {
                    errors.Add("XML ERROR: " + ex.Message);
                }
            }

            return errors;
        }
    }
}