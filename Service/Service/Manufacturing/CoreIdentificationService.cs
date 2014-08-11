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

        public IList<CoreIdentification> GetAllObjectsByContactId(int ContactId)
        {
            return _repository.GetAllObjectsByContactId(ContactId);
        }

        public IList<CoreIdentification> GetAllObjectsByWarehouseId(int WarehouseId)
        {
            return _repository.GetAllObjectsByWarehouseId(WarehouseId);
        }

        public CoreIdentification GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public CoreIdentification CreateObjectForContact(int ContactId, string Code, int Quantity, DateTime IdentifiedDate, int WarehouseId, IContactService _contactService)
        {
            CoreIdentification coreIdentification = new CoreIdentification
            {
                ContactId = ContactId,
                IsInHouse = false,
                Code = Code,
                Quantity = Quantity,
                IdentifiedDate = IdentifiedDate,
                WarehouseId = WarehouseId
            };
            return this.CreateObject(coreIdentification, _contactService);
        }

        public CoreIdentification CreateObjectForInHouse(string Code, int Quantity, DateTime IdentifiedDate, int WarehouseId, IContactService _contactService)
        {
            CoreIdentification coreIdentification = new CoreIdentification
            {
                ContactId = null,
                IsInHouse = true,
                Code = Code,
                Quantity = Quantity,
                IdentifiedDate = IdentifiedDate,
                WarehouseId = WarehouseId
            };
            return this.CreateObject(coreIdentification, _contactService);
        }

        public CoreIdentification CreateObject(CoreIdentification coreIdentification, IContactService _contactService)
        {
            coreIdentification.Errors = new Dictionary<String, String>();
            return (coreIdentification = _validator.ValidCreateObject(coreIdentification, this, _contactService) ? _repository.CreateObject(coreIdentification) : coreIdentification);
        }

        public CoreIdentification UpdateObject(CoreIdentification coreIdentification, IContactService _contactService)
        {
            return (coreIdentification = _validator.ValidUpdateObject(coreIdentification, this, _contactService) ? _repository.UpdateObject(coreIdentification) : coreIdentification);
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

        public CoreIdentification ConfirmObject(CoreIdentification coreIdentification, DateTime ConfirmationDate, ICoreIdentificationDetailService _coreIdentificationDetailService, IStockMutationService _stockMutationService,
                                                IRecoveryOrderService _recoveryOrderService, IRecoveryOrderDetailService _recoveryOrderDetailService, ICoreBuilderService _coreBuilderService,
                                                IItemService _itemService, IWarehouseItemService _warehouseItemService, IBarringService _barringService)
        {
            if (_validator.ValidConfirmObject(coreIdentification, _coreIdentificationDetailService, _coreBuilderService, _warehouseItemService))
            {
                coreIdentification.ConfirmationDate = ConfirmationDate;
                _repository.ConfirmObject(coreIdentification);
            }
            return coreIdentification;
        }

        public CoreIdentification UnconfirmObject(CoreIdentification coreIdentification, ICoreIdentificationDetailService _coreIdentificationDetailService, IStockMutationService _stockMutationService,
                                                  IRecoveryOrderService _recoveryOrderService, ICoreBuilderService _coreBuilderService, IItemService _itemService,
                                                  IWarehouseItemService _warehouseItemService, IBarringService _barringService)
        {
            if (_validator.ValidUnconfirmObject(coreIdentification, _recoveryOrderService))
            {
                _repository.UnconfirmObject(coreIdentification);
            }
            return coreIdentification;
        }

        public CoreIdentification CompleteObject(CoreIdentification coreIdentification, ICoreIdentificationDetailService _coreIdentificationDetailService)
        {
            if (_validator.ValidCompleteObject(coreIdentification, _coreIdentificationDetailService))
            {
                _repository.CompleteObject(coreIdentification);
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