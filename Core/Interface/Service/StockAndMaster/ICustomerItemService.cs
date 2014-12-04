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
        IList<CustomerItem> GetObjectsByWarehouseItemId(int WarehouseItemId);
        IList<CustomerItem> GetObjectsByItemId(int ItemId);
        IList<CustomerItem> GetObjectsByWarehouseId(int WarehouseId);
        CustomerItem GetObjectById(int Id);
        CustomerItem FindOrCreateObject(int contactId, int warehouseItemId);
        CustomerItem AddObject(Contact contact, WarehouseItem warehouseItem, IContactService _contactService, IWarehouseItemService _warehouseItemService);
        CustomerItem CreateObject(CustomerItem customerItem, IContactService _contactService, IWarehouseItemService _warehouseItemService);
        CustomerItem UpdateObject(CustomerItem customerItem, IContactService _contactService, IWarehouseItemService _warehouseItemService);
        CustomerItem SoftDeleteObject(CustomerItem customerItem);
        CustomerItem AdjustQuantity(CustomerItem customerItem, int quantity);
        bool DeleteObject(int Id);
    }
}