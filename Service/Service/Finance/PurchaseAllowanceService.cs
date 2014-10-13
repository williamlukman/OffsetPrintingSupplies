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
    public class PurchaseAllowanceService : IPurchaseAllowanceService
    {
        private IPurchaseAllowanceRepository _repository;
        private IPurchaseAllowanceValidator _validator;

        public PurchaseAllowanceService(IPurchaseAllowanceRepository _purchaseAllowanceRepository, IPurchaseAllowanceValidator _purchaseAllowanceValidator)
        {
            _repository = _purchaseAllowanceRepository;
            _validator = _purchaseAllowanceValidator;
        }

        public IPurchaseAllowanceValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<PurchaseAllowance> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<PurchaseAllowance> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<PurchaseAllowance> GetObjectsByCashBankId(int cashBankId)
        {
            return _repository.GetObjectsByCashBankId(cashBankId);
        }

        public PurchaseAllowance GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public IList<PurchaseAllowance> GetObjectsByContactId(int contactId)
        {
            return _repository.GetObjectsByContactId(contactId);
        }

        public PurchaseAllowance CreateObject(PurchaseAllowance purchaseAllowance, IPurchaseAllowanceDetailService _purchaseAllowanceDetailService,
                                            IPayableService _payableService, IContactService _contactService, ICashBankService _cashBankService)
        {
            purchaseAllowance.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(purchaseAllowance, this, _purchaseAllowanceDetailService, _payableService, _contactService, _cashBankService) ?
                    _repository.CreateObject(purchaseAllowance) : purchaseAllowance);
        }

        public PurchaseAllowance UpdateObject(PurchaseAllowance purchaseAllowance, IPurchaseAllowanceDetailService _purchaseAllowanceDetailService, IPayableService _payableService, IContactService _contactService, ICashBankService _cashBankService)
        {
            return (_validator.ValidUpdateObject(purchaseAllowance, this, _purchaseAllowanceDetailService, _payableService, _contactService, _cashBankService) ? _repository.UpdateObject(purchaseAllowance) : purchaseAllowance);
        }

        public PurchaseAllowance UpdateAmount(PurchaseAllowance purchaseAllowance)
        {
            return _repository.UpdateObject(purchaseAllowance);
        }

        public PurchaseAllowance SoftDeleteObject(PurchaseAllowance purchaseAllowance, IPurchaseAllowanceDetailService _purchaseAllowanceDetailService)
        {
            return (_validator.ValidDeleteObject(purchaseAllowance, _purchaseAllowanceDetailService) ? _repository.SoftDeleteObject(purchaseAllowance) : purchaseAllowance);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public PurchaseAllowance ConfirmObject(PurchaseAllowance purchaseAllowance, DateTime ConfirmationDate, IPurchaseAllowanceDetailService _purchaseAllowanceDetailService,
                                            ICashBankService _cashBankService, IPayableService _payableService, ICashMutationService _cashMutationService,
                                            IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            purchaseAllowance.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(purchaseAllowance, this, _purchaseAllowanceDetailService, _cashBankService, _payableService, _closingService))
            {
                IList<PurchaseAllowanceDetail> details = _purchaseAllowanceDetailService.GetObjectsByPurchaseAllowanceId(purchaseAllowance.Id);
                foreach (var detail in details)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _purchaseAllowanceDetailService.ConfirmObject(detail, ConfirmationDate, this, _payableService);
                }
                
                _repository.ConfirmObject(purchaseAllowance);

                if (!purchaseAllowance.IsGBCH)
                {
                    CashBank cashBank = _cashBankService.GetObjectById(purchaseAllowance.CashBankId);
                    CashMutation cashMutation = _cashMutationService.CreateCashMutationForPurchaseAllowance(purchaseAllowance, cashBank);
                    _cashMutationService.CashMutateObject(cashMutation, _cashBankService);
                    _generalLedgerJournalService.CreateConfirmationJournalForPurchaseAllowance(purchaseAllowance, cashBank, _accountService);
                }
            }
            return purchaseAllowance;
        }

        public PurchaseAllowance UnconfirmObject(PurchaseAllowance purchaseAllowance, IPurchaseAllowanceDetailService _purchaseAllowanceDetailService,
                                            ICashBankService _cashBankService, IPayableService _payableService, ICashMutationService _cashMutationService,
                                            IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            if (_validator.ValidUnconfirmObject(purchaseAllowance, _closingService))
            {
                IList<PurchaseAllowanceDetail> details = _purchaseAllowanceDetailService.GetObjectsByPurchaseAllowanceId(purchaseAllowance.Id);
                foreach (var detail in details)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _purchaseAllowanceDetailService.UnconfirmObject(detail, this, _payableService);
                }
                _repository.UnconfirmObject(purchaseAllowance);

                if (!purchaseAllowance.IsGBCH)
                {
                    CashBank cashBank = _cashBankService.GetObjectById(purchaseAllowance.CashBankId);
                    IList<CashMutation> cashMutations = _cashMutationService.SoftDeleteCashMutationForPurchaseAllowance(purchaseAllowance, cashBank);
                    foreach (var cashMutation in cashMutations)
                    {
                        _cashMutationService.ReverseCashMutateObject(cashMutation, _cashBankService);
                    }
                    _generalLedgerJournalService.CreateUnconfirmationJournalForPurchaseAllowance(purchaseAllowance, cashBank, _accountService);
                }
            }
            return purchaseAllowance;
        }

        public PurchaseAllowance ReconcileObject(PurchaseAllowance purchaseAllowance, DateTime ReconciliationDate, IPurchaseAllowanceDetailService _purchaseAllowanceDetailService,
                                              ICashMutationService _cashMutationService, ICashBankService _cashBankService, IPayableService _payableService,
                                              IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            purchaseAllowance.ReconciliationDate = ReconciliationDate;
            if (_validator.ValidReconcileObject(purchaseAllowance, _purchaseAllowanceDetailService, _cashBankService, _closingService))
            {
                CashBank cashBank = _cashBankService.GetObjectById(purchaseAllowance.CashBankId);
                CashMutation cashMutation = _cashMutationService.CreateCashMutationForPurchaseAllowance(purchaseAllowance, cashBank);
                _generalLedgerJournalService.CreateConfirmationJournalForPurchaseAllowance(purchaseAllowance, cashBank, _accountService);
                _repository.ReconcileObject(purchaseAllowance);

                _cashMutationService.CashMutateObject(cashMutation, _cashBankService);

                IList<PurchaseAllowanceDetail> purchaseAllowanceDetails = _purchaseAllowanceDetailService.GetObjectsByPurchaseAllowanceId(purchaseAllowance.Id);
                foreach(var purchaseAllowanceDetail in purchaseAllowanceDetails)
                {
                    Payable payable = _payableService.GetObjectById(purchaseAllowanceDetail.PayableId);
                    payable.PendingClearanceAmount -= purchaseAllowanceDetail.Amount;
                    if (payable.PendingClearanceAmount == 0 && payable.RemainingAmount == 0)
                    {
                        payable.IsCompleted = true;
                        payable.CompletionDate = DateTime.Now;
                    }
                    _payableService.UpdateObject(payable);
                }
            }
            return purchaseAllowance;
        }

        public PurchaseAllowance UnreconcileObject(PurchaseAllowance purchaseAllowance, IPurchaseAllowanceDetailService _purchaseAllowanceDetailService,
                                                ICashMutationService _cashMutationService, ICashBankService _cashBankService, IPayableService _payableService,
                                                IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            if (_validator.ValidUnreconcileObject(purchaseAllowance, _closingService))
            {
                CashBank cashBank = _cashBankService.GetObjectById(purchaseAllowance.CashBankId);
                _generalLedgerJournalService.CreateUnconfirmationJournalForPurchaseAllowance(purchaseAllowance, cashBank, _accountService);
                _repository.UnreconcileObject(purchaseAllowance);

                IList<CashMutation> cashMutations = _cashMutationService.SoftDeleteCashMutationForPurchaseAllowance(purchaseAllowance, cashBank);
                foreach (var cashMutation in cashMutations)
                {
                    _cashMutationService.ReverseCashMutateObject(cashMutation, _cashBankService);
                }

                IList<PurchaseAllowanceDetail> purchaseAllowanceDetails = _purchaseAllowanceDetailService.GetObjectsByPurchaseAllowanceId(purchaseAllowance.Id);
                foreach (var purchaseAllowanceDetail in purchaseAllowanceDetails)
                {
                    Payable payable = _payableService.GetObjectById(purchaseAllowanceDetail.PayableId);
                    payable.PendingClearanceAmount += purchaseAllowanceDetail.Amount;
                    if (payable.PendingClearanceAmount != 0 || payable.RemainingAmount != 0)
                    {
                        payable.IsCompleted = false;
                        payable.CompletionDate = null;
                    }
                    _payableService.UpdateObject(payable);
                }
            }
            return purchaseAllowance;
        }
    }
}