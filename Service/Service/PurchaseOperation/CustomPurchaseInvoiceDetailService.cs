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
    public class CustomPurchaseInvoiceDetailService : ICustomPurchaseInvoiceDetailService
    {
        private ICustomPurchaseInvoiceDetailRepository _repository;
        private ICustomPurchaseInvoiceDetailValidator _validator;
        public CustomPurchaseInvoiceDetailService(ICustomPurchaseInvoiceDetailRepository _customPurchaseInvoiceDetailRepository, ICustomPurchaseInvoiceDetailValidator _customPurchaseInvoiceDetailValidator)
        {
            _repository = _customPurchaseInvoiceDetailRepository;
            _validator = _customPurchaseInvoiceDetailValidator;
        }

        public ICustomPurchaseInvoiceDetailValidator GetValidator()
        {
            return _validator;
        }

        public IList<CustomPurchaseInvoiceDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<CustomPurchaseInvoiceDetail> GetObjectsByCustomPurchaseInvoiceId(int CustomPurchaseInvoiceId)
        {
            return _repository.GetObjectsByCustomPurchaseInvoiceId(CustomPurchaseInvoiceId);
        }

        public CustomPurchaseInvoiceDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public CustomPurchaseInvoiceDetail CreateObject(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail, ICustomPurchaseInvoiceService _customPurchaseInvoiceService, 
                                                     IItemService _itemService, IWarehouseItemService _warehouseItemService, IPriceMutationService _priceMutationService)
        {
            customPurchaseInvoiceDetail.Errors = new Dictionary<String, String>();
            if(_validator.ValidCreateObject(customPurchaseInvoiceDetail, _customPurchaseInvoiceService, this, _itemService, _warehouseItemService))
            {
                Item item = _itemService.GetObjectById(customPurchaseInvoiceDetail.ItemId);
                PriceMutation priceMutation = _priceMutationService.GetObjectById(item.PriceMutationId);
                CustomPurchaseInvoice customPurchaseInvoice = _customPurchaseInvoiceService.GetObjectById(customPurchaseInvoiceDetail.CustomPurchaseInvoiceId);
                customPurchaseInvoiceDetail.PriceMutationId = item.PriceMutationId;
                customPurchaseInvoiceDetail.Price = customPurchaseInvoiceDetail.ListedUnitPrice * (100 - customPurchaseInvoiceDetail.Discount) / 100;
                customPurchaseInvoiceDetail.Amount = priceMutation.Amount * customPurchaseInvoiceDetail.Quantity;
                customPurchaseInvoiceDetail = _repository.CreateObject(customPurchaseInvoiceDetail);
                customPurchaseInvoice.Total = CalculateTotal(customPurchaseInvoiceDetail.CustomPurchaseInvoiceId);
                _customPurchaseInvoiceService.GetRepository().UpdateObject(customPurchaseInvoice);
            }
            return customPurchaseInvoiceDetail;
        }

        public CustomPurchaseInvoiceDetail UpdateObject(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail, ICustomPurchaseInvoiceService _customPurchaseInvoiceService,
                                                     IItemService _itemService, IWarehouseItemService _warehouseItemService, IPriceMutationService _priceMutationService)
        {
            if (_validator.ValidUpdateObject(customPurchaseInvoiceDetail, _customPurchaseInvoiceService, this, _itemService, _warehouseItemService))
            {
                Item item = _itemService.GetObjectById(customPurchaseInvoiceDetail.ItemId);
                PriceMutation priceMutation = _priceMutationService.GetObjectById(item.PriceMutationId);
                CustomPurchaseInvoice customPurchaseInvoice = _customPurchaseInvoiceService.GetObjectById(customPurchaseInvoiceDetail.CustomPurchaseInvoiceId);
                customPurchaseInvoiceDetail.PriceMutationId = item.PriceMutationId;
                customPurchaseInvoiceDetail.Price = customPurchaseInvoiceDetail.ListedUnitPrice * (100 - customPurchaseInvoiceDetail.Discount) / 100;
                customPurchaseInvoiceDetail.Amount = priceMutation.Amount * customPurchaseInvoiceDetail.Quantity;
                customPurchaseInvoiceDetail = _repository.UpdateObject(customPurchaseInvoiceDetail);
                customPurchaseInvoice.Total = CalculateTotal(customPurchaseInvoiceDetail.CustomPurchaseInvoiceId);
                _customPurchaseInvoiceService.GetRepository().UpdateObject(customPurchaseInvoice);
            }
            return customPurchaseInvoiceDetail;
        }

        public CustomPurchaseInvoiceDetail ConfirmObject(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail, ICustomPurchaseInvoiceService _customPurchaseInvoiceService, IWarehouseItemService _warehouseItemService, 
                                                      IWarehouseService _warehouseService, IItemService _itemService, IBarringService _barringService, IStockMutationService _stockMutationService)
        {
            if(_validator.ValidConfirmObject(customPurchaseInvoiceDetail, _customPurchaseInvoiceService, _warehouseItemService))
            {
                CustomPurchaseInvoice customPurchaseInvoice = _customPurchaseInvoiceService.GetObjectById(customPurchaseInvoiceDetail.CustomPurchaseInvoiceId);
                WarehouseItem warehouseItem = _warehouseItemService.FindOrCreateObject(customPurchaseInvoice.WarehouseId, customPurchaseInvoiceDetail.ItemId);
                Item item = _itemService.GetObjectById(customPurchaseInvoiceDetail.ItemId);
                StockMutation stockMutation = new StockMutation()
                {
                    ItemId = customPurchaseInvoiceDetail.ItemId,
                    ItemCase = Core.Constants.Constant.ItemCase.Ready,
                    Status = Core.Constants.Constant.MutationStatus.Addition,
                    Quantity = customPurchaseInvoiceDetail.Quantity,
                    SourceDocumentId = customPurchaseInvoice.Id,
                    SourceDocumentType = Core.Constants.Constant.SourceDocumentType.CustomPurchaseInvoice,
                    SourceDocumentDetailId = customPurchaseInvoiceDetail.Id,
                    SourceDocumentDetailType = Core.Constants.Constant.SourceDocumentDetailType.CustomPurchaseInvoiceDetail,
                    WarehouseId = customPurchaseInvoice.WarehouseId,
                    WarehouseItemId = warehouseItem.Id
                };

                item.SellingPrice = customPurchaseInvoiceDetail.Price * (100 + item.Margin) / 100;
                decimal itemPrice = customPurchaseInvoiceDetail.Amount / customPurchaseInvoiceDetail.Quantity;
                item.AvgPrice = _itemService.CalculateAndUpdateAvgPrice(item, customPurchaseInvoiceDetail.Quantity, itemPrice);

                stockMutation = _stockMutationService.CreateObject(stockMutation, _warehouseService, _warehouseItemService, _itemService, _barringService);
                stockMutation.CreatedAt = (DateTime)customPurchaseInvoice.ConfirmationDate;
                _stockMutationService.UpdateObject(stockMutation, _warehouseService, _warehouseItemService, _itemService, _barringService);
                _stockMutationService.StockMutateObject(stockMutation, _itemService, _barringService, _warehouseItemService);
                customPurchaseInvoiceDetail.CoGS = customPurchaseInvoiceDetail.Quantity * item.AvgPrice;
                customPurchaseInvoiceDetail = _repository.ConfirmObject(customPurchaseInvoiceDetail);
            }
            return customPurchaseInvoiceDetail;
        }

        public CustomPurchaseInvoiceDetail UnconfirmObject(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail, IWarehouseItemService _warehouseItemService,
                                                      IWarehouseService _warehouseService, IItemService _itemService, IBarringService _barringService, IStockMutationService _stockMutationService)
        {
            if (_validator.ValidUnconfirmObject(customPurchaseInvoiceDetail))
            {
                Item item = _itemService.GetObjectById(customPurchaseInvoiceDetail.ItemId);
                decimal itemPrice = customPurchaseInvoiceDetail.Amount / customPurchaseInvoiceDetail.Quantity;
                // How to Reverse SellingPrice ?
                item.SellingPrice = itemPrice;
                item.AvgPrice = _itemService.CalculateAndUpdateAvgPrice(item, customPurchaseInvoiceDetail.Quantity * (-1), itemPrice);

                IList<StockMutation> stockMutations = _stockMutationService.GetObjectsBySourceDocumentDetailForItem(customPurchaseInvoiceDetail.ItemId, Core.Constants.Constant.SourceDocumentDetailType.CustomPurchaseInvoiceDetail, customPurchaseInvoiceDetail.Id);
                foreach (var stockMutation in stockMutations)
                {
                    stockMutation.Errors = new Dictionary<string, string>();
                    _stockMutationService.ReverseStockMutateObject(stockMutation, _itemService, _barringService, _warehouseItemService);
                    _stockMutationService.SoftDeleteObject(stockMutation, _warehouseService, _warehouseItemService, _itemService, _barringService);
                }
                customPurchaseInvoiceDetail.CoGS = 0;
                customPurchaseInvoiceDetail = _repository.UnconfirmObject(customPurchaseInvoiceDetail);
            }
            return customPurchaseInvoiceDetail;
        }

        public CustomPurchaseInvoiceDetail SoftDeleteObject(CustomPurchaseInvoiceDetail customPurchaseInvoiceDetail, ICustomPurchaseInvoiceService _customPurchaseInvoiceService)
        {
            return (customPurchaseInvoiceDetail = _validator.ValidDeleteObject(customPurchaseInvoiceDetail, _customPurchaseInvoiceService) ?
                    _repository.SoftDeleteObject(customPurchaseInvoiceDetail) : customPurchaseInvoiceDetail);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public decimal CalculateTotal(int CustomPurchaseInvoiceId)
        {
            IList<CustomPurchaseInvoiceDetail> customPurchaseInvoiceDetails = GetObjectsByCustomPurchaseInvoiceId(CustomPurchaseInvoiceId);
            decimal Total = 0;
            foreach (var customPurchaseInvoiceDetail in customPurchaseInvoiceDetails)
            {
                Total += customPurchaseInvoiceDetail.Amount;
            }
            return Total;
        }
    }
}
