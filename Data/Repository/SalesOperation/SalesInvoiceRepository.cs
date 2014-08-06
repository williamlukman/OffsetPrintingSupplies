using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repository
{
    public class SalesInvoiceRepository : EfRepository<SalesInvoice>, ISalesInvoiceRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public SalesInvoiceRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IList<SalesInvoice> GetAll()
        {
            return FindAll(si => !si.IsDeleted).ToList();
        }

        public SalesInvoice GetObjectById(int Id)
        {
            SalesInvoice salesInvoice = Find(si => si.Id == Id && !si.IsDeleted);
            if (salesInvoice != null) { salesInvoice.Errors = new Dictionary<string, string>(); }
            return salesInvoice;
        }

        public IList<SalesInvoice> GetObjectsByContactId(int contactId)
        {
            return FindAll(si => si.ContactId == contactId && !si.IsDeleted).ToList();
        }

        public SalesInvoice CreateObject(SalesInvoice salesInvoice)
        {
            salesInvoice.Code = SetObjectCode();
            salesInvoice.IsDeleted = false;
            salesInvoice.IsConfirmed = false;
            salesInvoice.CreatedAt = DateTime.Now;
            return Create(salesInvoice);
        }

        public SalesInvoice UpdateObject(SalesInvoice salesInvoice)
        {
            salesInvoice.UpdatedAt = DateTime.Now;
            Update(salesInvoice);
            return salesInvoice;
        }

        public SalesInvoice SoftDeleteObject(SalesInvoice salesInvoice)
        {
            salesInvoice.IsDeleted = true;
            salesInvoice.DeletedAt = DateTime.Now;
            Update(salesInvoice);
            return salesInvoice;
        }

        public bool DeleteObject(int Id)
        {
            SalesInvoice si = Find(x => x.Id == Id);
            return (Delete(si) == 1) ? true : false;
        }

        public SalesInvoice ConfirmObject(SalesInvoice salesInvoice)
        {
            salesInvoice.IsConfirmed = true;
            Update(salesInvoice);
            return salesInvoice;
        }

        public SalesInvoice UnconfirmObject(SalesInvoice salesInvoice)
        {
            salesInvoice.IsConfirmed = false;
            Update(salesInvoice);
            return salesInvoice;
        }

        public string SetObjectCode()
        {
            // Code: #{year}/#{total_number
            int totalobject = FindAll().Count() + 1;
            string Code = "#" + DateTime.Now.Year.ToString() + "/#" + totalobject;
            return Code;
        }
    }
}