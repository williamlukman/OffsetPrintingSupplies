using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IPurchaseInvoiceDetailService
    {
        IPurchaseInvoiceDetailValidator GetValidator();
        IList<PurchaseInvoiceDetail> GetObjectsByPurchaseInvoiceId(int purchaseInvoiceId);
        IList<PurchaseInvoiceDetail> GetObjectsByPurchaseReceivalDetailId(int purchaseReceivalDetailId);
        PurchaseInvoiceDetail GetObjectById(int Id);
        PurchaseInvoiceDetail CreateObject(PurchaseInvoiceDetail purchaseInvoiceDetail, IPurchaseInvoiceService _purchaseInvoiceService,
                                           IPurchaseOrderDetailService _purchaseOrderDetailService, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        PurchaseInvoiceDetail CreateObject(int purchaseInvoiceId, int purchaseReceivalDetailId, int quantity, decimal amount,
                                           IPurchaseInvoiceService _purchaseInvoiceService, IPurchaseOrderDetailService _purchaseOrderDetailService,
                                           IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        PurchaseInvoiceDetail UpdateObject(PurchaseInvoiceDetail purchaseInvoiceDetail, IPurchaseInvoiceService _purchaseInvoiceService,
                                           IPurchaseOrderDetailService _purchaseOrderDetailService, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        PurchaseInvoiceDetail SoftDeleteObject(PurchaseInvoiceDetail purchaseInvoiceDetail, IPurchaseInvoiceService _purchaseInvoiceService);
        bool DeleteObject(int Id);
        PurchaseInvoiceDetail ConfirmObject(PurchaseInvoiceDetail purchaseInvoiceDetail, DateTime ConfirmationDate, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        PurchaseInvoiceDetail UnconfirmObject(PurchaseInvoiceDetail purchaseInvoiceDetail, IPurchaseReceivalService _purchaseReceivalService, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
    }
}