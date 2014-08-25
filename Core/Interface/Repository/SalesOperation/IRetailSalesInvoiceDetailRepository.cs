using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;

namespace Core.Interface.Repository
{
    public interface IRetailSalesInvoiceDetailRepository : IRepository<RetailSalesInvoiceDetail>
    {
        IQueryable<RetailSalesInvoiceDetail> GetQueryable();
        IList<RetailSalesInvoiceDetail> GetAll();
        IList<RetailSalesInvoiceDetail> GetObjectsByRetailSalesInvoiceId(int RetailSalesInvoiceId);
        RetailSalesInvoiceDetail GetObjectById(int Id);
        RetailSalesInvoiceDetail ConfirmObject(RetailSalesInvoiceDetail retailSalesInvoiceDetail);
        RetailSalesInvoiceDetail UnconfirmObject(RetailSalesInvoiceDetail retailSalesInvoiceDetail);
        RetailSalesInvoiceDetail CreateObject(RetailSalesInvoiceDetail retailSalesInvoiceDetail);
        RetailSalesInvoiceDetail UpdateObject(RetailSalesInvoiceDetail retailSalesInvoiceDetail);
        RetailSalesInvoiceDetail SoftDeleteObject(RetailSalesInvoiceDetail retailSalesInvoiceDetail);
        bool DeleteObject(int Id);
        string SetObjectCode(string ParentCode);
    }
}
