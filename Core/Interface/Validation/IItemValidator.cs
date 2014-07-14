using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface IItemValidator
    {
        Item VHasItemType(Item item, IItemTypeService _itemTypeService);
        Item VHasUniqueSku(Item item, IItemService _itemService);
        Item VHasName(Item item);
        Item VHasCategory(Item item);
        Item VHasUoM(Item item);
        Item VQuantityMustBeZero(Item item, IWarehouseItemService _warehouseItemService);
        Item VIsInRecoveryAccessoryDetail(Item item, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService);
        Item VIsInRollerBuilderCompound(Item item, IRollerBuilderService _rollerBuilderService);
        Item VIsNotCoreNorRoller(Item item, IItemTypeService _itemTypeService);

        Item VCreateObject(Item item, IItemService _itemService, IItemTypeService _itemTypeService);
        Item VUpdateObject(Item item, IItemService _itemService, IItemTypeService _itemTypeService);
        Item VDeleteObject(Item item, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService);
        Item VDeleteCoreOrRoller(Item item, IWarehouseItemService _warehouseItemService);
        bool ValidCreateObject(Item item, IItemService _itemService, IItemTypeService _itemTypeService);
        bool ValidUpdateObject(Item item, IItemService _itemService, IItemTypeService _itemTypeService);
        bool ValidDeleteObject(Item item, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService);
        bool ValidDeleteCoreOrRoller(Item item, IWarehouseItemService _warehouseItemService);
        bool isValid(Item item);
        string PrintError(Item item);
    }
}