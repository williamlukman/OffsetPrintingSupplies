using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repository
{
    public class SalesAllowanceDetailRepository : EfRepository<SalesAllowanceDetail>, ISalesAllowanceDetailRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public SalesAllowanceDetailRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<SalesAllowanceDetail> GetQueryable()
        {
            return FindAll();
        }

        public IList<SalesAllowanceDetail> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<SalesAllowanceDetail> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month && !x.IsDeleted).ToList();
        }

        public IList<SalesAllowanceDetail> GetObjectsBySalesAllowanceId(int salesAllowance)
        {
            return FindAll(x => x.SalesAllowanceId == salesAllowance && !x.IsDeleted).ToList();
        }

        public IList<SalesAllowanceDetail> GetObjectsByReceivableId(int receivableId)
        {
            return FindAll(x => x.ReceivableId == receivableId && !x.IsDeleted).ToList();
        }

        public SalesAllowanceDetail GetObjectById(int Id)
        {
            SalesAllowanceDetail detail = Find(x => x.Id == Id);
            if (detail != null) { detail.Errors = new Dictionary<string, string>(); }
            return detail;
        }

        public SalesAllowanceDetail CreateObject(SalesAllowanceDetail receiptVoucherDetail)
        {
            string ParentCode = "";
            using (var db = GetContext())
            {
                ParentCode = (from obj in db.SalesAllowances
                              where obj.Id == receiptVoucherDetail.SalesAllowanceId
                              select obj.Code).FirstOrDefault();
            }
            receiptVoucherDetail.Code = SetObjectCode(ParentCode);
            receiptVoucherDetail.IsConfirmed = false;
            receiptVoucherDetail.IsDeleted = false;
            receiptVoucherDetail.CreatedAt = DateTime.Now;
            return Create(receiptVoucherDetail);
        }

        public SalesAllowanceDetail UpdateObject(SalesAllowanceDetail receiptVoucherDetail)
        {
            receiptVoucherDetail.UpdatedAt = DateTime.Now;
            Update(receiptVoucherDetail);
            return receiptVoucherDetail;
        }

        public SalesAllowanceDetail SoftDeleteObject(SalesAllowanceDetail receiptVoucherDetail)
        {
            receiptVoucherDetail.IsDeleted = true;
            receiptVoucherDetail.DeletedAt = DateTime.Now;
            Update(receiptVoucherDetail);
            return receiptVoucherDetail;
        }

        public bool DeleteObject(int Id)
        {
            SalesAllowanceDetail salesAllowanceDetail = Find(x => x.Id == Id);
            return (Delete(salesAllowanceDetail) == 1) ? true : false;
        }

        public SalesAllowanceDetail ConfirmObject(SalesAllowanceDetail receiptVoucherDetail)
        {
            receiptVoucherDetail.IsConfirmed = true;
            Update(receiptVoucherDetail);
            return receiptVoucherDetail;
        }

        public SalesAllowanceDetail UnconfirmObject(SalesAllowanceDetail receiptVoucherDetail)
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