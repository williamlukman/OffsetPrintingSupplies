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
        PurchaseDownPaymentAllocation VHasReceivable(PurchaseDownPaymentAllocation salesDownPaymentAllocation, IReceivableService _receivableService);
        PurchaseDownPaymentAllocation VHasContact(PurchaseDownPaymentAllocation salesDownPaymentAllocation, IContactService _contactService);
        PurchaseDownPaymentAllocation VHasAllocationDate(PurchaseDownPaymentAllocation salesDownPaymentAllocation);
        PurchaseDownPaymentAllocation VHasNoPurchaseDownPaymentAllocationDetail(PurchaseDownPaymentAllocation salesDownPaymentAllocation,
                                      IPurchaseDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService);
        PurchaseDownPaymentAllocation VHasPurchaseDownPaymentAllocationDetails(PurchaseDownPaymentAllocation salesDownPaymentAllocation,
                                      IPurchaseDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService);
        PurchaseDownPaymentAllocation VTotalAmountNotNegativeNorZero(PurchaseDownPaymentAllocation salesDownPaymentAllocation);
        PurchaseDownPaymentAllocation VHasNotBeenDeleted(PurchaseDownPaymentAllocation salesDownPaymentAllocation);
        PurchaseDownPaymentAllocation VHasBeenConfirmed(PurchaseDownPaymentAllocation salesDownPaymentAllocation);
        PurchaseDownPaymentAllocation VHasNotBeenConfirmed(PurchaseDownPaymentAllocation salesDownPaymentAllocation);
        PurchaseDownPaymentAllocation VTotalAmountEqualDetailsAmount(PurchaseDownPaymentAllocation salesDownPaymentAllocation,
                                      IPurchaseDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService);
        PurchaseDownPaymentAllocation VAllPurchaseDownPaymentAllocationDetailsAreConfirmable(PurchaseDownPaymentAllocation salesDownPaymentAllocation,
                                      IPurchaseDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService, IPayableService _payableService, IReceivableService _receivableService);
        PurchaseDownPaymentAllocation VGeneralLedgerPostingHasNotBeenClosed(PurchaseDownPaymentAllocation salesDownPaymentAllocation, IClosingService _closingService, int CaseConfirmUnconfirm);
        PurchaseDownPaymentAllocation VCreateObject(PurchaseDownPaymentAllocation salesDownPaymentAllocation, IPurchaseDownPaymentAllocationService _salesDownPaymentAllocationService,
                                                 IPurchaseDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService, IPurchaseDownPaymentService _salesDownPaymentService,
                                                 IContactService _contactService, IReceivableService _receivableService);
        PurchaseDownPaymentAllocation VUpdateObject(PurchaseDownPaymentAllocation salesDownPaymentAllocation, IPurchaseDownPaymentAllocationService _salesDownPaymentAllocationService,
                                                    IPurchaseDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService, IPurchaseDownPaymentService _salesDownPaymentService,
                                                    IContactService _contactService, IReceivableService _receivableService);
        PurchaseDownPaymentAllocation VDeleteObject(PurchaseDownPaymentAllocation salesDownPaymentAllocation, IPurchaseDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService);
        PurchaseDownPaymentAllocation VHasConfirmationDate(PurchaseDownPaymentAllocation salesDownPaymentAllocation);
        PurchaseDownPaymentAllocation VConfirmObject(PurchaseDownPaymentAllocation salesDownPaymentAllocation, IPurchaseDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService,
                                                  IPurchaseDownPaymentService _salesDownPaymentService, IPayableService _payableService, IReceivableService _receivableService,
                                                  IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        PurchaseDownPaymentAllocation VUnconfirmObject(PurchaseDownPaymentAllocation salesDownPaymentAllocation, IPurchaseDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService,
                                                    IPayableService _payableService, IReceivableService _receivableService, IAccountService _accountService,
                                                    IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        bool ValidCreateObject(PurchaseDownPaymentAllocation salesDownPaymentAllocation, IPurchaseDownPaymentAllocationService _salesDownPaymentAllocationService,
                               IPurchaseDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService, IPurchaseDownPaymentService _salesDownPaymentService,
                               IContactService _contactService, IReceivableService _receivableService);
        bool ValidUpdateObject(PurchaseDownPaymentAllocation salesDownPaymentAllocation, IPurchaseDownPaymentAllocationService _salesDownPaymentAllocationService,
                               IPurchaseDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService, IPurchaseDownPaymentService _salesDownPaymentService,
                               IContactService _contactService, IReceivableService _receivableService);
        bool ValidDeleteObject(PurchaseDownPaymentAllocation salesDownPaymentAllocation, IPurchaseDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService);
        bool ValidConfirmObject(PurchaseDownPaymentAllocation salesDownPaymentAllocation, IPurchaseDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService,
                                IPurchaseDownPaymentService _salesDownPaymentService, IPayableService _payableService, IReceivableService _receivableService,
                                IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        bool ValidUnconfirmObject(PurchaseDownPaymentAllocation salesDownPaymentAllocation, IPurchaseDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService,
                                  IPayableService _payableService, IReceivableService _receivableService, IAccountService _accountService,
                                  IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        bool isValid(PurchaseDownPaymentAllocation salesDownPaymentAllocation);
        string PrintError(PurchaseDownPaymentAllocation salesDownPaymentAllocation);
    }
}
