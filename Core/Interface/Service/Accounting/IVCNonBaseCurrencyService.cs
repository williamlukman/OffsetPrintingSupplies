using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IVCNonBaseCurrencyService
    {
        IQueryable<VCNonBaseCurrency> GetQueryable();
        IVCNonBaseCurrencyValidator GetValidator();
        IList<VCNonBaseCurrency> GetAll();
        VCNonBaseCurrency GetObjectById(int Id);
        VCNonBaseCurrency CreateObject(VCNonBaseCurrency validComb, IAccountService _accountService, IClosingService _closingService);
        VCNonBaseCurrency UpdateObject(VCNonBaseCurrency validComb, IAccountService _accountService, IClosingService _closingService);
        bool DeleteObject(int Id);
    }
}