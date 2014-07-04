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
        Item VQuantity(Item item);
        Item VQuantityMustBeZero(Item item);
        Item VIsInRecoveryAccessoryDetail(Item item, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService);
        Item VIsInRollerBuilderCompound(Item item, IRollerBuilderService _rollerBuilderService);

        Item VCreateObject(Item item, IItemService _itemService, IItemTypeService _itemTypeService);
        Item VUpdateObject(Item item, IItemService _itemService, IItemTypeService _itemTypeService);
        Item VDeleteObject(Item item, IRecoveryOrderDetailService _recoveryOrderDetailService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService,
                           IRollerBuilderService _rollerBuilderService);
        Item VAdjustQuantity(Item item);
        bool ValidCreateObject(Item item, IItemService _itemService, IItemTypeService _itemTypeService);
        bool ValidUpdateObject(Item item, IItemService _itemService, IItemTypeService _itemTypeService);
        bool ValidDeleteObject(Item item, IRecoveryOrderDetailService _recoveryOrderDetailService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService,
                               IRollerBuilderService _rollerBuilderService);
        bool ValidAdjustQuantity(Item item);
        bool isValid(Item item);
        string PrintError(Item item);
    }
}