using Core.Constants;
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

        public IList<BarringOrderDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<BarringOrderDetail> GetObjectsByBarringOrderId(int barringOrderId)
        {
            return _repository.GetObjectsByBarringOrderId(barringOrderId);
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

        public BarringOrderDetail SetJobScheduled(BarringOrderDetail barringOrderDetail)
        {
            return _repository.SetJobScheduled(barringOrderDetail);
        }

        public BarringOrderDetail UnsetJobScheduled(BarringOrderDetail barringOrderDetail)
        {
            return _repository.UnsetJobScheduled(barringOrderDetail);
        }

        public BarringOrderDetail AddLeftBar(BarringOrderDetail barringOrderDetail, IBarringService _barringService)
        {
            return (barringOrderDetail = _validator.ValidAddLeftBar(barringOrderDetail) ?
                                         _repository.AddLeftBar(barringOrderDetail) : barringOrderDetail);
        }

        public BarringOrderDetail RemoveLeftBar(BarringOrderDetail barringOrderDetail, IBarringService _barringService)
        {
            return (barringOrderDetail = _validator.ValidRemoveLeftBar(barringOrderDetail) ?
                                         _repository.RemoveLeftBar(barringOrderDetail) : barringOrderDetail);
        }

        public BarringOrderDetail AddRightBar(BarringOrderDetail barringOrderDetail, IBarringService _barringService)
        {
            return (barringOrderDetail = _validator.ValidAddRightBar(barringOrderDetail) ?
                                         _repository.AddRightBar(barringOrderDetail) : barringOrderDetail);
        }

        public BarringOrderDetail RemoveRightBar(BarringOrderDetail barringOrderDetail, IBarringService _barringService)
        {
            return (barringOrderDetail = _validator.ValidRemoveRightBar(barringOrderDetail) ?
                                         _repository.RemoveRightBar(barringOrderDetail) : barringOrderDetail);
        }

        public BarringOrderDetail CutObject(BarringOrderDetail barringOrderDetail)
        {
            return (barringOrderDetail = _validator.ValidCutObject(barringOrderDetail) ? _repository.CutObject(barringOrderDetail) : barringOrderDetail);
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

        public BarringOrderDetail RejectObject(BarringOrderDetail barringOrderDetail, IBarringOrderService _barringOrderService, IStockMutationService _stockMutationService,
                                               IBarringService _barringService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
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

                if (barringOrderDetail.IsBarRequired)
                {
                    Item leftbar = (barringOrderDetail.HasLeftBar && barring.LeftBarItemId != null) ? _itemService.GetObjectById((int)barring.LeftBarItemId) : null;
                    Item rightbar = (barringOrderDetail.HasRightBar && barring.RightBarItemId != null) ? _itemService.GetObjectById((int)barring.RightBarItemId) : null;
                    if (leftbar != null)
                    {
                        WarehouseItem warehouseLeftBar = _warehouseItemService.FindOrCreateObject(barringOrder.WarehouseId, leftbar.Id);
                        StockMutation stockMutationLeftBar = _stockMutationService.CreateStockMutationForBarringOrder(barringOrderDetail, warehouseLeftBar, CaseAddition);
                        StockMutateObject(stockMutationLeftBar, _itemService, _barringService, _warehouseItemService);
                    }
                    if (rightbar != null)
                    {
                        WarehouseItem warehouseRightBar = _warehouseItemService.FindOrCreateObject(barringOrder.WarehouseId, rightbar.Id);
                        StockMutation stockMutationRightBar = _stockMutationService.CreateStockMutationForBarringOrder(barringOrderDetail, warehouseRightBar, CaseAddition);
                        StockMutateObject(stockMutationRightBar, _itemService, _barringService, _warehouseItemService);
                    }
                }

                // deduce blanket quantity
                WarehouseItem warehouseBlanket = _warehouseItemService.FindOrCreateObject(barringOrder.WarehouseId, barring.BlanketItemId);
                StockMutation stockMutationBlanket = _stockMutationService.CreateStockMutationForBarringOrder(barringOrderDetail, warehouseBlanket, CaseAddition);
                StockMutateObject(stockMutationBlanket, _itemService, _barringService, _warehouseItemService);
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
                if (barringOrderDetail.IsBarRequired)
                {
                    Item leftbar = (barringOrderDetail.HasLeftBar && barring.LeftBarItemId != null) ? _itemService.GetObjectById((int)barring.LeftBarItemId) : null;
                    Item rightbar = (barringOrderDetail.HasRightBar && barring.RightBarItemId != null) ? _itemService.GetObjectById((int)barring.RightBarItemId) : null;
                    if (leftbar != null)
                    {
                        WarehouseItem warehouseLeftBar = _warehouseItemService.FindOrCreateObject(barringOrder.WarehouseId, leftbar.Id);
                        IList<StockMutation> stockMutationLeftBars = _stockMutationService.SoftDeleteStockMutationForBarringOrder(barringOrderDetail, warehouseLeftBar);
                        foreach (var stockMutationLeftBar in stockMutationLeftBars)
                        {
                            ReverseStockMutateObject(stockMutationLeftBar, _itemService, _barringService, _warehouseItemService);
                        }
                    }
                    if (rightbar != null)
                    {
                        WarehouseItem warehouseRightBar = _warehouseItemService.FindOrCreateObject(barringOrder.WarehouseId, rightbar.Id);
                        IList<StockMutation> stockMutationRightBars = _stockMutationService.SoftDeleteStockMutationForBarringOrder(barringOrderDetail, warehouseRightBar);
                        foreach (var stockMutationRightBar in stockMutationRightBars)
                        {
                            ReverseStockMutateObject(stockMutationRightBar, _itemService, _barringService, _warehouseItemService);
                        }
                    }
                }

                WarehouseItem warehouseBlanket = _warehouseItemService.FindOrCreateObject(barringOrder.WarehouseId, barring.BlanketItemId);
                IList<StockMutation> stockMutationBlankets = _stockMutationService.SoftDeleteStockMutationForBarringOrder(barringOrderDetail, warehouseBlanket);
                foreach (var stockMutationBlanket in stockMutationBlankets)
                {
                    ReverseStockMutateObject(stockMutationBlanket, _itemService, _barringService, _warehouseItemService);
                }
            }
            return barringOrderDetail;
        }

        public BarringOrderDetail FinishObject(BarringOrderDetail barringOrderDetail, IBarringOrderService _barringOrderService, IStockMutationService _stockMutationService,
                                               IBarringService _barringService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
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
                StockMutateObject(stockMutation, _itemService, _barringService, _warehouseItemService);

                // deduce bars quantity
                if (barringOrderDetail.IsBarRequired)
                {
                    Item leftbar = (barringOrderDetail.HasLeftBar && barring.LeftBarItemId != null) ? _itemService.GetObjectById((int)barring.LeftBarItemId) : null;
                    Item rightbar = (barringOrderDetail.HasRightBar && barring.RightBarItemId != null) ? _itemService.GetObjectById((int)barring.RightBarItemId) : null;
                    if (leftbar != null)
                    {
                        WarehouseItem warehouseLeftBar = _warehouseItemService.FindOrCreateObject(barringOrder.WarehouseId, leftbar.Id);
                        StockMutation stockMutationLeftBar = _stockMutationService.CreateStockMutationForBarringOrder(barringOrderDetail, warehouseLeftBar, CaseAdditionElse);
                        StockMutateObject(stockMutationLeftBar, _itemService, _barringService, _warehouseItemService);
                    }
                    if (rightbar != null)
                    {
                        WarehouseItem warehouseRightBar = _warehouseItemService.FindOrCreateObject(barringOrder.WarehouseId, rightbar.Id);
                        StockMutation stockMutationRightBar = _stockMutationService.CreateStockMutationForBarringOrder(barringOrderDetail, warehouseRightBar, CaseAdditionElse);
                        StockMutateObject(stockMutationRightBar, _itemService, _barringService, _warehouseItemService);
                    }
                }

                // deduce blanket quantity
                WarehouseItem warehouseBlanket = _warehouseItemService.FindOrCreateObject(barringOrder.WarehouseId, barring.BlanketItemId);
                StockMutation stockMutationBlanket = _stockMutationService.CreateStockMutationForBarringOrder(barringOrderDetail, warehouseBlanket, CaseAdditionElse);
                StockMutateObject(stockMutationBlanket, _itemService, _barringService, _warehouseItemService);
            }
            return barringOrderDetail;
        }

        public BarringOrderDetail UnfinishObject(BarringOrderDetail barringOrderDetail, IBarringOrderService _barringOrderService, IStockMutationService _stockMutationService,
                                                 IBarringService _barringService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidUnfinishObject(barringOrderDetail, _barringOrderService))
            {
                _repository.FinishObject(barringOrderDetail);
                // deduce barring order quantity final
                BarringOrder barringOrder = _barringOrderService.GetObjectById(barringOrderDetail.BarringOrderId);
                barringOrder.QuantityFinal -= 1;
                _barringOrderService.AdjustQuantity(barringOrder);

                // reverse stock mutation FinishObject
                Barring barring = _barringService.GetObjectById(barringOrderDetail.BarringId);
                WarehouseItem warehouseBarring = _warehouseItemService.FindOrCreateObject(barringOrder.WarehouseId, barring.Id);
                IList<StockMutation> stockMutations = _stockMutationService.SoftDeleteStockMutationForBarringOrder(barringOrderDetail, warehouseBarring);
                foreach (var stockMutation in stockMutations)
                {
                    ReverseStockMutateObject(stockMutation, _itemService, _barringService, _warehouseItemService);
                }

                if (barringOrderDetail.IsBarRequired)
                {
                    Item leftbar = (barringOrderDetail.HasLeftBar && barring.LeftBarItemId != null) ? _itemService.GetObjectById((int)barring.LeftBarItemId) : null;
                    Item rightbar = (barringOrderDetail.HasRightBar && barring.RightBarItemId != null) ? _itemService.GetObjectById((int)barring.RightBarItemId) : null;
                    if (leftbar != null)
                    {
                        WarehouseItem warehouseLeftBar = _warehouseItemService.FindOrCreateObject(barringOrder.WarehouseId, leftbar.Id);
                        IList<StockMutation> stockMutationLeftBars = _stockMutationService.SoftDeleteStockMutationForBarringOrder(barringOrderDetail, warehouseLeftBar);
                        foreach (var stockMutationLeftBar in stockMutationLeftBars)
                        {
                            ReverseStockMutateObject(stockMutationLeftBar, _itemService, _barringService, _warehouseItemService);
                        }
                    }
                    if (rightbar != null)
                    {
                        WarehouseItem warehouseRightBar = _warehouseItemService.FindOrCreateObject(barringOrder.WarehouseId, rightbar.Id);
                        IList<StockMutation> stockMutationRightBars = _stockMutationService.SoftDeleteStockMutationForBarringOrder(barringOrderDetail, warehouseRightBar);
                        foreach (var stockMutationRightBar in stockMutationRightBars)
                        {
                            ReverseStockMutateObject(stockMutationRightBar, _itemService, _barringService, _warehouseItemService);
                        }
                    }
                }

                WarehouseItem warehouseBlanket = _warehouseItemService.FindOrCreateObject(barringOrder.WarehouseId, barring.BlanketItemId);
                IList<StockMutation> stockMutationBlankets = _stockMutationService.SoftDeleteStockMutationForBarringOrder(barringOrderDetail, warehouseBlanket);
                foreach (var stockMutationBlanket in stockMutationBlankets)
                {
                    ReverseStockMutateObject(stockMutationBlanket, _itemService, _barringService, _warehouseItemService);
                }
            }
            return barringOrderDetail;
        }

        public void StockMutateObject(StockMutation stockMutation, IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            int Quantity = (stockMutation.Status == Constant.StockMutationStatus.Addition) ? stockMutation.Quantity : (-1) * stockMutation.Quantity;
            WarehouseItem warehouseItem = _warehouseItemService.GetObjectById(stockMutation.WarehouseItemId);
            Item item = _itemService.GetObjectById(warehouseItem.ItemId);
            Barring barring = _barringService.GetObjectById(warehouseItem.ItemId);
            if (barring == null)
            {
                _itemService.AdjustQuantity(item, Quantity);
            }
            else
            {
                _barringService.AdjustQuantity(barring, Quantity);
            }
            _warehouseItemService.AdjustQuantity(warehouseItem, Quantity);
        }

        public void ReverseStockMutateObject(StockMutation stockMutation, IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            int Quantity = (stockMutation.Status == Constant.StockMutationStatus.Deduction) ? stockMutation.Quantity : (-1) * stockMutation.Quantity;
            WarehouseItem warehouseItem = _warehouseItemService.GetObjectById(stockMutation.WarehouseItemId);
            Item item = _itemService.GetObjectById(warehouseItem.ItemId);
            Barring barring = _barringService.GetObjectById(warehouseItem.ItemId);
            if (barring == null)
            {
                _itemService.AdjustQuantity(item, Quantity);
            }
            else
            {
                _barringService.AdjustQuantity(barring, Quantity);
            }
            _warehouseItemService.AdjustQuantity(warehouseItem, Quantity);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

    }
}