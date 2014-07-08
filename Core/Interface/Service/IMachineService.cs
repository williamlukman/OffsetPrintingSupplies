using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Service
{
    public interface IMachineService
    {
        IMachineValidator GetValidator();
        IList<Machine> GetAll();
        Machine GetObjectById(int Id);
        Machine GetObjectByCode(string Code);
        Machine GetObjectByName(string Name);
        Machine CreateObject(Machine machine);
        Machine UpdateObject(Machine machine);
        Machine SoftDeleteObject(Machine machine, IRollerBuilderService _rollerBuilderService,
                                 ICoreIdentificationDetailService _coreIdentificationDetailService, IBarringService _barringService);
        bool DeleteObject(int Id);
        bool IsCodeDuplicated(Machine machine);
    }
}