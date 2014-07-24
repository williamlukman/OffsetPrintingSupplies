using Core.Constants;
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
    public class StockAdjustmentDetailService : IStockAdjustmentDetailService
    {
        private IStockAdjustmentDetailRepository _repository;
        private IStockAdjustmentDetailValidator _validator;

        public StockAdjustmentDetailService(IStockAdjustmentDetailRepository _stockAdjustmentDetailRepository, IStockAdjustmentDetailValidator _stockAdjustmentDetailValidator)
        {
            _repository = _stockAdjustmentDetailRepository;
            _validator = _stockAdjustmentDetailValidator;
        }

        public IStockAdjustmentDetailValidator GetValidator()
        {
            return _validator;
        }

        public IList<StockAdjustmentDetail> GetObjectsByStockAdjustmentId(int stockAdjustmentId)
        {
            return _repository.GetObjectsByStockAdjustmentId(stockAdjustmentId);
        }

        public StockAdjustmentDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public StockAdjustmentDetail CreateObject(StockAdjustmentDetail stockAdjustmentDetail, IStockAdjustmentService _stockAdjustmentService,
                                                  IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            stockAdjustmentDetail.Errors = new Dictionary<String, String>();
            return (stockAdjustmentDetail = _validator.ValidCreateObject(stockAdjustmentDetail, this, _stockAdjustmentService, _itemService, _warehouseItemService) ?
                                            _repository.CreateObject(stockAdjustmentDetail) : stockAdjustmentDetail);
        }

        public StockAdjustmentDetail CreateObject(int stockAdjustmentId, int itemId, int quantity,
                                                    IStockAdjustmentService _stockAdjustmentService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            StockAdjustmentDetail stockAdjustmentDetail = new StockAdjustmentDetail
            {
                StockAdjustmentId = stockAdjustmentId,
                ItemId = itemId,
                Quantity = quantity,
                // Price = price
            };
            return this.CreateObject(stockAdjustmentDetail, _stockAdjustmentService, _itemService, _warehouseItemService);
        }

        public StockAdjustmentDetail UpdateObject(StockAdjustmentDetail stockAdjustmentDetail, IStockAdjustmentService _stockAdjustmentService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            return (stockAdjustmentDetail = _validator.ValidUpdateObject(stockAdjustmentDetail, this, _stockAdjustmentService, _itemService, _warehouseItemService) ?
                                            _repository.UpdateObject(stockAdjustmentDetail) : stockAdjustmentDetail);
        }

        public StockAdjustmentDetail SoftDeleteObject(StockAdjustmentDetail stockAdjustmentDetail)
        {
            return (stockAdjustmentDetail = _validator.ValidDeleteObject(stockAdjustmentDetail) ? _repository.SoftDeleteObject(stockAdjustmentDetail) : stockAdjustmentDetail);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public StockAdjustmentDetail FinishObject(StockAdjustmentDetail stockAdjustmentDetail, IStockAdjustmentService _stockAdjustmentService, IStockMutationService _stockMutationService,
                                                   IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidFinishObject(stockAdjustmentDetail, _stockAdjustmentService, _itemService, _barringService, _warehouseItemService))
            {
                stockAdjustmentDetail = _repository.FinishObject(stockAdjustmentDetail);
                StockAdjustment stockAdjustment = _stockAdjustmentService.GetObjectById(stockAdjustmentDetail.StockAdjustmentId);
                Item item = _itemService.GetObjectById(stockAdjustmentDetail.ItemId);
                WarehouseItem warehouseItem = _warehouseItemService.FindOrCreateObject(stockAdjustment.WarehouseId, item.Id);
                StockMutation stockMutation = _stockMutationService.CreateStockMutationForStockAdjustment(stockAdjustmentDetail, warehouseItem);
                StockMutateObject(stockMutation, _itemService, _barringService, _warehouseItemService);
            }
            return stockAdjustmentDetail;
        }
        public StockAdjustmentDetail UnfinishObject(StockAdjustmentDetail stockAdjustmentDetail, IStockAdjustmentService _stockAdjustmentService, IStockMutationService _stockMutationService,
                                                     IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidUnfinishObject(stockAdjustmentDetail, _stockAdjustmentService, _itemService, _barringService, _warehouseItemService))
            {
                stockAdjustmentDetail = _repository.UnfinishObject(stockAdjustmentDetail);
                StockAdjustment stockAdjustment = _stockAdjustmentService.GetObjectById(stockAdjustmentDetail.StockAdjustmentId);
                Item item = _itemService.GetObjectById(stockAdjustmentDetail.ItemId);
                WarehouseItem warehouseItem = _warehouseItemService.FindOrCreateObject(stockAdjustment.WarehouseId, item.Id);
                IList<StockMutation> stockMutations = _stockMutationService.SoftDeleteStockMutationForStockAdjustment(stockAdjustmentDetail, warehouseItem);
                foreach (var stockMutation in stockMutations)
                {
                    ReverseStockMutateObject(stockMutation, _itemService, _barringService, _warehouseItemService);
                }
            }
            return stockAdjustmentDetail;
        }

        public void StockMutateObject(StockMutation stockMutation, IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            int Quantity = (stockMutation.Status == Constant.StockMutationStatus.Addition) ? stockMutation.Quantity : ((-1) * stockMutation.Quantity);
            // decimal stockAdjustmentDetailPrice = (stockMutation.Status == Constant.StockMutationStatus.Addition) ? stockAdjustmentDetail.Price : ((-1) * stockAdjustmentDetail.Price);
            WarehouseItem warehouseItem = _warehouseItemService.GetObjectById(stockMutation.WarehouseItemId);
            Item item = _itemService.GetObjectById(warehouseItem.ItemId);
            Barring barring = _barringService.GetObjectById(warehouseItem.ItemId);
            if (barring == null)
            {
                _itemService.AdjustQuantity(item, Quantity);
                // item.AvgCost = _barringService.CalculateAvgCost(item, stockAdjustmentDetail.Quantity, stockAdjustmentDetailPrice);
            }
            else
            {
                _barringService.AdjustQuantity(barring, Quantity);
                // barring.AvgCost = _barringService.CalculateAvgCost(barring, stockAdjustmentDetail.Quantity, stockAdjustmentDetailPrice);
            }
            _warehouseItemService.AdjustQuantity(warehouseItem, Quantity);
        }

        public void ReverseStockMutateObject(StockMutation stockMutation, IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            int Quantity = (stockMutation.Status == Constant.StockMutationStatus.Deduction) ? stockMutation.Quantity : ((-1) * stockMutation.Quantity);
            // decimal stockAdjustmentDetailPrice = (stockMutation.Status == Constant.StockMutationStatus.Addition) ? stockAdjustmentDetail.Price : ((-1) * stockAdjustmentDetail.Price);
            WarehouseItem warehouseItem = _warehouseItemService.GetObjectById(stockMutation.WarehouseItemId);
            Item item = _itemService.GetObjectById(warehouseItem.ItemId);
            Barring barring = _barringService.GetObjectById(warehouseItem.ItemId);
            if (barring == null)
            {
                _itemService.AdjustQuantity(item, Quantity);
                // item.AvgCost = _barringService.CalculateAvgCost(item, stockAdjustmentDetail.Quantity, stockAdjustmentDetailPrice);
            }
            else
            {
                _barringService.AdjustQuantity(barring, Quantity);
                // barring.AvgCost = _barringService.CalculateAvgCost(barring, stockAdjustmentDetail.Quantity, stockAdjustmentDetailPrice);
            }
            _warehouseItemService.AdjustQuantity(warehouseItem, Quantity);
        }
    }
}