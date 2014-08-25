using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IPurchaseReceivalDetailValidator
    {
        PurchaseReceivalDetail VHasPurchaseReceival(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalService _purchaseReceivalService);
        PurchaseReceivalDetail VPurchaseReceivalHasNotBeenConfirmed(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalService _purchaseReceivalService);
        PurchaseReceivalDetail VHasItem(PurchaseReceivalDetail purchaseReceivalDetail, IItemService _itemService);
        PurchaseReceivalDetail VNonNegativeQuantity(PurchaseReceivalDetail purchaseReceivalDetail);
        PurchaseReceivalDetail VHasPurchaseOrderDetail(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseOrderDetailService _purchaseOrderDetailService);
        PurchaseReceivalDetail VPurchaseReceivalAndPurchaseOrderDetailHaveTheSamePurchaseOrder(PurchaseReceivalDetail purchaseReceivalDetail,
                                                       IPurchaseReceivalService _purchaseReceivalService, IPurchaseOrderDetailService _purchaseOrderDetailService);
        PurchaseReceivalDetail VQuantityOfPurchaseReceivalDetailsIsLessThanOrEqualPurchaseOrderDetail(PurchaseReceivalDetail purchaseReceivalDetail,
                                                       IPurchaseReceivalDetailService _purchaseReceivalDetailService, IPurchaseOrderDetailService _purchaseOrderDetailService, bool CaseCreate);
        PurchaseReceivalDetail VUniquePurchaseOrderDetail(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService, IItemService _itemService);
        PurchaseReceivalDetail VPurchaseOrderDetailHasBeenConfirmed(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseOrderDetailService _purchaseOrderDetailService);
        PurchaseReceivalDetail VHasBeenConfirmed(PurchaseReceivalDetail purchaseReceivalDetail);
        PurchaseReceivalDetail VHasNotBeenConfirmed(PurchaseReceivalDetail purchaseReceivalDetail);
        PurchaseReceivalDetail VHasItemQuantity(PurchaseReceivalDetail purchaseReceivalDetail, IItemService _itemService);
        PurchaseReceivalDetail VHasConfirmationDate(PurchaseReceivalDetail purchaseReceivalDetail);
        PurchaseReceivalDetail VHasNoPurchaseInvoiceDetail(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService);
        PurchaseReceivalDetail VCreateObject(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService,
                                             IPurchaseReceivalService _purchaseReceivalService, IPurchaseOrderDetailService _purchaseOrderDetailService, IItemService _itemService);
        PurchaseReceivalDetail VUpdateObject(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService,
                                             IPurchaseReceivalService _purchaseReceivalService, IPurchaseOrderDetailService _purchaseOrderDetailService, IItemService _itemService);
        PurchaseReceivalDetail VDeleteObject(PurchaseReceivalDetail purchaseReceivalDetail);
        PurchaseReceivalDetail VConfirmObject(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService,
                                              IPurchaseOrderDetailService _purchaseOrderDetailService);
        PurchaseReceivalDetail VUnconfirmObject(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService, IItemService _itemService);
        bool ValidCreateObject(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService,
                               IPurchaseReceivalService _purchaseReceivalService, IPurchaseOrderDetailService _purchaseOrderDetailService, IItemService _itemService);
        bool ValidUpdateObject(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService,
                               IPurchaseReceivalService _purchaseReceivalService, IPurchaseOrderDetailService _purchaseOrderDetailService, IItemService _itemService);
        bool ValidDeleteObject(PurchaseReceivalDetail purchaseReceivalDetail);
        bool ValidConfirmObject(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService,
                                 IPurchaseOrderDetailService _purchaseOrderDetailService);
        bool ValidUnconfirmObject(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService, IItemService _itemService);
        bool isValid(PurchaseReceivalDetail purchaseReceivalDetail);
        string PrintError(PurchaseReceivalDetail purchaseReceivalDetail);
    }
}
