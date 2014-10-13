using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repository
{
    public class SalesAllowanceRepository : EfRepository<SalesAllowance>, ISalesAllowanceRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public SalesAllowanceRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<SalesAllowance> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<SalesAllowance> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<SalesAllowance> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month && !x.IsDeleted).ToList();
        }

        public SalesAllowance GetObjectById(int Id)
        {
            SalesAllowance salesAllowance = Find(x => x.Id == Id && !x.IsDeleted);
            if (salesAllowance != null) { salesAllowance.Errors = new Dictionary<string, string>(); }
            return salesAllowance;
        }

        public IList<SalesAllowance> GetObjectsByCashBankId(int cashBankId)
        {
            return FindAll(x => x.CashBankId == cashBankId && !x.IsDeleted).ToList();
        }

        public IList<SalesAllowance> GetObjectsByContactId(int contactId)
        {
            return FindAll(x => x.ContactId == contactId && !x.IsDeleted).ToList();
        }

        public SalesAllowance CreateObject(SalesAllowance salesAllowance)
        {
            salesAllowance.Code = SetObjectCode();
            salesAllowance.IsDeleted = false;
            salesAllowance.IsConfirmed = false;
            salesAllowance.IsReconciled = false;
            salesAllowance.CreatedAt = DateTime.Now;
            return Create(salesAllowance);
        }

        public SalesAllowance UpdateObject(SalesAllowance salesAllowance)
        {
            salesAllowance.UpdatedAt = DateTime.Now;
            Update(salesAllowance);
            return salesAllowance;
        }

        public SalesAllowance SoftDeleteObject(SalesAllowance salesAllowance)
        {
            salesAllowance.IsDeleted = true;
            salesAllowance.DeletedAt = DateTime.Now;
            Update(salesAllowance);
            return salesAllowance;
        }

        public bool DeleteObject(int Id)
        {
            SalesAllowance salesAllowance = Find(x => x.Id == Id);
            return (Delete(salesAllowance) == 1) ? true : false;
        }

        public SalesAllowance ConfirmObject(SalesAllowance salesAllowance)
        {
            salesAllowance.IsConfirmed = true;
            Update(salesAllowance);
            return salesAllowance;
        }

        public SalesAllowance UnconfirmObject(SalesAllowance salesAllowance)
        {
            salesAllowance.IsConfirmed = false;
            salesAllowance.ConfirmationDate = null;
            UpdateObject(salesAllowance);
            return salesAllowance;
        }

        public SalesAllowance ReconcileObject(SalesAllowance salesAllowance)
        {
            salesAllowance.IsReconciled = true;
            Update(salesAllowance);
            return salesAllowance;
        }

        public SalesAllowance UnreconcileObject(SalesAllowance salesAllowance)
        {
            salesAllowance.IsReconciled = false;
            salesAllowance.ReconciliationDate = null;
            UpdateObject(salesAllowance);
            return salesAllowance;
        }

        public string SetObjectCode()
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = DateTime.Today.Year.ToString() + "." + DateTime.Today.Month.ToString() + "." + totalnumberinthemonth;
            return Code;
        }
    }
}