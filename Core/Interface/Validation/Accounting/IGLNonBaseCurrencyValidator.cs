using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IGLNonBaseCurrencyValidator
    {
        GLNonBaseCurrency VCreateObject(GLNonBaseCurrency generalLedgerJournal, IAccountService _accountService);
        GLNonBaseCurrency VDeleteObject(GLNonBaseCurrency generalLedgerJournal);
        bool ValidCreateObject(GLNonBaseCurrency generalLedgerJournal, IAccountService _accountService);
        bool ValidDeleteObject(GLNonBaseCurrency generalLedgerJournal);
        bool isValid(GLNonBaseCurrency generalLedgerJournal);
        string PrintError(GLNonBaseCurrency generalLedgerJournal);
    }
}
