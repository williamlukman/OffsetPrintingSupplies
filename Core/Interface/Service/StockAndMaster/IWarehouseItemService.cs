using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IWarehouseItemService
    {
        IWarehouseItemValidator GetValidator();
        IWarehouseItemRepository GetRepository();
        IQueryable<WarehouseItem> GetQueryable();
        IList<WarehouseItem> GetAll();
        IList<WarehouseItem> GetObjectsByWarehouseId(int warehouseId);
        IList<WarehouseItem> GetObjectsByItemId(int itemId);
        WarehouseItem GetObjectById(int Id);
        WarehouseItem FindOrCreateObject(int warehouseId, int ItemId);
        WarehouseItem AddObject(Warehouse warehouse, Item item, IWarehouseService _warehouseService, IItemService _itemService);
        WarehouseItem CreateObject(WarehouseItem warehouseItem, IWarehouseService _warehouseService, IItemService _itemService);
        WarehouseItem UpdateObject(WarehouseItem warehouseItem, IWarehouseService _warehouseService, IItemService _itemService);
        WarehouseItem SoftDeleteObject(WarehouseItem warehouseItem);
        WarehouseItem AdjustCustomerQuantity(WarehouseItem warehouseItem, decimal quantity);
        WarehouseItem AdjustQuantity(WarehouseItem warehouseItem, decimal quantity);
        WarehouseItem AdjustPendingDelivery(WarehouseItem warehouseItem, decimal quantity);
        WarehouseItem AdjustPendingReceival(WarehouseItem warehouseItem, decimal quantity);
        bool DeleteObject(int Id);
    }
}