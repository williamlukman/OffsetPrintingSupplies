using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repository
{
    public class PurchaseInvoiceMigrationRepository : EfRepository<PurchaseInvoiceMigration>, IPurchaseInvoiceMigrationRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public PurchaseInvoiceMigrationRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<PurchaseInvoiceMigration> GetQueryable()
        {
            return FindAll();
        }

        public PurchaseInvoiceMigration GetObjectById(int Id)
        {
            PurchaseInvoiceMigration salesInvoiceMigration = Find(sim => sim.Id == Id);
            if (salesInvoiceMigration != null) { salesInvoiceMigration.Errors = new Dictionary<string, string>(); }
            return salesInvoiceMigration;
        }


        public PurchaseInvoiceMigration CreateObject(PurchaseInvoiceMigration salesInvoiceMigration)
        {
            salesInvoiceMigration.CreatedAt = DateTime.Now;
            return Create(salesInvoiceMigration);
        }
    }
}