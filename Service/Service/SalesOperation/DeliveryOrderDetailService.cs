using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


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

        public DeliveryOrderDetail CreateObject(DeliveryOrderDetail deliveryOrderDetail, 
                                                IDeliveryOrderService _deliveryOrderService, ISalesOrderDetailService _salesOrderDetailService,
                                                ISalesOrderService _salesOrderService, IItemService _itemService, ICustomerService _customerService)
        {
            deliveryOrderDetail.Errors = new Dictionary<String, String>();
            if (_validator.ValidCreateObject(deliveryOrderDetail, this, _deliveryOrderService, _salesOrderDetailService, _salesOrderService, _itemService, _customerService))
            { 
                deliveryOrderDetail.CustomerId = _deliveryOrderService.GetObjectById(deliveryOrderDetail.DeliveryOrderId).CustomerId;
                _repository.CreateObject(deliveryOrderDetail);
            }
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail CreateObject(int deliveryOrderId, int itemId, int quantity, int salesOrderDetailId,
                                                IDeliveryOrderService _deliveryOrderService, ISalesOrderDetailService _salesOrderDetailService,
                                                ISalesOrderService _salesOrderService, IItemService _itemService, ICustomerService _customerService)
        {
            DeliveryOrderDetail deliveryOrderDetail = new DeliveryOrderDetail
            {
                DeliveryOrderId = deliveryOrderId,
                ItemId = itemId,
                Quantity = quantity,
                SalesOrderDetailId = salesOrderDetailId
            };
            return this.CreateObject(deliveryOrderDetail, _deliveryOrderService, _salesOrderDetailService, _salesOrderService, _itemService, _customerService);
        }

        public DeliveryOrderDetail UpdateObject(DeliveryOrderDetail deliveryOrderDetail,
                                                IDeliveryOrderService _deliveryOrderService, ISalesOrderDetailService _salesOrderDetailService,
                                                ISalesOrderService _salesOrderService, IItemService _itemService, ICustomerService _customerService)
        {
            return (_validator.ValidUpdateObject(deliveryOrderDetail, this, _deliveryOrderService, _salesOrderDetailService, _salesOrderService, _itemService, _customerService) ?
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

        public DeliveryOrderDetail FinishObject(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderService _deliveryOrderService, ISalesOrderDetailService _salesOrderDetailService,
                                                IStockMutationService _stockMutationService, IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidFinishObject(deliveryOrderDetail, _deliveryOrderService, _itemService, _warehouseItemService))
            {
                deliveryOrderDetail = _repository.FinishObject(deliveryOrderDetail);

                // If valid, deliveryOrder.IsCompleted = true
                DeliveryOrder deliveryOrder = _deliveryOrderService.GetObjectById(deliveryOrderDetail.DeliveryOrderId);
                if (_deliveryOrderService.GetValidator().ValidCompleteObject(deliveryOrder, this))
                {
                    _deliveryOrderService.CompleteObject(deliveryOrder, this);
                }
    
                WarehouseItem warehouseItem = _warehouseItemService.FindOrCreateObject(deliveryOrder.WarehouseId, deliveryOrderDetail.ItemId);
                Item item = _itemService.GetObjectById(deliveryOrderDetail.ItemId);
                IList<StockMutation> stockMutations = _stockMutationService.CreateStockMutationForDeliveryOrder(deliveryOrderDetail, warehouseItem);
                foreach (var stockMutation in stockMutations)
                {
                    //item.PendingDelivery -= deliveryOrderDetail.Quantity;
                    //item.Quantity -= deliveryOrderDetail.Quantity;
                    _stockMutationService.StockMutateObject(stockMutation, _itemService, _barringService, _warehouseItemService);
                }

                // SalesOrderDetail.IsDelivered = true
                SalesOrderDetail salesOrderDetail = _salesOrderDetailService.GetObjectById(deliveryOrderDetail.SalesOrderDetailId);
                if (_salesOrderDetailService.GetValidator().ValidDeliverObject(salesOrderDetail, this))
                {
                    _salesOrderDetailService.DeliverObject(salesOrderDetail, this);
                }
            }
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail UnfinishObject(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderService _deliveryOrderService, IStockMutationService _stockMutationService,
                                                  IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidUnfinishObject(deliveryOrderDetail, _deliveryOrderService, this, _itemService))
            {
                deliveryOrderDetail = _repository.UnfinishObject(deliveryOrderDetail);
                DeliveryOrder deliveryOrder = _deliveryOrderService.GetObjectById(deliveryOrderDetail.DeliveryOrderId);
                WarehouseItem warehouseItem = _warehouseItemService.FindOrCreateObject(deliveryOrder.WarehouseId, deliveryOrderDetail.ItemId);
                Item item = _itemService.GetObjectById(deliveryOrderDetail.ItemId);
                IList<StockMutation> stockMutations = _stockMutationService.SoftDeleteStockMutationForDeliveryOrder(deliveryOrderDetail, warehouseItem);
                foreach (var stockMutation in stockMutations)
                {
                    //item.PendingDelivery += deliveryOrderDetail.Quantity;
                    //item.Quantity += deliveryOrderDetail.Quantity;
                    _stockMutationService.ReverseStockMutateObject(stockMutation, _itemService, _barringService, _warehouseItemService);
                }
            }
            return deliveryOrderDetail;
        }
    }
}