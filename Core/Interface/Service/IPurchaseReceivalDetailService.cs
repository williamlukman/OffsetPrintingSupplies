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
    public interface IPurchaseReceivalDetailService
    {
        IPurchaseReceivalDetailValidator GetValidator();
        IList<PurchaseReceivalDetail> GetObjectsByPurchaseReceivalId(int purchaseReceivalId);
        PurchaseReceivalDetail GetObjectById(int Id);
        PurchaseReceivalDetail GetObjectByPurchaseOrderDetailId(int purchaseOrderDetailId);
        PurchaseReceivalDetail CreateObject(PurchaseReceivalDetail purchaseReceivalDetail,
                                            IPurchaseReceivalService _purchaseReceivalService, IPurchaseOrderDetailService _purchaseOrderDetailService,
                                            IPurchaseOrderService _purchaseOrderService, IItemService _itemService, ICustomerService _customerService);
        PurchaseReceivalDetail CreateObject(int purchaseReceivalId, int itemId, int quantity, int purchaseOrderDetailId,
                                            IPurchaseReceivalService _purchaseReceivalService, IPurchaseOrderDetailService _purchaseOrderDetailService,
                                            IPurchaseOrderService _purchaseOrderService, IItemService _itemService, ICustomerService _customerService);
        PurchaseReceivalDetail UpdateObject(PurchaseReceivalDetail purchaseReceivalDetail,
                                            IPurchaseReceivalService _purchaseReceivalService, IPurchaseOrderDetailService _purchaseOrderDetailService,
                                            IPurchaseOrderService _purchaseOrderService, IItemService _itemService, ICustomerService _customerService);
        PurchaseReceivalDetail SoftDeleteObject(PurchaseReceivalDetail purchaseReceivalDetail);
        bool DeleteObject(int Id);
        PurchaseReceivalDetail FinishObject(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalService _purchaseReceivalService, IPurchaseOrderDetailService _purchaseOrderDetailService,
                                            IStockMutationService _stockMutationService, IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService);
        PurchaseReceivalDetail UnfinishObject(PurchaseReceivalDetail purchaseReceivalDetail, IPurchaseReceivalService _purchaseReceivalService, IPurchaseOrderDetailService _purchaseOrderDetailService,
                                              IStockMutationService _stockMutationService, IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService);
    }
}