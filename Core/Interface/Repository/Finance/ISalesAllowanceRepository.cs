using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ISalesAllowanceRepository : IRepository<SalesAllowance>
    {
        IQueryable<SalesAllowance> GetQueryable();
        IList<SalesAllowance> GetAll();
        IList<SalesAllowance> GetAllByMonthCreated();
        SalesAllowance GetObjectById(int Id);
        IList<SalesAllowance> GetObjectsByCashBankId(int cashBankId);
        IList<SalesAllowance> GetObjectsByContactId(int contactId);
        SalesAllowance CreateObject(SalesAllowance salesAllowance);
        SalesAllowance UpdateObject(SalesAllowance salesAllowance);
        SalesAllowance SoftDeleteObject(SalesAllowance salesAllowance);
        bool DeleteObject(int Id);
        SalesAllowance ConfirmObject(SalesAllowance salesAllowance);
        SalesAllowance UnconfirmObject(SalesAllowance salesAllowance);
        SalesAllowance ReconcileObject(SalesAllowance salesAllowance);
        SalesAllowance UnreconcileObject(SalesAllowance salesAllowance);
        string SetObjectCode();
    }
}