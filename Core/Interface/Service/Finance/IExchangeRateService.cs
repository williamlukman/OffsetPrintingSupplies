using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IExchangeRateService
    {
        IQueryable<ExchangeRate> GetQueryable();
        IExchangeRateValidator GetValidator();
        IList<ExchangeRate> GetAll();
        ExchangeRate GetObjectById(int Id);
        ExchangeRate CreateObject(ExchangeRate exchangeRate);
        ExchangeRate UpdateObject(ExchangeRate exchangeRate);
        ExchangeRate SoftDeleteObject(ExchangeRate exchangeRate);
        ExchangeRate GetLatestRate(DateTime date, Currency currency);
        bool IsExchangeRateDateDuplicated(ExchangeRate exchangeRate);
        bool DeleteObject(int Id);
    }
}