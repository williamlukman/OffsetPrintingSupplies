using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IPurchaseInvoiceMigrationRepository : IRepository<PurchaseInvoiceMigration>
    {
        IQueryable<PurchaseInvoiceMigration> GetQueryable();
        PurchaseInvoiceMigration GetObjectById(int Id);
        PurchaseInvoiceMigration CreateObject(PurchaseInvoiceMigration purchaseInvoice);
    }
}