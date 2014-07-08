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
    public class AbstractItemValidator : IAbstractItemValidator
    {

        public AbstractItem VHasItemType(AbstractItem item, IItemTypeService _itemTypeService)
        {
            ItemType itemType = _itemTypeService.GetObjectById(item.ItemTypeId);
            if (itemType == null)
            {
                item.Errors.Add("ItemType", "Tidak boleh tidak ada");
            }
            return item;
        }

        public AbstractItem VHasUniqueSku(AbstractItem item, IAbstractItemService _abstractItemService)
        {
            if (String.IsNullOrEmpty(item.Sku) || item.Sku.Trim() == "")
            {
                item.Errors.Add("Sku", "Tidak boleh kosong");
            }
            if (_abstractItemService.IsSkuDuplicated(item))
            {
                item.Errors.Add("Sku", "Tidak boleh diduplikasi");
            }
            return item;
        }

        public AbstractItem VHasName(AbstractItem item)
        {
            if (String.IsNullOrEmpty(item.Name) || item.Name.Trim() == "")
            {
                item.Errors.Add("Name", "Tidak boleh kosong");
            }
            return item;
        }

        public AbstractItem VHasCategory(AbstractItem item)
        {
            if (String.IsNullOrEmpty(item.Category) || item.Category.Trim() == "")
            {
                item.Errors.Add("Category", "Tidak boleh kosong");
            }
            return item;
        }

        public AbstractItem VHasUoM(AbstractItem item)
        {
            if (String.IsNullOrEmpty(item.UoM) || item.UoM.Trim() == "")
            {
                item.Errors.Add("UoM", "Tidak boleh kosong");
            }
            return item;
        }

        public AbstractItem VQuantity(AbstractItem item)
        {
            if (item.Quantity < 0)
            {
                item.Errors.Add("Quantity", "Tidak boleh negatif");
            }
            return item;
        }

        public AbstractItem VQuantityMustBeZero(AbstractItem item)
        {
            if (item.Quantity != 0)
            {
                item.Errors.Add("Quantity", "Harus 0");
            }
            return item;
        }

        public AbstractItem VCreateObject(AbstractItem item, IAbstractItemService _abstractItemService, IItemTypeService _itemTypeService)
        {
            VHasItemType(item, _itemTypeService);
            if (!isValid(item)) { return item; }
            VHasUniqueSku(item, _abstractItemService);
            if (!isValid(item)) { return item; }
            VHasName(item);
            if (!isValid(item)) { return item; }
            VHasCategory(item);
            if (!isValid(item)) { return item; }
            VHasUoM(item);
            if (!isValid(item)) { return item; }
            VQuantity(item);
            return item;
        }

        public AbstractItem VUpdateObject(AbstractItem item, IAbstractItemService _abstractItemService, IItemTypeService _itemTypeService)
        {
            return VCreateObject(item, _abstractItemService, _itemTypeService);
        }

        public AbstractItem VDeleteObject(AbstractItem item)
        {
            VQuantityMustBeZero(item);
            return item;
        }

        public AbstractItem VAdjustQuantity(AbstractItem item)
        {
            VQuantity(item);
            return item;
        }

        public bool ValidCreateObject(AbstractItem item, IAbstractItemService _abstractItemService , IItemTypeService _itemTypeService)
        {
            VCreateObject(item, _abstractItemService, _itemTypeService);
            return isValid(item);
        }

        public bool ValidUpdateObject(AbstractItem item, IAbstractItemService _abstractItemService, IItemTypeService _itemTypeService)
        {
            item.Errors.Clear();
            VUpdateObject(item, _abstractItemService, _itemTypeService);
            return isValid(item);
        }

        public bool ValidDeleteObject(AbstractItem item)
        {
            item.Errors.Clear();
            VDeleteObject(item);
            return isValid(item);
        }

        public bool ValidAdjustQuantity(AbstractItem item)
        {
            item.Errors.Clear();
            VAdjustQuantity(item);
            return isValid(item);
        }
        public bool isValid(AbstractItem obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(AbstractItem obj)
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
