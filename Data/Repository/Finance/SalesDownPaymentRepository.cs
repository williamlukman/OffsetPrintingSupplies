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
            SalesDownPayment purchaseDownPayment = Find(x => x.Id == Id && !x.IsDeleted);
            if (purchaseDownPayment != null) { purchaseDownPayment.Errors = new Dictionary<string, string>(); }
            return purchaseDownPayment;
        }

        public IList<SalesDownPayment> GetObjectsByCashBankId(int cashBankId)
        {
            return FindAll(x => x.CashBankId == cashBankId && !x.IsDeleted).ToList();
        }

        public IList<SalesDownPayment> GetObjectsByContactId(int contactId)
        {
            return FindAll(x => x.ContactId == contactId && !x.IsDeleted).ToList();
        }

        public SalesDownPayment CreateObject(SalesDownPayment purchaseDownPayment)
        {
            purchaseDownPayment.Code = SetObjectCode();
            purchaseDownPayment.IsDeleted = false;
            purchaseDownPayment.IsConfirmed = false;
            purchaseDownPayment.CreatedAt = DateTime.Now;
            return Create(purchaseDownPayment);
        }

        public SalesDownPayment UpdateObject(SalesDownPayment purchaseDownPayment)
        {
            purchaseDownPayment.UpdatedAt = DateTime.Now;
            Update(purchaseDownPayment);
            return purchaseDownPayment;
        }

        public SalesDownPayment SoftDeleteObject(SalesDownPayment purchaseDownPayment)
        {
            purchaseDownPayment.IsDeleted = true;
            purchaseDownPayment.DeletedAt = DateTime.Now;
            Update(purchaseDownPayment);
            return purchaseDownPayment;
        }

        public bool DeleteObject(int Id)
        {
            SalesDownPayment purchaseDownPayment = Find(x => x.Id == Id);
            return (Delete(purchaseDownPayment) == 1) ? true : false;
        }

        public SalesDownPayment ConfirmObject(SalesDownPayment purchaseDownPayment)
        {
            purchaseDownPayment.IsConfirmed = true;
            Update(purchaseDownPayment);
            return purchaseDownPayment;
        }

        public SalesDownPayment UnconfirmObject(SalesDownPayment purchaseDownPayment)
        {
            purchaseDownPayment.IsConfirmed = false;
            purchaseDownPayment.ConfirmationDate = null;
            UpdateObject(purchaseDownPayment);
            return purchaseDownPayment;
        }

        public string SetObjectCode()
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = DateTime.Today.Year.ToString() + "." + DateTime.Today.Month.ToString() + "." + totalnumberinthemonth;
            return Code;
        }
    }
}