using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IVirtualOrderRepository : IRepository<VirtualOrder>
    {
        IQueryable<VirtualOrder> GetQueryable();
        IList<VirtualOrder> GetAll();
        IList<VirtualOrder> GetAllByMonthCreated();
        IList<VirtualOrder> GetConfirmedObjects();
        VirtualOrder GetObjectById(int Id);
        IList<VirtualOrder> GetObjectsByContactId(int contactId);
        VirtualOrder CreateObject(VirtualOrder virtualOrder);
        VirtualOrder UpdateObject(VirtualOrder virtualOrder);
        VirtualOrder SoftDeleteObject(VirtualOrder virtualOrder);
        bool DeleteObject(int Id);
        VirtualOrder ConfirmObject(VirtualOrder virtualOrder);
        VirtualOrder UnconfirmObject(VirtualOrder virtualOrder);
        VirtualOrder SetDeliveryComplete(VirtualOrder virtualOrder);
        VirtualOrder UnsetDeliveryComplete(VirtualOrder virtualOrder);
        string SetObjectCode();
    }
}