using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IInterestIncomeRepository : IRepository<InterestIncome>
    {
        IQueryable<InterestIncome> GetQueryable();
        IList<InterestIncome> GetAll();
        IList<InterestIncome> GetAllByMonthCreated();
        IList<InterestIncome> GetObjectsByCashBankId(int cashBankId);
        InterestIncome GetObjectById(int Id);
        InterestIncome CreateObject(InterestIncome interestIncome);
        InterestIncome UpdateObject(InterestIncome interestIncome);
        InterestIncome SoftDeleteObject(InterestIncome interestIncome);
        bool DeleteObject(int Id);
        InterestIncome ConfirmObject(InterestIncome interestIncome);
        InterestIncome UnconfirmObject(InterestIncome interestIncome);
        string SetObjectCode();
    }
}