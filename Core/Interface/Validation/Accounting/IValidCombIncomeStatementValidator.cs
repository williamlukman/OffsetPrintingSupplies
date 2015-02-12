using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IValidCombIncomeStatementValidator
    {
        ValidCombIncomeStatement VHasAccount(ValidCombIncomeStatement validCombIncomeStatement, IAccountService _accountService);
        ValidCombIncomeStatement VHasClosing(ValidCombIncomeStatement validCombIncomeStatement, IClosingService _closingService);

        ValidCombIncomeStatement VCreateObject(ValidCombIncomeStatement validCombIncomeStatement, IAccountService _accountService, IClosingService _closingService);
        ValidCombIncomeStatement VUpdateObject(ValidCombIncomeStatement validCombIncomeStatement, IAccountService _accountService, IClosingService _closingService);
        ValidCombIncomeStatement VDeleteObject(ValidCombIncomeStatement validCombIncomeStatement);
        bool ValidCreateObject(ValidCombIncomeStatement validCombIncomeStatement, IAccountService _accountService, IClosingService _closingService);
        bool ValidUpdateObject(ValidCombIncomeStatement validCombIncomeStatement, IAccountService _accountService, IClosingService _closingService);
        bool ValidDeleteObject(ValidCombIncomeStatement validCombIncomeStatement);
        bool isValid(ValidCombIncomeStatement validCombIncomeStatement);
        string PrintError(ValidCombIncomeStatement validCombIncomeStatement);
    }
}
