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

        public IList<CoreIdentificationDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public CoreIdentificationDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public CoreIdentificationDetail GetObjectByName(string name)
        {
            return _repository.FindAll(c => c.Name == name && !c.IsDeleted).FirstOrDefault();
        }

        public CoreIdentificationDetail CreateObject(string Name, string Description)
        {
            CoreIdentificationDetail coreIdentificationDetail = new CoreIdentificationDetail
            {
                Name = Name,
                Description = Description
            };
            return this.CreateObject(coreIdentificationDetail);
        }

        public CoreIdentificationDetail CreateObject(CoreIdentificationDetail coreIdentificationDetail)
        {
            coreIdentificationDetail.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(coreIdentificationDetail, this) ? _repository.CreateObject(coreIdentificationDetail) : coreIdentificationDetail);
        }

        public CoreIdentificationDetail UpdateObject(CoreIdentificationDetail coreIdentificationDetail)
        {
            return (coreIdentificationDetail = _validator.ValidUpdateObject(coreIdentificationDetail, this) ? _repository.UpdateObject(coreIdentificationDetail) : coreIdentificationDetail);
        }

        public CoreIdentificationDetail SoftDeleteObject(CoreIdentificationDetail coreIdentificationDetail, IItemService _itemService)
        {
            return (coreIdentificationDetail = _validator.ValidDeleteObject(coreIdentificationDetail, _itemService) ? _repository.SoftDeleteObject(coreIdentificationDetail) : coreIdentificationDetail);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsNameDuplicated(CoreIdentificationDetail coreIdentificationDetail)
        {
            IQueryable<CoreIdentificationDetail> types = _repository.FindAll(x => x.Name == coreIdentificationDetail.Name && !x.IsDeleted && x.Id != coreIdentificationDetail.Id);
            return (types.Count() > 0 ? true : false);
        }

    }
}