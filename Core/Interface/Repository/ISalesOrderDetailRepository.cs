using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Repository
{
    public interface ISalesOrderDetailRepository : IRepository<SalesOrderDetail>
    {
        IList<SalesOrderDetail> GetObjectsBySalesOrderId(int salesOrderId);
        SalesOrderDetail GetObjectById(int Id);
        SalesOrderDetail CreateObject(SalesOrderDetail salesOrderDetail);
        SalesOrderDetail UpdateObject(SalesOrderDetail salesOrderDetail);
        SalesOrderDetail SoftDeleteObject(SalesOrderDetail salesOrderDetail);
        bool DeleteObject(int Id);
        SalesOrderDetail FinishObject(SalesOrderDetail salesOrderDetail);
        SalesOrderDetail UnfinishObject(SalesOrderDetail salesOrderDetail);
        SalesOrderDetail DeliverObject(SalesOrderDetail salesOrderDetail);
        string SetObjectCode(string ParentCode);
    }
}