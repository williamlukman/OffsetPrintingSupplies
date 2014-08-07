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
    public class RollerTypeService : IRollerTypeService
    {
        private IRollerTypeRepository _repository;
        private IRollerTypeValidator _validator;
        public RollerTypeService(IRollerTypeRepository _rollerTypeRepository, IRollerTypeValidator _rollerTypeValidator)
        {
            _repository = _rollerTypeRepository;
            _validator = _rollerTypeValidator;
        }

        public IRollerTypeValidator GetValidator()
        {
            return _validator;
        }

        public IList<RollerType> GetAll()
        {
            return _repository.GetAll();
        }

        public RollerType GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public RollerType GetObjectByName(string name)
        {
            return _repository.FindAll(c => c.Name == name && !c.IsDeleted).FirstOrDefault();
        }

        public RollerType CreateObject(string Name, string Description)
        {
            RollerType rollerType = new RollerType
            {
                Name = Name,
                Description = Description
            };
            return this.CreateObject(rollerType);
        }

        public RollerType CreateObject(RollerType rollerType)
        {
            rollerType.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(rollerType, this) ? _repository.CreateObject(rollerType) : rollerType);
        }

        public RollerType UpdateObject(RollerType rollerType)
        {
            return (rollerType = _validator.ValidUpdateObject(rollerType, this) ? _repository.UpdateObject(rollerType) : rollerType);
        }

        public RollerType SoftDeleteObject(RollerType rollerType, IRollerBuilderService _rollerBuilderService, ICoreIdentificationDetailService _coreIdentificationDetailService)
        {
            return (rollerType = _validator.ValidDeleteObject(rollerType, _rollerBuilderService, _coreIdentificationDetailService) ? _repository.SoftDeleteObject(rollerType) : rollerType);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsNameDuplicated(RollerType rollerType)
        {
            IQueryable<RollerType> types = _repository.FindAll(x => x.Name == rollerType.Name && !x.IsDeleted && x.Id != rollerType.Id);
            return (types.Count() > 0 ? true : false);
        }

    }
}