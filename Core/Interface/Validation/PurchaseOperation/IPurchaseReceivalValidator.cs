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
        PurchaseReceival VContact(PurchaseReceival purchaseReceival, IContactService _contactService);
        PurchaseReceival VReceivalDate(PurchaseReceival purchaseReceival);
        PurchaseReceival VHasNotBeenConfirmed(PurchaseReceival purchaseReceival);
        PurchaseReceival VHasBeenConfirmed(PurchaseReceival purchaseReceival);
        PurchaseReceival VHasPurchaseReceivalDetails(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        PurchaseReceival VAllDetailsHaveBeenFinished(PurchaseReceival purchaseOrder, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        PurchaseReceival VAllDetailsHaveNotBeenFinished(PurchaseReceival purchaseOrder, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        PurchaseReceival VCreateObject(PurchaseReceival purchaseReceival, IContactService _contactService);
        PurchaseReceival VUpdateObject(PurchaseReceival purchaseReceival, IContactService _contactService);
        PurchaseReceival VDeleteObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        PurchaseReceival VConfirmObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        PurchaseReceival VUnconfirmObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService, IItemService _itemService);
        PurchaseReceival VCompleteObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        bool ValidCreateObject(PurchaseReceival purchaseReceival, IContactService _contactService);
        bool ValidUpdateObject(PurchaseReceival purchaseReceival, IContactService _contactService);
        bool ValidDeleteObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        bool ValidConfirmObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        bool ValidUnconfirmObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService, IItemService _itemService);
        bool ValidCompleteObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        bool isValid(PurchaseReceival purchaseReceival);
        string PrintError(PurchaseReceival purchaseReceival);
    }
}
