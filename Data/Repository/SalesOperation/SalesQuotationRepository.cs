using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repository
{
    public class SalesQuotationRepository : EfRepository<SalesQuotation>, ISalesQuotationRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public SalesQuotationRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<SalesQuotation> GetQueryable()
        {
            return FindAll();
        }

        public IList<SalesQuotation> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<SalesQuotation> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month && !x.IsDeleted).ToList();
        }

        public IList<SalesQuotation> GetApprovedObjects()
        {
            return FindAll(x => x.IsApproved && !x.IsDeleted).ToList();
        }

        public IList<SalesQuotation> GetObjectsByContactId(int contactId)
        {
            return FindAll(x => x.ContactId == contactId && !x.IsDeleted).ToList();
        }

        public SalesQuotation GetObjectById(int Id)
        {
            SalesQuotation salesQuotation = Find(x => x.Id == Id && !x.IsDeleted);
            if (salesQuotation != null) { salesQuotation.Errors = new Dictionary<string, string>(); }
            return salesQuotation;
        }

        public SalesQuotation CreateObject(SalesQuotation salesQuotation)
        {
            salesQuotation.Code = SetObjectCode();
            salesQuotation.IsDeleted = false;
            salesQuotation.IsConfirmed = false;
            salesQuotation.CreatedAt = DateTime.Now;
            return Create(salesQuotation);
        }

        public SalesQuotation UpdateObject(SalesQuotation salesQuotation)
        {
            salesQuotation.UpdatedAt = DateTime.Now;
            Update(salesQuotation);
            return salesQuotation;
        }

        public SalesQuotation SoftDeleteObject(SalesQuotation salesQuotation)
        {
            salesQuotation.IsDeleted = true;
            salesQuotation.DeletedAt = DateTime.Now;
            Update(salesQuotation);
            return salesQuotation;
        }

        public bool DeleteObject(int Id)
        {
            SalesQuotation salesQuotation = Find(x => x.Id == Id);
            return (Delete(salesQuotation) == 1) ? true : false;
        }

        public SalesQuotation ConfirmObject(SalesQuotation salesQuotation)
        {
            salesQuotation.IsConfirmed = true;
            Update(salesQuotation);
            return salesQuotation;
        }

        public SalesQuotation UnconfirmObject(SalesQuotation salesQuotation)
        {
            salesQuotation.IsConfirmed = false;
            salesQuotation.ConfirmationDate = null;
            UpdateObject(salesQuotation);
            return salesQuotation;
        }

        public SalesQuotation ApproveObject(SalesQuotation salesQuotation)
        {
            salesQuotation.IsApproved = true;
            UpdateObject(salesQuotation);
            return salesQuotation;
        }

        public SalesQuotation RejectObject(SalesQuotation salesQuotation)
        {
            salesQuotation.IsRejected = true;
            UpdateObject(salesQuotation);
            return salesQuotation;
        }

        public string SetObjectCode()
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = DateTime.Today.Year.ToString() + "." + DateTime.Today.Month.ToString() + "." + totalnumberinthemonth;
            return Code;
        }
    }
}