using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IPurchaseDownPaymentValidator
    {
        PurchaseDownPayment VHasContact(PurchaseDownPayment purchaseDownPayment, IContactService _contactService);
        PurchaseDownPayment VHasReceivable(PurchaseDownPayment purchaseDownPayment, IReceivableService _receivableService);
        PurchaseDownPayment VHasPayable(PurchaseDownPayment purchaseDownPayment, IPayableService _payableService);
        PurchaseDownPayment VHasDownPaymentDate(PurchaseDownPayment purchaseDownPayment);
        PurchaseDownPayment VPayableHasNotBeenPaidAndHasNoPurchaseDownPaymentAllocation(PurchaseDownPayment purchaseDownPayment, IPayableService _payable,
                         IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService);
        PurchaseDownPayment VTotalAmountNotNegativeNorZero(PurchaseDownPayment purchaseDownPayment);
        PurchaseDownPayment VHasNotBeenDeleted(PurchaseDownPayment purchaseDownPayment);
        PurchaseDownPayment VHasBeenConfirmed(PurchaseDownPayment purchaseDownPayment);
        PurchaseDownPayment VHasNotBeenConfirmed(PurchaseDownPayment purchaseDownPayment);
        PurchaseDownPayment VGeneralLedgerPostingHasNotBeenClosed(PurchaseDownPayment purchaseDownPayment, IClosingService _closingService, int CaseConfirmUnconfirm);

        PurchaseDownPayment VCreateObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentService _purchaseDownPaymentService, IContactService _contactService);
        PurchaseDownPayment VUpdateObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentService _purchaseDownPaymentService, IContactService _contactService);
        PurchaseDownPayment VDeleteObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService);
        PurchaseDownPayment VHasConfirmationDate(PurchaseDownPayment purchaseDownPayment);
        PurchaseDownPayment VConfirmObject(PurchaseDownPayment purchaseDownPayment, IPayableService _payableService, IReceivableService _receivableService, IPurchaseDownPaymentService _purchaseDownPaymentService, IContactService _contactService,
                                        IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        PurchaseDownPayment VUnconfirmObject(PurchaseDownPayment purchaseDownPayment, IPayableService _payableService, IReceivableService _receivableService, IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService,
                                          IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService,
                                          IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);

        bool ValidCreateObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentService _purchaseDownPaymentService, IContactService _contactService);
        bool ValidUpdateObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentService _purchaseDownPaymentService, IContactService _contactService);
        bool ValidDeleteObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService);
        bool ValidConfirmObject(PurchaseDownPayment purchaseDownPayment, IPayableService _payableService, IReceivableService _receivableService, IPurchaseDownPaymentService _purchaseDownPaymentService, IContactService _contactService,
                                IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        bool ValidUnconfirmObject(PurchaseDownPayment purchaseDownPayment, IPayableService _payableService, IReceivableService _receivableService, IPurchaseDownPaymentAllocationService _purchaseDownPaymentAllocationService,
                                  IPurchaseDownPaymentAllocationDetailService _purchaseDownPaymentAllocationDetailService,
                                  IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        bool isValid(PurchaseDownPayment purchaseDownPayment);
        string PrintError(PurchaseDownPayment purchaseDownPayment);
    }
}
