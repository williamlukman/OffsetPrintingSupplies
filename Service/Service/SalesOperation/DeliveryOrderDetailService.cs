using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Constants;

namespace Service.Service
{
    public class DeliveryOrderDetailService : IDeliveryOrderDetailService
    {
        private IDeliveryOrderDetailRepository _repository;
        private IDeliveryOrderDetailValidator _validator;

        public DeliveryOrderDetailService(IDeliveryOrderDetailRepository _deliveryOrderDetailRepository, IDeliveryOrderDetailValidator _deliveryOrderDetailValidator)
        {
            _repository = _deliveryOrderDetailRepository;
            _validator = _deliveryOrderDetailValidator;
        }

        public IDeliveryOrderDetailValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<DeliveryOrderDetail> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<DeliveryOrderDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<DeliveryOrderDetail> GetObjectsByDeliveryOrderId(int deliveryOrderId)
        {
            return _repository.GetObjectsByDeliveryOrderId(deliveryOrderId);
        }

        public DeliveryOrderDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public IList<DeliveryOrderDetail> GetObjectsBySalesOrderDetailId(int salesOrderDetailId)
        {
            return _repository.GetObjectsBySalesOrderDetailId(salesOrderDetailId);
        }

        public DeliveryOrderDetail CreateObject(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderService _deliveryOrderService,
                                                ISalesOrderDetailService _salesOrderDetailService, ISalesOrderService _salesOrderService, IItemService _itemService)
        {
            deliveryOrderDetail.Errors = new Dictionary<String, String>();
            if (_validator.ValidCreateObject(deliveryOrderDetail, this, _deliveryOrderService, _salesOrderDetailService, _itemService))
            { 
                _repository.CreateObject(deliveryOrderDetail);
            }
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail UpdateObject(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderService _deliveryOrderService,
                                                ISalesOrderDetailService _salesOrderDetailService, ISalesOrderService _salesOrderService, IItemService _itemService)
        {
            return (_validator.ValidUpdateObject(deliveryOrderDetail, this, _deliveryOrderService, _salesOrderDetailService, _itemService) ?
                    _repository.UpdateObject(deliveryOrderDetail) : deliveryOrderDetail);
        }

        public DeliveryOrderDetail SoftDeleteObject(DeliveryOrderDetail deliveryOrderDetail)
        {
            return (_validator.ValidDeleteObject(deliveryOrderDetail) ? _repository.SoftDeleteObject(deliveryOrderDetail) : deliveryOrderDetail);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public DeliveryOrderDetail ConfirmObject(DeliveryOrderDetail deliveryOrderDetail, DateTime ConfirmationDate, IDeliveryOrderService _deliveryOrderService, ISalesOrderDetailService _salesOrderDetailService,
                                                 IStockMutationService _stockMutationService, IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService,
                                                 IServiceCostService _serviceCostService, ICustomerStockMutationService _customerStockMutationService, ICustomerItemService _customerItemService)
        {
            deliveryOrderDetail.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(deliveryOrderDetail, _deliveryOrderService, this, _salesOrderDetailService,
                                              _itemService, _warehouseItemService, _serviceCostService, _customerItemService))
            {
                DeliveryOrder deliveryOrder = _deliveryOrderService.GetObjectById(deliveryOrderDetail.DeliveryOrderId);
                SalesOrderDetail salesOrderDetail = _salesOrderDetailService.GetObjectById(deliveryOrderDetail.SalesOrderDetailId);
                WarehouseItem warehouseItem = _warehouseItemService.FindOrCreateObject(deliveryOrder.WarehouseId, deliveryOrderDetail.ItemId);
                Item item = _itemService.GetObjectById(deliveryOrderDetail.ItemId);
                if (!salesOrderDetail.IsService)
                {
                    IList<StockMutation> stockMutations = _stockMutationService.CreateStockMutationForDeliveryOrder(deliveryOrderDetail, warehouseItem);
                    foreach (var stockMutation in stockMutations)
                    {
                        _stockMutationService.StockMutateObject(stockMutation, _itemService, _blanketService, _warehouseItemService);
                    }
                    deliveryOrderDetail.COGS = deliveryOrderDetail.Quantity * item.AvgPrice;
                }
                else
                {
                    CustomerItem customerItem = _customerItemService.FindOrCreateObject(deliveryOrder.SalesOrder.ContactId, warehouseItem.Id);
                    IList<CustomerStockMutation> customerStockMutations = _customerStockMutationService.CreateCustomerStockMutationForDeliveryOrder(deliveryOrderDetail, customerItem);
                    foreach (var customerStockMutation in customerStockMutations)
                    {
                        _customerStockMutationService.StockMutateObject(customerStockMutation, false, _itemService, _customerItemService, _warehouseItemService);
                    }
                    deliveryOrderDetail.COGS = deliveryOrderDetail.Quantity * item.CustomerAvgPrice;
                }
                deliveryOrderDetail = _repository.ConfirmObject(deliveryOrderDetail);
                if (deliveryOrderDetail.OrderType != Constant.OrderTypeCase.PartDeliveryOrder)
                {
                    _salesOrderDetailService.SetDeliveryComplete(salesOrderDetail, deliveryOrderDetail.Quantity);
                }
            }
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail UnconfirmObject(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderService _deliveryOrderService, ISalesOrderService _salesOrderService,
                                                   ISalesOrderDetailService _salesOrderDetailService, ISalesInvoiceDetailService _salesInvoiceDetailService, IStockMutationService _stockMutationService,
                                                   IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService, ICustomerStockMutationService _customerStockMutationService, ICustomerItemService _customerItemService)
        {
            if (_validator.ValidUnconfirmObject(deliveryOrderDetail, _salesInvoiceDetailService))
            {
                DeliveryOrder deliveryOrder = _deliveryOrderService.GetObjectById(deliveryOrderDetail.DeliveryOrderId);
                SalesOrderDetail salesOrderDetail = _salesOrderDetailService.GetObjectById(deliveryOrderDetail.SalesOrderDetailId);
                WarehouseItem warehouseItem = _warehouseItemService.FindOrCreateObject(deliveryOrder.WarehouseId, deliveryOrderDetail.ItemId);
                Item item = _itemService.GetObjectById(deliveryOrderDetail.ItemId);
                if (!salesOrderDetail.IsService)
                {
                    IList<StockMutation> stockMutations = _stockMutationService.GetObjectsBySourceDocumentDetailForWarehouseItem(warehouseItem.Id, Constant.SourceDocumentDetailType.DeliveryOrderDetail, deliveryOrderDetail.Id);
                    foreach (var stockMutation in stockMutations)
                    {
                        _stockMutationService.ReverseStockMutateObject(stockMutation, _itemService, _blanketService, _warehouseItemService);
                    }
                    _stockMutationService.DeleteStockMutationForDeliveryOrder(deliveryOrderDetail, warehouseItem);
                } 
                else
                {
                    CustomerItem customerItem = _customerItemService.FindOrCreateObject(deliveryOrder.SalesOrder.ContactId, warehouseItem.Id);
                    IList<CustomerStockMutation> customerStockMutations = _customerStockMutationService.GetObjectsBySourceDocumentDetailForCustomerItem(customerItem.Id, Constant.SourceDocumentDetailType.DeliveryOrderDetail, deliveryOrderDetail.Id);
                    foreach (var customerStockMutation in customerStockMutations)
                    {
                        _customerStockMutationService.ReverseStockMutateObject(customerStockMutation, false, _itemService, _customerItemService, _warehouseItemService);
                    }
                    _customerStockMutationService.DeleteCustomerStockMutationForDeliveryOrder(deliveryOrderDetail, customerItem);
                }
                deliveryOrderDetail.COGS = 0;
                _salesOrderDetailService.UnsetDeliveryComplete(salesOrderDetail, deliveryOrderDetail.Quantity, _salesOrderService);
                deliveryOrderDetail = _repository.UnconfirmObject(deliveryOrderDetail);
            }
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail InvoiceObject(DeliveryOrderDetail deliveryOrderDetail, int Quantity)
        {
            deliveryOrderDetail.PendingInvoicedQuantity -= Quantity;
            if (deliveryOrderDetail.PendingInvoicedQuantity == 0) { deliveryOrderDetail.IsAllInvoiced = true; }
            _repository.UpdateObject(deliveryOrderDetail);
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail UndoInvoiceObject(DeliveryOrderDetail deliveryOrderDetail, int Quantity, IDeliveryOrderService _deliveryOrderService)
        {
            DeliveryOrder deliveryOrder = _deliveryOrderService.GetObjectById(deliveryOrderDetail.DeliveryOrderId);
            _deliveryOrderService.UnsetInvoiceComplete(deliveryOrder);

            deliveryOrderDetail.IsAllInvoiced = false;
            deliveryOrderDetail.PendingInvoicedQuantity += Quantity;
            _repository.UpdateObject(deliveryOrderDetail);
            return deliveryOrderDetail;
        }

    }
}