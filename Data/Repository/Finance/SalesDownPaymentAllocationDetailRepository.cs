using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repository
{
    public class SalesDownPaymentAllocationDetailRepository : EfRepository<SalesDownPaymentAllocationDetail>, ISalesDownPaymentAllocationDetailRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public SalesDownPaymentAllocationDetailRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<SalesDownPaymentAllocationDetail> GetQueryable()
        {
            return FindAll();
        }

        public IList<SalesDownPaymentAllocationDetail> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<SalesDownPaymentAllocationDetail> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month && !x.IsDeleted).ToList();
        }

        public IList<SalesDownPaymentAllocationDetail> GetObjectsBySalesDownPaymentAllocationId(int salesDownPaymentAllocationId)
        {
            return FindAll(x => x.SalesDownPaymentAllocationId == salesDownPaymentAllocationId && !x.IsDeleted).ToList();
        }

        public IList<SalesDownPaymentAllocationDetail> GetObjectsByReceivableId(int receivableId)
        {
            return FindAll(x => x.ReceivableId == receivableId && !x.IsDeleted).ToList();
        }

        public SalesDownPaymentAllocationDetail GetObjectByReceiptVoucherDetailId(int ReceiptVoucherDetailId)
        {
            SalesDownPaymentAllocationDetail detail = Find(x => x.ReceiptVoucherDetailId == ReceiptVoucherDetailId);
            if (detail != null) { detail.Errors = new Dictionary<string, string>(); }
            return detail;
        }

        public SalesDownPaymentAllocationDetail GetObjectById(int Id)
        {
            SalesDownPaymentAllocationDetail detail = Find(x => x.Id == Id);
            if (detail != null) { detail.Errors = new Dictionary<string, string>(); }
            return detail;
        }

        public SalesDownPaymentAllocationDetail CreateObject(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail)
        {
            string ParentCode = "";
            using (var db = GetContext())
            {
                ParentCode = (from obj in db.SalesDownPaymentAllocations
                              where obj.Id == salesDownPaymentAllocationDetail.SalesDownPaymentAllocationId
                              select obj.Code).FirstOrDefault();
            }
            salesDownPaymentAllocationDetail.Code = SetObjectCode(ParentCode);
            salesDownPaymentAllocationDetail.IsConfirmed = false;
            salesDownPaymentAllocationDetail.IsDeleted = false;
            salesDownPaymentAllocationDetail.CreatedAt = DateTime.Now;
            return Create(salesDownPaymentAllocationDetail);
        }

        public SalesDownPaymentAllocationDetail UpdateObject(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail)
        {
            salesDownPaymentAllocationDetail.UpdatedAt = DateTime.Now;
            Update(salesDownPaymentAllocationDetail);
            return salesDownPaymentAllocationDetail;
        }

        public SalesDownPaymentAllocationDetail SoftDeleteObject(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail)
        {
            salesDownPaymentAllocationDetail.IsDeleted = true;
            salesDownPaymentAllocationDetail.DeletedAt = DateTime.Now;
            Update(salesDownPaymentAllocationDetail);
            return salesDownPaymentAllocationDetail;
        }

        public bool DeleteObject(int Id)
        {
            SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail = Find(x => x.Id == Id);
            return (Delete(salesDownPaymentAllocationDetail) == 1) ? true : false;
        }

        public SalesDownPaymentAllocationDetail ConfirmObject(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail)
        {
            salesDownPaymentAllocationDetail.IsConfirmed = true;
            Update(salesDownPaymentAllocationDetail);
            return salesDownPaymentAllocationDetail;
        }

        public SalesDownPaymentAllocationDetail UnconfirmObject(SalesDownPaymentAllocationDetail salesDownPaymentAllocationDetail)
        {
            salesDownPaymentAllocationDetail.IsConfirmed = false;
            salesDownPaymentAllocationDetail.ConfirmationDate = null;
            UpdateObject(salesDownPaymentAllocationDetail);
            return salesDownPaymentAllocationDetail;
        }

        public string SetObjectCode(string ParentCode)
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = ParentCode + "." + totalnumberinthemonth;
            return Code;
        } 

    }
}