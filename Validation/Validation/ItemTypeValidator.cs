using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

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

        public ItemType VHasItem(ItemType itemType, IItemService _itemService)
        {
            IList<Item> list = _itemService.GetObjectsByItemTypeId(itemType.Id);
            if (list.Any())
            {
                itemType.Errors.Add("Generic", "Item tidak boleh ada yang terasosiakan dengan itemType");
            }
            return itemType;
        }

        public ItemType VCreateObject(ItemType itemType, IItemTypeService _itemTypeService)
        {
            VHasUniqueName(itemType, _itemTypeService);
            return itemType;
        }

        public ItemType VUpdateObject(ItemType itemType, IItemTypeService _itemTypeService)
        {
            VHasUniqueName(itemType, _itemTypeService);
            return itemType;
        }

        public ItemType VDeleteObject(ItemType itemType, IItemService _itemService)
        {
            VHasItem(itemType, _itemService);
            return itemType;
        }

        public bool ValidCreateObject(ItemType itemType, IItemTypeService _itemTypeService)
        {
            VCreateObject(itemType, _itemTypeService);
            return isValid(itemType);
        }

        public bool ValidUpdateObject(ItemType itemType, IItemTypeService _itemTypeService)
        {
            itemType.Errors.Clear();
            VUpdateObject(itemType, _itemTypeService);
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
