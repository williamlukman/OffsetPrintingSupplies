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
    public class WarehouseItemValidator : IWarehouseItemValidator
    {
        public WarehouseItem VHasItem(WarehouseItem warehouseItem, IItemService _itemService)
        {
            Item item = _itemService.GetObjectById(warehouseItem.ItemId);
            if (item == null)
            {
                warehouseItem.Errors.Add("ItemId", "Tidak terasosiasi dengan item");
            }
            return warehouseItem;
        }

        public WarehouseItem VHasWarehouse(WarehouseItem warehouseItem, IWarehouseService _warehouseService)
        {
            Warehouse warehouse = _warehouseService.GetObjectById(warehouseItem.WarehouseId);
            if (warehouse == null)
            {
                warehouseItem.Errors.Add("WarehouseId", "Tidak terasosiasi dengan warehouse");
            }
            return warehouseItem;
        }

        public WarehouseItem VNonNegativeQuantity(WarehouseItem warehouseItem)
        {
            if (warehouseItem.Quantity < 0)
            {
                warehouseItem.Errors.Add("Quantity", "Tidak boleh negatif");
            }
            return warehouseItem;
        }

        public WarehouseItem VQuantityMustBeZero(WarehouseItem warehouseItem)
        {
            if (warehouseItem.Quantity != 0)
            {
                warehouseItem.Errors.Add("Generic", "Quantity di setiap warehouse harus 0");
            }
            return warehouseItem;
        }

        public WarehouseItem VCreateObject(WarehouseItem warehouseItem, IWarehouseService _warehouseService, IItemService _itemService)
        {
            // Item Validation
            VHasItem(warehouseItem, _itemService);
            if (!isValid(warehouseItem)) { return warehouseItem; }
            VHasWarehouse(warehouseItem, _warehouseService);
            if (!isValid(warehouseItem)) { return warehouseItem; }
            VNonNegativeQuantity(warehouseItem);
            return warehouseItem;
        }

        public WarehouseItem VUpdateObject(WarehouseItem warehouseItem, IWarehouseService _warehouseService, IItemService _itemService)
        {
            return VCreateObject(warehouseItem, _warehouseService, _itemService);
        }

        public WarehouseItem VDeleteObject(WarehouseItem warehouseItem)
        {
            VQuantityMustBeZero(warehouseItem);
            return warehouseItem;
        }

        public WarehouseItem VAdjustQuantity(WarehouseItem warehouseItem)
        {
            VNonNegativeQuantity(warehouseItem);
            return warehouseItem;
        }

        public bool ValidCreateObject(WarehouseItem warehouseItem, IWarehouseService _warehouseService, IItemService _itemService)
        {
            VCreateObject(warehouseItem, _warehouseService, _itemService);
            return isValid(warehouseItem);
        }

        public bool ValidUpdateObject(WarehouseItem warehouseItem, IWarehouseService _warehouseService, IItemService _itemService)
        {
            warehouseItem.Errors.Clear();
            VUpdateObject(warehouseItem, _warehouseService, _itemService);
            return isValid(warehouseItem);
        }

        public bool ValidDeleteObject(WarehouseItem warehouseItem)
        {
            warehouseItem.Errors.Clear();
            VDeleteObject(warehouseItem);
            return isValid(warehouseItem);
        }

        public bool ValidAdjustQuantity(WarehouseItem warehouseItem)
        {
            warehouseItem.Errors.Clear();
            VAdjustQuantity(warehouseItem);
            return isValid(warehouseItem);
        }
        
        public bool isValid(WarehouseItem obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(WarehouseItem obj)
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
