using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repository
{
    public class PaymentVoucherDetailRepository : EfRepository<PaymentVoucherDetail>, IPaymentVoucherDetailRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public PaymentVoucherDetailRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IList<PaymentVoucherDetail> GetObjectsByPaymentVoucherId(int paymentVoucherId)
        {
            return FindAll(pvd => pvd.PaymentVoucherId == paymentVoucherId && !pvd.IsDeleted).ToList();
        }

        public IList<PaymentVoucherDetail> GetObjectsByPayableId(int payableId)
        {
            return FindAll(pvd => pvd.PayableId == payableId && !pvd.IsDeleted).ToList();
        }

        public PaymentVoucherDetail GetObjectById(int Id)
        {
            PaymentVoucherDetail detail = Find(pvd => pvd.Id == Id);
            if (detail != null) { detail.Errors = new Dictionary<string, string>(); }
            return detail;
        }

        public PaymentVoucherDetail CreateObject(PaymentVoucherDetail paymentVoucherDetail)
        {
            string ParentCode = "";
            using (var db = GetContext())
            {
                ParentCode = (from obj in db.PaymentVouchers
                              where obj.Id == paymentVoucherDetail.PaymentVoucherId
                              select obj.Code).FirstOrDefault();
            }
            paymentVoucherDetail.Code = SetObjectCode(ParentCode);
            paymentVoucherDetail.IsConfirmed = false;
            paymentVoucherDetail.IsDeleted = false;
            paymentVoucherDetail.CreatedAt = DateTime.Now;
            return Create(paymentVoucherDetail);
        }

        public PaymentVoucherDetail UpdateObject(PaymentVoucherDetail paymentVoucherDetail)
        {
            paymentVoucherDetail.UpdatedAt = DateTime.Now;
            Update(paymentVoucherDetail);
            return paymentVoucherDetail;
        }

        public PaymentVoucherDetail SoftDeleteObject(PaymentVoucherDetail paymentVoucherDetail)
        {
            paymentVoucherDetail.IsDeleted = true;
            paymentVoucherDetail.DeletedAt = DateTime.Now;
            Update(paymentVoucherDetail);
            return paymentVoucherDetail;
        }

        public bool DeleteObject(int Id)
        {
            PaymentVoucherDetail pvd = Find(x => x.Id == Id);
            return (Delete(pvd) == 1) ? true : false;
        }

        public PaymentVoucherDetail ConfirmObject(PaymentVoucherDetail paymentVoucherDetail)
        {
            paymentVoucherDetail.IsConfirmed = true;
            paymentVoucherDetail.ConfirmationDate = DateTime.Now;
            Update(paymentVoucherDetail);
            return paymentVoucherDetail;
        }

        public PaymentVoucherDetail UnconfirmObject(PaymentVoucherDetail paymentVoucherDetail)
        {
            paymentVoucherDetail.IsConfirmed = false;
            paymentVoucherDetail.ConfirmationDate = null;
            UpdateObject(paymentVoucherDetail);
            return paymentVoucherDetail;
        }

        public string SetObjectCode(string ParentCode)
        {
            // Code: #{parent_object.code}/#{total_number_objects}
            int totalobject = FindAll().Count() + 1;
            string Code = ParentCode + "/#" + totalobject;
            return Code;
        } 

    }
}