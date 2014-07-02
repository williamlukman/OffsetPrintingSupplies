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
        IList<RollerBuilder> GetObjectsByItemId(int ItemId);
        IList<RollerBuilder> GetObjectsByRollerTypeId(int rollerTypeId);
        IList<RollerBuilder> GetObjectsByCoreBuilderId(int coreBuilderId);
        IList<RollerBuilder> GetObjectsByMachineId(int machineId);
        RollerBuilder GetObjectById(int Id);
        Item GetUsedRoller(int id, IItemService _itemService);
        Item GetNewRoller(int id, IItemService _itemService);
        RollerBuilder CreateObject(RollerBuilder rollerBuilder, IMachineService _machineService, IItemService _itemService,
                                   IItemTypeService _itemTypeService, ICoreBuilderService _coreBuilderService, IRollerTypeService _rollerTypeService);
        RollerBuilder UpdateNameAndCategory(RollerBuilder rollerBuilder, IMachineService _machineService, IItemService _itemService,
                                   IItemTypeService _itemTypeService, ICoreBuilderService _coreBuilderService, IRollerTypeService _rollerTypeService);
        RollerBuilder UpdateMeasurement(RollerBuilder rollerBuilder);
        RollerBuilder SoftDeleteObject(RollerBuilder rollerBuilder, IItemService _itemService, IRecoveryOrderDetailService _recoveryOrderDetailService,
                                       IRecoveryAccessoryDetailService _recoveryAccessoryDetailService);
        bool DeleteObject(int Id, IItemService _itemService);
        bool IsBaseSkuDuplicated(RollerBuilder rollerBuilder);
    }
}