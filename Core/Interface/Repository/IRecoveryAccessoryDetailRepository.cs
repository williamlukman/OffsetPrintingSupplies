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
        IList<RecoveryAccessoryDetail> GetObjectsByItemId(int ItemId);
        RecoveryAccessoryDetail GetObjectById(int Id);
        RecoveryAccessoryDetail CreateObject(RecoveryAccessoryDetail recoveryAccessoryDetail);
        RecoveryAccessoryDetail UpdateObject(RecoveryAccessoryDetail recoveryAccessoryDetail);
        RecoveryAccessoryDetail SoftDeleteObject(RecoveryAccessoryDetail recoveryAccessoryDetail);
        RecoveryAccessoryDetail FinishObject(RecoveryAccessoryDetail recoveryAccessoryDetail);
        RecoveryAccessoryDetail UnfinishObject(RecoveryAccessoryDetail recoveryAccessoryDetail);
        bool DeleteObject(int Id);
    }
}