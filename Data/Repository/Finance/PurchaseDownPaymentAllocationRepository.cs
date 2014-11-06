using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repository
{
    public class PurchaseDownPaymentAllocationRepository : EfRepository<PurchaseDownPaymentAllocation>, IPurchaseDownPaymentAllocationRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public PurchaseDownPaymentAllocationRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<PurchaseDownPaymentAllocation> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<PurchaseDownPaymentAllocation> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<PurchaseDownPaymentAllocation> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month && !x.IsDeleted).ToList();
        }

        public PurchaseDownPaymentAllocation GetObjectById(int Id)
        {
            PurchaseDownPaymentAllocation purchaseDownPaymentAllocation = Find(x => x.Id == Id && !x.IsDeleted);
            if (purchaseDownPaymentAllocation != null) { purchaseDownPaymentAllocation.Errors = new Dictionary<string, string>(); }
            return purchaseDownPaymentAllocation;
        }

        public PurchaseDownPaymentAllocation GetObjectByReceivableId(int ReceivableId)
        {
            PurchaseDownPaymentAllocation purchaseDownPaymentAllocation = Find(x => x.ReceivableId == ReceivableId && !x.IsDeleted);
            if (purchaseDownPaymentAllocation != null) { purchaseDownPaymentAllocation.Errors = new Dictionary<string, string>(); }
            return purchaseDownPaymentAllocation;
        }

        public IList<PurchaseDownPaymentAllocation> GetObjectsByContactId(int contactId)
        {
            return FindAll(x => x.ContactId == contactId && !x.IsDeleted).ToList();
        }

        public PurchaseDownPaymentAllocation CreateObject(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation)
        {
            purchaseDownPaymentAllocation.Code = SetObjectCode();
            purchaseDownPaymentAllocation.IsDeleted = false;
            purchaseDownPaymentAllocation.IsConfirmed = false;
            purchaseDownPaymentAllocation.CreatedAt = DateTime.Now;
            return Create(purchaseDownPaymentAllocation);
        }

        public PurchaseDownPaymentAllocation UpdateObject(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation)
        {
            purchaseDownPaymentAllocation.UpdatedAt = DateTime.Now;
            Update(purchaseDownPaymentAllocation);
            return purchaseDownPaymentAllocation;
        }

        public PurchaseDownPaymentAllocation SoftDeleteObject(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation)
        {
            purchaseDownPaymentAllocation.IsDeleted = true;
            purchaseDownPaymentAllocation.DeletedAt = DateTime.Now;
            Update(purchaseDownPaymentAllocation);
            return purchaseDownPaymentAllocation;
        }

        public bool DeleteObject(int Id)
        {
            PurchaseDownPaymentAllocation purchaseDownPaymentAllocation = Find(x => x.Id == Id);
            return (Delete(purchaseDownPaymentAllocation) == 1) ? true : false;
        }

        public PurchaseDownPaymentAllocation ConfirmObject(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation)
        {
            purchaseDownPaymentAllocation.IsConfirmed = true;
            Update(purchaseDownPaymentAllocation);
            return purchaseDownPaymentAllocation;
        }

        public PurchaseDownPaymentAllocation UnconfirmObject(PurchaseDownPaymentAllocation purchaseDownPaymentAllocation)
        {
            purchaseDownPaymentAllocation.IsConfirmed = false;
            purchaseDownPaymentAllocation.ConfirmationDate = null;
            UpdateObject(purchaseDownPaymentAllocation);
            return purchaseDownPaymentAllocation;
        }

        public string SetObjectCode()
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = DateTime.Today.Year.ToString() + "." + DateTime.Today.Month.ToString() + "." + totalnumberinthemonth;
            return Code;
        }
    }
}