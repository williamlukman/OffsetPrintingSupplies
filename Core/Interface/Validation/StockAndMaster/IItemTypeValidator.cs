using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface IItemTypeValidator
    {
        ItemType VHasUniqueName(ItemType itemType, IItemTypeService _itemTypeService);
        ItemType VHasNoItem(ItemType itemType, IItemService _itemService);
        ItemType VNotALegacy(ItemType itemType);
        ItemType VHasAccount(ItemType itemType, IAccountService _accountService);
        ItemType VCreateObject(ItemType itemType, IItemTypeService _itemTypeService, IAccountService _accountService);
        ItemType VUpdateObject(ItemType itemType, IItemTypeService _itemTypeService, IAccountService _accountService);
        ItemType VDeleteObject(ItemType itemType, IItemService _itemService);
        bool ValidCreateObject(ItemType itemType, IItemTypeService _itemTypeService, IAccountService _accountService);
        bool ValidUpdateObject(ItemType itemType, IItemTypeService _itemTypeService, IAccountService _accountService);
        bool ValidDeleteObject(ItemType itemType, IItemService _itemService);
        bool isValid(ItemType itemType);
        string PrintError(ItemType itemType);
    }
}