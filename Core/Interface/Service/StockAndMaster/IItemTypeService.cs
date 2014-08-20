using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IItemTypeService
    {
        IItemTypeValidator GetValidator();
        IQueryable<ItemType> GetQueryable();
        IList<ItemType> GetAll();
        ItemType GetObjectById(int Id);
        ItemType GetObjectByName(string Name);
        ItemType CreateObject(ItemType itemType);
        ItemType CreateObject(string Name, string Description);
        ItemType CreateObject(string Name, string Description, bool IsLegacy);
        ItemType UpdateObject(ItemType itemType);
        ItemType SoftDeleteObject(ItemType itemType, IItemService _itemService);
        bool DeleteObject(int Id);
        bool IsNameDuplicated(ItemType itemType);
    }
}