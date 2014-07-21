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
    public class RecoveryOrderDetailService : IRecoveryOrderDetailService
    {
        private IRecoveryOrderDetailRepository _repository;
        private IRecoveryOrderDetailValidator _validator;
        public RecoveryOrderDetailService(IRecoveryOrderDetailRepository _recoveryOrderDetailRepository, IRecoveryOrderDetailValidator _recoveryOrderDetailValidator)
        {
            _repository = _recoveryOrderDetailRepository;
            _validator = _recoveryOrderDetailValidator;
        }

        public IRecoveryOrderDetailValidator GetValidator()
        {
            return _validator;
        }

        public IRecoveryOrderDetailRepository GetRepository()
        {
            return _repository;
        }

        public IList<RecoveryOrderDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<RecoveryOrderDetail> GetObjectsByRecoveryOrderId(int recoveryOrderId)
        {
            return _repository.GetObjectsByRecoveryOrderId(recoveryOrderId);
        }

        public IList<RecoveryOrderDetail> GetObjectsByCoreIdentificationDetailId(int coreIdentificationDetailId)
        {
            return _repository.GetObjectsByCoreIdentificationDetailId(coreIdentificationDetailId);
        }

        public IList<RecoveryOrderDetail> GetObjectsByRollerBuilderId(int rollerBuilderId)
        {
            return _repository.GetObjectsByRollerBuilderId(rollerBuilderId);
        }

        public RecoveryOrderDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public Item GetCore(RecoveryOrderDetail recoveryOrderDetail, ICoreIdentificationDetailService _coreIdentificationDetailService,
                            ICoreBuilderService _coreBuilderService, IItemService _itemService)
        {
            CoreIdentificationDetail coreIdentificationDetail = _coreIdentificationDetailService.GetObjectById(recoveryOrderDetail.CoreIdentificationDetailId);
            Item core = (coreIdentificationDetail.MaterialCase == Core.Constants.Constant.MaterialCase.New) ?
                          _coreBuilderService.GetNewCore(coreIdentificationDetail.CoreBuilderId) : _coreBuilderService.GetUsedCore(coreIdentificationDetail.CoreBuilderId);
            return core;
        }

        public Item GetRoller(RecoveryOrderDetail recoveryOrderDetail, ICoreIdentificationDetailService _coreIdentificationDetailService,
                              IRollerBuilderService _rollerBuilderService, IItemService _itemService)
        {
            CoreIdentificationDetail coreIdentificationDetail = _coreIdentificationDetailService.GetObjectById(recoveryOrderDetail.CoreIdentificationDetailId);
            Item roller = (coreIdentificationDetail.MaterialCase == Core.Constants.Constant.MaterialCase.New) ?
                          _rollerBuilderService.GetRollerNewCore(recoveryOrderDetail.RollerBuilderId) : _rollerBuilderService.GetRollerUsedCore(recoveryOrderDetail.RollerBuilderId);
            return roller;
        }

        public RecoveryOrderDetail CreateObject(int CoreIdentificationDetailId, int RollerBuilderId, string CoreTypeCase, string Acc, int RepairRequestCase,
                                                IRecoveryOrderService _recoveryOrderService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                                IRollerBuilderService _rollerBuilderService)
        {
            RecoveryOrderDetail recoveryOrderDetail = new RecoveryOrderDetail
            {
                CoreIdentificationDetailId = CoreIdentificationDetailId,
                RollerBuilderId = RollerBuilderId,
                CoreTypeCase = CoreTypeCase,
                Acc = Acc,
                RepairRequestCase = RepairRequestCase
            };
            return this.CreateObject(recoveryOrderDetail, _recoveryOrderService, _coreIdentificationDetailService, _rollerBuilderService);
        }

        public RecoveryOrderDetail CreateObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService,
                                                ICoreIdentificationDetailService _coreIdentificationDetailService, IRollerBuilderService _rollerBuilderService)
        {
            recoveryOrderDetail.Errors = new Dictionary<String, String>();
            return (recoveryOrderDetail = _validator.ValidCreateObject(recoveryOrderDetail, _recoveryOrderService, _coreIdentificationDetailService, _rollerBuilderService) ?
                                          _repository.CreateObject(recoveryOrderDetail) : recoveryOrderDetail);
        }

        public RecoveryOrderDetail UpdateObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService,
                                                ICoreIdentificationDetailService _coreIdentificationDetailService, IRollerBuilderService _rollerBuilderService)
        {
            return (recoveryOrderDetail = _validator.ValidUpdateObject(recoveryOrderDetail, _recoveryOrderService, _coreIdentificationDetailService, _rollerBuilderService) ?
                                          _repository.UpdateObject(recoveryOrderDetail) : recoveryOrderDetail);
        }

        public RecoveryOrderDetail SoftDeleteObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService)
        {
            return (recoveryOrderDetail = _validator.ValidDeleteObject(recoveryOrderDetail, _recoveryOrderService, _recoveryAccessoryDetailService) ?
                                          _repository.SoftDeleteObject(recoveryOrderDetail) : recoveryOrderDetail);
        }

        public RecoveryOrderDetail AddAccessory(RecoveryOrderDetail recoveryOrderDetail, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService)
        {
            return (recoveryOrderDetail = _validator.ValidAddAccessory(recoveryOrderDetail, _recoveryAccessoryDetailService) ? _repository.AddAccessory(recoveryOrderDetail) : recoveryOrderDetail);
        }

        public RecoveryOrderDetail RemoveAccessory(RecoveryOrderDetail recoveryOrderDetail, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService)
        {
            return (recoveryOrderDetail = _validator.ValidRemoveAccessory(recoveryOrderDetail, _recoveryAccessoryDetailService) ? _repository.RemoveAccessory(recoveryOrderDetail) : recoveryOrderDetail);
        }

        public RecoveryOrderDetail DisassembleObject(RecoveryOrderDetail recoveryOrderDetail)
        {
            return (recoveryOrderDetail = _validator.ValidDisassembleObject(recoveryOrderDetail) ? _repository.DisassembleObject(recoveryOrderDetail) : recoveryOrderDetail);
        }

        public RecoveryOrderDetail StripAndGlueObject(RecoveryOrderDetail recoveryOrderDetail)
        {
            return (recoveryOrderDetail = _validator.ValidStripAndGlueObject(recoveryOrderDetail) ? _repository.StripAndGlueObject(recoveryOrderDetail) : recoveryOrderDetail);
        }

        public RecoveryOrderDetail WrapObject(RecoveryOrderDetail recoveryOrderDetail)
        {
            return (recoveryOrderDetail = _validator.ValidWrapObject(recoveryOrderDetail) ? _repository.WrapObject(recoveryOrderDetail) : recoveryOrderDetail);
        }

        public RecoveryOrderDetail VulcanizeObject(RecoveryOrderDetail recoveryOrderDetail)
        {
            return (recoveryOrderDetail = _validator.ValidVulcanizeObject(recoveryOrderDetail) ? _repository.VulcanizeObject(recoveryOrderDetail) : recoveryOrderDetail);
        }

        public RecoveryOrderDetail FaceOffObject(RecoveryOrderDetail recoveryOrderDetail)
        {
            return (recoveryOrderDetail = _validator.ValidFaceOffObject(recoveryOrderDetail) ? _repository.FaceOffObject(recoveryOrderDetail) : recoveryOrderDetail);
        }

        public RecoveryOrderDetail ConventionalGrindObject(RecoveryOrderDetail recoveryOrderDetail)
        {
            return (recoveryOrderDetail = _validator.ValidConventionalGrindObject(recoveryOrderDetail) ? _repository.ConventionalGrindObject(recoveryOrderDetail) : recoveryOrderDetail);
        }

        public RecoveryOrderDetail CWCGrindObject(RecoveryOrderDetail recoveryOrderDetail)
        {
            return (recoveryOrderDetail = _validator.ValidCWCGrindObject(recoveryOrderDetail) ? _repository.CWCGrindObject(recoveryOrderDetail) : recoveryOrderDetail);
        }

        public RecoveryOrderDetail PolishAndQCObject(RecoveryOrderDetail recoveryOrderDetail)
        {
            return (recoveryOrderDetail = _validator.ValidPolishAndQCObject(recoveryOrderDetail) ? _repository.PolishAndQCObject(recoveryOrderDetail) : recoveryOrderDetail);
        }

        public RecoveryOrderDetail PackageObject(RecoveryOrderDetail recoveryOrderDetail)
        {
            return (recoveryOrderDetail = _validator.ValidPackageObject(recoveryOrderDetail) ? _repository.PackageObject(recoveryOrderDetail) : recoveryOrderDetail);
        }

        public RecoveryOrderDetail RejectObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService)
        {
            return (recoveryOrderDetail = _validator.ValidRejectObject(recoveryOrderDetail, _recoveryOrderService) ?
                                          _repository.RejectObject(recoveryOrderDetail) : recoveryOrderDetail);
        }

        public RecoveryOrderDetail UndoRejectObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService)
        {
            return (recoveryOrderDetail = _validator.ValidUndoRejectObject(recoveryOrderDetail, _recoveryOrderService) ?
                                          _repository.UndoRejectObject(recoveryOrderDetail) : recoveryOrderDetail);
        }

        public RecoveryOrderDetail FinishObject(RecoveryOrderDetail recoveryOrderDetail, ICoreIdentificationService _coreIdentificationService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                                IRecoveryOrderService _recoveryOrderService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService, ICoreBuilderService _coreBuilderService, IRollerBuilderService _rollerBuilderService,
                                                IItemService _itemService, IWarehouseItemService _warehouseItemService, IBarringService _barringService, IStockMutationService _stockMutationService)
        {
            if (_validator.ValidFinishObject(recoveryOrderDetail, _recoveryOrderService, _recoveryAccessoryDetailService))
            {
                // set object to finish
                _repository.FinishObject(recoveryOrderDetail);

                // add recovery order quantity final
                // if valid, complete recovery order = true 
                RecoveryOrder recoveryOrder = _recoveryOrderService.GetObjectById(recoveryOrderDetail.RecoveryOrderId);
                recoveryOrder.QuantityFinal += 1;
                _recoveryOrderService.AdjustQuantity(recoveryOrder);
                if (_recoveryOrderService.GetValidator().ValidCompleteObject(recoveryOrder, this, _recoveryAccessoryDetailService))
                {
                    _recoveryOrderService.CompleteObject(recoveryOrder, _coreIdentificationDetailService, this, _recoveryAccessoryDetailService);
                }

                // deduce compound
                // TODO

                // deduce core
                CoreIdentificationDetail coreIdentificationDetail = _coreIdentificationDetailService.GetObjectById(recoveryOrderDetail.CoreIdentificationDetailId);
                _coreIdentificationDetailService.UnsetJobScheduled(coreIdentificationDetail, _recoveryOrderService, this);
                _coreIdentificationDetailService.FinishObject(coreIdentificationDetail, _coreIdentificationService, _coreBuilderService, _stockMutationService,
                                                              _itemService, _barringService, _warehouseItemService);

                CoreBuilder coreBuilder = _coreBuilderService.GetObjectById(coreIdentificationDetail.CoreBuilderId);
                Item core = (coreIdentificationDetail.MaterialCase == Core.Constants.Constant.MaterialCase.New) ?
                            _coreBuilderService.GetNewCore(coreBuilder.Id) : _coreBuilderService.GetUsedCore(coreBuilder.Id);
                WarehouseItem warehouseCore = _warehouseItemService.GetObjectByWarehouseAndItem(recoveryOrder.WarehouseId, core.Id);
                StockMutation stockMutationCore = _stockMutationService.CreateStockMutationForRecoveryOrder(recoveryOrderDetail, warehouseCore);
                StockMutateObject(stockMutationCore, _itemService, _barringService, _warehouseItemService);

                // add roller
                RollerBuilder rollerBuilder = _rollerBuilderService.GetObjectById(recoveryOrderDetail.RollerBuilderId);
                Item roller = (coreIdentificationDetail.MaterialCase == Core.Constants.Constant.MaterialCase.New) ?
                            _rollerBuilderService.GetRollerNewCore(rollerBuilder.Id) : _rollerBuilderService.GetRollerUsedCore(rollerBuilder.Id);
                WarehouseItem warehouseRoller = _warehouseItemService.GetObjectByWarehouseAndItem(recoveryOrder.WarehouseId, roller.Id);
                StockMutation stockMutationRoller = _stockMutationService.CreateStockMutationForRecoveryOrder(recoveryOrderDetail, warehouseRoller);
                StockMutateObject(stockMutationRoller, _itemService, _barringService, _warehouseItemService);

                // deduce accessories
                IList<RecoveryAccessoryDetail> recoveryAccessoryDetails = _recoveryAccessoryDetailService.GetObjectsByRecoveryOrderDetailId(recoveryOrderDetail.Id);
                if (recoveryAccessoryDetails.Any())
                {
                    foreach (var recoveryAccessoryDetail in recoveryAccessoryDetails)
                    {
                        Item accessory = _itemService.GetObjectById(recoveryAccessoryDetail.ItemId);
                        WarehouseItem warehouseAccessory = _warehouseItemService.GetObjectByWarehouseAndItem(recoveryOrder.WarehouseId, accessory.Id);
                        StockMutation stockMutationAccessory = _stockMutationService.CreateStockMutationForRecoveryAccessory(recoveryAccessoryDetail, warehouseAccessory);
                        StockMutateObject(stockMutationAccessory, _itemService, _barringService, _warehouseItemService);
                    }
                }
            }
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail UnfinishObject(RecoveryOrderDetail recoveryOrderDetail, ICoreIdentificationService _coreIdentificationService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                                IRecoveryOrderService _recoveryOrderService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService, ICoreBuilderService _coreBuilderService, IRollerBuilderService _rollerBuilderService,
                                                IItemService _itemService, IWarehouseItemService _warehouseItemService, IBarringService _barringService, IStockMutationService _stockMutationService)
        {
            if (_validator.ValidUnfinishObject(recoveryOrderDetail, _recoveryOrderService, _recoveryAccessoryDetailService))
            {
                // unfinish object
                _repository.UnfinishObject(recoveryOrderDetail);

                // add recovery order quantity final
                RecoveryOrder recoveryOrder = _recoveryOrderService.GetObjectById(recoveryOrderDetail.RecoveryOrderId);
                recoveryOrder.QuantityFinal += 1;
                _recoveryOrderService.AdjustQuantity(recoveryOrder);
                
                // add compound
                // TODO
                    
                // add core
                CoreIdentificationDetail coreIdentificationDetail = _coreIdentificationDetailService.GetObjectById(recoveryOrderDetail.CoreIdentificationDetailId);
                _coreIdentificationDetailService.SetJobScheduled(coreIdentificationDetail, _recoveryOrderService, this);
                _coreIdentificationDetailService.UnfinishObject(coreIdentificationDetail, _coreIdentificationService, _coreBuilderService,
                                                                _stockMutationService, _itemService, _barringService, _warehouseItemService);
                CoreBuilder coreBuilder = _coreBuilderService.GetObjectById(coreIdentificationDetail.CoreBuilderId);
                Item core = (coreIdentificationDetail.MaterialCase == Core.Constants.Constant.MaterialCase.New) ?
                            _coreBuilderService.GetNewCore(coreBuilder.Id) : _coreBuilderService.GetUsedCore(coreBuilder.Id);
                WarehouseItem warehouseCore = _warehouseItemService.GetObjectByWarehouseAndItem(recoveryOrder.WarehouseId, core.Id);
                IList<StockMutation> stockMutationCores = _stockMutationService.SoftDeleteStockMutationForRecoveryOrder(recoveryOrderDetail, warehouseCore);
                foreach (var stockMutationCore in stockMutationCores)
                {
                    ReverseStockMutateObject(stockMutationCore, _itemService, _barringService, _warehouseItemService);
                }

                // deduce roller
                RollerBuilder rollerBuilder = _rollerBuilderService.GetObjectById(recoveryOrderDetail.RollerBuilderId);
                Item roller = (coreIdentificationDetail.MaterialCase == Core.Constants.Constant.MaterialCase.New) ?
                            _rollerBuilderService.GetRollerNewCore(rollerBuilder.Id) : _rollerBuilderService.GetRollerUsedCore(rollerBuilder.Id);
                WarehouseItem warehouseRoller = _warehouseItemService.GetObjectByWarehouseAndItem(recoveryOrder.WarehouseId, roller.Id);
                IList<StockMutation> stockMutationRollers = _stockMutationService.SoftDeleteStockMutationForRecoveryOrder(recoveryOrderDetail, warehouseRoller);
                foreach (var stockMutationRoller in stockMutationRollers)
                {
                    ReverseStockMutateObject(stockMutationRoller, _itemService, _barringService, _warehouseItemService);
                }

                // add accessories
                IList<RecoveryAccessoryDetail> recoveryAccessoryDetails = _recoveryAccessoryDetailService.GetObjectsByRecoveryOrderDetailId(recoveryOrderDetail.Id);
                if (recoveryAccessoryDetails.Any())
                {
                    foreach (var recoveryAccessoryDetail in recoveryAccessoryDetails)
                    {
                        Item accessory = _itemService.GetObjectById(recoveryAccessoryDetail.ItemId);
                        WarehouseItem warehouseAccessory = _warehouseItemService.GetObjectByWarehouseAndItem(recoveryOrder.WarehouseId, accessory.Id);
                        IList<StockMutation> stockMutationAccessories = _stockMutationService.SoftDeleteStockMutationForRecoveryAccessory(recoveryAccessoryDetail, warehouseAccessory);
                        foreach (var stockMutationAccessory in stockMutationAccessories)
                        {
                            ReverseStockMutateObject(stockMutationAccessory, _itemService, _barringService, _warehouseItemService);
                        }
                    }
                }
            }
            return recoveryOrderDetail;
        }

        public void StockMutateObject(StockMutation stockMutation, IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            int Quantity = (stockMutation.Status == Constant.StockMutationStatus.Addition) ? stockMutation.Quantity : (-1) * stockMutation.Quantity;
            WarehouseItem warehouseItem = _warehouseItemService.GetObjectById(stockMutation.WarehouseItemId);
            Item item = _itemService.GetObjectById(warehouseItem.ItemId);
            _itemService.AdjustQuantity(item, Quantity);
            _warehouseItemService.AdjustQuantity(warehouseItem, Quantity);
        }

        public void ReverseStockMutateObject(StockMutation stockMutation, IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            int Quantity = (stockMutation.Status == Constant.StockMutationStatus.Deduction) ? stockMutation.Quantity : (-1) * stockMutation.Quantity;
            WarehouseItem warehouseItem = _warehouseItemService.GetObjectById(stockMutation.WarehouseItemId);
            Item item = _itemService.GetObjectById(warehouseItem.ItemId);
            _itemService.AdjustQuantity(item, Quantity);
            _warehouseItemService.AdjustQuantity(warehouseItem, Quantity);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }
    }
}