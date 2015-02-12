using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ISalesInvoiceMigrationRepository : IRepository<SalesInvoiceMigration>
    {
        IQueryable<SalesInvoiceMigration> GetQueryable();
        SalesInvoiceMigration GetObjectById(int Id);
        SalesInvoiceMigration CreateObject(SalesInvoiceMigration salesInvoice);
    }
}