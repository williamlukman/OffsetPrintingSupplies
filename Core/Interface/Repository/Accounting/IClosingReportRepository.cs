using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IClosingReportRepository : IRepository<ClosingReport>
    {
        IQueryable<ClosingReport> GetQueryable();
        IList<ClosingReport> GetAll();
        ClosingReport GetObjectById(int Id);
        ClosingReport CreateObject(ClosingReport closingReport);
        ClosingReport SoftDeleteObject(ClosingReport closingReport);
        ClosingReport UpdateObject(ClosingReport closingReport); 
        bool DeleteObject(int Id);
    }
}