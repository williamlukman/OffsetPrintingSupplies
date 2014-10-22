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
        SalesDownPaymentAllocation VHasContact(SalesDownPaymentAllocation salesDownPaymentAllocation, IContactService _contactService);
        SalesDownPaymentAllocation VHasSalesDownPayment(SalesDownPaymentAllocation salesDownPaymentAllocation, ISalesDownPaymentService _salesDownPaymentService);
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
                                      ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService, IReceiptVoucherDetailService _receiptVoucherDetailService, IReceivableService _receivableService);
        SalesDownPaymentAllocation VCashBankIsGreaterThanOrEqualSalesDownPaymentAllocationDetails(SalesDownPaymentAllocation salesDownPaymentAllocation,
                                      ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService, ISalesDownPaymentService _salesDownPaymentService,
                                      ICashBankService _cashBankService);
        SalesDownPaymentAllocation VGeneralLedgerPostingHasNotBeenClosed(SalesDownPaymentAllocation salesDownPaymentAllocation, IClosingService _closingService, int CaseConfirmUnconfirm);
        SalesDownPaymentAllocation VCreateObject(SalesDownPaymentAllocation salesDownPaymentAllocation, ISalesDownPaymentAllocationService _salesDownPaymentAllocationService,
                                                    ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService, ISalesDownPaymentService _salesDownPaymentService,
                                                    IContactService _contactService);
        SalesDownPaymentAllocation VUpdateObject(SalesDownPaymentAllocation salesDownPaymentAllocation, ISalesDownPaymentAllocationService _salesDownPaymentAllocationService,
                                                    ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService, ISalesDownPaymentService _salesDownPaymentService,
                                                    IContactService _contactService);
        SalesDownPaymentAllocation VDeleteObject(SalesDownPaymentAllocation salesDownPaymentAllocation, ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService);
        SalesDownPaymentAllocation VHasConfirmationDate(SalesDownPaymentAllocation salesDownPaymentAllocation);
        SalesDownPaymentAllocation VConfirmObject(SalesDownPaymentAllocation salesDownPaymentAllocation, ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService,
                                   ISalesDownPaymentService _salesDownPaymentService, IReceivableService _receivableService, IReceiptVoucherDetailService _receiptVoucherDetailService, ICashBankService _cashBankService,
                                   IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        SalesDownPaymentAllocation VUnconfirmObject(SalesDownPaymentAllocation salesDownPaymentAllocation, ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService,
                                   IReceivableService _receivableService, IReceiptVoucherDetailService _receiptVoucherDetailService,
                                   IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        bool ValidCreateObject(SalesDownPaymentAllocation salesDownPaymentAllocation, ISalesDownPaymentAllocationService _salesDownPaymentAllocationService,
                               ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService, ISalesDownPaymentService _salesDownPaymentService,
                               IContactService _contactService);
        bool ValidUpdateObject(SalesDownPaymentAllocation salesDownPaymentAllocation, ISalesDownPaymentAllocationService _salesDownPaymentAllocationService,
                               ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService, ISalesDownPaymentService _salesDownPaymentService,
                               IContactService _contactService);
        bool ValidDeleteObject(SalesDownPaymentAllocation salesDownPaymentAllocation, ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService);
        bool ValidConfirmObject(SalesDownPaymentAllocation salesDownPaymentAllocation, ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService,
                                ISalesDownPaymentService _salesDownPaymentService, IReceivableService _receivableService, IReceiptVoucherDetailService _receiptVoucherDetailService, ICashBankService _cashBankService,
                                IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        bool ValidUnconfirmObject(SalesDownPaymentAllocation salesDownPaymentAllocation, ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService,
                                  IReceivableService _receivableService, IReceiptVoucherDetailService _receiptVoucherDetailService,
                                  IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        bool isValid(SalesDownPaymentAllocation salesDownPaymentAllocation);
        string PrintError(SalesDownPaymentAllocation salesDownPaymentAllocation);
    }
}
