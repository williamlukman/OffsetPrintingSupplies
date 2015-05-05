using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IClosingReportService
    {
        IQueryable<ClosingReport> GetQueryable();
        IClosingReportValidator GetValidator();
        IList<ClosingReport> GetAll();
        ClosingReport GetObjectById(int Id);
        ClosingReport CreateObject(ClosingReport closingReport, IAccountService _accountService);
        ClosingReport SoftDeleteObject(ClosingReport closingReport);
        ClosingReport UpdateObject(ClosingReport closingReport); 
        bool DeleteObject(int Id);
    }
}