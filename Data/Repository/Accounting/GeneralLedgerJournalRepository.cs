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
    public class GeneralLedgerJournalRepository : EfRepository<GeneralLedgerJournal>, IGeneralLedgerJournalRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public GeneralLedgerJournalRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<GeneralLedgerJournal> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<GeneralLedgerJournal> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<GeneralLedgerJournal> GetObjectsByAccountId(int accountId)
        {
            return FindAll(x => !x.IsDeleted).Include("Account").Where(x => x.Account.Id == accountId).ToList();
        }

        public GeneralLedgerJournal GetObjectById(int Id)
        {
            GeneralLedgerJournal generalLedgerJournal = Find(x => x.Id == Id && !x.IsDeleted);
            if (generalLedgerJournal != null) { generalLedgerJournal.Errors = new Dictionary<string, string>(); }
            return generalLedgerJournal;
        }

        public IList<GeneralLedgerJournal> GetObjectsBySourceDocument(int accountId, string SourceDocument, int SourceDocumentId)
        {
            return FindAll(x => x.SourceDocument == SourceDocument
                                && x.SourceDocumentId == SourceDocumentId && !x.IsDeleted).Include("Account")
                   .Where(x => x.Account.Id == accountId).ToList();
        }

        public GeneralLedgerJournal CreateObject(GeneralLedgerJournal generalLedgerJournal)
        {
            generalLedgerJournal.IsDeleted = false;
            generalLedgerJournal.CreatedAt = DateTime.Now;
            return Create(generalLedgerJournal);
        }

        public GeneralLedgerJournal SoftDeleteObject(GeneralLedgerJournal generalLedgerJournal)
        {
            generalLedgerJournal.IsDeleted = true;
            generalLedgerJournal.DeletedAt = DateTime.Now;
            Update(generalLedgerJournal);
            return generalLedgerJournal;
        }

        public bool DeleteObject(int Id)
        {
            GeneralLedgerJournal generalLedgerJournal = Find(x => x.Id == Id);
            return (Delete(generalLedgerJournal) == 1) ? true : false;
        }

    }
}