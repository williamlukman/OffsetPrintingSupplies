using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repository
{
    public class SalesDownPaymentDetailRepository : EfRepository<SalesDownPaymentDetail>, ISalesDownPaymentDetailRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public SalesDownPaymentDetailRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<SalesDownPaymentDetail> GetQueryable()
        {
            return FindAll();
        }

        public IList<SalesDownPaymentDetail> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<SalesDownPaymentDetail> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month && !x.IsDeleted).ToList();
        }

        public IList<SalesDownPaymentDetail> GetObjectsBySalesDownPaymentId(int salesDownPayment)
        {
            return FindAll(x => x.SalesDownPaymentId == salesDownPayment && !x.IsDeleted).ToList();
        }

        public IList<SalesDownPaymentDetail> GetObjectsByReceivableId(int receivableId)
        {
            return FindAll(x => x.ReceivableId == receivableId && !x.IsDeleted).ToList();
        }

        public SalesDownPaymentDetail GetObjectById(int Id)
        {
            SalesDownPaymentDetail detail = Find(x => x.Id == Id);
            if (detail != null) { detail.Errors = new Dictionary<string, string>(); }
            return detail;
        }

        public SalesDownPaymentDetail CreateObject(SalesDownPaymentDetail receiptVoucherDetail)
        {
            string ParentCode = "";
            using (var db = GetContext())
            {
                ParentCode = (from obj in db.SalesDownPayments
                              where obj.Id == receiptVoucherDetail.SalesDownPaymentId
                              select obj.Code).FirstOrDefault();
            }
            receiptVoucherDetail.Code = SetObjectCode(ParentCode);
            receiptVoucherDetail.IsConfirmed = false;
            receiptVoucherDetail.IsDeleted = false;
            receiptVoucherDetail.CreatedAt = DateTime.Now;
            return Create(receiptVoucherDetail);
        }

        public SalesDownPaymentDetail UpdateObject(SalesDownPaymentDetail receiptVoucherDetail)
        {
            receiptVoucherDetail.UpdatedAt = DateTime.Now;
            Update(receiptVoucherDetail);
            return receiptVoucherDetail;
        }

        public SalesDownPaymentDetail SoftDeleteObject(SalesDownPaymentDetail receiptVoucherDetail)
        {
            receiptVoucherDetail.IsDeleted = true;
            receiptVoucherDetail.DeletedAt = DateTime.Now;
            Update(receiptVoucherDetail);
            return receiptVoucherDetail;
        }

        public bool DeleteObject(int Id)
        {
            SalesDownPaymentDetail salesDownPaymentDetail = Find(x => x.Id == Id);
            return (Delete(salesDownPaymentDetail) == 1) ? true : false;
        }

        public SalesDownPaymentDetail ConfirmObject(SalesDownPaymentDetail receiptVoucherDetail)
        {
            receiptVoucherDetail.IsConfirmed = true;
            Update(receiptVoucherDetail);
            return receiptVoucherDetail;
        }

        public SalesDownPaymentDetail UnconfirmObject(SalesDownPaymentDetail receiptVoucherDetail)
        {
            receiptVoucherDetail.IsConfirmed = false;
            receiptVoucherDetail.ConfirmationDate = null;
            UpdateObject(receiptVoucherDetail);
            return receiptVoucherDetail;
        }

        public string SetObjectCode(string ParentCode)
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = ParentCode + "." + totalnumberinthemonth;
            return Code;
        } 

    }
}