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
    public class SalesInvoiceService : ISalesInvoiceService
    {
        private ISalesInvoiceRepository _repository;
        private ISalesInvoiceValidator _validator;

        public SalesInvoiceService(ISalesInvoiceRepository _salesInvoiceRepository, ISalesInvoiceValidator _salesInvoiceValidator)
        {
            _repository = _salesInvoiceRepository;
            _validator = _salesInvoiceValidator;
        }

        public ISalesInvoiceValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<SalesInvoice> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<SalesInvoice> GetAll()
        {
            return _repository.GetAll();
        }

        public SalesInvoice GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public IList<SalesInvoice> GetObjectsByDeliveryOrderId(int deliveryOrderId)
        {
            return _repository.GetObjectsByDeliveryOrderId(deliveryOrderId);
        }

        public SalesInvoice CreateObject(SalesInvoice salesInvoice, IDeliveryOrderService _deliveryOrderService)
        {
            salesInvoice.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(salesInvoice, this, _deliveryOrderService) ? _repository.CreateObject(salesInvoice) : salesInvoice);
        }

        public SalesInvoice UpdateObject(SalesInvoice salesInvoice, IDeliveryOrderService _deliveryOrderService, ISalesInvoiceDetailService _salesInvoiceDetailService)
        {
            return (_validator.ValidUpdateObject(salesInvoice, this, _deliveryOrderService, _salesInvoiceDetailService) ? _repository.UpdateObject(salesInvoice) : salesInvoice);
        }

        public SalesInvoice SoftDeleteObject(SalesInvoice salesInvoice, ISalesInvoiceDetailService _salesInvoiceDetailService)
        {
            return (_validator.ValidDeleteObject(salesInvoice, _salesInvoiceDetailService) ? _repository.SoftDeleteObject(salesInvoice) : salesInvoice);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public SalesInvoice ConfirmObject(SalesInvoice salesInvoice, DateTime ConfirmationDate, ISalesInvoiceDetailService _salesInvoiceDetailService, ISalesOrderService _salesOrderService, 
                                          ISalesOrderDetailService _salesOrderDetailService, IDeliveryOrderService _deliveryOrderService, IDeliveryOrderDetailService _deliveryOrderDetailService,
                                          IReceivableService _receivableService, IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService, 
                                          IServiceCostService _serviceCostService, IRollerBuilderService _rollerBuilderService, IItemService _itemService, IItemTypeService _itemTypeService,
                                          IContactService _contactService, IExchangeRateService _exchangeRateService, ICurrencyService _currencyService, IGLNonBaseCurrencyService _gLNonBaseCurrencyService)
        {
            salesInvoice.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(salesInvoice, _salesInvoiceDetailService, _deliveryOrderService, _deliveryOrderDetailService,
                                              _salesOrderDetailService, _serviceCostService, _closingService,_exchangeRateService, _currencyService))
            {
                // confirm details
                decimal TotalCOS = 0;
                decimal Rate = 0;
                Currency currency = _currencyService.GetObjectById(salesInvoice.CurrencyId);
                if (currency.IsBase == false)
                {
                    salesInvoice.ExchangeRateId = _exchangeRateService.GetLatestRate(salesInvoice.ConfirmationDate.Value, currency).Id;
                    salesInvoice.ExchangeRateAmount = _exchangeRateService.GetObjectById(salesInvoice.ExchangeRateId.Value).Rate;
                }
                else
                {
                    salesInvoice.ExchangeRateAmount = 1;
                }

                IList<SalesInvoiceDetail> details = _salesInvoiceDetailService.GetObjectsBySalesInvoiceId(salesInvoice.Id);
                foreach (var detail in details)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _salesInvoiceDetailService.ConfirmObject(detail,ConfirmationDate, _deliveryOrderDetailService, _salesOrderDetailService, _serviceCostService,
                                                             _rollerBuilderService, _itemService);
                    TotalCOS += detail.COS;
                    DeliveryOrderDetail deliveryOrderDetail = _deliveryOrderDetailService.GetObjectById(detail.DeliveryOrderDetailId);
                    Item item = _itemService.GetObjectById(deliveryOrderDetail.ItemId);
                    ItemType itemType = _itemTypeService.GetObjectById(item.ItemTypeId);
                    if (detail.COS > 0)
                    {
                        _generalLedgerJournalService.CreateConfirmationJournalForSalesInvoiceDetail(salesInvoice, itemType.AccountId.GetValueOrDefault(), detail.COS * salesInvoice.ExchangeRateAmount, _accountService);
                    }
                }
                // add all amount into amountreceivable
                // confirm object
                salesInvoice.TotalCOS = TotalCOS;
                salesInvoice = CalculateAmountReceivable(salesInvoice, _salesInvoiceDetailService);
                salesInvoice = _repository.ConfirmObject(salesInvoice);
                salesInvoice = _repository.UpdateObject(salesInvoice);
                DeliveryOrder deliveryOrder = _deliveryOrderService.GetObjectById(salesInvoice.DeliveryOrderId);
                SalesOrder salesOrder = _salesOrderService.GetObjectById(deliveryOrder.SalesOrderId);
                Contact contact = _contactService.GetObjectById(salesOrder.ContactId);
                _generalLedgerJournalService.CreateConfirmationJournalForSalesInvoice(salesInvoice, contact, _accountService, _exchangeRateService, _currencyService, _gLNonBaseCurrencyService);
                _deliveryOrderService.CheckAndSetInvoiceComplete(deliveryOrder, _deliveryOrderDetailService);
                // create receivable
                Receivable receivable = _receivableService.CreateObject(salesOrder.ContactId, Constant.ReceivableSource.SalesInvoice, salesInvoice.Id, salesInvoice.CurrencyId, salesInvoice.AmountReceivable,salesInvoice.ExchangeRateAmount, salesInvoice.DueDate);
            }
            return salesInvoice;
        }

        public SalesInvoice UnconfirmObject(SalesInvoice salesInvoice, ISalesInvoiceDetailService _salesInvoiceDetailService, IDeliveryOrderService _deliveryOrderService,
                                            IDeliveryOrderDetailService _deliveryOrderDetailService, IReceiptVoucherDetailService _receiptVoucherDetailService,
                                            IReceivableService _receivableService, ISalesOrderService _salesOrderService, IContactService _contactService,
                                            IItemService _itemService, IItemTypeService _itemTypeService,
                                            IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService,
                                            IExchangeRateService _exchangeRateService,ICurrencyService _currencyService, IGLNonBaseCurrencyService _gLNonBaseCurrencyService)
        {
            if (_validator.ValidUnconfirmObject(salesInvoice, _salesInvoiceDetailService, _receiptVoucherDetailService, _receivableService, _closingService))
            {
                IList<SalesInvoiceDetail> details = _salesInvoiceDetailService.GetObjectsBySalesInvoiceId(salesInvoice.Id);
                foreach (var detail in details)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _salesInvoiceDetailService.UnconfirmObject(detail, _deliveryOrderService, _deliveryOrderDetailService);
                    DeliveryOrderDetail deliveryOrderDetail = _deliveryOrderDetailService.GetObjectById(detail.DeliveryOrderDetailId);
                    Item item = _itemService.GetObjectById(deliveryOrderDetail.ItemId);
                    ItemType itemType = _itemTypeService.GetObjectById(item.ItemTypeId);
                    if (detail.COS > 0)
                    {
                        _generalLedgerJournalService.CreateUnconfirmationJournalForSalesInvoiceDetail(salesInvoice, itemType.AccountId.GetValueOrDefault(), detail.COS * salesInvoice.ExchangeRateAmount, _accountService);
                    }
                }
                DeliveryOrder deliveryOrder = _deliveryOrderService.GetObjectById(salesInvoice.DeliveryOrderId);
                SalesOrder salesOrder = _salesOrderService.GetObjectById(deliveryOrder.SalesOrderId);
                Contact contact = _contactService.GetObjectById(salesOrder.ContactId);
                _generalLedgerJournalService.CreateUnconfirmationJournalForSalesInvoice(salesInvoice, contact, _accountService, _exchangeRateService, _currencyService, _gLNonBaseCurrencyService);
                _repository.UnconfirmObject(salesInvoice);
                _deliveryOrderService.UnsetInvoiceComplete(deliveryOrder);
                Receivable receivable = _receivableService.GetObjectBySource(Constant.ReceivableSource.SalesInvoice, salesInvoice.Id);
                _receivableService.SoftDeleteObject(receivable);
            }
            return salesInvoice;
        }

        public SalesInvoice CalculateAmountReceivable(SalesInvoice salesInvoice, ISalesInvoiceDetailService _salesInvoiceDetailService)
        {
            IList<SalesInvoiceDetail> details = _salesInvoiceDetailService.GetObjectsBySalesInvoiceId(salesInvoice.Id);
            decimal AmountReceivable = 0;
            foreach (var detail in details)
            {
                AmountReceivable += detail.Amount;
            }
            decimal Discount = salesInvoice.Discount / 100 * AmountReceivable;
            decimal TaxableAmount = AmountReceivable - Discount;
            decimal Tax = salesInvoice.Tax / 100 * TaxableAmount;
            salesInvoice.DPP = TaxableAmount;
            salesInvoice.AmountReceivable = TaxableAmount + Tax;
            _repository.Update(salesInvoice);
            return salesInvoice;
        }
    }
}