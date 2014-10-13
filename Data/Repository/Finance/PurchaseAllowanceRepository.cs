using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repository
{
    public class PurchaseAllowanceRepository : EfRepository<PurchaseAllowance>, IPurchaseAllowanceRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public PurchaseAllowanceRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<PurchaseAllowance> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<PurchaseAllowance> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<PurchaseAllowance> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month && !x.IsDeleted).ToList();
        }

        public PurchaseAllowance GetObjectById(int Id)
        {
            PurchaseAllowance purchaseAllowance = Find(x => x.Id == Id && !x.IsDeleted);
            if (purchaseAllowance != null) { purchaseAllowance.Errors = new Dictionary<string, string>(); }
            return purchaseAllowance;
        }

        public IList<PurchaseAllowance> GetObjectsByCashBankId(int cashBankId)
        {
            return FindAll(x => x.CashBankId == cashBankId && !x.IsDeleted).ToList();
        }

        public IList<PurchaseAllowance> GetObjectsByContactId(int contactId)
        {
            return FindAll(x => x.ContactId == contactId && !x.IsDeleted).ToList();
        }

        public PurchaseAllowance CreateObject(PurchaseAllowance purchaseAllowance)
        {
            purchaseAllowance.Code = SetObjectCode();
            purchaseAllowance.IsDeleted = false;
            purchaseAllowance.IsConfirmed = false;
            purchaseAllowance.IsReconciled = false;
            purchaseAllowance.CreatedAt = DateTime.Now;
            return Create(purchaseAllowance);
        }

        public PurchaseAllowance UpdateObject(PurchaseAllowance purchaseAllowance)
        {
            purchaseAllowance.UpdatedAt = DateTime.Now;
            Update(purchaseAllowance);
            return purchaseAllowance;
        }

        public PurchaseAllowance SoftDeleteObject(PurchaseAllowance purchaseAllowance)
        {
            purchaseAllowance.IsDeleted = true;
            purchaseAllowance.DeletedAt = DateTime.Now;
            Update(purchaseAllowance);
            return purchaseAllowance;
        }

        public bool DeleteObject(int Id)
        {
            PurchaseAllowance purchaseAllowance = Find(x => x.Id == Id);
            return (Delete(purchaseAllowance) == 1) ? true : false;
        }

        public PurchaseAllowance ConfirmObject(PurchaseAllowance purchaseAllowance)
        {
            purchaseAllowance.IsConfirmed = true;
            Update(purchaseAllowance);
            return purchaseAllowance;
        }

        public PurchaseAllowance UnconfirmObject(PurchaseAllowance purchaseAllowance)
        {
            purchaseAllowance.IsConfirmed = false;
            purchaseAllowance.ConfirmationDate = null;
            UpdateObject(purchaseAllowance);
            return purchaseAllowance;
        }

        public PurchaseAllowance ReconcileObject(PurchaseAllowance purchaseAllowance)
        {
            purchaseAllowance.IsReconciled = true;
            Update(purchaseAllowance);
            return purchaseAllowance;
        }

        public PurchaseAllowance UnreconcileObject(PurchaseAllowance purchaseAllowance)
        {
            purchaseAllowance.IsReconciled = false;
            purchaseAllowance.ReconciliationDate = null;
            UpdateObject(purchaseAllowance);
            return purchaseAllowance;
        }

        public string SetObjectCode()
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = DateTime.Today.Year.ToString() + "." + DateTime.Today.Month.ToString() + "." + totalnumberinthemonth;
            return Code;
        }
    }
}