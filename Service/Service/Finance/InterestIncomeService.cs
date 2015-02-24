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
    public class InterestIncomeService : IInterestIncomeService
    {
        private IInterestIncomeRepository _repository;
        private IInterestIncomeValidator _validator;

        public InterestIncomeService(IInterestIncomeRepository _interestIncomeRepository, IInterestIncomeValidator _interestIncomeValidator)
        {
            _repository = _interestIncomeRepository;
            _validator = _interestIncomeValidator;
        }

        public IInterestIncomeValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<InterestIncome> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<InterestIncome> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<InterestIncome> GetObjectsByCashBankId(int cashBankId)
        {
            return _repository.GetObjectsByCashBankId(cashBankId);
        }

        public InterestIncome GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }
        
        public InterestIncome CreateObject(InterestIncome interestIncome, ICashBankService _cashBankService)
        {
            interestIncome.Errors = new Dictionary<String, String>();
            return (interestIncome = _validator.ValidCreateObject(interestIncome, _cashBankService) ? _repository.CreateObject(interestIncome) : interestIncome);
        }

        public InterestIncome CreateObject(int CashBankId, DateTime adjustmentDate, ICashBankService _cashBankService)
        {
            InterestIncome interestIncome = new InterestIncome
            {
                CashBankId = CashBankId,
                InterestDate = adjustmentDate
            };
            return this.CreateObject(interestIncome, _cashBankService);
        }

        public InterestIncome UpdateObject(InterestIncome interestIncome, ICashBankService _cashBankService)
        {
            return (interestIncome = _validator.ValidUpdateObject(interestIncome, _cashBankService) ? _repository.UpdateObject(interestIncome) : interestIncome);
        }

        public InterestIncome SoftDeleteObject(InterestIncome interestIncome)
        {
            return (interestIncome = _validator.ValidDeleteObject(interestIncome) ? _repository.SoftDeleteObject(interestIncome) : interestIncome);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public InterestIncome ConfirmObject(InterestIncome interestIncome, DateTime ConfirmationDate, ICashMutationService _cashMutationService, ICashBankService _cashBankService,
                                                IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService
                                              , ICurrencyService _currencyService, IExchangeRateService _exchangeRateService, IGLNonBaseCurrencyService _gLNonBaseCurrencyService)
        {
            interestIncome.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(interestIncome, _cashBankService, _closingService)) 
            {
                CashBank cashBank = _cashBankService.GetObjectById(interestIncome.CashBankId);
                if (_currencyService.GetObjectById(cashBank.CurrencyId).IsBase == false)
                {
                    interestIncome.ExchangeRateId = _exchangeRateService.GetLatestRate(interestIncome.ConfirmationDate.Value, _currencyService.GetObjectById(cashBank.CurrencyId)).Id;
                    interestIncome.ExchangeRateAmount = _exchangeRateService.GetObjectById(interestIncome.ExchangeRateId.Value).Rate;
                }
                else
                {
                    interestIncome.ExchangeRateAmount = 1;
                }
                CashMutation cashMutation = _cashMutationService.CreateCashMutationForInterestIncome(interestIncome, cashBank);
                // cashBank.Amount += interestIncome.Amount;
                _cashMutationService.CashMutateObject(cashMutation, _cashBankService,_currencyService);
                _generalLedgerJournalService.CreateConfirmationJournalForInterestIncome(interestIncome, cashBank, _accountService,_currencyService,_gLNonBaseCurrencyService);
                _repository.ConfirmObject(interestIncome);
            }
            return interestIncome;
        }

        public InterestIncome UnconfirmObject(InterestIncome interestIncome, ICashMutationService _cashMutationService, ICashBankService _cashBankService,
                                                  IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService
                                                , ICurrencyService _currencyService,IGLNonBaseCurrencyService _gLNonBaseCurrencyService)
        {
            if (_validator.ValidUnconfirmObject(interestIncome, _cashBankService, _closingService))
            {
                CashBank cashBank = _cashBankService.GetObjectById(interestIncome.CashBankId);
                IList<CashMutation> cashMutations = _cashMutationService.SoftDeleteCashMutationForInterestIncome(interestIncome, cashBank);
                // cashBank.Amount -= interestIncome.Amount;
                foreach (var cashMutation in cashMutations)
                {
                    _cashMutationService.ReverseCashMutateObject(cashMutation, _cashBankService,_currencyService);
                }
                _generalLedgerJournalService.CreateUnconfirmationJournalForInterestIncome(interestIncome, cashBank, _accountService,_currencyService,_gLNonBaseCurrencyService);
                _repository.UnconfirmObject(interestIncome);
            }
            return interestIncome;
        }
    }
}