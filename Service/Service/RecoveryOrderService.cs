using Core.Constants;
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

        public RecoveryOrder CreateObject(int CoreIdentificationId, string Code, int QuantityReceived, int WarehouseId, ICoreIdentificationService _coreIdentificationService)
        {
            RecoveryOrder recoveryOrder = new RecoveryOrder
            {
                CoreIdentificationId = CoreIdentificationId,
                Code = Code,
                QuantityReceived = QuantityReceived,
                WarehouseId = WarehouseId
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
                                             IRecoveryAccessoryDetailService _recoveryAccessoryDetailService, ICoreBuilderService _coreBuilderService, IStockMutationService _stockMutationService,
                                             IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService)
        {
            if (_validator.ValidConfirmObject(recoveryOrder, _coreIdentificationDetailService, _recoveryOrderDetailService, _coreBuilderService, _itemService, _warehouseItemService))
            {
                IList<RecoveryOrderDetail> details = _recoveryOrderDetailService.GetObjectsByRecoveryOrderId(recoveryOrder.Id);
                IDictionary<int, int> ValuePairItemIdQuantity = new Dictionary<int, int>();
                foreach (var detail in details)
                {
                    CoreIdentificationDetail coreIdentificationDetail = _coreIdentificationDetailService.GetObjectById(detail.CoreIdentificationDetailId);
                    _coreIdentificationDetailService.SetJobScheduled(coreIdentificationDetail);
                }
                _repository.ConfirmObject(recoveryOrder);
            }
            return recoveryOrder;
        }

        public RecoveryOrder UnconfirmObject(RecoveryOrder recoveryOrder, ICoreIdentificationDetailService _coreIdentificationDetailService, IRecoveryOrderDetailService _recoveryOrderDetailService,
                                             IRecoveryAccessoryDetailService _recoveryAccessoryDetailService, ICoreBuilderService _coreBuilderService, IStockMutationService _stockMutationService,
                                             IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService)
        {
            if (_validator.ValidUnconfirmObject(recoveryOrder, _recoveryOrderDetailService, _recoveryAccessoryDetailService))
            {
                IList<RecoveryOrderDetail> details = _recoveryOrderDetailService.GetObjectsByRecoveryOrderId(recoveryOrder.Id);
                IDictionary<int, int> ValuePairItemIdQuantity = new Dictionary<int, int>();
                foreach (var detail in details)
                {
                    CoreIdentificationDetail coreIdentificationDetail = _coreIdentificationDetailService.GetObjectById(detail.CoreIdentificationDetailId);
                    _coreIdentificationDetailService.UnsetJobScheduled(coreIdentificationDetail);
                }
                _repository.UnconfirmObject(recoveryOrder);
            }
            return recoveryOrder;
        }

        public RecoveryOrder FinishObject(RecoveryOrder recoveryOrder, ICoreIdentificationDetailService _coreIdentificationDetailService, IRecoveryOrderDetailService _recoveryOrderDetailService,
                                          IRecoveryAccessoryDetailService _recoveryAccessoryDetailService, ICoreBuilderService _coreBuilderService, IRollerBuilderService _rollerBuilderService,
                                          IItemService _itemService, IWarehouseItemService _warehouseItemService, IBarringService _barringService, IStockMutationService _stockMutationService)
        {
            // TODO validator for compound, core, roller, accessories
            if (_validator.ValidFinishObject(recoveryOrder, _recoveryOrderDetailService, _recoveryAccessoryDetailService))
            {
                IList<RecoveryOrderDetail> details = _recoveryOrderDetailService.GetObjectsByRecoveryOrderId(recoveryOrder.Id);
                int QuantityRejected = 0;
                int QuantityFinal = 0;
                foreach (var detail in details)
                {
                    CoreIdentificationDetail coreIdentificationDetail = _coreIdentificationDetailService.GetObjectById(detail.CoreIdentificationDetailId);
                    _coreIdentificationDetailService.UnsetJobScheduled(coreIdentificationDetail);
                    _coreIdentificationDetailService.FinishObject(coreIdentificationDetail);
                    QuantityRejected += detail.IsRejected ? 1 : 0;
                    QuantityFinal += (detail.IsPackaged && !detail.IsRejected) ? 1 : 0;

                    // remove compound

                    if (!detail.IsRejected)
                    {
                        // deduce core
                        CoreBuilder coreBuilder = _coreBuilderService.GetObjectById(coreIdentificationDetail.CoreBuilderId);
                        Item core = (coreIdentificationDetail.MaterialCase == Core.Constants.Constant.MaterialCase.New) ?
                                    _coreBuilderService.GetNewCore(coreBuilder.Id) : _coreBuilderService.GetUsedCore(coreBuilder.Id);
                        WarehouseItem warehouseCore = _warehouseItemService.GetObjectByWarehouseAndItem(recoveryOrder.WarehouseId, core.Id);
                        StockMutation stockMutationCore = _stockMutationService.CreateStockMutationForRecoveryOrder(detail, warehouseCore);
                        StockMutateObject(stockMutationCore, _itemService, _barringService, _warehouseItemService);

                        // add roller
                        RollerBuilder rollerBuilder = _rollerBuilderService.GetObjectById(detail.RollerBuilderId);
                        Item roller = (coreIdentificationDetail.MaterialCase == Core.Constants.Constant.MaterialCase.New) ?
                                    _rollerBuilderService.GetRollerNewCore(rollerBuilder.Id) : _rollerBuilderService.GetRollerUsedCore(rollerBuilder.Id);
                        WarehouseItem warehouseRoller = _warehouseItemService.GetObjectByWarehouseAndItem(recoveryOrder.WarehouseId, roller.Id);
                        StockMutation stockMutationRoller = _stockMutationService.CreateStockMutationForRecoveryOrder(detail, warehouseRoller);
                        StockMutateObject(stockMutationRoller, _itemService, _barringService, _warehouseItemService);
                        
                        // deduce accessories
                        IList<RecoveryAccessoryDetail> recoveryAccessoryDetails = _recoveryAccessoryDetailService.GetObjectsByRecoveryOrderDetailId(detail.Id);
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
                }

                recoveryOrder.QuantityRejected = QuantityRejected;
                recoveryOrder.QuantityFinal = QuantityFinal;
                _repository.FinishObject(recoveryOrder);
            }
            return recoveryOrder;
        }

        public RecoveryOrder UnfinishObject(RecoveryOrder recoveryOrder, ICoreIdentificationDetailService _coreIdentificationDetailService, IRecoveryOrderDetailService _recoveryOrderDetailService,
                                            IRecoveryAccessoryDetailService _recoveryAccessoryDetailService, ICoreBuilderService _coreBuilderService, IRollerBuilderService _rollerBuilderService,
                                            IItemService _itemService, IWarehouseItemService _warehouseItemService, IBarringService _barringService, IStockMutationService _stockMutationService)
        {
            if (_validator.ValidUnfinishObject(recoveryOrder))
            {
                IList<RecoveryOrderDetail> details = _recoveryOrderDetailService.GetObjectsByRecoveryOrderId(recoveryOrder.Id);
                foreach (var detail in details)
                {
                    CoreIdentificationDetail coreIdentificationDetail = _coreIdentificationDetailService.GetObjectById(detail.CoreIdentificationDetailId);
                    _coreIdentificationDetailService.SetJobScheduled(coreIdentificationDetail);
                    _coreIdentificationDetailService.UnfinishObject(coreIdentificationDetail);
                    
                    // add compound
                    
                    if (!detail.IsRejected)
                    {
                        // add core
                        CoreBuilder coreBuilder = _coreBuilderService.GetObjectById(coreIdentificationDetail.CoreBuilderId);
                        Item core = (coreIdentificationDetail.MaterialCase == Core.Constants.Constant.MaterialCase.New) ?
                                    _coreBuilderService.GetNewCore(coreBuilder.Id) : _coreBuilderService.GetUsedCore(coreBuilder.Id);
                        WarehouseItem warehouseCore = _warehouseItemService.GetObjectByWarehouseAndItem(recoveryOrder.WarehouseId, core.Id);
                        IList<StockMutation> stockMutationCores = _stockMutationService.SoftDeleteStockMutationForRecoveryOrder(detail, warehouseCore);
                        foreach (var stockMutationCore in stockMutationCores)
                        {
                            ReverseStockMutateObject(stockMutationCore, _itemService, _barringService, _warehouseItemService);
                        }

                        // deduce roller
                        RollerBuilder rollerBuilder = _rollerBuilderService.GetObjectById(detail.RollerBuilderId);
                        Item roller = (coreIdentificationDetail.MaterialCase == Core.Constants.Constant.MaterialCase.New) ?
                                    _rollerBuilderService.GetRollerNewCore(rollerBuilder.Id) : _rollerBuilderService.GetRollerUsedCore(rollerBuilder.Id);
                        WarehouseItem warehouseRoller = _warehouseItemService.GetObjectByWarehouseAndItem(recoveryOrder.WarehouseId, roller.Id);
                        IList<StockMutation> stockMutationRollers = _stockMutationService.SoftDeleteStockMutationForRecoveryOrder(detail, warehouseRoller);
                        foreach (var stockMutationRoller in stockMutationRollers)
                        {
                            ReverseStockMutateObject(stockMutationRoller, _itemService, _barringService, _warehouseItemService);
                        }

                        // add accessories
                        IList<RecoveryAccessoryDetail> recoveryAccessoryDetails = _recoveryAccessoryDetailService.GetObjectsByRecoveryOrderDetailId(detail.Id);
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
                }

                recoveryOrder.QuantityRejected = 0;
                recoveryOrder.QuantityFinal = 0;
                _repository.UnfinishObject(recoveryOrder);
            }
            return recoveryOrder;
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

        public bool IsCodeDuplicated(RecoveryOrder recoveryOrder)
        {
            IQueryable<RecoveryOrder> recoveryOrders = _repository.FindAll(x => x.Code == recoveryOrder.Code && !x.IsDeleted && x.Id != recoveryOrder.Id);
            return (recoveryOrders.Count() > 0 ? true : false);
        }

    }
}