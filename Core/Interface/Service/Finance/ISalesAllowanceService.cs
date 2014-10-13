using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ISalesAllowanceService
    {
        ISalesAllowanceValidator GetValidator();
        IQueryable<SalesAllowance> GetQueryable();
        IList<SalesAllowance> GetAll();
        SalesAllowance GetObjectById(int Id);
        IList<SalesAllowance> GetObjectsByCashBankId(int cashBankId);
        IList<SalesAllowance> GetObjectsByContactId(int contactId);
        SalesAllowance CreateObject(SalesAllowance salesAllowance, ISalesAllowanceDetailService _salesAllowanceDetailService, IReceivableService _receivableService,
                                    IContactService _contactService, ICashBankService _cashBankService);
        SalesAllowance UpdateAmount(SalesAllowance salesAllowance);
        SalesAllowance UpdateObject(SalesAllowance salesAllowance, ISalesAllowanceDetailService _salesAllowanceDetailService, IReceivableService _receivableService,
                                    IContactService _contactService, ICashBankService _cashBankService);
        SalesAllowance SoftDeleteObject(SalesAllowance salesAllowance, ISalesAllowanceDetailService _salesAllowanceDetailService);
        bool DeleteObject(int Id);
        SalesAllowance ConfirmObject(SalesAllowance salesAllowance, DateTime ConfirmationDate, ISalesAllowanceDetailService _salesAllowanceDetailService,
                                     ICashBankService _cashBankService, IReceivableService _receivableService, ICashMutationService _cashMutationService,
                                     IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        SalesAllowance UnconfirmObject(SalesAllowance salesAllowance, ISalesAllowanceDetailService _salesAllowanceDetailService,
                                       ICashBankService _cashBankService, IReceivableService _receivableService, ICashMutationService _cashMutationService,
                                       IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        SalesAllowance ReconcileObject(SalesAllowance salesAllowance, DateTime ReconciliationDate, ISalesAllowanceDetailService _salesAllowanceDetailService,
                                       ICashMutationService _cashMutationService, ICashBankService _cashBankService, IReceivableService _receivableService,
                                       IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
        SalesAllowance UnreconcileObject(SalesAllowance salesAllowance, ISalesAllowanceDetailService _salesAllowanceDetailService,
                                         ICashMutationService _cashMutationService, ICashBankService _cashBankService, IReceivableService _receivableService,
                                         IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService);
    }
}