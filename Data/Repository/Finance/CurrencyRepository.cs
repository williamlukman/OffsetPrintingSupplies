using Core.DomainModel;
using Core.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Context;
using Data.Repository;
using System.Data;

namespace Data.Repository
{
    public class CurrencyRepository : EfRepository<Currency>, ICurrencyRepository
    {

        private OffsetPrintingSuppliesEntities entities;
        public CurrencyRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<Currency> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<Currency> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public Currency GetObjectById(int Id)
        {
            Currency cb = Find(x => x.Id == Id && !x.IsDeleted);
            if (cb != null) { cb.Errors = new Dictionary<string, string>(); }
            return cb;
        }

        public Currency GetObjectByName(string Name)
        {
            Currency cb = Find(x => x.Name == Name && !x.IsDeleted);
            if (cb != null) { cb.Errors = new Dictionary<string, string>(); }
            return cb;
        }

        public Currency CreateObject(Currency currency)
        {
            currency.IsDeleted = false;
            currency.CreatedAt = DateTime.Now;
            return Create(currency);
        }

        public Currency UpdateObject(Currency currency)
        {
            currency.UpdatedAt = DateTime.Now;
            Update(currency);
            return currency;
        }

        public Currency SoftDeleteObject(Currency currency)
        {
            currency.IsDeleted = true;
            currency.DeletedAt = DateTime.Now;
            Update(currency);
            return currency;
        }

        public bool DeleteObject(int Id)
        {
            Currency currency = Find(x => x.Id == Id);           
            return (Delete(currency) == 1) ? true : false;
        }
        
    }
}