using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repository
{
    public class SalesDownPaymentRepository : EfRepository<SalesDownPayment>, ISalesDownPaymentRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public SalesDownPaymentRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<SalesDownPayment> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<SalesDownPayment> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<SalesDownPayment> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month && !x.IsDeleted).ToList();
        }

        public SalesDownPayment GetObjectById(int Id)
        {
            SalesDownPayment salesDownPayment = Find(x => x.Id == Id && !x.IsDeleted);
            if (salesDownPayment != null) { salesDownPayment.Errors = new Dictionary<string, string>(); }
            return salesDownPayment;
        }

        public IList<SalesDownPayment> GetObjectsByCashBankId(int cashBankId)
        {
            return FindAll(x => x.CashBankId == cashBankId && !x.IsDeleted).ToList();
        }

        public IList<SalesDownPayment> GetObjectsByContactId(int contactId)
        {
            return FindAll(x => x.ContactId == contactId && !x.IsDeleted).ToList();
        }

        public SalesDownPayment CreateObject(SalesDownPayment salesDownPayment)
        {
            salesDownPayment.Code = SetObjectCode();
            salesDownPayment.IsDeleted = false;
            salesDownPayment.IsConfirmed = false;
            salesDownPayment.IsReconciled = false;
            salesDownPayment.CreatedAt = DateTime.Now;
            return Create(salesDownPayment);
        }

        public SalesDownPayment UpdateObject(SalesDownPayment salesDownPayment)
        {
            salesDownPayment.UpdatedAt = DateTime.Now;
            Update(salesDownPayment);
            return salesDownPayment;
        }

        public SalesDownPayment SoftDeleteObject(SalesDownPayment salesDownPayment)
        {
            salesDownPayment.IsDeleted = true;
            salesDownPayment.DeletedAt = DateTime.Now;
            Update(salesDownPayment);
            return salesDownPayment;
        }

        public bool DeleteObject(int Id)
        {
            SalesDownPayment salesDownPayment = Find(x => x.Id == Id);
            return (Delete(salesDownPayment) == 1) ? true : false;
        }

        public SalesDownPayment ConfirmObject(SalesDownPayment salesDownPayment)
        {
            salesDownPayment.IsConfirmed = true;
            Update(salesDownPayment);
            return salesDownPayment;
        }

        public SalesDownPayment UnconfirmObject(SalesDownPayment salesDownPayment)
        {
            salesDownPayment.IsConfirmed = false;
            salesDownPayment.ConfirmationDate = null;
            UpdateObject(salesDownPayment);
            return salesDownPayment;
        }

        public SalesDownPayment ReconcileObject(SalesDownPayment salesDownPayment)
        {
            salesDownPayment.IsReconciled = true;
            Update(salesDownPayment);
            return salesDownPayment;
        }

        public SalesDownPayment UnreconcileObject(SalesDownPayment salesDownPayment)
        {
            salesDownPayment.IsReconciled = false;
            salesDownPayment.ReconciliationDate = null;
            UpdateObject(salesDownPayment);
            return salesDownPayment;
        }

        public string SetObjectCode()
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = DateTime.Today.Year.ToString() + "." + DateTime.Today.Month.ToString() + "." + totalnumberinthemonth;
            return Code;
        }
    }
}