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
    public class ExchangeRateRepository : EfRepository<ExchangeRate>, IExchangeRateRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public ExchangeRateRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<ExchangeRate> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<ExchangeRate> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }


        public ExchangeRate GetObjectById(int Id)
        {
            ExchangeRate exchangeRate = Find(x => x.Id == Id && !x.IsDeleted);
            if (exchangeRate != null) { exchangeRate.Errors = new Dictionary<string, string>(); }
            return exchangeRate;
        }

        public ExchangeRate CreateObject(ExchangeRate exchangeRate)
        {
            exchangeRate.IsDeleted = false;
            exchangeRate.CreatedAt = DateTime.Now;
            return Create(exchangeRate);
        }

        public ExchangeRate UpdateObject(ExchangeRate currency)
        {
            currency.UpdatedAt = DateTime.Now;
            Update(currency);
            return currency;
        }

        public ExchangeRate SoftDeleteObject(ExchangeRate exchangeRate)
        {
            exchangeRate.IsDeleted = true;
            exchangeRate.DeletedAt = DateTime.Now;
            Update(exchangeRate);
            return exchangeRate;
        }

        public bool DeleteObject(int Id)
        {
            ExchangeRate exchangeRate = Find(x => x.Id == Id);
            return (Delete(exchangeRate) == 1) ? true : false;
        }

    }
}