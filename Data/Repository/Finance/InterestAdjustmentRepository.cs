using Core.DomainModel;
using Core.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Context;
using System.Data;

namespace Data.Repository
{
    public class InterestAdjustmentRepository : EfRepository<InterestAdjustment>, IInterestAdjustmentRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public InterestAdjustmentRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<InterestAdjustment> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<InterestAdjustment> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<InterestAdjustment> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month && !x.IsDeleted).ToList();
        }

        public IList<InterestAdjustment> GetObjectsByCashBankId(int cashBankId)
        {
            return FindAll(x => x.CashBankId == cashBankId && !x.IsDeleted).ToList();
        }

        public InterestAdjustment GetObjectById(int Id)
        {
            InterestAdjustment interestAdjustment = Find(x => x.Id == Id && !x.IsDeleted);
            if (interestAdjustment != null) { interestAdjustment.Errors = new Dictionary<string, string>(); }
            return interestAdjustment;
        }

        public InterestAdjustment CreateObject(InterestAdjustment interestAdjustment)
        {
            interestAdjustment.Code = SetObjectCode();
            interestAdjustment.IsDeleted = false;
            interestAdjustment.IsConfirmed = false;
            interestAdjustment.CreatedAt = DateTime.Now;
            return Create(interestAdjustment);
        }

        public InterestAdjustment UpdateObject(InterestAdjustment interestAdjustment)
        {
            interestAdjustment.UpdatedAt = DateTime.Now;
            Update(interestAdjustment);
            return interestAdjustment;
        }

        public InterestAdjustment SoftDeleteObject(InterestAdjustment interestAdjustment)
        {
            interestAdjustment.IsDeleted = true;
            interestAdjustment.DeletedAt = DateTime.Now;
            Update(interestAdjustment);
            return interestAdjustment;
        }

        public InterestAdjustment ConfirmObject(InterestAdjustment interestAdjustment)
        {
            interestAdjustment.IsConfirmed = true;
            Update(interestAdjustment);
            return interestAdjustment;
        }

        public InterestAdjustment UnconfirmObject(InterestAdjustment interestAdjustment)
        {
            interestAdjustment.IsConfirmed = false;
            interestAdjustment.ConfirmationDate = null;
            interestAdjustment.UpdatedAt = DateTime.Now;
            Update(interestAdjustment);
            return interestAdjustment;
        }

        public bool DeleteObject(int Id)
        {
            InterestAdjustment interestAdjustment = Find(x => x.Id == Id);
            return (Delete(interestAdjustment) == 1) ? true : false;
        }

        public string SetObjectCode()
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = DateTime.Today.Year.ToString() + "." + DateTime.Today.Month.ToString() + "." + totalnumberinthemonth;
            return Code;
        }
    }
}