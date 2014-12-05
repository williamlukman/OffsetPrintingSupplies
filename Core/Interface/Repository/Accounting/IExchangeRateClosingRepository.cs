
using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IExchangeRateClosingRepository : IRepository<ExchangeRateClosing>
    {
        IQueryable<ExchangeRateClosing> GetQueryable();
        IList<ExchangeRateClosing> GetAll();
        ExchangeRateClosing GetObjectById(int Id);
        ExchangeRateClosing CreateObject(ExchangeRateClosing exchangeRateClosing);
        bool DeleteObject(int Id);
    }
}