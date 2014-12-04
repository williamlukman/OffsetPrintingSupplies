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
    public class VCNonBaseCurrencyValidator : IVCNonBaseCurrencyValidator
    {

        public VCNonBaseCurrency VCreateObject(VCNonBaseCurrency validComb, IAccountService _accountService, IClosingService _closingService)
        {
            return validComb;
        }

        public VCNonBaseCurrency VUpdateObject(VCNonBaseCurrency validComb, IAccountService _accountService, IClosingService _closingService)
        {
            return validComb;
        }

        public VCNonBaseCurrency VDeleteObject(VCNonBaseCurrency validComb)
        {
            return validComb;
        }

        public bool ValidCreateObject(VCNonBaseCurrency validComb, IAccountService _accountService, IClosingService _closingService)
        {
            VCreateObject(validComb, _accountService, _closingService);
            return isValid(validComb);
        }


        public bool ValidUpdateObject(VCNonBaseCurrency validComb, IAccountService _accountService, IClosingService _closingService)
        {
            validComb.Errors.Clear();
            VUpdateObject(validComb, _accountService, _closingService);
            return isValid(validComb);
        }

        public bool ValidDeleteObject(VCNonBaseCurrency validComb)
        {
            validComb.Errors.Clear();
            VDeleteObject(validComb);
            return isValid(validComb);
        }

        public bool isValid(VCNonBaseCurrency obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(VCNonBaseCurrency obj)
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
