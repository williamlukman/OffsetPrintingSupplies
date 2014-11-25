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
        CustomerItem VHasItem(CustomerItem customerItem, IItemService _itemService);
        CustomerItem VHasContact(CustomerItem customerItem, IContactService _contactService);
        CustomerItem VNonNegativeQuantity(CustomerItem customerItem);
        CustomerItem VQuantityMustBeZero(CustomerItem customerItem);
        CustomerItem VCreateObject(CustomerItem customerItem, IContactService _contactService, IItemService _itemService);
        CustomerItem VUpdateObject(CustomerItem customerItem, IContactService _contactService, IItemService _itemService);
        CustomerItem VDeleteObject(CustomerItem customerItem);
        CustomerItem VAdjustQuantity(CustomerItem customerItem);
        bool ValidCreateObject(CustomerItem customerItem, IContactService _contactService, IItemService _itemService);
        bool ValidUpdateObject(CustomerItem customerItem, IContactService _contactService, IItemService _itemService);
        bool ValidDeleteObject(CustomerItem customerItem);
        bool ValidAdjustQuantity(CustomerItem customerItem);
        //bool ValidAdjustPendingReceival(CustomerItem customerItem);
        //bool ValidAdjustPendingDelivery(CustomerItem customerItem);
        bool isValid(CustomerItem customerItem);
        string PrintError(CustomerItem customerItem);
    }
}