using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IDeliveryOrderDetailRepository : IRepository<DeliveryOrderDetail>
    {
        IQueryable<DeliveryOrderDetail> GetQueryable();
        IList<DeliveryOrderDetail> GetAll();
        IList<DeliveryOrderDetail> GetAllByMonthCreated();
        IList<DeliveryOrderDetail> GetObjectsByDeliveryOrderId(int deliveryOrderId);
        DeliveryOrderDetail GetObjectById(int Id);
        IList<DeliveryOrderDetail> GetObjectsBySalesOrderDetailId(int salesOrderDetailId);
        DeliveryOrderDetail CreateObject(DeliveryOrderDetail deliveryOrderDetail);
        DeliveryOrderDetail UpdateObject(DeliveryOrderDetail deliveryOrderDetail);
        DeliveryOrderDetail SoftDeleteObject(DeliveryOrderDetail deliveryOrderDetail);
        bool DeleteObject(int Id);
        DeliveryOrderDetail ConfirmObject(DeliveryOrderDetail deliveryOrderDetail);
        DeliveryOrderDetail UnconfirmObject(DeliveryOrderDetail deliveryOrderDetail);
        string SetObjectCode(string ParentCode);
    }
}