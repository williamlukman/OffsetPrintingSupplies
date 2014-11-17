using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ICashBankService
    {
        IQueryable<CashBank> GetQueryable();
        ICashBankValidator GetValidator();
        IList<CashBank> GetAll();
        CashBank GetObjectById(int Id);
        CashBank GetObjectByName(string Name);
        CashBank CreateObject(CashBank cashBank, IAccountService _accountService, ICurrencyService _currencyService);
        CashBank UpdateObject(CashBank cashBank, ICurrencyService _currencyService); 
        CashBank UpdateCashBankObject(CashBank cashBank, ICurrencyService _currencyService,ICashMutationService _cashkMutationService);
        CashBank SoftDeleteObject(CashBank cashBank, ICashMutationService _cashMutationService);
        bool DeleteObject(int Id);
        bool IsNameDuplicated(CashBank cashBank);
        decimal GetTotalCashBank();
    }
}