using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IBankAdministrationService
    {
        IQueryable<BankAdministration> GetQueryable();
        IBankAdministrationValidator GetValidator();
        IList<BankAdministration> GetAll();
        IList<BankAdministration> GetObjectsByCashBankId(int cashBankId);
        BankAdministration GetObjectById(int Id);
        BankAdministration CreateObject(BankAdministration bankAdministration, ICashBankService _cashBankService, ICurrencyService _currencyService, IExchangeRateService _exchangeRateService);
        BankAdministration CreateObject(int CashBankId, DateTime interestDate, ICashBankService _cashBankService, ICurrencyService _currencyService, IExchangeRateService _exchangeRateService);
        BankAdministration UpdateObject(BankAdministration bankAdministration, ICashBankService _cashBankService, ICurrencyService _currencyService, IExchangeRateService _exchangeRateService);
        BankAdministration SoftDeleteObject(BankAdministration bankAdministration);
        bool DeleteObject(int Id);
        BankAdministration ConfirmObject(BankAdministration bankAdministration, DateTime ConfirmationDate, ICashMutationService _cashMutationService, ICashBankService _cashBankService,
                                         IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService,
                                         ICurrencyService _currencyService, IExchangeRateService _exchangeRateService, IGLNonBaseCurrencyService _gLNonBaseCurrencyService, IBankAdministrationDetailService _bankAdministrationDetailService);
        BankAdministration UnconfirmObject(BankAdministration bankAdministration, ICashMutationService _cashMutationService, ICashBankService _cashBankService,
                                           IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService,
                                           ICurrencyService _currencyService, IGLNonBaseCurrencyService _gLNonBaseCurrencyService, IBankAdministrationDetailService _bankAdministrationDetailService);
        BankAdministration CalculateTotalAmount(BankAdministration bankAdministration, IBankAdministrationDetailService _bankAdministrationDetailService);
    }
}