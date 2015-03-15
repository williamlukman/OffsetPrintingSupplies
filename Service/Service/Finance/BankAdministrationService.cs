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
    public class BankAdministrationService : IBankAdministrationService
    {
        private IBankAdministrationRepository _repository;
        private IBankAdministrationValidator _validator;

        public BankAdministrationService(IBankAdministrationRepository _bankAdministrationRepository, IBankAdministrationValidator _bankAdministrationValidator)
        {
            _repository = _bankAdministrationRepository;
            _validator = _bankAdministrationValidator;
        }

        public IBankAdministrationValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<BankAdministration> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<BankAdministration> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<BankAdministration> GetObjectsByCashBankId(int cashBankId)
        {
            return _repository.GetObjectsByCashBankId(cashBankId);
        }

        public BankAdministration GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public BankAdministration CreateObject(BankAdministration bankAdministration, ICashBankService _cashBankService, ICurrencyService _currencyService, IExchangeRateService _exchangeRateService)
        {
            bankAdministration.Errors = new Dictionary<String, String>();
            if (_validator.ValidCreateObject(bankAdministration, _cashBankService))
            {
                CashBank cashBank = _cashBankService.GetObjectById(bankAdministration.CashBankId);
                if (cashBank.Currency.IsBase)
                {
                    bankAdministration.ExchangeRateAmount = 1;
                }
                else
                {
                    ExchangeRate xRate = _exchangeRateService.GetLatestRate(bankAdministration.AdministrationDate, cashBank.Currency);
                    bankAdministration.ExchangeRateId = xRate != null ? (int?)xRate.Id : null;
                    if (bankAdministration.ExchangeRateAmount <= 0)
                    {
                        xRate = _exchangeRateService.GetObjectById(bankAdministration.ExchangeRateId.GetValueOrDefault());
                        bankAdministration.ExchangeRateAmount = xRate != null ? xRate.Rate : 1;
                    }
                }
                bankAdministration = _repository.CreateObject(bankAdministration);
            }
            return bankAdministration;
        }

        public BankAdministration CreateObject(int CashBankId, DateTime interestDate, ICashBankService _cashBankService, ICurrencyService _currencyService, IExchangeRateService _exchangeRateService)
        {
            BankAdministration bankAdministration = new BankAdministration
            {
                CashBankId = CashBankId,
                AdministrationDate = interestDate
            };
            return this.CreateObject(bankAdministration, _cashBankService, _currencyService, _exchangeRateService);
        }

        public BankAdministration UpdateObject(BankAdministration bankAdministration, ICashBankService _cashBankService, ICurrencyService _currencyService, IExchangeRateService _exchangeRateService)
        {
            if (_validator.ValidUpdateObject(bankAdministration, _cashBankService))
            {
                CashBank cashBank = _cashBankService.GetObjectById(bankAdministration.CashBankId);
                if (cashBank.Currency.IsBase)
                {
                    bankAdministration.ExchangeRateAmount = 1;
                }
                else
                {
                    ExchangeRate xRate = _exchangeRateService.GetLatestRate(bankAdministration.AdministrationDate, cashBank.Currency);
                    bankAdministration.ExchangeRateId = xRate != null ? (int?)xRate.Id : null;
                    if (bankAdministration.ExchangeRateAmount <= 0)
                    {
                        xRate = _exchangeRateService.GetObjectById(bankAdministration.ExchangeRateId.GetValueOrDefault());
                        bankAdministration.ExchangeRateAmount = xRate != null ? xRate.Rate : 1;
                    }
                }
                _repository.UpdateObject(bankAdministration);
            }
            return bankAdministration;
        }

        public BankAdministration SoftDeleteObject(BankAdministration bankAdministration)
        {
            return (bankAdministration = _validator.ValidDeleteObject(bankAdministration) ? _repository.SoftDeleteObject(bankAdministration) : bankAdministration);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public BankAdministration ConfirmObject(BankAdministration bankAdministration, DateTime ConfirmationDate, ICashMutationService _cashMutationService, ICashBankService _cashBankService,
                                                IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService,
                                                ICurrencyService _currencyService, IExchangeRateService _exchangeRateService, IGLNonBaseCurrencyService _gLNonBaseCurrencyService, IBankAdministrationDetailService _bankAdministrationDetailService)
        {
            bankAdministration.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(bankAdministration, _cashBankService, _closingService)) 
            {
                _repository.ConfirmObject(bankAdministration);
                CashBank cashBank = _cashBankService.GetObjectById(bankAdministration.CashBankId);
                if (cashBank.Currency.IsBase)
                {
                    bankAdministration.ExchangeRateAmount = 1;
                }
                else
                {
                    ExchangeRate xRate = _exchangeRateService.GetLatestRate(bankAdministration.ConfirmationDate.GetValueOrDefault(), cashBank.Currency);
                    bankAdministration.ExchangeRateId = xRate != null ? (int?)xRate.Id : null;
                    if (bankAdministration.ExchangeRateAmount <= 0)
                    {
                        xRate = _exchangeRateService.GetObjectById(bankAdministration.ExchangeRateId.GetValueOrDefault());
                        bankAdministration.ExchangeRateAmount = xRate != null ? xRate.Rate : 1;
                    }
                }
                CashMutation cashMutation = _cashMutationService.CreateCashMutationForBankAdministration(bankAdministration, cashBank);
                // cashBank.Amount += bankAdministration.Amount;
                _cashMutationService.CashMutateObject(cashMutation, _cashBankService,_currencyService);
                _generalLedgerJournalService.CreateConfirmationJournalForBankAdministration(bankAdministration, cashBank, _accountService,_currencyService,_gLNonBaseCurrencyService, _bankAdministrationDetailService);
            }
            return bankAdministration;
        }

        public BankAdministration UnconfirmObject(BankAdministration bankAdministration, ICashMutationService _cashMutationService, ICashBankService _cashBankService,
                                                  IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService,
                                                  ICurrencyService _currencyService, IGLNonBaseCurrencyService _gLNonBaseCurrencyService, IBankAdministrationDetailService _bankAdministrationDetailService)
        {
            if (_validator.ValidUnconfirmObject(bankAdministration, _cashBankService, _closingService))
            {
                _repository.UnconfirmObject(bankAdministration);
                CashBank cashBank = _cashBankService.GetObjectById(bankAdministration.CashBankId);
                IList<CashMutation> cashMutations = _cashMutationService.SoftDeleteCashMutationForBankAdministration(bankAdministration, cashBank);
                // cashBank.Amount -= bankAdministration.Amount;
                foreach (var cashMutation in cashMutations)
                {
                    _cashMutationService.ReverseCashMutateObject(cashMutation, _cashBankService,_currencyService);
                }
                _generalLedgerJournalService.CreateUnconfirmationJournalForBankAdministration(bankAdministration, cashBank, _accountService,_currencyService,_gLNonBaseCurrencyService, _bankAdministrationDetailService);
            }
            return bankAdministration;
        }

        public BankAdministration CalculateTotalAmount(BankAdministration bankAdministration, IBankAdministrationDetailService _bankAdministrationDetailService)
        {
            IList<BankAdministrationDetail> paymenRequestDetails = _bankAdministrationDetailService.GetObjectsByBankAdministrationId(bankAdministration.Id);
            decimal total = 0;
            foreach (var detail in paymenRequestDetails)
            {
                total += detail.Amount * (detail.Status == Core.Constants.Constant.GeneralLedgerStatus.Credit ? 1 : -1);
            }
            bankAdministration.Amount = total;
            bankAdministration = _repository.UpdateObject(bankAdministration);
            return bankAdministration;
        }
    }
}