using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Service
{
    public interface IWarehouseService
    {
        IWarehouseValidator GetValidator();
        IList<Warehouse> GetAll();
        Warehouse GetObjectById(int Id);
        Warehouse GetObjectByCode(string Code);
        Warehouse GetObjectByName(string Name);
        Warehouse CreateObject(Warehouse warehouse, IWarehouseItemService _warehouseItemService, IItemService _itemService);
        Warehouse UpdateObject(Warehouse warehouse);
        Warehouse SoftDeleteObject(Warehouse warehouse, IWarehouseItemService _warehouseItemService, IWarehouseMutationOrderService _warehouseMutationOrderService);
        bool DeleteObject(int Id);
        bool IsCodeDuplicated(Warehouse warehouse);
    }
}