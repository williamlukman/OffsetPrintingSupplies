using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ISalesDownPaymentAllocationDetailRepository : IRepository<SalesDownPaymentAllocationDetail>
    {
        IQueryable<SalesDownPaymentAllocationDetail> GetQueryable();
        IList<SalesDownPaymentAllocationDetail> GetAll();
        IList<SalesDownPaymentAllocationDetail> GetAllByMonthCreated();
        IList<SalesDownPaymentAllocationDetail> GetObjectsBySalesDownPaymentAllocationId(int salesDownPaymentAllocationId);
        IList<SalesDownPaymentAllocationDetail> GetObjectsByReceivableId(int receivableId);
        SalesDownPaymentAllocationDetail GetObjectById(int Id);
        SalesDownPaymentAllocationDetail CreateObject(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail);
        SalesDownPaymentAllocationDetail UpdateObject(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail);
        SalesDownPaymentAllocationDetail SoftDeleteObject(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail);
        bool DeleteObject(int Id);
        SalesDownPaymentAllocationDetail ConfirmObject(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail);
        SalesDownPaymentAllocationDetail UnconfirmObject(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail);
        string SetObjectCode(string ParentCode);
    }
}