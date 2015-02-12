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
    public class ContactGroupService : IContactGroupService
    {
        private IContactGroupRepository _repository;
        private IContactGroupValidator _validator;
        public ContactGroupService(IContactGroupRepository _contactGroupRepository, IContactGroupValidator _contactGroupValidator)
        {
            _repository = _contactGroupRepository;
            _validator = _contactGroupValidator;
        }

        public IContactGroupValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<ContactGroup> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<ContactGroup> GetAll()
        {
            return _repository.GetAll();
        }

        public ContactGroup GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public ContactGroup GetObjectByName(string name)
        {
            return _repository.GetObjectByName(name);
        }

        public ContactGroup CreateObject(ContactGroup contactGroup)
        {
            contactGroup.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(contactGroup, this) ? _repository.CreateObject(contactGroup) : contactGroup);
        }

        public ContactGroup UpdateObject(ContactGroup contactGroup)
        {
            return (contactGroup = _validator.ValidUpdateObject(contactGroup, this) ? _repository.UpdateObject(contactGroup) : contactGroup);
        }

        public ContactGroup SoftDeleteObject(ContactGroup contactGroup, IContactService _contactService)
        {
            return (contactGroup = _validator.ValidDeleteObject(contactGroup, _contactService) ? _repository.SoftDeleteObject(contactGroup) : contactGroup);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsNameDuplicated(ContactGroup contactGroup)
        {
            IQueryable<ContactGroup> types = _repository.FindAll(x => x.Name == contactGroup.Name && !x.IsDeleted && x.Id != contactGroup.Id);
            return (types.Count() > 0 ? true : false);
        }

    }
}