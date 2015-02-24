using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IInterestAdjustmentService
    {
        IQueryable<InterestAdjustment> GetQueryable();
        IInterestAdjustmentValidator GetValidator();
        IList<InterestAdjustment> GetAll();
        IList<InterestAdjustment> GetObjectsByCashBankId(int cashBankId);
        InterestAdjustment GetObjectById(int Id);
        InterestAdjustment CreateObject(InterestAdjustment interestAdjustment, ICashBankService _cashBankService);
        InterestAdjustment CreateObject(int CashBankId, DateTime interestDate, ICashBankService _cashBankService);
        InterestAdjustment UpdateObject(InterestAdjustment interestAdjustment, ICashBankService _cashBankService);
        InterestAdjustment SoftDeleteObject(InterestAdjustment interestAdjustment);
        bool DeleteObject(int Id);
        InterestAdjustment ConfirmObject(InterestAdjustment interestAdjustment, DateTime ConfirmationDate, ICashMutationService _cashMutationService, ICashBankService _cashBankService,
                                         IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService,
                                         ICurrencyService _currencyService, IExchangeRateService _exchangeRateService,IGLNonBaseCurrencyService _gLNonBaseCurrencyService);
        InterestAdjustment UnconfirmObject(InterestAdjustment interestAdjustment, ICashMutationService _cashMutationService, ICashBankService _cashBankService,
                                           IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService, ICurrencyService _currencyService, IGLNonBaseCurrencyService _gLNonBaseCurrencyService);
    }
}