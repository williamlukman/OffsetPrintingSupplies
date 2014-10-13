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
    public class SalesDownPaymentService : ISalesDownPaymentService
    {
        private ISalesDownPaymentRepository _repository;
        private ISalesDownPaymentValidator _validator;

        public SalesDownPaymentService(ISalesDownPaymentRepository _salesDownPaymentRepository, ISalesDownPaymentValidator _salesDownPaymentValidator)
        {
            _repository = _salesDownPaymentRepository;
            _validator = _salesDownPaymentValidator;
        }

        public ISalesDownPaymentValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<SalesDownPayment> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<SalesDownPayment> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<SalesDownPayment> GetObjectsByCashBankId(int cashBankId)
        {
            return _repository.GetObjectsByCashBankId(cashBankId);
        }

        public SalesDownPayment GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public IList<SalesDownPayment> GetObjectsByContactId(int contactId)
        {
            return _repository.GetObjectsByContactId(contactId);
        }

        public SalesDownPayment CreateObject(SalesDownPayment salesDownPayment, ISalesDownPaymentDetailService _salesDownPaymentDetailService,
                                            IReceivableService _receivableService, IContactService _contactService, ICashBankService _cashBankService)
        {
            salesDownPayment.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(salesDownPayment, this, _salesDownPaymentDetailService, _receivableService, _contactService, _cashBankService) ?
                    _repository.CreateObject(salesDownPayment) : salesDownPayment);
        }

        public SalesDownPayment UpdateObject(SalesDownPayment salesDownPayment, ISalesDownPaymentDetailService _salesDownPaymentDetailService, IReceivableService _receivableService, IContactService _contactService, ICashBankService _cashBankService)
        {
            return (_validator.ValidUpdateObject(salesDownPayment, this, _salesDownPaymentDetailService, _receivableService, _contactService, _cashBankService) ? _repository.UpdateObject(salesDownPayment) : salesDownPayment);
        }

        public SalesDownPayment UpdateAmount(SalesDownPayment salesDownPayment)
        {
            return _repository.UpdateObject(salesDownPayment);
        }

        public SalesDownPayment SoftDeleteObject(SalesDownPayment salesDownPayment, ISalesDownPaymentDetailService _salesDownPaymentDetailService)
        {
            return (_validator.ValidDeleteObject(salesDownPayment, _salesDownPaymentDetailService) ? _repository.SoftDeleteObject(salesDownPayment) : salesDownPayment);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public SalesDownPayment ConfirmObject(SalesDownPayment salesDownPayment, DateTime ConfirmationDate, ISalesDownPaymentDetailService _salesDownPaymentDetailService,
                                            ICashBankService _cashBankService, IReceivableService _receivableService, ICashMutationService _cashMutationService,
                                            IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            salesDownPayment.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(salesDownPayment, this, _salesDownPaymentDetailService, _cashBankService, _receivableService, _closingService))
            {
                IList<SalesDownPaymentDetail> details = _salesDownPaymentDetailService.GetObjectsBySalesDownPaymentId(salesDownPayment.Id);
                foreach (var detail in details)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _salesDownPaymentDetailService.ConfirmObject(detail, ConfirmationDate, this, _receivableService);
                }
                _repository.ConfirmObject(salesDownPayment);

                if (!salesDownPayment.IsGBCH)
                {
                    CashBank cashBank = _cashBankService.GetObjectById(salesDownPayment.CashBankId);
                    CashMutation cashMutation = _cashMutationService.CreateCashMutationForSalesDownPayment(salesDownPayment, cashBank);
                    _cashMutationService.CashMutateObject(cashMutation, _cashBankService);
                    _generalLedgerJournalService.CreateConfirmationJournalForSalesDownPayment(salesDownPayment, cashBank, _accountService);
                }
            }
            return salesDownPayment;
        }

        public SalesDownPayment UnconfirmObject(SalesDownPayment salesDownPayment, ISalesDownPaymentDetailService _salesDownPaymentDetailService,
                                            ICashBankService _cashBankService, IReceivableService _receivableService, ICashMutationService _cashMutationService,
                                            IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            if (_validator.ValidUnconfirmObject(salesDownPayment, _salesDownPaymentDetailService, _cashBankService, _closingService))
            {
                IList<SalesDownPaymentDetail> details = _salesDownPaymentDetailService.GetObjectsBySalesDownPaymentId(salesDownPayment.Id);
                foreach (var detail in details)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _salesDownPaymentDetailService.UnconfirmObject(detail, this, _receivableService);
                }
                _repository.UnconfirmObject(salesDownPayment);

                if (!salesDownPayment.IsGBCH)
                {
                    CashBank cashBank = _cashBankService.GetObjectById(salesDownPayment.CashBankId);
                    IList<CashMutation> cashMutations = _cashMutationService.SoftDeleteCashMutationForSalesDownPayment(salesDownPayment, cashBank);
                    foreach (var cashMutation in cashMutations)
                    {
                        _cashMutationService.ReverseCashMutateObject(cashMutation, _cashBankService);
                    }
                    _generalLedgerJournalService.CreateUnconfirmationJournalForSalesDownPayment(salesDownPayment, cashBank, _accountService);
                }
            }
            return salesDownPayment;
        }

        public SalesDownPayment ReconcileObject(SalesDownPayment salesDownPayment, DateTime ReconciliationDate, ISalesDownPaymentDetailService _salesDownPaymentDetailService,
                                              ICashMutationService _cashMutationService, ICashBankService _cashBankService, IReceivableService _receivableService,
                                              IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            salesDownPayment.ReconciliationDate = ReconciliationDate;
            if (_validator.ValidReconcileObject(salesDownPayment, _closingService))
            {
                CashBank cashBank = _cashBankService.GetObjectById(salesDownPayment.CashBankId);
                CashMutation cashMutation = _cashMutationService.CreateCashMutationForSalesDownPayment(salesDownPayment, cashBank);

                _generalLedgerJournalService.CreateConfirmationJournalForSalesDownPayment(salesDownPayment, cashBank, _accountService);
                _repository.ReconcileObject(salesDownPayment);
                _cashMutationService.CashMutateObject(cashMutation, _cashBankService);

                IList<SalesDownPaymentDetail> salesDownPaymentDetails = _salesDownPaymentDetailService.GetObjectsBySalesDownPaymentId(salesDownPayment.Id);
                foreach(var salesDownPaymentDetail in salesDownPaymentDetails)
                {
                    Receivable receivable = _receivableService.GetObjectById(salesDownPaymentDetail.ReceivableId);
                    receivable.PendingClearanceAmount -= salesDownPaymentDetail.Amount;
                    if (receivable.PendingClearanceAmount == 0 && receivable.RemainingAmount == 0)
                    {
                        receivable.IsCompleted = true;
                        receivable.CompletionDate = DateTime.Now;
                    }
                    _receivableService.UpdateObject(receivable);
                }
            }
            return salesDownPayment;
        }

        public SalesDownPayment UnreconcileObject(SalesDownPayment salesDownPayment, ISalesDownPaymentDetailService _salesDownPaymentDetailService,
                                                ICashMutationService _cashMutationService, ICashBankService _cashBankService, IReceivableService _receivableService,
                                                IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            if (_validator.ValidUnreconcileObject(salesDownPayment, _salesDownPaymentDetailService, _cashBankService, _closingService))
            {
                CashBank cashBank = _cashBankService.GetObjectById(salesDownPayment.CashBankId);
                _generalLedgerJournalService.CreateUnconfirmationJournalForSalesDownPayment(salesDownPayment, cashBank, _accountService);
                _repository.UnreconcileObject(salesDownPayment);

                IList<CashMutation> cashMutations = _cashMutationService.SoftDeleteCashMutationForSalesDownPayment(salesDownPayment, cashBank);
                foreach (var cashMutation in cashMutations)
                {
                    _cashMutationService.ReverseCashMutateObject(cashMutation, _cashBankService);
                }

                IList<SalesDownPaymentDetail> salesDownPaymentDetails = _salesDownPaymentDetailService.GetObjectsBySalesDownPaymentId(salesDownPayment.Id);
                foreach (var salesDownPaymentDetail in salesDownPaymentDetails)
                {
                    Receivable receivable = _receivableService.GetObjectById(salesDownPaymentDetail.ReceivableId);
                    receivable.PendingClearanceAmount += salesDownPaymentDetail.Amount;
                    if (receivable.PendingClearanceAmount != 0 || receivable.RemainingAmount != 0)
                    {
                        receivable.IsCompleted = false;
                        receivable.CompletionDate = null;
                    }
                    _receivableService.UpdateObject(receivable);
                }
            }
            return salesDownPayment;
        }
    }
}