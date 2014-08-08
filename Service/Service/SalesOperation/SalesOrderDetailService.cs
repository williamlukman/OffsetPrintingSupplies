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
    public class SalesOrderDetailService : ISalesOrderDetailService
    {
        private ISalesOrderDetailRepository _repository;
        private ISalesOrderDetailValidator _validator;

        public SalesOrderDetailService(ISalesOrderDetailRepository _salesOrderDetailRepository, ISalesOrderDetailValidator _salesOrderDetailValidator)
        {
            _repository = _salesOrderDetailRepository;
            _validator = _salesOrderDetailValidator;
        }

        public ISalesOrderDetailValidator GetValidator()
        {
            return _validator;
        }

        public IList<SalesOrderDetail> GetObjectsBySalesOrderId(int salesOrderId)
        {
            return _repository.GetObjectsBySalesOrderId(salesOrderId);
        }

        public SalesOrderDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public SalesOrderDetail CreateObject(SalesOrderDetail salesOrderDetail, ISalesOrderService _salesOrderService, IItemService _itemService)
        {
            salesOrderDetail.Errors = new Dictionary<String, String>();
            if (_validator.ValidCreateObject(salesOrderDetail, this, _salesOrderService, _itemService))
            {
                salesOrderDetail.ContactId = _salesOrderService.GetObjectById(salesOrderDetail.SalesOrderId).ContactId;
                _repository.CreateObject(salesOrderDetail);
            }
            return salesOrderDetail;
        }

        public SalesOrderDetail CreateObject(int salesOrderId, int itemId, int quantity, decimal price, ISalesOrderService _salesOrderService, IItemService _itemService)
        {
            SalesOrderDetail sod = new SalesOrderDetail
            {
                SalesOrderId = salesOrderId,
                ItemId = itemId,
                Quantity = quantity,
                Price = price
            };
            return this.CreateObject(sod, _salesOrderService, _itemService);
        }

        public SalesOrderDetail UpdateObject(SalesOrderDetail salesOrderDetail, ISalesOrderService _salesOrderService, IItemService _itemService)
        {
            return (_validator.ValidUpdateObject(salesOrderDetail, this, _salesOrderService, _itemService) ? _repository.UpdateObject(salesOrderDetail) : salesOrderDetail);
        }

        public SalesOrderDetail SoftDeleteObject(SalesOrderDetail salesOrderDetail)
        {
            return (_validator.ValidDeleteObject(salesOrderDetail) ? _repository.SoftDeleteObject(salesOrderDetail) : salesOrderDetail);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public SalesOrderDetail ConfirmObject(SalesOrderDetail salesOrderDetail, DateTime ConfirmationDate, IStockMutationService _stockMutationService,
                                             IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidConfirmObject(salesOrderDetail))
            {
                salesOrderDetail.ConfirmationDate = ConfirmationDate;
                salesOrderDetail = _repository.ConfirmObject(salesOrderDetail);

                Item item = _itemService.GetObjectById(salesOrderDetail.ItemId);
                StockMutation stockMutation = _stockMutationService.CreateStockMutationForSalesOrder(salesOrderDetail, item);
                _stockMutationService.StockMutateObject(stockMutation, _itemService, _barringService, _warehouseItemService);
                // item.PendingDelivery += salesOrderDetail.Quantity;
            }
            return salesOrderDetail;
        }

        public SalesOrderDetail UnconfirmObject(SalesOrderDetail salesOrderDetail, ISalesOrderService _salesOrderService, IDeliveryOrderDetailService _deliveryOrderDetailService,
                                               IStockMutationService _stockMutationService, IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidUnconfirmObject(salesOrderDetail, _deliveryOrderDetailService, _itemService))
            {
                salesOrderDetail = _repository.UnconfirmObject(salesOrderDetail);
                Item item = _itemService.GetObjectById(salesOrderDetail.ItemId);
                IList<StockMutation> stockMutations = _stockMutationService.SoftDeleteStockMutationForSalesOrder(salesOrderDetail, item);
                foreach (var stockMutation in stockMutations)
                {
                    _stockMutationService.ReverseStockMutateObject(stockMutation, _itemService, _barringService, _warehouseItemService);
                    //item.PendingDelivery -= salesOrderDetail.Quantity;
                }
            }
            return salesOrderDetail;
        }
    }
}