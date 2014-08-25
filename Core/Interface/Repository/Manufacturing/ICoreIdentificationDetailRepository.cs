using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ICoreIdentificationDetailRepository : IRepository<CoreIdentificationDetail>
    {
        IQueryable<CoreIdentificationDetail> GetQueryable();
        IList<CoreIdentificationDetail> GetAll();
        IList<CoreIdentificationDetail> GetObjectsByCoreIdentificationId(int CoreIdentificationId);
        IList<CoreIdentificationDetail> GetObjectsByCoreBuilderId(int CoreBuilderId);
        IList<CoreIdentificationDetail> GetObjectsByRollerTypeId(int rollerTypeId);
        IList<CoreIdentificationDetail> GetObjectsByMachineId(int machineId);
        CoreIdentificationDetail GetObjectById(int Id);
        CoreIdentificationDetail CreateObject(CoreIdentificationDetail coreIdentificationDetail);
        CoreIdentificationDetail UpdateObject(CoreIdentificationDetail coreIdentificationDetail);
        CoreIdentificationDetail SoftDeleteObject(CoreIdentificationDetail coreIdentificationDetail);
        CoreIdentificationDetail SetJobScheduled(CoreIdentificationDetail coreIdentificationDetail);
        CoreIdentificationDetail UnsetJobScheduled(CoreIdentificationDetail coreIdentificationDetail);
        CoreIdentificationDetail ConfirmObject(CoreIdentificationDetail coreIdentificationDetail);
        CoreIdentificationDetail UnconfirmObject(CoreIdentificationDetail coreIdentificationDetail);
        CoreIdentificationDetail DeliverObject(CoreIdentificationDetail coreIdentificationDetail);
        CoreIdentificationDetail UndoDeliverObject(CoreIdentificationDetail coreIdentificationDetail);
        CoreIdentificationDetail BuildRoller(CoreIdentificationDetail coreIdentificationDetail);
        bool DeleteObject(int Id);
    }
}