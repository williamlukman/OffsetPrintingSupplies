using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ITemporaryDeliveryOrderRepository : IRepository<TemporaryDeliveryOrder>
    {
        IQueryable<TemporaryDeliveryOrder> GetQueryable();
        IList<TemporaryDeliveryOrder> GetAll();
        IList<TemporaryDeliveryOrder> GetAllByMonthCreated();
        TemporaryDeliveryOrder GetObjectById(int Id);
        IList<TemporaryDeliveryOrder> GetObjectsByDeliveryOrderId(int deliveryOrderId);
        IList<TemporaryDeliveryOrder> GetObjectsByVirtualOrderId(int virtualOrderId);
        IList<TemporaryDeliveryOrder> GetConfirmedObjects();
        TemporaryDeliveryOrder CreateObject(TemporaryDeliveryOrder temporaryDeliveryOrder);
        TemporaryDeliveryOrder UpdateObject(TemporaryDeliveryOrder temporaryDeliveryOrder);
        TemporaryDeliveryOrder SoftDeleteObject(TemporaryDeliveryOrder temporaryDeliveryOrder);
        bool DeleteObject(int Id);
        TemporaryDeliveryOrder ConfirmObject(TemporaryDeliveryOrder temporaryDeliveryOrder);
        TemporaryDeliveryOrder UnconfirmObject(TemporaryDeliveryOrder temporaryDeliveryOrder);
        TemporaryDeliveryOrder SetReconcileComplete(TemporaryDeliveryOrder temporaryDeliveryOrder);
        TemporaryDeliveryOrder UnsetReconcileComplete(TemporaryDeliveryOrder temporaryDeliveryOrder);
        string SetObjectCode();
    }
}