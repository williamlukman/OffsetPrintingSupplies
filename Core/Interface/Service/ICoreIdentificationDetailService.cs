using Core.DomainModel;
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
        IList<CoreIdentificationDetail> GetAll();
        IList<CoreIdentificationDetail> GetObjectsByCoreIdentificationId(int CoreIdentificationId);
        CoreIdentificationDetail GetObjectById(int Id);
        CoreIdentificationDetail GetObjectByDetailId(int CoreIdentificationId, int DetailId);
        CoreIdentificationDetail CreateObject(CoreIdentificationDetail coreIdentificationDetail);
        CoreIdentificationDetail CreateObject(int DetailId, int MaterialCase, int CoreBuilderId, int RollerTypeId, int MachineId,
                                              decimal RD, decimal CD, decimal RL, decimal WL, decimal TL);
        CoreIdentificationDetail UpdateObject(CoreIdentificationDetail coreIdentificationDetail);
        CoreIdentificationDetail SoftDeleteObject(CoreIdentificationDetail coreIdentificationDetail, IRecoveryOrderDetailService _recoveryOrderDetailService);
        bool DeleteObject(int Id);
    }
}