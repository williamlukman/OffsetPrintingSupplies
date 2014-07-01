using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface IRollerTypeValidator
    {
        RollerType VHasUniqueName(RollerType rollerType, IRollerTypeService _rollerTypeService);
        RollerType VHasRollerBuilder(RollerType rollerType, IRollerBuilderService _rollerBuilderService);
        RollerType VHasCoreIdentificationDetail(RollerType rollerType, ICoreIdentificationDetailService _coreIdentificationDetailService);
        RollerType VCreateObject(RollerType RollerType, IRollerTypeService _rollerTypeService);
        RollerType VUpdateObject(RollerType RollerType, IRollerTypeService _rollerTypeService);
        RollerType VDeleteObject(RollerType RollerType, IRollerBuilderService _rollerBuilderService, ICoreIdentificationDetailService _coreIdentificationDetailService);
        bool ValidCreateObject(RollerType RollerType, IRollerTypeService _rollerTypeService);
        bool ValidUpdateObject(RollerType RollerType, IRollerTypeService _rollerTypeService);
        bool ValidDeleteObject(RollerType RollerType, IRollerBuilderService _rollerBuilderService, ICoreIdentificationDetailService _coreIdentificationDetailService);
        bool isValid(RollerType RollerType);
        string PrintError(RollerType RollerType);
    }
}