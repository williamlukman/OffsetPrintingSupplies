using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ISalesQuotationDetailRepository : IRepository<SalesQuotationDetail>
    {
        IQueryable<SalesQuotationDetail> GetQueryable();
        IList<SalesQuotationDetail> GetAll();
        IList<SalesQuotationDetail> GetAllByMonthCreated();
        IList<SalesQuotationDetail> GetObjectsBySalesQuotationId(int salesQuotationId);
        IList<SalesQuotationDetail> GetObjectsByItemId(int itemId);
        SalesQuotationDetail GetObjectById(int Id);
        SalesQuotationDetail CreateObject(SalesQuotationDetail salesQuotationDetail);
        SalesQuotationDetail UpdateObject(SalesQuotationDetail salesQuotationDetail);
        SalesQuotationDetail SoftDeleteObject(SalesQuotationDetail salesQuotationDetail);
        bool DeleteObject(int Id);
        SalesQuotationDetail ConfirmObject(SalesQuotationDetail salesQuotationDetail);
        SalesQuotationDetail UnconfirmObject(SalesQuotationDetail salesQuotationDetail);
        string SetObjectCode(string ParentCode);
    }
}