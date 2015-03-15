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
    public class BankAdministrationDetailValidator : IBankAdministrationDetailValidator
    {
        public BankAdministrationDetail VHasBankAdministration(BankAdministrationDetail bankAdministrationDetail, IBankAdministrationService _bankAdministrationService)
        {
            BankAdministration bankAdministration = _bankAdministrationService.GetObjectById(bankAdministrationDetail.BankAdministrationId);
            if (bankAdministration == null)
            {
                bankAdministrationDetail.Errors.Add("BankAdministrationId", "Tidak boleh tidak ada");
            }
            return bankAdministrationDetail;
        }

        public BankAdministrationDetail VHasAccount(BankAdministrationDetail bankAdministrationDetail, IAccountService _accountService)
        {
            Account account = _accountService.GetObjectById(bankAdministrationDetail.AccountId);
            if (account == null)
            {
                bankAdministrationDetail.Errors.Add("AccountId", "Tidak boleh tidak ada");
            }
            return bankAdministrationDetail;
        }

        public BankAdministrationDetail VHasNotBeenConfirmed(BankAdministrationDetail bankAdministrationDetail, IBankAdministrationService _bankAdministrationService)
        {
            var bankAdministration = _bankAdministrationService.GetObjectById(bankAdministrationDetail.BankAdministrationId);
            if (bankAdministration.IsConfirmed)
            {
                bankAdministrationDetail.Errors.Add("Generic", "Sudah dikonfirmasi");
            }
            return bankAdministrationDetail;
        }

        public BankAdministrationDetail VHasBeenConfirmed(BankAdministrationDetail bankAdministrationDetail, IBankAdministrationService _bankAdministrationService)
        {
            var bankAdministration = _bankAdministrationService.GetObjectById(bankAdministrationDetail.BankAdministrationId);
            if (!bankAdministration.IsConfirmed)
            {
                bankAdministrationDetail.Errors.Add("Generic", "Belum dikonfirmasi");
            }
            return bankAdministrationDetail;
        }

        public BankAdministrationDetail VHasNotBeenDeleted(BankAdministrationDetail bankAdministrationDetail)
        {
            if (bankAdministrationDetail.IsDeleted)
            {
                bankAdministrationDetail.Errors.Add("Generic", "Sudah didelete");
            }
            return bankAdministrationDetail;
        }

        public BankAdministrationDetail VAmountIsTheSameWithBankAdministrationAmount(BankAdministrationDetail bankAdministrationDetail, IBankAdministrationService _bankAdministrationService)
        {
            BankAdministration bankAdministration = _bankAdministrationService.GetObjectById(bankAdministrationDetail.BankAdministrationId);
            if (bankAdministrationDetail.Amount != bankAdministration.Amount)
            {
                bankAdministration.Errors.Add("Generic", "Account Payable Non Trading tidak memiliki amount yang sama dengan Payment Request");
            }
            return bankAdministrationDetail;
        }

        public BankAdministrationDetail VNonNegativeAmount(BankAdministrationDetail bankAdministrationDetail)
        {
            if (bankAdministrationDetail.Amount < 0)
            {
                bankAdministrationDetail.Errors.Add("Amount", "Tidak boleh kurang dari 0");
            }
            return bankAdministrationDetail;
        }

        //public BankAdministrationDetail VStatusIsCredit(BankAdministrationDetail bankAdministrationDetail)
        //{
        //    if (bankAdministrationDetail.Status == Constant.GeneralLedgerStatus.Debit)
        //    {
        //        bankAdministrationDetail.Errors.Add("Generic", "Sistem mengharapkan posting General Ledger credit");
        //    }
        //    return bankAdministrationDetail;
        //}

        //public BankAdministrationDetail VStatusIsDebit(BankAdministrationDetail bankAdministrationDetail)
        //{
        //    if (bankAdministrationDetail.Status == Constant.GeneralLedgerStatus.Credit)
        //    {
        //        bankAdministrationDetail.Errors.Add("Generic", "Sistem mengharapkan posting General Ledger debit");
        //    }
        //    return bankAdministrationDetail;
        //}

        //public BankAdministrationDetail VNotLegacyObject(BankAdministrationDetail bankAdministrationDetail)
        //{
        //    if (bankAdministrationDetail.IsLegacy)
        //    {
        //        bankAdministrationDetail.Errors.Add("Generic", "Hanya untuk non legacy object");
        //    }
        //    return bankAdministrationDetail;
        //}

        //public BankAdministrationDetail VIsLegacyObject(BankAdministrationDetail bankAdministrationDetail)
        //{
        //    if (!bankAdministrationDetail.IsLegacy)
        //    {
        //        bankAdministrationDetail.Errors.Add("Generic", "Hanya untuk legacy object");
        //    }
        //    return bankAdministrationDetail;
        //}

        public BankAdministrationDetail VNotCashBankObject(BankAdministrationDetail bankAdministrationDetail, IAccountService _accountService)
        {
            var account = _accountService.GetObjectById(bankAdministrationDetail.AccountId);
            if (account.IsCashBankAccount)
            {
                bankAdministrationDetail.Errors.Add("Generic", "Hanya untuk non-cashbank account");
            }
            return bankAdministrationDetail;
        }

        public BankAdministrationDetail VCreateObject(BankAdministrationDetail bankAdministrationDetail, IBankAdministrationService _bankAdministrationService, IBankAdministrationDetailService _bankAdministrationDetailService, IAccountService _accountService)
        {
            VHasBankAdministration(bankAdministrationDetail, _bankAdministrationService);
            if (!isValid(bankAdministrationDetail)) { return bankAdministrationDetail; }
            VHasAccount(bankAdministrationDetail, _accountService);
            if (!isValid(bankAdministrationDetail)) { return bankAdministrationDetail; }
            VHasNotBeenConfirmed(bankAdministrationDetail, _bankAdministrationService);
            if (!isValid(bankAdministrationDetail)) { return bankAdministrationDetail; }
            VHasNotBeenDeleted(bankAdministrationDetail);
            if (!isValid(bankAdministrationDetail)) { return bankAdministrationDetail; }
            //VNotLegacyObject(bankAdministrationDetail);
            VNotCashBankObject(bankAdministrationDetail, _accountService);
            if (!isValid(bankAdministrationDetail)) { return bankAdministrationDetail; }
            VNonNegativeAmount(bankAdministrationDetail);
            //if (!isValid(bankAdministrationDetail)) { return bankAdministrationDetail; }
            //VStatusIsDebit(bankAdministrationDetail);
            return bankAdministrationDetail;
        }

        public BankAdministrationDetail VUpdateObject(BankAdministrationDetail bankAdministrationDetail, IBankAdministrationService _bankAdministrationService, IBankAdministrationDetailService _bankAdministrationDetailService, IAccountService _accountService)
        {
            VHasNotBeenConfirmed(bankAdministrationDetail, _bankAdministrationService);
            if (!isValid(bankAdministrationDetail)) { return bankAdministrationDetail; }
            VCreateObject(bankAdministrationDetail, _bankAdministrationService, _bankAdministrationDetailService, _accountService);
            return bankAdministrationDetail;    
        }

        public BankAdministrationDetail VCreateLegacyObject(BankAdministrationDetail bankAdministrationDetail, IBankAdministrationService _bankAdministrationService, IBankAdministrationDetailService _bankAdministrationDetailService, IAccountService _accountService)
        {
            VHasBankAdministration(bankAdministrationDetail, _bankAdministrationService);
            if (!isValid(bankAdministrationDetail)) { return bankAdministrationDetail; }
            VHasAccount(bankAdministrationDetail, _accountService);
            if (!isValid(bankAdministrationDetail)) { return bankAdministrationDetail; }
            VHasNotBeenConfirmed(bankAdministrationDetail, _bankAdministrationService);
            if (!isValid(bankAdministrationDetail)) { return bankAdministrationDetail; }
            VHasNotBeenDeleted(bankAdministrationDetail);
            if (!isValid(bankAdministrationDetail)) { return bankAdministrationDetail; }
            //VIsLegacyObject(bankAdministrationDetail);
            VNotCashBankObject(bankAdministrationDetail, _accountService);
            if (!isValid(bankAdministrationDetail)) { return bankAdministrationDetail; }
            VNonNegativeAmount(bankAdministrationDetail);
            if (!isValid(bankAdministrationDetail)) { return bankAdministrationDetail; }
            VAmountIsTheSameWithBankAdministrationAmount(bankAdministrationDetail, _bankAdministrationService);
            //if (!isValid(bankAdministrationDetail)) { return bankAdministrationDetail; }
            //VStatusIsCredit(bankAdministrationDetail);
            return bankAdministrationDetail;
        }

        public BankAdministrationDetail VUpdateLegacyObject(BankAdministrationDetail bankAdministrationDetail, IBankAdministrationService _bankAdministrationService, IBankAdministrationDetailService _bankAdministrationDetailService, IAccountService _accountService)
        {
            VHasNotBeenConfirmed(bankAdministrationDetail, _bankAdministrationService);
            if (!isValid(bankAdministrationDetail)) { return bankAdministrationDetail; }
            VCreateLegacyObject(bankAdministrationDetail, _bankAdministrationService, _bankAdministrationDetailService, _accountService);
            return bankAdministrationDetail;
        }
        public BankAdministrationDetail VDeleteObject(BankAdministrationDetail bankAdministrationDetail, IBankAdministrationService _bankAdministrationService)
        {
            VHasNotBeenConfirmed(bankAdministrationDetail, _bankAdministrationService);
            if (!isValid(bankAdministrationDetail)) { return bankAdministrationDetail; }
            VHasNotBeenDeleted(bankAdministrationDetail);
            //if (!isValid(bankAdministrationDetail)) { return bankAdministrationDetail; }
            //VNotLegacyObject(bankAdministrationDetail);
            return bankAdministrationDetail;
        }

        public BankAdministrationDetail VHasConfirmationDate(BankAdministrationDetail obj)
        {
            if (obj.ConfirmationDate == null)
            {
                obj.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return obj;
        }

        public BankAdministrationDetail VConfirmObject(BankAdministrationDetail bankAdministrationDetail, IBankAdministrationService _bankAdministrationService)
        {
            VHasConfirmationDate(bankAdministrationDetail);
            if (!isValid(bankAdministrationDetail)) { return bankAdministrationDetail; }
            VHasNotBeenConfirmed(bankAdministrationDetail, _bankAdministrationService);
            return bankAdministrationDetail;
        }

        public BankAdministrationDetail VUnconfirmObject(BankAdministrationDetail bankAdministrationDetail, IBankAdministrationService _bankAdministrationService)
        {
            VHasBeenConfirmed(bankAdministrationDetail, _bankAdministrationService);
            return bankAdministrationDetail;
        }

        public bool ValidCreateObject(BankAdministrationDetail bankAdministrationDetail, IBankAdministrationService _bankAdministrationService, IBankAdministrationDetailService _bankAdministrationDetailService, IAccountService _accountService)
        {
            VCreateObject(bankAdministrationDetail, _bankAdministrationService, _bankAdministrationDetailService, _accountService);
            return isValid(bankAdministrationDetail);
        }

        public bool ValidUpdateObject(BankAdministrationDetail bankAdministrationDetail, IBankAdministrationService _bankAdministrationService, IBankAdministrationDetailService _bankAdministrationDetailService, IAccountService _accountService)
        {
            VUpdateObject(bankAdministrationDetail, _bankAdministrationService, _bankAdministrationDetailService, _accountService);
            return isValid(bankAdministrationDetail);
        }

        public bool ValidCreateLegacyObject(BankAdministrationDetail bankAdministrationDetail, IBankAdministrationService _bankAdministrationService, IBankAdministrationDetailService _bankAdministrationDetailService, IAccountService _accountService)
        {
            VCreateLegacyObject(bankAdministrationDetail, _bankAdministrationService, _bankAdministrationDetailService, _accountService);
            return isValid(bankAdministrationDetail);
        }

        public bool ValidUpdateLegacyObject(BankAdministrationDetail bankAdministrationDetail, IBankAdministrationService _bankAdministrationService, IBankAdministrationDetailService _bankAdministrationDetailService, IAccountService _accountService)
        {
            VUpdateLegacyObject(bankAdministrationDetail, _bankAdministrationService, _bankAdministrationDetailService, _accountService);
            return isValid(bankAdministrationDetail);
        }

        public bool ValidDeleteObject(BankAdministrationDetail bankAdministrationDetail, IBankAdministrationService _bankAdministrationService)
        {
            VDeleteObject(bankAdministrationDetail, _bankAdministrationService);
            return isValid(bankAdministrationDetail);
        }

        public bool ValidConfirmObject(BankAdministrationDetail bankAdministrationDetail, IBankAdministrationService _bankAdministrationService)
        {
            VConfirmObject(bankAdministrationDetail, _bankAdministrationService);
            return isValid(bankAdministrationDetail);
        }

        public bool ValidUnconfirmObject(BankAdministrationDetail bankAdministrationDetail, IBankAdministrationService _bankAdministrationService)
        {
            VUnconfirmObject(bankAdministrationDetail, _bankAdministrationService);
            return isValid(bankAdministrationDetail);
        }

        public bool isValid(BankAdministrationDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(BankAdministrationDetail obj)
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