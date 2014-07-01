using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Service
{
    public interface IRollerTypeService
    {
        IRollerTypeValidator GetValidator();
        IList<RollerType> GetAll();
        RollerType GetObjectById(int Id);
        RollerType GetObjectByName(string Name);
        RollerType CreateObject(RollerType rollerType);
        RollerType CreateObject(string Name, string Description);
        RollerType UpdateObject(RollerType rollerType);
        RollerType SoftDeleteObject(RollerType rollerType, IRollerBuilderService _rollerBuilderService, ICoreIdentificationDetailService _coreIdentificationDetailService);
        bool DeleteObject(int Id);
        bool IsNameDuplicated(RollerType rollerType);
    }
}