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
        CoreBuilder GetObjectById(int Id);
        Item GetUsedCore(int id, IItemService _itemService);
        Item GetNewCore(int id, IItemService _itemService);
        CoreBuilder CreateObject(CoreBuilder coreBuilder, IItemService _itemService, IItemTypeService _itemTypeService);
        CoreBuilder UpdateObject(CoreBuilder coreBuilder, IItemService _itemService);
        CoreBuilder SoftDeleteObject(CoreBuilder coreBuilder, IItemService _itemService, ICoreIdentificationDetailService _coreIdentificationDetailService);
        bool DeleteObject(int Id, IItemService _itemService);
        bool IsBaseSkuDuplicated(CoreBuilder coreBuilder);
    }
}