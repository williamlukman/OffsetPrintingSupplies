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
        ItemType CreateObject(ItemType itemType, IAccountService _accountService);
        ItemType CreateObject(string Name, string KodeUoM, string Description, IAccountService _accountService);
        ItemType CreateObject(string Name, string KodeUoM, string Description, bool IsLegacy, IAccountService _accountService);
        ItemType CreateObject(string Name, string KodeUoM, string Description, bool IsLegacy, Account account, IAccountService _accountService);
        ItemType UpdateObject(ItemType itemType, IAccountService _accountService);
        ItemType SoftDeleteObject(ItemType itemType, IItemService _itemService);
        bool DeleteObject(int Id);
        bool IsNameDuplicated(ItemType itemType);
    }
}