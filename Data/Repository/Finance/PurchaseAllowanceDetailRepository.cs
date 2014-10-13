using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repository
{
    public class PurchaseAllowanceDetailRepository : EfRepository<PurchaseAllowanceDetail>, IPurchaseAllowanceDetailRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public PurchaseAllowanceDetailRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<PurchaseAllowanceDetail> GetQueryable()
        {
            return FindAll();
        }

        public IList<PurchaseAllowanceDetail> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<PurchaseAllowanceDetail> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month && !x.IsDeleted).ToList();
        }

        public IList<PurchaseAllowanceDetail> GetObjectsByPurchaseAllowanceId(int purchaseAllowanceId)
        {
            return FindAll(x => x.PurchaseAllowanceId == purchaseAllowanceId && !x.IsDeleted).ToList();
        }

        public IList<PurchaseAllowanceDetail> GetObjectsByPayableId(int payableId)
        {
            return FindAll(x => x.PayableId == payableId && !x.IsDeleted).ToList();
        }

        public PurchaseAllowanceDetail GetObjectById(int Id)
        {
            PurchaseAllowanceDetail detail = Find(x => x.Id == Id);
            if (detail != null) { detail.Errors = new Dictionary<string, string>(); }
            return detail;
        }

        public PurchaseAllowanceDetail CreateObject(PurchaseAllowanceDetail purchaseAllowanceDetail)
        {
            string ParentCode = "";
            using (var db = GetContext())
            {
                ParentCode = (from obj in db.PurchaseAllowances
                              where obj.Id == purchaseAllowanceDetail.PurchaseAllowanceId
                              select obj.Code).FirstOrDefault();
            }
            purchaseAllowanceDetail.Code = SetObjectCode(ParentCode);
            purchaseAllowanceDetail.IsConfirmed = false;
            purchaseAllowanceDetail.IsDeleted = false;
            purchaseAllowanceDetail.CreatedAt = DateTime.Now;
            return Create(purchaseAllowanceDetail);
        }

        public PurchaseAllowanceDetail UpdateObject(PurchaseAllowanceDetail purchaseAllowanceDetail)
        {
            purchaseAllowanceDetail.UpdatedAt = DateTime.Now;
            Update(purchaseAllowanceDetail);
            return purchaseAllowanceDetail;
        }

        public PurchaseAllowanceDetail SoftDeleteObject(PurchaseAllowanceDetail purchaseAllowanceDetail)
        {
            purchaseAllowanceDetail.IsDeleted = true;
            purchaseAllowanceDetail.DeletedAt = DateTime.Now;
            Update(purchaseAllowanceDetail);
            return purchaseAllowanceDetail;
        }

        public bool DeleteObject(int Id)
        {
            PurchaseAllowanceDetail purchaseAllowanceDetail = Find(x => x.Id == Id);
            return (Delete(purchaseAllowanceDetail) == 1) ? true : false;
        }

        public PurchaseAllowanceDetail ConfirmObject(PurchaseAllowanceDetail purchaseAllowanceDetail)
        {
            purchaseAllowanceDetail.IsConfirmed = true;
            Update(purchaseAllowanceDetail);
            return purchaseAllowanceDetail;
        }

        public PurchaseAllowanceDetail UnconfirmObject(PurchaseAllowanceDetail purchaseAllowanceDetail)
        {
            purchaseAllowanceDetail.IsConfirmed = false;
            purchaseAllowanceDetail.ConfirmationDate = null;
            UpdateObject(purchaseAllowanceDetail);
            return purchaseAllowanceDetail;
        }

        public string SetObjectCode(string ParentCode)
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = ParentCode + "." + totalnumberinthemonth;
            return Code;
        } 

    }
}