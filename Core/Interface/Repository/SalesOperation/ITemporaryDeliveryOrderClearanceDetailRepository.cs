using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ITemporaryDeliveryOrderClearanceDetailRepository : IRepository<TemporaryDeliveryOrderClearanceDetail>
    {
        IQueryable<TemporaryDeliveryOrderClearanceDetail> GetQueryable();
        IList<TemporaryDeliveryOrderClearanceDetail> GetAll();
        IList<TemporaryDeliveryOrderClearanceDetail> GetAllByMonthCreated();
        IList<TemporaryDeliveryOrderClearanceDetail> GetObjectsByTemporaryDeliveryOrderClearanceId(int temporaryDeliveryOrderClearanceId);
        TemporaryDeliveryOrderClearanceDetail GetObjectByCode(string Code); 
        TemporaryDeliveryOrderClearanceDetail GetObjectById(int Id);
        IList<TemporaryDeliveryOrderClearanceDetail> GetObjectsByTemporaryDeliveryOrderDetailId(int temporaryDeliveryOrderDetailId);
        TemporaryDeliveryOrderClearanceDetail CreateObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail);
        TemporaryDeliveryOrderClearanceDetail UpdateObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail);
        TemporaryDeliveryOrderClearanceDetail SoftDeleteObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail);
        bool DeleteObject(int Id);
        TemporaryDeliveryOrderClearanceDetail ConfirmObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail);
        TemporaryDeliveryOrderClearanceDetail UnconfirmObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail);
        string SetObjectCode(string ParentCode);
    }
}