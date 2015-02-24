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
    public class InterestIncomeRepository : EfRepository<InterestIncome>, IInterestIncomeRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public InterestIncomeRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<InterestIncome> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<InterestIncome> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<InterestIncome> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month && !x.IsDeleted).ToList();
        }

        public IList<InterestIncome> GetObjectsByCashBankId(int cashBankId)
        {
            return FindAll(x => x.CashBankId == cashBankId && !x.IsDeleted).ToList();
        }

        public InterestIncome GetObjectById(int Id)
        {
            InterestIncome interestIncome = Find(x => x.Id == Id && !x.IsDeleted);
            if (interestIncome != null) { interestIncome.Errors = new Dictionary<string, string>(); }
            return interestIncome;
        }

        public InterestIncome CreateObject(InterestIncome interestIncome)
        {
            interestIncome.Code = SetObjectCode();
            interestIncome.IsDeleted = false;
            interestIncome.IsConfirmed = false;
            interestIncome.CreatedAt = DateTime.Now;
            return Create(interestIncome);
        }

        public InterestIncome UpdateObject(InterestIncome interestIncome)
        {
            interestIncome.UpdatedAt = DateTime.Now;
            Update(interestIncome);
            return interestIncome;
        }

        public InterestIncome SoftDeleteObject(InterestIncome interestIncome)
        {
            interestIncome.IsDeleted = true;
            interestIncome.DeletedAt = DateTime.Now;
            Update(interestIncome);
            return interestIncome;
        }

        public InterestIncome ConfirmObject(InterestIncome interestIncome)
        {
            interestIncome.IsConfirmed = true;
            Update(interestIncome);
            return interestIncome;
        }

        public InterestIncome UnconfirmObject(InterestIncome interestIncome)
        {
            interestIncome.IsConfirmed = false;
            interestIncome.ConfirmationDate = null;
            interestIncome.UpdatedAt = DateTime.Now;
            Update(interestIncome);
            return interestIncome;
        }

        public bool DeleteObject(int Id)
        {
            InterestIncome interestIncome = Find(x => x.Id == Id);
            return (Delete(interestIncome) == 1) ? true : false;
        }

        public string SetObjectCode()
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = DateTime.Today.Year.ToString() + "." + DateTime.Today.Month.ToString() + "." + totalnumberinthemonth;
            return Code;
        }
    }
}