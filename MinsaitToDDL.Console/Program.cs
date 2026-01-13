using System;
using Newtonsoft.Json;
using MinsaitToDDL.Lib;
using MinsaitToDDL.Lib.Models;

namespace MinsaitToDDL.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var MinsaitParser = new MinsaitParser();
            var serializeOptions = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            while (true)
            {
                System.Console.Clear();
                System.Console.WriteLine("=== Minsait / DDL Conversion Console ===");
                System.Console.WriteLine("Select an option:");
                System.Console.WriteLine("1) Parse Minsait XML and convert to DDL");
                System.Console.WriteLine("2) Parse DDL JSON and convert to XML Invoice");
                System.Console.WriteLine("3) Parse DDL JSON sample string and convert to XML Invoice");
                System.Console.WriteLine("0) Exit");
                System.Console.Write("> ");

                var choice = System.Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ProcessMinsait(MinsaitParser, serializeOptions);
                        break;

                    case "2":
                        ProcessDdl(MinsaitParser, serializeOptions);
                        break;

                    case "3":
                        ProcessDdlFromString(MinsaitParser);
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
        // OPTION 1 - Parse Minsait XML ➜ DDL
        // =====================================================
        static void ProcessMinsait(
            MinsaitParser MinsaitParser,
            JsonSerializerSettings serializeOptions)
        {
            System.Console.WriteLine("\n=== Parsing Minsait Sample File ===");

            var itemTransaction = MinsaitParser.Parse(
                Properties.Resources.MinsaitSampleFile1);

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
            MinsaitParser MinsaitParser,
            JsonSerializerSettings serializeOptions)
        {
            System.Console.WriteLine("\n=== Parsing Minsait Sample File to get DDL JSON ===");

            var itemTransaction = MinsaitParser.Parse(
                Properties.Resources.MinsaitSampleFile1);

            var ddlJson = JsonConvert.SerializeObject(
                itemTransaction,
                Formatting.Indented,
                serializeOptions);

            System.Console.WriteLine("\n--- Generated DDL JSON ---");
            System.Console.WriteLine(ddlJson);

            var ddlObject = JsonConvert.DeserializeObject<ItemTransaction>(ddlJson);

            System.Console.WriteLine("\n--- Mapping DDL to Minsait Invoice ---");

            var invoice = MinsaitToDDL.Lib.MinsaitToDDL.MapFromDdl(ddlObject);
            var xml = MinsaitToDDL.Lib.MinsaitToDDL.SerializeInvoiceToXml(invoice);

            System.Console.WriteLine("\n--- Minsait XML Output ---");
            System.Console.WriteLine(xml);

            System.Console.WriteLine("\nPress Enter to continue...");
            System.Console.ReadLine();
        }

        // =====================================================
        // OPTION 3 - Parse Raw DDL JSON String ➜ XML Invoice
        // =====================================================
        static void ProcessDdlFromString(
            MinsaitParser MinsaitParser)
        {
            System.Console.WriteLine("\n=== Parsing DDL JSON Sample String ===");

            var ddlJsonString = Properties.Resources.dllSampleString2;

            System.Console.WriteLine("\n--- Loaded DDL JSON ---");
            System.Console.WriteLine(ddlJsonString);

            System.Console.WriteLine("\n--- Mapping JSON to Minsait Invoice ---");

            var invoice = MinsaitToDDL.Lib.MinsaitToDDL.MapFromDdlString(ddlJsonString);
            var xml = MinsaitToDDL.Lib.MinsaitToDDL.SerializeInvoiceToXml(invoice);

            System.Console.WriteLine("\n--- Minsait XML Output ---");
            System.Console.WriteLine(xml);

            System.Console.WriteLine("\nPress Enter to continue...");
            System.Console.ReadLine();
        }
    }
}