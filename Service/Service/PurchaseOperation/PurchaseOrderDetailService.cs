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
    public class PurchaseOrderDetailService : IPurchaseOrderDetailService
    {
        private IPurchaseOrderDetailRepository _repository;
        private IPurchaseOrderDetailValidator _validator;

        public PurchaseOrderDetailService(IPurchaseOrderDetailRepository _purchaseOrderDetailRepository, IPurchaseOrderDetailValidator _purchaseOrderDetailValidator)
        {
            _repository = _purchaseOrderDetailRepository;
            _validator = _purchaseOrderDetailValidator;
        }

        public IPurchaseOrderDetailValidator GetValidator()
        {
            return _validator;
        }

        public IList<PurchaseOrderDetail> GetObjectsByPurchaseOrderId(int purchaseOrderId)
        {
            return _repository.GetObjectsByPurchaseOrderId(purchaseOrderId);
        }

        public PurchaseOrderDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public PurchaseOrderDetail CreateObject(PurchaseOrderDetail purchaseOrderDetail, IPurchaseOrderService _purchaseOrderService, IItemService _itemService)
        {
            purchaseOrderDetail.Errors = new Dictionary<String, String>();
            if (_validator.ValidCreateObject(purchaseOrderDetail, this, _purchaseOrderService, _itemService))
            {
                purchaseOrderDetail.CustomerId = _purchaseOrderService.GetObjectById(purchaseOrderDetail.PurchaseOrderId).CustomerId;
                return _repository.CreateObject(purchaseOrderDetail);
            }
            else
            {
                return purchaseOrderDetail;
            }
        }

        public PurchaseOrderDetail CreateObject(int purchaseOrderId, int itemId, int quantity, decimal price, IPurchaseOrderService _purchaseOrderService, IItemService _itemService)
        {
            PurchaseOrderDetail purchaseOrderDetail = new PurchaseOrderDetail
            {
                PurchaseOrderId = purchaseOrderId,
                ItemId = itemId,
                Quantity = quantity,
                Price = price
            };
            return this.CreateObject(purchaseOrderDetail, _purchaseOrderService, _itemService);
        }

        public PurchaseOrderDetail UpdateObject(PurchaseOrderDetail purchaseOrderDetail, IPurchaseOrderService _purchaseOrderService, IItemService _itemService)
        {
            return (_validator.ValidUpdateObject(purchaseOrderDetail, this, _purchaseOrderService, _itemService) ?
                     _repository.UpdateObject(purchaseOrderDetail) : purchaseOrderDetail);
        }

        public PurchaseOrderDetail SoftDeleteObject(PurchaseOrderDetail purchaseOrderDetail)
        {
            return (_validator.ValidDeleteObject(purchaseOrderDetail) ? _repository.SoftDeleteObject(purchaseOrderDetail) : purchaseOrderDetail);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public PurchaseOrderDetail FinishObject(PurchaseOrderDetail purchaseOrderDetail, IPurchaseOrderService _purchaseOrderService, IStockMutationService _stockMutationService,
                                                IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidFinishObject(purchaseOrderDetail))
            {
                purchaseOrderDetail = _repository.FinishObject(purchaseOrderDetail);

                // complete purchase order
                PurchaseOrder purchaseOrder = _purchaseOrderService.GetObjectById(purchaseOrderDetail.PurchaseOrderId);
                if (_purchaseOrderService.GetValidator().ValidCompleteObject(purchaseOrder, this))
                {
                    _purchaseOrderService.CompleteObject(purchaseOrder, this);
                }

                Item item = _itemService.GetObjectById(purchaseOrderDetail.ItemId);
                StockMutation stockMutation = _stockMutationService.CreateStockMutationForPurchaseOrder(purchaseOrderDetail, item);
                //item.PendingReceival += purchaseOrderDetail.Quantity;
                _stockMutationService.StockMutateObject(stockMutation, _itemService, _barringService, _warehouseItemService);
            }
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail UnfinishObject(PurchaseOrderDetail purchaseOrderDetail, IPurchaseOrderService _purchaseOrderService, IPurchaseReceivalDetailService _purchaseReceivalDetailService,
                                                  IStockMutationService _stockMutationService, IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidUnfinishObject(purchaseOrderDetail, _purchaseOrderService, this, _purchaseReceivalDetailService, _itemService))
            {
                purchaseOrderDetail = _repository.UnfinishObject(purchaseOrderDetail);
                Item item = _itemService.GetObjectById(purchaseOrderDetail.ItemId);
                IList<StockMutation> stockMutations = _stockMutationService.SoftDeleteStockMutationForPurchaseOrder(purchaseOrderDetail, item);
                foreach (var stockMutation in stockMutations)
                {
                    //item.PendingReceival -= purchaseOrderDetail.Quantity;
                    _stockMutationService.ReverseStockMutateObject(stockMutation, _itemService, _barringService, _warehouseItemService);
                }
            }
            return purchaseOrderDetail;
        }
    }
}