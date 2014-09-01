using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Constants;
using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;

namespace Service.Service
{
    public class RetailPurchaseInvoiceDetailService : IRetailPurchaseInvoiceDetailService
    {
        private IRetailPurchaseInvoiceDetailRepository _repository;
        private IRetailPurchaseInvoiceDetailValidator _validator;
        public RetailPurchaseInvoiceDetailService(IRetailPurchaseInvoiceDetailRepository _retailPurchaseInvoiceDetailRepository, IRetailPurchaseInvoiceDetailValidator _retailPurchaseInvoiceDetailValidator)
        {
            _repository = _retailPurchaseInvoiceDetailRepository;
            _validator = _retailPurchaseInvoiceDetailValidator;
        }

        public IRetailPurchaseInvoiceDetailValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<RetailPurchaseInvoiceDetail> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<RetailPurchaseInvoiceDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<RetailPurchaseInvoiceDetail> GetObjectsByRetailPurchaseInvoiceId(int RetailPurchaseInvoiceId)
        {
            return _repository.GetObjectsByRetailPurchaseInvoiceId(RetailPurchaseInvoiceId);
        }

        public RetailPurchaseInvoiceDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public RetailPurchaseInvoiceDetail CreateObject(RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail, IRetailPurchaseInvoiceService _retailPurchaseInvoiceService, 
                                                     IItemService _itemService, IWarehouseItemService _warehouseItemService, IPriceMutationService _priceMutationService)
        {
            retailPurchaseInvoiceDetail.Errors = new Dictionary<String, String>();
            if(_validator.ValidCreateObject(retailPurchaseInvoiceDetail, _retailPurchaseInvoiceService, this, _itemService, _warehouseItemService))
            {
                Item item = _itemService.GetObjectById(retailPurchaseInvoiceDetail.ItemId);
                PriceMutation priceMutation = _priceMutationService.GetObjectById(item.PriceMutationId);
                RetailPurchaseInvoice retailPurchaseInvoice = _retailPurchaseInvoiceService.GetObjectById(retailPurchaseInvoiceDetail.RetailPurchaseInvoiceId);
                retailPurchaseInvoiceDetail.PriceMutationId = item.PriceMutationId;
                retailPurchaseInvoiceDetail.Amount = priceMutation.Amount * retailPurchaseInvoiceDetail.Quantity;
                retailPurchaseInvoiceDetail = _repository.CreateObject(retailPurchaseInvoiceDetail);
                retailPurchaseInvoice.Total += retailPurchaseInvoiceDetail.Amount;
                _retailPurchaseInvoiceService.GetRepository().UpdateObject(retailPurchaseInvoice);
            }
            return retailPurchaseInvoiceDetail;
        }

        public RetailPurchaseInvoiceDetail UpdateObject(RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail, IRetailPurchaseInvoiceService _retailPurchaseInvoiceService,
                                                     IItemService _itemService, IWarehouseItemService _warehouseItemService, IPriceMutationService _priceMutationService)
        {
            if (_validator.ValidUpdateObject(retailPurchaseInvoiceDetail, _retailPurchaseInvoiceService, this, _itemService, _warehouseItemService))
            {
                Item item = _itemService.GetObjectById(retailPurchaseInvoiceDetail.ItemId);
                PriceMutation priceMutation = _priceMutationService.GetObjectById(item.PriceMutationId);
                RetailPurchaseInvoice retailPurchaseInvoice = _retailPurchaseInvoiceService.GetObjectById(retailPurchaseInvoiceDetail.RetailPurchaseInvoiceId);
                retailPurchaseInvoiceDetail.PriceMutationId = item.PriceMutationId;
                retailPurchaseInvoiceDetail.Amount = priceMutation.Amount * retailPurchaseInvoiceDetail.Quantity;
                retailPurchaseInvoiceDetail = _repository.UpdateObject(retailPurchaseInvoiceDetail);
                retailPurchaseInvoice.Total += retailPurchaseInvoiceDetail.Amount;
                _retailPurchaseInvoiceService.GetRepository().UpdateObject(retailPurchaseInvoice);
            }
            return retailPurchaseInvoiceDetail;
        }

        public RetailPurchaseInvoiceDetail ConfirmObject(RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail, IRetailPurchaseInvoiceService _retailPurchaseInvoiceService, IWarehouseItemService _warehouseItemService, 
                                                      IWarehouseService _warehouseService, IItemService _itemService, IBlanketService _blanketService, IStockMutationService _stockMutationService)
        {
            if(_validator.ValidConfirmObject(retailPurchaseInvoiceDetail, _retailPurchaseInvoiceService, _warehouseItemService))
            {
                RetailPurchaseInvoice retailPurchaseInvoice = _retailPurchaseInvoiceService.GetObjectById(retailPurchaseInvoiceDetail.RetailPurchaseInvoiceId);
                WarehouseItem warehouseItem = _warehouseItemService.FindOrCreateObject(retailPurchaseInvoice.WarehouseId, retailPurchaseInvoiceDetail.ItemId);
                Item item = _itemService.GetObjectById(retailPurchaseInvoiceDetail.ItemId);
                StockMutation stockMutation = new StockMutation()
                {
                    ItemId = retailPurchaseInvoiceDetail.ItemId,
                    ItemCase = Core.Constants.Constant.ItemCase.Ready,
                    Status = Core.Constants.Constant.MutationStatus.Deduction,
                    Quantity = retailPurchaseInvoiceDetail.Quantity,
                    SourceDocumentId = retailPurchaseInvoice.Id,
                    SourceDocumentType = Core.Constants.Constant.SourceDocumentType.RetailPurchaseInvoice,
                    SourceDocumentDetailId = retailPurchaseInvoiceDetail.Id,
                    SourceDocumentDetailType = Core.Constants.Constant.SourceDocumentDetailType.RetailPurchaseInvoiceDetail,
                    WarehouseId = retailPurchaseInvoice.WarehouseId,
                    WarehouseItemId = warehouseItem.Id
                };
                stockMutation = _stockMutationService.CreateObject(stockMutation, _warehouseService, _warehouseItemService, _itemService, _blanketService);
                stockMutation.CreatedAt = (DateTime)retailPurchaseInvoice.ConfirmationDate;
                _stockMutationService.UpdateObject(stockMutation, _warehouseService, _warehouseItemService, _itemService, _blanketService);
                _stockMutationService.StockMutateObject(stockMutation, _itemService, _blanketService, _warehouseItemService);
                retailPurchaseInvoiceDetail.CoGS = retailPurchaseInvoiceDetail.Quantity * item.AvgPrice;
                retailPurchaseInvoiceDetail = _repository.ConfirmObject(retailPurchaseInvoiceDetail);
            }
            return retailPurchaseInvoiceDetail;
        }

        public RetailPurchaseInvoiceDetail UnconfirmObject(RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail, IWarehouseItemService _warehouseItemService,
                                                      IWarehouseService _warehouseService, IItemService _itemService, IBlanketService _blanketService, IStockMutationService _stockMutationService)
        {
            if (_validator.ValidUnconfirmObject(retailPurchaseInvoiceDetail))
            {
                IList<StockMutation> stockMutations = _stockMutationService.GetObjectsBySourceDocumentDetailForItem(retailPurchaseInvoiceDetail.ItemId, Core.Constants.Constant.SourceDocumentDetailType.RetailPurchaseInvoiceDetail, retailPurchaseInvoiceDetail.Id);
                foreach (var stockMutation in stockMutations)
                {
                    stockMutation.Errors = new Dictionary<string, string>();
                    _stockMutationService.ReverseStockMutateObject(stockMutation, _itemService, _blanketService, _warehouseItemService);
                    _stockMutationService.SoftDeleteObject(stockMutation, _warehouseService, _warehouseItemService, _itemService, _blanketService);
                }
                retailPurchaseInvoiceDetail.CoGS = 0;
                retailPurchaseInvoiceDetail = _repository.UnconfirmObject(retailPurchaseInvoiceDetail);
            }
            return retailPurchaseInvoiceDetail;
        }

        public RetailPurchaseInvoiceDetail SoftDeleteObject(RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail, IRetailPurchaseInvoiceService _retailPurchaseInvoiceService)
        {
            return (retailPurchaseInvoiceDetail = _validator.ValidDeleteObject(retailPurchaseInvoiceDetail, _retailPurchaseInvoiceService) ?
                    _repository.SoftDeleteObject(retailPurchaseInvoiceDetail) : retailPurchaseInvoiceDetail);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }
    }
}
