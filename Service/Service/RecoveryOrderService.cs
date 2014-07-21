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

        public RecoveryOrder CompleteObject(RecoveryOrder recoveryOrder, ICoreIdentificationDetailService _coreIdentificationDetailService, IRecoveryOrderDetailService _recoveryOrderDetailService,
                                          IRecoveryAccessoryDetailService _recoveryAccessoryDetailService)
        {
            if (_validator.ValidCompleteObject(recoveryOrder, _recoveryOrderDetailService, _recoveryAccessoryDetailService))
            {
                _repository.CompleteObject(recoveryOrder);
            }
            return recoveryOrder;
        }

        public RecoveryOrder AdjustQuantity(RecoveryOrder recoveryOrder)
        {
            return (recoveryOrder = _validator.ValidAdjustQuantity(recoveryOrder) ? _repository.AdjustQuantity(recoveryOrder) : recoveryOrder);
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