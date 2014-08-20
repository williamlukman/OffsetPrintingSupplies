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

        public IQueryable<RecoveryOrderDetail> GetQueryable()
        {
            return _repository.GetQueryable();
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

        public RecoveryOrderDetail DisassembleObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService)
        {
            return (recoveryOrderDetail = _validator.ValidDisassembleObject(recoveryOrderDetail, _recoveryOrderService) ? _repository.DisassembleObject(recoveryOrderDetail) : recoveryOrderDetail);
        }

        public RecoveryOrderDetail StripAndGlueObject(RecoveryOrderDetail recoveryOrderDetail)
        {
            return (recoveryOrderDetail = _validator.ValidStripAndGlueObject(recoveryOrderDetail) ? _repository.StripAndGlueObject(recoveryOrderDetail) : recoveryOrderDetail);
        }

        public RecoveryOrderDetail WrapObject(RecoveryOrderDetail recoveryOrderDetail, int CompoundUsage, IRecoveryOrderService _recoveryOrderService,
                                              IRollerBuilderService _rollerBuilderService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            recoveryOrderDetail.CompoundUsage = CompoundUsage;
            return (recoveryOrderDetail = _validator.ValidWrapObject(recoveryOrderDetail, _recoveryOrderService, _rollerBuilderService, _itemService, _warehouseItemService) ? _repository.WrapObject(recoveryOrderDetail) : recoveryOrderDetail);
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

        public RecoveryOrderDetail RejectObject(RecoveryOrderDetail recoveryOrderDetail, DateTime RejectedDate, ICoreIdentificationService _coreIdentificationService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                                IRecoveryOrderService _recoveryOrderService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService, ICoreBuilderService _coreBuilderService, IRollerBuilderService _rollerBuilderService,
                                                IItemService _itemService, IWarehouseItemService _warehouseItemService, IBarringService _barringService, IStockMutationService _stockMutationService)
        {
            recoveryOrderDetail.RejectedDate = RejectedDate;
            if (_validator.ValidRejectObject(recoveryOrderDetail, _recoveryOrderService))
            {
                _repository.RejectObject(recoveryOrderDetail);

                // add recovery order quantity reject
                // if valid, complete recovery order = true 
                RecoveryOrder recoveryOrder = _recoveryOrderService.GetObjectById(recoveryOrderDetail.RecoveryOrderId);
                recoveryOrder.QuantityRejected += 1;
                _recoveryOrderService.AdjustQuantity(recoveryOrder);
                if (_recoveryOrderService.GetValidator().ValidCompleteObject(recoveryOrder, this, _recoveryAccessoryDetailService))
                {
                    _recoveryOrderService.CompleteObject(recoveryOrder, _coreIdentificationDetailService, this, _recoveryAccessoryDetailService);
                }

                bool CaseAddition = false;

                // deduce compound
                RollerBuilder rollerBuilder = _rollerBuilderService.GetObjectById(recoveryOrderDetail.RollerBuilderId);
                Item compound = _itemService.GetObjectById(rollerBuilder.CompoundId);
                WarehouseItem warehouseCompound = _warehouseItemService.FindOrCreateObject(recoveryOrder.WarehouseId, compound.Id);
                StockMutation stockMutationCompound = _stockMutationService.CreateStockMutationForRecoveryOrderCompound(recoveryOrderDetail, warehouseCompound, CaseAddition);
                _stockMutationService.StockMutateObject(stockMutationCompound, _itemService, _barringService, _warehouseItemService);

                // deduce core
                CoreIdentificationDetail coreIdentificationDetail = _coreIdentificationDetailService.GetObjectById(recoveryOrderDetail.CoreIdentificationDetailId);
                _coreIdentificationDetailService.UnsetJobScheduled(coreIdentificationDetail, _recoveryOrderService, this);

                CoreBuilder coreBuilder = _coreBuilderService.GetObjectById(coreIdentificationDetail.CoreBuilderId);
                Item core = (coreIdentificationDetail.MaterialCase == Core.Constants.Constant.MaterialCase.New) ?
                            _coreBuilderService.GetNewCore(coreBuilder.Id) : _coreBuilderService.GetUsedCore(coreBuilder.Id);
                WarehouseItem warehouseCore = _warehouseItemService.FindOrCreateObject(recoveryOrder.WarehouseId, core.Id);
                StockMutation stockMutationCore = _stockMutationService.CreateStockMutationForRecoveryOrder(recoveryOrderDetail, warehouseCore, CaseAddition);
                _stockMutationService.StockMutateObject(stockMutationCore, _itemService, _barringService, _warehouseItemService);
               

                // accesories uncounted
            }
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail UndoRejectObject(RecoveryOrderDetail recoveryOrderDetail, ICoreIdentificationService _coreIdentificationService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                                IRecoveryOrderService _recoveryOrderService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService, ICoreBuilderService _coreBuilderService, IRollerBuilderService _rollerBuilderService,
                                                IItemService _itemService, IWarehouseItemService _warehouseItemService, IBarringService _barringService, IStockMutationService _stockMutationService)
        {
            if (_validator.ValidUndoRejectObject(recoveryOrderDetail, _recoveryOrderService))
            {
                _repository.UndoRejectObject(recoveryOrderDetail);

                // add recovery order quantity reject
                // if valid, complete recovery order = true 
                RecoveryOrder recoveryOrder = _recoveryOrderService.GetObjectById(recoveryOrderDetail.RecoveryOrderId);
                recoveryOrder.QuantityRejected -= 1;
                _recoveryOrderService.AdjustQuantity(recoveryOrder);

                // reverse stock mutate compound
                RollerBuilder rollerBuilder = _rollerBuilderService.GetObjectById(recoveryOrderDetail.RollerBuilderId);
                Item compound = _itemService.GetObjectById(rollerBuilder.CompoundId);
                WarehouseItem warehouseCompound = _warehouseItemService.FindOrCreateObject(recoveryOrder.WarehouseId, compound.Id);
                IList<StockMutation> stockMutationCompounds = _stockMutationService.SoftDeleteStockMutationForRecoveryOrder(recoveryOrderDetail, warehouseCompound);
                foreach (var stockMutationCompound in stockMutationCompounds)
                {
                    _stockMutationService.ReverseStockMutateObject(stockMutationCompound, _itemService, _barringService, _warehouseItemService);
                }

                // reverse stock mutate core
                CoreIdentificationDetail coreIdentificationDetail = _coreIdentificationDetailService.GetObjectById(recoveryOrderDetail.CoreIdentificationDetailId);
                _coreIdentificationDetailService.SetJobScheduled(coreIdentificationDetail, _recoveryOrderService, this);

                CoreBuilder coreBuilder = _coreBuilderService.GetObjectById(coreIdentificationDetail.CoreBuilderId);
                Item core = (coreIdentificationDetail.MaterialCase == Core.Constants.Constant.MaterialCase.New) ?
                            _coreBuilderService.GetNewCore(coreBuilder.Id) : _coreBuilderService.GetUsedCore(coreBuilder.Id);
                WarehouseItem warehouseCore = _warehouseItemService.FindOrCreateObject(recoveryOrder.WarehouseId, core.Id);
                
                IList<StockMutation> stockMutationCores = _stockMutationService.SoftDeleteStockMutationForRecoveryOrder(recoveryOrderDetail, warehouseCore);
                foreach (var stockMutationCore in stockMutationCores)
                {
                    _stockMutationService.ReverseStockMutateObject(stockMutationCore, _itemService, _barringService, _warehouseItemService);
                }

                // accesories uncounted
            }
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail FinishObject(RecoveryOrderDetail recoveryOrderDetail, DateTime FinishedDate, ICoreIdentificationService _coreIdentificationService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                                IRecoveryOrderService _recoveryOrderService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService, ICoreBuilderService _coreBuilderService, IRollerBuilderService _rollerBuilderService,
                                                IItemService _itemService, IWarehouseItemService _warehouseItemService, IBarringService _barringService, IStockMutationService _stockMutationService)
        {
            recoveryOrderDetail.FinishedDate = FinishedDate;
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

                bool CaseAdditionCompound = false;
                bool CaseAdditionCore = false;
                bool CaseAdditionRoller = true;
             
                // deduce compound
                RollerBuilder rollerBuilder = _rollerBuilderService.GetObjectById(recoveryOrderDetail.RollerBuilderId);
                Item compound = _itemService.GetObjectById(rollerBuilder.CompoundId);
                WarehouseItem warehouseCompound = _warehouseItemService.FindOrCreateObject(recoveryOrder.WarehouseId, compound.Id);
                StockMutation stockMutationCompound = _stockMutationService.CreateStockMutationForRecoveryOrderCompound(recoveryOrderDetail, warehouseCompound, CaseAdditionCompound);
                _stockMutationService.StockMutateObject(stockMutationCompound, _itemService, _barringService, _warehouseItemService);

                CoreIdentificationDetail coreIdentificationDetail = _coreIdentificationDetailService.GetObjectById(recoveryOrderDetail.CoreIdentificationDetailId);
                _coreIdentificationDetailService.UnsetJobScheduled(coreIdentificationDetail, _recoveryOrderService, this);
                _coreIdentificationDetailService.BuildRoller(coreIdentificationDetail);

                // deduce core
                CoreBuilder coreBuilder = _coreBuilderService.GetObjectById(coreIdentificationDetail.CoreBuilderId);
                Item core = (coreIdentificationDetail.MaterialCase == Core.Constants.Constant.MaterialCase.New) ?
                            _coreBuilderService.GetNewCore(coreBuilder.Id) : _coreBuilderService.GetUsedCore(coreBuilder.Id);
                WarehouseItem warehouseCore = _warehouseItemService.FindOrCreateObject(recoveryOrder.WarehouseId, core.Id);
                StockMutation stockMutationCore = _stockMutationService.CreateStockMutationForRecoveryOrder(recoveryOrderDetail, warehouseCore, CaseAdditionCore);
                _stockMutationService.StockMutateObject(stockMutationCore, _itemService, _barringService, _warehouseItemService);

                // add roller
                Item roller = (coreIdentificationDetail.MaterialCase == Core.Constants.Constant.MaterialCase.New) ?
                            _rollerBuilderService.GetRollerNewCore(rollerBuilder.Id) : _rollerBuilderService.GetRollerUsedCore(rollerBuilder.Id);
                WarehouseItem warehouseRoller = _warehouseItemService.FindOrCreateObject(recoveryOrder.WarehouseId, roller.Id);
                StockMutation stockMutationRoller = _stockMutationService.CreateStockMutationForRecoveryOrder(recoveryOrderDetail, warehouseRoller, CaseAdditionRoller);
                _stockMutationService.StockMutateObject(stockMutationRoller, _itemService, _barringService, _warehouseItemService);

                // deduce accessories
                IList<RecoveryAccessoryDetail> recoveryAccessoryDetails = _recoveryAccessoryDetailService.GetObjectsByRecoveryOrderDetailId(recoveryOrderDetail.Id);
                if (recoveryAccessoryDetails.Any())
                {
                    foreach (var recoveryAccessoryDetail in recoveryAccessoryDetails)
                    {
                        Item accessory = _itemService.GetObjectById(recoveryAccessoryDetail.ItemId);
                        WarehouseItem warehouseAccessory = _warehouseItemService.FindOrCreateObject(recoveryOrder.WarehouseId, accessory.Id);
                        StockMutation stockMutationAccessory = _stockMutationService.CreateStockMutationForRecoveryAccessory(recoveryAccessoryDetail, warehouseAccessory);
                        _stockMutationService.StockMutateObject(stockMutationAccessory, _itemService, _barringService, _warehouseItemService);
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

                // reverse stock mutate compound
                RollerBuilder rollerBuilder = _rollerBuilderService.GetObjectById(recoveryOrderDetail.RollerBuilderId);
                Item compound = _itemService.GetObjectById(rollerBuilder.CompoundId);
                WarehouseItem warehouseCompound = _warehouseItemService.FindOrCreateObject(recoveryOrder.WarehouseId, compound.Id);
                IList<StockMutation> stockMutationCompounds = _stockMutationService.SoftDeleteStockMutationForRecoveryOrder(recoveryOrderDetail, warehouseCompound);
                foreach (var stockMutationCompound in stockMutationCompounds)
                {
                    _stockMutationService.ReverseStockMutateObject(stockMutationCompound, _itemService, _barringService, _warehouseItemService);
                }
                
                // reverse stock mutate core
                CoreIdentificationDetail coreIdentificationDetail = _coreIdentificationDetailService.GetObjectById(recoveryOrderDetail.CoreIdentificationDetailId);
                _coreIdentificationDetailService.SetJobScheduled(coreIdentificationDetail, _recoveryOrderService, this);

                CoreBuilder coreBuilder = _coreBuilderService.GetObjectById(coreIdentificationDetail.CoreBuilderId);
                Item core = (coreIdentificationDetail.MaterialCase == Core.Constants.Constant.MaterialCase.New) ?
                            _coreBuilderService.GetNewCore(coreBuilder.Id) : _coreBuilderService.GetUsedCore(coreBuilder.Id);
                WarehouseItem warehouseCore = _warehouseItemService.FindOrCreateObject(recoveryOrder.WarehouseId, core.Id);
                IList<StockMutation> stockMutationCores = _stockMutationService.SoftDeleteStockMutationForRecoveryOrder(recoveryOrderDetail, warehouseCore);
                foreach (var stockMutationCore in stockMutationCores)
                {
                    _stockMutationService.ReverseStockMutateObject(stockMutationCore, _itemService, _barringService, _warehouseItemService);
                }

                // reverse stock mutate roller
                Item roller = (coreIdentificationDetail.MaterialCase == Core.Constants.Constant.MaterialCase.New) ?
                            _rollerBuilderService.GetRollerNewCore(rollerBuilder.Id) : _rollerBuilderService.GetRollerUsedCore(rollerBuilder.Id);
                WarehouseItem warehouseRoller = _warehouseItemService.FindOrCreateObject(recoveryOrder.WarehouseId, roller.Id);
                IList<StockMutation> stockMutationRollers = _stockMutationService.SoftDeleteStockMutationForRecoveryOrder(recoveryOrderDetail, warehouseRoller);
                foreach (var stockMutationRoller in stockMutationRollers)
                {
                    _stockMutationService.ReverseStockMutateObject(stockMutationRoller, _itemService, _barringService, _warehouseItemService);
                }

                // reverse stock mutate accessories
                IList<RecoveryAccessoryDetail> recoveryAccessoryDetails = _recoveryAccessoryDetailService.GetObjectsByRecoveryOrderDetailId(recoveryOrderDetail.Id);
                if (recoveryAccessoryDetails.Any())
                {
                    foreach (var recoveryAccessoryDetail in recoveryAccessoryDetails)
                    {
                        Item accessory = _itemService.GetObjectById(recoveryAccessoryDetail.ItemId);
                        WarehouseItem warehouseAccessory = _warehouseItemService.FindOrCreateObject(recoveryOrder.WarehouseId, accessory.Id);
                        IList<StockMutation> stockMutationAccessories = _stockMutationService.SoftDeleteStockMutationForRecoveryAccessory(recoveryAccessoryDetail, warehouseAccessory);
                        foreach (var stockMutationAccessory in stockMutationAccessories)
                        {
                            _stockMutationService.ReverseStockMutateObject(stockMutationAccessory, _itemService, _barringService, _warehouseItemService);
                        }
                    }
                }
            }
            return recoveryOrderDetail;
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }
    }
}