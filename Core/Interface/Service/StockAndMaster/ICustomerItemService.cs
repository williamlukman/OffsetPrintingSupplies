using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ICustomerItemService
    {
        ICustomerItemValidator GetValidator();
        ICustomerItemRepository GetRepository();
        IQueryable<CustomerItem> GetQueryable();
        IList<CustomerItem> GetAll();
        IList<CustomerItem> GetObjectsByContactId(int contactId);
        IList<CustomerItem> GetObjectsByItemId(int itemId);
        CustomerItem GetObjectById(int Id);
        CustomerItem FindOrCreateObject(int contactId, int ItemId);
        CustomerItem AddObject(Contact contact, Item item, IContactService _contactService, IItemService _itemService);
        CustomerItem CreateObject(CustomerItem customerItem, IContactService _contactService, IItemService _itemService);
        CustomerItem UpdateObject(CustomerItem customerItem, IContactService _contactService, IItemService _itemService);
        CustomerItem SoftDeleteObject(CustomerItem customerItem);
        CustomerItem AdjustQuantity(CustomerItem customerItem, int quantity);
        bool DeleteObject(int Id);
    }
}