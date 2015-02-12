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
    public class SubTypeService : ISubTypeService
    {
        private ISubTypeRepository _repository;
        private ISubTypeValidator _validator;
        public SubTypeService(ISubTypeRepository _subTypeRepository, ISubTypeValidator _subTypeValidator)
        {
            _repository = _subTypeRepository;
            _validator = _subTypeValidator;
        }

        public ISubTypeValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<SubType> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<SubType> GetAll()
        {
            return _repository.GetAll();
        }

        public SubType GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public SubType GetObjectByName(string name)
        {
            return _repository.GetObjectByName(name);
        }

        public SubType CreateObject(SubType subType, IItemTypeService _itemTypeService)
        {
            subType.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(subType, this, _itemTypeService) ? _repository.CreateObject(subType) : subType);
        }

        public SubType UpdateObject(SubType subType, IItemTypeService _itemTypeService)
        {
            return (subType = _validator.ValidUpdateObject(subType, this, _itemTypeService) ? _repository.UpdateObject(subType) : subType);
        }

        public SubType SoftDeleteObject(SubType subType, IItemService _itemService)
        {
            return (subType = _validator.ValidDeleteObject(subType, _itemService) ? _repository.SoftDeleteObject(subType) : subType);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsNameDuplicated(SubType subType)
        {
            IQueryable<SubType> types = _repository.FindAll(x => x.Name == subType.Name && !x.IsDeleted && x.Id != subType.Id);
            return (types.Count() > 0 ? true : false);
        }

    }
}