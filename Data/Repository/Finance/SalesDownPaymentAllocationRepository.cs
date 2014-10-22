using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repository
{
    public class SalesDownPaymentAllocationRepository : EfRepository<SalesDownPaymentAllocation>, ISalesDownPaymentAllocationRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public SalesDownPaymentAllocationRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<SalesDownPaymentAllocation> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<SalesDownPaymentAllocation> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<SalesDownPaymentAllocation> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month && !x.IsDeleted).ToList();
        }

        public SalesDownPaymentAllocation GetObjectById(int Id)
        {
            SalesDownPaymentAllocation salesDownPaymentAllocation = Find(x => x.Id == Id && !x.IsDeleted);
            if (salesDownPaymentAllocation != null) { salesDownPaymentAllocation.Errors = new Dictionary<string, string>(); }
            return salesDownPaymentAllocation;
        }

        public SalesDownPaymentAllocation GetObjectBySalesDownPaymentId(int salesDownPaymentId)
        {
            SalesDownPaymentAllocation salesDownPaymentAllocation = Find(x => x.SalesDownPaymentId == salesDownPaymentId && !x.IsDeleted);
            if (salesDownPaymentAllocation != null) { salesDownPaymentAllocation.Errors = new Dictionary<string, string>(); }
            return salesDownPaymentAllocation;
        }

        public IList<SalesDownPaymentAllocation> GetObjectsByContactId(int contactId)
        {
            return FindAll(x => x.ContactId == contactId && !x.IsDeleted).ToList();
        }

        public SalesDownPaymentAllocation CreateObject(SalesDownPaymentAllocation salesDownPaymentAllocation)
        {
            salesDownPaymentAllocation.Code = SetObjectCode();
            salesDownPaymentAllocation.IsDeleted = false;
            salesDownPaymentAllocation.IsConfirmed = false;
            salesDownPaymentAllocation.CreatedAt = DateTime.Now;
            return Create(salesDownPaymentAllocation);
        }

        public SalesDownPaymentAllocation UpdateObject(SalesDownPaymentAllocation salesDownPaymentAllocation)
        {
            salesDownPaymentAllocation.UpdatedAt = DateTime.Now;
            Update(salesDownPaymentAllocation);
            return salesDownPaymentAllocation;
        }

        public SalesDownPaymentAllocation SoftDeleteObject(SalesDownPaymentAllocation salesDownPaymentAllocation)
        {
            salesDownPaymentAllocation.IsDeleted = true;
            salesDownPaymentAllocation.DeletedAt = DateTime.Now;
            Update(salesDownPaymentAllocation);
            return salesDownPaymentAllocation;
        }

        public bool DeleteObject(int Id)
        {
            SalesDownPaymentAllocation salesDownPaymentAllocation = Find(x => x.Id == Id);
            return (Delete(salesDownPaymentAllocation) == 1) ? true : false;
        }

        public SalesDownPaymentAllocation ConfirmObject(SalesDownPaymentAllocation salesDownPaymentAllocation)
        {
            salesDownPaymentAllocation.IsConfirmed = true;
            Update(salesDownPaymentAllocation);
            return salesDownPaymentAllocation;
        }

        public SalesDownPaymentAllocation UnconfirmObject(SalesDownPaymentAllocation salesDownPaymentAllocation)
        {
            salesDownPaymentAllocation.IsConfirmed = false;
            salesDownPaymentAllocation.ConfirmationDate = null;
            UpdateObject(salesDownPaymentAllocation);
            return salesDownPaymentAllocation;
        }

        public string SetObjectCode()
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = DateTime.Today.Year.ToString() + "." + DateTime.Today.Month.ToString() + "." + totalnumberinthemonth;
            return Code;
        }
    }
}