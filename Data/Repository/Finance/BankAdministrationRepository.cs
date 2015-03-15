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
    public class BankAdministrationRepository : EfRepository<BankAdministration>, IBankAdministrationRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public BankAdministrationRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<BankAdministration> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<BankAdministration> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<BankAdministration> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month && !x.IsDeleted).ToList();
        }

        public IList<BankAdministration> GetObjectsByCashBankId(int cashBankId)
        {
            return FindAll(x => x.CashBankId == cashBankId && !x.IsDeleted).ToList();
        }

        public BankAdministration GetObjectById(int Id)
        {
            BankAdministration bankAdministration = Find(x => x.Id == Id && !x.IsDeleted);
            if (bankAdministration != null) { bankAdministration.Errors = new Dictionary<string, string>(); }
            return bankAdministration;
        }

        public BankAdministration CreateObject(BankAdministration bankAdministration)
        {
            bankAdministration.Code = SetObjectCode();
            bankAdministration.IsDeleted = false;
            bankAdministration.IsConfirmed = false;
            bankAdministration.CreatedAt = DateTime.Now;
            return Create(bankAdministration);
        }

        public BankAdministration UpdateObject(BankAdministration bankAdministration)
        {
            bankAdministration.UpdatedAt = DateTime.Now;
            Update(bankAdministration);
            return bankAdministration;
        }

        public BankAdministration SoftDeleteObject(BankAdministration bankAdministration)
        {
            bankAdministration.IsDeleted = true;
            bankAdministration.DeletedAt = DateTime.Now;
            Update(bankAdministration);
            return bankAdministration;
        }

        public BankAdministration ConfirmObject(BankAdministration bankAdministration)
        {
            bankAdministration.IsConfirmed = true;
            Update(bankAdministration);
            return bankAdministration;
        }

        public BankAdministration UnconfirmObject(BankAdministration bankAdministration)
        {
            bankAdministration.IsConfirmed = false;
            bankAdministration.ConfirmationDate = null;
            bankAdministration.UpdatedAt = DateTime.Now;
            Update(bankAdministration);
            return bankAdministration;
        }

        public bool DeleteObject(int Id)
        {
            BankAdministration bankAdministration = Find(x => x.Id == Id);
            return (Delete(bankAdministration) == 1) ? true : false;
        }

        public string SetObjectCode()
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = DateTime.Today.Year.ToString() + "." + DateTime.Today.Month.ToString() + "." + totalnumberinthemonth;
            return Code;
        }
    }
}