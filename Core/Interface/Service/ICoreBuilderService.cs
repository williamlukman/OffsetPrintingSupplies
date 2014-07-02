using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Service
{
    public interface ICoreBuilderService
    {
        ICoreBuilderValidator GetValidator();
        IList<CoreBuilder> GetAll();
        IList<CoreBuilder> GetObjectsByItemId(int ItemId);
        CoreBuilder GetObjectById(int Id);
        Item GetUsedCore(int id);
        Item GetNewCore(int id);
        CoreBuilder CreateObject(CoreBuilder coreBuilder, IItemService _itemService, IItemTypeService _itemTypeService);
        CoreBuilder CreateObject(string BaseSku, string SkuNewCore, string SkuUsedCore, string Name, string Category,
                                 IItemService _itemService, IItemTypeService _itemTypeService);
        CoreBuilder UpdateObject(CoreBuilder coreBuilder, IItemService _itemService, IItemTypeService _itemTypeService);
        CoreBuilder SoftDeleteObject(CoreBuilder coreBuilder, IItemService _itemService, ICoreIdentificationDetailService _coreIdentificationDetailService);
        bool DeleteObject(int Id, IItemService _itemService);
        bool IsBaseSkuDuplicated(CoreBuilder coreBuilder);
    }
}