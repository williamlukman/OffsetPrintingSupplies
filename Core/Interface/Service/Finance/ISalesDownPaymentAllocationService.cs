using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ISalesDownPaymentAllocationService
    {
        ISalesDownPaymentAllocationValidator GetValidator();
        IQueryable<SalesDownPaymentAllocation> GetQueryable();
        IList<SalesDownPaymentAllocation> GetAll();
        SalesDownPaymentAllocation GetObjectById(int Id);
        SalesDownPaymentAllocation GetObjectBySalesDownPaymentId(int salesDownPaymentId);
        IList<SalesDownPaymentAllocation> GetObjectsByContactId(int contactId);
        SalesDownPaymentAllocation CreateObject(SalesDownPaymentAllocation salesDownPaymentAllocation, ISalesDownPaymentService _salesDownPaymentService, 
                                                   ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService, IContactService _contactService);
        SalesDownPaymentAllocation UpdateObject(SalesDownPaymentAllocation salesDownPaymentAllocation, ISalesDownPaymentService _salesDownPaymentService,
                                                   ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService, IContactService _contactService);
        SalesDownPaymentAllocation SoftDeleteObject(SalesDownPaymentAllocation salesDownPaymentAllocation, ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService);
        bool DeleteObject(int Id);
        SalesDownPaymentAllocation ConfirmObject(SalesDownPaymentAllocation salesDownPaymentAllocation, DateTime ConfirmationDate, ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService,
                                                 ISalesDownPaymentService _salesDownPaymentService, IReceivableService _receivableService, IReceiptVoucherDetailService _receiptVoucherDetailService, ICashBankService _cashBankService,
                                                 IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        SalesDownPaymentAllocation UnconfirmObject(SalesDownPaymentAllocation salesDownPaymentAllocation, ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService,
                                                   ISalesDownPaymentService _salesDownPaymentService, IReceivableService _receivableService, IReceiptVoucherDetailService _receiptVoucherDetailService,
                                                   IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
    }
}