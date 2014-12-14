using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IPurchaseDownPaymentService
    {
        IPurchaseDownPaymentValidator GetValidator();
        IQueryable<PurchaseDownPayment> GetQueryable();
        IList<PurchaseDownPayment> GetAll();
        PurchaseDownPayment GetObjectById(int Id);
        IList<PurchaseDownPayment> GetObjectsByContactId(int contactId);
        PurchaseDownPayment CreateObject(PurchaseDownPayment purchaseDownPayment, IContactService _contactService);
        PurchaseDownPayment UpdateObject(PurchaseDownPayment purchaseDownPayment, IContactService _contactService);
        PurchaseDownPayment SoftDeleteObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService);
        bool DeleteObject(int Id);
        PurchaseDownPayment ConfirmObject(PurchaseDownPayment purchaseDownPayment, DateTime ConfirmationDate, IPayableService _payableService, IReceivableService _receivableService,
                                                 IContactService _contactService, IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService,
                                                 ICurrencyService _currencyService, IExchangeRateService _exchangeRateService, IGLNonBaseCurrencyService _glNonBaseCurrencyService);
        PurchaseDownPayment UnconfirmObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService,
                                                IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService, IPayableService _payableService, IReceivableService _receivableService,
                                                IContactService _contactService, IAccountService _accountService,
                                                IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService, ICurrencyService _currencyService, IGLNonBaseCurrencyService _gLNonBaseCurrencyService);
    }
}