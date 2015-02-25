using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Validation;
using Core.DomainModel;
using Core.Interface.Service;

namespace Validation.Validation
{
    public class InterestAdjustmentValidator : IInterestAdjustmentValidator
    {
        public InterestAdjustment VHasCashBank(InterestAdjustment interestAdjustment, ICashBankService _cashBankService)
        {
            CashBank cashBank = _cashBankService.GetObjectById(interestAdjustment.CashBankId);
            if (cashBank == null)
            {
                interestAdjustment.Errors.Add("CashBank", "Tidak terasosiasi dengan cashBank");
            }
            return interestAdjustment;
        }

        public InterestAdjustment VIncomeDate(InterestAdjustment interestAdjustment)
        {
            if (interestAdjustment.InterestDate == null)
            {
                interestAdjustment.Errors.Add("IncomeDate", "Tidak boleh kosong");
            }
            return interestAdjustment;
        }

        public InterestAdjustment VHasNotBeenConfirmed(InterestAdjustment interestAdjustment)
        {
            if (interestAdjustment.IsConfirmed)
            {
                interestAdjustment.Errors.Add("Generic", "Tidak boleh sudah dikonfirmasi");
            }
            return interestAdjustment;
        }

        public InterestAdjustment VHasBeenConfirmed(InterestAdjustment interestAdjustment)
        {
            if (!interestAdjustment.IsConfirmed)
            {
                interestAdjustment.Errors.Add("Generic", "Harus sudah dikonfirmasi");
            }
            return interestAdjustment;
        }

        public InterestAdjustment VPositiveAmount(InterestAdjustment interestAdjustment)
        {
            if (interestAdjustment.Amount <= 0)
            {
                interestAdjustment.Errors.Add("Amount", "Harus lebih besar dari 0");
            }
            return interestAdjustment;
        }

        public InterestAdjustment VIsValidTaxAmount(InterestAdjustment interestAdjustment)
        {
            if (interestAdjustment.TaxAmount < 0)
            {
                interestAdjustment.Errors.Add("TaxAmount", "Harus lebih besar atau sama dengan 0");
            }
            else if (interestAdjustment.TaxAmount >= interestAdjustment.Amount)
            {
                interestAdjustment.Errors.Add("TaxAmount", "Harus lebih kecil dari Amount");
            }
            return interestAdjustment;
        }

        public InterestAdjustment VIsValidExchangeRateAmount(InterestAdjustment interestAdjustment)
        {
            if (interestAdjustment.ExchangeRateAmount < 0)
            {
                interestAdjustment.Errors.Add("ExchangeRateAmount", "Harus lebih besar atau sama dengan 0 (0 = mengambil nilai dari table Exchange Rate)");
            }
            return interestAdjustment;
        }

        public InterestAdjustment VNonNegativeNorZeroCashBankAmount(InterestAdjustment interestAdjustment, ICashBankService _cashBankService, bool CaseConfirm)
        {
            CashBank cashBank = _cashBankService.GetObjectById(interestAdjustment.CashBankId);
            if (CaseConfirm && ((interestAdjustment.Amount - interestAdjustment.TaxAmount) * (interestAdjustment.IsExpense ? -1 : 1)) < 0)
            {
                if (cashBank.Amount + ((interestAdjustment.Amount - interestAdjustment.TaxAmount) * (interestAdjustment.IsExpense ? -1 : 1)) < 0)
                {
                    interestAdjustment.Errors.Add("Generic", "Final CashBank Amount tidak boleh kurang dari 0");
                }
            }
            else if (!CaseConfirm && ((interestAdjustment.Amount - interestAdjustment.TaxAmount) * (interestAdjustment.IsExpense ? -1 : 1)) > 0)
            {
                if (cashBank.Amount - ((interestAdjustment.Amount - interestAdjustment.TaxAmount) * (interestAdjustment.IsExpense ? -1 : 1)) < 0)
                {
                    interestAdjustment.Errors.Add("Generic", "Amount (minus Tax Amount) tidak boleh lebih besar dari CashBank Amount");
                }
            }
            return interestAdjustment;
        }

        public InterestAdjustment VGeneralLedgerPostingHasNotBeenClosed(InterestAdjustment interestAdjustment, IClosingService _closingService)
        {
            if (_closingService.IsDateClosed(interestAdjustment.InterestDate))
            {
                interestAdjustment.Errors.Add("Generic", "Ledger sudah tutup buku");
            }
            return interestAdjustment;
        }

        public InterestAdjustment VCreateObject(InterestAdjustment interestAdjustment, ICashBankService _cashBankService)
        {
            VIncomeDate(interestAdjustment);
            if (!isValid(interestAdjustment)) { return interestAdjustment; }
            VHasCashBank(interestAdjustment, _cashBankService);
            if (!isValid(interestAdjustment)) { return interestAdjustment; }
            VPositiveAmount(interestAdjustment);
            if (!isValid(interestAdjustment)) { return interestAdjustment; }
            VIsValidTaxAmount(interestAdjustment);
            if (!isValid(interestAdjustment)) { return interestAdjustment; }
            VIsValidExchangeRateAmount(interestAdjustment);
            if (!isValid(interestAdjustment)) { return interestAdjustment; }
            VNonNegativeNorZeroCashBankAmount(interestAdjustment, _cashBankService, true);
            return interestAdjustment;
        }

        public InterestAdjustment VUpdateObject(InterestAdjustment interestAdjustment, ICashBankService _cashBankService)
        {
            VHasNotBeenConfirmed(interestAdjustment);
            if (!isValid(interestAdjustment)) { return interestAdjustment; }
            VCreateObject(interestAdjustment, _cashBankService);
            return interestAdjustment;
        }

        public InterestAdjustment VDeleteObject(InterestAdjustment interestAdjustment)
        {
            VHasNotBeenConfirmed(interestAdjustment);
            return interestAdjustment;
        }

        public InterestAdjustment VHasConfirmationDate(InterestAdjustment obj)
        {
            if (obj.ConfirmationDate == null)
            {
                obj.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return obj;
        }

        public InterestAdjustment VConfirmObject(InterestAdjustment interestAdjustment, ICashBankService _cashBankService, IClosingService _closingService)
        {
            VHasConfirmationDate(interestAdjustment);
            if (!isValid(interestAdjustment)) { return interestAdjustment; }
            VHasNotBeenConfirmed(interestAdjustment);
            if (!isValid(interestAdjustment)) { return interestAdjustment; }
            VNonNegativeNorZeroCashBankAmount(interestAdjustment, _cashBankService, true);
            if (!isValid(interestAdjustment)) { return interestAdjustment; }
            VGeneralLedgerPostingHasNotBeenClosed(interestAdjustment, _closingService);
            return interestAdjustment;
        }

        public InterestAdjustment VUnconfirmObject(InterestAdjustment interestAdjustment, ICashBankService _cashBankService, IClosingService _closingService)
        {
            VHasBeenConfirmed(interestAdjustment);
            if (!isValid(interestAdjustment)) { return interestAdjustment; }
            VNonNegativeNorZeroCashBankAmount(interestAdjustment, _cashBankService, false);
            if (!isValid(interestAdjustment)) { return interestAdjustment; }
            VGeneralLedgerPostingHasNotBeenClosed(interestAdjustment, _closingService);
            return interestAdjustment;
        }

        public bool ValidCreateObject(InterestAdjustment interestAdjustment, ICashBankService _cashBankService)
        {
            VCreateObject(interestAdjustment, _cashBankService);
            return isValid(interestAdjustment);
        }

        public bool ValidUpdateObject(InterestAdjustment interestAdjustment, ICashBankService _cashBankService)
        {
            interestAdjustment.Errors.Clear();
            VUpdateObject(interestAdjustment, _cashBankService);
            return isValid(interestAdjustment);
        }

        public bool ValidDeleteObject(InterestAdjustment interestAdjustment)
        {
            interestAdjustment.Errors.Clear();
            VDeleteObject(interestAdjustment);
            return isValid(interestAdjustment);
        }

        public bool ValidConfirmObject(InterestAdjustment interestAdjustment, ICashBankService _cashBankService, IClosingService _closingService)
        {
            interestAdjustment.Errors.Clear();
            VConfirmObject(interestAdjustment, _cashBankService, _closingService);
            return isValid(interestAdjustment);
        }

        public bool ValidUnconfirmObject(InterestAdjustment interestAdjustment, ICashBankService _cashBankService, IClosingService _closingService)
        {
            interestAdjustment.Errors.Clear();
            VUnconfirmObject(interestAdjustment, _cashBankService, _closingService);
            return isValid(interestAdjustment);
        }

        public bool isValid(InterestAdjustment obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(InterestAdjustment obj)
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