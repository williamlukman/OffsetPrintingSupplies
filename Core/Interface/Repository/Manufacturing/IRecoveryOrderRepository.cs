using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IRecoveryOrderRepository : IRepository<RecoveryOrder>
    {
        IQueryable<RecoveryOrder> GetQueryable();
        IList<RecoveryOrder> GetAll();
        IList<RecoveryOrder> GetAllObjectsInHouse();
        IList<RecoveryOrder> GetAllObjectsByContactId(int ContactId);
        IList<RecoveryOrder> GetObjectsByCoreIdentificationId(int coreIdentificationId);
        RecoveryOrder GetObjectById(int Id);
        RecoveryOrder CreateObject(RecoveryOrder recoveryOrder);
        RecoveryOrder UpdateObject(RecoveryOrder recoveryOrder);
        RecoveryOrder SoftDeleteObject(RecoveryOrder recoveryOrder);
        RecoveryOrder ConfirmObject(RecoveryOrder recoveryOrder);
        RecoveryOrder UnconfirmObject(RecoveryOrder recoveryOrder);
        RecoveryOrder CompleteObject(RecoveryOrder recoveryOrder);
        RecoveryOrder AdjustQuantity(RecoveryOrder recoveryOrder);
        bool DeleteObject(int Id);
    }
}