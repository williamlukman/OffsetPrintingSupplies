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
        PurchaseReceivalDetail VHasItem(PurchaseReceivalDetail purchaseReceivalDetail, IItemService _itemService);
        PurchaseReceivalDetail VContact(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalService _purchaseReceivalService, IPurchaseOrderService _purchaseOrderService,
                                        IPurchaseOrderDetailService _purchaseOrderDetailService, IContactService _contactService);
        PurchaseReceivalDetail VQuantityCreate(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseOrderDetailService _purchaseOrderDetailService);
        PurchaseReceivalDetail VQuantityUpdate(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseOrderDetailService _purchaseOrderDetailService);
        PurchaseReceivalDetail VHasPurchaseOrderDetail(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseOrderDetailService _purchaseOrderDetailService);
        PurchaseReceivalDetail VUniquePurchaseOrderDetail(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService, IItemService _itemService);
        PurchaseReceivalDetail VHasBeenFinished(PurchaseReceivalDetail purchaseReceivalDetail);
        PurchaseReceivalDetail VHasNotBeenFinished(PurchaseReceivalDetail purchaseReceivalDetail);
        PurchaseReceivalDetail VPurchaseReceivalHasNotBeenCompleted(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalService _purchaseReceivalService);
        PurchaseReceivalDetail VHasItemQuantity(PurchaseReceivalDetail purchaseReceivalDetail, IItemService _itemService);
        PurchaseReceivalDetail VCreateObject(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService,
                                             IPurchaseReceivalService _purchaseReceivalService, IPurchaseOrderDetailService _purchaseOrderDetailService,
                                             IPurchaseOrderService _purchaseOrderService, IItemService _itemService, IContactService _contactService);
        PurchaseReceivalDetail VUpdateObject(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService,
                                             IPurchaseReceivalService _purchaseReceivalService, IPurchaseOrderDetailService _purchaseOrderDetailService,
                                             IPurchaseOrderService _purchaseOrderService, IItemService _itemService, IContactService _contactService);
        PurchaseReceivalDetail VDeleteObject(PurchaseReceivalDetail purchaseReceivalDetail);
        PurchaseReceivalDetail VFinishObject(PurchaseReceivalDetail purchaseReceivalDetail);
        PurchaseReceivalDetail VUnfinishObject(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalService _purchaseReceivalService,
                                               IPurchaseReceivalDetailService _purchaseReceivalDetailService, IItemService _itemService);
        bool ValidCreateObject(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService,
                               IPurchaseReceivalService _purchaseReceivalService, IPurchaseOrderDetailService _purchaseOrderDetailService,
                               IPurchaseOrderService _purchaseOrderService, IItemService _itemService, IContactService _contactService);
        bool ValidUpdateObject(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService,
                               IPurchaseReceivalService _purchaseReceivalService, IPurchaseOrderDetailService _purchaseOrderDetailService,
                               IPurchaseOrderService _purchaseOrderService, IItemService _itemService, IContactService _contactService);
        bool ValidDeleteObject(PurchaseReceivalDetail purchaseReceivalDetail);
        bool ValidFinishObject(PurchaseReceivalDetail purchaseReceivalDetail);
        bool ValidUnfinishObject(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalService _purchaseReceivalService,
                                 IPurchaseReceivalDetailService _purchaseReceivalDetailService, IItemService _itemService);
        bool isValid(PurchaseReceivalDetail purchaseReceivalDetail);
        string PrintError(PurchaseReceivalDetail purchaseReceivalDetail);
    }
}
