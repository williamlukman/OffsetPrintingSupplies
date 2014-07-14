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
        WarehouseItem VZeroQuantity(WarehouseItem warehouseItem);
        WarehouseItem VCreateObject(WarehouseItem warehouseItem);
        WarehouseItem VUpdateObject(WarehouseItem warehouseItem);
        WarehouseItem VDeleteObject(WarehouseItem warehouseItem);
        bool ValidCreateObject(WarehouseItem warehouseItem, IWarehouseService _warehouseService, IItemService _itemService);
        bool ValidUpdateObject(WarehouseItem warehouseItem, IWarehouseService _warehouseService, IItemService _itemService);
        bool ValidDeleteObject(WarehouseItem warehouseItem);
        bool isValid(WarehouseItem warehouseItem);
        string PrintError(WarehouseItem warehouseItem);
    }
}