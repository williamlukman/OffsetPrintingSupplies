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
    public class CashBankAdjustmentService : ICashBankAdjustmentService
    {
        private ICashBankAdjustmentRepository _repository;
        private ICashBankAdjustmentValidator _validator;

        public CashBankAdjustmentService(ICashBankAdjustmentRepository _cashBankAdjustmentRepository, ICashBankAdjustmentValidator _cashBankAdjustmentValidator)
        {
            _repository = _cashBankAdjustmentRepository;
            _validator = _cashBankAdjustmentValidator;
        }

        public ICashBankAdjustmentValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<CashBankAdjustment> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<CashBankAdjustment> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<CashBankAdjustment> GetObjectsByCashBankId(int cashBankId)
        {
            return _repository.GetObjectsByCashBankId(cashBankId);
        }

        public CashBankAdjustment GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }
        
        public CashBankAdjustment CreateObject(CashBankAdjustment cashBankAdjustment, ICashBankService _cashBankService)
        {
            cashBankAdjustment.Errors = new Dictionary<String, String>();
            return (cashBankAdjustment = _validator.ValidCreateObject(cashBankAdjustment, _cashBankService) ? _repository.CreateObject(cashBankAdjustment) : cashBankAdjustment);
        }

        public CashBankAdjustment CreateObject(int CashBankId, DateTime adjustmentDate, ICashBankService _cashBankService)
        {
            CashBankAdjustment cashBankAdjustment = new CashBankAdjustment
            {
                CashBankId = CashBankId,
                AdjustmentDate = adjustmentDate
            };
            return this.CreateObject(cashBankAdjustment, _cashBankService);
        }

        public CashBankAdjustment UpdateObject(CashBankAdjustment cashBankAdjustment, ICashBankService _cashBankService)
        {
            return (cashBankAdjustment = _validator.ValidUpdateObject(cashBankAdjustment, _cashBankService) ? _repository.UpdateObject(cashBankAdjustment) : cashBankAdjustment);
        }

        public CashBankAdjustment SoftDeleteObject(CashBankAdjustment cashBankAdjustment)
        {
            return (cashBankAdjustment = _validator.ValidDeleteObject(cashBankAdjustment) ? _repository.SoftDeleteObject(cashBankAdjustment) : cashBankAdjustment);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public CashBankAdjustment ConfirmObject(CashBankAdjustment cashBankAdjustment, DateTime ConfirmationDate, ICashMutationService _cashMutationService, ICashBankService _cashBankService,
                                                IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService
                                              , ICurrencyService _currencyService, IExchangeRateService _exchangeRateService)
        {
            cashBankAdjustment.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(cashBankAdjustment, _cashBankService, _closingService))
            {
                CashBank cashBank = _cashBankService.GetObjectById(cashBankAdjustment.CashBankId);
                if (_currencyService.GetObjectById(cashBank.CurrencyId).IsBase == false)
                {
                    cashBankAdjustment.ExchangeRateId = _exchangeRateService.GetLatestRate(cashBankAdjustment.ConfirmationDate.Value, cashBank.CurrencyId).Id;
                    cashBankAdjustment.ExchangeRateAmount = _exchangeRateService.GetObjectById(cashBankAdjustment.ExchangeRateId.Value).Rate;
                }
                else
                {
                    cashBankAdjustment.ExchangeRateAmount = 1;
                }
                CashMutation cashMutation = _cashMutationService.CreateCashMutationForCashBankAdjustment(cashBankAdjustment, cashBank);
                // cashBank.Amount += cashBankAdjustment.Amount;
                _cashMutationService.CashMutateObject(cashMutation, _cashBankService,_currencyService);
                _generalLedgerJournalService.CreateConfirmationJournalForCashBankAdjustment(cashBankAdjustment, cashBank, _accountService);
                _repository.ConfirmObject(cashBankAdjustment);
            }
            return cashBankAdjustment;
        }

        public CashBankAdjustment UnconfirmObject(CashBankAdjustment cashBankAdjustment, ICashMutationService _cashMutationService, ICashBankService _cashBankService,
                                                  IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService
                                                , ICurrencyService _currencyService)
        {
            if (_validator.ValidUnconfirmObject(cashBankAdjustment, _cashBankService, _closingService))
            {
                CashBank cashBank = _cashBankService.GetObjectById(cashBankAdjustment.CashBankId);
                IList<CashMutation> cashMutations = _cashMutationService.SoftDeleteCashMutationForCashBankAdjustment(cashBankAdjustment, cashBank);
                // cashBank.Amount -= cashBankAdjustment.Amount;
                foreach (var cashMutation in cashMutations)
                {
                    _cashMutationService.ReverseCashMutateObject(cashMutation, _cashBankService,_currencyService);
                }
                _generalLedgerJournalService.CreateUnconfirmationJournalForCashBankAdjustment(cashBankAdjustment, cashBank, _accountService);
                _repository.UnconfirmObject(cashBankAdjustment);
            }
            return cashBankAdjustment;
        }
    }
}