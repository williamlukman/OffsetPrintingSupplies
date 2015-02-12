using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IItemRepository : IRepository<Item>
    {
        IQueryable<Item> GetQueryable();
        IQueryable<Item> GetQueryableObjectsByItemTypeId(int ItemTypeId);
        IList<Item> GetAll();
        IList<Item> GetObjectsByItemTypeId(int ItemTypeId);
        IList<Item> GetObjectsBySubTypeId(int SubTypeId);
        IList<Item> GetObjectsByUoMId(int UoMId);
        Item GetObjectById(int Id);
        Item GetObjectBySku(string Sku);
        Item CreateObject(Item item);
        Item UpdateObject(Item item);
        Item SoftDeleteObject(Item item);
        bool DeleteObject(int Id);
        bool IsSkuDuplicated(Item item);
    }
}