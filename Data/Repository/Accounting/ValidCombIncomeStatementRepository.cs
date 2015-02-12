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
    public class ValidCombIncomeStatementRepository : EfRepository<ValidCombIncomeStatement>, IValidCombIncomeStatementRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public ValidCombIncomeStatementRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<ValidCombIncomeStatement> GetQueryable()
        {
            return FindAll();
        }

        public IList<ValidCombIncomeStatement> GetAll()
        {
            return FindAll().ToList();
        }

        public ValidCombIncomeStatement GetObjectById(int Id)
        {
            ValidCombIncomeStatement validCombIncomeStatement = Find(x => x.Id == Id /*&& !x.IsDeleted*/);
            if (validCombIncomeStatement != null) { validCombIncomeStatement.Errors = new Dictionary<string, string>(); }
            return validCombIncomeStatement;
        }

        public ValidCombIncomeStatement FindOrCreateObjectByAccountAndClosing(int accountId, int closingId)
        {
            ValidCombIncomeStatement validCombIncomeStatement = Find(x => x.AccountId == accountId && x.ClosingId == closingId);
            if (validCombIncomeStatement == null)
            {
                validCombIncomeStatement = new ValidCombIncomeStatement()
                {
                    AccountId = accountId,
                    ClosingId = closingId,
                    Amount = 0
                };
                validCombIncomeStatement = CreateObject(validCombIncomeStatement);
            }
            validCombIncomeStatement.Errors = new Dictionary<string, string>();
            return validCombIncomeStatement;
        }
     
        public ValidCombIncomeStatement CreateObject(ValidCombIncomeStatement validCombIncomeStatement)
        {
            validCombIncomeStatement.CreatedAt = DateTime.Now;
            return Create(validCombIncomeStatement);
        }

        public ValidCombIncomeStatement UpdateObject(ValidCombIncomeStatement validCombIncomeStatement)
        {
            Update(validCombIncomeStatement);
            return validCombIncomeStatement;
        }

        /*public ValidCombIncomeStatement SoftDeleteObject(ValidCombIncomeStatement validCombIncomeStatement)
        {
            validCombIncomeStatement.IsDeleted = true;
            validCombIncomeStatement.DeletedAt = DateTime.Now;
            Update(validCombIncomeStatement);
            return validCombIncomeStatement;
        }*/

        public bool DeleteObject(int Id)
        {
            ValidCombIncomeStatement validCombIncomeStatement = Find(x => x.Id == Id);
            return (Delete(validCombIncomeStatement) == 1) ? true : false;
        }

    }
}