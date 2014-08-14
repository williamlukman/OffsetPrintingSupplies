using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;

namespace Data.Repository
{
    public class RetailSalesInvoiceRepository : EfRepository<RetailSalesInvoice>, IRetailSalesInvoiceRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public RetailSalesInvoiceRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IList<RetailSalesInvoice> GetAll()
        {
            return FindAll().ToList();
        }

        public RetailSalesInvoice GetObjectById(int Id)
        {
            RetailSalesInvoice retailSalesInvoice = Find(x => x.Id == Id && !x.IsDeleted);
            if (retailSalesInvoice != null) { retailSalesInvoice.Errors = new Dictionary<string, string>(); }
            return retailSalesInvoice;
        }

        public RetailSalesInvoice CreateObject(RetailSalesInvoice retailSalesInvoice)
        {
            retailSalesInvoice.IsDeleted = false;
            retailSalesInvoice.CreatedAt = DateTime.Now;
            return Create(retailSalesInvoice);
        }

        public RetailSalesInvoice UpdateObject(RetailSalesInvoice retailSalesInvoice)
        {
            retailSalesInvoice.UpdatedAt = DateTime.Now;
            Update(retailSalesInvoice);
            return retailSalesInvoice;
        }

        public RetailSalesInvoice ConfirmObject(RetailSalesInvoice retailSalesInvoice)
        {
            retailSalesInvoice.IsConfirmed = true;
            Update(retailSalesInvoice);
            return retailSalesInvoice;
        }

        public RetailSalesInvoice UnconfirmObject(RetailSalesInvoice retailSalesInvoice)
        {
            retailSalesInvoice.IsConfirmed = false;
            retailSalesInvoice.ConfirmationDate = null;
            Update(retailSalesInvoice);
            return retailSalesInvoice;
        }

        public RetailSalesInvoice PaidObject(RetailSalesInvoice retailSalesInvoice)
        {
            retailSalesInvoice.IsPaid = true;
            Update(retailSalesInvoice);
            return retailSalesInvoice;
        }

        public RetailSalesInvoice UnpaidObject(RetailSalesInvoice retailSalesInvoice)
        {
            retailSalesInvoice.IsPaid = false;
            Update(retailSalesInvoice);
            return retailSalesInvoice;
        }

        public RetailSalesInvoice SoftDeleteObject(RetailSalesInvoice retailSalesInvoice)
        {
            retailSalesInvoice.IsDeleted = true;
            retailSalesInvoice.DeletedAt = DateTime.Now;
            Update(retailSalesInvoice);
            return retailSalesInvoice;
        }

        public bool DeleteObject(int Id)
        {
            RetailSalesInvoice retailSalesInvoice = Find(x => x.Id == Id);
            return (Delete(retailSalesInvoice) == 1) ? true : false;
        }
    }
}
