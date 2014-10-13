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
        IList<PurchaseDownPayment> GetObjectsByCashBankId(int cashBankId);
        IList<PurchaseDownPayment> GetObjectsByContactId(int contactId);
        PurchaseDownPayment CreateObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService, IPayableService _payableService,
                                    IContactService _contactService, ICashBankService _cashBankService);
        PurchaseDownPayment UpdateAmount(PurchaseDownPayment purchaseDownPayment);
        PurchaseDownPayment UpdateObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService, IPayableService _payableService,
                                    IContactService _contactService, ICashBankService _cashBankService);
        PurchaseDownPayment SoftDeleteObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService);
        bool DeleteObject(int Id);
        PurchaseDownPayment ConfirmObject(PurchaseDownPayment purchaseDownPayment, DateTime ConfirmationDate, IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService,
                                     ICashBankService _cashBankService, IPayableService _payableService, ICashMutationService _cashMutationService,
                                     IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        PurchaseDownPayment UnconfirmObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService,
                                       ICashBankService _cashBankService, IPayableService _payableService, ICashMutationService _cashMutationService,
                                       IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        PurchaseDownPayment ReconcileObject(PurchaseDownPayment purchaseDownPayment, DateTime ReconciliationDate, IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService,
                                       ICashMutationService _cashMutationService, ICashBankService _cashBankService, IPayableService _payableService,
                                       IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        PurchaseDownPayment UnreconcileObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService,
                                         ICashMutationService _cashMutationService, ICashBankService _cashBankService, IPayableService _payableService,
                                         IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
    }
}