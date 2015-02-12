using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IReceiptRequestService
    {
        IQueryable<ReceiptRequest> GetQueryable();
        IReceiptRequestValidator GetValidator();
        IList<ReceiptRequest> GetAll();
        ReceiptRequest GetObjectById(int Id);
        IList<ReceiptRequest> GetObjectsByContactId(int contactId);
        ReceiptRequest CreateObject(ReceiptRequest ReceiptRequest, IContactService _contactService, IReceiptRequestDetailService _ReceiptRequestDetailService,
                                    IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        ReceiptRequest UpdateObject(ReceiptRequest ReceiptRequest, IContactService _contactService, IReceiptRequestDetailService _ReceiptRequestDetailService,
                                    IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        ReceiptRequest SoftDeleteObject(ReceiptRequest ReceiptRequest);
        bool DeleteObject(int Id);
        ReceiptRequest ConfirmObject(ReceiptRequest ReceiptRequest, DateTime ConfirmationDate, IReceivableService _ReceivableService,
                                     IReceiptRequestDetailService _ReceiptRequestDetailService, IAccountService _accountService,
                                     IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService,
                                     IExchangeRateService _exchangeRateService, IGLNonBaseCurrencyService _gLNonBaseCurrencyService,
                                     ICurrencyService _currencyService);
        ReceiptRequest UnconfirmObject(ReceiptRequest ReceiptRequest, IReceiptRequestDetailService _ReceiptRequestDetailService, IReceivableService _ReceivableService,
                                       IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService,
                                       IGLNonBaseCurrencyService _gLNonBaseCurrencyService, ICurrencyService _currentService);
        ReceiptRequest CalculateTotalAmount(ReceiptRequest ReceiptRequest, IReceiptRequestDetailService _ReceiptRequestDetailService);
    }
}