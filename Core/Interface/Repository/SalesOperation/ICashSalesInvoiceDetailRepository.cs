using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;

namespace Core.Interface.Repository
{
    public interface ICashSalesInvoiceDetailRepository : IRepository<CashSalesInvoiceDetail>
    {
        IQueryable<CashSalesInvoiceDetail> GetQueryable();
        IList<CashSalesInvoiceDetail> GetAll();
        IQueryable<CashSalesInvoiceDetail> GetQueryableObjectsByCashSalesInvoiceId(int CashSalesInvoiceId);
        IList<CashSalesInvoiceDetail> GetObjectsByCashSalesInvoiceId(int CashSalesInvoiceId);
        CashSalesInvoiceDetail GetObjectById(int Id);
        CashSalesInvoiceDetail ConfirmObject(CashSalesInvoiceDetail cashSalesInvoiceDetail);
        CashSalesInvoiceDetail UnconfirmObject(CashSalesInvoiceDetail cashSalesInvoiceDetail);
        CashSalesInvoiceDetail CreateObject(CashSalesInvoiceDetail cashSalesInvoiceDetail);
        CashSalesInvoiceDetail UpdateObject(CashSalesInvoiceDetail cashSalesInvoiceDetail);
        CashSalesInvoiceDetail SoftDeleteObject(CashSalesInvoiceDetail cashSalesInvoiceDetail);
        bool DeleteObject(int Id);
        string SetObjectCode(string ParentCode);
    }
}
