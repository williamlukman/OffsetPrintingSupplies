using Core.DomainModel;
using Core.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Context;
using System.Data;
using System.Data.Entity;

namespace Data.Repository
{
    public class GLNonBaseCurrencyRepository : EfRepository<GLNonBaseCurrency>, IGLNonBaseCurrencyRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public GLNonBaseCurrencyRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<GLNonBaseCurrency> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<GLNonBaseCurrency> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public GLNonBaseCurrency GetObjectById(int Id)
        {
            GLNonBaseCurrency generalLedgerJournal = Find(x => x.Id == Id && !x.IsDeleted);
            if (generalLedgerJournal != null) { generalLedgerJournal.Errors = new Dictionary<string, string>(); }
            return generalLedgerJournal;
        }


        public GLNonBaseCurrency CreateObject(GLNonBaseCurrency generalLedgerJournal)
        {
            generalLedgerJournal.IsDeleted = false;
            generalLedgerJournal.CreatedAt = DateTime.Now;
            return Create(generalLedgerJournal);
        }

        public GLNonBaseCurrency SoftDeleteObject(GLNonBaseCurrency generalLedgerJournal)
        {
            generalLedgerJournal.IsDeleted = true;
            generalLedgerJournal.DeletedAt = DateTime.Now;
            Update(generalLedgerJournal);
            return generalLedgerJournal;
        }

        public bool DeleteObject(int Id)
        {
            GLNonBaseCurrency generalLedgerJournal = Find(x => x.Id == Id);
            return (Delete(generalLedgerJournal) == 1) ? true : false;
        }

    }
}