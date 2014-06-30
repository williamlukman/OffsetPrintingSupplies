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
        RecoveryOrder CreateObject(RecoveryOrder coreIdentification);
        RecoveryOrder UpdateObject(RecoveryOrder coreIdentification);
        RecoveryOrder SoftDeleteObject(RecoveryOrder coreIdentification);
        bool DeleteObject(int Id);
    }
}