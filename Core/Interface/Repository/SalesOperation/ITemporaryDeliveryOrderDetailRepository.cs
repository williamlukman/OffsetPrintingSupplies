using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ITemporaryDeliveryOrderDetailRepository : IRepository<TemporaryDeliveryOrderDetail>
    {
        IQueryable<TemporaryDeliveryOrderDetail> GetQueryable();
        IList<TemporaryDeliveryOrderDetail> GetAll();
        IList<TemporaryDeliveryOrderDetail> GetAllByMonthCreated();
        IList<TemporaryDeliveryOrderDetail> GetObjectsByTemporaryDeliveryOrderId(int temporaryDeliveryOrderId);
        TemporaryDeliveryOrderDetail GetObjectById(int Id);
        IList<TemporaryDeliveryOrderDetail> GetObjectsBySalesOrderDetailId(int salesOrderDetailId);
        IList<TemporaryDeliveryOrderDetail> GetObjectsByVirtualOrderDetailId(int salesOrderDetailId);
        TemporaryDeliveryOrderDetail CreateObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail);
        TemporaryDeliveryOrderDetail UpdateObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail);
        TemporaryDeliveryOrderDetail SoftDeleteObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail);
        bool DeleteObject(int Id);
        TemporaryDeliveryOrderDetail ConfirmObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail);
        TemporaryDeliveryOrderDetail UnconfirmObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail);
        string SetObjectCode(string ParentCode);
    }
}