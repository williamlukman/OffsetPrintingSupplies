using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface ISalesDownPaymentValidator
    {
        SalesDownPayment VHasContact(SalesDownPayment salesDownPayment, IContactService _contactService);
        SalesDownPayment VHasPayable(SalesDownPayment salesDownPayment, IPayableService _payableService);
        SalesDownPayment VHasReceivable(SalesDownPayment salesDownPayment, IReceivableService _receivableService);
        SalesDownPayment VHasDownPaymentDate(SalesDownPayment salesDownPayment);
        SalesDownPayment VReceivableHasNotBeenPaidAndHasNoSalesDownPaymentAllocation(SalesDownPayment salesDownPayment, IReceivableService _receivable,
                         ISalesDownPaymentAllocationService _salesDownPaymentAllocationService);
        SalesDownPayment VTotalAmountNotNegativeNorZero(SalesDownPayment salesDownPayment);
        SalesDownPayment VHasNotBeenDeleted(SalesDownPayment salesDownPayment);
        SalesDownPayment VHasBeenConfirmed(SalesDownPayment salesDownPayment);
        SalesDownPayment VHasNotBeenConfirmed(SalesDownPayment salesDownPayment);
        SalesDownPayment VGeneralLedgerPostingHasNotBeenClosed(SalesDownPayment salesDownPayment, IClosingService _closingService, int CaseConfirmUnconfirm);

        SalesDownPayment VCreateObject(SalesDownPayment salesDownPayment, ISalesDownPaymentService _salesDownPaymentService, IContactService _contactService);
        SalesDownPayment VUpdateObject(SalesDownPayment salesDownPayment, ISalesDownPaymentService _salesDownPaymentService, IContactService _contactService);
        SalesDownPayment VDeleteObject(SalesDownPayment salesDownPayment, ISalesDownPaymentAllocationService _salesDownPaymentAllocationService);
        SalesDownPayment VHasConfirmationDate(SalesDownPayment salesDownPayment);
        SalesDownPayment VConfirmObject(SalesDownPayment salesDownPayment, IReceivableService _receivableService, IPayableService _payableService, ISalesDownPaymentService _salesDownPaymentService, IContactService _contactService,
                                        IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        SalesDownPayment VUnconfirmObject(SalesDownPayment salesDownPayment, IReceivableService _receivableService, IPayableService _payableService, ISalesDownPaymentAllocationService _salesDownPaymentAllocationService,
                                          ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService,
                                          IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);

        bool ValidCreateObject(SalesDownPayment salesDownPayment, ISalesDownPaymentService _salesDownPaymentService, IContactService _contactService);
        bool ValidUpdateObject(SalesDownPayment salesDownPayment, ISalesDownPaymentService _salesDownPaymentService, IContactService _contactService);
        bool ValidDeleteObject(SalesDownPayment salesDownPayment, ISalesDownPaymentAllocationService _salesDownPaymentAllocationService);
        bool ValidConfirmObject(SalesDownPayment salesDownPayment, IReceivableService _receivableService, IPayableService _payableService, ISalesDownPaymentService _salesDownPaymentService, IContactService _contactService, 
                                IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        bool ValidUnconfirmObject(SalesDownPayment salesDownPayment, IReceivableService _receivableService, IPayableService _payableService, ISalesDownPaymentAllocationService _salesDownPaymentAllocationService,
                                  ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService,
                                  IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        bool isValid(SalesDownPayment salesDownPayment);
        string PrintError(SalesDownPayment salesDownPayment);
    }
}
