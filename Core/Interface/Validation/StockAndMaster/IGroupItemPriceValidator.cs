using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IGroupItemPriceValidator
    {
        GroupItemPrice VNonNegativePrice(GroupItemPrice groupItemPrice);
        GroupItemPrice VHasDifferentPrice(GroupItemPrice groupItemPrice, IPriceMutationService _priceMutationService);
        GroupItemPrice VSameGroupId(GroupItemPrice groupItemPrice, int oldGroupId);
        GroupItemPrice VSameItemId(GroupItemPrice groupItemPrice, int oldItemId);
        GroupItemPrice VHasUniqueIdCombination(GroupItemPrice groupItemPrice, IGroupItemPriceService _groupItemPriceService);
        GroupItemPrice VIsValidGroup(GroupItemPrice groupItemPrice, IContactGroupService _contactGroupService);
        GroupItemPrice VIsValidItem(GroupItemPrice groupItemPrice, IItemService _itemService);

        GroupItemPrice VCreateObject(GroupItemPrice groupItemPrice, IGroupItemPriceService _groupItemPriceService, IContactGroupService _contactGroupService, IItemService _itemService);
        GroupItemPrice VUpdateObject(GroupItemPrice groupItemPrice, int oldGroupId, int oldItemId, IGroupItemPriceService _groupItemPriceService, IPriceMutationService _priceMutationService);
        GroupItemPrice VDeleteObject(GroupItemPrice groupItemPrice);
        bool ValidCreateObject(GroupItemPrice groupItemPrice, IGroupItemPriceService _groupItemPriceService, IContactGroupService _contactGroupService, IItemService _itemService);
        bool ValidUpdateObject(GroupItemPrice groupItemPrice, int oldGroupId, int oldItemId, IGroupItemPriceService _groupItemPriceService, IPriceMutationService _priceMutationService);
        bool ValidDeleteObject(GroupItemPrice groupItemPrice);
        bool isValid(GroupItemPrice groupItemPrice);
        string PrintError(GroupItemPrice groupItemPrice);
    }
}
