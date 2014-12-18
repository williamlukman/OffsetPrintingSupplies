using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repository
{
    public class SalesInvoiceMigrationRepository : EfRepository<SalesInvoiceMigration>, ISalesInvoiceMigrationRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public SalesInvoiceMigrationRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<SalesInvoiceMigration> GetQueryable()
        {
            return FindAll();
        }

        public SalesInvoiceMigration GetObjectById(int Id)
        {
            SalesInvoiceMigration salesInvoiceMigration = Find(sim => sim.Id == Id);
            if (salesInvoiceMigration != null) { salesInvoiceMigration.Errors = new Dictionary<string, string>(); }
            return salesInvoiceMigration;
        }


        public SalesInvoiceMigration CreateObject(SalesInvoiceMigration salesInvoiceMigration)
        {
            salesInvoiceMigration.CreatedAt = DateTime.Now;
            return Create(salesInvoiceMigration);
        }
    }
}