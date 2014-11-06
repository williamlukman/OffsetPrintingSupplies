using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ISalesDownPaymentAllocationRepository : IRepository<SalesDownPaymentAllocation>
    {
        IQueryable<SalesDownPaymentAllocation> GetQueryable();
        IList<SalesDownPaymentAllocation> GetAll();
        IList<SalesDownPaymentAllocation> GetAllByMonthCreated();
        SalesDownPaymentAllocation GetObjectById(int Id);
        SalesDownPaymentAllocation GetObjectByPayableId(int PayableId);
        IList<SalesDownPaymentAllocation> GetObjectsByContactId(int contactId);
        SalesDownPaymentAllocation CreateObject(SalesDownPaymentAllocation salesDownPaymentAllocation);
        SalesDownPaymentAllocation UpdateObject(SalesDownPaymentAllocation salesDownPaymentAllocation);
        SalesDownPaymentAllocation SoftDeleteObject(SalesDownPaymentAllocation salesDownPaymentAllocation);
        bool DeleteObject(int Id);
        SalesDownPaymentAllocation ConfirmObject(SalesDownPaymentAllocation salesDownPaymentAllocation);
        SalesDownPaymentAllocation UnconfirmObject(SalesDownPaymentAllocation salesDownPaymentAllocation);
        string SetObjectCode();
    }
}