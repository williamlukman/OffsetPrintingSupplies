using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IPurchaseDownPaymentAllocationService
    {
        IPurchaseDownPaymentAllocationValidator GetValidator();
        IQueryable<PurchaseDownPaymentAllocation> GetQueryable();
        IList<PurchaseDownPaymentAllocation> GetAll();
        PurchaseDownPaymentAllocation GetObjectById(int Id);
        PurchaseDownPaymentAllocation GetObjectByPurchaseDownPaymentId(int purchaseDownPaymentId);
        IList<PurchaseDownPaymentAllocation> GetObjectsByContactId(int contactId);
        PurchaseDownPaymentAllocation CreateObject(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation, IPurchaseDownPaymentService _purchaseDownPaymentService, 
                                                   IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService, IContactService _contactService);
        PurchaseDownPaymentAllocation UpdateObject(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation, IPurchaseDownPaymentService _purchaseDownPaymentService,
                                                   IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService, IContactService _contactService);
        PurchaseDownPaymentAllocation SoftDeleteObject(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation, IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService);
        bool DeleteObject(int Id);
        PurchaseDownPaymentAllocation ConfirmObject(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation, DateTime ConfirmationDate, IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService,
                                                    IPurchaseDownPaymentService _purchaseDownPaymentService, IPayableService _payableService, IPaymentVoucherDetailService _paymentVoucherDetailService, ICashBankService _cashBankService,
                                                    IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        PurchaseDownPaymentAllocation UnconfirmObject(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation, IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService,
                                                      IPurchaseDownPaymentService _purchaseDownPaymentService, IPayableService _payableService, IPaymentVoucherDetailService _paymentVoucherDetailService,
                                                      IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
    }
}