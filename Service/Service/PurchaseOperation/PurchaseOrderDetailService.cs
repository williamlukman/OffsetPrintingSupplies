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

        public IQueryable<PurchaseOrderDetail> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<PurchaseOrderDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<PurchaseOrderDetail> GetObjectsByPurchaseOrderId(int purchaseOrderId)
        {
            return _repository.GetObjectsByPurchaseOrderId(purchaseOrderId);
        }

        public IList<PurchaseOrderDetail> GetObjectsByItemId(int itemId)
        {
            return _repository.GetObjectsByItemId(itemId);
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
                return _repository.CreateObject(purchaseOrderDetail);
            }
            else
            {
                return purchaseOrderDetail;
            }
        }

        public PurchaseOrderDetail UpdateObject(PurchaseOrderDetail purchaseOrderDetail, decimal PendingDiff, IPurchaseOrderService _purchaseOrderService, IItemService _itemService,
                                                IPurchaseReceivalService _purchaseReceivalService, IPurchaseReceivalDetailService _purchaseReceivalDetailService,
                                                IStockMutationService _stockMutationService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidUpdateObject(purchaseOrderDetail, this, _purchaseOrderService, _itemService, _purchaseReceivalService, _purchaseReceivalDetailService))
            {
                if (PendingDiff != 0 && purchaseOrderDetail.IsConfirmed)
                {
                    var item = _itemService.GetObjectById(purchaseOrderDetail.ItemId);
                    StockMutation stockMutation = _stockMutationService.CreateStockMutationForPurchaseOrderDetail(purchaseOrderDetail, item, PendingDiff);
                    //item.PendingReceival += PendingDiff;
                    _stockMutationService.StockMutateObject(stockMutation, _itemService, _blanketService, _warehouseItemService);

                }
                _repository.UpdateObject(purchaseOrderDetail);
            }
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail SoftDeleteObject(PurchaseOrderDetail purchaseOrderDetail)
        {
            return (_validator.ValidDeleteObject(purchaseOrderDetail) ? _repository.SoftDeleteObject(purchaseOrderDetail) : purchaseOrderDetail);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public PurchaseOrderDetail ConfirmObject(PurchaseOrderDetail purchaseOrderDetail, DateTime ConfirmationDate, IStockMutationService _stockMutationService,
                                                 IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService)
        {
            purchaseOrderDetail.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(purchaseOrderDetail))
            {
                purchaseOrderDetail = _repository.ConfirmObject(purchaseOrderDetail);

                Item item = _itemService.GetObjectById(purchaseOrderDetail.ItemId);
                StockMutation stockMutation = _stockMutationService.CreateStockMutationForPurchaseOrder(purchaseOrderDetail, item);
                //item.PendingReceival += purchaseOrderDetail.Quantity;
                _stockMutationService.StockMutateObject(stockMutation, _itemService, _blanketService, _warehouseItemService);
            }
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail UnconfirmObject(PurchaseOrderDetail purchaseOrderDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService,
                                                   IStockMutationService _stockMutationService, IItemService _itemService, IBlanketService _blanketService,
                                                   IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidUnconfirmObject(purchaseOrderDetail, _purchaseReceivalDetailService, _itemService))
            {
                purchaseOrderDetail = _repository.UnconfirmObject(purchaseOrderDetail);
                Item item = _itemService.GetObjectById(purchaseOrderDetail.ItemId);
                IList<StockMutation> stockMutations = _stockMutationService.GetStockMutationForPurchaseOrder(purchaseOrderDetail, item);
                foreach (var stockMutation in stockMutations)
                {
                    //item.PendingReceival -= purchaseOrderDetail.Quantity;
                    _stockMutationService.ReverseStockMutateObject(stockMutation, _itemService, _blanketService, _warehouseItemService);
                }
            }
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail SetReceivalComplete(PurchaseOrderDetail purchaseOrderDetail, decimal Quantity)
        {
            purchaseOrderDetail.PendingReceivalQuantity -= Quantity;
            if (purchaseOrderDetail.PendingReceivalQuantity == 0) { purchaseOrderDetail.IsAllReceived = true; }
            _repository.UpdateObject(purchaseOrderDetail);
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail UnsetReceivalComplete(PurchaseOrderDetail purchaseOrderDetail, decimal Quantity, IPurchaseOrderService _purchaseOrderService)
        {
            PurchaseOrder purchaseOrder = _purchaseOrderService.GetObjectById(purchaseOrderDetail.PurchaseOrderId);
            _purchaseOrderService.UnsetReceivalComplete(purchaseOrder);

            purchaseOrderDetail.IsAllReceived = false;
            purchaseOrderDetail.PendingReceivalQuantity += Quantity;
            _repository.UpdateObject(purchaseOrderDetail);
            return purchaseOrderDetail;
        }

    }
}