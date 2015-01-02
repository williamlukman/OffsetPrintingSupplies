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

        public IQueryable<PurchaseReceivalDetail> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<PurchaseReceivalDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<PurchaseReceivalDetail> GetObjectsByPurchaseReceivalId(int purchaseReceivalId)
        {
            return _repository.GetObjectsByPurchaseReceivalId(purchaseReceivalId);
        }

        public PurchaseReceivalDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public IList<PurchaseReceivalDetail> GetObjectsByPurchaseOrderDetailId(int purchaseOrderDetailId)
        {
            return _repository.GetObjectsByPurchaseOrderDetailId(purchaseOrderDetailId);
        }

        public PurchaseReceivalDetail CreateObject(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalService _purchaseReceivalService,
                                                     IPurchaseOrderDetailService _purchaseOrderDetailService, IPurchaseOrderService _purchaseOrderService,
                                                     IItemService _itemService)
        {
            purchaseReceivalDetail.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(purchaseReceivalDetail, this, _purchaseReceivalService, _purchaseOrderDetailService, _itemService) ?
                    _repository.CreateObject(purchaseReceivalDetail) : purchaseReceivalDetail);
        }

        public PurchaseReceivalDetail CreateObject(int purchaseReceivalId, int itemId, int quantity, int purchaseOrderDetailId,
                                                    IPurchaseReceivalService _purchaseReceivalService, IPurchaseOrderDetailService _purchaseOrderDetailService,
                                                    IPurchaseOrderService _purchaseOrderService, IItemService _itemService)
        {
            PurchaseReceivalDetail purchaseReceivalDetail = new PurchaseReceivalDetail
            {
                PurchaseReceivalId = purchaseReceivalId,
                ItemId = itemId,
                Quantity = quantity,
                PurchaseOrderDetailId = purchaseOrderDetailId
            };
            return this.CreateObject(purchaseReceivalDetail, _purchaseReceivalService, _purchaseOrderDetailService, _purchaseOrderService, _itemService);
        }

        public PurchaseReceivalDetail UpdateObject(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalService _purchaseReceivalService,
                                                   IPurchaseOrderDetailService _purchaseOrderDetailService, IPurchaseOrderService _purchaseOrderService,
                                                   IItemService _itemService)
        {
            return (_validator.ValidUpdateObject(purchaseReceivalDetail, this, _purchaseReceivalService, _purchaseOrderDetailService, _itemService) ?
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

        public PurchaseReceivalDetail ConfirmObject(PurchaseReceivalDetail purchaseReceivalDetail, DateTime ConfirmationDate, IPurchaseReceivalService _purchaseReceivalService,
                                                    IPurchaseOrderDetailService _purchaseOrderDetailService, IStockMutationService _stockMutationService,
                                                    IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService)
        {
            purchaseReceivalDetail.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(purchaseReceivalDetail, this, _purchaseOrderDetailService))
            {
                PurchaseReceival purchaseReceival = _purchaseReceivalService.GetObjectById(purchaseReceivalDetail.PurchaseReceivalId);
                WarehouseItem warehouseItem = _warehouseItemService.FindOrCreateObject(purchaseReceival.WarehouseId, purchaseReceivalDetail.ItemId);
                Item item = _itemService.GetObjectById(purchaseReceivalDetail.ItemId);
                IList<StockMutation> stockMutations = _stockMutationService.CreateStockMutationForPurchaseReceival(purchaseReceivalDetail, warehouseItem);
                foreach (var stockMutation in stockMutations)
                {
                    //item.PendingReceival -= purchaseReceivalDetail.Quantity;
                    //item.Quantity += purchaseReceivalDetail.Quantity;
                    _stockMutationService.StockMutateObject(stockMutation, _itemService, _blanketService, _warehouseItemService);
                }
                decimal itemPrice = _purchaseOrderDetailService.GetObjectById(purchaseReceivalDetail.PurchaseOrderDetailId).Price * purchaseReceival.ExchangeRateAmount;
                item.AvgPrice = _itemService.CalculateAndUpdateAvgPrice(item, purchaseReceivalDetail.Quantity, itemPrice);
                purchaseReceivalDetail.COGS = item.AvgPrice * purchaseReceivalDetail.Quantity;
                purchaseReceivalDetail = _repository.ConfirmObject(purchaseReceivalDetail);
                PurchaseOrderDetail purchaseOrderDetail = _purchaseOrderDetailService.GetObjectById(purchaseReceivalDetail.PurchaseOrderDetailId);
                _purchaseOrderDetailService.SetReceivalComplete(purchaseOrderDetail, purchaseReceivalDetail.Quantity);
            }
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail UnconfirmObject(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalService _purchaseReceivalService,
                                                      IPurchaseOrderService _purchaseOrderService, IPurchaseOrderDetailService _purchaseOrderDetailService,
                                                      IPurchaseInvoiceDetailService _purchaseInvoiceDetailService, IStockMutationService _stockMutationService,
                                                      IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidUnconfirmObject(purchaseReceivalDetail, _purchaseInvoiceDetailService, _itemService))
            {
                PurchaseReceival purchaseReceival = _purchaseReceivalService.GetObjectById(purchaseReceivalDetail.PurchaseReceivalId);
                WarehouseItem warehouseItem = _warehouseItemService.FindOrCreateObject(purchaseReceival.WarehouseId, purchaseReceivalDetail.ItemId);
                Item item = _itemService.GetObjectById(purchaseReceivalDetail.ItemId);
                IList<StockMutation> stockMutations = _stockMutationService.DeleteStockMutationForPurchaseReceival(purchaseReceivalDetail, warehouseItem);
                foreach (var stockMutation in stockMutations)
                {
                    //item.PendingReceival += purchaseReceivalDetail.Quantity;
                    //item.Quantity -= purchaseReceivalDetail.Quantity;
                    _stockMutationService.ReverseStockMutateObject(stockMutation, _itemService, _blanketService, _warehouseItemService);
                }
                decimal itemPrice = _purchaseOrderDetailService.GetObjectById(purchaseReceivalDetail.PurchaseOrderDetailId).Price * purchaseReceival.ExchangeRateAmount;
                item.AvgPrice = _itemService.CalculateAndUpdateAvgPrice(item, purchaseReceivalDetail.Quantity * (-1), itemPrice);
                purchaseReceivalDetail.COGS = 0;
                purchaseReceivalDetail = _repository.UnconfirmObject(purchaseReceivalDetail);
                PurchaseOrderDetail purchaseOrderDetail = _purchaseOrderDetailService.GetObjectById(purchaseReceivalDetail.PurchaseOrderDetailId);
                _purchaseOrderDetailService.UnsetReceivalComplete(purchaseOrderDetail, purchaseReceivalDetail.Quantity, _purchaseOrderService);
            }
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail InvoiceObject(PurchaseReceivalDetail purchaseReceivalDetail, decimal Quantity)
        {
            purchaseReceivalDetail.PendingInvoicedQuantity -= Quantity;
            if (purchaseReceivalDetail.PendingInvoicedQuantity == 0) { purchaseReceivalDetail.IsAllInvoiced = true; }
            _repository.UpdateObject(purchaseReceivalDetail);
            return purchaseReceivalDetail;
        }

        public PurchaseReceivalDetail UndoInvoiceObject(PurchaseReceivalDetail purchaseReceivalDetail, decimal Quantity, IPurchaseReceivalService _purchaseReceivalService)
        {
            PurchaseReceival purchaseReceival = _purchaseReceivalService.GetObjectById(purchaseReceivalDetail.PurchaseReceivalId);
            _purchaseReceivalService.UnsetInvoiceComplete(purchaseReceival);

            purchaseReceivalDetail.IsAllInvoiced = false;
            purchaseReceivalDetail.PendingInvoicedQuantity += Quantity;
            _repository.UpdateObject(purchaseReceivalDetail);
            return purchaseReceivalDetail;
        }
    }
}