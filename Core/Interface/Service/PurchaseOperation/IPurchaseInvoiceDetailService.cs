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
        PurchaseInvoiceDetail GetObjectById(int Id);
        PurchaseReceivalDetail GetPurchaseReceivalDetailByPurchaseReceivalDetailId(int purchaseReceivalDetailId);
        PurchaseInvoiceDetail CreateObject(PurchaseInvoiceDetail purchaseInvoiceDetail, IPurchaseInvoiceService _purchaseInvoiceService,
                                           IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        PurchaseInvoiceDetail CreateObject(int purchaseInvoiceId, int purchaseReceivalDetailId, int quantity, decimal amount,
                                           IPurchaseInvoiceService _purchaseInvoiceService, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        PurchaseInvoiceDetail UpdateObject(PurchaseInvoiceDetail purchaseInvoiceDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        PurchaseInvoiceDetail SoftDeleteObject(PurchaseInvoiceDetail purchaseInvoiceDetail);
        bool DeleteObject(int Id);
        PurchaseInvoiceDetail ConfirmObject(PurchaseInvoiceDetail purchaseInvoiceDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        PurchaseInvoiceDetail UnconfirmObject(PurchaseInvoiceDetail purchaseInvoiceDetail, IPaymentVoucherDetailService _paymentVoucherDetailService, IPayableService _payableService);
    }
}