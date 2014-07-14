using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class RecoveryOrderService : IRecoveryOrderService
    {
        private IRecoveryOrderRepository _repository;
        private IRecoveryOrderValidator _validator;
        public RecoveryOrderService(IRecoveryOrderRepository _recoveryOrderRepository, IRecoveryOrderValidator _recoveryOrderValidator)
        {
            _repository = _recoveryOrderRepository;
            _validator = _recoveryOrderValidator;
        }

        public IRecoveryOrderValidator GetValidator()
        {
            return _validator;
        }

        public IList<RecoveryOrder> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<RecoveryOrder> GetAllObjectsInHouse()
        {
            return _repository.GetAllObjectsInHouse();
        }

        public IList<RecoveryOrder> GetAllObjectsByCustomerId(int CustomerId)
        {
            return _repository.GetAllObjectsByCustomerId(CustomerId);
        }

        public IList<RecoveryOrder> GetObjectsByCoreIdentificationId(int coreIdentificationId)
        {
            return _repository.GetObjectsByCoreIdentificationId(coreIdentificationId);
        }

        public RecoveryOrder GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public RecoveryOrder CreateObject(int CoreIdentificationId, string Code, int QuantityReceived, ICoreIdentificationService _coreIdentificationService)
        {
            RecoveryOrder recoveryOrder = new RecoveryOrder
            {
                CoreIdentificationId = CoreIdentificationId,
                Code = Code,
                QuantityReceived = QuantityReceived
            };
            return this.CreateObject(recoveryOrder, _coreIdentificationService);
        }

        public RecoveryOrder CreateObject(RecoveryOrder recoveryOrder, ICoreIdentificationService _coreIdentificationService)
        {
            recoveryOrder.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(recoveryOrder, _coreIdentificationService, this) ? _repository.CreateObject(recoveryOrder) : recoveryOrder);
        }

        public RecoveryOrder UpdateObject(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService, ICoreIdentificationService _coreIdentificationService)
        {
            return (recoveryOrder = _validator.ValidUpdateObject(recoveryOrder, _recoveryOrderDetailService, _coreIdentificationService, this) ? _repository.UpdateObject(recoveryOrder) : recoveryOrder);
        }

        public RecoveryOrder SoftDeleteObject(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService)
        {
            if (_validator.ValidDeleteObject(recoveryOrder, _recoveryOrderDetailService, _recoveryAccessoryDetailService))
            {
                ICollection<RecoveryOrderDetail> details = _recoveryOrderDetailService.GetObjectsByRecoveryOrderId(recoveryOrder.Id);
                ICollection<RecoveryAccessoryDetail> accessories = new Collection<RecoveryAccessoryDetail>();
                foreach (var detail in details)
                {
                    // populate accessories
                    _recoveryAccessoryDetailService.GetObjectsByRecoveryOrderDetailId(detail.Id).ToList().ForEach(x => accessories.Add(x));
                    // delete details
                    _recoveryOrderDetailService.GetRepository().SoftDeleteObject(detail);
                }
                foreach (var accessory in accessories)
                {
                    // delete accessories
                    _recoveryAccessoryDetailService.GetRepository().SoftDeleteObject(accessory);
                }
                _repository.SoftDeleteObject(recoveryOrder);
            }
            return recoveryOrder;
        }

        public RecoveryOrder ConfirmObject(RecoveryOrder recoveryOrder, ICoreIdentificationDetailService _coreIdentificationDetailService, IRecoveryOrderDetailService _recoveryOrderDetailService,
                                           IRecoveryAccessoryDetailService _recoveryAccessoryDetailService, ICoreBuilderService _coreBuilderService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidConfirmObject(recoveryOrder, _coreIdentificationDetailService, _recoveryOrderDetailService, _coreBuilderService, _itemService, _warehouseItemService))
            {
                IList<RecoveryOrderDetail> details = _recoveryOrderDetailService.GetObjectsByRecoveryOrderId(recoveryOrder.Id);
                IDictionary<int, int> ValuePairItemIdQuantity = new Dictionary<int, int>();
                foreach (var detail in details)
                {
                    CoreIdentificationDetail coreIdentificationDetail = _coreIdentificationDetailService.GetObjectById(detail.CoreIdentificationDetailId);
                    CoreBuilder coreBuilder = _coreBuilderService.GetObjectById(coreIdentificationDetail.CoreBuilderId);
                    Item item = (coreIdentificationDetail.MaterialCase == Core.Constants.Constant.MaterialCase.New) ?
                                _coreBuilderService.GetNewCore(coreBuilder.Id) : _coreBuilderService.GetUsedCore(coreBuilder.Id);
                    WarehouseItem warehouseItem = _warehouseItemService.GetObjectByWarehouseAndItem(recoveryOrder.WarehouseId, item.Id);
                    if (ValuePairItemIdQuantity.ContainsKey(warehouseItem.Id))
                    {
                        ValuePairItemIdQuantity[warehouseItem.Id] -= 1;
                    }
                    else
                    {
                        ValuePairItemIdQuantity.Add(warehouseItem.Id, -1);
                    }
                }

                foreach (var ValuePair in ValuePairItemIdQuantity)
                {
                    WarehouseItem warehouseItem = _warehouseItemService.GetObjectById(ValuePair.Key);
                    _warehouseItemService.AdjustQuantity(warehouseItem, ValuePair.Value);
                }
                _repository.ConfirmObject(recoveryOrder);
            }
            return recoveryOrder;
        }

        public RecoveryOrder UnconfirmObject(RecoveryOrder recoveryOrder, ICoreIdentificationDetailService _coreIdentificationDetailService, IRecoveryOrderDetailService _recoveryOrderDetailService,
                                             IRecoveryAccessoryDetailService _recoveryAccessoryDetailService, ICoreBuilderService _coreBuilderService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidUnconfirmObject(recoveryOrder, _recoveryOrderDetailService, _recoveryAccessoryDetailService))
            {
                IList<RecoveryOrderDetail> details = _recoveryOrderDetailService.GetObjectsByRecoveryOrderId(recoveryOrder.Id);
                IDictionary<int, int> ValuePairItemIdQuantity = new Dictionary<int, int>();
                foreach (var detail in details)
                {
                    CoreIdentificationDetail coreIdentificationDetail = _coreIdentificationDetailService.GetObjectById(detail.CoreIdentificationDetailId);
                    CoreBuilder coreBuilder = _coreBuilderService.GetObjectById(coreIdentificationDetail.CoreBuilderId);
                    Item item = (coreIdentificationDetail.MaterialCase == Core.Constants.Constant.MaterialCase.New) ?
                                _coreBuilderService.GetNewCore(coreBuilder.Id) : _coreBuilderService.GetUsedCore(coreBuilder.Id);
                    WarehouseItem warehouseItem = _warehouseItemService.GetObjectByWarehouseAndItem(recoveryOrder.WarehouseId, item.Id);
                    if (ValuePairItemIdQuantity.ContainsKey(warehouseItem.Id))
                    {
                        ValuePairItemIdQuantity[warehouseItem.Id] += 1;
                    }
                    else
                    {
                        ValuePairItemIdQuantity.Add(warehouseItem.Id, 1);
                    }
                }

                foreach (var ValuePair in ValuePairItemIdQuantity)
                {
                    WarehouseItem warehouseItem = _warehouseItemService.GetObjectById(ValuePair.Key);
                    _warehouseItemService.AdjustQuantity(warehouseItem, ValuePair.Value);
                }
                _repository.UnconfirmObject(recoveryOrder);
            }
            return recoveryOrder;
        }

        public RecoveryOrder FinishObject(RecoveryOrder recoveryOrder, ICoreIdentificationDetailService _coreIdentificationDetailService, IRecoveryOrderDetailService _recoveryOrderDetailService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService,
                                          ICoreBuilderService _coreBuilderService, IRollerBuilderService _rollerBuilderService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidFinishObject(recoveryOrder, _recoveryOrderDetailService, _recoveryAccessoryDetailService))
            {
                IList<RecoveryOrderDetail> details = _recoveryOrderDetailService.GetObjectsByRecoveryOrderId(recoveryOrder.Id);
                IDictionary<int, int> ValuePairRejectedItemIdQuantity = new Dictionary<int, int>();
                IDictionary<int, int> ValuePairPackagedItemIdQuantity = new Dictionary<int, int>();
                int QuantityRejected = 0;
                int QuantityFinal = 0;
                foreach (var detail in details)
                {
                    CoreIdentificationDetail coreIdentificationDetail = _coreIdentificationDetailService.GetObjectById(detail.CoreIdentificationDetailId);
                    CoreBuilder coreBuilder = _coreBuilderService.GetObjectById(coreIdentificationDetail.CoreBuilderId);
                    RollerBuilder rollerBuilder = _rollerBuilderService.GetObjectById(detail.RollerBuilderId);
                    Item core = (coreIdentificationDetail.MaterialCase == Core.Constants.Constant.MaterialCase.New) ?
                                _coreBuilderService.GetNewCore(coreBuilder.Id) : _coreBuilderService.GetUsedCore(coreBuilder.Id);
                    Item roller = (coreIdentificationDetail.MaterialCase == Core.Constants.Constant.MaterialCase.New) ?
                                _rollerBuilderService.GetRollerNewCore(rollerBuilder.Id) : _rollerBuilderService.GetRollerUsedCore(rollerBuilder.Id);
                    WarehouseItem warehouseCore = _warehouseItemService.GetObjectByWarehouseAndItem(recoveryOrder.WarehouseId, core.Id);
                    WarehouseItem warehouseRoller = _warehouseItemService.GetObjectByWarehouseAndItem(recoveryOrder.WarehouseId, roller.Id);
                    QuantityRejected += detail.IsRejected ? 1 : 0;
                    QuantityFinal += (detail.IsPackaged && !detail.IsRejected) ? 1 : 0;
                    if (detail.IsRejected)
                    {
                        // Core quantity increases
                        if (ValuePairRejectedItemIdQuantity.ContainsKey(warehouseCore.Id))
                        {
                            ValuePairRejectedItemIdQuantity[warehouseCore.Id] += 1;
                        }
                        else
                        {
                            ValuePairRejectedItemIdQuantity.Add(warehouseCore.Id, 1);
                        }

                    }
                    else if (detail.IsPackaged && !detail.IsRejected)
                    {
                        // Roller quantity increases
                        if (ValuePairPackagedItemIdQuantity.ContainsKey(warehouseRoller.Id))
                        {
                            ValuePairPackagedItemIdQuantity[warehouseRoller.Id] += 1;
                        }
                        else
                        {
                            ValuePairPackagedItemIdQuantity.Add(warehouseRoller.Id, 1);
                        }
                    }
                }

                // do nothing for rejected items

                foreach (var ValuePairPackaged in ValuePairPackagedItemIdQuantity)
                {
                    WarehouseItem warehouseRoller = _warehouseItemService.GetObjectById(ValuePairPackaged.Key);
                    _warehouseItemService.AdjustQuantity(warehouseRoller, ValuePairPackaged.Value);
                }
                recoveryOrder.QuantityRejected = QuantityRejected;
                recoveryOrder.QuantityFinal = QuantityFinal;
                _repository.FinishObject(recoveryOrder);
            }
            return recoveryOrder;
        }

        public RecoveryOrder UnfinishObject(RecoveryOrder recoveryOrder, ICoreIdentificationDetailService _coreIdentificationDetailService, IRecoveryOrderDetailService _recoveryOrderDetailService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService,
                                            ICoreBuilderService _coreBuilderService, IRollerBuilderService _rollerBuilderService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidUnfinishObject(recoveryOrder))
            {
                IList<RecoveryOrderDetail> details = _recoveryOrderDetailService.GetObjectsByRecoveryOrderId(recoveryOrder.Id);
                IDictionary<int, int> ValuePairRejectedItemIdQuantity = new Dictionary<int, int>();
                IDictionary<int, int> ValuePairPackagedItemIdQuantity = new Dictionary<int, int>();
                foreach (var detail in details)
                {
                    CoreIdentificationDetail coreIdentificationDetail = _coreIdentificationDetailService.GetObjectById(detail.CoreIdentificationDetailId);
                    CoreBuilder coreBuilder = _coreBuilderService.GetObjectById(coreIdentificationDetail.CoreBuilderId);
                    RollerBuilder rollerBuilder = _rollerBuilderService.GetObjectById(detail.RollerBuilderId);
                    Item core = (coreIdentificationDetail.MaterialCase == Core.Constants.Constant.MaterialCase.New) ?
                                _coreBuilderService.GetNewCore(coreBuilder.Id) : _coreBuilderService.GetUsedCore(coreBuilder.Id);
                    Item roller = (coreIdentificationDetail.MaterialCase == Core.Constants.Constant.MaterialCase.New) ?
                                _rollerBuilderService.GetRollerNewCore(rollerBuilder.Id) : _rollerBuilderService.GetRollerUsedCore(rollerBuilder.Id);
                    WarehouseItem warehouseCore = _warehouseItemService.GetObjectByWarehouseAndItem(recoveryOrder.WarehouseId, core.Id);
                    WarehouseItem warehouseRoller = _warehouseItemService.GetObjectByWarehouseAndItem(recoveryOrder.WarehouseId, roller.Id);
                    if (detail.IsRejected)
                    {
                        // Core quantity increases
                        if (ValuePairRejectedItemIdQuantity.ContainsKey(warehouseCore.Id))
                        {
                            ValuePairRejectedItemIdQuantity[warehouseCore.Id] -= 1;
                        }
                        else
                        {
                            ValuePairRejectedItemIdQuantity.Add(warehouseCore.Id, -1);
                        }

                    }
                    else if (detail.IsPackaged && !detail.IsRejected)
                    {
                        // Roller quantity increases
                        if (ValuePairPackagedItemIdQuantity.ContainsKey(warehouseRoller.Id))
                        {
                            ValuePairPackagedItemIdQuantity[warehouseRoller.Id] -= 1;
                        }
                        else
                        {
                            ValuePairPackagedItemIdQuantity.Add(warehouseRoller.Id, -1);
                        }
                    }
                }

                // rejected items are ignored

                foreach (var ValuePairPackaged in ValuePairPackagedItemIdQuantity)
                {
                    WarehouseItem warehouseItem = _warehouseItemService.GetObjectById(ValuePairPackaged.Key);
                    _warehouseItemService.AdjustQuantity(warehouseItem, ValuePairPackaged.Value);
                }
                recoveryOrder.QuantityRejected = 0;
                recoveryOrder.QuantityFinal = 0;
                _repository.UnfinishObject(recoveryOrder);
            }
            return recoveryOrder;
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsCodeDuplicated(RecoveryOrder recoveryOrder)
        {
            IQueryable<RecoveryOrder> recoveryOrders = _repository.FindAll(x => x.Code == recoveryOrder.Code && !x.IsDeleted && x.Id != recoveryOrder.Id);
            return (recoveryOrders.Count() > 0 ? true : false);
        }

    }
}