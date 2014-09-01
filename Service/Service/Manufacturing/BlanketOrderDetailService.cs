﻿using Core.Constants;
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
    public class BlanketOrderDetailService : IBlanketOrderDetailService
    {
        private IBlanketOrderDetailRepository _repository;
        private IBlanketOrderDetailValidator _validator;
        public BlanketOrderDetailService(IBlanketOrderDetailRepository _blanketOrderDetailRepository, IBlanketOrderDetailValidator _blanketOrderDetailValidator)
        {
            _repository = _blanketOrderDetailRepository;
            _validator = _blanketOrderDetailValidator;
        }

        public IBlanketOrderDetailValidator GetValidator()
        {
            return _validator;
        }

        public IBlanketOrderDetailRepository GetRepository()
        {
            return _repository;
        }

        public IQueryable<BlanketOrderDetail> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<BlanketOrderDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<BlanketOrderDetail> GetObjectsByBlanketOrderId(int blanketOrderId)
        {
            return _repository.GetObjectsByBlanketOrderId(blanketOrderId);
        }

        public IList<BlanketOrderDetail> GetObjectsByBlanketId(int blanketId)
        {
            return _repository.GetObjectsByBlanketId(blanketId);
        }

        public BlanketOrderDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public Blanket GetBlanket(BlanketOrderDetail blanketOrderDetail, IBlanketService _blanketService)
        {
            return _blanketService.GetObjectById(blanketOrderDetail.BlanketId);
        }
        
        public BlanketOrderDetail CreateObject(BlanketOrderDetail blanketOrderDetail, IBlanketOrderService _blanketOrderService, IBlanketService _blanketService)
        {
            blanketOrderDetail.Errors = new Dictionary<String, String>();
            return (blanketOrderDetail = _validator.ValidCreateObject(blanketOrderDetail, _blanketOrderService, _blanketService) ?
                                          _repository.CreateObject(blanketOrderDetail) : blanketOrderDetail);
        }

        public BlanketOrderDetail UpdateObject(BlanketOrderDetail blanketOrderDetail, IBlanketOrderService _blanketOrderService, IBlanketService _blanketService)
        {
            return (blanketOrderDetail = _validator.ValidUpdateObject(blanketOrderDetail, _blanketOrderService, _blanketService) ?
                                          _repository.UpdateObject(blanketOrderDetail) : blanketOrderDetail);
        }

        public BlanketOrderDetail SoftDeleteObject(BlanketOrderDetail blanketOrderDetail, IBlanketOrderService _blanketOrderService)
        {
            return (blanketOrderDetail = _validator.ValidDeleteObject(blanketOrderDetail, _blanketOrderService) ?
                                          _repository.SoftDeleteObject(blanketOrderDetail) : blanketOrderDetail);
        }

        public BlanketOrderDetail CutObject(BlanketOrderDetail blanketOrderDetail, IBlanketOrderService _blanketOrderService)
        {
            return (blanketOrderDetail = _validator.ValidCutObject(blanketOrderDetail, _blanketOrderService) ? _repository.CutObject(blanketOrderDetail) : blanketOrderDetail);
        }

        public BlanketOrderDetail SideSealObject(BlanketOrderDetail blanketOrderDetail)
        {
            return (blanketOrderDetail = _validator.ValidSideSealObject(blanketOrderDetail) ? _repository.SideSealObject(blanketOrderDetail) : blanketOrderDetail);
        }

        public BlanketOrderDetail PrepareObject(BlanketOrderDetail blanketOrderDetail)
        {
            return (blanketOrderDetail = _validator.ValidPrepareObject(blanketOrderDetail) ? _repository.PrepareObject(blanketOrderDetail) : blanketOrderDetail);
        }

        public BlanketOrderDetail ApplyTapeAdhesiveToObject(BlanketOrderDetail blanketOrderDetail)
        {
            return (blanketOrderDetail = _validator.ValidApplyTapeAdhesiveToObject(blanketOrderDetail) ? _repository.ApplyTapeAdhesiveToObject(blanketOrderDetail) : blanketOrderDetail);
        }

        public BlanketOrderDetail MountObject(BlanketOrderDetail blanketOrderDetail)
        {
            return (blanketOrderDetail = _validator.ValidMountObject(blanketOrderDetail) ? _repository.MountObject(blanketOrderDetail) : blanketOrderDetail);
        }

        public BlanketOrderDetail HeatPressObject(BlanketOrderDetail blanketOrderDetail)
        {
            return (blanketOrderDetail = _validator.ValidHeatPressObject(blanketOrderDetail) ? _repository.HeatPressObject(blanketOrderDetail) : blanketOrderDetail);
        }

        public BlanketOrderDetail PullOffTestObject(BlanketOrderDetail blanketOrderDetail)
        {
            return (blanketOrderDetail = _validator.ValidPullOffTestObject(blanketOrderDetail) ? _repository.PullOffTestObject(blanketOrderDetail) : blanketOrderDetail);
        }

        public BlanketOrderDetail QCAndMarkObject(BlanketOrderDetail blanketOrderDetail)
        {
            return (blanketOrderDetail = _validator.ValidQCAndMarkObject(blanketOrderDetail) ? _repository.QCAndMarkObject(blanketOrderDetail) : blanketOrderDetail);
        }

        public BlanketOrderDetail PackageObject(BlanketOrderDetail blanketOrderDetail)
        {
            return (blanketOrderDetail = _validator.ValidPackageObject(blanketOrderDetail) ? _repository.PackageObject(blanketOrderDetail) : blanketOrderDetail);
        }

        public BlanketOrderDetail RejectObject(BlanketOrderDetail blanketOrderDetail, DateTime RejectedDate, IBlanketOrderService _blanketOrderService, IStockMutationService _stockMutationService,
                                               IBlanketService _blanketService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            blanketOrderDetail.RejectedDate = RejectedDate;
            if (_validator.ValidRejectObject(blanketOrderDetail, _blanketOrderService))
            {
                _repository.RejectObject(blanketOrderDetail);

                // add blanket order reject quantity
                // if valid, complete blanket order = true
                BlanketOrder blanketOrder = _blanketOrderService.GetObjectById(blanketOrderDetail.BlanketOrderId);
                blanketOrder.QuantityRejected += 1;
                _blanketOrderService.AdjustQuantity(blanketOrder);
                if (_blanketOrderService.GetValidator().ValidCompleteObject(blanketOrder, this))
                {
                    _blanketOrderService.CompleteObject(blanketOrder, this, _blanketService, _itemService, _warehouseItemService);
                }

                // deduce bars quantity
                Blanket blanket = _blanketService.GetObjectById(blanketOrderDetail.BlanketId);
                bool CaseAddition = false;

                if (blanket.HasLeftBar)
                {
                    Item leftbar = _blanketService.GetLeftBarItem(blanket);
                    WarehouseItem warehouseLeftBar = _warehouseItemService.FindOrCreateObject(blanketOrder.WarehouseId, leftbar.Id);
                    StockMutation stockMutationLeftBar = _stockMutationService.CreateStockMutationForBlanketOrder(blanketOrderDetail, warehouseLeftBar, CaseAddition);
                    _stockMutationService.StockMutateObject(stockMutationLeftBar, _itemService, _blanketService, _warehouseItemService);
                }
                if (blanket.HasRightBar)
                {
                    Item rightbar = _blanketService.GetRightBarItem(blanket);
                    WarehouseItem warehouseRightBar = _warehouseItemService.FindOrCreateObject(blanketOrder.WarehouseId, rightbar.Id);
                    StockMutation stockMutationRightBar = _stockMutationService.CreateStockMutationForBlanketOrder(blanketOrderDetail, warehouseRightBar, CaseAddition);
                    _stockMutationService.StockMutateObject(stockMutationRightBar, _itemService, _blanketService, _warehouseItemService);
                }

                // deduce rollBlanket quantity
                WarehouseItem warehouseRollBlanket = _warehouseItemService.FindOrCreateObject(blanketOrder.WarehouseId, blanket.RollBlanketItemId);
                StockMutation stockMutationRollBlanket = _stockMutationService.CreateStockMutationForBlanketOrder(blanketOrderDetail, warehouseRollBlanket, CaseAddition);
                _stockMutationService.StockMutateObject(stockMutationRollBlanket, _itemService, _blanketService, _warehouseItemService);
            }
            return blanketOrderDetail;
        }

        public BlanketOrderDetail UndoRejectObject(BlanketOrderDetail blanketOrderDetail, IBlanketOrderService _blanketOrderService, IStockMutationService _stockMutationService,
                                               IBlanketService _blanketService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidUndoRejectObject(blanketOrderDetail, _blanketOrderService))
            {
                _repository.UndoRejectObject(blanketOrderDetail);

                // deduce blanket order reject quantity
                BlanketOrder blanketOrder = _blanketOrderService.GetObjectById(blanketOrderDetail.BlanketOrderId);
                blanketOrder.QuantityRejected -= 1;
                _blanketOrderService.AdjustQuantity(blanketOrder);

                // reverse stock mutation of RejectObject
                Blanket blanket = _blanketService.GetObjectById(blanketOrderDetail.BlanketId);
                if (blanket.HasLeftBar)
                {
                    Item leftbar = _blanketService.GetLeftBarItem(blanket);
                    WarehouseItem warehouseLeftBar = _warehouseItemService.FindOrCreateObject(blanketOrder.WarehouseId, leftbar.Id);
                    IList<StockMutation> stockMutationLeftBars = _stockMutationService.SoftDeleteStockMutationForBlanketOrder(blanketOrderDetail, warehouseLeftBar);
                    foreach (var stockMutationLeftBar in stockMutationLeftBars)
                    {
                        _stockMutationService.ReverseStockMutateObject(stockMutationLeftBar, _itemService, _blanketService, _warehouseItemService);
                    }
                }
                if (blanket.HasRightBar)
                {
                    Item rightbar = _blanketService.GetRightBarItem(blanket);
                    WarehouseItem warehouseRightBar = _warehouseItemService.FindOrCreateObject(blanketOrder.WarehouseId, rightbar.Id);
                    IList<StockMutation> stockMutationRightBars = _stockMutationService.SoftDeleteStockMutationForBlanketOrder(blanketOrderDetail, warehouseRightBar);
                    foreach (var stockMutationRightBar in stockMutationRightBars)
                    {
                        _stockMutationService.ReverseStockMutateObject(stockMutationRightBar, _itemService, _blanketService, _warehouseItemService);
                    }
                }

                WarehouseItem warehouseRollBlanket = _warehouseItemService.FindOrCreateObject(blanketOrder.WarehouseId, blanket.RollBlanketItemId);
                IList<StockMutation> stockMutationRollBlankets = _stockMutationService.SoftDeleteStockMutationForBlanketOrder(blanketOrderDetail, warehouseRollBlanket);
                foreach (var stockMutationRollBlanket in stockMutationRollBlankets)
                {
                    _stockMutationService.ReverseStockMutateObject(stockMutationRollBlanket, _itemService, _blanketService, _warehouseItemService);
                }
            }
            return blanketOrderDetail;
        }

        public BlanketOrderDetail FinishObject(BlanketOrderDetail blanketOrderDetail, DateTime FinishedDate, IBlanketOrderService _blanketOrderService, IStockMutationService _stockMutationService,
                                               IBlanketService _blanketService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            blanketOrderDetail.FinishedDate = FinishedDate;
            if (_validator.ValidFinishObject(blanketOrderDetail, _blanketOrderService))
            {
                _repository.FinishObject(blanketOrderDetail);

                // add blanket order quantity final
                // if valid, complete blanket order = true
                BlanketOrder blanketOrder = _blanketOrderService.GetObjectById(blanketOrderDetail.BlanketOrderId);
                blanketOrder.QuantityFinal += 1;
                _blanketOrderService.AdjustQuantity(blanketOrder);
                if (_blanketOrderService.GetValidator().ValidCompleteObject(blanketOrder, this))
                {
                    _blanketOrderService.CompleteObject(blanketOrder, this, _blanketService, _itemService, _warehouseItemService);
                }

                bool CaseAdditionElse = false;
                bool CaseAdditionBlanket = true;

                // add blanket quantity
                Blanket blanket = _blanketService.GetObjectById(blanketOrderDetail.BlanketId);
                WarehouseItem warehouseBlanket = _warehouseItemService.FindOrCreateObject(blanketOrder.WarehouseId, blanket.Id);
                StockMutation stockMutation = _stockMutationService.CreateStockMutationForBlanketOrder(blanketOrderDetail, warehouseBlanket, CaseAdditionBlanket);
                _stockMutationService.StockMutateObject(stockMutation, _itemService, _blanketService, _warehouseItemService);

                // deduce bars quantity
                if (blanket.HasLeftBar)
                {
                    Item leftbar = _blanketService.GetLeftBarItem(blanket);
                    WarehouseItem warehouseLeftBar = _warehouseItemService.FindOrCreateObject(blanketOrder.WarehouseId, leftbar.Id);
                    StockMutation stockMutationLeftBar = _stockMutationService.CreateStockMutationForBlanketOrder(blanketOrderDetail, warehouseLeftBar, CaseAdditionElse);
                    _stockMutationService.StockMutateObject(stockMutationLeftBar, _itemService, _blanketService, _warehouseItemService);
                }
                if (blanket.HasRightBar)
                {
                    Item rightbar = _blanketService.GetRightBarItem(blanket);
                    WarehouseItem warehouseRightBar = _warehouseItemService.FindOrCreateObject(blanketOrder.WarehouseId, rightbar.Id);
                    StockMutation stockMutationRightBar = _stockMutationService.CreateStockMutationForBlanketOrder(blanketOrderDetail, warehouseRightBar, CaseAdditionElse);
                    _stockMutationService.StockMutateObject(stockMutationRightBar, _itemService, _blanketService, _warehouseItemService);
                }

                // deduce rollBlanket quantity
                WarehouseItem warehouseRollBlanket = _warehouseItemService.FindOrCreateObject(blanketOrder.WarehouseId, blanket.RollBlanketItemId);
                StockMutation stockMutationRollBlanket = _stockMutationService.CreateStockMutationForBlanketOrder(blanketOrderDetail, warehouseRollBlanket, CaseAdditionElse);
                _stockMutationService.StockMutateObject(stockMutationRollBlanket, _itemService, _blanketService, _warehouseItemService);
            }
            return blanketOrderDetail;
        }

        public BlanketOrderDetail UnfinishObject(BlanketOrderDetail blanketOrderDetail, IBlanketOrderService _blanketOrderService, IStockMutationService _stockMutationService,
                                                 IBlanketService _blanketService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidUnfinishObject(blanketOrderDetail, _blanketOrderService))
            {
                _repository.UnfinishObject(blanketOrderDetail);
                // deduce blanket order quantity final
                BlanketOrder blanketOrder = _blanketOrderService.GetObjectById(blanketOrderDetail.BlanketOrderId);
                blanketOrder.QuantityFinal -= 1;
                _blanketOrderService.AdjustQuantity(blanketOrder);

                // reverse stock mutation
                Blanket blanket = _blanketService.GetObjectById(blanketOrderDetail.BlanketId);
                WarehouseItem warehouseBlanket = _warehouseItemService.FindOrCreateObject(blanketOrder.WarehouseId, blanket.Id);
                IList<StockMutation> stockMutations = _stockMutationService.SoftDeleteStockMutationForBlanketOrder(blanketOrderDetail, warehouseBlanket);
                foreach (var stockMutation in stockMutations)
                {
                    _stockMutationService.ReverseStockMutateObject(stockMutation, _itemService, _blanketService, _warehouseItemService);
                }

                if (blanket.HasLeftBar)
                {
                    Item leftbar = _blanketService.GetLeftBarItem(blanket);
                    WarehouseItem warehouseLeftBar = _warehouseItemService.FindOrCreateObject(blanketOrder.WarehouseId, leftbar.Id);
                    IList<StockMutation> stockMutationLeftBars = _stockMutationService.SoftDeleteStockMutationForBlanketOrder(blanketOrderDetail, warehouseLeftBar);
                    foreach (var stockMutationLeftBar in stockMutationLeftBars)
                    {
                        _stockMutationService.ReverseStockMutateObject(stockMutationLeftBar, _itemService, _blanketService, _warehouseItemService);
                    }
                }
                if (blanket.HasRightBar)
                {
                    Item rightbar = _blanketService.GetRightBarItem(blanket);
                    WarehouseItem warehouseRightBar = _warehouseItemService.FindOrCreateObject(blanketOrder.WarehouseId, rightbar.Id);
                    IList<StockMutation> stockMutationRightBars = _stockMutationService.SoftDeleteStockMutationForBlanketOrder(blanketOrderDetail, warehouseRightBar);
                    foreach (var stockMutationRightBar in stockMutationRightBars)
                    {
                        _stockMutationService.ReverseStockMutateObject(stockMutationRightBar, _itemService, _blanketService, _warehouseItemService);
                    }
                }

                WarehouseItem warehouseRollBlanket = _warehouseItemService.FindOrCreateObject(blanketOrder.WarehouseId, blanket.RollBlanketItemId);
                IList<StockMutation> stockMutationRollBlankets = _stockMutationService.SoftDeleteStockMutationForBlanketOrder(blanketOrderDetail, warehouseRollBlanket);
                foreach (var stockMutationRollBlanket in stockMutationRollBlankets)
                {
                    _stockMutationService.ReverseStockMutateObject(stockMutationRollBlanket, _itemService, _blanketService, _warehouseItemService);
                }
            }
            return blanketOrderDetail;
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

    }
}