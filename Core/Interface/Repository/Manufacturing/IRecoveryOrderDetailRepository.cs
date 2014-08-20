using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IRecoveryOrderDetailRepository : IRepository<RecoveryOrderDetail>
    {
        IQueryable<RecoveryOrderDetail> GetQueryable();
        IList<RecoveryOrderDetail> GetAll();
        IList<RecoveryOrderDetail> GetObjectsByRecoveryOrderId(int recoveryOrderId);
        IList<RecoveryOrderDetail> GetObjectsByCoreIdentificationDetailId(int coreIdentificationDetailId);
        IList<RecoveryOrderDetail> GetObjectsByRollerBuilderId(int rollerBuilderId);
        RecoveryOrderDetail GetObjectById(int Id);
        RecoveryOrderDetail CreateObject(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail UpdateObject(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail SoftDeleteObject(RecoveryOrderDetail recoveryOrderDetail);

        RecoveryOrderDetail AddAccessory(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail RemoveAccessory(RecoveryOrderDetail recoveryOrderDetail);
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
        RecoveryOrderDetail UndoRejectObject(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail FinishObject(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail UnfinishObject(RecoveryOrderDetail recoveryOrderDetail);
        bool DeleteObject(int Id);
    }
}