using Core.Constants;
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
    public class BarringOrderDetailService : IBarringOrderDetailService
    {
        private IBarringOrderDetailRepository _repository;
        private IBarringOrderDetailValidator _validator;
        public BarringOrderDetailService(IBarringOrderDetailRepository _barringOrderDetailRepository, IBarringOrderDetailValidator _barringOrderDetailValidator)
        {
            _repository = _barringOrderDetailRepository;
            _validator = _barringOrderDetailValidator;
        }

        public IBarringOrderDetailValidator GetValidator()
        {
            return _validator;
        }

        public IBarringOrderDetailRepository GetRepository()
        {
            return _repository;
        }

        public IQueryable<BarringOrderDetail> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<BarringOrderDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<BarringOrderDetail> GetObjectsByBarringOrderId(int barringOrderId)
        {
            return _repository.GetObjectsByBarringOrderId(barringOrderId);
        }

        public IList<BarringOrderDetail> GetObjectsByBarringId(int barringId)
        {
            return _repository.GetObjectsByBarringId(barringId);
        }

        public BarringOrderDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public Barring GetBarring(BarringOrderDetail barringOrderDetail, IBarringService _barringService)
        {
            return _barringService.GetObjectById(barringOrderDetail.BarringId);
        }
        
        public BarringOrderDetail CreateObject(BarringOrderDetail barringOrderDetail, IBarringOrderService _barringOrderService, IBarringService _barringService)
        {
            barringOrderDetail.Errors = new Dictionary<String, String>();
            return (barringOrderDetail = _validator.ValidCreateObject(barringOrderDetail, _barringOrderService, _barringService) ?
                                          _repository.CreateObject(barringOrderDetail) : barringOrderDetail);
        }

        public BarringOrderDetail UpdateObject(BarringOrderDetail barringOrderDetail, IBarringOrderService _barringOrderService, IBarringService _barringService)
        {
            return (barringOrderDetail = _validator.ValidUpdateObject(barringOrderDetail, _barringOrderService, _barringService) ?
                                          _repository.UpdateObject(barringOrderDetail) : barringOrderDetail);
        }

        public BarringOrderDetail SoftDeleteObject(BarringOrderDetail barringOrderDetail, IBarringOrderService _barringOrderService)
        {
            return (barringOrderDetail = _validator.ValidDeleteObject(barringOrderDetail, _barringOrderService) ?
                                          _repository.SoftDeleteObject(barringOrderDetail) : barringOrderDetail);
        }

        public BarringOrderDetail CutObject(BarringOrderDetail barringOrderDetail, IBarringOrderService _barringOrderService)
        {
            return (barringOrderDetail = _validator.ValidCutObject(barringOrderDetail, _barringOrderService) ? _repository.CutObject(barringOrderDetail) : barringOrderDetail);
        }

        public BarringOrderDetail SideSealObject(BarringOrderDetail barringOrderDetail)
        {
            return (barringOrderDetail = _validator.ValidSideSealObject(barringOrderDetail) ? _repository.SideSealObject(barringOrderDetail) : barringOrderDetail);
        }

        public BarringOrderDetail PrepareObject(BarringOrderDetail barringOrderDetail)
        {
            return (barringOrderDetail = _validator.ValidPrepareObject(barringOrderDetail) ? _repository.PrepareObject(barringOrderDetail) : barringOrderDetail);
        }

        public BarringOrderDetail ApplyTapeAdhesiveToObject(BarringOrderDetail barringOrderDetail)
        {
            return (barringOrderDetail = _validator.ValidApplyTapeAdhesiveToObject(barringOrderDetail) ? _repository.ApplyTapeAdhesiveToObject(barringOrderDetail) : barringOrderDetail);
        }

        public BarringOrderDetail MountObject(BarringOrderDetail barringOrderDetail)
        {
            return (barringOrderDetail = _validator.ValidMountObject(barringOrderDetail) ? _repository.MountObject(barringOrderDetail) : barringOrderDetail);
        }

        public BarringOrderDetail HeatPressObject(BarringOrderDetail barringOrderDetail)
        {
            return (barringOrderDetail = _validator.ValidHeatPressObject(barringOrderDetail) ? _repository.HeatPressObject(barringOrderDetail) : barringOrderDetail);
        }

        public BarringOrderDetail PullOffTestObject(BarringOrderDetail barringOrderDetail)
        {
            return (barringOrderDetail = _validator.ValidPullOffTestObject(barringOrderDetail) ? _repository.PullOffTestObject(barringOrderDetail) : barringOrderDetail);
        }

        public BarringOrderDetail QCAndMarkObject(BarringOrderDetail barringOrderDetail)
        {
            return (barringOrderDetail = _validator.ValidQCAndMarkObject(barringOrderDetail) ? _repository.QCAndMarkObject(barringOrderDetail) : barringOrderDetail);
        }

        public BarringOrderDetail PackageObject(BarringOrderDetail barringOrderDetail)
        {
            return (barringOrderDetail = _validator.ValidPackageObject(barringOrderDetail) ? _repository.PackageObject(barringOrderDetail) : barringOrderDetail);
        }

        public BarringOrderDetail RejectObject(BarringOrderDetail barringOrderDetail, DateTime RejectedDate, IBarringOrderService _barringOrderService, IStockMutationService _stockMutationService,
                                               IBarringService _barringService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            barringOrderDetail.RejectedDate = RejectedDate;
            if (_validator.ValidRejectObject(barringOrderDetail, _barringOrderService))
            {
                _repository.RejectObject(barringOrderDetail);

                // add barring order reject quantity
                // if valid, complete barring order = true
                BarringOrder barringOrder = _barringOrderService.GetObjectById(barringOrderDetail.BarringOrderId);
                barringOrder.QuantityRejected += 1;
                _barringOrderService.AdjustQuantity(barringOrder);
                if (_barringOrderService.GetValidator().ValidCompleteObject(barringOrder, this))
                {
                    _barringOrderService.CompleteObject(barringOrder, this, _barringService, _itemService, _warehouseItemService);
                }

                // deduce bars quantity
                Barring barring = _barringService.GetObjectById(barringOrderDetail.BarringId);
                bool CaseAddition = false;

                if (barring.HasLeftBar)
                {
                    Item leftbar = _barringService.GetLeftBarItem(barring);
                    WarehouseItem warehouseLeftBar = _warehouseItemService.FindOrCreateObject(barringOrder.WarehouseId, leftbar.Id);
                    StockMutation stockMutationLeftBar = _stockMutationService.CreateStockMutationForBarringOrder(barringOrderDetail, warehouseLeftBar, CaseAddition);
                    _stockMutationService.StockMutateObject(stockMutationLeftBar, _itemService, _barringService, _warehouseItemService);
                }
                if (barring.HasRightBar)
                {
                    Item rightbar = _barringService.GetRightBarItem(barring);
                    WarehouseItem warehouseRightBar = _warehouseItemService.FindOrCreateObject(barringOrder.WarehouseId, rightbar.Id);
                    StockMutation stockMutationRightBar = _stockMutationService.CreateStockMutationForBarringOrder(barringOrderDetail, warehouseRightBar, CaseAddition);
                    _stockMutationService.StockMutateObject(stockMutationRightBar, _itemService, _barringService, _warehouseItemService);
                }

                // deduce blanket quantity
                WarehouseItem warehouseBlanket = _warehouseItemService.FindOrCreateObject(barringOrder.WarehouseId, barring.BlanketItemId);
                StockMutation stockMutationBlanket = _stockMutationService.CreateStockMutationForBarringOrder(barringOrderDetail, warehouseBlanket, CaseAddition);
                _stockMutationService.StockMutateObject(stockMutationBlanket, _itemService, _barringService, _warehouseItemService);
            }
            return barringOrderDetail;
        }

        public BarringOrderDetail UndoRejectObject(BarringOrderDetail barringOrderDetail, IBarringOrderService _barringOrderService, IStockMutationService _stockMutationService,
                                               IBarringService _barringService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidUndoRejectObject(barringOrderDetail, _barringOrderService))
            {
                _repository.UndoRejectObject(barringOrderDetail);

                // deduce barring order reject quantity
                BarringOrder barringOrder = _barringOrderService.GetObjectById(barringOrderDetail.BarringOrderId);
                barringOrder.QuantityRejected -= 1;
                _barringOrderService.AdjustQuantity(barringOrder);

                // reverse stock mutation of RejectObject
                Barring barring = _barringService.GetObjectById(barringOrderDetail.BarringId);
                if (barring.HasLeftBar)
                {
                    Item leftbar = _barringService.GetLeftBarItem(barring);
                    WarehouseItem warehouseLeftBar = _warehouseItemService.FindOrCreateObject(barringOrder.WarehouseId, leftbar.Id);
                    IList<StockMutation> stockMutationLeftBars = _stockMutationService.SoftDeleteStockMutationForBarringOrder(barringOrderDetail, warehouseLeftBar);
                    foreach (var stockMutationLeftBar in stockMutationLeftBars)
                    {
                        _stockMutationService.ReverseStockMutateObject(stockMutationLeftBar, _itemService, _barringService, _warehouseItemService);
                    }
                }
                if (barring.HasRightBar)
                {
                    Item rightbar = _barringService.GetRightBarItem(barring);
                    WarehouseItem warehouseRightBar = _warehouseItemService.FindOrCreateObject(barringOrder.WarehouseId, rightbar.Id);
                    IList<StockMutation> stockMutationRightBars = _stockMutationService.SoftDeleteStockMutationForBarringOrder(barringOrderDetail, warehouseRightBar);
                    foreach (var stockMutationRightBar in stockMutationRightBars)
                    {
                        _stockMutationService.ReverseStockMutateObject(stockMutationRightBar, _itemService, _barringService, _warehouseItemService);
                    }
                }

                WarehouseItem warehouseBlanket = _warehouseItemService.FindOrCreateObject(barringOrder.WarehouseId, barring.BlanketItemId);
                IList<StockMutation> stockMutationBlankets = _stockMutationService.SoftDeleteStockMutationForBarringOrder(barringOrderDetail, warehouseBlanket);
                foreach (var stockMutationBlanket in stockMutationBlankets)
                {
                    _stockMutationService.ReverseStockMutateObject(stockMutationBlanket, _itemService, _barringService, _warehouseItemService);
                }
            }
            return barringOrderDetail;
        }

        public BarringOrderDetail FinishObject(BarringOrderDetail barringOrderDetail, DateTime FinishedDate, IBarringOrderService _barringOrderService, IStockMutationService _stockMutationService,
                                               IBarringService _barringService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            barringOrderDetail.FinishedDate = FinishedDate;
            if (_validator.ValidFinishObject(barringOrderDetail, _barringOrderService))
            {
                _repository.FinishObject(barringOrderDetail);

                // add barring order quantity final
                // if valid, complete barring order = true
                BarringOrder barringOrder = _barringOrderService.GetObjectById(barringOrderDetail.BarringOrderId);
                barringOrder.QuantityFinal += 1;
                _barringOrderService.AdjustQuantity(barringOrder);
                if (_barringOrderService.GetValidator().ValidCompleteObject(barringOrder, this))
                {
                    _barringOrderService.CompleteObject(barringOrder, this, _barringService, _itemService, _warehouseItemService);
                }

                bool CaseAdditionElse = false;
                bool CaseAdditionBarring = true;

                // add barring quantity
                Barring barring = _barringService.GetObjectById(barringOrderDetail.BarringId);
                WarehouseItem warehouseBarring = _warehouseItemService.FindOrCreateObject(barringOrder.WarehouseId, barring.Id);
                StockMutation stockMutation = _stockMutationService.CreateStockMutationForBarringOrder(barringOrderDetail, warehouseBarring, CaseAdditionBarring);
                _stockMutationService.StockMutateObject(stockMutation, _itemService, _barringService, _warehouseItemService);

                // deduce bars quantity
                if (barring.HasLeftBar)
                {
                    Item leftbar = _barringService.GetLeftBarItem(barring);
                    WarehouseItem warehouseLeftBar = _warehouseItemService.FindOrCreateObject(barringOrder.WarehouseId, leftbar.Id);
                    StockMutation stockMutationLeftBar = _stockMutationService.CreateStockMutationForBarringOrder(barringOrderDetail, warehouseLeftBar, CaseAdditionElse);
                    _stockMutationService.StockMutateObject(stockMutationLeftBar, _itemService, _barringService, _warehouseItemService);
                }
                if (barring.HasRightBar)
                {
                    Item rightbar = _barringService.GetRightBarItem(barring);
                    WarehouseItem warehouseRightBar = _warehouseItemService.FindOrCreateObject(barringOrder.WarehouseId, rightbar.Id);
                    StockMutation stockMutationRightBar = _stockMutationService.CreateStockMutationForBarringOrder(barringOrderDetail, warehouseRightBar, CaseAdditionElse);
                    _stockMutationService.StockMutateObject(stockMutationRightBar, _itemService, _barringService, _warehouseItemService);
                }

                // deduce blanket quantity
                WarehouseItem warehouseBlanket = _warehouseItemService.FindOrCreateObject(barringOrder.WarehouseId, barring.BlanketItemId);
                StockMutation stockMutationBlanket = _stockMutationService.CreateStockMutationForBarringOrder(barringOrderDetail, warehouseBlanket, CaseAdditionElse);
                _stockMutationService.StockMutateObject(stockMutationBlanket, _itemService, _barringService, _warehouseItemService);
            }
            return barringOrderDetail;
        }

        public BarringOrderDetail UnfinishObject(BarringOrderDetail barringOrderDetail, IBarringOrderService _barringOrderService, IStockMutationService _stockMutationService,
                                                 IBarringService _barringService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidUnfinishObject(barringOrderDetail, _barringOrderService))
            {
                _repository.UnfinishObject(barringOrderDetail);
                // deduce barring order quantity final
                BarringOrder barringOrder = _barringOrderService.GetObjectById(barringOrderDetail.BarringOrderId);
                barringOrder.QuantityFinal -= 1;
                _barringOrderService.AdjustQuantity(barringOrder);

                // reverse stock mutation
                Barring barring = _barringService.GetObjectById(barringOrderDetail.BarringId);
                WarehouseItem warehouseBarring = _warehouseItemService.FindOrCreateObject(barringOrder.WarehouseId, barring.Id);
                IList<StockMutation> stockMutations = _stockMutationService.SoftDeleteStockMutationForBarringOrder(barringOrderDetail, warehouseBarring);
                foreach (var stockMutation in stockMutations)
                {
                    _stockMutationService.ReverseStockMutateObject(stockMutation, _itemService, _barringService, _warehouseItemService);
                }

                if (barring.HasLeftBar)
                {
                    Item leftbar = _barringService.GetLeftBarItem(barring);
                    WarehouseItem warehouseLeftBar = _warehouseItemService.FindOrCreateObject(barringOrder.WarehouseId, leftbar.Id);
                    IList<StockMutation> stockMutationLeftBars = _stockMutationService.SoftDeleteStockMutationForBarringOrder(barringOrderDetail, warehouseLeftBar);
                    foreach (var stockMutationLeftBar in stockMutationLeftBars)
                    {
                        _stockMutationService.ReverseStockMutateObject(stockMutationLeftBar, _itemService, _barringService, _warehouseItemService);
                    }
                }
                if (barring.HasRightBar)
                {
                    Item rightbar = _barringService.GetRightBarItem(barring);
                    WarehouseItem warehouseRightBar = _warehouseItemService.FindOrCreateObject(barringOrder.WarehouseId, rightbar.Id);
                    IList<StockMutation> stockMutationRightBars = _stockMutationService.SoftDeleteStockMutationForBarringOrder(barringOrderDetail, warehouseRightBar);
                    foreach (var stockMutationRightBar in stockMutationRightBars)
                    {
                        _stockMutationService.ReverseStockMutateObject(stockMutationRightBar, _itemService, _barringService, _warehouseItemService);
                    }
                }

                WarehouseItem warehouseBlanket = _warehouseItemService.FindOrCreateObject(barringOrder.WarehouseId, barring.BlanketItemId);
                IList<StockMutation> stockMutationBlankets = _stockMutationService.SoftDeleteStockMutationForBarringOrder(barringOrderDetail, warehouseBlanket);
                foreach (var stockMutationBlanket in stockMutationBlankets)
                {
                    _stockMutationService.ReverseStockMutateObject(stockMutationBlanket, _itemService, _barringService, _warehouseItemService);
                }
            }
            return barringOrderDetail;
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

    }
}