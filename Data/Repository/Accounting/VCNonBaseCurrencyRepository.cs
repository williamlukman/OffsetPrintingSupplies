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
    public class VCNonBaseCurrencyRepository : EfRepository<VCNonBaseCurrency>, IVCNonBaseCurrencyRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public VCNonBaseCurrencyRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<VCNonBaseCurrency> GetQueryable()
        {
            return FindAll();
        }

        public IList<VCNonBaseCurrency> GetAll()
        {
            return FindAll().ToList();
        }

        public VCNonBaseCurrency GetObjectById(int Id)
        {
            VCNonBaseCurrency validComb = Find(x => x.Id == Id /*&& !x.IsDeleted*/);
            if (validComb != null) { validComb.Errors = new Dictionary<string, string>(); }
            return validComb;
        }

        public VCNonBaseCurrency CreateObject(VCNonBaseCurrency validComb)
        {
            validComb.CreatedAt = DateTime.Now;
            return Create(validComb);
        }

        public VCNonBaseCurrency UpdateObject(VCNonBaseCurrency validComb)
        {
            Update(validComb);
            return validComb;
        }

        /*public VCNonBaseCurrency SoftDeleteObject(VCNonBaseCurrency validComb)
        {
            validComb.IsDeleted = true;
            validComb.DeletedAt = DateTime.Now;
            Update(validComb);
            return validComb;
        }*/

        public bool DeleteObject(int Id)
        {
            VCNonBaseCurrency validComb = Find(x => x.Id == Id);
            return (Delete(validComb) == 1) ? true : false;
        }
    }
}