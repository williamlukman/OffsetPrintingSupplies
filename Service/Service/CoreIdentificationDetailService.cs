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
    public class CoreIdentificationDetailService : ICoreIdentificationDetailService
    {
        private ICoreIdentificationDetailRepository _repository;
        private ICoreIdentificationDetailValidator _validator;
        public CoreIdentificationDetailService(ICoreIdentificationDetailRepository _coreIdentificationDetailRepository, ICoreIdentificationDetailValidator _coreIdentificationDetailValidator)
        {
            _repository = _coreIdentificationDetailRepository;
            _validator = _coreIdentificationDetailValidator;
        }

        public ICoreIdentificationDetailValidator GetValidator()
        {
            return _validator;
        }

        public ICoreIdentificationDetailRepository GetRepository()
        {
            return _repository;
        }

        public IList<CoreIdentificationDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<CoreIdentificationDetail> GetObjectsByCoreIdentificationId(int CoreIdentificationId)
        {
            return _repository.GetObjectsByCoreIdentificationId(CoreIdentificationId);
        }

        public IList<CoreIdentificationDetail> GetObjectsByCoreBuilderId(int CoreBuilderId)
        {
            return _repository.GetObjectsByCoreBuilderId(CoreBuilderId);
        }

        public IList<CoreIdentificationDetail> GetObjectsByRollerTypeId(int rollerTypeId)
        {
            return _repository.GetObjectsByRollerTypeId(rollerTypeId);
        }

        public IList<CoreIdentificationDetail> GetObjectsByMachineId(int machineId)
        {
            return _repository.GetObjectsByMachineId(machineId);
        }

        public CoreIdentificationDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public CoreIdentificationDetail GetObjectByDetailId(int CoreIdentificationId, int DetailId)
        {
            return _repository.Find(x => x.CoreIdentificationId == CoreIdentificationId && x.DetailId == DetailId && !x.IsDeleted);
        }

        public Item GetCore(CoreIdentificationDetail coreIdentificationDetail, ICoreBuilderService _coreBuilderService)
        {
            Item core = (coreIdentificationDetail.MaterialCase == Core.Constants.Constant.MaterialCase.New) ?
                        _coreBuilderService.GetNewCore(coreIdentificationDetail.CoreBuilderId) :
                        _coreBuilderService.GetUsedCore(coreIdentificationDetail.CoreBuilderId);
            return core;
        }

        public CoreIdentificationDetail CreateObject(int CoreIdentificationId, int DetailId, int MaterialCase, int CoreBuilderId, int RollerTypeId,
                                                     int MachineId, decimal RD, decimal CD, decimal RL, decimal WL, decimal TL, ICoreIdentificationService _coreIdentificationService,
                                                     ICoreBuilderService _coreBuilderService, IRollerTypeService _rollerTypeService, IMachineService _machineService)
        {
            CoreIdentificationDetail coreIdentificationDetail = new CoreIdentificationDetail
            {
                CoreIdentificationId = CoreIdentificationId,
                DetailId = DetailId,
                MaterialCase = MaterialCase,
                CoreBuilderId = CoreBuilderId,
                RollerTypeId = RollerTypeId,
                MachineId = MachineId,
                RD = RD,
                CD = CD,
                RL = RL,
                WL = WL,
                TL = TL
            };
            return this.CreateObject(coreIdentificationDetail, _coreIdentificationService, _coreBuilderService, _rollerTypeService, _machineService);
        }

        public CoreIdentificationDetail CreateObject(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService,
                                                     ICoreBuilderService _coreBuilderService, IRollerTypeService _rollerTypeService, IMachineService _machineService)
        {
            coreIdentificationDetail.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(coreIdentificationDetail, _coreIdentificationService, this, _coreBuilderService, _rollerTypeService, _machineService) ?
                    _repository.CreateObject(coreIdentificationDetail) : coreIdentificationDetail);
        }

        public CoreIdentificationDetail UpdateObject(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService,
                                                     ICoreBuilderService _coreBuilderService, IRollerTypeService _rollerTypeService, IMachineService _machineService)
        {
            return (coreIdentificationDetail = _validator.ValidUpdateObject(coreIdentificationDetail, _coreIdentificationService, this, _coreBuilderService, _rollerTypeService, _machineService) ?
                                               _repository.UpdateObject(coreIdentificationDetail) : coreIdentificationDetail);
        }

        public CoreIdentificationDetail SoftDeleteObject(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService,
                                                         IRecoveryOrderDetailService _recoveryOrderDetailService)
        {
            return (coreIdentificationDetail = _validator.ValidDeleteObject(coreIdentificationDetail, _coreIdentificationService, _recoveryOrderDetailService) ?
                                               _repository.SoftDeleteObject(coreIdentificationDetail) : coreIdentificationDetail);
        }

        public CoreIdentificationDetail SetJobScheduled(CoreIdentificationDetail coreIdentificationDetail, IRecoveryOrderService _recoveryOrderService, IRecoveryOrderDetailService _recoveryOrderDetailService)
        {
            return (coreIdentificationDetail = _validator.ValidSetJobScheduled(coreIdentificationDetail, _recoveryOrderService, _recoveryOrderDetailService) ? _repository.SetJobScheduled(coreIdentificationDetail) : coreIdentificationDetail);
        }

        public CoreIdentificationDetail UnsetJobScheduled(CoreIdentificationDetail coreIdentificationDetail, IRecoveryOrderService _recoveryOrderService, IRecoveryOrderDetailService _recoveryOrderDetailService)
        {
            return (coreIdentificationDetail = _validator.ValidUnsetJobScheduled(coreIdentificationDetail, _recoveryOrderService, _recoveryOrderDetailService) ? _repository.UnsetJobScheduled(coreIdentificationDetail) : coreIdentificationDetail);
        }

        public CoreIdentificationDetail FinishObject(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService,
                                                     ICoreBuilderService _coreBuilderService, IStockMutationService _stockMutationService,
                                                     IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            if( _validator.ValidFinishObject(coreIdentificationDetail, _coreIdentificationService, this, _coreBuilderService))
            {
                // add customer core
                CoreIdentification coreIdentification = _coreIdentificationService.GetObjectById(coreIdentificationDetail.CoreIdentificationId);
                int MaterialCase = coreIdentificationDetail.MaterialCase;
                Item item = (MaterialCase == Core.Constants.Constant.MaterialCase.New ?
                                _coreBuilderService.GetNewCore(coreIdentificationDetail.CoreBuilderId) :
                                _coreBuilderService.GetUsedCore(coreIdentificationDetail.CoreBuilderId));
                WarehouseItem warehouseItem = _warehouseItemService.GetObjectByWarehouseAndItem(coreIdentification.WarehouseId, item.Id);
                StockMutation stockMutation = _stockMutationService.CreateStockMutationForCoreIdentification(coreIdentificationDetail, warehouseItem);
                StockMutateObject(stockMutation, _itemService, _barringService, _warehouseItemService);

                _repository.FinishObject(coreIdentificationDetail);
            }
            return coreIdentificationDetail;
        }

        public CoreIdentificationDetail UnfinishObject(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService,
                                                     ICoreBuilderService _coreBuilderService, IStockMutationService _stockMutationService,
                                                     IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidUnfinishObject(coreIdentificationDetail, _coreIdentificationService, this, _coreBuilderService))
            {
                // reduce customer core
                CoreIdentification coreIdentification = _coreIdentificationService.GetObjectById(coreIdentificationDetail.CoreIdentificationId);
                int MaterialCase = coreIdentificationDetail.MaterialCase;
                Item item = (MaterialCase == Core.Constants.Constant.MaterialCase.New ?
                                _coreBuilderService.GetNewCore(coreIdentificationDetail.CoreBuilderId) :
                                _coreBuilderService.GetUsedCore(coreIdentificationDetail.CoreBuilderId));
                WarehouseItem warehouseItem = _warehouseItemService.GetObjectByWarehouseAndItem(coreIdentification.WarehouseId, item.Id);
                IList<StockMutation> stockMutations = _stockMutationService.SoftDeleteStockMutationForCoreIdentification(coreIdentificationDetail, warehouseItem);
                foreach (var stockMutation in stockMutations)
                {
                    ReverseStockMutateObject(stockMutation, _itemService, _barringService, _warehouseItemService);
                }
                _repository.UnfinishObject(coreIdentificationDetail);
            }
            return coreIdentificationDetail;
        }

        public CoreIdentificationDetail DeliverObject(CoreIdentificationDetail coreIdentificationDetail)
        {
            return (coreIdentificationDetail = _validator.ValidDeliverObject(coreIdentificationDetail) ? _repository.DeliverObject(coreIdentificationDetail) : coreIdentificationDetail);
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
            int reverseQuantity = (stockMutation.Status == Constant.StockMutationStatus.Deduction) ? stockMutation.Quantity : (-1) * stockMutation.Quantity;
            WarehouseItem warehouseItem = _warehouseItemService.GetObjectById(stockMutation.WarehouseItemId);
            Item item = _itemService.GetObjectById(warehouseItem.ItemId);
            _itemService.AdjustQuantity(item, reverseQuantity);
            _warehouseItemService.AdjustQuantity(warehouseItem, reverseQuantity);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }


    }
}