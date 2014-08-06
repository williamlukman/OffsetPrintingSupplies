using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IPurchaseInvoiceDetailValidator
    {
        PurchaseInvoiceDetail VHasPurchaseReceivalDetail(PurchaseInvoiceDetail purchaseInvoiceDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        PurchaseInvoiceDetail VQuantity(PurchaseInvoiceDetail purchaseInvoiceDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        PurchaseInvoiceDetail VPrice(PurchaseInvoiceDetail purchaseInvoiceDetail);
        PurchaseInvoiceDetail VIsUniquePurchaseReceivalDetail(PurchaseInvoiceDetail purchaseInvoiceDetail, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService,
                                                              IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        PurchaseInvoiceDetail VHasPayment(PurchaseInvoiceDetail purchaseInvoiceDetail, IPaymentVoucherDetailService _paymentVoucherDetailService, IPayableService _payableService);
        PurchaseInvoiceDetail VHasNotBeenConfirmed(PurchaseInvoiceDetail purchaseInvoiceDetail);
        PurchaseInvoiceDetail VHasBeenConfirmed(PurchaseInvoiceDetail purchaseInvoiceDetail);
        PurchaseInvoiceDetail VCreateObject(PurchaseInvoiceDetail purchaseInvoiceDetail, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService,
                                            IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        PurchaseInvoiceDetail VUpdateObject(PurchaseInvoiceDetail purchaseInvoiceDetail, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService,
                                            IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        PurchaseInvoiceDetail VDeleteObject(PurchaseInvoiceDetail purchaseInvoiceDetail);
        PurchaseInvoiceDetail VConfirmObject(PurchaseInvoiceDetail purchaseInvoiceDetail, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService,
                                             IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        PurchaseInvoiceDetail VUnconfirmObject(PurchaseInvoiceDetail purchaseInvoiceDetail, IPaymentVoucherDetailService _paymentVoucherDetailService, IPayableService _payableService);
        bool ValidCreateObject(PurchaseInvoiceDetail purchaseInvoiceDetail, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        bool ValidUpdateObject(PurchaseInvoiceDetail purchaseInvoiceDetail, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        bool ValidDeleteObject(PurchaseInvoiceDetail purchaseInvoiceDetail);
        bool ValidConfirmObject(PurchaseInvoiceDetail purchaseInvoiceDetail, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        bool ValidUnconfirmObject(PurchaseInvoiceDetail purchaseInvoiceDetail, IPaymentVoucherDetailService _paymentVoucherDetailService, IPayableService _payableService);
        bool isValid(PurchaseInvoiceDetail purchaseInvoiceDetail);
        string PrintError(PurchaseInvoiceDetail purchaseInvoiceDetail);
    }
}
