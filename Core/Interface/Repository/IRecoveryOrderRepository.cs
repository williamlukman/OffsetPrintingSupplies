using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Repository
{
    public interface IRecoveryOrderRepository : IRepository<RecoveryOrder>
    {
        IList<RecoveryOrder> GetAll();
        IList<RecoveryOrder> GetAllObjectsInHouse();
        IList<RecoveryOrder> GetAllObjectsByCustomerId(int CustomerId);
        RecoveryOrder GetObjectById(int Id);
        RecoveryOrder CreateObject(RecoveryOrder recoveryOrder);
        RecoveryOrder UpdateObject(RecoveryOrder recoveryOrder);
        RecoveryOrder SoftDeleteObject(RecoveryOrder recoveryOrder);
        RecoveryOrder ConfirmObject(RecoveryOrder recoveryOrder);
        RecoveryOrder UnconfirmObject(RecoveryOrder recoveryOrder);
        RecoveryOrder FinishObject(RecoveryOrder recoveryOrder);
        RecoveryOrder UnfinishObject(RecoveryOrder recoveryOrder);
        bool DeleteObject(int Id);
    }
}