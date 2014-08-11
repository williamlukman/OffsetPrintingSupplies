using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IPurchaseReceivalService
    {
        IPurchaseReceivalValidator GetValidator();
        IList<PurchaseReceival> GetAll();
        PurchaseReceival GetObjectById(int Id);
        IList<PurchaseReceival> GetObjectsByPurchaseOrderId(int purchaseOrderId);
        PurchaseReceival CreateObject(PurchaseReceival purchaseReceival, IPurchaseOrderService _purchaseOrderService);
        PurchaseReceival CreateObject(int warehouseId, int purchaseOrderId, DateTime ReceivalDate, IPurchaseOrderService _purchaseOrderService);
        PurchaseReceival UpdateObject(PurchaseReceival purchaseReceival, IPurchaseOrderService _purchaseOrderService);
        PurchaseReceival SoftDeleteObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        bool DeleteObject(int Id);
        PurchaseReceival ConfirmObject(PurchaseReceival purchaseReceival, DateTime ConfirmationDate, IPurchaseReceivalDetailService _purchaseReceivalDetailService,
                                       IPurchaseOrderDetailService _purchaseOrderDetailService, IStockMutationService _stockMutationService, IItemService _itemService,
                                       IBarringService _barringService, IWarehouseItemService _warehouseItemService);
        PurchaseReceival UnconfirmObject(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService,
                                         IPurchaseInvoiceService _purchaseInvoiceService, IPurchaseInvoiceDetailService _purchaseInvoiceDetailService,
                                         IPurchaseOrderDetailService _purchaseOrderDetailService, IStockMutationService _stockMutationService, IItemService _itemService,
                                         IBarringService _barringService, IWarehouseItemService _warehouseItemService);
        PurchaseReceival CheckAndSetInvoiceComplete(PurchaseReceival purchaseReceival, IPurchaseReceivalDetailService _purchaseReceivalDetailService);
        PurchaseReceival UnsetInvoiceComplete(PurchaseReceival purchaseReceival);
    }
}