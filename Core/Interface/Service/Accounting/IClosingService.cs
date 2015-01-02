using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IClosingService
    {
        IQueryable<Closing> GetQueryable();
        IClosingValidator GetValidator();
        IList<Closing> GetAll();
        Closing GetObjectById(int Id);
        Closing GetObjectByPeriodAndYear(int Period, int YearPeriod);
        Closing CreateObject(Closing closing, IList<ExchangeRateClosing> exchangeRateClosing,IAccountService _accountService, IValidCombService _validCombService,IExchangeRateClosingService _exchangeRateClosingService);
        Closing CloseObject(Closing closing, IAccountService _accountService,
                                   IGeneralLedgerJournalService _generalLedgerJournalService, IValidCombService _validCombService,
            IGLNonBaseCurrencyService _gLNonBaseCurrencyService, IExchangeRateClosingService _exchangeRateClosingService,
            IVCNonBaseCurrencyService _vCNonBaseCurrencyService,ICashBankService _cashBankService);
        Closing OpenObject(Closing closing, IAccountService _accountService, IValidCombService _validCombService, IVCNonBaseCurrencyService _vCNonBaseCurrencyService, IGeneralLedgerJournalService _generalLedgerJournalService
            , IExchangeRateClosingService _exchangeRateClosingService);
        Closing DeleteObject(Closing closing, IAccountService _accountService, IValidCombService _validCombService, IVCNonBaseCurrencyService _vCNonBaseCurrencyService);
        bool IsDateClosed(DateTime DateToCheck);
    }
}