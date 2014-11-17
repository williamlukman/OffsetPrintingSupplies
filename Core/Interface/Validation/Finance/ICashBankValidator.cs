using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface ICashBankValidator
    {
        CashBank VName(CashBank cashBank, ICashBankService _cashBankService);
        CashBank VHasNoCashMutation(CashBank cashBank, ICashMutationService _cashMutationService);
        CashBank VNonNegativeAmount(CashBank cashBank);
        CashBank VCreateObject(CashBank cashBank, ICashBankService _cashBankService, ICurrencyService _currencyService);
        CashBank VUpdateObject(CashBank cashBank, ICashBankService _cashBankService, ICurrencyService _currencyService);
        CashBank VDeleteObject(CashBank cashBank, ICashMutationService _cashMutationService);
        CashBank VAdjustAmount(CashBank cashBank);
        bool ValidCreateObject(CashBank cashBank, ICashBankService _cashBankService, ICurrencyService _currencyService);
        bool ValidUpdateObject(CashBank cashBank, ICashBankService _cashBankService, ICurrencyService _currencyService);
        bool ValidUpdateCashBankObject(CashBank cashBank, ICashBankService _cashBankService, ICurrencyService _currencyService
            , ICashMutationService _cashMutationService);
        bool ValidDeleteObject(CashBank cashBank, ICashMutationService _cashMutationService);
        bool ValidAdjustAmount(CashBank cashBank);
        bool isValid(CashBank cashBank);
        string PrintError(CashBank cashBank);
    }
}
