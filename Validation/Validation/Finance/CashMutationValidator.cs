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
    public class CashMutationValidator : ICashMutationValidator
    {

        public CashMutation VHasCashBank(CashMutation cashMutation, ICashBankService _cashBankService)
        {
            CashBank cashBank = _cashBankService.GetObjectById(cashMutation.CashBankId);
            if (cashBank == null)
            {
                cashMutation.Errors.Add("CashBankId", "Tidak terasosiasi dengan cashBank");
            }
            return cashMutation;
        }

        public CashMutation VStatus(CashMutation cashMutation)
        {
            if (!cashMutation.Status.Equals(Constant.MutationStatus.Addition) &&
                !cashMutation.Status.Equals(Constant.MutationStatus.Deduction))
            {
                cashMutation.Errors.Add("Status", "Harus merupakan bagian dari Constant.CashMutationStatus");
            }
            return cashMutation;
        }

        public CashMutation VSourceDocumentType(CashMutation cashMutation)
        {
            if (!cashMutation.SourceDocumentType.Equals(Constant.SourceDocumentType.CashBankAdjustment) &&
                !cashMutation.SourceDocumentType.Equals(Constant.SourceDocumentType.PaymentVoucher) &&
                !cashMutation.SourceDocumentType.Equals(Constant.SourceDocumentType.ReceiptVoucher))
            {
                cashMutation.Errors.Add("SourceDocumentType", "Harus merupakan bagian dari Constant.SourceDocumentType");
            }
            return cashMutation;
        }

        public CashMutation VNonNegativeNorZeroAmount(CashMutation cashMutation)
        {
            if (cashMutation.Amount <= 0)
            {
                cashMutation.Errors.Add("Amout", "Tidak boleh negatif atau 0");
            }
            return cashMutation;
        }

        public CashMutation VCreateObject(CashMutation cashMutation, ICashBankService _cashBankService)
        {
            VHasCashBank(cashMutation, _cashBankService);
            if (!isValid(cashMutation)) { return cashMutation; }
            VStatus(cashMutation);
            if (!isValid(cashMutation)) { return cashMutation; }
            VSourceDocumentType(cashMutation);
            if (!isValid(cashMutation)) { return cashMutation; }
            VNonNegativeNorZeroAmount(cashMutation);
            return cashMutation;
        }

        public CashMutation VDeleteObject(CashMutation cashMutation)
        {
            return cashMutation;
        }

        public bool ValidCreateObject(CashMutation cashMutation, ICashBankService _cashBankService)
        {
            VCreateObject(cashMutation, _cashBankService);
            return isValid(cashMutation);
        }

        public bool ValidDeleteObject(CashMutation cashMutation)
        {
            cashMutation.Errors.Clear();
            VDeleteObject(cashMutation);
            return isValid(cashMutation);
        }

        public bool isValid(CashMutation obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(CashMutation obj)
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
