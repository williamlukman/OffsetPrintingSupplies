using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class StockAdjustmentService : IStockAdjustmentService
    {
        private IStockAdjustmentRepository _repository;
        private IStockAdjustmentValidator _validator;

        public StockAdjustmentService(IStockAdjustmentRepository _stockAdjustmentRepository, IStockAdjustmentValidator _stockAdjustmentValidator)
        {
            _repository = _stockAdjustmentRepository;
            _validator = _stockAdjustmentValidator;
        }

        public IStockAdjustmentValidator GetValidator()
        {
            return _validator;
        }

        public IList<StockAdjustment> GetAll()
        {
            return _repository.GetAll();
        }

        public StockAdjustment GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }
        
        public StockAdjustment CreateObject(StockAdjustment stockAdjustment, IWarehouseService _warehouseService)
        {
            stockAdjustment.Errors = new Dictionary<String, String>();
            return (stockAdjustment = _validator.ValidCreateObject(stockAdjustment, _warehouseService) ? _repository.CreateObject(stockAdjustment) : stockAdjustment);
        }

        public StockAdjustment CreateObject(int WarehouseId, DateTime adjustmentDate, IWarehouseService _warehouseService)
        {
            StockAdjustment stockAdjustment = new StockAdjustment
            {
                WarehouseId = WarehouseId,
                AdjustmentDate = adjustmentDate
            };
            return this.CreateObject(stockAdjustment, _warehouseService);
        }

        public StockAdjustment UpdateObject(StockAdjustment stockAdjustment, IWarehouseService _warehouseService)
        {
            return (stockAdjustment = _validator.ValidUpdateObject(stockAdjustment, _warehouseService) ? _repository.UpdateObject(stockAdjustment) : stockAdjustment);
        }

        public StockAdjustment SoftDeleteObject(StockAdjustment stockAdjustment, IStockAdjustmentDetailService _stockAdjustmentDetailService)
        {
            return (stockAdjustment = _validator.ValidDeleteObject(stockAdjustment) ? _repository.SoftDeleteObject(stockAdjustment) : stockAdjustment);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public void AdjustStock(StockAdjustment stockAdjustment, StockAdjustmentDetail detail, IStockAdjustmentDetailService _stockAdjustmentDetailService,
                                   IStockMutationService _stockMutationService, IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService, bool ConfirmCase)
        {
            int stockAdjustmentDetailQuantity = ConfirmCase ? detail.Quantity : ((-1) * detail.Quantity);
            //decimal stockAdjustmentDetailPrice = ConfirmCase ? stockAdjustmentDetail.Price : ((-1) * stockAdjustmentDetail.Price);
            Item item = _itemService.GetObjectById(detail.ItemId);
            WarehouseItem warehouseItem = _warehouseItemService.GetObjectByWarehouseAndItem(stockAdjustment.WarehouseId, item.Id);
            if (item.GetType() == typeof(Barring))
            {
                Barring barring = _barringService.GetObjectById(item.Id);
                // barring.AvgCost = _barringService.CalculateAvgCost(barring, stockAdjustmentDetail.Quantity, stockAdjustmentDetailPrice);
                _barringService.AdjustQuantity(barring, stockAdjustmentDetailQuantity);
            }
            else
            {
                // item.AvgCost = _barringService.CalculateAvgCost(item, stockAdjustmentDetail.Quantity, stockAdjustmentDetailPrice);
                _itemService.AdjustQuantity(item, stockAdjustmentDetailQuantity);
            }
            _warehouseItemService.AdjustQuantity(warehouseItem, stockAdjustmentDetailQuantity);
            if (ConfirmCase)
            {
                StockMutation stockMutation = _stockMutationService.CreateStockMutationForStockAdjustment(detail, warehouseItem);
            }
            else
            {
                IList<StockMutation> stockMutations = _stockMutationService.SoftDeleteStockMutationForStockAdjustment(detail, warehouseItem);
            }
        }

        public StockAdjustment ConfirmObject(StockAdjustment stockAdjustment, IStockAdjustmentDetailService _stockAdjustmentDetailService,
                                             IStockMutationService _stockMutationService, IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidConfirmObject(stockAdjustment, this, _stockAdjustmentDetailService, _itemService, _barringService, _warehouseItemService))
            {
                IList<StockAdjustmentDetail> details = _stockAdjustmentDetailService.GetObjectsByStockAdjustmentId(stockAdjustment.Id);
                foreach (var detail in details)
                {
                    if (!_stockAdjustmentDetailService.GetValidator().ValidConfirmObject(detail, this, _itemService, _barringService, _warehouseItemService))
                    {
                        stockAdjustment.Errors.Add("Generic", "Tidak dapat mengkonfirmasi stock adjustment");
                        return stockAdjustment;
                    }
                }

                _repository.ConfirmObject(stockAdjustment);
                bool ConfirmCase = true;
                foreach (var detail in details)
                {
                    detail.ConfirmedAt = stockAdjustment.ConfirmedAt;
                    _stockAdjustmentDetailService.ConfirmObject(detail, this, _stockMutationService, _itemService, _barringService, _warehouseItemService);
                    AdjustStock(stockAdjustment, detail, _stockAdjustmentDetailService, _stockMutationService, _itemService, _barringService, _warehouseItemService, ConfirmCase);
                }
            }
            return stockAdjustment;
        }

        public StockAdjustment UnconfirmObject(StockAdjustment stockAdjustment, IStockAdjustmentDetailService _stockAdjustmentDetailService,
                                               IStockMutationService _stockMutationService, IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidUnconfirmObject(stockAdjustment, this, _stockAdjustmentDetailService, _itemService, _barringService, _warehouseItemService))
            {
                IList<StockAdjustmentDetail> details = _stockAdjustmentDetailService.GetObjectsByStockAdjustmentId(stockAdjustment.Id);
                foreach (var detail in details)
                {
                    if (!_stockAdjustmentDetailService.GetValidator().ValidUnconfirmObject(detail, this, _itemService, _barringService, _warehouseItemService))
                    {
                        stockAdjustment.Errors.Add("Generic", "Tidak dapat meng unkonfirmasi stock adjustment");
                        return stockAdjustment;
                    }
                }

                _repository.UnconfirmObject(stockAdjustment);
                bool ConfirmCase = false;
                foreach (var detail in details)
                {
                    detail.ConfirmedAt = stockAdjustment.ConfirmedAt;
                    _stockAdjustmentDetailService.ConfirmObject(detail, this, _stockMutationService, _itemService, _barringService, _warehouseItemService);
                    AdjustStock(stockAdjustment, detail, _stockAdjustmentDetailService, _stockMutationService, _itemService, _barringService, _warehouseItemService, ConfirmCase);
                }
            }
            return stockAdjustment;
        }
    }
}