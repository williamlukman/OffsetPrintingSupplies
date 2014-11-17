using Core.Constants;
using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Service
{
    public class SalesAllowanceService : ISalesAllowanceService
    {
        private ISalesAllowanceRepository _repository;
        private ISalesAllowanceValidator _validator;

        public SalesAllowanceService(ISalesAllowanceRepository _salesAllowanceRepository, ISalesAllowanceValidator _salesAllowanceValidator)
        {
            _repository = _salesAllowanceRepository;
            _validator = _salesAllowanceValidator;
        }

        public ISalesAllowanceValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<SalesAllowance> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<SalesAllowance> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<SalesAllowance> GetObjectsByCashBankId(int cashBankId)
        {
            return _repository.GetObjectsByCashBankId(cashBankId);
        }

        public SalesAllowance GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public IList<SalesAllowance> GetObjectsByContactId(int contactId)
        {
            return _repository.GetObjectsByContactId(contactId);
        }

        public SalesAllowance CreateObject(SalesAllowance salesAllowance, ISalesAllowanceDetailService _salesAllowanceDetailService,
                                            IReceivableService _receivableService, IContactService _contactService, ICashBankService _cashBankService)
        {
            salesAllowance.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(salesAllowance, this, _salesAllowanceDetailService, _receivableService, _contactService, _cashBankService) ?
                    _repository.CreateObject(salesAllowance) : salesAllowance);
        }

        public SalesAllowance UpdateObject(SalesAllowance salesAllowance, ISalesAllowanceDetailService _salesAllowanceDetailService, IReceivableService _receivableService, IContactService _contactService, ICashBankService _cashBankService)
        {
            return (_validator.ValidUpdateObject(salesAllowance, this, _salesAllowanceDetailService, _receivableService, _contactService, _cashBankService) ? _repository.UpdateObject(salesAllowance) : salesAllowance);
        }

        public SalesAllowance UpdateAmount(SalesAllowance salesAllowance)
        {
            return _repository.UpdateObject(salesAllowance);
        }

        public SalesAllowance SoftDeleteObject(SalesAllowance salesAllowance, ISalesAllowanceDetailService _salesAllowanceDetailService)
        {
            return (_validator.ValidDeleteObject(salesAllowance, _salesAllowanceDetailService) ? _repository.SoftDeleteObject(salesAllowance) : salesAllowance);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public SalesAllowance ConfirmObject(SalesAllowance salesAllowance, DateTime ConfirmationDate, ISalesAllowanceDetailService _salesAllowanceDetailService,
                                            ICashBankService _cashBankService, IReceivableService _receivableService, ICashMutationService _cashMutationService,
                                            IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService, ICurrencyService _currencyService)
        {
            salesAllowance.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(salesAllowance, this, _salesAllowanceDetailService, _cashBankService, _receivableService, _closingService))
            {
                IList<SalesAllowanceDetail> details = _salesAllowanceDetailService.GetObjectsBySalesAllowanceId(salesAllowance.Id);
                foreach (var detail in details)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _salesAllowanceDetailService.ConfirmObject(detail, ConfirmationDate, this, _receivableService);
                }
                _repository.ConfirmObject(salesAllowance);

                if (!salesAllowance.IsGBCH)
                {
                    CashBank cashBank = _cashBankService.GetObjectById(salesAllowance.CashBankId);
                    CashMutation cashMutation = _cashMutationService.CreateCashMutationForSalesAllowance(salesAllowance, cashBank);
                    _cashMutationService.CashMutateObject(cashMutation, _cashBankService,_currencyService);
                    _generalLedgerJournalService.CreateConfirmationJournalForSalesAllowance(salesAllowance, cashBank, _accountService);
                }
            }
            return salesAllowance;
        }

        public SalesAllowance UnconfirmObject(SalesAllowance salesAllowance, ISalesAllowanceDetailService _salesAllowanceDetailService,
                                            ICashBankService _cashBankService, IReceivableService _receivableService, ICashMutationService _cashMutationService,
                                            IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService, ICurrencyService _currencyService)
        {
            if (_validator.ValidUnconfirmObject(salesAllowance, _salesAllowanceDetailService, _cashBankService, _closingService))
            {
                IList<SalesAllowanceDetail> details = _salesAllowanceDetailService.GetObjectsBySalesAllowanceId(salesAllowance.Id);
                foreach (var detail in details)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _salesAllowanceDetailService.UnconfirmObject(detail, this, _receivableService);
                }
                _repository.UnconfirmObject(salesAllowance);

                if (!salesAllowance.IsGBCH)
                {
                    CashBank cashBank = _cashBankService.GetObjectById(salesAllowance.CashBankId);
                    IList<CashMutation> cashMutations = _cashMutationService.SoftDeleteCashMutationForSalesAllowance(salesAllowance, cashBank);
                    foreach (var cashMutation in cashMutations)
                    {
                        _cashMutationService.ReverseCashMutateObject(cashMutation, _cashBankService,_currencyService);
                    }
                    _generalLedgerJournalService.CreateUnconfirmationJournalForSalesAllowance(salesAllowance, cashBank, _accountService);
                }
            }
            return salesAllowance;
        }

        public SalesAllowance ReconcileObject(SalesAllowance salesAllowance, DateTime ReconciliationDate, ISalesAllowanceDetailService _salesAllowanceDetailService,
                                              ICashMutationService _cashMutationService, ICashBankService _cashBankService, IReceivableService _receivableService,
                                              IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService, ICurrencyService _currencyService)
        {
            salesAllowance.ReconciliationDate = ReconciliationDate;
            if (_validator.ValidReconcileObject(salesAllowance, _closingService))
            {
                CashBank cashBank = _cashBankService.GetObjectById(salesAllowance.CashBankId);
                CashMutation cashMutation = _cashMutationService.CreateCashMutationForSalesAllowance(salesAllowance, cashBank);

                _generalLedgerJournalService.CreateConfirmationJournalForSalesAllowance(salesAllowance, cashBank, _accountService);
                _repository.ReconcileObject(salesAllowance);
                _cashMutationService.CashMutateObject(cashMutation, _cashBankService,_currencyService);

                IList<SalesAllowanceDetail> salesAllowanceDetails = _salesAllowanceDetailService.GetObjectsBySalesAllowanceId(salesAllowance.Id);
                foreach(var salesAllowanceDetail in salesAllowanceDetails)
                {
                    Receivable receivable = _receivableService.GetObjectById(salesAllowanceDetail.ReceivableId);
                    receivable.PendingClearanceAmount -= salesAllowanceDetail.Amount;
                    if (receivable.PendingClearanceAmount == 0 && receivable.RemainingAmount == 0)
                    {
                        receivable.IsCompleted = true;
                        receivable.CompletionDate = DateTime.Now;
                    }
                    _receivableService.UpdateObject(receivable);
                }
            }
            return salesAllowance;
        }

        public SalesAllowance UnreconcileObject(SalesAllowance salesAllowance, ISalesAllowanceDetailService _salesAllowanceDetailService,
                                                ICashMutationService _cashMutationService, ICashBankService _cashBankService, IReceivableService _receivableService,
                                                IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService, ICurrencyService _currencyService)
        {
            if (_validator.ValidUnreconcileObject(salesAllowance, _salesAllowanceDetailService, _cashBankService, _closingService))
            {
                CashBank cashBank = _cashBankService.GetObjectById(salesAllowance.CashBankId);
                _generalLedgerJournalService.CreateUnconfirmationJournalForSalesAllowance(salesAllowance, cashBank, _accountService);
                _repository.UnreconcileObject(salesAllowance);

                IList<CashMutation> cashMutations = _cashMutationService.SoftDeleteCashMutationForSalesAllowance(salesAllowance, cashBank);
                foreach (var cashMutation in cashMutations)
                {
                    _cashMutationService.ReverseCashMutateObject(cashMutation, _cashBankService,_currencyService);
                }

                IList<SalesAllowanceDetail> salesAllowanceDetails = _salesAllowanceDetailService.GetObjectsBySalesAllowanceId(salesAllowance.Id);
                foreach (var salesAllowanceDetail in salesAllowanceDetails)
                {
                    Receivable receivable = _receivableService.GetObjectById(salesAllowanceDetail.ReceivableId);
                    receivable.PendingClearanceAmount += salesAllowanceDetail.Amount;
                    if (receivable.PendingClearanceAmount != 0 || receivable.RemainingAmount != 0)
                    {
                        receivable.IsCompleted = false;
                        receivable.CompletionDate = null;
                    }
                    _receivableService.UpdateObject(receivable);
                }
            }
            return salesAllowance;
        }
    }
}