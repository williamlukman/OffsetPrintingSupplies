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
                purchaseOrderDetail.ContactId = _purchaseOrderService.GetObjectById(purchaseOrderDetail.PurchaseOrderId).ContactId;
                return _repository.CreateObject(purchaseOrderDetail);
            }
            else
            {
                return purchaseOrderDetail;
            }
        }

        public PurchaseOrderDetail CreateObject(int purchaseOrderId, int itemId, int quantity, decimal price, IPurchaseOrderService _purchaseOrderService, IItemService _itemService)
        {
            PurchaseOrderDetail pod = new PurchaseOrderDetail
            {
                PurchaseOrderId = purchaseOrderId,
                ItemId = itemId,
                ContactId = 0,
                Quantity = quantity,
                Price = price
            };
            return this.CreateObject(pod, _purchaseOrderService, _itemService);
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

        public PurchaseOrderDetail ConfirmObject(PurchaseOrderDetail purchaseOrderDetail, IStockMutationService _stockMutationService, IItemService _itemService)
        {
            if (_validator.ValidConfirmObject(purchaseOrderDetail))
            {
                purchaseOrderDetail = _repository.ConfirmObject(purchaseOrderDetail);
                Item item = _itemService.GetObjectById(purchaseOrderDetail.ItemId);
                item.PendingReceival += purchaseOrderDetail.Quantity;
                _itemService.UpdateObject(item);
                StockMutation sm = _stockMutationService.CreateStockMutationForPurchaseOrder(purchaseOrderDetail, item);
            }
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail UnconfirmObject(PurchaseOrderDetail purchaseOrderDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService, IStockMutationService _stockMutationService, IItemService _itemService)
        {
            if (_validator.ValidUnconfirmObject(purchaseOrderDetail, this, _purchaseReceivalDetailService, _itemService))
            {
                purchaseOrderDetail = _repository.UnconfirmObject(purchaseOrderDetail);
                Item item = _itemService.GetObjectById(purchaseOrderDetail.ItemId);
                item.PendingReceival -= purchaseOrderDetail.Quantity;
                _itemService.UpdateObject(item);
                IList<StockMutation> sm = _stockMutationService.SoftDeleteStockMutationForPurchaseOrder(purchaseOrderDetail, item);
            }
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail FulfilObject(PurchaseOrderDetail purchaseOrderDetail)
        {
            return _repository.FulfilObject(purchaseOrderDetail);
        }

    }
}