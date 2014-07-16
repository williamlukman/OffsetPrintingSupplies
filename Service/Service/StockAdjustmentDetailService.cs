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

        public StockAdjustmentDetail ConfirmObject(StockAdjustmentDetail stockAdjustmentDetail, IStockAdjustmentService _stockAdjustmentService, IStockMutationService _stockMutationService,
                                                   IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidConfirmObject(stockAdjustmentDetail, _stockAdjustmentService, _itemService, _barringService, _warehouseItemService))
            {
                stockAdjustmentDetail = _repository.ConfirmObject(stockAdjustmentDetail);
            }
            return stockAdjustmentDetail;
        }

        public StockAdjustmentDetail UnconfirmObject(StockAdjustmentDetail stockAdjustmentDetail, IStockAdjustmentService _stockAdjustmentService, IStockMutationService _stockMutationService,
                                                     IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidUnconfirmObject(stockAdjustmentDetail, _stockAdjustmentService, _itemService, _barringService, _warehouseItemService))
            {
                stockAdjustmentDetail = _repository.UnconfirmObject(stockAdjustmentDetail);
            }
            return stockAdjustmentDetail;
        }
    }
}