using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Validation;
using Core.DomainModel;
using Core.Interface.Service;
using Core.Constants;

namespace Validation.Validation
{
    public class GeneralLedgerJournalValidator : IGeneralLedgerJournalValidator
    {

        public GeneralLedgerJournal VIsValidSourceDocument(GeneralLedgerJournal generalLedgerJournal)
        {
            if (!generalLedgerJournal.SourceDocument.Equals(Constant.SourceDocument.PaymentVoucherDetail) &&
                !generalLedgerJournal.SourceDocument.Equals(Constant.SourceDocument.ReceiptVoucherDetail) &&
                !generalLedgerJournal.SourceDocument.Equals(Constant.SourceDocument.CustomPurchaseInvoiceDetail) &&
                !generalLedgerJournal.SourceDocument.Equals(Constant.SourceDocument.CashSalesInvoiceDetailDetail) &&
                !generalLedgerJournal.SourceDocument.Equals(Constant.SourceDocument.RetailSalesInvoiceDetail))
            {
                generalLedgerJournal.Errors.Add("SourceDocument", "Harus merupakan bagian dari Constant.SourceDocument");
            }
            return generalLedgerJournal;
        }

        public GeneralLedgerJournal VCreateObject(GeneralLedgerJournal generalLedgerJournal)
        {
            VIsValidSourceDocument(generalLedgerJournal);
            return generalLedgerJournal;
        }

        public GeneralLedgerJournal VDeleteObject(GeneralLedgerJournal generalLedgerJournal)
        {
            return generalLedgerJournal;
        }

        public bool ValidCreateObject(GeneralLedgerJournal generalLedgerJournal)
        {
            VCreateObject(generalLedgerJournal);
            return isValid(generalLedgerJournal);
        }

        public bool ValidDeleteObject(GeneralLedgerJournal generalLedgerJournal)
        {
            generalLedgerJournal.Errors.Clear();
            VDeleteObject(generalLedgerJournal);
            return isValid(generalLedgerJournal);
        }

        public bool isValid(GeneralLedgerJournal obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(GeneralLedgerJournal obj)
        {
            string erroroutput = "";
            KeyValuePair<string, string> first = obj.Errors.ElementAt(0);
            erroroutput += first.Key + "," + first.Value;
            foreach (KeyValuePair<string, string> pair in obj.Errors.Skip(1))
            {
                erroroutput += Environment.NewLine;
                erroroutput += pair.Key + "," + pair.Value;
            }
            return erroroutput;
        }

    }
}
