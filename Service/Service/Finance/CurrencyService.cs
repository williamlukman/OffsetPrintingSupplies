using Core.DomainModel;
using Core.Constants;
using Core.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Service;
using Core.Interface.Validation;
using Data.Repository;

namespace Service.Service
{
    public class CurrencyService : ICurrencyService
    {
        private ICurrencyRepository _repository;
        private ICurrencyValidator _validator;
        public CurrencyService(ICurrencyRepository _currencyRepository, ICurrencyValidator _currencyValidator)
        {
            _repository = _currencyRepository;
            _validator = _currencyValidator;
        }

        public ICurrencyValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<Currency> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<Currency> GetAll()
        {
            return _repository.GetAll();
        }

        public Currency GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public Currency GetObjectByName(string Name)
        {
            return _repository.GetObjectByName(Name);
        }

        public string GenerateAccountCode(IAccountService _accountService,string accountLegacyCode,int id)
        {
            int ParentId = _accountService.GetObjectByLegacyCode(accountLegacyCode).Id;
            string parentCode = _accountService.GetObjectById(ParentId).Code;
            return parentCode + id.ToString();
        }
        //Tambah Account Currency Di COA
        public Currency CreateObject(Currency currency,IAccountService _accountService)
        {
            currency.Errors = new Dictionary<string, string>();
            if (_validator.ValidCreateObject(currency, this))
            {
                _repository.CreateObject(currency);
                string CodeAr = GenerateAccountCode(_accountService, Constant.AccountLegacyCode.AccountReceivable,currency.Id);
                Account arAccount = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountReceivable);
                Account accountar = new Account()
                {
                    Name = "Account Receivable " + currency.Name,
                    Level = arAccount.Level + 1,
                    Group = Constant.AccountGroup.Asset,
                    LegacyCode = Constant.AccountLegacyCode.AccountReceivable + currency.Id,
                    Code = CodeAr,
                    IsCashBankAccount = false,
                    IsLeaf = true,
                    ParentId = arAccount.Id
                };
                _accountService.CreateCashBankAccount(accountar, _accountService);

                string CodearGBCH = GenerateAccountCode(_accountService, Constant.AccountLegacyCode.GBCHReceivable, currency.Id);
                Account arGBCHAccount = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.GBCHReceivable);
                Account accountarGBCH = new Account()
                {
                    Name = "GBCH Receivable " + currency.Name,
                    Level = arGBCHAccount.Level + 1,
                    Group = Constant.AccountGroup.Asset,
                    LegacyCode = Constant.AccountLegacyCode.GBCHReceivable + currency.Id,
                    Code = CodearGBCH,
                    IsCashBankAccount = false,
                    IsLeaf = true,
                    ParentId = arGBCHAccount.Id
                };
                _accountService.CreateCashBankAccount(accountarGBCH, _accountService);

                string CodeAp = GenerateAccountCode(_accountService, Constant.AccountLegacyCode.AccountPayable, currency.Id);
                Account apAccount = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.AccountPayable);
                Account accountap = new Account()
                {
                    Name = "Account Payable " + currency.Name,
                    Level = apAccount.Level + 1,
                    Group = Constant.AccountGroup.Liability,
                    LegacyCode = Constant.AccountLegacyCode.AccountPayable + currency.Id,
                    Code = CodeAp,
                    IsCashBankAccount = false,
                    IsLeaf = true,
                    ParentId = apAccount.Id
                };
                _accountService.CreateCashBankAccount(accountap, _accountService);

                string CodeApGBCH = GenerateAccountCode(_accountService, Constant.AccountLegacyCode.GBCHPayable, currency.Id);
                Account apGBCHAccount = _accountService.GetObjectByLegacyCode(Constant.AccountLegacyCode.GBCHPayable);
                Account accountapGBCH = new Account()
                {
                    Name = "GBCH Payable " + currency.Name,
                    Level = apGBCHAccount.Level + 1,
                    Group = Constant.AccountGroup.Liability,
                    LegacyCode = Constant.AccountLegacyCode.GBCHPayable + currency.Id,
                    Code = CodeApGBCH,
                    IsCashBankAccount = false,
                    IsLeaf = true,
                    ParentId = apGBCHAccount.Id
                };
                _accountService.CreateCashBankAccount(accountapGBCH, _accountService);

            }
            return currency;
        }

        public Currency UpdateObject(Currency currency, ICashBankService _cashBankService, IPaymentRequestService _paymentRequestService
            , IPurchaseOrderService _purchaseOrderService, ISalesOrderService _salesOrderService, ISalesInvoiceService _salesInvoiceService
            , IPurchaseInvoiceService _purchaseInvoiceService, IPayableService _payableService, IReceivableService _receivableService
            , IPaymentVoucherService _paymentVoucherService, IReceiptVoucherService _receiptVoucherService)
        {
            return (currency = _validator.ValidUpdateObject(currency, this,_cashBankService,_paymentRequestService
            , _purchaseOrderService, _salesOrderService, _salesInvoiceService
            , _purchaseInvoiceService, _payableService, _receivableService
            , _paymentVoucherService, _receiptVoucherService) ? _repository.UpdateObject(currency) : currency);
        }

        public Currency SoftDeleteObject(Currency currency, ICashBankService _cashBankService, IPaymentRequestService _paymentRequestService
            , IPurchaseOrderService _purchaseOrderService, ISalesOrderService _salesOrderService, ISalesInvoiceService _salesInvoiceService
            , IPurchaseInvoiceService _purchaseInvoiceService, IPayableService _payableService, IReceivableService _receivableService
            , IPaymentVoucherService _paymentVoucherService, IReceiptVoucherService _receiptVoucherService)
        {
            return (currency = _validator.ValidDeleteObject(currency,this,_cashBankService,_paymentRequestService
            , _purchaseOrderService, _salesOrderService, _salesInvoiceService
            , _purchaseInvoiceService, _payableService, _receivableService
            , _paymentVoucherService, _receiptVoucherService) ? _repository.SoftDeleteObject(currency) : currency);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsNameDuplicated(Currency currency)
        {
            IQueryable<Currency> currencys = _repository.FindAll(cb => cb.Name == currency.Name && !cb.IsDeleted && cb.Id != currency.Id);
            return (currencys.Count() > 0 ? true : false);
        }

      
    }
}