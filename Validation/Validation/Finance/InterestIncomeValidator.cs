using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Validation;
using Core.DomainModel;
using Core.Interface.Service;

namespace Validation.Validation
{
    public class InterestIncomeValidator : IInterestIncomeValidator
    {
        public InterestIncome VHasCashBank(InterestIncome interestIncome, ICashBankService _cashBankService)
        {
            CashBank cashBank = _cashBankService.GetObjectById(interestIncome.CashBankId);
            if (cashBank == null)
            {
                interestIncome.Errors.Add("CashBank", "Tidak terasosiasi dengan cashBank");
            }
            return interestIncome;
        }

        public InterestIncome VIncomeDate(InterestIncome interestIncome)
        {
            if (interestIncome.InterestDate == null)
            {
                interestIncome.Errors.Add("IncomeDate", "Tidak boleh kosong");
            }
            return interestIncome;
        }

        public InterestIncome VHasNotBeenConfirmed(InterestIncome interestIncome)
        {
            if (interestIncome.IsConfirmed)
            {
                interestIncome.Errors.Add("Generic", "Tidak boleh sudah dikonfirmasi");
            }
            return interestIncome;
        }

        public InterestIncome VHasBeenConfirmed(InterestIncome interestIncome)
        {
            if (!interestIncome.IsConfirmed)
            {
                interestIncome.Errors.Add("Generic", "Harus sudah dikonfirmasi");
            }
            return interestIncome;
        }

        public InterestIncome VNonZeroAmount(InterestIncome interestIncome)
        {
            if (interestIncome.Amount == 0)
            {
                interestIncome.Errors.Add("Amount", "Tidak boleh 0");
            }
            return interestIncome;
        }

        public InterestIncome VNonNegativeNorZeroCashBankAmount(InterestIncome interestIncome, ICashBankService _cashBankService, bool CaseConfirm)
        {
            CashBank cashBank = _cashBankService.GetObjectById(interestIncome.CashBankId);
            if (CaseConfirm && interestIncome.Amount < 0)
            {
                if (cashBank.Amount + interestIncome.Amount < 0)
                {
                    interestIncome.Errors.Add("Generic", "Final CashBank Amount tidak boleh kurang dari 0");
                }
            }
            else if (!CaseConfirm && interestIncome.Amount > 0)
            {
                if (cashBank.Amount - interestIncome.Amount < 0)
                {
                    interestIncome.Errors.Add("Generic", "Adjustment Amount tidak boleh lebih besar dari CashBank Amount");
                }
            }
            return interestIncome;
        }

        public InterestIncome VGeneralLedgerPostingHasNotBeenClosed(InterestIncome interestIncome, IClosingService _closingService)
        {
            if (_closingService.IsDateClosed(interestIncome.InterestDate))
            {
                interestIncome.Errors.Add("Generic", "Ledger sudah tutup buku");
            }
            return interestIncome;
        }

        public InterestIncome VCreateObject(InterestIncome interestIncome, ICashBankService _cashBankService)
        {
            VIncomeDate(interestIncome);
            if (!isValid(interestIncome)) { return interestIncome; }
            VHasCashBank(interestIncome, _cashBankService);
            if (!isValid(interestIncome)) { return interestIncome; }
            VNonZeroAmount(interestIncome);
            if (!isValid(interestIncome)) { return interestIncome; }
            VNonNegativeNorZeroCashBankAmount(interestIncome, _cashBankService, true);
            return interestIncome;
        }

        public InterestIncome VUpdateObject(InterestIncome interestIncome, ICashBankService _cashBankService)
        {
            VCreateObject(interestIncome, _cashBankService);
            if (!isValid(interestIncome)) { return interestIncome; }
            VHasNotBeenConfirmed(interestIncome);
            return interestIncome;
        }

        public InterestIncome VDeleteObject(InterestIncome interestIncome)
        {
            VHasNotBeenConfirmed(interestIncome);
            return interestIncome;
        }

        public InterestIncome VHasConfirmationDate(InterestIncome obj)
        {
            if (obj.ConfirmationDate == null)
            {
                obj.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return obj;
        }

        public InterestIncome VConfirmObject(InterestIncome interestIncome, ICashBankService _cashBankService, IClosingService _closingService)
        {
            VHasConfirmationDate(interestIncome);
            if (!isValid(interestIncome)) { return interestIncome; }
            VHasNotBeenConfirmed(interestIncome);
            if (!isValid(interestIncome)) { return interestIncome; }
            VNonNegativeNorZeroCashBankAmount(interestIncome, _cashBankService, true);
            if (!isValid(interestIncome)) { return interestIncome; }
            VGeneralLedgerPostingHasNotBeenClosed(interestIncome, _closingService);
            return interestIncome;
        }

        public InterestIncome VUnconfirmObject(InterestIncome interestIncome, ICashBankService _cashBankService, IClosingService _closingService)
        {
            VHasBeenConfirmed(interestIncome);
            if (!isValid(interestIncome)) { return interestIncome; }
            VNonNegativeNorZeroCashBankAmount(interestIncome, _cashBankService, false);
            if (!isValid(interestIncome)) { return interestIncome; }
            VGeneralLedgerPostingHasNotBeenClosed(interestIncome, _closingService);
            return interestIncome;
        }

        public bool ValidCreateObject(InterestIncome interestIncome, ICashBankService _cashBankService)
        {
            VCreateObject(interestIncome, _cashBankService);
            return isValid(interestIncome);
        }

        public bool ValidUpdateObject(InterestIncome interestIncome, ICashBankService _cashBankService)
        {
            interestIncome.Errors.Clear();
            VUpdateObject(interestIncome, _cashBankService);
            return isValid(interestIncome);
        }

        public bool ValidDeleteObject(InterestIncome interestIncome)
        {
            interestIncome.Errors.Clear();
            VDeleteObject(interestIncome);
            return isValid(interestIncome);
        }

        public bool ValidConfirmObject(InterestIncome interestIncome, ICashBankService _cashBankService, IClosingService _closingService)
        {
            interestIncome.Errors.Clear();
            VConfirmObject(interestIncome, _cashBankService, _closingService);
            return isValid(interestIncome);
        }

        public bool ValidUnconfirmObject(InterestIncome interestIncome, ICashBankService _cashBankService, IClosingService _closingService)
        {
            interestIncome.Errors.Clear();
            VUnconfirmObject(interestIncome, _cashBankService, _closingService);
            return isValid(interestIncome);
        }

        public bool isValid(InterestIncome obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(InterestIncome obj)
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