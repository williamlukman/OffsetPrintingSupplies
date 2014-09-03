using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface IMachineValidator
    {
        Machine VHasUniqueCode(Machine machine, IMachineService _machineService);
        Machine VNameNotEmpty(Machine machine);
        Machine VIsNotInBlanket(Machine machine, IBlanketService _blanketService);
        Machine VIsNotInRollerBuilder(Machine machine, IRollerBuilderService _rollerBuilderService);
        Machine VIsNotInCoreIdentificationDetail(Machine machine, ICoreIdentificationDetailService _coreIdentificationDetailService);
        Machine VCreateObject(Machine machine, IMachineService _machineService);
        Machine VUpdateObject(Machine machine, IMachineService _machineService);
        Machine VDeleteObject(Machine machine, IRollerBuilderService _rollerBuilderService, ICoreIdentificationDetailService _coreIdentificationDetailService, IBlanketService _blanketService);
        bool ValidCreateObject(Machine machine, IMachineService _machineService);
        bool ValidUpdateObject(Machine machine, IMachineService _machineService);
        bool ValidDeleteObject(Machine machine, IRollerBuilderService _rollerBuilderService, ICoreIdentificationDetailService _coreIdentificationDetailService, IBlanketService _blanketService);
        bool isValid(Machine machine);
        string PrintError(Machine machine);
    }
}