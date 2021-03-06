﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Validation;
using Core.DomainModel;
using Core.Interface.Service;

namespace Validation.Validation
{
    public class CashBankMutationValidator : ICashBankMutationValidator
    {
        public CashBankMutation VHasMutationDate(CashBankMutation cashBankMutation)
        {
            if (cashBankMutation.MutationDate == null)
            {
                cashBankMutation.Errors.Add("MutationDate", "Tidak boleh kosong");
            }
            return cashBankMutation;
        }

        public CashBankMutation VHasNoBukti(CashBankMutation cashBankMutation)
        {
            if (cashBankMutation.NoBukti == null || cashBankMutation.NoBukti.Trim() == "")
            {
                cashBankMutation.Errors.Add("NoBukti", "Tidak boleh kosong");
            }
            return cashBankMutation;
        }

        public CashBankMutation VHasDifferentCashBank(CashBankMutation cashBankMutation)
        {
            if (cashBankMutation.SourceCashBankId == cashBankMutation.TargetCashBankId)
            {
                cashBankMutation.Errors.Add("Generic", "Source CashBank dan Target CashBank tidak boleh sama");
            }
            return cashBankMutation;
        }
         
        public CashBankMutation VHasSameCurrency(CashBankMutation cashBankMutation,ICashBankService _cashBankService)
        {
            CashBank sourceCashBank = _cashBankService.GetObjectById(cashBankMutation.SourceCashBankId);
            CashBank targetCashBank = _cashBankService.GetObjectById(cashBankMutation.TargetCashBankId);
            if (sourceCashBank.CurrencyId != targetCashBank.CurrencyId)
            {
                cashBankMutation.Errors.Add("Generic", "Source CashBank Currency dan Target CashBank Currency tidak boleh berbeda");
            }
            return cashBankMutation;
        }

        public CashBankMutation VHasSourceCashBank(CashBankMutation cashBankMutation, ICashBankService _cashBankService)
        {
            CashBank sourceCashBank = _cashBankService.GetObjectById(cashBankMutation.SourceCashBankId);
            if (sourceCashBank == null)
            {
                cashBankMutation.Errors.Add("SourceCashBankId", "Tidak terasosiasi dengan source cashbank");
            }
            return cashBankMutation;
        }

        public CashBankMutation VHasTargetCashBank(CashBankMutation cashBankMutation, ICashBankService _cashBankService)
        {
            CashBank targetCashBank = _cashBankService.GetObjectById(cashBankMutation.TargetCashBankId);
            if (targetCashBank == null)
            {
                cashBankMutation.Errors.Add("TargetCashBankId", "Tidak terasosiasi dengan target cashbank");
            }
            return cashBankMutation;
        }

        public CashBankMutation VHasNotBeenConfirmed(CashBankMutation cashBankMutation)
        {
            if (cashBankMutation.IsConfirmed)
            {
                cashBankMutation.Errors.Add("Generic", "Tidak boleh sudah dikonfirmasi");
            }
            return cashBankMutation;
        }

        public CashBankMutation VHasBeenConfirmed(CashBankMutation cashBankMutation)
        {
            if (!cashBankMutation.IsConfirmed)
            {
                cashBankMutation.Errors.Add("Generic", "Harus sudah dikonfirmasi");
            }
            return cashBankMutation;
        }

        public CashBankMutation VHasNotBeenDeleted(CashBankMutation cashBankMutation)
        {
            if (cashBankMutation.IsDeleted)
            {
                cashBankMutation.Errors.Add("Generic", "Sudah dihapus");
            }
            return cashBankMutation;
        }

        public CashBankMutation VNonNegativeNorZeroAmount(CashBankMutation cashBankMutation)
        {
            if (cashBankMutation.Amount <= 0)
            {
                cashBankMutation.Errors.Add("Amount", "Harus lebih besar dari 0");
            }
            return cashBankMutation;
        }

        public CashBankMutation VNonNegativeNorZeroSourceCashBank(CashBankMutation cashBankMutation, ICashBankService _cashBankService)
        {
            CashBank sourceCashBank = _cashBankService.GetObjectById(cashBankMutation.SourceCashBankId);
            if (sourceCashBank.Amount - cashBankMutation.Amount < 0)
            {
                cashBankMutation.Errors.Add("Amount", "Tidak boleh melebihi jumlah amount dari source CashBank");
            }
            return cashBankMutation;
        }

        public CashBankMutation VNonNegativeNorZeroTargetCashBank(CashBankMutation cashBankMutation, ICashBankService _cashBankService)
        {
            CashBank targetCashBank = _cashBankService.GetObjectById(cashBankMutation.TargetCashBankId);
            if (targetCashBank.Amount - cashBankMutation.Amount < 0)
            {
                cashBankMutation.Errors.Add("Amount", "Tidak boleh melebihi jumlah amount dari target CashBank");
            }
            return cashBankMutation;
        }

        public CashBankMutation VGeneralLedgerPostingHasNotBeenClosed(CashBankMutation cashBankMutation, IClosingService _closingService)
        {
            if (_closingService.IsDateClosed(cashBankMutation.MutationDate))
            {
                cashBankMutation.Errors.Add("Generic", "Ledger sudah tutup buku");
            }
            return cashBankMutation;
        }

        public CashBankMutation VCreateObject(CashBankMutation cashBankMutation, ICashBankService _cashBankService)
        {
            VHasNoBukti(cashBankMutation);
            if (!isValid(cashBankMutation)) { return cashBankMutation; }
            VHasDifferentCashBank(cashBankMutation);
            if (!isValid(cashBankMutation)) { return cashBankMutation; }
            VHasSourceCashBank(cashBankMutation, _cashBankService);
            if (!isValid(cashBankMutation)) { return cashBankMutation; }
            VHasTargetCashBank(cashBankMutation, _cashBankService);
            if (!isValid(cashBankMutation)) { return cashBankMutation; }
            VNonNegativeNorZeroAmount(cashBankMutation);
            if (!isValid(cashBankMutation)) { return cashBankMutation; }
            VNonNegativeNorZeroSourceCashBank(cashBankMutation, _cashBankService);
            if (!isValid(cashBankMutation)) { return cashBankMutation; }
            VHasSameCurrency(cashBankMutation, _cashBankService);
            return cashBankMutation;
        }

        public CashBankMutation VUpdateObject(CashBankMutation cashBankMutation, ICashBankService _cashBankService)
        {
            VHasNotBeenConfirmed(cashBankMutation);
            if (!isValid(cashBankMutation)) { return cashBankMutation; }
            VCreateObject(cashBankMutation, _cashBankService);
            return cashBankMutation;
        }

        public CashBankMutation VDeleteObject(CashBankMutation cashBankMutation)
        {
            VHasNotBeenConfirmed(cashBankMutation);
            return cashBankMutation;
        }

        public CashBankMutation VHasConfirmationDate(CashBankMutation obj)
        {
            if (obj.ConfirmationDate == null)
            {
                obj.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return obj;
        }

        public CashBankMutation VConfirmObject(CashBankMutation cashBankMutation, ICashBankService _cashBankService, IClosingService _closingService)
        {
            VHasConfirmationDate(cashBankMutation);
            if (!isValid(cashBankMutation)) { return cashBankMutation; }
            VHasNotBeenDeleted(cashBankMutation);
            if (!isValid(cashBankMutation)) { return cashBankMutation; }
            VHasNotBeenConfirmed(cashBankMutation);
            if (!isValid(cashBankMutation)) { return cashBankMutation; }
            VNonNegativeNorZeroAmount(cashBankMutation);
            if (!isValid(cashBankMutation)) { return cashBankMutation; }
            VNonNegativeNorZeroSourceCashBank(cashBankMutation, _cashBankService);
            if (!isValid(cashBankMutation)) { return cashBankMutation; }
            VGeneralLedgerPostingHasNotBeenClosed(cashBankMutation, _closingService);
            return cashBankMutation;
        }

        public CashBankMutation VUnconfirmObject(CashBankMutation cashBankMutation, ICashBankService _cashBankService, IClosingService _closingService)
        {
            VHasNotBeenDeleted(cashBankMutation);
            if (!isValid(cashBankMutation)) { return cashBankMutation; }
            VHasBeenConfirmed(cashBankMutation);
            if (!isValid(cashBankMutation)) { return cashBankMutation; }
            VNonNegativeNorZeroTargetCashBank(cashBankMutation, _cashBankService);
            if (!isValid(cashBankMutation)) { return cashBankMutation; }
            VGeneralLedgerPostingHasNotBeenClosed(cashBankMutation, _closingService);
            return cashBankMutation;
        }

        public bool ValidCreateObject(CashBankMutation cashBankMutation, ICashBankService _cashBankService)
        {
            VCreateObject(cashBankMutation, _cashBankService);
            return isValid(cashBankMutation);
        }

        public bool ValidUpdateObject(CashBankMutation cashBankMutation, ICashBankService _cashBankService)
        {
            cashBankMutation.Errors.Clear();
            VUpdateObject(cashBankMutation, _cashBankService);
            return isValid(cashBankMutation);
        }

        public bool ValidDeleteObject(CashBankMutation cashBankMutation)
        {
            cashBankMutation.Errors.Clear();
            VDeleteObject(cashBankMutation);
            return isValid(cashBankMutation);
        }

        public bool ValidConfirmObject(CashBankMutation cashBankMutation, ICashBankService _cashBankService, IClosingService _closingService)
        {
            cashBankMutation.Errors.Clear();
            VConfirmObject(cashBankMutation, _cashBankService, _closingService);
            return isValid(cashBankMutation);
        }

        public bool ValidUnconfirmObject(CashBankMutation cashBankMutation, ICashBankService _cashBankService, IClosingService _closingService)
        {
            cashBankMutation.Errors.Clear();
            VUnconfirmObject(cashBankMutation, _cashBankService, _closingService);
            return isValid(cashBankMutation);
        }

        public bool isValid(CashBankMutation obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(CashBankMutation obj)
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