using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IInterestIncomeService
    {
        IQueryable<InterestIncome> GetQueryable();
        IInterestIncomeValidator GetValidator();
        IList<InterestIncome> GetAll();
        IList<InterestIncome> GetObjectsByCashBankId(int cashBankId);
        InterestIncome GetObjectById(int Id);
        InterestIncome CreateObject(InterestIncome interestIncome, ICashBankService _cashBankService);
        InterestIncome CreateObject(int CashBankId, DateTime AdjustmentDate, ICashBankService _cashBankService);
        InterestIncome UpdateObject(InterestIncome interestIncome, ICashBankService _cashBankService);
        InterestIncome SoftDeleteObject(InterestIncome interestIncome);
        bool DeleteObject(int Id);
        InterestIncome ConfirmObject(InterestIncome interestIncome, DateTime ConfirmationDate, ICashMutationService _cashMutationService, ICashBankService _cashBankService,
                                         IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService,
                                         ICurrencyService _currencyService, IExchangeRateService _exchangeRateService,IGLNonBaseCurrencyService _gLNonBaseCurrencyService);
        InterestIncome UnconfirmObject(InterestIncome interestIncome, ICashMutationService _cashMutationService, ICashBankService _cashBankService,
                                           IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService, ICurrencyService _currencyService, IGLNonBaseCurrencyService _gLNonBaseCurrencyService);
    }
}