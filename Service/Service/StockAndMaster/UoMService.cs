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
    public class UoMService : IUoMService
    {
        private IUoMRepository _repository;
        private IUoMValidator _validator;
        public UoMService(IUoMRepository _UoMRepository, IUoMValidator _UoMValidator)
        {
            _repository = _UoMRepository;
            _validator = _UoMValidator;
        }

        public IUoMValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<UoM> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<UoM> GetAll()
        {
            return _repository.GetAll();
        }

        public UoM GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public UoM GetObjectByName(string name)
        {
            return _repository.GetObjectByName(name);
        }

        public UoM CreateObject(string Name)
        {
            UoM unitOfMeasurement = new UoM
            {
                Name = Name,
            };
            return this.CreateObject(unitOfMeasurement);
        }

        public UoM CreateObject(UoM unitOfMeasurement)
        {
            unitOfMeasurement.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(unitOfMeasurement, this) ? _repository.CreateObject(unitOfMeasurement) : unitOfMeasurement);
        }

        public UoM UpdateObject(UoM unitOfMeasurement)
        {
            return (unitOfMeasurement = _validator.ValidUpdateObject(unitOfMeasurement, this) ? _repository.UpdateObject(unitOfMeasurement) : unitOfMeasurement);
        }

        public UoM SoftDeleteObject(UoM unitOfMeasurement, IItemService _itemService)
        {
            return (unitOfMeasurement = _validator.ValidDeleteObject(unitOfMeasurement, _itemService) ? _repository.SoftDeleteObject(unitOfMeasurement) : unitOfMeasurement);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsNameDuplicated(UoM unitOfMeasurement)
        {
            IQueryable<UoM> types = _repository.FindAll(x => x.Name == unitOfMeasurement.Name && !x.IsDeleted && x.Id != unitOfMeasurement.Id);
            return (types.Count() > 0 ? true : false);
        }

    }
}