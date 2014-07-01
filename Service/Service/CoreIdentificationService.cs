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

        public CoreIdentification GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public CoreIdentification GetObjectByName(string name)
        {
            return _repository.FindAll(c => c.Name == name && !c.IsDeleted).FirstOrDefault();
        }

        public CoreIdentification CreateObject(string Name, string Description)
        {
            CoreIdentification coreIdentification = new CoreIdentification
            {
                Name = Name,
                Description = Description
            };
            return this.CreateObject(coreIdentification);
        }

        public CoreIdentification CreateObject(CoreIdentification coreIdentification)
        {
            coreIdentification.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(coreIdentification, this) ? _repository.CreateObject(coreIdentification) : coreIdentification);
        }

        public CoreIdentification UpdateObject(CoreIdentification coreIdentification)
        {
            return (coreIdentification = _validator.ValidUpdateObject(coreIdentification, this) ? _repository.UpdateObject(coreIdentification) : coreIdentification);
        }

        public CoreIdentification SoftDeleteObject(CoreIdentification coreIdentification, IItemService _itemService)
        {
            return (coreIdentification = _validator.ValidDeleteObject(coreIdentification, _itemService) ? _repository.SoftDeleteObject(coreIdentification) : coreIdentification);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsNameDuplicated(CoreIdentification coreIdentification)
        {
            IQueryable<CoreIdentification> types = _repository.FindAll(x => x.Name == coreIdentification.Name && !x.IsDeleted && x.Id != coreIdentification.Id);
            return (types.Count() > 0 ? true : false);
        }

    }
}