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
    public class PurchaseInvoiceService : IPurchaseInvoiceService
    {
        private IPurchaseInvoiceRepository _repository;
        private IPurchaseInvoiceValidator _validator;

        public PurchaseInvoiceService(IPurchaseInvoiceRepository _purchaseInvoiceRepository, IPurchaseInvoiceValidator _purchaseInvoiceValidator)
        {
            _repository = _purchaseInvoiceRepository;
            _validator = _purchaseInvoiceValidator;
        }

        public IPurchaseInvoiceValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<PurchaseInvoice> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<PurchaseInvoice> GetAll()
        {
            return _repository.GetAll();
        }

        public PurchaseInvoice GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public IList<PurchaseInvoice> GetObjectsByPurchaseReceivalId(int purchaseReceivalId)
        {
            return _repository.GetObjectsByPurchaseReceivalId(purchaseReceivalId);
        }

        public PurchaseInvoice CreateObject(PurchaseInvoice purchaseInvoice, IPurchaseReceivalService _purchaseReceivalService)
        {
            purchaseInvoice.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(purchaseInvoice, this, _purchaseReceivalService) ? _repository.CreateObject(purchaseInvoice) : purchaseInvoice);
        }

        public PurchaseInvoice UpdateObject(PurchaseInvoice purchaseInvoice, IPurchaseReceivalService _purchaseReceivalService, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService)
        {
            return (_validator.ValidUpdateObject(purchaseInvoice, this, _purchaseReceivalService, _purchaseInvoiceDetailService) ? _repository.UpdateObject(purchaseInvoice) : purchaseInvoice);
        }

        public PurchaseInvoice SoftDeleteObject(PurchaseInvoice purchaseInvoice, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService)
        {
            return (_validator.ValidDeleteObject(purchaseInvoice, _purchaseInvoiceDetailService) ? _repository.SoftDeleteObject(purchaseInvoice) : purchaseInvoice);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public PurchaseInvoice ConfirmObject(PurchaseInvoice purchaseInvoice, DateTime ConfirmationDate, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService, IPurchaseOrderService _purchaseOrderService,
                                             IPurchaseReceivalService _purchaseReceivalService, IPurchaseReceivalDetailService _purchaseReceivalDetailService, IPayableService _payableService,
                                             IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService,
                                             ICurrencyService _currencyService, IExchangeRateService _exchangeRateService, IGLNonBaseCurrencyService _gLNonBaseCurrencyService)
        {
            purchaseInvoice.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(purchaseInvoice, _purchaseInvoiceDetailService, _purchaseReceivalService, _purchaseReceivalDetailService, _closingService,_exchangeRateService, _currencyService))
            {
                // confirm details
                // add all amount into amountpayable
                IList<PurchaseInvoiceDetail> details = _purchaseInvoiceDetailService.GetObjectsByPurchaseInvoiceId(purchaseInvoice.Id);
                foreach (var detail in details)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _purchaseInvoiceDetailService.ConfirmObject(detail, ConfirmationDate, _purchaseReceivalDetailService);
                }
                purchaseInvoice = CalculateAmountPayable(purchaseInvoice, _purchaseInvoiceDetailService);
                purchaseInvoice = _repository.ConfirmObject(purchaseInvoice);

                Currency currency = _currencyService.GetObjectById(purchaseInvoice.CurrencyId);
                if (currency.IsBase == false)
                {
                    purchaseInvoice.ExchangeRateId = _exchangeRateService.GetLatestRate(purchaseInvoice.ConfirmationDate.Value, currency).Id;
                    purchaseInvoice.ExchangeRateAmount = _exchangeRateService.GetObjectById(purchaseInvoice.ExchangeRateId.Value).Rate;
                }
                else
                {
                    purchaseInvoice.ExchangeRateAmount = 1;
                }
                // confirm object
                // create payable
                purchaseInvoice = _repository.UpdateObject(purchaseInvoice);
                PurchaseReceival purchaseReceival = _purchaseReceivalService.GetObjectById(purchaseInvoice.PurchaseReceivalId);
                _generalLedgerJournalService.CreateConfirmationJournalForPurchaseInvoice(purchaseInvoice, purchaseReceival,
                                             _accountService, _gLNonBaseCurrencyService, _currencyService);
                _purchaseReceivalService.CheckAndSetInvoiceComplete(purchaseReceival, _purchaseReceivalDetailService);
                PurchaseOrder purchaseOrder = _purchaseOrderService.GetObjectById(purchaseReceival.PurchaseOrderId);
                Payable payable = _payableService.CreateObject(purchaseOrder.ContactId, Constant.PayableSource.PurchaseInvoice, purchaseInvoice.Id,purchaseInvoice.CurrencyId, purchaseInvoice.AmountPayable,purchaseInvoice.ExchangeRateAmount, purchaseInvoice.DueDate);
            }
            return purchaseInvoice;
        }

        public PurchaseInvoice UnconfirmObject(PurchaseInvoice purchaseInvoice, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService,
                                               IPurchaseReceivalService _purchaseReceivalService, IPurchaseReceivalDetailService _purchaseReceivalDetailService,
                                               IPaymentVoucherDetailService _paymentVoucherDetailService, IPayableService _payableService,
                                               IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService,
                                               IClosingService _closingService, IGLNonBaseCurrencyService _gLNonBaseCurrencyService, ICurrencyService _currencyService)
        {
            if (_validator.ValidUnconfirmObject(purchaseInvoice, _purchaseInvoiceDetailService, _paymentVoucherDetailService, _payableService, _closingService))
            {
                IList<PurchaseInvoiceDetail> details = _purchaseInvoiceDetailService.GetObjectsByPurchaseInvoiceId(purchaseInvoice.Id);
                foreach (var detail in details)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _purchaseInvoiceDetailService.UnconfirmObject(detail, _purchaseReceivalService, _purchaseReceivalDetailService);
                }
                PurchaseReceival purchaseReceival = _purchaseReceivalService.GetObjectById(purchaseInvoice.PurchaseReceivalId);
                _generalLedgerJournalService.CreateUnconfirmationJournalForPurchaseInvoice(purchaseInvoice, purchaseReceival, _accountService,
                                             _gLNonBaseCurrencyService, _currencyService);
                _repository.UnconfirmObject(purchaseInvoice);
                _purchaseReceivalService.UnsetInvoiceComplete(purchaseReceival);
                Payable payable = _payableService.GetObjectBySource(Constant.PayableSource.PurchaseInvoice, purchaseInvoice.Id);
                _payableService.SoftDeleteObject(payable);
            }
            return purchaseInvoice;
        }

        public PurchaseInvoice CalculateAmountPayable(PurchaseInvoice purchaseInvoice, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService)
        {
            IList<PurchaseInvoiceDetail> details = _purchaseInvoiceDetailService.GetObjectsByPurchaseInvoiceId(purchaseInvoice.Id);
            decimal AmountPayable = 0;
            foreach (var detail in details)
            {
                AmountPayable += detail.Amount;
            }
            decimal Discount = purchaseInvoice.Discount / 100 * AmountPayable;
            decimal TaxableAmount = AmountPayable - Discount;
            decimal Tax = purchaseInvoice.Tax / 100 * TaxableAmount;
            purchaseInvoice.AmountPayable = TaxableAmount + Tax;
            _repository.Update(purchaseInvoice);
            return purchaseInvoice;
        }
    }
}