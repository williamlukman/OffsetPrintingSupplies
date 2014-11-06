using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface ISalesDownPaymentAllocationValidator
    {
        SalesDownPaymentAllocation VHasPayable(SalesDownPaymentAllocation salesDownPaymentAllocation, IPayableService _payableService);
        SalesDownPaymentAllocation VHasContact(SalesDownPaymentAllocation salesDownPaymentAllocation, IContactService _contactService);
        SalesDownPaymentAllocation VHasAllocationDate(SalesDownPaymentAllocation salesDownPaymentAllocation);
        SalesDownPaymentAllocation VHasNoSalesDownPaymentAllocationDetail(SalesDownPaymentAllocation salesDownPaymentAllocation,
                                      ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService);
        SalesDownPaymentAllocation VHasSalesDownPaymentAllocationDetails(SalesDownPaymentAllocation salesDownPaymentAllocation,
                                      ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService);
        SalesDownPaymentAllocation VTotalAmountNotNegativeNorZero(SalesDownPaymentAllocation salesDownPaymentAllocation);
        SalesDownPaymentAllocation VHasNotBeenDeleted(SalesDownPaymentAllocation salesDownPaymentAllocation);
        SalesDownPaymentAllocation VHasBeenConfirmed(SalesDownPaymentAllocation salesDownPaymentAllocation);
        SalesDownPaymentAllocation VHasNotBeenConfirmed(SalesDownPaymentAllocation salesDownPaymentAllocation);
        SalesDownPaymentAllocation VTotalAmountEqualDetailsAmount(SalesDownPaymentAllocation salesDownPaymentAllocation,
                                      ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService);
        SalesDownPaymentAllocation VAllSalesDownPaymentAllocationDetailsAreConfirmable(SalesDownPaymentAllocation salesDownPaymentAllocation,
                                      ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService, IReceivableService _receivableService, IPayableService _payableService);
        SalesDownPaymentAllocation VGeneralLedgerPostingHasNotBeenClosed(SalesDownPaymentAllocation salesDownPaymentAllocation, IClosingService _closingService, int CaseConfirmUnconfirm);
        SalesDownPaymentAllocation VCreateObject(SalesDownPaymentAllocation salesDownPaymentAllocation, ISalesDownPaymentAllocationService _salesDownPaymentAllocationService,
                                                 ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService, ISalesDownPaymentService _salesDownPaymentService,
                                                 IContactService _contactService, IPayableService _payableService);
        SalesDownPaymentAllocation VUpdateObject(SalesDownPaymentAllocation salesDownPaymentAllocation, ISalesDownPaymentAllocationService _salesDownPaymentAllocationService,
                                                    ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService, ISalesDownPaymentService _salesDownPaymentService,
                                                    IContactService _contactService, IPayableService _payableService);
        SalesDownPaymentAllocation VDeleteObject(SalesDownPaymentAllocation salesDownPaymentAllocation, ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService);
        SalesDownPaymentAllocation VHasConfirmationDate(SalesDownPaymentAllocation salesDownPaymentAllocation);
        SalesDownPaymentAllocation VConfirmObject(SalesDownPaymentAllocation salesDownPaymentAllocation, ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService,
                                                  ISalesDownPaymentService _salesDownPaymentService, IReceivableService _receivableService, IPayableService _payableService,
                                                  IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        SalesDownPaymentAllocation VUnconfirmObject(SalesDownPaymentAllocation salesDownPaymentAllocation, ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService,
                                                    IReceivableService _receivableService, IPayableService _payableService, IAccountService _accountService,
                                                    IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        bool ValidCreateObject(SalesDownPaymentAllocation salesDownPaymentAllocation, ISalesDownPaymentAllocationService _salesDownPaymentAllocationService,
                               ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService, ISalesDownPaymentService _salesDownPaymentService,
                               IContactService _contactService, IPayableService _payableService);
        bool ValidUpdateObject(SalesDownPaymentAllocation salesDownPaymentAllocation, ISalesDownPaymentAllocationService _salesDownPaymentAllocationService,
                               ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService, ISalesDownPaymentService _salesDownPaymentService,
                               IContactService _contactService, IPayableService _payableService);
        bool ValidDeleteObject(SalesDownPaymentAllocation salesDownPaymentAllocation, ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService);
        bool ValidConfirmObject(SalesDownPaymentAllocation salesDownPaymentAllocation, ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService,
                                ISalesDownPaymentService _salesDownPaymentService, IReceivableService _receivableService, IPayableService _payableService,
                                IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        bool ValidUnconfirmObject(SalesDownPaymentAllocation salesDownPaymentAllocation, ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService,
                                  IReceivableService _receivableService, IPayableService _payableService, IAccountService _accountService,
                                  IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        bool isValid(SalesDownPaymentAllocation salesDownPaymentAllocation);
        string PrintError(SalesDownPaymentAllocation salesDownPaymentAllocation);
    }
}
