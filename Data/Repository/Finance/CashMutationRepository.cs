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
    public class CashMutationRepository : EfRepository<CashMutation>, ICashMutationRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public CashMutationRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<CashMutation> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<CashMutation> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<CashMutation> GetObjectsByCashBankId(int cashBankId)
        {
            return FindAll(x => x.CashBankId == cashBankId && !x.IsDeleted).ToList();
        }

        public CashMutation GetObjectById(int Id)
        {
            CashMutation cashMutation = Find(x => x.Id == Id && !x.IsDeleted);
            if (cashMutation != null) { cashMutation.Errors = new Dictionary<string, string>(); }
            return cashMutation;
        }

        public IList<CashMutation> GetObjectsBySourceDocument(int cashBankId, string SourceDocumentType, int SourceDocumentId)
        {
            return FindAll(x => x.CashBankId == cashBankId && x.SourceDocumentType == SourceDocumentType
                                && x.SourceDocumentId == SourceDocumentId && !x.IsDeleted).ToList();
        }

        public CashMutation CreateObject(CashMutation cashMutation)
        {
            cashMutation.IsDeleted = false;
            cashMutation.CreatedAt = DateTime.Now;
            return Create(cashMutation);
        }

        public CashMutation SoftDeleteObject(CashMutation cashMutation)
        {
            cashMutation.IsDeleted = true;
            cashMutation.DeletedAt = DateTime.Now;
            Update(cashMutation);
            return cashMutation;
        }

        public bool DeleteObject(int Id)
        {
            CashMutation cashMutation = Find(x => x.Id == Id);
            return (Delete(cashMutation) == 1) ? true : false;
        }

    }
}