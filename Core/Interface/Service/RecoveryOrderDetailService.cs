using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Service
{
    public interface IRecoveryOrderDetailService
    {
        IRecoveryOrderDetailValidator GetValidator();
        IList<RecoveryOrderDetail> GetAll();
        IList<RecoveryOrderDetail> GetObjectsByRecoveryOrderId(int recoveryOrderId);
        RecoveryOrderDetail GetObjectById(int Id);
        RecoveryOrderDetail CreateObject(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail UpdateObject(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail SoftDeleteObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService);

        RecoveryOrderDetail DisassembleObject(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail StripAndGlueObject(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail WrapObject(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail VulcanizeObject(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail FaceOffObject(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail ConventionalGrindObject(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail CWCGrindObject(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail PolishAndQCObject(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail PackageObject(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail RejectObject(RecoveryOrderDetail recoveryOrderDetail);
        bool DeleteObject(int Id, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService);
    }
}