using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Service
{
    public interface ICoreIdentificationDetailService
    {
        ICoreIdentificationDetailValidator GetValidator();
        ICoreIdentificationDetailRepository GetRepository();
        IList<CoreIdentificationDetail> GetAll();
        IList<CoreIdentificationDetail> GetObjectsByCoreIdentificationId(int CoreIdentificationId);
        IList<CoreIdentificationDetail> GetObjectsByCoreBuilderId(int CoreBuilderId);
        CoreIdentificationDetail GetObjectById(int Id);
        CoreIdentificationDetail GetObjectByDetailId(int CoreIdentificationId, int DetailId);
        CoreIdentificationDetail CreateObject(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService,
                                                     ICoreBuilderService _coreBuilderService, IRollerTypeService _rollerTypeService, IMachineService _machineService);
        CoreIdentificationDetail CreateObject(int DetailId, int MaterialCase, int CoreBuilderId, int RollerTypeId, int MachineId,
                                              decimal RD, decimal CD, decimal RL, decimal WL, decimal TL, ICoreIdentificationService _coreIdentificationService,
                                              ICoreBuilderService _coreBuilderService, IRollerTypeService _rollerTypeService, IMachineService _machineService);
        CoreIdentificationDetail UpdateObject(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService,
                                              ICoreBuilderService _coreBuilderService, IRollerTypeService _rollerTypeService, IMachineService _machineService);
        CoreIdentificationDetail SoftDeleteObject(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService,
                                                  IRecoveryOrderDetailService _recoveryOrderDetailService);
        bool DeleteObject(int Id);
    }
}