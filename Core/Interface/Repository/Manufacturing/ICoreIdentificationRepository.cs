using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ICoreIdentificationRepository : IRepository<CoreIdentification>
    {
        IList<CoreIdentification> GetAll();
        IList<CoreIdentification> GetAllObjectsInHouse();
        IList<CoreIdentification> GetAllObjectsByContactId(int ContactId);
        IList<CoreIdentification> GetAllObjectsByWarehouseId(int WarehouseId);
        IList<CoreIdentification> GetConfirmedObjects();
        IList<CoreIdentification> GetConfirmedNotCompletedObjects();
        CoreIdentification GetObjectById(int Id);
        CoreIdentification CreateObject(CoreIdentification coreIdentification);
        CoreIdentification UpdateObject(CoreIdentification coreIdentification);
        CoreIdentification SoftDeleteObject(CoreIdentification coreIdentification);
        CoreIdentification ConfirmObject(CoreIdentification coreIdentification);
        CoreIdentification UnconfirmObject(CoreIdentification coreIdentification);
        CoreIdentification CompleteObject(CoreIdentification coreIdentification);
        bool DeleteObject(int Id);
    }
}