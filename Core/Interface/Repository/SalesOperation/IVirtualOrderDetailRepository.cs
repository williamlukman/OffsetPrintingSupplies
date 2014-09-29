using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IVirtualOrderDetailRepository : IRepository<VirtualOrderDetail>
    {
        IQueryable<VirtualOrderDetail> GetQueryable();
        IList<VirtualOrderDetail> GetAll();
        IList<VirtualOrderDetail> GetAllByMonthCreated();
        IList<VirtualOrderDetail> GetObjectsByVirtualOrderId(int virtualOrderId);
        IList<VirtualOrderDetail> GetObjectsByItemId(int itemId);
        VirtualOrderDetail GetObjectById(int Id);
        VirtualOrderDetail CreateObject(VirtualOrderDetail virtualOrderDetail);
        VirtualOrderDetail UpdateObject(VirtualOrderDetail virtualOrderDetail);
        VirtualOrderDetail SoftDeleteObject(VirtualOrderDetail virtualOrderDetail);
        bool DeleteObject(int Id);
        VirtualOrderDetail ConfirmObject(VirtualOrderDetail virtualOrderDetail);
        VirtualOrderDetail UnconfirmObject(VirtualOrderDetail virtualOrderDetail);
        string SetObjectCode(string ParentCode);
    }
}