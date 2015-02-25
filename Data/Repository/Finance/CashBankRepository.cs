using Core.DomainModel;
using Core.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Context;
using Data.Repository;
using System.Data;
using System.Linq.Dynamic;
using System.Data.Entity;

namespace Data.Repository
{
    public class CashBankRepository : EfRepository<CashBank>, ICashBankRepository
    {

        private OffsetPrintingSuppliesEntities entities;
        public CashBankRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<CashBank> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<CashBank> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public CashBank GetObjectById(int Id)
        {
            CashBank cb = FindAll(x => x.Id == Id && !x.IsDeleted).Include(x => x.Currency).FirstOrDefault();
            if (cb != null) { cb.Errors = new Dictionary<string, string>(); }
            return cb;
        }

        public CashBank GetObjectByName(string Name)
        {
            CashBank cb = Find(x => x.Name == Name && !x.IsDeleted);
            if (cb != null) { cb.Errors = new Dictionary<string, string>(); }
            return cb;
        }

        public CashBank CreateObject(CashBank cashbank)
        {
            cashbank.IsDeleted = false;
            cashbank.CreatedAt = DateTime.Now;
            return Create(cashbank);
        }

        public CashBank UpdateObject(CashBank cashbank)
        {
            cashbank.UpdatedAt = DateTime.Now;
            Update(cashbank);
            return cashbank;
        }

        public CashBank SoftDeleteObject(CashBank cashbank)
        {
            cashbank.IsDeleted = true;
            cashbank.DeletedAt = DateTime.Now;
            Update(cashbank);
            return cashbank;
        }

        public bool DeleteObject(int Id)
        {
            CashBank cashbank = Find(x => x.Id == Id);           
            return (Delete(cashbank) == 1) ? true : false;
        }
        
    }
}