using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IPurchaseOrderDetailValidator
    {
        PurchaseOrderDetail VHasPurchaseOrder(PurchaseOrderDetail purchaseOrderDetail, IPurchaseOrderService _purchaseOrderService);
        PurchaseOrderDetail VHasItem(PurchaseOrderDetail purchaseOrderDetail, IItemService _itemService);
        PurchaseOrderDetail VNonZeroNorNegativeQuantity(PurchaseOrderDetail purchaseOrderDetail);
        PurchaseOrderDetail VNonNegativePrice(PurchaseOrderDetail purchaseOrderDetail);
        PurchaseOrderDetail VUniquePurchaseOrderDetail(PurchaseOrderDetail purchaseOrderDetail, IPurchaseOrderDetailService _purchaseOrderDetailService, IItemService _itemService);
        PurchaseOrderDetail VHasBeenConfirmed(PurchaseOrderDetail purchaseOrderDetail);
        PurchaseOrderDetail VHasNotBeenConfirmed(PurchaseOrderDetail purchaseOrderDetail);
        PurchaseOrderDetail VHasItemPendingReceival(PurchaseOrderDetail purchaseOrderDetail, IItemService _itemService);
        PurchaseOrderDetail VHasNoPurchaseReceivalDetail(PurchaseOrderDetail purchaseOrderDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        PurchaseOrderDetail VHasConfirmationDate(PurchaseOrderDetail purchaseOrderDetail);
        PurchaseOrderDetail VCreateObject(PurchaseOrderDetail purchaseOrderDetail, IPurchaseOrderDetailService _purchaseOrderDetailService, IPurchaseOrderService _purchaseOrderService, IItemService _itemService);
        PurchaseOrderDetail VUpdateObject(PurchaseOrderDetail purchaseOrderDetail, IPurchaseOrderDetailService _purchaseOrderDetailService, IPurchaseOrderService _purchaseOrderService, IItemService _itemService);
        PurchaseOrderDetail VDeleteObject(PurchaseOrderDetail purchaseOrderDetail);
        PurchaseOrderDetail VConfirmObject(PurchaseOrderDetail purchaseOrderDetail);
        PurchaseOrderDetail VUnconfirmObject(PurchaseOrderDetail purchaseOrderDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService, IItemService _itemService);
        bool ValidCreateObject(PurchaseOrderDetail purchaseOrderDetail, IPurchaseOrderDetailService _purchaseOrderDetailService, IPurchaseOrderService _purchaseOrderService, IItemService _itemService);
        bool ValidUpdateObject(PurchaseOrderDetail purchaseOrderDetail, IPurchaseOrderDetailService _purchaseOrderDetailService, IPurchaseOrderService _purchaseOrderService, IItemService _itemService);
        bool ValidDeleteObject(PurchaseOrderDetail purchaseOrderDetail);
        bool ValidConfirmObject(PurchaseOrderDetail purchaseOrderDetail);
        bool ValidUnconfirmObject(PurchaseOrderDetail purchaseOrderDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService, IItemService _itemService);
        bool isValid(PurchaseOrderDetail purchaseOrderDetail);
        string PrintError(PurchaseOrderDetail purchaseOrderDetail);
    }
}
