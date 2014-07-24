using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IPurchaseOrderValidator
    {
        PurchaseOrder VCustomer(PurchaseOrder purchaseOrder, ICustomerService _customerService);
        PurchaseOrder VPurchaseDate(PurchaseOrder purchaseOrder);
        PurchaseOrder VIsConfirmed(PurchaseOrder purchaseOrder);
        PurchaseOrder VHasPurchaseOrderDetails(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService);
        PurchaseOrder VCreateObject(PurchaseOrder purchaseOrder, ICustomerService _customerService);
        PurchaseOrder VUpdateObject(PurchaseOrder purchaseOrder, ICustomerService _customerService);
        PurchaseOrder VDeleteObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService);
        PurchaseOrder VConfirmObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService);
        PurchaseOrder VUnconfirmObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService, IPurchaseReceivalDetailService _purchaseReceivalDetailService, IItemService _itemService);
        PurchaseOrder VCompleteObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService);
        bool ValidCreateObject(PurchaseOrder purchaseOrder, ICustomerService _customerService);
        bool ValidUpdateObject(PurchaseOrder purchaseOrder, ICustomerService _customerService);
        bool ValidDeleteObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService);
        bool ValidConfirmObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService);
        bool ValidUnconfirmObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService, IPurchaseReceivalDetailService _purchaseReceivalDetailService, IItemService _itemService);
        bool ValidCompleteObject(PurchaseOrder purchaseOrder, IPurchaseOrderDetailService _purchaseOrderDetailService);
        bool isValid(PurchaseOrder purchaseOrder);
        string PrintError(PurchaseOrder purchaseOrder);
    }
}
