using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ITemporaryDeliveryOrderClearanceRepository : IRepository<TemporaryDeliveryOrderClearance>
    {
        IQueryable<TemporaryDeliveryOrderClearance> GetQueryable();
        IList<TemporaryDeliveryOrderClearance> GetAll();
        IList<TemporaryDeliveryOrderClearance> GetAllByMonthCreated();
        TemporaryDeliveryOrderClearance GetObjectById(int Id);
        IList<TemporaryDeliveryOrderClearance> GetObjectsByTemporaryDeliveryOrderId(int TemporaryDeliveryOrderId);
        IList<TemporaryDeliveryOrderClearance> GetConfirmedObjects();
        TemporaryDeliveryOrderClearance CreateObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance);
        TemporaryDeliveryOrderClearance UpdateObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance);
        TemporaryDeliveryOrderClearance SoftDeleteObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance);
        bool DeleteObject(int Id);
        TemporaryDeliveryOrderClearance ConfirmObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance);
        TemporaryDeliveryOrderClearance UnconfirmObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance);
        string SetObjectCode();
    }
}