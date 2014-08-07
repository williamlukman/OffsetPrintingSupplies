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
    public class PurchaseReceivalDetailService : IPurchaseReceivalDetailService
    {
        private IPurchaseReceivalDetailRepository _repository;
        private IPurchaseReceivalDetailValidator _validator;

        public PurchaseReceivalDetailService(IPurchaseReceivalDetailRepository _purchaseReceivalDetailRepository, IPurchaseReceivalDetailValidator _purchaseReceivalDetailValidator)
        {
            _repository = _purchaseReceivalDetailRepository;
            _validator = _purchaseReceivalDetailValidator;
        }

        public IPurchaseReceivalDetailValidator GetValidator()
        {
            return _validator;
        }

        public IList<PurchaseReceivalDetail> GetObjectsByPurchaseReceivalId(int purchaseReceivalId)
        {
            return _repository.GetObjectsByPurchaseReceivalId(purchaseReceivalId);
        }

        public PurchaseReceivalDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public PurchaseReceivalDetail GetObjectByPurchaseOrderDetailId(int purchaseOrderDetailId)
        {
            return _repository.GetObjectByPurchaseOrderDetailId(purchaseOrderDetailId);
        }

        public PurchaseReceivalDetail CreateObject(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalService _purchaseReceivalService,
                                                     IPurchaseOrderDetailService _purchaseOrderDetailService, IPurchaseOrderService _purchaseOrderService,
                                                     IItemService _itemService, ICustomerService _customerService)
        {
            purchaseReceivalDetail.Errors = new Dictionary<String, String>();
            if (_validator.ValidCreateObject(purchaseReceivalDetail, this, _purchaseReceivalService,
                                        _purchaseOrderDetailService, _purchaseOrderService, _itemService, _customerService))
            {
                purchaseReceivalDetail.CustomerId = _purchaseReceivalService.GetObjectById(purchaseReceivalDetail.PurchaseReceivalId).CustomerId;
                return _repository.CreateObject(purchaseReceivalDetail);
            }
            else
            {
                return purchaseReceivalDetail;
            }
        }

        public PurchaseReceivalDetail CreateObject(int purchaseReceivalId, int itemId, int quantity, int purchaseOrderDetailId,
                                                    IPurchaseReceivalService _purchaseReceivalService, IPurchaseOrderDetailService _purchaseOrderDetailService,
                                                    IPurchaseOrderService _purchaseOrderService, IItemService _itemService, ICustomerService _customerService)
        {
            PurchaseReceivalDetail prd = new PurchaseReceivalDetail
            {
                PurchaseReceivalId = purchaseReceivalId,
                ItemId = itemId,
                Quantity = quantity,
                PurchaseOrderDetailId = purchaseOrderDetailId
            };
            return this.CreateObject(prd, _purchaseReceivalService, _purchaseOrderDetailService, _purchaseOrderService, _itemService, _customerService);
        }


        public PurchaseReceivalDetail UpdateObject(PurchaseReceivalDetail purchaseReceivalDetail,
                                                    IPurchaseReceivalService _purchaseReceivalService, IPurchaseOrderDetailService _purchaseOrderDetailService,
                                                    IPurchaseOrderService _purchaseOrderService, IItemService _itemService, ICustomerService _customerService)
        {
            return (_validator.ValidUpdateObject(purchaseReceivalDetail, this, _purchaseReceivalService, _purchaseOrderDetailService, _purchaseOrderService, _itemService, _customerService) ?
                    _repository.UpdateObject(purchaseReceivalDetail) : purchaseReceivalDetail);
        }

        public PurchaseReceivalDetail SoftDeleteObject(PurchaseReceivalDetail purchaseReceivalDetail)
        {
            return (_validator.ValidDeleteObject(purchaseReceivalDetail) ? _repository.SoftDeleteObject(purchaseReceivalDetail) : purchaseReceivalDetail);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public PurchaseReceivalDetail FinishObject(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalService _purchaseReceivalService, IPurchaseOrderDetailService _purchaseOrderDetailService,
                                                   IStockMutationService _stockMutationService, IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidFinishObject(purchaseReceivalDetail))
            {
                purchaseReceivalDetail = _repository.FinishObject(purchaseReceivalDetail);
                PurchaseReceival purchaseReceival = _purchaseReceivalService.GetObjectById(purchaseReceivalDetail.PurchaseReceivalId);
                WarehouseItem warehouseItem = _warehouseItemService.FindOrCreateObject(purchaseReceival.WarehouseId, purchaseReceivalDetail.ItemId);
                Item item = _itemService.GetObjectById(purchaseReceivalDetail.ItemId);
                IList<StockMutation> stockMutations = _stockMutationService.CreateStockMutationForPurchaseReceival(purchaseReceivalDetail, warehouseItem);
                foreach (var stockMutation in stockMutations)
                {
                    // decimal itemPrice = _purchaseOrderDetailService.GetObjectById(purchaseReceivalDetail.PurchaseOrderDetailId).Price;
                    // item.AvgCost = _itemService.CalculateAvgCost(item, purchaseReceivalDetail.Quantity, itemPrice);
                    //item.PendingReceival -= purchaseReceivalDetail.Quantity;
                    //item.Quantity += purchaseReceivalDetail.Quantity;
                    _stockMutationService.StockMutateObject(stockMutation, _itemService, _barringService, _warehouseItemService);
                }
            }
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail UnfinishObject(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalService _purchaseReceivalService, IPurchaseOrderDetailService _purchaseOrderDetailService,
                                                     IStockMutationService _stockMutationService, IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidUnfinishObject(purchaseReceivalDetail, _purchaseReceivalService, this, _itemService))
            {
                purchaseReceivalDetail = _repository.UnfinishObject(purchaseReceivalDetail);
                PurchaseReceival purchaseReceival = _purchaseReceivalService.GetObjectById(purchaseReceivalDetail.PurchaseReceivalId);
                WarehouseItem warehouseItem = _warehouseItemService.FindOrCreateObject(purchaseReceival.WarehouseId, purchaseReceivalDetail.ItemId);
                Item item = _itemService.GetObjectById(purchaseReceivalDetail.ItemId);
                IList<StockMutation> stockMutations = _stockMutationService.SoftDeleteStockMutationForPurchaseReceival(purchaseReceivalDetail, warehouseItem);
                foreach (var stockMutation in stockMutations)
                {
                    //decimal itemPrice = _purchaseOrderDetailService.GetObjectById(purchaseReceivalDetail.PurchaseOrderDetailId).Price;
                    //item.AvgCost = _itemService.CalculateAvgCost(item, purchaseReceivalDetail.Quantity * (-1), itemPrice);
                    //item.PendingReceival += purchaseReceivalDetail.Quantity;
                    //item.Quantity -= purchaseReceivalDetail.Quantity;
                    _stockMutationService.ReverseStockMutateObject(stockMutation, _itemService, _barringService, _warehouseItemService);
                }
            }
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail InvoiceObject(PurchaseReceivalDetail purchaseReceivalDetail, int Quantity)
        {
            purchaseReceivalDetail.PendingInvoicedQuantity -= Quantity;
            purchaseReceivalDetail.InvoicedQuantity += Quantity;
            if (purchaseReceivalDetail.PendingInvoicedQuantity == 0) { purchaseReceivalDetail.IsAllInvoiced = true; }
            _repository.UpdateObject(purchaseReceivalDetail);
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail UndoInvoiceObject(PurchaseReceivalDetail purchaseReceivalDetail, int Quantity, IPurchaseReceivalService _purchaseReceivalService)
        {
            PurchaseReceival purchaseReceival = _purchaseReceivalService.GetObjectById(purchaseReceivalDetail.PurchaseReceivalId);
            _purchaseReceivalService.UnsetInvoiceComplete(purchaseReceival);

            purchaseReceivalDetail.IsAllInvoiced = false;
            purchaseReceivalDetail.PendingInvoicedQuantity += Quantity;
            purchaseReceivalDetail.InvoicedQuantity -= Quantity;
            _repository.UpdateObject(purchaseReceivalDetail);
            return purchaseReceivalDetail;
        }
    }
}