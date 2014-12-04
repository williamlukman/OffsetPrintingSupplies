using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IGLNonBaseCurrencyService
    {
        IQueryable<GLNonBaseCurrency> GetQueryable();
        IGLNonBaseCurrencyValidator GetValidator();
        IList<GLNonBaseCurrency> GetAll();
        GLNonBaseCurrency GetObjectById(int Id);
        GLNonBaseCurrency CreateObject(GLNonBaseCurrency generalLedgerJournal, IAccountService _accountService);
        GLNonBaseCurrency SoftDeleteObject(GLNonBaseCurrency generalLedgerJournal);
        bool DeleteObject(int Id);
    }
}