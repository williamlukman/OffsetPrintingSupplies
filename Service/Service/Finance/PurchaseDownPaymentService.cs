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
    public class PurchaseDownPaymentService : IPurchaseDownPaymentService
    {
        private IPurchaseDownPaymentRepository _repository;
        private IPurchaseDownPaymentValidator _validator;

        public PurchaseDownPaymentService(IPurchaseDownPaymentRepository _purchaseDownPaymentRepository, IPurchaseDownPaymentValidator _purchaseDownPaymentValidator)
        {
            _repository = _purchaseDownPaymentRepository;
            _validator = _purchaseDownPaymentValidator;
        }

        public IPurchaseDownPaymentValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<PurchaseDownPayment> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<PurchaseDownPayment> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<PurchaseDownPayment> GetObjectsByCashBankId(int cashBankId)
        {
            return _repository.GetObjectsByCashBankId(cashBankId);
        }

        public PurchaseDownPayment GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public IList<PurchaseDownPayment> GetObjectsByContactId(int contactId)
        {
            return _repository.GetObjectsByContactId(contactId);
        }

        public PurchaseDownPayment CreateObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService,
                                            IPayableService _payableService, IContactService _contactService, ICashBankService _cashBankService)
        {
            purchaseDownPayment.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(purchaseDownPayment, this, _purchaseDownPaymentDetailService, _payableService, _contactService, _cashBankService) ?
                    _repository.CreateObject(purchaseDownPayment) : purchaseDownPayment);
        }

        public PurchaseDownPayment UpdateObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService, IPayableService _payableService, IContactService _contactService, ICashBankService _cashBankService)
        {
            return (_validator.ValidUpdateObject(purchaseDownPayment, this, _purchaseDownPaymentDetailService, _payableService, _contactService, _cashBankService) ? _repository.UpdateObject(purchaseDownPayment) : purchaseDownPayment);
        }

        public PurchaseDownPayment UpdateAmount(PurchaseDownPayment purchaseDownPayment)
        {
            return _repository.UpdateObject(purchaseDownPayment);
        }

        public PurchaseDownPayment SoftDeleteObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService)
        {
            return (_validator.ValidDeleteObject(purchaseDownPayment, _purchaseDownPaymentDetailService) ? _repository.SoftDeleteObject(purchaseDownPayment) : purchaseDownPayment);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public PurchaseDownPayment ConfirmObject(PurchaseDownPayment purchaseDownPayment, DateTime ConfirmationDate, IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService,
                                            ICashBankService _cashBankService, IPayableService _payableService, ICashMutationService _cashMutationService,
                                            IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            purchaseDownPayment.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(purchaseDownPayment, this, _purchaseDownPaymentDetailService, _cashBankService, _payableService, _closingService))
            {
                IList<PurchaseDownPaymentDetail> details = _purchaseDownPaymentDetailService.GetObjectsByPurchaseDownPaymentId(purchaseDownPayment.Id);
                foreach (var detail in details)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _purchaseDownPaymentDetailService.ConfirmObject(detail, ConfirmationDate, this, _payableService);
                }
                
                _repository.ConfirmObject(purchaseDownPayment);

                if (!purchaseDownPayment.IsGBCH)
                {
                    CashBank cashBank = _cashBankService.GetObjectById(purchaseDownPayment.CashBankId);
                    CashMutation cashMutation = _cashMutationService.CreateCashMutationForPurchaseDownPayment(purchaseDownPayment, cashBank);
                    _cashMutationService.CashMutateObject(cashMutation, _cashBankService);
                    _generalLedgerJournalService.CreateConfirmationJournalForPurchaseDownPayment(purchaseDownPayment, cashBank, _accountService);
                }
            }
            return purchaseDownPayment;
        }

        public PurchaseDownPayment UnconfirmObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService,
                                            ICashBankService _cashBankService, IPayableService _payableService, ICashMutationService _cashMutationService,
                                            IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            if (_validator.ValidUnconfirmObject(purchaseDownPayment, _closingService))
            {
                IList<PurchaseDownPaymentDetail> details = _purchaseDownPaymentDetailService.GetObjectsByPurchaseDownPaymentId(purchaseDownPayment.Id);
                foreach (var detail in details)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _purchaseDownPaymentDetailService.UnconfirmObject(detail, this, _payableService);
                }
                _repository.UnconfirmObject(purchaseDownPayment);

                if (!purchaseDownPayment.IsGBCH)
                {
                    CashBank cashBank = _cashBankService.GetObjectById(purchaseDownPayment.CashBankId);
                    IList<CashMutation> cashMutations = _cashMutationService.SoftDeleteCashMutationForPurchaseDownPayment(purchaseDownPayment, cashBank);
                    foreach (var cashMutation in cashMutations)
                    {
                        _cashMutationService.ReverseCashMutateObject(cashMutation, _cashBankService);
                    }
                    _generalLedgerJournalService.CreateUnconfirmationJournalForPurchaseDownPayment(purchaseDownPayment, cashBank, _accountService);
                }
            }
            return purchaseDownPayment;
        }

        public PurchaseDownPayment ReconcileObject(PurchaseDownPayment purchaseDownPayment, DateTime ReconciliationDate, IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService,
                                              ICashMutationService _cashMutationService, ICashBankService _cashBankService, IPayableService _payableService,
                                              IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            purchaseDownPayment.ReconciliationDate = ReconciliationDate;
            if (_validator.ValidReconcileObject(purchaseDownPayment, _purchaseDownPaymentDetailService, _cashBankService, _closingService))
            {
                CashBank cashBank = _cashBankService.GetObjectById(purchaseDownPayment.CashBankId);
                CashMutation cashMutation = _cashMutationService.CreateCashMutationForPurchaseDownPayment(purchaseDownPayment, cashBank);
                _generalLedgerJournalService.CreateConfirmationJournalForPurchaseDownPayment(purchaseDownPayment, cashBank, _accountService);
                _repository.ReconcileObject(purchaseDownPayment);

                _cashMutationService.CashMutateObject(cashMutation, _cashBankService);

                IList<PurchaseDownPaymentDetail> purchaseDownPaymentDetails = _purchaseDownPaymentDetailService.GetObjectsByPurchaseDownPaymentId(purchaseDownPayment.Id);
                foreach(var purchaseDownPaymentDetail in purchaseDownPaymentDetails)
                {
                    Payable payable = _payableService.GetObjectById(purchaseDownPaymentDetail.PayableId);
                    payable.PendingClearanceAmount -= purchaseDownPaymentDetail.Amount;
                    if (payable.PendingClearanceAmount == 0 && payable.RemainingAmount == 0)
                    {
                        payable.IsCompleted = true;
                        payable.CompletionDate = DateTime.Now;
                    }
                    _payableService.UpdateObject(payable);
                }
            }
            return purchaseDownPayment;
        }

        public PurchaseDownPayment UnreconcileObject(PurchaseDownPayment purchaseDownPayment, IPurchaseDownPaymentDetailService _purchaseDownPaymentDetailService,
                                                ICashMutationService _cashMutationService, ICashBankService _cashBankService, IPayableService _payableService,
                                                IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            if (_validator.ValidUnreconcileObject(purchaseDownPayment, _closingService))
            {
                CashBank cashBank = _cashBankService.GetObjectById(purchaseDownPayment.CashBankId);
                _generalLedgerJournalService.CreateUnconfirmationJournalForPurchaseDownPayment(purchaseDownPayment, cashBank, _accountService);
                _repository.UnreconcileObject(purchaseDownPayment);

                IList<CashMutation> cashMutations = _cashMutationService.SoftDeleteCashMutationForPurchaseDownPayment(purchaseDownPayment, cashBank);
                foreach (var cashMutation in cashMutations)
                {
                    _cashMutationService.ReverseCashMutateObject(cashMutation, _cashBankService);
                }

                IList<PurchaseDownPaymentDetail> purchaseDownPaymentDetails = _purchaseDownPaymentDetailService.GetObjectsByPurchaseDownPaymentId(purchaseDownPayment.Id);
                foreach (var purchaseDownPaymentDetail in purchaseDownPaymentDetails)
                {
                    Payable payable = _payableService.GetObjectById(purchaseDownPaymentDetail.PayableId);
                    payable.PendingClearanceAmount += purchaseDownPaymentDetail.Amount;
                    if (payable.PendingClearanceAmount != 0 || payable.RemainingAmount != 0)
                    {
                        payable.IsCompleted = false;
                        payable.CompletionDate = null;
                    }
                    _payableService.UpdateObject(payable);
                }
            }
            return purchaseDownPayment;
        }
    }
}