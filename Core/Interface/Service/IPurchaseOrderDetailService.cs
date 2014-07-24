using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Service
{
    public interface IPurchaseOrderDetailService
    {
        IPurchaseOrderDetailValidator GetValidator();
        IList<PurchaseOrderDetail> GetObjectsByPurchaseOrderId(int purchaseOrderId);
        PurchaseOrderDetail GetObjectById(int Id);
        PurchaseOrderDetail CreateObject(PurchaseOrderDetail purchaseOrderDetail, IPurchaseOrderService _purchaseOrderService, IItemService _itemService);
        PurchaseOrderDetail CreateObject(int purchaseOrderId, int itemId, int quantity, decimal Price, IPurchaseOrderService _purchaseOrderService, IItemService _itemService);
        PurchaseOrderDetail UpdateObject(PurchaseOrderDetail purchaseOrderDetail, IPurchaseOrderService _purchaseOrderService, IItemService _itemService);
        PurchaseOrderDetail SoftDeleteObject(PurchaseOrderDetail purchaseOrderDetail);
        bool DeleteObject(int Id);
        PurchaseOrderDetail FinishObject(PurchaseOrderDetail purchaseOrderDetail, IStockMutationService _stockMutationService, IItemService _itemService);
        PurchaseOrderDetail UnfinishObject(PurchaseOrderDetail purchaseOrderDetail, IPurchaseReceivalDetailService _purchaseReceivalDetailService, IStockMutationService _stockMutationService, IItemService _itemService);
        PurchaseOrderDetail ReceiveObject(PurchaseOrderDetail purchaseOrderDetail);
    }
}