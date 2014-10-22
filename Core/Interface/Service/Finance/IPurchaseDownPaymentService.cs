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
        PurchaseDownPayment CreateObject(PurchaseDownPayment purchaseDownPayment, IContactService _contactService,
                                         IPaymentVoucherDetailService _paymentVoucherDetailService, IPayableService _payableService,
                                         ICashBankService _cashBankService, IPaymentVoucherService _paymentVoucherService);
        PurchaseDownPayment UpdateObject(PurchaseDownPayment purchaseDownPayment, IContactService _contactService,
                                         IPaymentVoucherDetailService _paymentVoucherDetailService, IPayableService _payableService,
                                         ICashBankService _cashBankService, IPaymentVoucherService _paymentVoucherService);
        PurchaseDownPayment SoftDeleteObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService,
                                             IPaymentVoucherDetailService _paymentVoucherDetailService, IPaymentVoucherService _paymentVoucherService);
        bool DeleteObject(int Id);
        PurchaseDownPayment ConfirmObject(PurchaseDownPayment purchaseDownPayment, DateTime ConfirmationDate, ICashBankService _cashBankService, IPaymentVoucherService _paymentVoucherService,
                                          IPaymentVoucherDetailService _paymentVoucherDetailService, IPayableService _payableService, IContactService _contactService,
                                          IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        PurchaseDownPayment UnconfirmObject(PurchaseDownPayment purchaseDownPayment, ICashBankService _cashBankService,
                                            IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService, IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService,
                                            IPaymentVoucherService _paymentVoucherService, IPaymentVoucherDetailService _paymentVoucherDetailService, IPayableService _payableService, IContactService _contactService,
                                            IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
    }
}