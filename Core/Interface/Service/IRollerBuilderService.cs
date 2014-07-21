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
        IList<RollerBuilder> GetObjectsByCompoundId(int compoundId);
        IList<RollerBuilder> GetObjectsByCoreBuilderId(int coreBuilderId);
        IList<RollerBuilder> GetObjectsByItemId(int ItemId);
        IList<RollerBuilder> GetObjectsByMachineId(int machineId);
        IList<RollerBuilder> GetObjectsByRollerTypeId(int rollerTypeId);
        RollerBuilder GetObjectById(int Id);
        Item GetRollerUsedCore(int id);
        Item GetRollerNewCore(int id);
        RollerBuilder CreateObject(string BaseSku, string SkuRollerNewCore, string SkuRollerUsedCore, string Name, string Category,
                                   int CD, int RD, int RL, int WL, int TL,
                                   IMachineService _machineService, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService,
                                   ICoreBuilderService _coreBuilderService, IRollerTypeService _rollerTypeService,
                                   IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService);
        RollerBuilder CreateObject(RollerBuilder rollerBuilder, IMachineService _machineService, IUoMService _uomService, IItemService _itemService,
                                   IItemTypeService _itemTypeService, ICoreBuilderService _coreBuilderService, IRollerTypeService _rollerTypeService,
                                   IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService);
        RollerBuilder UpdateNameAndCategory(RollerBuilder rollerBuilder, IMachineService _machineService, IUoMService _uomService, IItemService _itemService,
                                   IItemTypeService _itemTypeService, ICoreBuilderService _coreBuilderService, IRollerTypeService _rollerTypeService);
        RollerBuilder UpdateMeasurement(RollerBuilder rollerBuilder);
        RollerBuilder SoftDeleteObject(RollerBuilder rollerBuilder, IItemService _itemService, IRecoveryOrderDetailService _recoveryOrderDetailService,
                                        ICoreBuilderService _coreBuilderService, IWarehouseItemService _warehouseItemService);
        bool DeleteObject(int Id);
        bool IsBaseSkuDuplicated(RollerBuilder rollerBuilder);
    }
}