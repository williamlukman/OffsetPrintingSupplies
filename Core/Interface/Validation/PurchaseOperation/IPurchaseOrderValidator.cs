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
        PurchaseOrder VHasUniqueNomorSurat(PurchaseOrder purchaseOrder, IPurchaseOrderService _purchaseOrderService);
        PurchaseOrder VHasContact(PurchaseOrder purchaseOrder, IContactService _contactService);
        PurchaseOrder VHasPurchaseDate(PurchaseOrder purchaseOrder);
        PurchaseOrder VHasBeenConfirmed(PurchaseOrder purchaseOrder);
        PurchaseOrder VHasNotBeenConfirmed(PurchaseOrder purchaseOrder);
        PurchaseOrder VHasPurchaseOrderDetails(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService);
        PurchaseOrder VHasNoPurchaseOrderDetail(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService);
        PurchaseOrder VHasNoPurchaseReceival(PurchaseOrder purchaseOrder, IPurchaseReceivalService _purchaseReceivalService);
        PurchaseOrder VHasConfirmationDate(PurchaseOrder purchaseOrder);
        PurchaseOrder VCreateObject(PurchaseOrder purchaseOrder, IPurchaseOrderService _purchaseOrderService, IContactService _contactService);
        PurchaseOrder VUpdateObject(PurchaseOrder purchaseOrder, IPurchaseOrderService _purchaseOrderService, IContactService _contactService);
        PurchaseOrder VDeleteObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService);
        PurchaseOrder VConfirmObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService);
        PurchaseOrder VUnconfirmObject(PurchaseOrder purchaseOrder, IPurchaseReceivalService _purchaseReceivalService);
        bool ValidCreateObject(PurchaseOrder purchaseOrder, IPurchaseOrderService _purchaseOrderService, IContactService _contactService);
        bool ValidUpdateObject(PurchaseOrder purchaseOrder, IPurchaseOrderService _purchaseOrderService, IContactService _contactService);
        bool ValidDeleteObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService);
        bool ValidConfirmObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService);
        bool ValidUnconfirmObject(PurchaseOrder purchaseOrder, IPurchaseReceivalService _purchaseReceivalService);
        bool isValid(PurchaseOrder purchaseOrder);
        string PrintError(PurchaseOrder purchaseOrder);
    }
}
