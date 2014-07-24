using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Repository
{
    public interface IDeliveryOrderDetailRepository : IRepository<DeliveryOrderDetail>
    {
        IList<DeliveryOrderDetail> GetObjectsByDeliveryOrderId(int deliveryOrderId);
        DeliveryOrderDetail GetObjectById(int Id);
        DeliveryOrderDetail GetObjectBySalesOrderDetailId(int salesOrderDetailId);
        DeliveryOrderDetail CreateObject(DeliveryOrderDetail deliveryOrderDetail);
        DeliveryOrderDetail UpdateObject(DeliveryOrderDetail deliveryOrderDetail);
        DeliveryOrderDetail SoftDeleteObject(DeliveryOrderDetail deliveryOrderDetail);
        bool DeleteObject(int Id);
        DeliveryOrderDetail FinishObject(DeliveryOrderDetail deliveryOrderDetail);
        DeliveryOrderDetail UnfinishObject(DeliveryOrderDetail deliveryOrderDetail);
        string SetObjectCode(string ParentCode);
    }
}