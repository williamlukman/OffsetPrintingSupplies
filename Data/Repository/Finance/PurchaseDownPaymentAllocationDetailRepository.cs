using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repository
{
    public class PurchaseDownPaymentAllocationDetailRepository : EfRepository<PurchaseDownPaymentAllocationDetail>, IPurchaseDownPaymentAllocationDetailRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public PurchaseDownPaymentAllocationDetailRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<PurchaseDownPaymentAllocationDetail> GetQueryable()
        {
            return FindAll();
        }

        public IList<PurchaseDownPaymentAllocationDetail> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<PurchaseDownPaymentAllocationDetail> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month && !x.IsDeleted).ToList();
        }

        public IList<PurchaseDownPaymentAllocationDetail> GetObjectsByPurchaseDownPaymentAllocationId(int purchaseDownPaymentAllocationId)
        {
            return FindAll(x => x.PurchaseDownPaymentAllocationId == purchaseDownPaymentAllocationId && !x.IsDeleted).ToList();
        }

        public IList<PurchaseDownPaymentAllocationDetail> GetObjectsByPayableId(int payableId)
        {
            return FindAll(x => x.PayableId == payableId && !x.IsDeleted).ToList();
        }

        public PurchaseDownPaymentAllocationDetail GetObjectByPaymentVoucherDetailId(int PaymentVoucherDetailId)
        {
            PurchaseDownPaymentAllocationDetail detail = Find(x => x.PaymentVoucherDetailId == PaymentVoucherDetailId);
            if (detail != null) { detail.Errors = new Dictionary<string, string>(); }
            return detail;
        }

        public PurchaseDownPaymentAllocationDetail GetObjectById(int Id)
        {
            PurchaseDownPaymentAllocationDetail detail = Find(x => x.Id == Id);
            if (detail != null) { detail.Errors = new Dictionary<string, string>(); }
            return detail;
        }

        public PurchaseDownPaymentAllocationDetail CreateObject(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail)
        {
            string ParentCode = "";
            using (var db = GetContext())
            {
                ParentCode = (from obj in db.PurchaseDownPaymentAllocations
                              where obj.Id == purchaseDownPaymentAllocationDetail.PurchaseDownPaymentAllocationId
                              select obj.Code).FirstOrDefault();
            }
            purchaseDownPaymentAllocationDetail.Code = SetObjectCode(ParentCode);
            purchaseDownPaymentAllocationDetail.IsConfirmed = false;
            purchaseDownPaymentAllocationDetail.IsDeleted = false;
            purchaseDownPaymentAllocationDetail.CreatedAt = DateTime.Now;
            return Create(purchaseDownPaymentAllocationDetail);
        }

        public PurchaseDownPaymentAllocationDetail UpdateObject(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail)
        {
            purchaseDownPaymentAllocationDetail.UpdatedAt = DateTime.Now;
            Update(purchaseDownPaymentAllocationDetail);
            return purchaseDownPaymentAllocationDetail;
        }

        public PurchaseDownPaymentAllocationDetail SoftDeleteObject(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail)
        {
            purchaseDownPaymentAllocationDetail.IsDeleted = true;
            purchaseDownPaymentAllocationDetail.DeletedAt = DateTime.Now;
            Update(purchaseDownPaymentAllocationDetail);
            return purchaseDownPaymentAllocationDetail;
        }

        public bool DeleteObject(int Id)
        {
            PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail = Find(x => x.Id == Id);
            return (Delete(purchaseDownPaymentAllocationDetail) == 1) ? true : false;
        }

        public PurchaseDownPaymentAllocationDetail ConfirmObject(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail)
        {
            purchaseDownPaymentAllocationDetail.IsConfirmed = true;
            Update(purchaseDownPaymentAllocationDetail);
            return purchaseDownPaymentAllocationDetail;
        }

        public PurchaseDownPaymentAllocationDetail UnconfirmObject(PurchaseDownPaymentAllocationDetail purchaseDownPaymentAllocationDetail)
        {
            purchaseDownPaymentAllocationDetail.IsConfirmed = false;
            purchaseDownPaymentAllocationDetail.ConfirmationDate = null;
            UpdateObject(purchaseDownPaymentAllocationDetail);
            return purchaseDownPaymentAllocationDetail;
        }

        public string SetObjectCode(string ParentCode)
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = ParentCode + "." + totalnumberinthemonth;
            return Code;
        } 

    }
}