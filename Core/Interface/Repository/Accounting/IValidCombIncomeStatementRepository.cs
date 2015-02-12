using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IValidCombIncomeStatementRepository : IRepository<ValidCombIncomeStatement>
    {
        IQueryable<ValidCombIncomeStatement> GetQueryable();
        IList<ValidCombIncomeStatement> GetAll();
        ValidCombIncomeStatement GetObjectById(int Id);
        ValidCombIncomeStatement FindOrCreateObjectByAccountAndClosing(int accountId, int closingId);
        ValidCombIncomeStatement CreateObject(ValidCombIncomeStatement validCombIncomeStatement);
        ValidCombIncomeStatement UpdateObject(ValidCombIncomeStatement validCombIncomeStatement);
        //ValidCombIncomeStatement SoftDeleteObject(ValidCombIncomeStatement validCombIncomeStatement);
        bool DeleteObject(int Id);
    }
}