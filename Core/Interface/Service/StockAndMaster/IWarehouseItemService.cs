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
        IQueryable<WarehouseItem> GetQueryable();
        IWarehouseItemValidator GetValidator();
        IWarehouseItemRepository GetRepository();
        IList<WarehouseItem> GetAll();
        IQueryable<WarehouseItem> GetQueryableObjectsByWarehouseId(int warehouseId);
        IList<WarehouseItem> GetObjectsByWarehouseId(int warehouseId);
        IQueryable<WarehouseItem> GetQueryableObjectsByItemId(int ItemId);
        IList<WarehouseItem> GetObjectsByItemId(int itemId);
        WarehouseItem GetObjectById(int Id);
        WarehouseItem FindOrCreateObject(int warehouseId, int ItemId);
        WarehouseItem AddObject(Warehouse warehouse, Item item, IWarehouseService _warehouseService, IItemService _itemService);
        WarehouseItem CreateObject(WarehouseItem warehouseItem, IWarehouseService _warehouseService, IItemService _itemService);
        WarehouseItem UpdateObject(WarehouseItem warehouseItem, IWarehouseService _warehouseService, IItemService _itemService);
        WarehouseItem SoftDeleteObject(WarehouseItem warehouseItem);
        WarehouseItem AdjustQuantity(WarehouseItem warehouseItem, int quantity);
        WarehouseItem AdjustPendingDelivery(WarehouseItem warehouseItem, int quantity);
        WarehouseItem AdjustPendingReceival(WarehouseItem warehouseItem, int quantity);
        bool DeleteObject(int Id);
    }
}