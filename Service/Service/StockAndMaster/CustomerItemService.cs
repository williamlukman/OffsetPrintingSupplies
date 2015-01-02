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

        public IList<CustomerItem> GetObjectsByWarehouseItemId(int WarehouseItemId)
        {
            return _repository.GetObjectsByWarehouseItemId(WarehouseItemId);
        }

        public IList<CustomerItem> GetObjectsByItemId(int ItemId)
        {
            return _repository.GetObjectsByItemId(ItemId);
        }

        public IList<CustomerItem> GetObjectsByWarehouseId(int WarehouseId)
        {
            return _repository.GetObjectsByWarehouseId(WarehouseId);
        }

        public CustomerItem GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public CustomerItem FindOrCreateObject(int contactId, int warehouseItemId)
        {
            return _repository.FindOrCreateObject(contactId, warehouseItemId);
        }

        public CustomerItem AddObject(Contact contact, WarehouseItem warehouseItem, IContactService _contactService, IWarehouseItemService _warehouseItemService)
        {
            CustomerItem customerItem = new CustomerItem()
            {
                ContactId = contact.Id,
                WarehouseItemId = warehouseItem.Id,
                Quantity = 0
            };
            return CreateObject(customerItem, _contactService, _warehouseItemService);
        }

        public CustomerItem CreateObject(CustomerItem customerItem, IContactService _contactService, IWarehouseItemService _warehouseItemService)
        {
            customerItem.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(customerItem, _contactService, _warehouseItemService) ? _repository.CreateObject(customerItem) : customerItem);
        }

        public CustomerItem UpdateObject(CustomerItem customerItem, IContactService _contactService, IWarehouseItemService _warehouseItemService)
        {
            return (customerItem = _validator.ValidUpdateObject(customerItem, _contactService, _warehouseItemService) ? _repository.UpdateObject(customerItem) : customerItem);
        }

        public CustomerItem SoftDeleteObject(CustomerItem customerItem)
        {
            return (customerItem = _validator.ValidDeleteObject(customerItem) ? _repository.SoftDeleteObject(customerItem) : customerItem);
        }

        public CustomerItem AdjustQuantity(CustomerItem customerItem, decimal quantity)
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