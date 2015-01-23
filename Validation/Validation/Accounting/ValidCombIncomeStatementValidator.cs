using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Validation;
using Core.DomainModel;
using Core.Interface.Service;
using Core.Constants;

namespace Validation.Validation
{
    public class ValidCombIncomeStatementValidator : IValidCombIncomeStatementValidator
    {

        public ValidCombIncomeStatement VHasAccount(ValidCombIncomeStatement validCombIncomeStatement, IAccountService _accountService)
        {
            Account account = _accountService.GetObjectById(validCombIncomeStatement.AccountId);
            if (account == null)
            {
                validCombIncomeStatement.Errors.Add("Account", "Tidak valid");
            }
            return validCombIncomeStatement;
        }

        public ValidCombIncomeStatement VHasClosing(ValidCombIncomeStatement validCombIncomeStatement, IClosingService _closingService)
        {
            Closing closing = _closingService.GetObjectById(validCombIncomeStatement.ClosingId);
            if (closing == null)
            {
                validCombIncomeStatement.Errors.Add("Closing", "Tidak valid");
            }
            return validCombIncomeStatement;
        }

        public ValidCombIncomeStatement VCreateObject(ValidCombIncomeStatement validCombIncomeStatement, IAccountService _accountService, IClosingService _closingService)
        {
            VHasAccount(validCombIncomeStatement, _accountService);
            if (!isValid(validCombIncomeStatement)) { return validCombIncomeStatement; }
            VHasClosing(validCombIncomeStatement, _closingService);
            return validCombIncomeStatement;
        }

        public ValidCombIncomeStatement VUpdateObject(ValidCombIncomeStatement validCombIncomeStatement, IAccountService _accountService, IClosingService _closingService)
        {
            VHasAccount(validCombIncomeStatement, _accountService);
            if (!isValid(validCombIncomeStatement)) { return validCombIncomeStatement; }
            VHasClosing(validCombIncomeStatement, _closingService);
            return validCombIncomeStatement;
        }

        public ValidCombIncomeStatement VDeleteObject(ValidCombIncomeStatement validCombIncomeStatement)
        {
            return validCombIncomeStatement;
        }

        public bool ValidCreateObject(ValidCombIncomeStatement validCombIncomeStatement, IAccountService _accountService, IClosingService _closingService)
        {
            VCreateObject(validCombIncomeStatement, _accountService, _closingService);
            return isValid(validCombIncomeStatement);
        }


        public bool ValidUpdateObject(ValidCombIncomeStatement validCombIncomeStatement, IAccountService _accountService, IClosingService _closingService)
        {
            validCombIncomeStatement.Errors.Clear();
            VUpdateObject(validCombIncomeStatement, _accountService, _closingService);
            return isValid(validCombIncomeStatement);
        }

        public bool ValidDeleteObject(ValidCombIncomeStatement validCombIncomeStatement)
        {
            validCombIncomeStatement.Errors.Clear();
            VDeleteObject(validCombIncomeStatement);
            return isValid(validCombIncomeStatement);
        }

        public bool isValid(ValidCombIncomeStatement obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(ValidCombIncomeStatement obj)
        {
            string erroroutput = "";
            KeyValuePair<string, string> first = obj.Errors.ElementAt(0);
            erroroutput += first.Key + "," + first.Value;
            foreach (KeyValuePair<string, string> pair in obj.Errors.Skip(1))
            {
                erroroutput += Environment.NewLine;
                erroroutput += pair.Key + "," + pair.Value;
            }
            return erroroutput;
        }

    }
}
