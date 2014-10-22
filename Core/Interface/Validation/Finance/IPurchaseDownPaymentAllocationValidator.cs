using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IPurchaseDownPaymentAllocationValidator
    {
        PurchaseDownPaymentAllocation VHasContact(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation, IContactService _contactService);
        PurchaseDownPaymentAllocation VHasPurchaseDownPayment(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation, IPurchaseDownPaymentService _purchaseDownPaymentService);
        PurchaseDownPaymentAllocation VHasAllocationDate(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation);
        PurchaseDownPaymentAllocation VHasNoPurchaseDownPaymentAllocationDetail(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation,
                                      IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService);
        PurchaseDownPaymentAllocation VHasPurchaseDownPaymentAllocationDetails(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation,
                                      IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService);
        PurchaseDownPaymentAllocation VTotalAmountNotNegativeNorZero(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation);
        PurchaseDownPaymentAllocation VHasNotBeenDeleted(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation);
        PurchaseDownPaymentAllocation VHasBeenConfirmed(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation);
        PurchaseDownPaymentAllocation VHasNotBeenConfirmed(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation);
        PurchaseDownPaymentAllocation VTotalAmountEqualDetailsAmount(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation,
                                      IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService);
        PurchaseDownPaymentAllocation VAllPurchaseDownPaymentAllocationDetailsAreConfirmable(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation,
                                      IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService, IPaymentVoucherDetailService _paymentVoucherDetailService, IPayableService _payableService);
        PurchaseDownPaymentAllocation VCashBankIsGreaterThanOrEqualPurchaseDownPaymentAllocationDetails(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation,
                                      IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService, IPurchaseDownPaymentService _purchaseDownPaymentService,
                                      ICashBankService _cashBankService);
        PurchaseDownPaymentAllocation VGeneralLedgerPostingHasNotBeenClosed(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation, IClosingService _closingService, int CaseConfirmUnconfirm);
        PurchaseDownPaymentAllocation VCreateObject(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation, IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService,
                                                    IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService, IPurchaseDownPaymentService _purchaseDownPaymentService,
                                                    IContactService _contactService);
        PurchaseDownPaymentAllocation VUpdateObject(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation, IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService,
                                                    IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService, IPurchaseDownPaymentService _purchaseDownPaymentService,
                                                    IContactService _contactService);
        PurchaseDownPaymentAllocation VDeleteObject(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation, IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService);
        PurchaseDownPaymentAllocation VHasConfirmationDate(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation);
        PurchaseDownPaymentAllocation VConfirmObject(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation, IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService,
                                      IPurchaseDownPaymentService _purchaseDownPaymentService, IPayableService _payableService, IPaymentVoucherDetailService _paymentVoucherDetailService, ICashBankService _cashBankService,
                                      IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        PurchaseDownPaymentAllocation VUnconfirmObject(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation, IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService,
                                      IPayableService _payableService, IPaymentVoucherDetailService _paymentVoucherDetailService,
                                      IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        bool ValidCreateObject(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation, IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService,
                               IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService, IPurchaseDownPaymentService _purchaseDownPaymentService,
                               IContactService _contactService);
        bool ValidUpdateObject(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation, IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService,
                               IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService, IPurchaseDownPaymentService _purchaseDownPaymentService,
                               IContactService _contactService);
        bool ValidDeleteObject(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation, IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService);
        bool ValidConfirmObject(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation, IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService,
                                IPurchaseDownPaymentService _purchaseDownPaymentService, IPayableService _payableService, IPaymentVoucherDetailService _paymentVoucherDetailService, ICashBankService _cashBankService,
                                IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        bool ValidUnconfirmObject(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation, IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService,
                                  IPayableService _payableService, IPaymentVoucherDetailService _paymentVoucherDetailService,
                                  IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        bool isValid(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation);
        string PrintError(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation);
    }
}
