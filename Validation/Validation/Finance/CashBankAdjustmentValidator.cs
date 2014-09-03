﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Validation;
using Core.DomainModel;
using Core.Interface.Service;

namespace Validation.Validation
{
    public class CashBankAdjustmentValidator : ICashBankAdjustmentValidator
    {
        public CashBankAdjustment VHasCashBank(CashBankAdjustment cashBankAdjustment, ICashBankService _cashBankService)
        {
            CashBank cashBank = _cashBankService.GetObjectById(cashBankAdjustment.CashBankId);
            if (cashBank == null)
            {
                cashBankAdjustment.Errors.Add("CashBankId", "Tidak terasosiasi dengan cashBank");
            }
            return cashBankAdjustment;
        }

        public CashBankAdjustment VAdjustmentDate(CashBankAdjustment cashBankAdjustment)
        {
            if (cashBankAdjustment.AdjustmentDate == null)
            {
                cashBankAdjustment.Errors.Add("AdjustmentDate", "Tidak boleh kosong");
            }
            return cashBankAdjustment;
        }

        public CashBankAdjustment VHasNotBeenConfirmed(CashBankAdjustment cashBankAdjustment)
        {
            if (cashBankAdjustment.IsConfirmed)
            {
                cashBankAdjustment.Errors.Add("Generic", "Tidak boleh sudah dikonfirmasi");
            }
            return cashBankAdjustment;
        }

        public CashBankAdjustment VHasBeenConfirmed(CashBankAdjustment cashBankAdjustment)
        {
            if (!cashBankAdjustment.IsConfirmed)
            {
                cashBankAdjustment.Errors.Add("Generic", "Harus sudah dikonfirmasi");
            }
            return cashBankAdjustment;
        }

        public CashBankAdjustment VNonZeroAmount(CashBankAdjustment cashBankAdjustment)
        {
            if (cashBankAdjustment.Amount == 0)
            {
                cashBankAdjustment.Errors.Add("Amount", "Tidak boleh 0");
            }
            return cashBankAdjustment;
        }

        public CashBankAdjustment VNonNegativeNorZeroCashBankAmount(CashBankAdjustment cashBankAdjustment, ICashBankService _cashBankService, bool CaseConfirm)
        {
            CashBank cashBank = _cashBankService.GetObjectById(cashBankAdjustment.CashBankId);
            if (CaseConfirm && cashBankAdjustment.Amount < 0)
            {
                if (cashBank.Amount + cashBankAdjustment.Amount < 0)
                {
                    cashBankAdjustment.Errors.Add("Generic", "CashBank.Amount tidak boleh kurang dari adjustment amount");
                }
            }
            else if (!CaseConfirm && cashBankAdjustment.Amount > 0)
            {
                if (cashBank.Amount - cashBankAdjustment.Amount < 0)
                {
                    cashBankAdjustment.Errors.Add("Generic", "CashBank.Amount tidak boleh kurang dari adjustment amount");
                }
            }
            return cashBankAdjustment;
        }

        public CashBankAdjustment VCreateObject(CashBankAdjustment cashBankAdjustment, ICashBankService _cashBankService)
        {
            VAdjustmentDate(cashBankAdjustment);
            if (!isValid(cashBankAdjustment)) { return cashBankAdjustment; }
            VHasCashBank(cashBankAdjustment, _cashBankService);
            if (!isValid(cashBankAdjustment)) { return cashBankAdjustment; }
            VNonZeroAmount(cashBankAdjustment);
            if (!isValid(cashBankAdjustment)) { return cashBankAdjustment; }
            VNonNegativeNorZeroCashBankAmount(cashBankAdjustment, _cashBankService, true);
            return cashBankAdjustment;
        }

        public CashBankAdjustment VUpdateObject(CashBankAdjustment cashBankAdjustment, ICashBankService _cashBankService)
        {
            VCreateObject(cashBankAdjustment, _cashBankService);
            if (!isValid(cashBankAdjustment)) { return cashBankAdjustment; }
            VHasNotBeenConfirmed(cashBankAdjustment);
            return cashBankAdjustment;
        }

        public CashBankAdjustment VDeleteObject(CashBankAdjustment cashBankAdjustment)
        {
            VHasNotBeenConfirmed(cashBankAdjustment);
            return cashBankAdjustment;
        }

        public CashBankAdjustment VHasConfirmationDate(CashBankAdjustment obj)
        {
            if (obj.ConfirmationDate == null)
            {
                obj.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return obj;
        }

        public CashBankAdjustment VConfirmObject(CashBankAdjustment cashBankAdjustment, ICashBankService _cashBankService)
        {
            VHasConfirmationDate(cashBankAdjustment);
            if (!isValid(cashBankAdjustment)) { return cashBankAdjustment; }
            VHasNotBeenConfirmed(cashBankAdjustment);
            if (!isValid(cashBankAdjustment)) { return cashBankAdjustment; }
            VNonNegativeNorZeroCashBankAmount(cashBankAdjustment, _cashBankService, true);
            return cashBankAdjustment;
        }

        public CashBankAdjustment VUnconfirmObject(CashBankAdjustment cashBankAdjustment, ICashBankService _cashBankService)
        {
            VHasBeenConfirmed(cashBankAdjustment);
            if (!isValid(cashBankAdjustment)) { return cashBankAdjustment; }
            VNonNegativeNorZeroCashBankAmount(cashBankAdjustment, _cashBankService, false);
            return cashBankAdjustment;
        }

        public bool ValidCreateObject(CashBankAdjustment cashBankAdjustment, ICashBankService _cashBankService)
        {
            VCreateObject(cashBankAdjustment, _cashBankService);
            return isValid(cashBankAdjustment);
        }

        public bool ValidUpdateObject(CashBankAdjustment cashBankAdjustment, ICashBankService _cashBankService)
        {
            cashBankAdjustment.Errors.Clear();
            VUpdateObject(cashBankAdjustment, _cashBankService);
            return isValid(cashBankAdjustment);
        }

        public bool ValidDeleteObject(CashBankAdjustment cashBankAdjustment)
        {
            cashBankAdjustment.Errors.Clear();
            VDeleteObject(cashBankAdjustment);
            return isValid(cashBankAdjustment);
        }

        public bool ValidConfirmObject(CashBankAdjustment cashBankAdjustment, ICashBankService _cashBankService)
        {
            cashBankAdjustment.Errors.Clear();
            VConfirmObject(cashBankAdjustment, _cashBankService);
            return isValid(cashBankAdjustment);
        }

        public bool ValidUnconfirmObject(CashBankAdjustment cashBankAdjustment, ICashBankService _cashBankService)
        {
            cashBankAdjustment.Errors.Clear();
            VUnconfirmObject(cashBankAdjustment, _cashBankService);
            return isValid(cashBankAdjustment);
        }

        public bool isValid(CashBankAdjustment obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(CashBankAdjustment obj)
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