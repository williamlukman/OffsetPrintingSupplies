using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IVirtualOrderDetailValidator
    {
        VirtualOrderDetail VHasVirtualOrder(VirtualOrderDetail virtualOrderDetail, IVirtualOrderService _virtualOrderService);
        VirtualOrderDetail VVirtualOrderHasNotBeenConfirmed(VirtualOrderDetail virtualOrderDetail, IVirtualOrderService _virtualOrderService);
        VirtualOrderDetail VHasItem(VirtualOrderDetail virtualOrderDetail, IItemService _itemService);
        VirtualOrderDetail VNonZeroNorNegativeQuantity(VirtualOrderDetail virtualOrderDetail);
        VirtualOrderDetail VNonNegativePrice(VirtualOrderDetail virtualOrderDetail);
        VirtualOrderDetail VUniqueVirtualOrderDetail(VirtualOrderDetail virtualOrderDetail, IVirtualOrderDetailService _virtualOrderDetailService, IItemService _itemService);
        VirtualOrderDetail VHasBeenConfirmed(VirtualOrderDetail virtualOrderDetail);
        VirtualOrderDetail VHasNotBeenConfirmed(VirtualOrderDetail virtualOrderDetail);
        VirtualOrderDetail VHasItemPendingDelivery(VirtualOrderDetail virtualOrderDetail, IItemService _itemService);
        VirtualOrderDetail VHasNoTemporaryDeliveryOrderDetail(VirtualOrderDetail virtualOrderDetail, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService);
        VirtualOrderDetail VHasConfirmationDate(VirtualOrderDetail virtualOrderDetail);
        VirtualOrderDetail VCreateObject(VirtualOrderDetail virtualOrderDetail, IVirtualOrderDetailService _virtualOrderDetailService, IVirtualOrderService _virtualOrderService, IItemService _itemService);
        VirtualOrderDetail VUpdateObject(VirtualOrderDetail virtualOrderDetail, IVirtualOrderDetailService _virtualOrderDetailService, IVirtualOrderService _virtualOrderService, IItemService _itemService);
        VirtualOrderDetail VDeleteObject(VirtualOrderDetail virtualOrderDetail);
        VirtualOrderDetail VConfirmObject(VirtualOrderDetail virtualOrderDetail);
        VirtualOrderDetail VUnconfirmObject(VirtualOrderDetail virtualOrderDetail, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService, IItemService _itemService);
        bool ValidCreateObject(VirtualOrderDetail virtualOrderDetail, IVirtualOrderDetailService _virtualOrderDetailService, IVirtualOrderService _virtualOrderService, IItemService _itemService);
        bool ValidUpdateObject(VirtualOrderDetail virtualOrderDetail, IVirtualOrderDetailService _virtualOrderDetailService, IVirtualOrderService _virtualOrderService, IItemService _itemService);
        bool ValidDeleteObject(VirtualOrderDetail virtualOrderDetail);
        bool ValidConfirmObject(VirtualOrderDetail virtualOrderDetail);
        bool ValidUnconfirmObject(VirtualOrderDetail virtualOrderDetail, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService, IItemService _itemService);
        bool isValid(VirtualOrderDetail virtualOrderDetail);
        string PrintError(VirtualOrderDetail virtualOrderDetail);
    }
}
