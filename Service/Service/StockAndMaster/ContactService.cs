﻿using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Service
{
    public class ContactService : IContactService
    {
        private IContactRepository _repository;
        private IContactValidator _validator;
        public ContactService(IContactRepository _contactRepository, IContactValidator _contactValidator)
        {
            _repository = _contactRepository;
            _validator = _contactValidator;
        }

        public IContactValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<Contact> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<Contact> GetAll()
        {
            return _repository.GetAll();
        }

        public Contact GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public Contact GetObjectByName(string name)
        {
            return _repository.GetObjectByName(name);
        }

        public Contact CreateObject(Contact contact)
        {
            contact.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(contact, this) ? _repository.CreateObject(contact) : contact);
        }

        public Contact UpdateObject(Contact contact)
        {
            return (contact = _validator.ValidUpdateObject(contact, this) ? _repository.UpdateObject(contact) : contact);
        }

        public Contact SoftDeleteObject(Contact contact, ICoreIdentificationService _coreIdentificationService, IBlanketService _blanketService,
                                        IPurchaseOrderService _purchaseOrderService, ISalesOrderService _salesOrderService,
                                        ISalesQuotationService _salesQuotationService, IVirtualOrderService _virtualOrderService)
        {
            return (contact = _validator.ValidDeleteObject(contact, _coreIdentificationService, _blanketService, _purchaseOrderService, _salesOrderService,
                              _salesQuotationService, _virtualOrderService) ?
                    _repository.SoftDeleteObject(contact) : contact);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsNameDuplicated(Contact contact)
        {
            IQueryable<Contact> contacts = _repository.FindAll(x => x.Name == contact.Name && !x.IsDeleted && x.Id != contact.Id && x.ContactType == contact.ContactType);
            return (contacts.Count() > 0 ? true : false);
        }
    }
}