using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IPurchaseOrderDetailService
    {
        IPurchaseOrderDetailValidator GetValidator();
        IQueryable<PurchaseOrderDetail> GetQueryable();
        IList<PurchaseOrderDetail> GetAll();
        IList<PurchaseOrderDetail> GetObjectsByPurchaseOrderId(int purchaseOrderId);
        IList<PurchaseOrderDetail> GetObjectsByItemId(int itemId);
        PurchaseOrderDetail GetObjectById(int Id);
        PurchaseOrderDetail CreateObject(PurchaseOrderDetail purchaseOrderDetail, IPurchaseOrderService _purchaseOrderService, IItemService _itemService);
        PurchaseOrderDetail UpdateObject(PurchaseOrderDetail purchaseOrderDetail, IPurchaseOrderService _purchaseOrderService, IItemService _itemService);
        PurchaseOrderDetail SoftDeleteObject(PurchaseOrderDetail purchaseOrderDetail);
        bool DeleteObject(int Id);
        PurchaseOrderDetail ConfirmObject(PurchaseOrderDetail purchaseOrderDetail, DateTime ConfirmationDate, IStockMutationService _stockMutationService,
                                         IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService);
        PurchaseOrderDetail UnconfirmObject(PurchaseOrderDetail purchaseOrderDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService,
                                           IStockMutationService _stockMutationService, IItemService _itemService, IBlanketService _blanketService,
                                           IWarehouseItemService _warehouseItemService);
        PurchaseOrderDetail SetReceivalComplete(PurchaseOrderDetail purchaseOrderDetail, decimal Quantity);
        PurchaseOrderDetail UnsetReceivalComplete(PurchaseOrderDetail purchaseOrderDetail, decimal Quantity, IPurchaseOrderService _purchaseOrderService);
   }
}