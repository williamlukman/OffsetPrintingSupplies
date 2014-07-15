using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Repository
{
    public interface ICoreIdentificationRepository : IRepository<CoreIdentification>
    {
        IList<CoreIdentification> GetAll();
        IList<CoreIdentification> GetAllObjectsInHouse();
        IList<CoreIdentification> GetAllObjectsByCustomerId(int CustomerId);
        IList<CoreIdentification> GetAllObjectsByWarehouseId(int WarehouseId);
        CoreIdentification GetObjectById(int Id);
        CoreIdentification CreateObject(CoreIdentification coreIdentification);
        CoreIdentification UpdateObject(CoreIdentification coreIdentification);
        CoreIdentification SoftDeleteObject(CoreIdentification coreIdentification);
        CoreIdentification ConfirmObject(CoreIdentification coreIdentification);
        CoreIdentification UnconfirmObject(CoreIdentification coreIdentification);
        bool DeleteObject(int Id);
    }
}