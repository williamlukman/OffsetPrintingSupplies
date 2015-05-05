using Core.DomainModel;
using Core.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Context;
using System.Data;
using System.Data.Entity;

namespace Data.Repository
{
    public class ClosingReportRepository : EfRepository<ClosingReport>, IClosingReportRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public ClosingReportRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<ClosingReport> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<ClosingReport> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public ClosingReport GetObjectById(int Id)
        {
            ClosingReport closingReport = Find(x => x.Id == Id && !x.IsDeleted);
            if (closingReport != null) { closingReport.Errors = new Dictionary<string, string>(); }
            return closingReport;
        }


        public ClosingReport CreateObject(ClosingReport closingReport)
        {
            closingReport.IsDeleted = false;
            closingReport.CreatedAt = DateTime.Now;
            return Create(closingReport);
        }

        public ClosingReport UpdateObject(ClosingReport closingReport)
        {
            Update(closingReport);
            return closingReport;
        }

        public ClosingReport SoftDeleteObject(ClosingReport closingReport)
        {
            closingReport.IsDeleted = true;
            closingReport.DeletedAt = DateTime.Now;
            Update(closingReport);
            return closingReport;
        }

        public bool DeleteObject(int Id)
        {
            ClosingReport closingReport = Find(x => x.Id == Id);
            return (Delete(closingReport) == 1) ? true : false;
        }

    }
}