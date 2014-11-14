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
        ExchangeRate GetObjectByName(string Name);
        ExchangeRate CreateObject(ExchangeRate exchangeRate);
        ExchangeRate UpdateObject(ExchangeRate exchangeRate);
        ExchangeRate SoftDeleteObject(ExchangeRate exchangeRate);
        bool DeleteObject(int Id);
        bool IsNameDuplicated(ExchangeRate exchangeRate);
        decimal GetTotalExchangeRate();
    }
}