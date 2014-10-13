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
        IList<SalesDownPayment> GetObjectsByCashBankId(int cashBankId);
        IList<SalesDownPayment> GetObjectsByContactId(int contactId);
        SalesDownPayment CreateObject(SalesDownPayment salesDownPayment, ISalesDownPaymentDetailService _salesDownPaymentDetailService, IReceivableService _receivableService,
                                    IContactService _contactService, ICashBankService _cashBankService);
        SalesDownPayment UpdateAmount(SalesDownPayment salesDownPayment);
        SalesDownPayment UpdateObject(SalesDownPayment salesDownPayment, ISalesDownPaymentDetailService _salesDownPaymentDetailService, IReceivableService _receivableService,
                                    IContactService _contactService, ICashBankService _cashBankService);
        SalesDownPayment SoftDeleteObject(SalesDownPayment salesDownPayment, ISalesDownPaymentDetailService _salesDownPaymentDetailService);
        bool DeleteObject(int Id);
        SalesDownPayment ConfirmObject(SalesDownPayment salesDownPayment, DateTime ConfirmationDate, ISalesDownPaymentDetailService _salesDownPaymentDetailService,
                                     ICashBankService _cashBankService, IReceivableService _receivableService, ICashMutationService _cashMutationService,
                                     IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        SalesDownPayment UnconfirmObject(SalesDownPayment salesDownPayment, ISalesDownPaymentDetailService _salesDownPaymentDetailService,
                                       ICashBankService _cashBankService, IReceivableService _receivableService, ICashMutationService _cashMutationService,
                                       IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        SalesDownPayment ReconcileObject(SalesDownPayment salesDownPayment, DateTime ReconciliationDate, ISalesDownPaymentDetailService _salesDownPaymentDetailService,
                                       ICashMutationService _cashMutationService, ICashBankService _cashBankService, IReceivableService _receivableService,
                                       IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        SalesDownPayment UnreconcileObject(SalesDownPayment salesDownPayment, ISalesDownPaymentDetailService _salesDownPaymentDetailService,
                                         ICashMutationService _cashMutationService, ICashBankService _cashBankService, IReceivableService _receivableService,
                                         IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
    }
}