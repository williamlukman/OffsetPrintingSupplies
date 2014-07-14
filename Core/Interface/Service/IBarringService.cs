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
    public interface IBarringService
    {
        IBarringValidator GetValidator();
        IBarringRepository GetRepository();
        IList<Barring> GetAll();
        IList<Barring> GetObjectsByItemTypeId(int ItemTypeId);
        IList<Barring> GetObjectsByMachineId(int machineId);
        IList<Barring> GetObjectsByCustomerId(int customerId);
        Barring GetObjectById(int Id);
        Barring GetObjectBySku(string Sku);
        Barring CreateObject(Barring barring, IBarringService _barringService, IItemService _itemService, IItemTypeService _itemTypeService,
                             ICustomerService _customerService, IMachineService _machineService, 
                             IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService);
        Barring UpdateObject(Barring barring, IBarringService _barringService, IItemService _itemService, IItemTypeService _itemTypeService,
                             ICustomerService _customerService, IMachineService _machineService,
                             IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService);
        Barring SoftDeleteObject(Barring barring, IWarehouseItemService _warehouseItemService);
        bool DeleteObject(int Id);
        bool IsSkuDuplicated(Barring barring);
    }
}