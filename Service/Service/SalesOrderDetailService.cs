using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                return _repository.CreateObject(salesOrderDetail);
            }
            else
            {
                return salesOrderDetail;
            }
        }

        public SalesOrderDetail CreateObject(int salesOrderId, int itemId, int quantity, decimal price, ISalesOrderService _salesOrderService, IItemService _itemService)
        {
            SalesOrderDetail sod = new SalesOrderDetail
            {
                SalesOrderId = salesOrderId,
                ItemId = itemId,
                ContactId = 0,
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

        public SalesOrderDetail ConfirmObject(SalesOrderDetail salesOrderDetail, IStockMutationService _stockMutationService, IItemService _itemService)
        {
            if (_validator.ValidConfirmObject(salesOrderDetail))
            {
                salesOrderDetail = _repository.ConfirmObject(salesOrderDetail);
                Item item = _itemService.GetObjectById(salesOrderDetail.ItemId);
                item.PendingDelivery += salesOrderDetail.Quantity;
                _itemService.UpdateObject(item);
                StockMutation sm = _stockMutationService.CreateStockMutationForSalesOrder(salesOrderDetail, item);
            }
            return salesOrderDetail;
        }

        public SalesOrderDetail UnconfirmObject(SalesOrderDetail salesOrderDetail, IDeliveryOrderDetailService _deliveryOrderDetailService, IStockMutationService _stockMutationService, IItemService _itemService)
        {
            if (_validator.ValidUnconfirmObject(salesOrderDetail, this, _deliveryOrderDetailService, _itemService))
            {
                salesOrderDetail = _repository.UnconfirmObject(salesOrderDetail);
                Item item = _itemService.GetObjectById(salesOrderDetail.ItemId);
                item.PendingDelivery -= salesOrderDetail.Quantity;
                _itemService.UpdateObject(item);
                IList<StockMutation> sm = _stockMutationService.SoftDeleteStockMutationForSalesOrder(salesOrderDetail, item);
            }
            return salesOrderDetail;
        }

        public SalesOrderDetail FulfilObject(SalesOrderDetail salesOrderDetail)
        {
            return _repository.FulfilObject(salesOrderDetail);
        }
    }
}