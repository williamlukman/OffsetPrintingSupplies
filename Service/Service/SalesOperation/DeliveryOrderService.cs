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
    public class DeliveryOrderService : IDeliveryOrderService
    {
        private IDeliveryOrderRepository _repository;
        private IDeliveryOrderValidator _validator;

        public DeliveryOrderService(IDeliveryOrderRepository _deliveryOrderRepository, IDeliveryOrderValidator _deliveryOrderValidator)
        {
            _repository = _deliveryOrderRepository;
            _validator = _deliveryOrderValidator;
        }

        public IDeliveryOrderValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<DeliveryOrder> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<DeliveryOrder> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<DeliveryOrder> GetConfirmedObjects()
        {
            return _repository.GetConfirmedObjects();
        }

        public DeliveryOrder GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public IList<DeliveryOrder> GetObjectsBySalesOrderId(int salesOrderId)
        {
            return _repository.GetObjectsBySalesOrderId(salesOrderId);
        }

        public DeliveryOrder CreateObject(DeliveryOrder deliveryOrder, ISalesOrderService _salesOrderService, IWarehouseService _warehouseService)
        {
            deliveryOrder.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(deliveryOrder, this, _salesOrderService, _warehouseService) ? _repository.CreateObject(deliveryOrder) : deliveryOrder);
        }

        public DeliveryOrder UpdateObject(DeliveryOrder deliveryOrder, ISalesOrderService _salesOrderService, IWarehouseService _warehouseService)
        {
            return (_validator.ValidUpdateObject(deliveryOrder, this, _salesOrderService, _warehouseService) ? _repository.UpdateObject(deliveryOrder) : deliveryOrder);
        }

        public DeliveryOrder SoftDeleteObject(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService)
        {
            return (_validator.ValidDeleteObject(deliveryOrder, _deliveryOrderDetailService) ? _repository.SoftDeleteObject(deliveryOrder) : deliveryOrder);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public DeliveryOrder ConfirmObject(DeliveryOrder deliveryOrder, DateTime ConfirmationDate, IDeliveryOrderDetailService _deliveryOrderDetailService,
                                           ISalesOrderService _salesOrderService, ISalesOrderDetailService _salesOrderDetailService, IStockMutationService _stockMutationService,
                                           IItemService _itemService, IItemTypeService _itemTypeService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService,
                                           IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService,
                                           IServiceCostService _serviceCostService, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService,
                                           ITemporaryDeliveryOrderService _temporaryDeliveryOrderService, ICustomerStockMutationService _customerStockMutationService, 
                                           ICustomerItemService _customerItemService, ICurrencyService _currencyService, IExchangeRateService _exchangeRateService)
        {
            deliveryOrder.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(deliveryOrder, _deliveryOrderDetailService, this, _itemService, _warehouseItemService, _salesOrderDetailService, _serviceCostService, _customerItemService))
            {
                IList<DeliveryOrderDetail> deliveryOrderDetailValidations = _deliveryOrderDetailService.GetObjectsByDeliveryOrderId(deliveryOrder.Id);
                foreach (var detail in deliveryOrderDetailValidations)
                {
                    detail.Errors = new Dictionary<string, string>();
                    detail.ConfirmationDate = ConfirmationDate;
                    if (!(_deliveryOrderDetailService.GetValidator().ValidConfirmObject(detail, this, _deliveryOrderDetailService, _salesOrderDetailService, _itemService, _warehouseItemService, _serviceCostService, _customerItemService)))
                    {
                        deliveryOrder.Errors.Add("Generic", detail.Errors.FirstOrDefault().Value);
                        return deliveryOrder;
                    }
                }
                decimal TotalCOGS = 0;
                SalesOrder salesOrder = _salesOrderService.GetObjectById(deliveryOrder.SalesOrderId);
                Currency currency = _currencyService.GetObjectById(salesOrder.CurrencyId);
                if (currency.IsBase == false)
                {
                    deliveryOrder.ExchangeRateId = _exchangeRateService.GetLatestRate(deliveryOrder.ConfirmationDate.Value, currency).Id;
                    deliveryOrder.ExchangeRateAmount = _exchangeRateService.GetObjectById(deliveryOrder.ExchangeRateId.Value).Rate;
                }
                else
                {
                    deliveryOrder.ExchangeRateAmount = 1;
                }
                
                IList<DeliveryOrderDetail> deliveryOrderDetails = _deliveryOrderDetailService.GetObjectsByDeliveryOrderId(deliveryOrder.Id);
                foreach (var detail in deliveryOrderDetails)
                {
                    SalesOrderDetail salesOrderDetail = _salesOrderDetailService.GetObjectById(detail.SalesOrderDetailId);
                    detail.Errors = new Dictionary<string, string>();
                    _deliveryOrderDetailService.ConfirmObject(detail, ConfirmationDate, this, _salesOrderDetailService, _stockMutationService, _itemService,
                                                              _blanketService, _warehouseItemService, _serviceCostService, _customerStockMutationService, _customerItemService);
                    if (detail.OrderType == Core.Constants.Constant.OrderTypeCase.SampleOrder ||
                        detail.OrderType == Core.Constants.Constant.OrderTypeCase.TrialOrder ||
                        detail.OrderType == Core.Constants.Constant.OrderTypeCase.Consignment ||
                        detail.OrderType == Core.Constants.Constant.OrderTypeCase.PartDeliveryOrder)
                    {
                        TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail = _temporaryDeliveryOrderDetailService.GetObjectByCode(detail.OrderCode);
                        if (temporaryDeliveryOrderDetail != null)
                        {
                            _temporaryDeliveryOrderDetailService.CompleteObject(temporaryDeliveryOrderDetail);
                        }
                    }
                    Item item = _itemService.GetObjectById(detail.ItemId);
                    ItemType itemType = _itemTypeService.GetObjectById(item.ItemTypeId);
                    Currency itemCurrency = item.CurrencyId == null ? _currencyService.GetQueryable().Where(x => x.IsBase && !x.IsDeleted).FirstOrDefault() : _currencyService.GetObjectById(item.CurrencyId.Value);
                    TotalCOGS += detail.COGS;
                    _generalLedgerJournalService.CreateConfirmationJournalForDeliveryOrderDetail(deliveryOrder, itemType.AccountId.GetValueOrDefault(), detail.COGS, _accountService);
                }
                deliveryOrder.TotalCOGS = TotalCOGS;
                _repository.ConfirmObject(deliveryOrder);
                _generalLedgerJournalService.CreateConfirmationJournalForDeliveryOrder(deliveryOrder, _accountService);
                _salesOrderService.CheckAndSetDeliveryComplete(salesOrder, _salesOrderDetailService);
                IList<TemporaryDeliveryOrder> temporaryDeliveryOrders = _temporaryDeliveryOrderService.GetObjectsByDeliveryOrderId(deliveryOrder.Id);
                foreach (var temporaryDeliveryOrder in temporaryDeliveryOrders)
                {
                    _temporaryDeliveryOrderService.CheckAndSetDeliveryComplete(temporaryDeliveryOrder, _temporaryDeliveryOrderDetailService);
                }
            }
            return deliveryOrder;
        }

        public DeliveryOrder UnconfirmObject(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService, ISalesInvoiceService _salesInvoiceService,
                                             ISalesInvoiceDetailService _salesInvoiceDetailService, ISalesOrderService _salesOrderService, ISalesOrderDetailService _salesOrderDetailService,
                                             IStockMutationService _stockMutationService, IItemService _itemService, IItemTypeService _itemTypeService, IBlanketService _blanketService,
                                             IWarehouseItemService _warehouseItemService, IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService,
                                             IClosingService _closingService, ICustomerStockMutationService _customerStockMutationService, ICustomerItemService _customerItemService)
        {
            if (_validator.ValidUnconfirmObject(deliveryOrder, _salesInvoiceService))
            {
                IList<DeliveryOrderDetail> deliveryOrderDetailValidations = _deliveryOrderDetailService.GetObjectsByDeliveryOrderId(deliveryOrder.Id);
                foreach (var detail in deliveryOrderDetailValidations)
                {
                    detail.Errors = new Dictionary<string, string>();
                    if (!(_deliveryOrderDetailService.GetValidator().ValidUnconfirmObject(detail, _salesInvoiceDetailService)))
                    {
                        deliveryOrder.Errors.Add("Generic", detail.Errors.FirstOrDefault().Value);
                        return deliveryOrder;
                    }
                }
                _generalLedgerJournalService.CreateUnconfirmationJournalForDeliveryOrder(deliveryOrder, _accountService);
                IList<DeliveryOrderDetail> deliveryOrderDetails = _deliveryOrderDetailService.GetObjectsByDeliveryOrderId(deliveryOrder.Id);
                foreach (var detail in deliveryOrderDetails)
                {
                    detail.Errors = new Dictionary<string, string>();
                    Item item = _itemService.GetObjectById(detail.ItemId);
                    ItemType itemType = _itemTypeService.GetObjectById(item.ItemTypeId);
                    _generalLedgerJournalService.CreateUnconfirmationJournalForDeliveryOrderDetail(deliveryOrder, itemType.AccountId.GetValueOrDefault(), detail.COGS, _accountService);
                    _deliveryOrderDetailService.UnconfirmObject(detail, this, _salesOrderService, _salesOrderDetailService,
                                                                _salesInvoiceDetailService, _stockMutationService,
                                                                _itemService, _blanketService, _warehouseItemService, _customerStockMutationService, _customerItemService);
                }
                deliveryOrder.TotalCOGS = 0;
                _repository.UnconfirmObject(deliveryOrder);
            }
            return deliveryOrder;
        }

        public DeliveryOrder CheckAndSetInvoiceComplete(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService)
        {
            IList<DeliveryOrderDetail> details = _deliveryOrderDetailService.GetObjectsByDeliveryOrderId(deliveryOrder.Id);

            foreach (var detail in details)
            {
                if (!detail.IsAllInvoiced)
                {
                    return deliveryOrder;
                }
            }
            return _repository.SetInvoiceComplete(deliveryOrder);
        }

        public DeliveryOrder UnsetInvoiceComplete(DeliveryOrder deliveryOrder)
        {
            _repository.UnsetInvoiceComplete(deliveryOrder);
            return deliveryOrder;
        }
    }
}