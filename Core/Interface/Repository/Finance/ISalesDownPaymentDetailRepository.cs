using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ISalesDownPaymentDetailRepository : IRepository<SalesDownPaymentDetail>
    {
        IQueryable<SalesDownPaymentDetail> GetQueryable();
        IList<SalesDownPaymentDetail> GetAll();
        IList<SalesDownPaymentDetail> GetAllByMonthCreated();
        IList<SalesDownPaymentDetail> GetObjectsBySalesDownPaymentId(int salesDownPaymentId);
        IList<SalesDownPaymentDetail> GetObjectsByReceivableId(int receivableId);
        SalesDownPaymentDetail GetObjectById(int Id);
        SalesDownPaymentDetail CreateObject(SalesDownPaymentDetail salesDownPaymentDetail);
        SalesDownPaymentDetail UpdateObject(SalesDownPaymentDetail salesDownPaymentDetail);
        SalesDownPaymentDetail SoftDeleteObject(SalesDownPaymentDetail salesDownPaymentDetail);
        bool DeleteObject(int Id);
        SalesDownPaymentDetail ConfirmObject(SalesDownPaymentDetail salesDownPaymentDetail);
        SalesDownPaymentDetail UnconfirmObject(SalesDownPaymentDetail salesDownPaymentDetail);
        string SetObjectCode(string ParentCode);
    }
}