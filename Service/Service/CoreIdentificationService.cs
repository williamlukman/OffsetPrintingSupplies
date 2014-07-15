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
    public class CoreIdentificationService : ICoreIdentificationService
    {
        private ICoreIdentificationRepository _repository;
        private ICoreIdentificationValidator _validator;
        public CoreIdentificationService(ICoreIdentificationRepository _coreIdentificationRepository, ICoreIdentificationValidator _coreIdentificationValidator)
        {
            _repository = _coreIdentificationRepository;
            _validator = _coreIdentificationValidator;
        }

        public ICoreIdentificationValidator GetValidator()
        {
            return _validator;
        }

        public IList<CoreIdentification> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<CoreIdentification> GetAllObjectsInHouse()
        {
            return _repository.GetAllObjectsInHouse();
        }

        public IList<CoreIdentification> GetAllObjectsByCustomerId(int CustomerId)
        {
            return _repository.GetAllObjectsByCustomerId(CustomerId);
        }

        public CoreIdentification GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public CoreIdentification CreateObjectForCustomer(int CustomerId, string Code, int Quantity, DateTime IdentifiedDate, ICustomerService _customerService)
        {
            CoreIdentification coreIdentification = new CoreIdentification
            {
                CustomerId = CustomerId,
                IsInHouse = false,
                Code = Code,
                Quantity = Quantity,
                IdentifiedDate = IdentifiedDate
            };
            return this.CreateObject(coreIdentification, _customerService);
        }

        public CoreIdentification CreateObjectForInHouse(string Code, int Quantity, DateTime IdentifiedDate, ICustomerService _customerService)
        {
            CoreIdentification coreIdentification = new CoreIdentification
            {
                CustomerId = null,
                IsInHouse = true,
                Code = Code,
                Quantity = Quantity,
                IdentifiedDate = IdentifiedDate
            };
            return this.CreateObject(coreIdentification, _customerService);
        }

        public CoreIdentification CreateObject(CoreIdentification coreIdentification, ICustomerService _customerService)
        {
            coreIdentification.Errors = new Dictionary<String, String>();
            return (coreIdentification = _validator.ValidCreateObject(coreIdentification, this, _customerService) ? _repository.CreateObject(coreIdentification) : coreIdentification);
        }

        public CoreIdentification UpdateObject(CoreIdentification coreIdentification, ICustomerService _customerService)
        {
            return (coreIdentification = _validator.ValidUpdateObject(coreIdentification, this, _customerService) ? _repository.UpdateObject(coreIdentification) : coreIdentification);
        }

        public CoreIdentification SoftDeleteObject(CoreIdentification coreIdentification, ICoreIdentificationDetailService _coreIdentificationDetailService, IRecoveryOrderService _recoveryOrderService)
        {
            if (_validator.ValidDeleteObject(coreIdentification, _recoveryOrderService))
            {
                IList<CoreIdentificationDetail> details = _coreIdentificationDetailService.GetObjectsByCoreIdentificationId(coreIdentification.Id);
                foreach (var detail in details)
                {
                    _coreIdentificationDetailService.GetRepository().SoftDeleteObject(detail);
                }
                _repository.SoftDeleteObject(coreIdentification);
            }
            return coreIdentification;
        }

        public CoreIdentification ConfirmObject(CoreIdentification coreIdentification, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                                IRecoveryOrderService _recoveryOrderService, IRecoveryOrderDetailService _recoveryOrderDetailService, ICoreBuilderService _coreBuilderService,
                                                IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidConfirmObject(coreIdentification, _coreIdentificationDetailService, _coreBuilderService, _warehouseItemService))
            {
                if (coreIdentification.CustomerId != null)
                {
                    IList<CoreIdentificationDetail> details = _coreIdentificationDetailService.GetObjectsByCoreIdentificationId(coreIdentification.Id);
                    foreach (var detail in details)
                    {
                        int MaterialCase = detail.MaterialCase;
                        Item item = (MaterialCase == Core.Constants.Constant.MaterialCase.New ?
                                        _coreBuilderService.GetNewCore(detail.CoreBuilderId) :
                                        _coreBuilderService.GetUsedCore(detail.CoreBuilderId));
                        _itemService.AdjustQuantity(item, 1);
                        WarehouseItem warehouseItem = _warehouseItemService.GetObjectByWarehouseAndItem(coreIdentification.WarehouseId, item.Id);
                        _warehouseItemService.AdjustQuantity(warehouseItem, 1);
                    }
                }
                _repository.ConfirmObject(coreIdentification);
            }
            return coreIdentification;
        }

        public CoreIdentification UnconfirmObject(CoreIdentification coreIdentification, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                                  IRecoveryOrderService _recoveryOrderService, ICoreBuilderService _coreBuilderService, IItemService _itemService,
                                                  IWarehouseItemService _warehouseItemService)
        {
            if (_validator.ValidUnconfirmObject(coreIdentification, _recoveryOrderService))
            {
                if (coreIdentification.CustomerId != null)
                {
                    IList<CoreIdentificationDetail> details = _coreIdentificationDetailService.GetObjectsByCoreIdentificationId(coreIdentification.Id);
                    foreach (var detail in details)
                    {
                        int MaterialCase = detail.MaterialCase;
                        Item item = (MaterialCase == Core.Constants.Constant.MaterialCase.New ?
                                        _coreBuilderService.GetNewCore(detail.CoreBuilderId) :
                                        _coreBuilderService.GetUsedCore(detail.CoreBuilderId));
                        _itemService.AdjustQuantity(item, -1);
                        WarehouseItem warehouseItem = _warehouseItemService.GetObjectByWarehouseAndItem(coreIdentification.WarehouseId, item.Id);
                        _warehouseItemService.AdjustQuantity(warehouseItem, -1);
                    }
                }
                _repository.UnconfirmObject(coreIdentification);
            }
            return coreIdentification;
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsCodeDuplicated(CoreIdentification coreIdentification)
        {
            IQueryable<CoreIdentification> coreIdentifications = _repository.FindAll(x => x.Code == coreIdentification.Code && !x.IsDeleted && x.Id != coreIdentification.Id);
            return (coreIdentifications.Count() > 0 ? true : false);
        }
    }
}