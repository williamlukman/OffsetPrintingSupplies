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
    public class ContactDetailService : IContactDetailService
    {
        private IContactDetailRepository _repository;
        private IContactDetailValidator _validator;
        public ContactDetailService(IContactDetailRepository _contactDetailRepository, IContactDetailValidator _contactDetailValidator)
        {
            _repository = _contactDetailRepository;
            _validator = _contactDetailValidator;
        }

        public IContactDetailValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<ContactDetail> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<ContactDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public ContactDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public ContactDetail GetObjectByName(string name)
        {
            return _repository.GetObjectByName(name);
        }

        public ContactDetail CreateObject(ContactDetail contactDetail, IContactService _contactService)
        {
            contactDetail.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(contactDetail, this, _contactService) ? _repository.CreateObject(contactDetail) : contactDetail);
        }

        public ContactDetail UpdateObject(ContactDetail contactDetail, IContactService _contactService)
        {
            return (contactDetail = _validator.ValidUpdateObject(contactDetail, this, _contactService) ? _repository.UpdateObject(contactDetail) : contactDetail);
        }

        public ContactDetail SoftDeleteObject(ContactDetail contactDetail)
        {
            return (contactDetail = _validator.ValidDeleteObject(contactDetail) ?
                    _repository.SoftDeleteObject(contactDetail) : contactDetail);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsNameDuplicated(ContactDetail contactDetail)
        {
            IQueryable<ContactDetail> contactDetails = _repository.FindAll(x => x.Name == contactDetail.Name && !x.IsDeleted && x.Id != contactDetail.Id);
            return (contactDetails.Count() > 0 ? true : false);
        }
    }
}