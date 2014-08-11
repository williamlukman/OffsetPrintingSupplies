using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IPurchaseReceivalValidator
    {
        PurchaseReceival VHasPurchaseOrder(PurchaseReceival purchaseReceival, IPurchaseOrderService _purchaseOrderService);
        PurchaseReceival VHasReceivalDate(PurchaseReceival purchaseReceival);
        PurchaseReceival VHasNotBeenConfirmed(PurchaseReceival purchaseReceival);
        PurchaseReceival VHasBeenConfirmed(PurchaseReceival purchaseReceival);
        PurchaseReceival VPurchaseOrderHasBeenConfirmed(PurchaseReceival purchaseReceival, IPurchaseOrderService _purchaseOrderService);
        PurchaseReceival VHasPurchaseReceivalDetails(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        PurchaseReceival VHasNoPurchaseReceivalDetail(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        PurchaseReceival VHasConfirmationDate(PurchaseReceival purchaseReceival);
        PurchaseReceival VHasNoPurchaseInvoice(PurchaseReceival purchaseReceival, IPurchaseInvoiceService _purchaseInvoiceService);
        PurchaseReceival VCreateObject(PurchaseReceival purchaseReceival, IPurchaseOrderService _purchaseOrderService);
        PurchaseReceival VUpdateObject(PurchaseReceival purchaseReceival, IPurchaseOrderService _purchaseOrderService);
        PurchaseReceival VDeleteObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        PurchaseReceival VConfirmObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        PurchaseReceival VUnconfirmObject(PurchaseReceival purchaseReceival, IPurchaseInvoiceService _purchaseInvoiceService);
        bool ValidCreateObject(PurchaseReceival purchaseReceival, IPurchaseOrderService _purchaseOrderService);
        bool ValidUpdateObject(PurchaseReceival purchaseReceival, IPurchaseOrderService _purchaseOrderService);
        bool ValidDeleteObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        bool ValidConfirmObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        bool ValidUnconfirmObject(PurchaseReceival purchaseReceival, IPurchaseInvoiceService _purchaseInvoiceService);
        bool isValid(PurchaseReceival purchaseReceival);
        string PrintError(PurchaseReceival purchaseReceival);
    }
}
