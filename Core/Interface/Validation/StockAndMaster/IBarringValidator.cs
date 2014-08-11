using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface IBarringValidator
    {
        Barring VHasItemTypeAndIsLegacy(Barring barring, IItemTypeService _itemTypeService);
        Barring VHasUniqueSku(Barring barring, IBarringService _barringService);
        Barring VHasName(Barring barring);
        Barring VHasCategory(Barring barring);
        Barring VHasUoM(Barring barring, IUoMService _uomService);
        Barring VWarehouseQuantityMustBeZero(Barring barring, IWarehouseItemService _warehouseItemService);
        Barring VNonNegativeQuantity(Barring barring);

        Barring VHasBlanket(Barring barring, IItemService _itemService);
        Barring VHasContact(Barring barring, IContactService _contactService);
        Barring VHasMachine(Barring barring, IMachineService _machineService);
        Barring VHasMeasurement(Barring barring);

        Barring VCreateObject(Barring barring, IBarringService _barringService, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService,
                              IContactService _contactService, IMachineService _machineService);
        Barring VUpdateObject(Barring barring, IBarringService _barringService, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService,
                              IContactService _contactService, IMachineService _machineService);
        Barring VDeleteObject(Barring barring, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService);
        Barring VAdjustQuantity(Barring barring);
        Barring VAdjustPendingDelivery(Barring barring);
        Barring VAdjustPendingReceival(Barring barring);

        Barring VAddLeftBar(Barring barring, IItemService _itemService);
        Barring VRemoveLeftBar(Barring barring, IItemService _itemService);
        Barring VAddRightBar(Barring barring, IItemService _itemService);
        Barring VRemoveRightBar(Barring barring, IItemService _itemService);

        bool ValidCreateObject(Barring barring, IBarringService _barringService, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService,
                                     IContactService _contactService, IMachineService _machineService);
        bool ValidUpdateObject(Barring barring, IBarringService _barringService, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService,
                                     IContactService _contactService, IMachineService _machineService);
        bool ValidDeleteObject(Barring barring, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemSerrvice);
        bool ValidAdjustQuantity(Barring barring);
        bool ValidAdjustPendingDelivery(Barring barring);
        bool ValidAdjustPendingReceival(Barring barring);
        bool ValidAddLeftBar(Barring barring, IItemService _itemService);
        bool ValidRemoveLeftBar(Barring barring, IItemService _itemService);
        bool ValidAddRightBar(Barring barring, IItemService _itemService);
        bool ValidRemoveRightBar(Barring barring, IItemService _itemService);
        bool isValid(Barring barring);
        string PrintError(Barring barring);
    }
}