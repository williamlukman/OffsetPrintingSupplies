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
            return (_validator.ValidCreateObject(recoveryOrder, _coreIdentificationService) ? _repository.CreateObject(recoveryOrder) : recoveryOrder);
        }

        public RecoveryOrder UpdateObject(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService, ICoreIdentificationService _coreIdentificationService)
        {
            return (recoveryOrder = _validator.ValidUpdateObject(recoveryOrder, _recoveryOrderDetailService, _coreIdentificationService) ? _repository.UpdateObject(recoveryOrder) : recoveryOrder);
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

        public RecoveryOrder ConfirmObject(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService, IRecoveryAccessoryDetailRepository _recoveryAccessoryDetailService, IItemService _itemService)
        {
            return (recoveryOrder = _validator.ValidConfirmObject(recoveryOrder, _recoveryOrderDetailService, _recoveryAccessoryDetailService, _itemService) ?
                                    _repository.UpdateObject(recoveryOrder) : recoveryOrder);
        }

        public RecoveryOrder UnconfirmObject(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService,
                                             IItemService _itemService)
        {
            return (recoveryOrder = _validator.ValidUnconfirmObject(recoveryOrder, _recoveryOrderDetailService, _recoveryAccessoryDetailService, _itemService ) ?
                                    _repository.UnconfirmObject(recoveryOrder) : recoveryOrder);
        }

        public RecoveryOrder FinishObject(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService,
                                          IItemService _itemService)
        {
            return (recoveryOrder = _validator.ValidFinishObject(recoveryOrder, _recoveryOrderDetailService, _recoveryAccessoryDetailService, _itemService) ?
                                    _repository.FinishObject(recoveryOrder) : recoveryOrder);
        }

        public RecoveryOrder UnfinishObject(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService,
                                            IItemService _itemService)
        {
            return (recoveryOrder = _validator.ValidUnfinishObject(recoveryOrder, _recoveryOrderDetailService, _recoveryAccessoryDetailService, _itemService) ?
                                    _repository.UnfinishObject(recoveryOrder) : recoveryOrder);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsCodeDuplicated(RecoveryOrder recoveryOrder)
        {
            IQueryable<RecoveryOrder> recoveryOrders = _repository.FindAll(x => x.Code == recoveryOrder.Code && !x.IsDeleted && x.Id != recoveryOrders.Id);
            return (recoveryOrders.Count() > 0 ? true : false);
        }

    }
}