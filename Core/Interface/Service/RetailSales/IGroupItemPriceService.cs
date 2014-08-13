using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Validation;

namespace Core.Interface.Service
{
    public interface IGroupItemPriceService
    {
        IGroupItemPriceValidator GetValidator();
        IList<GroupItemPrice> GetAll();
        GroupItemPrice GetObjectById(int Id);
        GroupItemPrice CreateObject(GroupItemPrice groupItemPrice, IGroupService _groupService, IItemService _itemService, IPriceMutationService _priceMutationService);
        GroupItemPrice UpdateObject(GroupItemPrice groupItemPrice, IItemService _itemService, IPriceMutationService _priceMutationService);
        GroupItemPrice SoftDeleteObject(GroupItemPrice groupItemPrice);
        bool DeleteObject(int Id);
        bool IsGroupItemPriceDuplicated(GroupItemPrice groupItemPrice);
    }
}
