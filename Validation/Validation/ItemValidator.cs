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
    public class ItemValidator : IItemValidator
    {

        public Item VHasItemType(Item item, IItemTypeService _itemTypeService)
        {
            ItemType itemType = _itemTypeService.GetObjectById(item.ItemTypeId);
            if (itemType == null)
            {
                item.Errors.Add("ItemType", "Tidak boleh tidak ada");
            }
            return item;
        }

        public Item VHasUniqueSku(Item item, IItemService _itemService)
        {
            if (String.IsNullOrEmpty(item.Sku) || item.Sku.Trim() == "")
            {
                item.Errors.Add("Sku", "Tidak boleh kosong");
            }
            if (_itemService.IsSkuDuplicated(item))
            {
                item.Errors.Add("Sku", "Tidak boleh diduplikasi");
            }
            return item;
        }

        public Item VHasName(Item item)
        {
            if (String.IsNullOrEmpty(item.Name) || item.Name.Trim() == "")
            {
                item.Errors.Add("Name", "Tidak boleh kosong");
            }
            return item;
        }

        public Item VHasCategory(Item item)
        {
            if (String.IsNullOrEmpty(item.Category) || item.Category.Trim() == "")
            {
                item.Errors.Add("Category", "Tidak boleh kosong");
            }
            return item;
        }

        public Item VHasUoM(Item item)
        {
            if (String.IsNullOrEmpty(item.UoM) || item.UoM.Trim() == "")
            {
                item.Errors.Add("UoM", "Tidak boleh kosong");
            }
            return item;
        }

        public Item VQuantity(Item item)
        {
            if (item.Quantity < 0)
            {
                item.Errors.Add("Quantity", "Tidak boleh negatif");
            }
            return item;
        }

        public Item VIsInRecoveryAccessoryDetail(Item item, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService)
        {
            IList<RecoveryAccessoryDetail> accessories = _recoveryAccessoryDetailService.GetObjectsByAccessoryId(item.Id);
            if (accessories.Any())
            {
                item.Errors.Add("Generic", "Tidak boleh terasosiasi dengan Recovery Accessory Detail");
            }
            return item;
        }

        public Item VIsInCoreBuilder(Item item, ICoreBuilderService _coreBuilderService)
        {
            IList<CoreBuilder> coreBuilders = _coreBuilderService.GetObjectsByItemId(item.Id);
            if (coreBuilders.Any())
            {
                item.Errors.Add("Generic", "Tidak boleh terasosiasi dengan Core Builder");
            }
            return item;
        }

        public Item VIsInRollerBuilder(Item item, IRollerBuilderService _rollerBuilderService)
        {
            IList<RollerBuilder> rollerBuilders = _rollerBuilderService.GetObjectsByItemId(item.Id);
            if (rollerBuilders.Any())
            {
                item.Errors.Add("Generic", "Tidak boleh terasosiasi dengan Roller Builder");
            }
            return item;
        }

        public Item VCreateObject(Item item, IItemService _itemService, IItemTypeService _itemTypeService)
        {
            VHasItemType(item, _itemTypeService);
            if (!isValid(item)) { return item; }
            VHasUniqueSku(item, _itemService);
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

        public Item VUpdateObject(Item item, IItemService _itemService, IItemTypeService _itemTypeService)
        {
            return VCreateObject(item, _itemService, _itemTypeService);
        }

        public Item VDeleteObject(Item item, IRecoveryOrderDetailService _recoveryOrderDetailService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService,
                               ICoreBuilderService _coreBuilderService, IRollerBuilderService _rollerBuilderService)
        {
            VIsInRecoveryAccessoryDetail(item, _recoveryAccessoryDetailService);
            if (!isValid(item)) { return item; }
            VIsInCoreBuilder(item, _coreBuilderService);
            if (!isValid(item)) { return item; }
            VIsInRollerBuilder(item, _rollerBuilderService);
            return item;
        }

        public Item VAdjustQuantity(Item item)
        {
            VQuantity(item);
            return item;
        }

        public bool ValidCreateObject(Item item, IItemService _itemService , IItemTypeService _itemTypeService)
        {
            VCreateObject(item, _itemService, _itemTypeService);
            return isValid(item);
        }

        public bool ValidUpdateObject(Item item, IItemService _itemService, IItemTypeService _itemTypeService)
        {
            item.Errors.Clear();
            VUpdateObject(item, _itemService, _itemTypeService);
            return isValid(item);
        }

        public bool ValidDeleteObject(Item item, IRecoveryOrderDetailService _recoveryOrderDetailService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService,
                                      ICoreBuilderService _coreBuilderService, IRollerBuilderService _rollerBuilderService)
        {
            item.Errors.Clear();
            VDeleteObject(item, _recoveryOrderDetailService, _recoveryAccessoryDetailService, _coreBuilderService, _rollerBuilderService);
            return isValid(item);
        }

        public bool ValidAdjustQuantity(Item item)
        {
            item.Errors.Clear();
            VAdjustQuantity(item);
            return isValid(item);
        }
        public bool isValid(Item obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(Item obj)
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
