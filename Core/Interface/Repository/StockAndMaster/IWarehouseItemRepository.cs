using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IWarehouseItemRepository : IRepository<WarehouseItem>
    {
        IQueryable<WarehouseItem> GetQueryable();
        IList<WarehouseItem> GetAll();
        IList<WarehouseItem> GetObjectsByWarehouseId(int WarehouseId);
        IList<WarehouseItem> GetObjectsByItemId(int itemId);
        WarehouseItem GetObjectById(int Id);
        WarehouseItem FindOrCreateObject(int WarehouseId, int ItemId);
        WarehouseItem CreateObject(WarehouseItem warehouseItem);
        WarehouseItem UpdateObject(WarehouseItem warehouseItem);
        WarehouseItem SoftDeleteObject(WarehouseItem warehouseItem);
        bool DeleteObject(int Id);
    }
}