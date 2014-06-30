using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Repository
{
    public interface IRecoveryOrderDetailRepository : IRepository<RecoveryOrderDetail>
    {
        IList<RecoveryOrderDetail> GetAll();
        IList<RecoveryOrderDetail> GetObjectsByRecoveryOrderId(int recoveryOrderId);
        RecoveryOrderDetail GetObjectById(int Id);
        RecoveryOrderDetail CreateObject(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail UpdateObject(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail SoftDeleteObject(RecoveryOrderDetail recoveryOrderDetail);
        bool DeleteObject(int Id);
    }
}