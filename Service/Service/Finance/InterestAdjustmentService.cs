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
    public class InterestAdjustmentService : IInterestAdjustmentService
    {
        private IInterestAdjustmentRepository _repository;
        private IInterestAdjustmentValidator _validator;

        public InterestAdjustmentService(IInterestAdjustmentRepository _interestAdjustmentRepository, IInterestAdjustmentValidator _interestAdjustmentValidator)
        {
            _repository = _interestAdjustmentRepository;
            _validator = _interestAdjustmentValidator;
        }

        public IInterestAdjustmentValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<InterestAdjustment> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<InterestAdjustment> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<InterestAdjustment> GetObjectsByCashBankId(int cashBankId)
        {
            return _repository.GetObjectsByCashBankId(cashBankId);
        }

        public InterestAdjustment GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }
        
        public InterestAdjustment CreateObject(InterestAdjustment interestAdjustment, ICashBankService _cashBankService)
        {
            interestAdjustment.Errors = new Dictionary<String, String>();
            return (interestAdjustment = _validator.ValidCreateObject(interestAdjustment, _cashBankService) ? _repository.CreateObject(interestAdjustment) : interestAdjustment);
        }

        public InterestAdjustment CreateObject(int CashBankId, DateTime interestDate, ICashBankService _cashBankService)
        {
            InterestAdjustment interestAdjustment = new InterestAdjustment
            {
                CashBankId = CashBankId,
                InterestDate = interestDate
            };
            return this.CreateObject(interestAdjustment, _cashBankService);
        }

        public InterestAdjustment UpdateObject(InterestAdjustment interestAdjustment, ICashBankService _cashBankService)
        {
            return (interestAdjustment = _validator.ValidUpdateObject(interestAdjustment, _cashBankService) ? _repository.UpdateObject(interestAdjustment) : interestAdjustment);
        }

        public InterestAdjustment SoftDeleteObject(InterestAdjustment interestAdjustment)
        {
            return (interestAdjustment = _validator.ValidDeleteObject(interestAdjustment) ? _repository.SoftDeleteObject(interestAdjustment) : interestAdjustment);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public InterestAdjustment ConfirmObject(InterestAdjustment interestAdjustment, DateTime ConfirmationDate, ICashMutationService _cashMutationService, ICashBankService _cashBankService,
                                                IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService
                                              , ICurrencyService _currencyService, IExchangeRateService _exchangeRateService, IGLNonBaseCurrencyService _gLNonBaseCurrencyService)
        {
            interestAdjustment.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(interestAdjustment, _cashBankService, _closingService)) 
            {
                CashBank cashBank = _cashBankService.GetObjectById(interestAdjustment.CashBankId);
                if (_currencyService.GetObjectById(cashBank.CurrencyId).IsBase == false)
                {
                    interestAdjustment.ExchangeRateId = _exchangeRateService.GetLatestRate(interestAdjustment.ConfirmationDate.Value, _currencyService.GetObjectById(cashBank.CurrencyId)).Id;
                    if (interestAdjustment.ExchangeRateAmount <= 0)
                    {
                        interestAdjustment.ExchangeRateAmount = _exchangeRateService.GetObjectById(interestAdjustment.ExchangeRateId.Value).Rate;
                    }
                }
                else
                {
                    interestAdjustment.ExchangeRateAmount = 1;
                }
                CashMutation cashMutation = _cashMutationService.CreateCashMutationForInterestAdjustment(interestAdjustment, cashBank);
                // cashBank.Amount += interestAdjustment.Amount;
                _cashMutationService.CashMutateObject(cashMutation, _cashBankService,_currencyService);
                _generalLedgerJournalService.CreateConfirmationJournalForInterestAdjustment(interestAdjustment, cashBank, _accountService,_currencyService,_gLNonBaseCurrencyService);
                _repository.ConfirmObject(interestAdjustment);
            }
            return interestAdjustment;
        }

        public InterestAdjustment UnconfirmObject(InterestAdjustment interestAdjustment, ICashMutationService _cashMutationService, ICashBankService _cashBankService,
                                                  IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService
                                                , ICurrencyService _currencyService,IGLNonBaseCurrencyService _gLNonBaseCurrencyService)
        {
            if (_validator.ValidUnconfirmObject(interestAdjustment, _cashBankService, _closingService))
            {
                CashBank cashBank = _cashBankService.GetObjectById(interestAdjustment.CashBankId);
                IList<CashMutation> cashMutations = _cashMutationService.SoftDeleteCashMutationForInterestAdjustment(interestAdjustment, cashBank);
                // cashBank.Amount -= interestAdjustment.Amount;
                foreach (var cashMutation in cashMutations)
                {
                    _cashMutationService.ReverseCashMutateObject(cashMutation, _cashBankService,_currencyService);
                }
                _generalLedgerJournalService.CreateUnconfirmationJournalForInterestAdjustment(interestAdjustment, cashBank, _accountService,_currencyService,_gLNonBaseCurrencyService);
                _repository.UnconfirmObject(interestAdjustment);
            }
            return interestAdjustment;
        }
    }
}