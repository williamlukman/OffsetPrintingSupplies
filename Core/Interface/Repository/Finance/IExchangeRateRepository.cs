using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IExchangeRateRepository : IRepository<ExchangeRate>
    {
        IQueryable<ExchangeRate> GetQueryable();
        IList<ExchangeRate> GetAll();
        ExchangeRate GetObjectById(int Id);
        ExchangeRate CreateObject(ExchangeRate exchangeRate);
        ExchangeRate UpdateObject(ExchangeRate exchangeRate);
        ExchangeRate SoftDeleteObject(ExchangeRate exchangeRate);
        bool DeleteObject(int Id);
    }
}