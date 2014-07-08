using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Repository
{
    public interface IAbstractItemRepository : IRepository<AbstractItem>
    {
        IList<AbstractItem> GetAll();
        IList<AbstractItem> GetObjectsByItemTypeId(int ItemTypeId);
        int GetQuantityById(int Id);
        AbstractItem GetObjectById(int Id);
        AbstractItem GetObjectBySku(string Sku);
        AbstractItem CreateObject(AbstractItem abstractItem);
        AbstractItem UpdateObject(AbstractItem abstractItem);
        AbstractItem SoftDeleteObject(AbstractItem abstractItem);
        bool DeleteObject(int Id);
        bool IsSkuDuplicated(AbstractItem abstractItem);
    }
}