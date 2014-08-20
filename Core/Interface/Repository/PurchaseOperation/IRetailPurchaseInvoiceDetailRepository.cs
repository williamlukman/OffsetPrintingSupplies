using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;

namespace Core.Interface.Repository
{
    public interface IRetailPurchaseInvoiceDetailRepository : IRepository<RetailPurchaseInvoiceDetail>
    {
        IQueryable<RetailPurchaseInvoiceDetail> GetQueryable();
        IList<RetailPurchaseInvoiceDetail> GetAll();
        IList<RetailPurchaseInvoiceDetail> GetObjectsByRetailPurchaseInvoiceId(int RetailPurchaseInvoiceId);
        RetailPurchaseInvoiceDetail GetObjectById(int Id);
        RetailPurchaseInvoiceDetail ConfirmObject(RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail);
        RetailPurchaseInvoiceDetail UnconfirmObject(RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail);
        RetailPurchaseInvoiceDetail CreateObject(RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail);
        RetailPurchaseInvoiceDetail UpdateObject(RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail);
        RetailPurchaseInvoiceDetail SoftDeleteObject(RetailPurchaseInvoiceDetail retailPurchaseInvoiceDetail);
        string SetObjectCode(string ParentCode);
        bool DeleteObject(int Id);
    }
}
