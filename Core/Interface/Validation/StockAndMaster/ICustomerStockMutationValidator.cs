using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface ICustomerStockMutationValidator
    {
        CustomerStockMutation VHasItem(CustomerStockMutation customerStockMutation, IItemService _itemService);
        //CustomerStockMutation VHasContact(CustomerStockMutation customerStockMutation, IContactService _contactService);
        CustomerStockMutation VHasCustomerItem(CustomerStockMutation customerStockMutation, ICustomerItemService _customerItemService);
        CustomerStockMutation VItemCase(CustomerStockMutation customerStockMutation);
        CustomerStockMutation VStatus(CustomerStockMutation customerStockMutation);
        CustomerStockMutation VSourceDocumentType(CustomerStockMutation customerStockMutation);
        CustomerStockMutation VSourceDocumentDetailType(CustomerStockMutation customerStockMutation);
        CustomerStockMutation VNonNegativeNorZeroQuantity(CustomerStockMutation customerStockMutation);
        CustomerStockMutation VCreateObject(CustomerStockMutation customerStockMutation, ICustomerItemService _customerItemService, IItemService _itemService);
        CustomerStockMutation VUpdateObject(CustomerStockMutation customerStockMutation, ICustomerItemService _customerItemService, IItemService _itemService);
        CustomerStockMutation VDeleteObject(CustomerStockMutation customerStockMutation, ICustomerItemService _customerItemService);
        bool ValidCreateObject(CustomerStockMutation customerStockMutation, ICustomerItemService _customerItemService, IItemService _itemService);
        bool ValidUpdateObject(CustomerStockMutation customerStockMutation, ICustomerItemService _customerItemService, IItemService _itemService);
        bool ValidDeleteObject(CustomerStockMutation customerStockMutation, ICustomerItemService _customerItemService);
        bool isValid(CustomerStockMutation customerStockMutation);
        string PrintError(CustomerStockMutation customerStockMutation);
    }
}
