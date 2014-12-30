using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class ItemTypeValidator : IItemTypeValidator
    {
        public ItemType VHasUniqueName(ItemType itemType, IItemTypeService _itemTypeService)
        {
            if (String.IsNullOrEmpty(itemType.Name) || itemType.Name.Trim() == "")
            {
                itemType.Errors.Add("Name", "Tidak boleh kosong");
            }
            if (_itemTypeService.IsNameDuplicated(itemType))
            {
                itemType.Errors.Add("Name", "Tidak boleh diduplikasi");
            }
            return itemType;
        }

        public ItemType VHasNoItem(ItemType itemType, IItemService _itemService)
        {
            IList<Item> list = _itemService.GetObjectsByItemTypeId(itemType.Id);
            if (list.Any())
            {
                itemType.Errors.Add("Generic", "Item tidak boleh ada yang terasosiakan dengan itemType");
            }
            return itemType;
        }

        public ItemType VNotALegacy(ItemType itemType)
        {
            if (itemType.IsLegacy)
            {
                itemType.Errors.Add("Generic", "Cannot update legacy item " + itemType.Name); 
            }
            return itemType;
        }

        public ItemType VHasAccount(ItemType itemType, IAccountService _accountService)
        {
            Account account = _accountService.GetObjectById(itemType.AccountId.GetValueOrDefault());
            if (account == null)
            {
                itemType.Errors.Add("AccountId", "Harus terasosiasi dengan COA");
            }
            return itemType;
        }

        public ItemType VCreateObject(ItemType itemType, IItemTypeService _itemTypeService, IAccountService _accountService)
        {
            VHasUniqueName(itemType, _itemTypeService);
            if (!isValid(itemType)) { return itemType; }
            VHasAccount(itemType, _accountService);
            return itemType;
        }

        public ItemType VUpdateObject(ItemType itemType, IItemTypeService _itemTypeService, IAccountService _accountService)
        {
            VHasUniqueName(itemType, _itemTypeService);
            if (!isValid(itemType)) { return itemType; }
            VNotALegacy(itemType);
            if (!isValid(itemType)) { return itemType; }
            VHasAccount(itemType, _accountService);
            return itemType;
        }

        public ItemType VDeleteObject(ItemType itemType, IItemService _itemService)
        {
            VHasNoItem(itemType, _itemService);
            if (!isValid(itemType)) { return itemType; }
            VNotALegacy(itemType);
            return itemType;
        }

        public bool ValidCreateObject(ItemType itemType, IItemTypeService _itemTypeService, IAccountService _accountService)
        {
            VCreateObject(itemType, _itemTypeService, _accountService);
            return isValid(itemType);
        }

        public bool ValidUpdateObject(ItemType itemType, IItemTypeService _itemTypeService, IAccountService _accountService)
        {
            itemType.Errors.Clear();
            VUpdateObject(itemType, _itemTypeService, _accountService);
            return isValid(itemType);
        }

        public bool ValidDeleteObject(ItemType itemType, IItemService _itemService)
        {
            itemType.Errors.Clear();
            VDeleteObject(itemType, _itemService);
            return isValid(itemType);
        }

        public bool isValid(ItemType obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(ItemType obj)
        {
            string erroroutput = "";
            KeyValuePair<string, string> first = obj.Errors.ElementAt(0);
            erroroutput += first.Key + "," + first.Value;
            foreach (KeyValuePair<string, string> pair in obj.Errors.Skip(1))
            {
                erroroutput += Environment.NewLine;
                erroroutput += pair.Key + "," + pair.Value;
            }
            return erroroutput;
        }

    }
}
