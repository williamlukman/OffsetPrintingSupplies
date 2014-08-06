using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repository
{
    public class SalesInvoiceDetailRepository : EfRepository<SalesInvoiceDetail>, ISalesInvoiceDetailRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public SalesInvoiceDetailRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IList<SalesInvoiceDetail> GetObjectsBySalesInvoiceId(int salesInvoiceId)
        {
            return FindAll(sid => sid.SalesInvoiceId == salesInvoiceId && !sid.IsDeleted).ToList();
        }

        public SalesInvoiceDetail GetObjectById(int Id)
        {
            SalesInvoiceDetail detail = Find(sid => sid.Id == Id);
            if (detail != null) { detail.Errors = new Dictionary<string, string>(); }
            return detail;
        }

        public SalesInvoiceDetail CreateObject(SalesInvoiceDetail salesInvoiceDetail)
        {
            string ParentCode = "";
            using (var db = GetContext())
            {
                ParentCode = (from obj in db.SalesInvoices
                              where obj.Id == salesInvoiceDetail.SalesInvoiceId
                              select obj.Code).FirstOrDefault();
            }
            salesInvoiceDetail.Code = SetObjectCode(ParentCode);
            salesInvoiceDetail.IsConfirmed = false;
            salesInvoiceDetail.IsDeleted = false;
            salesInvoiceDetail.CreatedAt = DateTime.Now;
            return Create(salesInvoiceDetail);
        }

        public SalesInvoiceDetail UpdateObject(SalesInvoiceDetail salesInvoiceDetail)
        {
            salesInvoiceDetail.UpdatedAt = DateTime.Now;
            Update(salesInvoiceDetail);
            return salesInvoiceDetail;
        }

        public SalesInvoiceDetail SoftDeleteObject(SalesInvoiceDetail salesInvoiceDetail)
        {
            salesInvoiceDetail.IsDeleted = true;
            salesInvoiceDetail.DeletedAt = DateTime.Now;
            Update(salesInvoiceDetail);
            return salesInvoiceDetail;
        }

        public bool DeleteObject(int Id)
        {
            SalesInvoiceDetail sid = Find(x => x.Id == Id);
            return (Delete(sid) == 1) ? true : false;
        }

        public SalesInvoiceDetail ConfirmObject(SalesInvoiceDetail salesInvoiceDetail)
        {
            salesInvoiceDetail.IsConfirmed = true;
            Update(salesInvoiceDetail);
            return salesInvoiceDetail;
        }

        public SalesInvoiceDetail UnconfirmObject(SalesInvoiceDetail salesInvoiceDetail)
        {
            salesInvoiceDetail.IsConfirmed = false;
            Update(salesInvoiceDetail);
            return salesInvoiceDetail;
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