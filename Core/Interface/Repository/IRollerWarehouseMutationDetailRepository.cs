using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Repository
{
    public interface IRollerWarehouseMutationDetailRepository : IRepository<RollerWarehouseMutationDetail>
    {
        IList<RollerWarehouseMutationDetail> GetAll();
        IList<RollerWarehouseMutationDetail> GetObjectsByRollerWarehouseMutationId(int rollerWarehouseMutationId);
        RollerWarehouseMutationDetail GetObjectByCoreIdentificationDetailId(int coreIdentificationDetailId);
        RollerWarehouseMutationDetail GetObjectById(int Id);
        RollerWarehouseMutationDetail CreateObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail);
        RollerWarehouseMutationDetail UpdateObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail);
        RollerWarehouseMutationDetail SoftDeleteObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail);
        RollerWarehouseMutationDetail FinishObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail);
        RollerWarehouseMutationDetail UnfinishObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail);
        bool DeleteObject(int Id);
    }
}