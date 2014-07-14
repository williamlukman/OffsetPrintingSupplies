using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface IBarringValidator
    {
        Barring VHasItemType(Barring barring, IItemTypeService _itemTypeService);
        Barring VHasUniqueSku(Barring barring, IBarringService _barringService);
        Barring VHasName(Barring barring);
        Barring VHasCategory(Barring barring);
        Barring VHasUoM(Barring barring);
        Barring VQuantityMustBeZero(Barring barring, IWarehouseItemService _warehouseItemService);

        Barring VHasBlanket(Barring barring, IItemService _itemService);
        Barring VHasCustomer(Barring barring, ICustomerService _customerService);
        Barring VHasMachine(Barring barring, IMachineService _machineService);
        Barring VHasMeasurement(Barring barring);

        Barring VCreateObject(Barring barring, IBarringService _barringService, IItemService _itemService, IItemTypeService _itemTypeService,
                              ICustomerService _customerService, IMachineService _machineService);
        Barring VUpdateObject(Barring barring, IBarringService _barringService, IItemService _itemService, IItemTypeService _itemTypeService,
                              ICustomerService _customerService, IMachineService _machineService);
        Barring VDeleteObject(Barring barring, IWarehouseItemService _warehouseItemService);
        Barring VAddLeftBar(Barring barring, IItemService _itemService);
        Barring VRemoveLeftBar(Barring barring, IItemService _itemService);
        Barring VAddRightBar(Barring barring, IItemService _itemService);
        Barring VRemoveRightBar(Barring barring, IItemService _itemService);

        bool ValidCreateObject(Barring barring, IBarringService _barringService, IItemService _itemService, IItemTypeService _itemTypeService,
                                     ICustomerService _customerService, IMachineService _machineService);
        bool ValidUpdateObject(Barring barring, IBarringService _barringService, IItemService _itemService, IItemTypeService _itemTypeService,
                                     ICustomerService _customerService, IMachineService _machineService);
        bool ValidDeleteObject(Barring barring, IWarehouseItemService _warehouseItemSerrvice);
        bool ValidAddLeftBar(Barring barring, IItemService _itemService);
        bool ValidRemoveLeftBar(Barring barring, IItemService _itemService);
        bool ValidAddRightBar(Barring barring, IItemService _itemService);
        bool ValidRemoveRightBar(Barring barring, IItemService _itemService);
        bool isValid(Barring barring);
        string PrintError(Barring barring);
    }
}