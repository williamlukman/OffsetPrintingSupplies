using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Service
{
    public interface IRollerBuilderService
    {
        IRollerBuilderValidator GetValidator();
        IList<RollerBuilder> GetAll();
        IList<RollerBuilder> GetObjectsByCoreBuilderId(int coreBuilderId);
        RollerBuilder GetObjectById(int Id);
        Item GetUsedRoller(int id, IItemService _itemService);
        Item GetNewRoller(int id, IItemService _itemService);
        RollerBuilder CreateObject(RollerBuilder rollerBuilder, IItemService _itemService, IRollerTypeService _rollerTypeService);
        RollerBuilder UpdateObject(RollerBuilder rollerBuilder, IItemService _itemService, IRollerTypeService _rollerTypeService);
        RollerBuilder SoftDeleteObject(RollerBuilder rollerBuilder, IItemService _itemService, IRecoveryOrderDetailService _recoveryOrderDetailService,
                                       IRecoveryAccessoryDetailService _recoveryAccessoryDetailService);
        bool DeleteObject(int Id, IItemService _itemService);
        bool IsBaseSkuDuplicated(RollerBuilder rollerBuilder);
    }
}