using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ISalesOrderRepository : IRepository<SalesOrder>
    {
        IQueryable<SalesOrder> GetQueryable();
        IList<SalesOrder> GetAll();
        IList<SalesOrder> GetAllByMonthCreated();
        IList<SalesOrder> GetConfirmedObjects();
        SalesOrder GetObjectById(int Id);
        IList<SalesOrder> GetObjectsByContactId(int contactId);
        SalesOrder CreateObject(SalesOrder salesOrder);
        SalesOrder UpdateObject(SalesOrder salesOrder);
        SalesOrder SoftDeleteObject(SalesOrder salesOrder);
        bool DeleteObject(int Id);
        SalesOrder ConfirmObject(SalesOrder salesOrder);
        SalesOrder UnconfirmObject(SalesOrder salesOrder);
        SalesOrder SetDeliveryComplete(SalesOrder salesOrder);
        SalesOrder UnsetDeliveryComplete(SalesOrder salesOrder);
        string SetObjectCode();
    }
}