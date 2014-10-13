using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ISalesAllowanceDetailRepository : IRepository<SalesAllowanceDetail>
    {
        IQueryable<SalesAllowanceDetail> GetQueryable();
        IList<SalesAllowanceDetail> GetAll();
        IList<SalesAllowanceDetail> GetAllByMonthCreated();
        IList<SalesAllowanceDetail> GetObjectsBySalesAllowanceId(int salesAllowanceId);
        IList<SalesAllowanceDetail> GetObjectsByReceivableId(int receivableId);
        SalesAllowanceDetail GetObjectById(int Id);
        SalesAllowanceDetail CreateObject(SalesAllowanceDetail salesAllowanceDetail);
        SalesAllowanceDetail UpdateObject(SalesAllowanceDetail salesAllowanceDetail);
        SalesAllowanceDetail SoftDeleteObject(SalesAllowanceDetail salesAllowanceDetail);
        bool DeleteObject(int Id);
        SalesAllowanceDetail ConfirmObject(SalesAllowanceDetail salesAllowanceDetail);
        SalesAllowanceDetail UnconfirmObject(SalesAllowanceDetail salesAllowanceDetail);
        string SetObjectCode(string ParentCode);
    }
}