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
    public class GLNonBaseCurrencyValidator : IGLNonBaseCurrencyValidator
    {

        public GLNonBaseCurrency VCreateObject(GLNonBaseCurrency generalLedgerJournal, IAccountService _accountService)
        {
            return generalLedgerJournal;
        }

        public GLNonBaseCurrency VDeleteObject(GLNonBaseCurrency generalLedgerJournal)
        {
            return generalLedgerJournal;
        }

        public bool ValidCreateObject(GLNonBaseCurrency generalLedgerJournal, IAccountService _accountService)
        {
            VCreateObject(generalLedgerJournal, _accountService);
            return isValid(generalLedgerJournal);
        }

        public bool ValidDeleteObject(GLNonBaseCurrency generalLedgerJournal)
        {
            generalLedgerJournal.Errors.Clear();
            VDeleteObject(generalLedgerJournal);
            return isValid(generalLedgerJournal);
        }

        public bool isValid(GLNonBaseCurrency obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(GLNonBaseCurrency obj)
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
