using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Service
{
    public interface IItemService
    {
        IItemValidator GetValidator();
        IList<Item> GetAll();
        IList<Item> GetObjectsByItemTypeId(int ItemTypeId);
        Item GetObjectById(int Id);
        Item GetObjectBySku(string Sku);
        Item CreateObject(Item item, IItemTypeService _itemTypeService);
        Item UpdateObject(Item item, IItemTypeService _itemTypeService);
        Item SoftDeleteObject(Item item, IRecoveryOrderDetailService _recoveryOrderDetailService,
                                         IRecoveryAccessoryDetailService _recoveryAccessoryDetailService,
                                         IRollerBuilderService _rollerBuilderService);
        bool DeleteObject(int Id);
        bool IsSkuDuplicated(Item item);
    }
}