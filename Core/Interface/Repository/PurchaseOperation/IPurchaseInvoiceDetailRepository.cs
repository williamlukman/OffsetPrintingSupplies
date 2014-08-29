using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IPurchaseInvoiceDetailRepository : IRepository<PurchaseInvoiceDetail>
    {
        IQueryable<PurchaseInvoiceDetail> GetQueryable();
        IList<PurchaseInvoiceDetail> GetAll();
        IList<PurchaseInvoiceDetail> GetAllByMonthCreated();
        IQueryable<PurchaseInvoiceDetail> GetQueryableObjectsByPurchaseInvoiceId(int purchaseInvoiceId);
        IList<PurchaseInvoiceDetail> GetObjectsByPurchaseInvoiceId(int purchaseInvoiceId);
        IQueryable<PurchaseInvoiceDetail> GetQueryableObjectsByPurchaseReceivalDetailId(int purchaseReceivalDetailId);
        IList<PurchaseInvoiceDetail> GetObjectsByPurchaseReceivalDetailId(int purchaseReceivalDetailId);
        PurchaseInvoiceDetail GetObjectById(int Id);
        PurchaseInvoiceDetail CreateObject(PurchaseInvoiceDetail purchaseInvoiceDetail);
        PurchaseInvoiceDetail UpdateObject(PurchaseInvoiceDetail purchaseInvoiceDetail);
        PurchaseInvoiceDetail SoftDeleteObject(PurchaseInvoiceDetail purchaseInvoiceDetail);
        bool DeleteObject(int Id);
        PurchaseInvoiceDetail ConfirmObject(PurchaseInvoiceDetail purchaseInvoiceDetail);
        PurchaseInvoiceDetail UnconfirmObject(PurchaseInvoiceDetail purchaseInvoiceDetail);
        string SetObjectCode(string ParentCode);
    }
}