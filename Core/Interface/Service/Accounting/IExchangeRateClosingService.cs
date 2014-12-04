using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IExchangeRateClosingService
    {
        IQueryable<ExchangeRateClosing> GetQueryable();
        IExchangeRateClosingValidator GetValidator();
        IList<ExchangeRateClosing> GetAll();
        ExchangeRateClosing GetObjectById(int Id); 
        ExchangeRateClosing CreateObject(ExchangeRateClosing exchangeRateExchangeRateClosing);
        bool DeleteObject(int Id, IAccountService _accountService, IValidCombService _validCombService);
    }
}