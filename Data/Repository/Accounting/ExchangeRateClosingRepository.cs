using Core.DomainModel;
using Core.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Context;
using System.Data;

namespace Data.Repository
{
    public class ExchangeRateClosingRepository : EfRepository<ExchangeRateClosing>, IExchangeRateClosingRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public ExchangeRateClosingRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<ExchangeRateClosing> GetQueryable()
        {
            return FindAll();
        }

        public IList<ExchangeRateClosing> GetAll()
        {
            return FindAll().ToList();
        }

        public ExchangeRateClosing GetObjectById(int Id)
        {
            ExchangeRateClosing exchangeRateClosing = Find(x => x.Id == Id /*&& !x.IsDeleted*/);
            if (exchangeRateClosing != null) { exchangeRateClosing.Errors = new Dictionary<string, string>(); }
            return exchangeRateClosing;
        }


        public ExchangeRateClosing CreateObject(ExchangeRateClosing exchangeRateClosing)
        {
            //exchangeRateClosing.IsDeleted = false;
            return Create(exchangeRateClosing);
        }

        public bool DeleteObject(int Id)
        {
            ExchangeRateClosing exchangeRateClosing = Find(x => x.Id == Id);
            return (Delete(exchangeRateClosing) == 1) ? true : false;
        }

    }
}