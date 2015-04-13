using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IBlanketWarehouseMutationDetailRepository : IRepository<BlanketWarehouseMutationDetail>
    {
        IQueryable<BlanketWarehouseMutationDetail> GetQueryable();
        IList<BlanketWarehouseMutationDetail> GetAll();
        IList<BlanketWarehouseMutationDetail> GetAllByMonthCreated();
        IList<BlanketWarehouseMutationDetail> GetObjectsByBlanketWarehouseMutationId(int blanketWarehouseMutationId);
        BlanketWarehouseMutationDetail GetObjectByBlanketOrderDetailId(int blanketOrderDetailId);
        BlanketWarehouseMutationDetail GetObjectById(int Id);
        BlanketWarehouseMutationDetail CreateObject(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail);
        BlanketWarehouseMutationDetail UpdateObject(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail);
        BlanketWarehouseMutationDetail SoftDeleteObject(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail);
        BlanketWarehouseMutationDetail ConfirmObject(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail);
        BlanketWarehouseMutationDetail UnconfirmObject(BlanketWarehouseMutationDetail blanketWarehouseMutationDetail);
        bool DeleteObject(int Id);
        string SetObjectCode(string ParentCode);
    }
}