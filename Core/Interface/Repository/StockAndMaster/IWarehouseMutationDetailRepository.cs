using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IWarehouseMutationDetailRepository : IRepository<WarehouseMutationDetail>
    {
        IQueryable<WarehouseMutationDetail> GetQueryable();
        IList<WarehouseMutationDetail> GetAll();
        IList<WarehouseMutationDetail> GetAllByMonthCreated();
        IList<WarehouseMutationDetail> GetObjectsByWarehouseMutationId(int warehouseMutationId);
        WarehouseMutationDetail GetObjectById(int Id);
        WarehouseMutationDetail CreateObject(WarehouseMutationDetail warehouseMutationDetail);
        WarehouseMutationDetail UpdateObject(WarehouseMutationDetail warehouseMutationDetail);
        WarehouseMutationDetail SoftDeleteObject(WarehouseMutationDetail warehouseMutationDetail);
        WarehouseMutationDetail ConfirmObject(WarehouseMutationDetail warehouseMutationDetail);
        WarehouseMutationDetail UnconfirmObject(WarehouseMutationDetail warehouseMutationDetail);
        bool DeleteObject(int Id);
        string SetObjectCode(string ParentCode);
    }
}