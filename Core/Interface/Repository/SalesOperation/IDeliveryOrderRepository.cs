using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IDeliveryOrderRepository : IRepository<DeliveryOrder>
    {
        IQueryable<DeliveryOrder> GetQueryable();
        IList<DeliveryOrder> GetAll();
        IList<DeliveryOrder> GetAllByMonthCreated();
        DeliveryOrder GetObjectById(int Id);
        IQueryable<DeliveryOrder> GetQueryableObjectsBySalesOrderId(int salesOrderId);
        IList<DeliveryOrder> GetObjectsBySalesOrderId(int salesOrderId);
        IQueryable<DeliveryOrder> GetQueryableConfirmedObjects();
        IList<DeliveryOrder> GetConfirmedObjects();
        DeliveryOrder CreateObject(DeliveryOrder deliveryOrder);
        DeliveryOrder UpdateObject(DeliveryOrder deliveryOrder);
        DeliveryOrder SoftDeleteObject(DeliveryOrder deliveryOrder);
        bool DeleteObject(int Id);
        DeliveryOrder ConfirmObject(DeliveryOrder deliveryOrder);
        DeliveryOrder UnconfirmObject(DeliveryOrder deliveryOrder);
        DeliveryOrder SetInvoiceComplete(DeliveryOrder deliveryOrder);
        DeliveryOrder UnsetInvoiceComplete(DeliveryOrder deliveryOrder);
        string SetObjectCode();
    }
}