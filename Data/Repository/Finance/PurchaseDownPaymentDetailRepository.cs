using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repository
{
    public class PurchaseDownPaymentDetailRepository : EfRepository<PurchaseDownPaymentDetail>, IPurchaseDownPaymentDetailRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public PurchaseDownPaymentDetailRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<PurchaseDownPaymentDetail> GetQueryable()
        {
            return FindAll();
        }

        public IList<PurchaseDownPaymentDetail> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<PurchaseDownPaymentDetail> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month && !x.IsDeleted).ToList();
        }

        public IList<PurchaseDownPaymentDetail> GetObjectsByPurchaseDownPaymentId(int purchaseDownPaymentId)
        {
            return FindAll(x => x.PurchaseDownPaymentId == purchaseDownPaymentId && !x.IsDeleted).ToList();
        }

        public IList<PurchaseDownPaymentDetail> GetObjectsByPayableId(int payableId)
        {
            return FindAll(x => x.PayableId == payableId && !x.IsDeleted).ToList();
        }

        public PurchaseDownPaymentDetail GetObjectById(int Id)
        {
            PurchaseDownPaymentDetail detail = Find(x => x.Id == Id);
            if (detail != null) { detail.Errors = new Dictionary<string, string>(); }
            return detail;
        }

        public PurchaseDownPaymentDetail CreateObject(PurchaseDownPaymentDetail purchaseDownPaymentDetail)
        {
            string ParentCode = "";
            using (var db = GetContext())
            {
                ParentCode = (from obj in db.PurchaseDownPayments
                              where obj.Id == purchaseDownPaymentDetail.PurchaseDownPaymentId
                              select obj.Code).FirstOrDefault();
            }
            purchaseDownPaymentDetail.Code = SetObjectCode(ParentCode);
            purchaseDownPaymentDetail.IsConfirmed = false;
            purchaseDownPaymentDetail.IsDeleted = false;
            purchaseDownPaymentDetail.CreatedAt = DateTime.Now;
            return Create(purchaseDownPaymentDetail);
        }

        public PurchaseDownPaymentDetail UpdateObject(PurchaseDownPaymentDetail purchaseDownPaymentDetail)
        {
            purchaseDownPaymentDetail.UpdatedAt = DateTime.Now;
            Update(purchaseDownPaymentDetail);
            return purchaseDownPaymentDetail;
        }

        public PurchaseDownPaymentDetail SoftDeleteObject(PurchaseDownPaymentDetail purchaseDownPaymentDetail)
        {
            purchaseDownPaymentDetail.IsDeleted = true;
            purchaseDownPaymentDetail.DeletedAt = DateTime.Now;
            Update(purchaseDownPaymentDetail);
            return purchaseDownPaymentDetail;
        }

        public bool DeleteObject(int Id)
        {
            PurchaseDownPaymentDetail purchaseDownPaymentDetail = Find(x => x.Id == Id);
            return (Delete(purchaseDownPaymentDetail) == 1) ? true : false;
        }

        public PurchaseDownPaymentDetail ConfirmObject(PurchaseDownPaymentDetail purchaseDownPaymentDetail)
        {
            purchaseDownPaymentDetail.IsConfirmed = true;
            Update(purchaseDownPaymentDetail);
            return purchaseDownPaymentDetail;
        }

        public PurchaseDownPaymentDetail UnconfirmObject(PurchaseDownPaymentDetail purchaseDownPaymentDetail)
        {
            purchaseDownPaymentDetail.IsConfirmed = false;
            purchaseDownPaymentDetail.ConfirmationDate = null;
            UpdateObject(purchaseDownPaymentDetail);
            return purchaseDownPaymentDetail;
        }

        public string SetObjectCode(string ParentCode)
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = ParentCode + "." + totalnumberinthemonth;
            return Code;
        } 

    }
}