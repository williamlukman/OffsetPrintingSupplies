using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Repository
{
    public interface IRecoveryAccessoryDetailRepository : IRepository<RecoveryAccessoryDetail>
    {
        IList<RecoveryAccessoryDetail> GetAll();
        IList<RecoveryAccessoryDetail> GetObjectsByRecoveryOrderDetailId(int recoveryOrderDetailId);
        RecoveryAccessoryDetail GetObjectById(int Id);
        RecoveryAccessoryDetail CreateObject(RecoveryAccessoryDetail recoveryAccessoryDetail);
        RecoveryAccessoryDetail UpdateObject(RecoveryAccessoryDetail recoveryAccessoryDetail);
        RecoveryAccessoryDetail SoftDeleteObject(RecoveryAccessoryDetail recoveryAccessoryDetail);
        RecoveryAccessoryDetail ConfirmObject(RecoveryAccessoryDetail recoveryAccessoryDetail);
        RecoveryAccessoryDetail UnconfirmObject(RecoveryAccessoryDetail recoveryAccessoryDetail);
        bool DeleteObject(int Id);
    }
}