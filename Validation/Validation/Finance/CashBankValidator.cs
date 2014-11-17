using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Validation;
using Core.DomainModel;
using Core.Interface.Service;

namespace Validation.Validation
{
    public class CashBankValidator : ICashBankValidator
    {

        public CashBank VName(CashBank cashBank, ICashBankService _cashBankService)
        {
            if (String.IsNullOrEmpty(cashBank.Name) || cashBank.Name.Trim() == "")
            {
                cashBank.Errors.Add("Name", "Tidak boleh kosong");
            }
            if (_cashBankService.IsNameDuplicated(cashBank))
            {
                cashBank.Errors.Add("Name", "Tidak boleh ada duplikasi");
            }
            return cashBank;
        }

        public CashBank VHasNoCashMutation(CashBank cashBank, ICashMutationService _cashMutationService)
        {
            IList<CashMutation> cashMutations = _cashMutationService.GetObjectsByCashBankId(cashBank.Id);
            if (cashMutations.Any())
            {
                cashBank.Errors.Add("Generic", "Tidak boleh ada asosiasi dengan cash mutations");
            }
            return cashBank;
        }

        public CashBank VNonNegativeAmount(CashBank cashBank)
        {
            if (cashBank.Amount < 0)
            {
                cashBank.Errors.Add("Generic", "Amount tidak boleh kurang dari 0");
            }
            return cashBank;
        }

        public CashBank VCurrency(CashBank cashBank,ICurrencyService _currencyService)
        { 
            if (_currencyService.GetObjectById(cashBank.CurrencyId) == null)
            {
                cashBank.Errors.Add("CurrencyId", "Invalid Currency");
            }
            return cashBank;
        }
 
        public CashBank VCreateObject(CashBank cashBank, ICashBankService _cashBankService,ICurrencyService _currencyService)
        {
            VName(cashBank, _cashBankService);
            if (!isValid(cashBank)) { return cashBank; }
            VNonNegativeAmount(cashBank);
            if (!isValid(cashBank)) { return cashBank; }
            VCurrency(cashBank, _currencyService);
            return cashBank;
        }

        public CashBank VUpdateObject(CashBank cashBank, ICashBankService _cashBankService, ICurrencyService _currencyService)
        {
            VCreateObject(cashBank, _cashBankService,_currencyService);
            return cashBank;
        }
         
        public CashBank VUpdateCashBankObject(CashBank cashBank, ICashBankService _cashBankService, ICurrencyService _currencyService,
            ICashMutationService _cashMutationService)
        {
            VCreateObject(cashBank, _cashBankService, _currencyService);
            if (!isValid(cashBank)) { return cashBank; }
            VHasNoCashMutation(cashBank, _cashMutationService);
            return cashBank;
        }

        public CashBank VDeleteObject(CashBank cashBank, ICashMutationService _cashMutationService)
        {
            VHasNoCashMutation(cashBank, _cashMutationService);
            return cashBank;
        }

        public CashBank VAdjustAmount(CashBank cashBank)
        {
            VNonNegativeAmount(cashBank);
            return cashBank;
        }

        public bool ValidCreateObject(CashBank cashBank, ICashBankService _cashBankService, ICurrencyService _currencyService)
        {
            VCreateObject(cashBank, _cashBankService,_currencyService);
            return isValid(cashBank);
        }

        public bool ValidUpdateObject(CashBank cashBank, ICashBankService _cashBankService, ICurrencyService _currencyService)
        {
            cashBank.Errors.Clear();
            VUpdateObject(cashBank, _cashBankService,_currencyService);
            return isValid(cashBank);
        }
         
        public bool ValidUpdateCashBankObject(CashBank cashBank, ICashBankService _cashBankService, ICurrencyService _currencyService
            ,ICashMutationService _cashMutationService)
        {
            cashBank.Errors.Clear(); 
            VUpdateCashBankObject(cashBank, _cashBankService, _currencyService,_cashMutationService);
            return isValid(cashBank);
        }

        public bool ValidDeleteObject(CashBank cashBank, ICashMutationService _cashMutationService)
        {
            cashBank.Errors.Clear();
            VDeleteObject(cashBank, _cashMutationService);
            return isValid(cashBank);
        }

        public bool ValidAdjustAmount(CashBank cashBank)
        {
            cashBank.Errors.Clear();
            VAdjustAmount(cashBank);
            return isValid(cashBank);
        }

        public bool isValid(CashBank obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(CashBank obj)
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
