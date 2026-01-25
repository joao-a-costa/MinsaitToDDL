# MinsaitToDDL

MinsaitToDDL is a **.NET integration solution** designed to parse, validate, transform, and generate **Minsait XML business documents** (Invoices and Orders) into a unified internal **DDL (Domain Data Layer)** model.

This project is intended for **ERP integrations, EDI pipelines, middleware platforms, and financial document processing**, providing a clean separation between:
- External Minsait XML formats
- Internal business/domain models
- Validation and transformation logic

---

## ✨ Key Features

- ✅ Parse **Minsait Invoice XML**
- ✅ Parse **Minsait Order XML**
- ✅ Reverse parsing (DDL → Minsait XML)
- ✅ XML validation using official **XSD schemas**
- ✅ Strongly typed domain models
- ✅ AutoMapper-based transformation layer
- ✅ Console application for testing and automation
- ✅ Library-ready architecture (NuGet compatible)

---

## 🏗 Solution Structure

```
MinsaitToDDL
│
├── MinsaitToDDL.Lib
│   ├── Interfaces
│   │   └── IMinsaitParser.cs
│   │
│   ├── Models
│   │   ├── DDL
│   │   │   ├── ItemTransaction.cs
│   │   │   ├── Detail.cs
│   │   │   ├── TaxList.cs
│   │   │   └── ...
│   │   │
│   │   └── Minsait
│   │       ├── Common
│   │       ├── Invoice
│   │       ├── Order
│   │       └── ...
│   │
│   ├── Parsers
│   │   ├── MinsaitInvoiceParser.cs
│   │   ├── MinsaitOrderParser.cs
│   │   ├── MinsaitInvoiceReverseParser.cs
│   │   ├── MinsaitOrderReverseParser.cs
│   │   └── MinsaitReverseParser.cs
│   │
│   ├── Validation
│   │   ├── XmlSchemaValidator.cs
│   │   └── MinsaitSchemaResolver.cs
│   │
│   ├── Enums
│   └── MinsaitInvoiceMapper.cs
│
├── MinsaitToDDL.Console
│   ├── Program.cs
│   └── Resources
│       ├── invoice.xml
│       ├── order.xml
│       └── schemas
│
├── MinsaitToDDL.sln
├── LICENSE.txt
└── README.md
```

---

## 🧠 Architecture Overview

### Minsait Models
- Represent the **original XML structure**
- Designed for **exact deserialization**
- Located under `Models/Minsait/*`

### DDL Models
- Internal, normalized business entities
- Independent of Minsait-specific rules
- Optimized for ERP and database usage

### Parsers
- Convert XML → Objects → DDL
- Enforce schema validation
- Implement shared interfaces

### Reverse Parsers
- Convert DDL → Minsait-compliant XML
- Ensure output matches XSD rules

---

## 🔄 Data Flow

```
Minsait XML
   ↓ (XSD Validation)
Minsait Models
   ↓ (AutoMapper)
DDL Models
   ↓ (Business Logic / ERP)
DDL Models
   ↓ (Reverse Parser)
Minsait XML
```

---

## ▶️ Console Application

The console project demonstrates:
- Loading sample XML files
- Validating against XSD
- Parsing to DDL
- Generating XML back

### Example

```bash
dotnet run --project MinsaitToDDL.Console
```

You can modify `Program.cs` to:
- Batch process files
- Integrate with queues or APIs
- Export results to files or databases

---

## 🧪 Validation

XML validation is performed using `XmlSchemaValidator`:
- Prevents invalid documents early
- Supports multiple schemas
- Clear error reporting

---

## 📦 Usage as Library

You can reference `MinsaitToDDL.Lib` from another project:

```csharp
var parser = new MinsaitInvoiceParser();
var transaction = parser.Parse(xmlString);
```

Reverse generation:

```csharp
var reverseParser = new MinsaitInvoiceReverseParser();
var xml = reverseParser.Generate(transaction);
```

---

## 🔧 Requirements

- .NET 6.0 or higher
- AutoMapper
- System.Xml
- System.Xml.Schema

---

## 📄 License

This project is licensed under the terms defined in `LICENSE.txt`.

---

## 👤 Author / Maintainer

João Costa