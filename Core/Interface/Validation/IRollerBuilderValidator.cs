using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface IRollerBuilderValidator
    {
        RollerBuilder VHasUniqueBaseSku(RollerBuilder rollerBuilder, IRollerBuilderService _rollerBuilderService);
        RollerBuilder VNameNotEmpty(RollerBuilder rollerBuilder);
        RollerBuilder VHasMachine(RollerBuilder rollerBuilder, IMachineService _machineService);
        RollerBuilder VHasCompound(RollerBuilder rollerBuilder, IItemService _itemService);
        RollerBuilder VHasCoreBuilder(RollerBuilder rollerBuilder, ICoreBuilderService _coreBuilderService);
        RollerBuilder VHasRollerType(RollerBuilder rollerBuilder, IRollerTypeService _rollerTypeService);
        RollerBuilder VHasRollerUsedCoreItem(RollerBuilder rollerBuilder, IItemService _itemService);
        RollerBuilder VHasRollerNewCoreItem(RollerBuilder rollerBuilder, IItemService _itemService);
        RollerBuilder VHasMeasurement(RollerBuilder rollerBuilder);
        RollerBuilder VHasUoM(RollerBuilder rollerBuilder, IUoMService _uomService);
        RollerBuilder VIsInRecoveryOrderDetails(RollerBuilder rollerBuilder, IRecoveryOrderDetailService _recoveryOrderDetailService);
        RollerBuilder VCreateObject(RollerBuilder rollerBuilder, IRollerBuilderService _rollerBuilderService, IMachineService _machineService,
                                    IUoMService _uomService, IItemService _itemService, ICoreBuilderService _coreBuilderService, IRollerTypeService _rollerTypeService);
        RollerBuilder VUpdateObject(RollerBuilder rollerBuilder, IRollerBuilderService _rollerBuilderService, IMachineService _machineService,
                                    IUoMService _uomService, IItemService _itemService, ICoreBuilderService _coreBuilderService, IRollerTypeService _rollerTypeService);
        RollerBuilder VDeleteObject(RollerBuilder rollerBuilder, IRecoveryOrderDetailService _recoveryOrderDetailService);
        bool ValidCreateObject(RollerBuilder rollerBuilder, IRollerBuilderService _rollerBuilderService, IMachineService _machineService,
                               IUoMService _uomService, IItemService _itemService, ICoreBuilderService _coreBuilderService, IRollerTypeService _rollerTypeService);
        bool ValidUpdateObject(RollerBuilder rollerBuilder, IRollerBuilderService _rollerBuilderService, IMachineService _machineService,
                               IUoMService _uomService, IItemService _itemService, ICoreBuilderService _coreBuilderService, IRollerTypeService _rollerTypeService);
        bool ValidDeleteObject(RollerBuilder rollerBuilder, IRecoveryOrderDetailService _recoveryOrderDetailService);
        bool isValid(RollerBuilder rollerBuilder);
        string PrintError(RollerBuilder rollerBuilder);
    }
}