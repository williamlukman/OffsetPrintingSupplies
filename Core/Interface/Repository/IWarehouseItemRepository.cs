using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Repository
{
    public interface IWarehouseItemRepository : IRepository<WarehouseItem>
    {
        IList<WarehouseItem> GetAll();
        IList<WarehouseItem> GetObjectsByWarehouseId(int WarehouseId);
        IList<WarehouseItem> GetObjectsByItemId(int itemId);
        WarehouseItem GetObjectById(int Id);
        WarehouseItem GetObjectByWarehouseAndItem(int WarehouseId, int itemId);
        WarehouseItem CreateObject(WarehouseItem warehouseItem);
        WarehouseItem UpdateObject(WarehouseItem warehouseItem);
        WarehouseItem SoftDeleteObject(WarehouseItem warehouseItem);
        bool DeleteObject(int Id);
    }
}