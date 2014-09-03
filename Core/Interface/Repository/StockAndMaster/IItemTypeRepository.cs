using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IItemTypeRepository : IRepository<ItemType>
    {
        IQueryable<ItemType> GetQueryable();
        IList<ItemType> GetAll();
        ItemType GetObjectById(int Id);
        ItemType GetObjectByName(string Name);
        ItemType CreateObject(ItemType itemType);
        ItemType UpdateObject(ItemType itemType);
        ItemType SoftDeleteObject(ItemType itemType);
        bool DeleteObject(int Id);
    }
}