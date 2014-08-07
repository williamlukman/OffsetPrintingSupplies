using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IPurchaseOrderValidator
    {
        PurchaseOrder VContact(PurchaseOrder purchaseOrder, IContactService _contactService);
        PurchaseOrder VPurchaseDate(PurchaseOrder purchaseOrder);
        PurchaseOrder VHasBeenConfirmed(PurchaseOrder purchaseOrder);
        PurchaseOrder VHasNotBeenConfirmed(PurchaseOrder purchaseOrder);
        PurchaseOrder VHasPurchaseOrderDetails(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService);
        PurchaseOrder VAllDetailsHaveBeenFinished(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService);
        PurchaseOrder VAllDetailsHaveNotBeenFinished(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService);
        PurchaseOrder VCreateObject(PurchaseOrder purchaseOrder, IContactService _contactService);
        PurchaseOrder VUpdateObject(PurchaseOrder purchaseOrder, IContactService _contactService);
        PurchaseOrder VDeleteObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService);
        PurchaseOrder VConfirmObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService);
        PurchaseOrder VUnconfirmObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService, IPurchaseReceivalDetailService _purchaseReceivalDetailService, IItemService _itemService);
        PurchaseOrder VCompleteObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService);
        bool ValidCreateObject(PurchaseOrder purchaseOrder, IContactService _contactService);
        bool ValidUpdateObject(PurchaseOrder purchaseOrder, IContactService _contactService);
        bool ValidDeleteObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService);
        bool ValidConfirmObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService);
        bool ValidUnconfirmObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService, IPurchaseReceivalDetailService _purchaseReceivalDetailService, IItemService _itemService);
        bool ValidCompleteObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService);
        bool isValid(PurchaseOrder purchaseOrder);
        string PrintError(PurchaseOrder purchaseOrder);
    }
}
