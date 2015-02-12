using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface ISubTypeValidator
    {
        SubType VHasUniqueName(SubType subType, ISubTypeService _itemTypeService);
        SubType VHasItemType(SubType subType, IItemTypeService _itemTypeService);
        SubType VHasNoItem(SubType subType, IItemService _itemService);
        SubType VCreateObject(SubType subType, ISubTypeService _subTypeService, IItemTypeService _itemTypeService);
        SubType VUpdateObject(SubType subType, ISubTypeService _subTypeService, IItemTypeService _itemTypeService);
        SubType VDeleteObject(SubType subType, IItemService _itemService);
        bool ValidCreateObject(SubType subType, ISubTypeService _subTypeService, IItemTypeService _itemTypeService);
        bool ValidUpdateObject(SubType subType, ISubTypeService _subTypeService, IItemTypeService _itemTypeService);
        bool ValidDeleteObject(SubType subType, IItemService _itemService);
        bool isValid(SubType subType);
        string PrintError(SubType subType);
    }
}