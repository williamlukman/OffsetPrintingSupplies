using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IPurchaseReceivalDetailService
    {
        IPurchaseReceivalDetailValidator GetValidator();
        IQueryable<PurchaseReceivalDetail> GetQueryable();
        IList<PurchaseReceivalDetail> GetAll();
        IList<PurchaseReceivalDetail> GetObjectsByPurchaseReceivalId(int purchaseReceivalId);
        PurchaseReceivalDetail GetObjectById(int Id);
        IList<PurchaseReceivalDetail> GetObjectsByPurchaseOrderDetailId(int purchaseOrderDetailId);
        PurchaseReceivalDetail CreateObject(PurchaseReceivalDetail purchaseReceivalDetail,
                                            IPurchaseReceivalService _purchaseReceivalService, IPurchaseOrderDetailService _purchaseOrderDetailService,
                                            IPurchaseOrderService _purchaseOrderService, IItemService _itemService);
        PurchaseReceivalDetail CreateObject(int purchaseReceivalId, int itemId, int quantity, int purchaseOrderDetailId,
                                            IPurchaseReceivalService _purchaseReceivalService, IPurchaseOrderDetailService _purchaseOrderDetailService,
                                            IPurchaseOrderService _purchaseOrderService, IItemService _itemService);
        PurchaseReceivalDetail UpdateObject(PurchaseReceivalDetail purchaseReceivalDetail,
                                            IPurchaseReceivalService _purchaseReceivalService, IPurchaseOrderDetailService _purchaseOrderDetailService,
                                            IPurchaseOrderService _purchaseOrderService, IItemService _itemService);
        PurchaseReceivalDetail SoftDeleteObject(PurchaseReceivalDetail purchaseReceivalDetail);
        bool DeleteObject(int Id);
        PurchaseReceivalDetail ConfirmObject(PurchaseReceivalDetail purchaseReceivalDetail, DateTime ConfirmationDate, IPurchaseReceivalService _purchaseReceivalService,
                                             IPurchaseOrderDetailService _purchaseOrderDetailService, IStockMutationService _stockMutationService,
                                             IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService);
        PurchaseReceivalDetail UnconfirmObject(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalService _purchaseReceivalService,
                                               IPurchaseOrderService _purchaseOrderService, IPurchaseOrderDetailService _purchaseOrderDetailService,
                                               IPurchaseInvoiceDetailService _purchaseInvoiceDetailService, IStockMutationService _stockMutationService,
                                               IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService);
        PurchaseReceivalDetail InvoiceObject(PurchaseReceivalDetail purchaseReceivalDetail, int Quantity);
        PurchaseReceivalDetail UndoInvoiceObject(PurchaseReceivalDetail purchaseReceivalDetail, int Quantity, IPurchaseReceivalService _purchaseReceivalService);
    }
}