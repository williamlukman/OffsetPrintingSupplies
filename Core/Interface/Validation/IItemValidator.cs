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
        Item VIsInRecoveryAccessoryDetail(Item item, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService);
        Item VIsInCoreBuilder(Item item, ICoreBuilderService _coreBuilderService);
        Item VIsInRollerBuilder(Item item, ICoreBuilderService _rollerBuilderService);

        Item VCreateObject(Item item, IItemService _itemService, IItemTypeService _itemTypeService);
        Item VUpdateObject(Item item, IItemService _itemService, IItemTypeService _itemTypeService);
        Item VDeleteObject(Item item, IRecoveryOrderDetailService _recoveryOrderDetailService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService,
                               ICoreBuilderService _coreBuilderService, IRollerBuilderService _rollerBuilderService);
        Item VAdjustQuantity(Item item);
        bool ValidCreateObject(Item item, IItemTypeService _itemTypeService, IItemService _itemService);
        bool ValidUpdateObject(Item item, IItemTypeService _itemTypeService, IItemService _itemService);
        bool ValidDeleteObject(Item item, IRecoveryOrderDetailService _recoveryOrderDetailService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService,
                               ICoreBuilderService _coreBuilderService, IRollerBuilderService _rollerBuilderService);
        bool ValidAdjustQuantity(Item item);
        bool isValid(Item item);
        string PrintError(Item item);
    }
}