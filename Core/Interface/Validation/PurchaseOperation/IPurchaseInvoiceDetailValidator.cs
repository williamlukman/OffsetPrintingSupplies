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
        PurchaseInvoiceDetail VHasPurchaseInvoice(PurchaseInvoiceDetail purchaseInvoiceDetail, IPurchaseInvoiceService _purchaseInvoiceService);
        PurchaseInvoiceDetail VHasPurchaseReceivalDetail(PurchaseInvoiceDetail purchaseInvoiceDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        PurchaseInvoiceDetail VPurchaseReceivalDetailAndPurchaseInvoiceMustHaveTheSamePurchaseReceival(PurchaseInvoiceDetail purchaseInvoiceDetail,
                                                         IPurchaseReceivalDetailService _purchaseReceivalDetailService, IPurchaseInvoiceService purchaseInvoiceService);
        PurchaseInvoiceDetail VIsUniquePurchaseReceivalDetail(PurchaseInvoiceDetail purchaseInvoiceDetail, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService,
                                                              IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        PurchaseInvoiceDetail VNonNegativeNorZeroQuantity(PurchaseInvoiceDetail purchaseInvoiceDetail);
        PurchaseInvoiceDetail VQuantityIsLessThanOrEqualPendingInvoiceQuantity(PurchaseInvoiceDetail purchaseInvoiceDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        PurchaseInvoiceDetail VHasNotBeenConfirmed(PurchaseInvoiceDetail purchaseInvoiceDetail);
        PurchaseInvoiceDetail VHasBeenConfirmed(PurchaseInvoiceDetail purchaseInvoiceDetail);
        PurchaseInvoiceDetail VCreateObject(PurchaseInvoiceDetail purchaseInvoiceDetail, IPurchaseInvoiceService _purchaseInvoiceService, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService,
                                            IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        PurchaseInvoiceDetail VUpdateObject(PurchaseInvoiceDetail purchaseInvoiceDetail, IPurchaseInvoiceService _purchaseInvoiceService, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService,
                                            IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        PurchaseInvoiceDetail VDeleteObject(PurchaseInvoiceDetail purchaseInvoiceDetail);
        PurchaseInvoiceDetail VHasConfirmationDate(PurchaseInvoiceDetail purchaseInvoiceDetail);
        PurchaseInvoiceDetail VConfirmObject(PurchaseInvoiceDetail purchaseInvoiceDetail, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService,
                                             IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        PurchaseInvoiceDetail VUnconfirmObject(PurchaseInvoiceDetail purchaseInvoiceDetail);
        bool ValidCreateObject(PurchaseInvoiceDetail purchaseInvoiceDetail, IPurchaseInvoiceService _purchaseInvoiceService, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        bool ValidUpdateObject(PurchaseInvoiceDetail purchaseInvoiceDetail, IPurchaseInvoiceService _purchaseInvoiceService, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        bool ValidDeleteObject(PurchaseInvoiceDetail purchaseInvoiceDetail);
        bool ValidConfirmObject(PurchaseInvoiceDetail purchaseInvoiceDetail, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        bool ValidUnconfirmObject(PurchaseInvoiceDetail purchaseInvoiceDetail);
        bool isValid(PurchaseInvoiceDetail purchaseInvoiceDetail);
        string PrintError(PurchaseInvoiceDetail purchaseInvoiceDetail);
    }
}
