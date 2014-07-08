using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface IItemTypeValidator
    {
        ItemType VHasUniqueName(ItemType itemType, IItemTypeService _itemTypeService);
        ItemType VHasItem(ItemType itemType, IAbstractItemService _abstractItemService);
        ItemType VCreateObject(ItemType itemType, IItemTypeService _itemTypeService);
        ItemType VUpdateObject(ItemType itemType, IItemTypeService _itemTypeService);
        ItemType VDeleteObject(ItemType itemType, IAbstractItemService _abstractItemService);
        bool ValidCreateObject(ItemType itemType, IItemTypeService _itemTypeService);
        bool ValidUpdateObject(ItemType itemType, IItemTypeService _itemTypeService);
        bool ValidDeleteObject(ItemType itemType, IAbstractItemService _abstractItemService);
        bool isValid(ItemType itemType);
        string PrintError(ItemType itemType);
    }
}