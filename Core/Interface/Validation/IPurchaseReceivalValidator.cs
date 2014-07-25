using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IPurchaseReceivalValidator
    {
        PurchaseReceival VCustomer(PurchaseReceival purchaseReceival, ICustomerService _customerService);
        PurchaseReceival VReceivalDate(PurchaseReceival purchaseReceival);
        PurchaseReceival VHasNotBeenConfirmed(PurchaseReceival purchaseReceival);
        PurchaseReceival VHasBeenConfirmed(PurchaseReceival purchaseReceival);
        PurchaseReceival VHasPurchaseReceivalDetails(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        PurchaseReceival VAllDetailsHaveBeenFinished(PurchaseReceival purchaseOrder, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        PurchaseReceival VCreateObject(PurchaseReceival purchaseReceival, ICustomerService _customerService);
        PurchaseReceival VUpdateObject(PurchaseReceival purchaseReceival, ICustomerService _customerService);
        PurchaseReceival VDeleteObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        PurchaseReceival VConfirmObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        PurchaseReceival VUnconfirmObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService, IItemService _itemService);
        PurchaseReceival VCompleteObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        bool ValidCreateObject(PurchaseReceival purchaseReceival, ICustomerService _customerService);
        bool ValidUpdateObject(PurchaseReceival purchaseReceival, ICustomerService _customerService);
        bool ValidDeleteObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        bool ValidConfirmObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        bool ValidUnconfirmObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService, IItemService _itemService);
        bool ValidCompleteObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        bool isValid(PurchaseReceival purchaseReceival);
        string PrintError(PurchaseReceival purchaseReceival);
    }
}
