using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface IWarehouseValidator
    {
        Warehouse VHasUniqueCode(Warehouse warehouse, IWarehouseService _warehouseService);
        Warehouse VWarehouseQuantityMustBeZero(Warehouse warehouse, IWarehouseItemService _warehouseItemService);
        Warehouse VIsInCoreIdentification(Warehouse warehouse, ICoreIdentificationService _coreIdentificationService);
        Warehouse VIsInBarringOrderAndIncomplete(Warehouse warehouse, IBarringOrderService _barringOrderService);
        Warehouse VCreateObject(Warehouse warehouse, IWarehouseService _warehouseService);
        Warehouse VUpdateObject(Warehouse warehouse, IWarehouseService _warehouseService);
        Warehouse VDeleteObject(Warehouse warehouse, IWarehouseItemService _warehouseItemService, ICoreIdentificationService _coreIdentificationService, IBarringOrderService _barringOrderService);
        bool ValidCreateObject(Warehouse warehouse, IWarehouseService _warehouseService);
        bool ValidUpdateObject(Warehouse warehouse, IWarehouseService _warehouseService);
        bool ValidDeleteObject(Warehouse warehouse, IWarehouseItemService _warehouseItemService, ICoreIdentificationService _coreIdentificationService, IBarringOrderService _barringOrderService);
        bool isValid(Warehouse warehouse);
        string PrintError(Warehouse warehouse);
    }
}