using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ISalesDownPaymentRepository : IRepository<SalesDownPayment>
    {
        IQueryable<SalesDownPayment> GetQueryable();
        IList<SalesDownPayment> GetAll();
        IList<SalesDownPayment> GetAllByMonthCreated();
        SalesDownPayment GetObjectById(int Id);
        IList<SalesDownPayment> GetObjectsByContactId(int contactId);
        SalesDownPayment CreateObject(SalesDownPayment salesDownPayment);
        SalesDownPayment UpdateObject(SalesDownPayment salesDownPayment);
        SalesDownPayment SoftDeleteObject(SalesDownPayment salesDownPayment);
        bool DeleteObject(int Id);
        SalesDownPayment ConfirmObject(SalesDownPayment salesDownPayment);
        SalesDownPayment UnconfirmObject(SalesDownPayment salesDownPayment);
        string SetObjectCode();
    }
}