using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ISalesQuotationRepository : IRepository<SalesQuotation>
    {
        IQueryable<SalesQuotation> GetQueryable();
        IList<SalesQuotation> GetAll();
        IList<SalesQuotation> GetAllByMonthCreated();
        IList<SalesQuotation> GetApprovedObjects();
        IList<SalesQuotation> GetObjectsByContactId(int contactId);
        SalesQuotation GetObjectById(int Id);
        SalesQuotation CreateObject(SalesQuotation salesQuotation);
        SalesQuotation UpdateObject(SalesQuotation salesQuotation);
        SalesQuotation SoftDeleteObject(SalesQuotation salesQuotation);
        bool DeleteObject(int Id);
        SalesQuotation ConfirmObject(SalesQuotation salesQuotation);
        SalesQuotation UnconfirmObject(SalesQuotation salesQuotation);
        SalesQuotation ApproveObject(SalesQuotation salesQuotation);
        SalesQuotation RejectObject(SalesQuotation salesQuotation);
        string SetObjectCode();
    }
}