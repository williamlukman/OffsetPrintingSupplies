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

            if (!generalLedgerJournal.SourceDocument.Equals(Constant.GeneralLedgerSource.CashBankAdjustment) &&
                !generalLedgerJournal.SourceDocument.Equals(Constant.GeneralLedgerSource.CashBankMutation) &&
                !generalLedgerJournal.SourceDocument.Equals(Constant.GeneralLedgerSource.BankAdministration) &&
                !generalLedgerJournal.SourceDocument.Equals(Constant.GeneralLedgerSource.Memorial) &&
                !generalLedgerJournal.SourceDocument.Equals(Constant.GeneralLedgerSource.PaymentRequest) &&
                !generalLedgerJournal.SourceDocument.Equals(Constant.GeneralLedgerSource.PaymentVoucher) &&
                !generalLedgerJournal.SourceDocument.Equals(Constant.GeneralLedgerSource.PurchaseDownPayment) &&
                !generalLedgerJournal.SourceDocument.Equals(Constant.GeneralLedgerSource.PurchaseDownPaymentAllocation) &&
                !generalLedgerJournal.SourceDocument.Equals(Constant.GeneralLedgerSource.PurchaseAllowance) &&
                !generalLedgerJournal.SourceDocument.Equals(Constant.GeneralLedgerSource.PurchaseReceival) &&
                !generalLedgerJournal.SourceDocument.Equals(Constant.GeneralLedgerSource.PurchaseInvoice) &&
                !generalLedgerJournal.SourceDocument.Equals(Constant.GeneralLedgerSource.PurchaseInvoiceMigration) &&
                !generalLedgerJournal.SourceDocument.Equals(Constant.GeneralLedgerSource.StockAdjustment) &&
                !generalLedgerJournal.SourceDocument.Equals(Constant.GeneralLedgerSource.ReceiptVoucher) &&
                !generalLedgerJournal.SourceDocument.Equals(Constant.GeneralLedgerSource.DeliveryOrder) &&
                !generalLedgerJournal.SourceDocument.Equals(Constant.GeneralLedgerSource.SalesInvoice) &&
                !generalLedgerJournal.SourceDocument.Equals(Constant.GeneralLedgerSource.SalesInvoiceMigration) &&
                !generalLedgerJournal.SourceDocument.Equals(Constant.GeneralLedgerSource.SalesDownPayment) &&
                !generalLedgerJournal.SourceDocument.Equals(Constant.GeneralLedgerSource.SalesDownPaymentAllocation) &&
                !generalLedgerJournal.SourceDocument.Equals(Constant.GeneralLedgerSource.SalesAllowance) &&
                !generalLedgerJournal.SourceDocument.Equals(Constant.GeneralLedgerSource.RecoveryOrderDetail) &&
                !generalLedgerJournal.SourceDocument.Equals(Constant.GeneralLedgerSource.RecoveryAccessoryDetail) &&
                !generalLedgerJournal.SourceDocument.Equals(Constant.GeneralLedgerSource.BlanketOrderDetail) &&
                !generalLedgerJournal.SourceDocument.Equals(Constant.GeneralLedgerSource.BlendingWorkOrder) &&
                !generalLedgerJournal.SourceDocument.Equals(Constant.GeneralLedgerSource.Repacking) &&
                !generalLedgerJournal.SourceDocument.Equals(Constant.GeneralLedgerSource.CustomerStockAdjustment) &&
                !generalLedgerJournal.SourceDocument.Equals(Constant.GeneralLedgerSource.Closing))
            {
                generalLedgerJournal.Errors.Add("SourceDocument", "Harus merupakan bagian dari Constant.SourceDocument");
            }
            return generalLedgerJournal;
        }

        public GeneralLedgerJournal VIsLeafAccount(GeneralLedgerJournal generalLedgerJournal, IAccountService _accountService)
        {
            Account account = _accountService.GetObjectById(generalLedgerJournal.AccountId);

            if (!account.IsLeaf)
            {
                generalLedgerJournal.Errors.Add("Generic", "Non leaf account bukan sebuah item journal"); 
            }
            return generalLedgerJournal;
        }

        public GeneralLedgerJournal VCreateObject(GeneralLedgerJournal generalLedgerJournal, IAccountService _accountService)
        {
            VIsValidSourceDocument(generalLedgerJournal);
            if (!isValid(generalLedgerJournal)) { return generalLedgerJournal; }
            VIsLeafAccount(generalLedgerJournal, _accountService);
            return generalLedgerJournal;
        }

        public GeneralLedgerJournal VDeleteObject(GeneralLedgerJournal generalLedgerJournal)
        {
            return generalLedgerJournal;
        }

        public bool ValidCreateObject(GeneralLedgerJournal generalLedgerJournal, IAccountService _accountService)
        {
            VCreateObject(generalLedgerJournal, _accountService);
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
