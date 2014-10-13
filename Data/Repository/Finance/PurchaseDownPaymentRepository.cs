using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repository
{
    public class PurchaseDownPaymentRepository : EfRepository<PurchaseDownPayment>, IPurchaseDownPaymentRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public PurchaseDownPaymentRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<PurchaseDownPayment> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<PurchaseDownPayment> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<PurchaseDownPayment> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month && !x.IsDeleted).ToList();
        }

        public PurchaseDownPayment GetObjectById(int Id)
        {
            PurchaseDownPayment purchaseDownPayment = Find(x => x.Id == Id && !x.IsDeleted);
            if (purchaseDownPayment != null) { purchaseDownPayment.Errors = new Dictionary<string, string>(); }
            return purchaseDownPayment;
        }

        public IList<PurchaseDownPayment> GetObjectsByCashBankId(int cashBankId)
        {
            return FindAll(x => x.CashBankId == cashBankId && !x.IsDeleted).ToList();
        }

        public IList<PurchaseDownPayment> GetObjectsByContactId(int contactId)
        {
            return FindAll(x => x.ContactId == contactId && !x.IsDeleted).ToList();
        }

        public PurchaseDownPayment CreateObject(PurchaseDownPayment purchaseDownPayment)
        {
            purchaseDownPayment.Code = SetObjectCode();
            purchaseDownPayment.IsDeleted = false;
            purchaseDownPayment.IsConfirmed = false;
            purchaseDownPayment.IsReconciled = false;
            purchaseDownPayment.CreatedAt = DateTime.Now;
            return Create(purchaseDownPayment);
        }

        public PurchaseDownPayment UpdateObject(PurchaseDownPayment purchaseDownPayment)
        {
            purchaseDownPayment.UpdatedAt = DateTime.Now;
            Update(purchaseDownPayment);
            return purchaseDownPayment;
        }

        public PurchaseDownPayment SoftDeleteObject(PurchaseDownPayment purchaseDownPayment)
        {
            purchaseDownPayment.IsDeleted = true;
            purchaseDownPayment.DeletedAt = DateTime.Now;
            Update(purchaseDownPayment);
            return purchaseDownPayment;
        }

        public bool DeleteObject(int Id)
        {
            PurchaseDownPayment purchaseDownPayment = Find(x => x.Id == Id);
            return (Delete(purchaseDownPayment) == 1) ? true : false;
        }

        public PurchaseDownPayment ConfirmObject(PurchaseDownPayment purchaseDownPayment)
        {
            purchaseDownPayment.IsConfirmed = true;
            Update(purchaseDownPayment);
            return purchaseDownPayment;
        }

        public PurchaseDownPayment UnconfirmObject(PurchaseDownPayment purchaseDownPayment)
        {
            purchaseDownPayment.IsConfirmed = false;
            purchaseDownPayment.ConfirmationDate = null;
            UpdateObject(purchaseDownPayment);
            return purchaseDownPayment;
        }

        public PurchaseDownPayment ReconcileObject(PurchaseDownPayment purchaseDownPayment)
        {
            purchaseDownPayment.IsReconciled = true;
            Update(purchaseDownPayment);
            return purchaseDownPayment;
        }

        public PurchaseDownPayment UnreconcileObject(PurchaseDownPayment purchaseDownPayment)
        {
            purchaseDownPayment.IsReconciled = false;
            purchaseDownPayment.ReconciliationDate = null;
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