using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IVirtualOrderDetailService
    {
        IVirtualOrderDetailValidator GetValidator();
        IQueryable<VirtualOrderDetail> GetQueryable();
        IList<VirtualOrderDetail> GetAll();
        IList<VirtualOrderDetail> GetObjectsByVirtualOrderId(int virtualOrderId);
        IList<VirtualOrderDetail> GetObjectsByItemId(int itemId);
        VirtualOrderDetail GetObjectById(int Id);
        VirtualOrderDetail CreateObject(VirtualOrderDetail virtualOrderDetail, IVirtualOrderService _virtualOrderService, IItemService _itemService);
        VirtualOrderDetail UpdateObject(VirtualOrderDetail virtualOrderDetail, IVirtualOrderService _virtualOrderService, IItemService _itemService);
        VirtualOrderDetail SoftDeleteObject(VirtualOrderDetail virtualOrderDetail);
        bool DeleteObject(int Id);
        VirtualOrderDetail ConfirmObject(VirtualOrderDetail virtualOrderDetail, DateTime ConfirmationDate, IStockMutationService _stockMutationService,
                                      IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService);
        VirtualOrderDetail UnconfirmObject(VirtualOrderDetail virtualOrderDetail, IVirtualOrderService _virtualOrderService, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService,
                                        IStockMutationService _stockMutationService, IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService);
        VirtualOrderDetail SetDeliveryComplete(VirtualOrderDetail virtualOrderDetail, decimal Quantity);
        VirtualOrderDetail UnsetDeliveryComplete(VirtualOrderDetail virtualOrderDetail, decimal Quantity, IVirtualOrderService _virtualOrderService);
    }
}