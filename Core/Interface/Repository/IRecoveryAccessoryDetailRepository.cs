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
        RecoveryAccessoryDetail CreateObject(RecoveryAccessoryDetail rollerRecoveryAccessoryDetail);
        RecoveryAccessoryDetail UpdateObject(RecoveryAccessoryDetail rollerRecoveryAccessoryDetail);
        RecoveryAccessoryDetail SoftDeleteObject(RecoveryAccessoryDetail rollerRecoveryAccessoryDetail);
        bool DeleteObject(int Id);
    }
}