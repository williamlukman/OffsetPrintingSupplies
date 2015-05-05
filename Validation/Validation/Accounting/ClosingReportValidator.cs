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
    public class ClosingReportValidator : IClosingReportValidator
    {

        public ClosingReport VCreateObject(ClosingReport closingReport, IAccountService _accountService)
        {
            return closingReport;
        }

        public ClosingReport VDeleteObject(ClosingReport closingReport)
        {
            return closingReport;
        }

        public bool ValidCreateObject(ClosingReport closingReport, IAccountService _accountService)
        {
            VCreateObject(closingReport, _accountService);
            return isValid(closingReport);
        }

        public bool ValidDeleteObject(ClosingReport closingReport)
        {
            closingReport.Errors.Clear();
            VDeleteObject(closingReport);
            return isValid(closingReport);
        }

        public bool isValid(ClosingReport obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(ClosingReport obj)
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
