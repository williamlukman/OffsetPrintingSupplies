using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ISubTypeService
    {
        ISubTypeValidator GetValidator();
        IQueryable<SubType> GetQueryable();
        IList<SubType> GetAll();
        SubType GetObjectById(int Id);
        SubType GetObjectByName(string Name);
        SubType CreateObject(SubType subType, IItemTypeService _itemTypeService);
        SubType UpdateObject(SubType subType, IItemTypeService _itemTypeService);
        SubType SoftDeleteObject(SubType subType, IItemService _itemService);
        bool DeleteObject(int Id);
        bool IsNameDuplicated(SubType subType);
    }
}