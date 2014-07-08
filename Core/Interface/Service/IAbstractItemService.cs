using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Service
{
    public interface IAbstractItemService
    {
        IAbstractItemValidator GetValidator();
        IAbstractItemRepository GetRepository();
        IList<AbstractItem> GetAll();
        IList<AbstractItem> GetAllAccessories(IAbstractItemService _abstractItemService, IItemTypeService _itemTypeService);
        IList<AbstractItem> GetObjectsByItemTypeId(int ItemTypeId);
        int GetQuantityById(int Id);
        AbstractItem GetObjectById(int Id);
        AbstractItem GetObjectBySku(string Sku);
        AbstractItem CreateObject(AbstractItem abstractItem, IItemTypeService _itemTypeService);
        AbstractItem UpdateObject(AbstractItem abstractItem, IItemTypeService _itemTypeService);
        AbstractItem SoftDeleteObject(AbstractItem abstractItem);
        AbstractItem AdjustQuantity(AbstractItem abstractItem, int Quantity);
        bool DeleteObject(int Id);
        bool IsSkuDuplicated(AbstractItem abstractItem);
    }
}