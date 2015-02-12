using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IValidCombIncomeStatementService
    {
        IQueryable<ValidCombIncomeStatement> GetQueryable();
        IValidCombIncomeStatementValidator GetValidator();
        IList<ValidCombIncomeStatement> GetAll();
        ValidCombIncomeStatement GetObjectById(int Id);
        ValidCombIncomeStatement FindOrCreateObjectByAccountAndClosing(int AccountId, int ClosingId);
        ValidCombIncomeStatement CreateObject(ValidCombIncomeStatement validCombIncomeStatement, IAccountService _accountService, IClosingService _closingService);
        ValidCombIncomeStatement UpdateObject(ValidCombIncomeStatement validCombIncomeStatement, IAccountService _accountService, IClosingService _closingService);
        bool DeleteObject(int Id);
        ValidCombIncomeStatement CalculateTotalAmount(ValidCombIncomeStatement validCombIncomeStatement, IAccountService _accountService, IClosingService _closingService);
    }
}