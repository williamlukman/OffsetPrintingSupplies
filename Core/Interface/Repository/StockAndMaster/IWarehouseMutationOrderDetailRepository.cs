using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IWarehouseMutationOrderDetailRepository : IRepository<WarehouseMutationOrderDetail>
    {
        IQueryable<WarehouseMutationOrderDetail> GetQueryable();
        IList<WarehouseMutationOrderDetail> GetAll();
        IList<WarehouseMutationOrderDetail> GetAllByMonthCreated();
        IQueryable<WarehouseMutationOrderDetail> GetQueryableObjectsByWarehouseMutationOrderId(int warehouseMutationOrderId);
        IList<WarehouseMutationOrderDetail> GetObjectsByWarehouseMutationOrderId(int warehouseMutationOrderId);
        WarehouseMutationOrderDetail GetObjectById(int Id);
        WarehouseMutationOrderDetail CreateObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail);
        WarehouseMutationOrderDetail UpdateObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail);
        WarehouseMutationOrderDetail SoftDeleteObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail);
        WarehouseMutationOrderDetail ConfirmObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail);
        WarehouseMutationOrderDetail UnconfirmObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail);
        bool DeleteObject(int Id);
        string SetObjectCode(string ParentCode);
    }
}