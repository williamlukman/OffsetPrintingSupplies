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
    public class AccountRepository : EfRepository<Account>, IAccountRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public AccountRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<Account> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<Account> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public Account GetObjectById(int Id)
        {
            Account account = Find(x => x.Id == Id && !x.IsDeleted);
            if (account != null) { account.Errors = new Dictionary<string, string>(); }
            return account;
        }

        public Account GetObjectByIsLegacy(bool IsLegacy)
        {
            Account account = Find(x => x.IsLegacy && !x.IsDeleted);
            if (account != null) { account.Errors = new Dictionary<string, string>(); }
            return account;
        }

        public Account CreateObject(Account account)
        {
            account.IsDeleted = false;
            account.CreatedAt = DateTime.Now;
            return Create(account);
        }

        public Account SoftDeleteObject(Account account)
        {
            account.IsDeleted = true;
            account.DeletedAt = DateTime.Now;
            Update(account);
            return account;
        }

        public bool DeleteObject(int Id)
        {
            Account account = Find(x => x.Id == Id);
            return (Delete(account) == 1) ? true : false;
        }

    }
}