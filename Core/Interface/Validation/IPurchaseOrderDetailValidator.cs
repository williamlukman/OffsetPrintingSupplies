using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IPurchaseOrderDetailValidator
    {
        PurchaseOrderDetail VHasPurchaseOrder(PurchaseOrderDetail purchaseOrderDetail, IPurchaseOrderService _purchaseOrderService);
        PurchaseOrderDetail VHasItem(PurchaseOrderDetail purchaseOrderDetail, IItemService _itemService);
        PurchaseOrderDetail VQuantity(PurchaseOrderDetail purchaseOrderDetail);
        PurchaseOrderDetail VPrice(PurchaseOrderDetail purchaseOrderDetail);
        PurchaseOrderDetail VUniquePurchaseOrderDetail(PurchaseOrderDetail purchaseOrderDetail, IPurchaseOrderDetailService _purchaseOrderDetailService, IItemService _itemService);
        PurchaseOrderDetail VIsConfirmed(PurchaseOrderDetail purchaseOrderDetail);
        PurchaseOrderDetail VHasItemPendingReceival(PurchaseOrderDetail purchaseOrderDetail, IItemService _itemService);
        PurchaseOrderDetail VConfirmedPurchaseReceival(PurchaseOrderDetail purchaseOrderDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        PurchaseOrderDetail VCreateObject(PurchaseOrderDetail purchaseOrderDetail, IPurchaseOrderDetailService _purchaseOrderDetailService, IPurchaseOrderService _purchaseOrderService, IItemService _itemService);
        PurchaseOrderDetail VUpdateObject(PurchaseOrderDetail purchaseOrderDetail, IPurchaseOrderDetailService _purchaseOrderDetailService, IPurchaseOrderService _purchaseOrderService, IItemService _itemService);
        PurchaseOrderDetail VDeleteObject(PurchaseOrderDetail purchaseOrderDetail);
        PurchaseOrderDetail VFinishObject(PurchaseOrderDetail purchaseOrderDetail);
        PurchaseOrderDetail VUnfinishObject(PurchaseOrderDetail purchaseOrderDetail, IPurchaseOrderDetailService _purchaseOrderDetailService, IPurchaseReceivalDetailService _purchaseReceivalDetailService, IItemService _itemService);
        PurchaseOrderDetail VReceiveObject(PurchaseOrderDetail purchaseOrderDetail);
        bool ValidCreateObject(PurchaseOrderDetail purchaseOrderDetail, IPurchaseOrderDetailService _purchaseOrderDetailService, IPurchaseOrderService _purchaseOrderService, IItemService _itemService);
        bool ValidUpdateObject(PurchaseOrderDetail purchaseOrderDetail, IPurchaseOrderDetailService _purchaseOrderDetailService, IPurchaseOrderService _purchaseOrderService, IItemService _itemService);
        bool ValidDeleteObject(PurchaseOrderDetail purchaseOrderDetail);
        bool ValidFinishObject(PurchaseOrderDetail purchaseOrderDetail);
        bool ValidUnfinishObject(PurchaseOrderDetail purchaseOrderDetail, IPurchaseOrderDetailService _purchaseOrderDetailService, IPurchaseReceivalDetailService _purchaseReceivalDetailService, IItemService _itemService);
        bool ValidReceiveObject(PurchaseOrderDetail purchaseOrderDetail);
        bool isValid(PurchaseOrderDetail purchaseOrderDetail);
        string PrintError(PurchaseOrderDetail purchaseOrderDetail);
    }
}
