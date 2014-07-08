using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface IAbstractItemValidator
    {
        AbstractItem VHasItemType(AbstractItem abstractItem, IItemTypeService _itemTypeService);
        AbstractItem VHasUniqueSku(AbstractItem abstractItem, IAbstractItemService _abstractItemService);
        AbstractItem VHasName(AbstractItem abstractItem);
        AbstractItem VHasCategory(AbstractItem abstractItem);
        AbstractItem VHasUoM(AbstractItem abstractItem);
        AbstractItem VQuantity(AbstractItem abstractItem);
        AbstractItem VQuantityMustBeZero(AbstractItem abstractItem);

        AbstractItem VCreateObject(AbstractItem abstractItem, IAbstractItemService _abstractItemService, IItemTypeService _itemTypeService);
        AbstractItem VUpdateObject(AbstractItem abstractItem, IAbstractItemService _abstractItemService, IItemTypeService _itemTypeService);
        AbstractItem VDeleteObject(AbstractItem abstractItem);
        AbstractItem VAdjustQuantity(AbstractItem abstractItem);
        bool ValidCreateObject(AbstractItem abstractItem, IAbstractItemService _abstractItemService, IItemTypeService _itemTypeService);
        bool ValidUpdateObject(AbstractItem abstractItem, IAbstractItemService _abstractItemService, IItemTypeService _itemTypeService);
        bool ValidDeleteObject(AbstractItem abstractItem);
        bool ValidAdjustQuantity(AbstractItem abstractItem);
        bool isValid(AbstractItem abstractItem);
        string PrintError(AbstractItem abstractItem);
    }
}