using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IInterestAdjustmentRepository : IRepository<InterestAdjustment>
    {
        IQueryable<InterestAdjustment> GetQueryable();
        IList<InterestAdjustment> GetAll();
        IList<InterestAdjustment> GetAllByMonthCreated();
        IList<InterestAdjustment> GetObjectsByCashBankId(int cashBankId);
        InterestAdjustment GetObjectById(int Id);
        InterestAdjustment CreateObject(InterestAdjustment interestAdjustment);
        InterestAdjustment UpdateObject(InterestAdjustment interestAdjustment);
        InterestAdjustment SoftDeleteObject(InterestAdjustment interestAdjustment);
        bool DeleteObject(int Id);
        InterestAdjustment ConfirmObject(InterestAdjustment interestAdjustment);
        InterestAdjustment UnconfirmObject(InterestAdjustment interestAdjustment);
        string SetObjectCode();
    }
}