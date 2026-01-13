using System;
using Newtonsoft.Json;
using MinsatToDDL.Lib;
using MinsatToDDL.Lib.Models;

namespace MinsatToDDL.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var minsatParser = new MinsatParser();
            var serializeOptions = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            while (true)
            {
                System.Console.Clear();
                System.Console.WriteLine("=== Minsat / DDL Conversion Console ===");
                System.Console.WriteLine("Select an option:");
                System.Console.WriteLine("1) Parse Minsat XML and convert to DDL");
                System.Console.WriteLine("2) Parse DDL JSON and convert to XML Invoice");
                System.Console.WriteLine("3) Parse DDL JSON sample string and convert to XML Invoice");
                System.Console.WriteLine("0) Exit");
                System.Console.Write("> ");

                var choice = System.Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ProcessMinsat(minsatParser, serializeOptions);
                        break;

                    case "2":
                        ProcessDdl(minsatParser, serializeOptions);
                        break;

                    case "3":
                        ProcessDdlFromString(minsatParser);
                        break;

                    case "0":
                        System.Console.WriteLine("Exiting...");
                        return;

                    default:
                        System.Console.WriteLine("Invalid option. Press Enter to try again...");
                        System.Console.ReadLine();
                        break;
                }
            }
        }

        // =====================================================
        // OPTION 1 - Parse Minsat XML ➜ DDL
        // =====================================================
        static void ProcessMinsat(
            MinsatParser minsatParser,
            JsonSerializerSettings serializeOptions)
        {
            System.Console.WriteLine("\n=== Parsing Minsat Sample File ===");

            var itemTransaction = minsatParser.Parse(
                Properties.Resources.MinsatSampleFile1);

            var json = JsonConvert.SerializeObject(
                itemTransaction,
                Formatting.Indented,
                serializeOptions);

            System.Console.WriteLine("\n--- DDL JSON Output ---");
            System.Console.WriteLine(json);

            System.Console.WriteLine("\nPress Enter to continue...");
            System.Console.ReadLine();
        }

        // =====================================================
        // OPTION 2 - Parse DDL JSON ➜ XML Invoice
        // =====================================================
        static void ProcessDdl(
            MinsatParser minsatParser,
            JsonSerializerSettings serializeOptions)
        {
            System.Console.WriteLine("\n=== Parsing Minsat Sample File to get DDL JSON ===");

            var itemTransaction = minsatParser.Parse(
                Properties.Resources.MinsatSampleFile1);

            var ddlJson = JsonConvert.SerializeObject(
                itemTransaction,
                Formatting.Indented,
                serializeOptions);

            System.Console.WriteLine("\n--- Generated DDL JSON ---");
            System.Console.WriteLine(ddlJson);

            var ddlObject = JsonConvert.DeserializeObject<ItemTransaction>(ddlJson);

            System.Console.WriteLine("\n--- Mapping DDL to Minsat Invoice ---");

            var invoice = MinsatToDDL.Lib.MinsatToDDL.MapFromDdl(ddlObject);
            var xml = MinsatToDDL.Lib.MinsatToDDL.SerializeInvoiceToXml(invoice);

            System.Console.WriteLine("\n--- Minsat XML Output ---");
            System.Console.WriteLine(xml);

            System.Console.WriteLine("\nPress Enter to continue...");
            System.Console.ReadLine();
        }

        // =====================================================
        // OPTION 3 - Parse Raw DDL JSON String ➜ XML Invoice
        // =====================================================
        static void ProcessDdlFromString(
            MinsatParser minsatParser)
        {
            System.Console.WriteLine("\n=== Parsing DDL JSON Sample String ===");

            var ddlJsonString = Properties.Resources.dllSampleString2;

            System.Console.WriteLine("\n--- Loaded DDL JSON ---");
            System.Console.WriteLine(ddlJsonString);

            System.Console.WriteLine("\n--- Mapping JSON to Minsat Invoice ---");

            var invoice = MinsatToDDL.Lib.MinsatToDDL.MapFromDdlString(ddlJsonString);
            var xml = MinsatToDDL.Lib.MinsatToDDL.SerializeInvoiceToXml(invoice);

            System.Console.WriteLine("\n--- Minsat XML Output ---");
            System.Console.WriteLine(xml);

            System.Console.WriteLine("\nPress Enter to continue...");
            System.Console.ReadLine();
        }
    }
}