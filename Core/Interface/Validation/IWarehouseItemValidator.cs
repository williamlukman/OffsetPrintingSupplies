using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface IWarehouseItemValidator
    {
        WarehouseItem VHasItem(WarehouseItem warehouseItem, IItemService _itemService);
        WarehouseItem VHasWarehouse(WarehouseItem warehouseItem, IWarehouseService _warehouseService);
        WarehouseItem VNonNegativeQuantity(WarehouseItem warehouseItem);
        WarehouseItem VQuantityMustBeZero(WarehouseItem warehouseItem);
        WarehouseItem VCreateObject(WarehouseItem warehouseItem, IWarehouseService _warehouseService, IItemService _itemService);
        WarehouseItem VUpdateObject(WarehouseItem warehouseItem, IWarehouseService _warehouseService, IItemService _itemService);
        WarehouseItem VDeleteObject(WarehouseItem warehouseItem);
        WarehouseItem VAdjustQuantity(WarehouseItem warehouseItem);
        bool ValidCreateObject(WarehouseItem warehouseItem, IWarehouseService _warehouseService, IItemService _itemService);
        bool ValidUpdateObject(WarehouseItem warehouseItem, IWarehouseService _warehouseService, IItemService _itemService);
        bool ValidDeleteObject(WarehouseItem warehouseItem);
        bool ValidAdjustQuantity(WarehouseItem warehouseItem);
        bool isValid(WarehouseItem warehouseItem);
        string PrintError(WarehouseItem warehouseItem);
    }
}