using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repository
{
    public class BankAdministrationDetailRepository : EfRepository<BankAdministrationDetail>, IBankAdministrationDetailRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public BankAdministrationDetailRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<BankAdministrationDetail> GetQueryable()
        {
            return FindAll();
        }

        public IList<BankAdministrationDetail> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<BankAdministrationDetail> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month && !x.IsDeleted).ToList();
        }

        public IList<BankAdministrationDetail> GetObjectsByBankAdministrationId(int bankAdministrationId)
        {
            return FindAll(x => x.BankAdministrationId == bankAdministrationId && !x.IsDeleted).ToList();
        }

        public IList<BankAdministrationDetail> GetNonLegacyObjectsByBankAdministrationId(int bankAdministrationId)
        {
            return FindAll(x => x.BankAdministrationId == bankAdministrationId && !x.IsLegacy && !x.IsDeleted).ToList();
        }

        public BankAdministrationDetail GetLegacyObjectByBankAdministrationId(int bankAdministrationId)
        {
            BankAdministrationDetail detail = Find(x => x.BankAdministrationId == bankAdministrationId && x.IsLegacy && !x.IsDeleted);
            if (detail != null) { detail.Errors = new Dictionary<string, string>(); }
            return detail;
        }

        public BankAdministrationDetail GetObjectById(int Id)
        {
            BankAdministrationDetail detail = Find(x => x.Id == Id);
            if (detail != null) { detail.Errors = new Dictionary<string, string>(); }
            return detail;
        }

        public BankAdministrationDetail CreateObject(BankAdministrationDetail bankAdministrationDetail)
        {
            string ParentCode = "";
            using (var db = GetContext())
            {
                ParentCode = (from obj in db.BankAdministrations
                              where obj.Id == bankAdministrationDetail.BankAdministrationId
                              select obj.Code).FirstOrDefault();
            }
            bankAdministrationDetail.Code = SetObjectCode(ParentCode);
            bankAdministrationDetail.IsConfirmed = false;
            bankAdministrationDetail.IsDeleted = false;
            bankAdministrationDetail.CreatedAt = DateTime.Now;
            return Create(bankAdministrationDetail);
        }

        public BankAdministrationDetail UpdateObject(BankAdministrationDetail bankAdministrationDetail)
        {
            bankAdministrationDetail.UpdatedAt = DateTime.Now;
            Update(bankAdministrationDetail);
            return bankAdministrationDetail;
        }

        public BankAdministrationDetail SoftDeleteObject(BankAdministrationDetail bankAdministrationDetail)
        {
            bankAdministrationDetail.IsDeleted = true;
            bankAdministrationDetail.DeletedAt = DateTime.Now;
            Update(bankAdministrationDetail);
            return bankAdministrationDetail;
        }

        public bool DeleteObject(int Id)
        {
            BankAdministrationDetail bankAdministrationDetail = Find(x => x.Id == Id);
            return (Delete(bankAdministrationDetail) == 1) ? true : false;
        }

        public BankAdministrationDetail ConfirmObject(BankAdministrationDetail bankAdministrationDetail)
        {
            bankAdministrationDetail.IsConfirmed = true;
            Update(bankAdministrationDetail);
            return bankAdministrationDetail;
        }

        public BankAdministrationDetail UnconfirmObject(BankAdministrationDetail bankAdministrationDetail)
        {
            bankAdministrationDetail.IsConfirmed = false;
            bankAdministrationDetail.ConfirmationDate = null;
            UpdateObject(bankAdministrationDetail);
            return bankAdministrationDetail;
        }

        public string SetObjectCode(string ParentCode)
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = ParentCode + "." + totalnumberinthemonth;
            return Code;
        } 
    }
}