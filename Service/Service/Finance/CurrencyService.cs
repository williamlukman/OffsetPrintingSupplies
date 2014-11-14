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

        public Currency CreateObject(Currency currency)
        {
            currency.Errors = new Dictionary<string, string>();
            if (_validator.ValidCreateObject(currency, this))
            {
                Currency newcurrency = new Currency()
                {
                    Name = currency.Name,
                    Description = currency.Description,
                    IsBase = false,
                };
                _repository.CreateObject(currency);
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