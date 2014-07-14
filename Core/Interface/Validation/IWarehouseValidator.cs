using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface IWarehouseValidator
    {
        Warehouse VHasUniqueCode(Warehouse warehouse, IWarehouseService _warehouseService);
        Warehouse VCreateObject(Warehouse warehouse, IWarehouseService _warehouseService);
        Warehouse VUpdateObject(Warehouse warehouse, IWarehouseService _warehouseService);
        Warehouse VDeleteObject(Warehouse warehouse, IWarehouseItemService _warehouseItemService, IWarehouseMutationOrderService _warehouseMutationOrderService);
        bool ValidCreateObject(Warehouse warehouse, IWarehouseService _warehouseService);
        bool ValidUpdateObject(Warehouse warehouse, IWarehouseService _warehouseService);
        bool ValidDeleteObject(Warehouse warehouse, IWarehouseItemService _warehouseItemService, IWarehouseMutationOrderService _warehouseMutationOrderService);
        bool isValid(Warehouse warehouse);
        string PrintError(Warehouse warehouse);
    }
}