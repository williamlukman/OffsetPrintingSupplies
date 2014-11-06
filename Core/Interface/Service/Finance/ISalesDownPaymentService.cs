using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ISalesDownPaymentService
    {
        ISalesDownPaymentValidator GetValidator();
        IQueryable<SalesDownPayment> GetQueryable();
        IList<SalesDownPayment> GetAll();
        SalesDownPayment GetObjectById(int Id);
        IList<SalesDownPayment> GetObjectsByContactId(int contactId);
        SalesDownPayment CreateObject(SalesDownPayment salesDownPayment, IContactService _contactService);
        SalesDownPayment UpdateObject(SalesDownPayment salesDownPayment, IContactService _contactService);
        SalesDownPayment SoftDeleteObject(SalesDownPayment salesDownPayment, ISalesDownPaymentAllocationService _salesDownPaymentAllocationService);
        bool DeleteObject(int Id);
        SalesDownPayment ConfirmObject(SalesDownPayment salesDownPayment, DateTime ConfirmationDate, IReceivableService _receivableService,
                                       IPayableService _payableService, IContactService _contactService, IAccountService _accountService,
                                       IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        SalesDownPayment UnconfirmObject(SalesDownPayment salesDownPayment, ISalesDownPaymentAllocationService _salesDownPaymentAllocationService,
                                         ISalesDownPaymentAllocationDetailService _salesDownPaymentAllocationDetailService,
                                         IReceivableService _receivableService, IPayableService _payableService, IContactService _contactService,
                                         IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
    }
}