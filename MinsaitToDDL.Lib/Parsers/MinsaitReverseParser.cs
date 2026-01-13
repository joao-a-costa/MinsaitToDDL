using System;
using MinsaitToDDL.Lib.Models;

namespace MinsaitToDDL.Lib
{
    public class MinsaitReverseParser
    {
        public string MapToXml(ItemTransaction transaction)
        {
            // heurística simples (podes melhorar depois)
            if (transaction.TotalTransactionAmount.HasValue)
            {
                // INVOICE
                var invoice = MinsaitInvoiceMapper.MapFromDdl(transaction);
                return MinsaitInvoiceMapper.SerializeInvoiceToXml(invoice);
            }

            throw new NotSupportedException(
                "DDL transaction type not supported for XML export");
        }
    }
}