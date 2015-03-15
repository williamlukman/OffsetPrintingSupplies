using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Validation;
using Core.DomainModel;
using Core.Interface.Service;

namespace Validation.Validation
{
    public class BankAdministrationValidator : IBankAdministrationValidator
    {
        public BankAdministration VHasCashBank(BankAdministration bankAdministration, ICashBankService _cashBankService)
        {
            CashBank cashBank = _cashBankService.GetObjectById(bankAdministration.CashBankId);
            if (cashBank == null)
            {
                bankAdministration.Errors.Add("CashBank", "Tidak terasosiasi dengan cashBank");
            }
            return bankAdministration;
        }

        public BankAdministration VIncomeDate(BankAdministration bankAdministration)
        {
            if (bankAdministration.AdministrationDate == null)
            {
                bankAdministration.Errors.Add("IncomeDate", "Tidak boleh kosong");
            }
            return bankAdministration;
        }

        public BankAdministration VHasNotBeenConfirmed(BankAdministration bankAdministration)
        {
            if (bankAdministration.IsConfirmed)
            {
                bankAdministration.Errors.Add("Generic", "Tidak boleh sudah dikonfirmasi");
            }
            return bankAdministration;
        }

        public BankAdministration VHasBeenConfirmed(BankAdministration bankAdministration)
        {
            if (!bankAdministration.IsConfirmed)
            {
                bankAdministration.Errors.Add("Generic", "Harus sudah dikonfirmasi");
            }
            return bankAdministration;
        }

        //public BankAdministration VPositiveAmount(BankAdministration bankAdministration)
        //{
        //    if (bankAdministration.BiayaAdminAmount < 0)
        //    {
        //        bankAdministration.Errors.Add("BiayaAdminAmount", "Harus lebih besar atau sama dengan 0");
        //    }
        //    else if (bankAdministration.BiayaBungaAmount < 0)
        //    {
        //        bankAdministration.Errors.Add("BiayaBungaAmount", "Harus lebih besar atau sama dengan 0");
        //    }
        //    else if (bankAdministration.PendapatanJasaAmount < 0)
        //    {
        //        bankAdministration.Errors.Add("PendapatanJasaAmount", "Harus lebih besar atau sama dengan 0");
        //    }
        //    else if (bankAdministration.PendapatanBungaAmount < 0)
        //    {
        //        bankAdministration.Errors.Add("PendapatanBungaAmount", "Harus lebih besar atau sama dengan 0");
        //    }
        //    else if (bankAdministration.PengembalianPiutangAmount < 0)
        //    {
        //        bankAdministration.Errors.Add("PengembalianPiutangAmount", "Harus lebih besar atau sama dengan 0");
        //    }
        //    return bankAdministration;
        //}

        //public BankAdministration VIsValidTaxAmount(BankAdministration bankAdministration)
        //{
        //    if (bankAdministration.TaxAmount < 0)
        //    {
        //        bankAdministration.Errors.Add("TaxAmount", "Harus lebih besar atau sama dengan 0");
        //    }
        //    else if (bankAdministration.TaxAmount >= bankAdministration.Amount)
        //    {
        //        bankAdministration.Errors.Add("TaxAmount", "Harus lebih kecil dari Amount");
        //    }
        //    return bankAdministration;
        //}

        public BankAdministration VIsValidExchangeRateAmount(BankAdministration bankAdministration)
        {
            if (bankAdministration.ExchangeRateAmount < 0)
            {
                bankAdministration.Errors.Add("ExchangeRateAmount", "Harus lebih besar atau sama dengan 0 (0 = mengambil nilai dari table Exchange Rate)");
            }
            return bankAdministration;
        }

        public BankAdministration VNonNegativeNorZeroCashBankAmount(BankAdministration bankAdministration, ICashBankService _cashBankService, bool CaseConfirm)
        {
            CashBank cashBank = _cashBankService.GetObjectById(bankAdministration.CashBankId);
            //decimal Amount = (bankAdministration.PendapatanJasaAmount + bankAdministration.PendapatanBungaAmount + bankAdministration.PengembalianPiutangAmount) -
            //    (bankAdministration.BiayaAdminAmount + bankAdministration.BiayaBungaAmount) * (CaseConfirm ? 1 : -1);
            decimal Amount = bankAdministration.Amount;
            if (cashBank.Amount + Amount < 0)
            {
                bankAdministration.Errors.Add("Generic", "Final CashBank Amount tidak boleh kurang dari 0");
            }
            return bankAdministration;
        }

        public BankAdministration VGeneralLedgerPostingHasNotBeenClosed(BankAdministration bankAdministration, IClosingService _closingService)
        {
            if (_closingService.IsDateClosed(bankAdministration.AdministrationDate))
            {
                bankAdministration.Errors.Add("Generic", "Ledger sudah tutup buku");
            }
            return bankAdministration;
        }

        public BankAdministration VCreateObject(BankAdministration bankAdministration, ICashBankService _cashBankService)
        {
            VIncomeDate(bankAdministration);
            if (!isValid(bankAdministration)) { return bankAdministration; }
            VHasCashBank(bankAdministration, _cashBankService);
            if (!isValid(bankAdministration)) { return bankAdministration; }
            VIsValidExchangeRateAmount(bankAdministration);
            if (!isValid(bankAdministration)) { return bankAdministration; }
            //VPositiveAmount(bankAdministration);
            //if (!isValid(bankAdministration)) { return bankAdministration; }
            //VIsValidTaxAmount(bankAdministration);
            //if (!isValid(bankAdministration)) { return bankAdministration; }
            VNonNegativeNorZeroCashBankAmount(bankAdministration, _cashBankService, true);
            return bankAdministration;
        }

        public BankAdministration VUpdateObject(BankAdministration bankAdministration, ICashBankService _cashBankService)
        {
            VHasNotBeenConfirmed(bankAdministration);
            if (!isValid(bankAdministration)) { return bankAdministration; }
            VCreateObject(bankAdministration, _cashBankService);
            return bankAdministration;
        }

        public BankAdministration VDeleteObject(BankAdministration bankAdministration)
        {
            VHasNotBeenConfirmed(bankAdministration);
            return bankAdministration;
        }

        public BankAdministration VHasConfirmationDate(BankAdministration obj)
        {
            if (obj.ConfirmationDate == null)
            {
                obj.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return obj;
        }

        public BankAdministration VConfirmObject(BankAdministration bankAdministration, ICashBankService _cashBankService, IClosingService _closingService)
        {
            VHasConfirmationDate(bankAdministration);
            if (!isValid(bankAdministration)) { return bankAdministration; }
            VHasNotBeenConfirmed(bankAdministration);
            if (!isValid(bankAdministration)) { return bankAdministration; }
            VNonNegativeNorZeroCashBankAmount(bankAdministration, _cashBankService, true);
            if (!isValid(bankAdministration)) { return bankAdministration; }
            VGeneralLedgerPostingHasNotBeenClosed(bankAdministration, _closingService);
            return bankAdministration;
        }

        public BankAdministration VUnconfirmObject(BankAdministration bankAdministration, ICashBankService _cashBankService, IClosingService _closingService)
        {
            VHasBeenConfirmed(bankAdministration);
            if (!isValid(bankAdministration)) { return bankAdministration; }
            VNonNegativeNorZeroCashBankAmount(bankAdministration, _cashBankService, false);
            if (!isValid(bankAdministration)) { return bankAdministration; }
            VGeneralLedgerPostingHasNotBeenClosed(bankAdministration, _closingService);
            return bankAdministration;
        }

        public bool ValidCreateObject(BankAdministration bankAdministration, ICashBankService _cashBankService)
        {
            VCreateObject(bankAdministration, _cashBankService);
            return isValid(bankAdministration);
        }

        public bool ValidUpdateObject(BankAdministration bankAdministration, ICashBankService _cashBankService)
        {
            bankAdministration.Errors.Clear();
            VUpdateObject(bankAdministration, _cashBankService);
            return isValid(bankAdministration);
        }

        public bool ValidDeleteObject(BankAdministration bankAdministration)
        {
            bankAdministration.Errors.Clear();
            VDeleteObject(bankAdministration);
            return isValid(bankAdministration);
        }

        public bool ValidConfirmObject(BankAdministration bankAdministration, ICashBankService _cashBankService, IClosingService _closingService)
        {
            bankAdministration.Errors.Clear();
            VConfirmObject(bankAdministration, _cashBankService, _closingService);
            return isValid(bankAdministration);
        }

        public bool ValidUnconfirmObject(BankAdministration bankAdministration, ICashBankService _cashBankService, IClosingService _closingService)
        {
            bankAdministration.Errors.Clear();
            VUnconfirmObject(bankAdministration, _cashBankService, _closingService);
            return isValid(bankAdministration);
        }

        public bool isValid(BankAdministration obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(BankAdministration obj)
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