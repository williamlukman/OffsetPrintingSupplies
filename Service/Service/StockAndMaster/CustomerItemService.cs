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
    public class CustomerItemService : ICustomerItemService
    {
        private ICustomerItemRepository _repository;
        private ICustomerItemValidator _validator;
        public CustomerItemService(ICustomerItemRepository _customerItemRepository, ICustomerItemValidator _customerItemValidator)
        {
            _repository = _customerItemRepository;
            _validator = _customerItemValidator;
        }

        public ICustomerItemValidator GetValidator()
        {
            return _validator;
        }

        public ICustomerItemRepository GetRepository()
        {
            return _repository;
        }

        public IQueryable<CustomerItem> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<CustomerItem> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<CustomerItem> GetObjectsByContactId(int contactId)
        {
            return _repository.GetObjectsByContactId(contactId);
        }

        public IList<CustomerItem> GetObjectsByItemId(int ItemId)
        {
            return _repository.GetObjectsByItemId(ItemId);
        }

        public CustomerItem GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public CustomerItem FindOrCreateObject(int contactId, int itemId)
        {
            return _repository.FindOrCreateObject(contactId, itemId);
        }

        public CustomerItem AddObject(Contact contact, Item item, IContactService _contactService, IItemService _itemService)
        {
            CustomerItem customerItem = new CustomerItem()
            {
                ContactId = contact.Id,
                ItemId = item.Id,
                Quantity = 0,
                Contact = contact,
                Item = item
            };
            return CreateObject(customerItem, _contactService, _itemService);
        }

        public CustomerItem CreateObject(CustomerItem customerItem, IContactService _contactService, IItemService _itemService)
        {
            customerItem.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(customerItem, _contactService, _itemService) ? _repository.CreateObject(customerItem) : customerItem);
        }

        public CustomerItem UpdateObject(CustomerItem customerItem, IContactService _contactService, IItemService _itemService)
        {
            return (customerItem = _validator.ValidUpdateObject(customerItem, _contactService, _itemService) ? _repository.UpdateObject(customerItem) : customerItem);
        }

        public CustomerItem SoftDeleteObject(CustomerItem customerItem)
        {
            return (customerItem = _validator.ValidDeleteObject(customerItem) ? _repository.SoftDeleteObject(customerItem) : customerItem);
        }

        public CustomerItem AdjustQuantity(CustomerItem customerItem, int quantity)
        {
            customerItem.Quantity += quantity;
            if (_validator.ValidAdjustQuantity(customerItem)) 
            {
                _repository.UpdateObject(customerItem);
            }
            return customerItem;
        }

        //public CustomerItem AdjustPendingReceival(CustomerItem customerItem, int quantity)
        //{
        //    customerItem.PendingReceival += quantity;
        //    if (_validator.ValidAdjustPendingReceival(customerItem))
        //    {
        //        _repository.UpdateObject(customerItem);
        //    }
        //    return customerItem;
        //}

        //public CustomerItem AdjustPendingDelivery(CustomerItem customerItem, int quantity)
        //{
        //    customerItem.PendingDelivery += quantity;
        //    if (_validator.ValidAdjustPendingDelivery(customerItem))
        //    {
        //        _repository.UpdateObject(customerItem);
        //    }
        //    return customerItem;
        //}

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }
    }
}