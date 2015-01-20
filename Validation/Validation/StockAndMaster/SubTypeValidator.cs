using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class SubTypeValidator : ISubTypeValidator
    {
        public SubType VHasUniqueName(SubType subType, ISubTypeService _subTypeService)
        {
            if (String.IsNullOrEmpty(subType.Name) || subType.Name.Trim() == "")
            {
                subType.Errors.Add("Name", "Tidak boleh kosong");
            }
            if (_subTypeService.IsNameDuplicated(subType))
            {
                subType.Errors.Add("Name", "Tidak boleh diduplikasi");
            }
            return subType;
        }

        public SubType VHasNoItem(SubType subType, IItemService _itemService)
        {
            IList<Item> list = _itemService.GetObjectsBySubTypeId(subType.Id);
            if (list.Any())
            {
                subType.Errors.Add("Generic", "Item tidak boleh ada yang terasosiakan dengan subType");
            }
            return subType;
        }

        public SubType VHasItemType(SubType subType, IItemTypeService _itemTypeService)
        {
            ItemType itemType = _itemTypeService.GetObjectById(subType.ItemTypeId);
            if (itemType == null)
            {
                subType.Errors.Add("ItemTypeId", "Harus terasosiasi dengan Item Type");
            }
            return subType;
        }

        public SubType VCreateObject(SubType subType, ISubTypeService _subTypeService, IItemTypeService _itemTypeService)
        {
            VHasUniqueName(subType, _subTypeService);
            if (!isValid(subType)) { return subType; }
            VHasItemType(subType, _itemTypeService);
            return subType;
        }

        public SubType VUpdateObject(SubType subType, ISubTypeService _subTypeService, IItemTypeService _itemTypeService)
        {
            VHasUniqueName(subType, _subTypeService);
            if (!isValid(subType)) { return subType; }
            VHasItemType(subType, _itemTypeService);
            return subType;
        }

        public SubType VDeleteObject(SubType subType, IItemService _itemService)
        {
            VHasNoItem(subType, _itemService);
            return subType;
        }

        public bool ValidCreateObject(SubType subType, ISubTypeService _subTypeService, IItemTypeService _itemTypeService)
        {
            VCreateObject(subType, _subTypeService, _itemTypeService);
            return isValid(subType);
        }

        public bool ValidUpdateObject(SubType subType, ISubTypeService _subTypeService, IItemTypeService _itemTypeService)
        {
            subType.Errors.Clear();
            VUpdateObject(subType, _subTypeService, _itemTypeService);
            return isValid(subType);
        }

        public bool ValidDeleteObject(SubType subType, IItemService _itemService)
        {
            subType.Errors.Clear();
            VDeleteObject(subType, _itemService);
            return isValid(subType);
        }

        public bool isValid(SubType obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(SubType obj)
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
