using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;

namespace Data.Repository
{
    public class RetailSalesInvoiceDetailRepository : EfRepository<RetailSalesInvoiceDetail>, IRetailSalesInvoiceDetailRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public RetailSalesInvoiceDetailRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IList<RetailSalesInvoiceDetail> GetAll()
        {
            return FindAll().ToList();
        }

        public IList<RetailSalesInvoiceDetail> GetObjectsByRetailSalesInvoiceId(int RetailSalesInvoiceId)
        {
            return FindAll(x => x.RetailSalesInvoiceId == RetailSalesInvoiceId && !x.IsDeleted).ToList();
        }

        public RetailSalesInvoiceDetail GetObjectById(int Id)
        {
            RetailSalesInvoiceDetail retailSalesInvoiceDetail = Find(x => x.Id == Id && !x.IsDeleted);
            if (retailSalesInvoiceDetail != null) { retailSalesInvoiceDetail.Errors = new Dictionary<string, string>(); }
            return retailSalesInvoiceDetail;
        }

        public RetailSalesInvoiceDetail CreateObject(RetailSalesInvoiceDetail retailSalesInvoiceDetail)
        {
            retailSalesInvoiceDetail.IsDeleted = false;
            retailSalesInvoiceDetail.CreatedAt = DateTime.Now;
            return Create(retailSalesInvoiceDetail);
        }

        public RetailSalesInvoiceDetail UpdateObject(RetailSalesInvoiceDetail retailSalesInvoiceDetail)
        {
            retailSalesInvoiceDetail.UpdatedAt = DateTime.Now;
            Update(retailSalesInvoiceDetail);
            return retailSalesInvoiceDetail;
        }

        public RetailSalesInvoiceDetail ConfirmObject(RetailSalesInvoiceDetail retailSalesInvoiceDetail)
        {
            Update(retailSalesInvoiceDetail);
            return retailSalesInvoiceDetail;
        }

        public RetailSalesInvoiceDetail UnconfirmObject(RetailSalesInvoiceDetail retailSalesInvoiceDetail)
        {
            Update(retailSalesInvoiceDetail);
            return retailSalesInvoiceDetail;
        }

        public RetailSalesInvoiceDetail SoftDeleteObject(RetailSalesInvoiceDetail retailSalesInvoiceDetail)
        {
            retailSalesInvoiceDetail.IsDeleted = true;
            retailSalesInvoiceDetail.DeletedAt = DateTime.Now;
            Update(retailSalesInvoiceDetail);
            return retailSalesInvoiceDetail;
        }

        public bool DeleteObject(int Id)
        {
            RetailSalesInvoiceDetail retailSalesInvoiceDetail = Find(x => x.Id == Id);
            return (Delete(retailSalesInvoiceDetail) == 1) ? true : false;
        }
    }
}
