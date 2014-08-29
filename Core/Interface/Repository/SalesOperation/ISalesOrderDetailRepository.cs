using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ISalesOrderDetailRepository : IRepository<SalesOrderDetail>
    {
        IQueryable<SalesOrderDetail> GetQueryable();
        IList<SalesOrderDetail> GetAll();
        IList<SalesOrderDetail> GetAllByMonthCreated();
        IQueryable<SalesOrderDetail> GetQueryableObjectsBySalesOrderId(int salesOrderId);
        IList<SalesOrderDetail> GetObjectsBySalesOrderId(int salesOrderId);
        IList<SalesOrderDetail> GetObjectsByItemId(int itemId);
        SalesOrderDetail GetObjectById(int Id);
        SalesOrderDetail CreateObject(SalesOrderDetail salesOrderDetail);
        SalesOrderDetail UpdateObject(SalesOrderDetail salesOrderDetail);
        SalesOrderDetail SoftDeleteObject(SalesOrderDetail salesOrderDetail);
        bool DeleteObject(int Id);
        SalesOrderDetail ConfirmObject(SalesOrderDetail salesOrderDetail);
        SalesOrderDetail UnconfirmObject(SalesOrderDetail salesOrderDetail);
        string SetObjectCode(string ParentCode);
    }
}