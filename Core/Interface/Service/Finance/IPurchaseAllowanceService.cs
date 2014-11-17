using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IPurchaseAllowanceService
    {
        IPurchaseAllowanceValidator GetValidator();
        IQueryable<PurchaseAllowance> GetQueryable();
        IList<PurchaseAllowance> GetAll();
        PurchaseAllowance GetObjectById(int Id);
        IList<PurchaseAllowance> GetObjectsByCashBankId(int cashBankId);
        IList<PurchaseAllowance> GetObjectsByContactId(int contactId);
        PurchaseAllowance CreateObject(PurchaseAllowance purchaseAllowance, IPurchaseAllowanceDetailService _purchaseAllowanceDetailService, IPayableService _payableService,
                                    IContactService _contactService, ICashBankService _cashBankService);
        PurchaseAllowance UpdateAmount(PurchaseAllowance purchaseAllowance);
        PurchaseAllowance UpdateObject(PurchaseAllowance purchaseAllowance, IPurchaseAllowanceDetailService _purchaseAllowanceDetailService, IPayableService _payableService,
                                    IContactService _contactService, ICashBankService _cashBankService);
        PurchaseAllowance SoftDeleteObject(PurchaseAllowance purchaseAllowance, IPurchaseAllowanceDetailService _purchaseAllowanceDetailService);
        bool DeleteObject(int Id);
        PurchaseAllowance ConfirmObject(PurchaseAllowance purchaseAllowance, DateTime ConfirmationDate, IPurchaseAllowanceDetailService _purchaseAllowanceDetailService,
                                     ICashBankService _cashBankService, IPayableService _payableService, ICashMutationService _cashMutationService,
                                     IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService, ICurrencyService _currencyService);
        PurchaseAllowance UnconfirmObject(PurchaseAllowance purchaseAllowance, IPurchaseAllowanceDetailService _purchaseAllowanceDetailService,
                                       ICashBankService _cashBankService, IPayableService _payableService, ICashMutationService _cashMutationService,
                                       IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService, ICurrencyService _currencyService);
        PurchaseAllowance ReconcileObject(PurchaseAllowance purchaseAllowance, DateTime ReconciliationDate, IPurchaseAllowanceDetailService _purchaseAllowanceDetailService,
                                       ICashMutationService _cashMutationService, ICashBankService _cashBankService, IPayableService _payableService,
                                       IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService, ICurrencyService _currencyService);
        PurchaseAllowance UnreconcileObject(PurchaseAllowance purchaseAllowance, IPurchaseAllowanceDetailService _purchaseAllowanceDetailService,
                                         ICashMutationService _cashMutationService, ICashBankService _cashBankService, IPayableService _payableService,
                                         IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService, ICurrencyService _currencyService);
    }
}