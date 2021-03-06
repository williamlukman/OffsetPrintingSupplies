using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public IQueryable<StockAdjustment> GetQueryable()
        {
            return _repository.GetQueryable();
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

        public StockAdjustment ConfirmObject(StockAdjustment stockAdjustment, DateTime ConfirmationDate, IStockAdjustmentDetailService _stockAdjustmentDetailService,
                                             IStockMutationService _stockMutationService, IItemService _itemService, IItemTypeService _itemTypeService,
                                             IBlanketService _blanketService, IWarehouseItemService _warehouseItemService,
                                             IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            stockAdjustment.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(stockAdjustment, this, _stockAdjustmentDetailService, _itemService, _blanketService, _warehouseItemService, _closingService))
            {
                decimal Total = 0;
                IList<StockAdjustmentDetail> stockAdjustmentDetails = _stockAdjustmentDetailService.GetObjectsByStockAdjustmentId(stockAdjustment.Id);
                foreach (var detail in stockAdjustmentDetails)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _stockAdjustmentDetailService.ConfirmObject(detail, ConfirmationDate, this, _stockMutationService, _itemService, _blanketService, _warehouseItemService);
                    Total += detail.Quantity * detail.Price;
                    Item item = _itemService.GetObjectById(detail.ItemId);
                    ItemType itemType = _itemTypeService.GetObjectById(item.ItemTypeId);
                    _generalLedgerJournalService.CreateConfirmationJournalForStockAdjustmentDetail(stockAdjustment, itemType.AccountId.GetValueOrDefault(), detail.Quantity * detail.Price, _accountService);
                }
                stockAdjustment.Total = Total;
                _repository.ConfirmObject(stockAdjustment);
                _generalLedgerJournalService.CreateConfirmationJournalForStockAdjustment(stockAdjustment, _accountService);
            }
            return stockAdjustment;
        }

        public StockAdjustment UnconfirmObject(StockAdjustment stockAdjustment, IStockAdjustmentDetailService _stockAdjustmentDetailService,
                                               IStockMutationService _stockMutationService, IItemService _itemService, IItemTypeService _itemTypeService,
                                               IBlanketService _blanketService, IWarehouseItemService _warehouseItemService,
                                               IAccountService _accountService, IGeneralLedgerJournalService _generalLedgerJournalService, IClosingService _closingService)
        {
            if (_validator.ValidUnconfirmObject(stockAdjustment, this, _stockAdjustmentDetailService, _itemService, _blanketService, _warehouseItemService, _closingService))
            {
                IList<StockAdjustmentDetail> stockAdjustmentDetails = _stockAdjustmentDetailService.GetObjectsByStockAdjustmentId(stockAdjustment.Id);
                foreach (var detail in stockAdjustmentDetails)
                {
                    detail.Errors = new Dictionary<string, string>();
                    _stockAdjustmentDetailService.UnconfirmObject(detail, this, _stockMutationService, _itemService, _blanketService, _warehouseItemService);
                    Item item = _itemService.GetObjectById(detail.ItemId);
                    ItemType itemType = _itemTypeService.GetObjectById(item.ItemTypeId);
                    _generalLedgerJournalService.CreateUnconfirmationJournalForStockAdjustmentDetail(stockAdjustment, itemType.AccountId.GetValueOrDefault(), detail.Quantity * detail.Price, _accountService);
                }
                _generalLedgerJournalService.CreateUnconfirmationJournalForStockAdjustment(stockAdjustment, _accountService);
                stockAdjustment.Total = 0;
                _repository.UnconfirmObject(stockAdjustment);
            }
            return stockAdjustment;
        }
    }
}