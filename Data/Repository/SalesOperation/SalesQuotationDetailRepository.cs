using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repository
{
    public class SalesQuotationDetailRepository : EfRepository<SalesQuotationDetail>, ISalesQuotationDetailRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public SalesQuotationDetailRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<SalesQuotationDetail> GetQueryable()
        {
            return FindAll();
        }

        public IList<SalesQuotationDetail> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<SalesQuotationDetail> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month && !x.IsDeleted).ToList();
        }

        public IList<SalesQuotationDetail> GetObjectsBySalesQuotationId(int salesQuotationId)
        {
            return FindAll(sod => sod.SalesQuotationId == salesQuotationId && !sod.IsDeleted).ToList();
        }

        public IList<SalesQuotationDetail> GetObjectsByItemId(int itemId)
        {
            return FindAll(x => x.ItemId == itemId && !x.IsDeleted).ToList();
        }

        public SalesQuotationDetail GetObjectById(int Id)
        {
            SalesQuotationDetail detail = Find(sod => sod.Id == Id);
            if (detail != null) { detail.Errors = new Dictionary<string, string>(); }
            return detail;
        }

        public SalesQuotationDetail CreateObject(SalesQuotationDetail salesQuotationDetail)
        {
            string ParentCode = "";
            using (var db = GetContext())
            {
                ParentCode = (from obj in db.SalesQuotations
                              where obj.Id == salesQuotationDetail.SalesQuotationId
                              select obj.Code).FirstOrDefault();
            }
            salesQuotationDetail.Code = SetObjectCode(ParentCode);
            salesQuotationDetail.IsConfirmed = false;
            salesQuotationDetail.IsDeleted = false;
            salesQuotationDetail.CreatedAt = DateTime.Now;
            return Create(salesQuotationDetail);
        }

        public SalesQuotationDetail UpdateObject(SalesQuotationDetail salesQuotationDetail)
        {
            salesQuotationDetail.UpdatedAt = DateTime.Now;
            Update(salesQuotationDetail);
            return salesQuotationDetail;
        }

        public SalesQuotationDetail SoftDeleteObject(SalesQuotationDetail salesQuotationDetail)
        {
            salesQuotationDetail.IsDeleted = true;
            salesQuotationDetail.DeletedAt = DateTime.Now;
            Update(salesQuotationDetail);
            return salesQuotationDetail;
        }

        public bool DeleteObject(int Id)
        {
            SalesQuotationDetail salesQuotationDetail = Find(x => x.Id == Id);
            return (Delete(salesQuotationDetail) == 1) ? true : false;
        }

        public SalesQuotationDetail ConfirmObject(SalesQuotationDetail salesQuotationDetail)
        {
            salesQuotationDetail.IsConfirmed = true;
            Update(salesQuotationDetail);
            return salesQuotationDetail;
        }

        public SalesQuotationDetail UnconfirmObject(SalesQuotationDetail salesQuotationDetail)
        {
            salesQuotationDetail.IsConfirmed = false;
            salesQuotationDetail.ConfirmationDate = null;
            UpdateObject(salesQuotationDetail);
            return salesQuotationDetail;
        }

        public string SetObjectCode(string ParentCode)
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = ParentCode + "." + totalnumberinthemonth;
            return Code;
        } 

    }
}