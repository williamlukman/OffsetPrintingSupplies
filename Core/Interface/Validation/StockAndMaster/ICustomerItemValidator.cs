using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface ICustomerItemValidator
    {
        CustomerItem VCreateObject(CustomerItem customerItem, IContactService _contactService, IWarehouseItemService _warehouseItemService);
        CustomerItem VUpdateObject(CustomerItem customerItem, IContactService _contactService, IWarehouseItemService _warehouseItemService);
        CustomerItem VDeleteObject(CustomerItem customerItem);
        CustomerItem VAdjustQuantity(CustomerItem customerItem);
        bool ValidCreateObject(CustomerItem customerItem, IContactService _contactService, IWarehouseItemService _warehouseItemService);
        bool ValidUpdateObject(CustomerItem customerItem, IContactService _contactService, IWarehouseItemService _warehouseItemService);
        bool ValidDeleteObject(CustomerItem customerItem);
        bool ValidAdjustQuantity(CustomerItem customerItem);
        bool isValid(CustomerItem customerItem);
        string PrintError(CustomerItem customerItem);
    }
}