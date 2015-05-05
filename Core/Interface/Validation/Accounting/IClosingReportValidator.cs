using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IClosingReportValidator
    {
        ClosingReport VCreateObject(ClosingReport closingReport, IAccountService _accountService);
        ClosingReport VDeleteObject(ClosingReport closingReport);
        bool ValidCreateObject(ClosingReport closingReport, IAccountService _accountService);
        bool ValidDeleteObject(ClosingReport closingReport);
        bool isValid(ClosingReport closingReport);
        string PrintError(ClosingReport closingReport);
    }
}
