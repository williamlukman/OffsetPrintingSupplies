using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ICashBankMutationService
    {
        IQueryable<CashBankMutation> GetQueryable();
        ICashBankMutationValidator GetValidator();
        IList<CashBankMutation> GetAll();
        CashBankMutation GetObjectById(int Id);
        CashBank GetSourceCashBank(CashBankMutation cashBankMutation);
        CashBank GetTargetCashBank(CashBankMutation cashBankMutation);
        CashBankMutation CreateObject(CashBankMutation cashBankMutation, ICashBankService _cashBankService);
        CashBankMutation UpdateObject(CashBankMutation cashBankMutation, ICashBankService _cashBankService);
        CashBankMutation SoftDeleteObject(CashBankMutation cashBankMutation);
        bool DeleteObject(int Id);
        CashBankMutation ConfirmObject(CashBankMutation cashBankMutation, DateTime ConfirmationDate, ICashMutationService _cashMutationService, ICashBankService _cashBankService,
                                       IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService,
                                       ICurrencyService _currencyService, IExchangeRateService _exchangeRateService);
        CashBankMutation UnconfirmObject(CashBankMutation cashBankMutation, ICashMutationService _cashMutationService, ICashBankService _cashBankService,
                                         IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService,
                                         ICurrencyService _currencyService);

    }
}