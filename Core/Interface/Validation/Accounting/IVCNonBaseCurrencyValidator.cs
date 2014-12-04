using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IVCNonBaseCurrencyValidator
    {

        VCNonBaseCurrency VCreateObject(VCNonBaseCurrency validComb, IAccountService _accountService, IClosingService _closingService);
        VCNonBaseCurrency VUpdateObject(VCNonBaseCurrency validComb, IAccountService _accountService, IClosingService _closingService);
        VCNonBaseCurrency VDeleteObject(VCNonBaseCurrency validComb);
        bool ValidCreateObject(VCNonBaseCurrency validComb, IAccountService _accountService, IClosingService _closingService);
        bool ValidUpdateObject(VCNonBaseCurrency validComb, IAccountService _accountService, IClosingService _closingService);
        bool ValidDeleteObject(VCNonBaseCurrency validComb);
        bool isValid(VCNonBaseCurrency validComb);
        string PrintError(VCNonBaseCurrency validComb);
    }
}
