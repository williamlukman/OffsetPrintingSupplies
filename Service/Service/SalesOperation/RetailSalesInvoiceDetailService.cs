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
    public class RetailSalesInvoiceDetailService : IRetailSalesInvoiceDetailService
    {
        private IRetailSalesInvoiceDetailRepository _repository;
        private IRetailSalesInvoiceDetailValidator _validator;
        public RetailSalesInvoiceDetailService(IRetailSalesInvoiceDetailRepository _retailSalesInvoiceDetailRepository, IRetailSalesInvoiceDetailValidator _retailSalesInvoiceDetailValidator)
        {
            _repository = _retailSalesInvoiceDetailRepository;
            _validator = _retailSalesInvoiceDetailValidator;
        }

        public IRetailSalesInvoiceDetailValidator GetValidator()
        {
            return _validator;
        }

        public IList<RetailSalesInvoiceDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<RetailSalesInvoiceDetail> GetObjectsByRetailSalesInvoiceId(int RetailSalesInvoiceId)
        {
            return _repository.GetObjectsByRetailSalesInvoiceId(RetailSalesInvoiceId);
        }

        public RetailSalesInvoiceDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public RetailSalesInvoiceDetail CreateObject(RetailSalesInvoiceDetail retailSalesInvoiceDetail, IRetailSalesInvoiceService _retailSalesInvoiceService, 
                                                     IItemService _itemService, IWarehouseItemService _warehouseItemService, IPriceMutationService _priceMutationService)
        {
            retailSalesInvoiceDetail.Errors = new Dictionary<String, String>();
            if(_validator.ValidCreateObject(retailSalesInvoiceDetail, _retailSalesInvoiceService, this, _itemService, _warehouseItemService))
            {
                Item item = _itemService.GetObjectById(retailSalesInvoiceDetail.ItemId);
                PriceMutation priceMutation = _priceMutationService.GetObjectById(item.PriceMutationId);
                RetailSalesInvoice retailSalesInvoice = _retailSalesInvoiceService.GetObjectById(retailSalesInvoiceDetail.RetailSalesInvoiceId);
                retailSalesInvoiceDetail.PriceMutationId = item.PriceMutationId;
                retailSalesInvoiceDetail.Amount = priceMutation.Amount * retailSalesInvoiceDetail.Quantity;
                retailSalesInvoiceDetail = _repository.CreateObject(retailSalesInvoiceDetail);
                retailSalesInvoice.Total += retailSalesInvoiceDetail.Amount;
                _retailSalesInvoiceService.GetRepository().UpdateObject(retailSalesInvoice);
            }
            return retailSalesInvoiceDetail;
        }

        public RetailSalesInvoiceDetail UpdateObject(RetailSalesInvoiceDetail retailSalesInvoiceDetail, IRetailSalesInvoiceService _retailSalesInvoiceService,
                                                     IItemService _itemService, IWarehouseItemService _warehouseItemService, IPriceMutationService _priceMutationService)
        {
            if (_validator.ValidUpdateObject(retailSalesInvoiceDetail, _retailSalesInvoiceService, this, _itemService, _warehouseItemService))
            {
                Item item = _itemService.GetObjectById(retailSalesInvoiceDetail.ItemId);
                PriceMutation priceMutation = _priceMutationService.GetObjectById(item.PriceMutationId);
                RetailSalesInvoice retailSalesInvoice = _retailSalesInvoiceService.GetObjectById(retailSalesInvoiceDetail.RetailSalesInvoiceId);
                retailSalesInvoiceDetail.PriceMutationId = item.PriceMutationId;
                retailSalesInvoiceDetail.Amount = priceMutation.Amount * retailSalesInvoiceDetail.Quantity;
                retailSalesInvoiceDetail = _repository.UpdateObject(retailSalesInvoiceDetail);
                retailSalesInvoice.Total += retailSalesInvoiceDetail.Amount;
                _retailSalesInvoiceService.GetRepository().UpdateObject(retailSalesInvoice);
            }
            return retailSalesInvoiceDetail;
        }

        public RetailSalesInvoiceDetail ConfirmObject(RetailSalesInvoiceDetail retailSalesInvoiceDetail, IRetailSalesInvoiceService _retailSalesInvoiceService, IWarehouseItemService _warehouseItemService, 
                                                      IWarehouseService _warehouseService, IItemService _itemService, IBarringService _barringService, IStockMutationService _stockMutationService)
        {
            if(_validator.ValidConfirmObject(retailSalesInvoiceDetail, _retailSalesInvoiceService, _warehouseItemService))
            {
                RetailSalesInvoice retailSalesInvoice = _retailSalesInvoiceService.GetObjectById(retailSalesInvoiceDetail.RetailSalesInvoiceId);
                WarehouseItem warehouseItem = _warehouseItemService.FindOrCreateObject(retailSalesInvoice.WarehouseId, retailSalesInvoiceDetail.ItemId);
                Item item = _itemService.GetObjectById(retailSalesInvoiceDetail.ItemId);
                StockMutation stockMutation = new StockMutation()
                {
                    ItemId = retailSalesInvoiceDetail.ItemId,
                    ItemCase = Core.Constants.Constant.ItemCase.Ready,
                    Status = Core.Constants.Constant.MutationStatus.Deduction,
                    Quantity = retailSalesInvoiceDetail.Quantity,
                    SourceDocumentId = retailSalesInvoice.Id,
                    SourceDocumentType = Core.Constants.Constant.SourceDocumentType.RetailSalesInvoice,
                    SourceDocumentDetailId = retailSalesInvoiceDetail.Id,
                    SourceDocumentDetailType = Core.Constants.Constant.SourceDocumentDetailType.RetailSalesInvoiceDetail,
                    WarehouseId = retailSalesInvoice.WarehouseId,
                    WarehouseItemId = warehouseItem.Id
                };
                stockMutation = _stockMutationService.CreateObject(stockMutation, _warehouseService, _warehouseItemService, _itemService, _barringService);
                stockMutation.CreatedAt = (DateTime)retailSalesInvoice.ConfirmationDate;
                _stockMutationService.UpdateObject(stockMutation, _warehouseService, _warehouseItemService, _itemService, _barringService);
                _stockMutationService.StockMutateObject(stockMutation, _itemService, _barringService, _warehouseItemService);
                retailSalesInvoiceDetail.CoGS = retailSalesInvoiceDetail.Quantity * item.AvgPrice;
                retailSalesInvoiceDetail = _repository.ConfirmObject(retailSalesInvoiceDetail);
            }
            return retailSalesInvoiceDetail;
        }

        public RetailSalesInvoiceDetail UnconfirmObject(RetailSalesInvoiceDetail retailSalesInvoiceDetail, IWarehouseItemService _warehouseItemService,
                                                      IWarehouseService _warehouseService, IItemService _itemService, IBarringService _barringService, IStockMutationService _stockMutationService)
        {
            if (_validator.ValidUnconfirmObject(retailSalesInvoiceDetail))
            {
                IList<StockMutation> stockMutations = _stockMutationService.GetObjectsBySourceDocumentDetailForItem(retailSalesInvoiceDetail.ItemId, Core.Constants.Constant.SourceDocumentDetailType.RetailSalesInvoiceDetail, retailSalesInvoiceDetail.Id);
                foreach (var stockMutation in stockMutations)
                {
                    stockMutation.Errors = new Dictionary<string, string>();
                    _stockMutationService.ReverseStockMutateObject(stockMutation, _itemService, _barringService, _warehouseItemService);
                    _stockMutationService.SoftDeleteObject(stockMutation, _warehouseService, _warehouseItemService, _itemService, _barringService);
                }
                retailSalesInvoiceDetail.CoGS = 0;
                retailSalesInvoiceDetail = _repository.UnconfirmObject(retailSalesInvoiceDetail);
            }
            return retailSalesInvoiceDetail;
        }

        public RetailSalesInvoiceDetail SoftDeleteObject(RetailSalesInvoiceDetail retailSalesInvoiceDetail, IRetailSalesInvoiceService _retailSalesInvoiceService)
        {
            return (retailSalesInvoiceDetail = _validator.ValidDeleteObject(retailSalesInvoiceDetail, _retailSalesInvoiceService) ?
                    _repository.SoftDeleteObject(retailSalesInvoiceDetail) : retailSalesInvoiceDetail);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }
    }
}
