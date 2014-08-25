using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface ISalesOrderDetailValidator
    {
        SalesOrderDetail VHasSalesOrder(SalesOrderDetail salesOrderDetail, ISalesOrderService _salesOrderService);
        SalesOrderDetail VSalesOrderHasNotBeenConfirmed(SalesOrderDetail salesOrderDetail, ISalesOrderService _salesOrderService);
        SalesOrderDetail VHasItem(SalesOrderDetail salesOrderDetail, IItemService _itemService);
        SalesOrderDetail VNonZeroNorNegativeQuantity(SalesOrderDetail salesOrderDetail);
        SalesOrderDetail VNonNegativePrice(SalesOrderDetail salesOrderDetail);
        SalesOrderDetail VUniqueSalesOrderDetail(SalesOrderDetail salesOrderDetail, ISalesOrderDetailService _salesOrderDetailService, IItemService _itemService);
        SalesOrderDetail VHasBeenConfirmed(SalesOrderDetail salesOrderDetail);
        SalesOrderDetail VHasNotBeenConfirmed(SalesOrderDetail salesOrderDetail);
        SalesOrderDetail VHasItemPendingDelivery(SalesOrderDetail salesOrderDetail, IItemService _itemService);
        SalesOrderDetail VHasNoDeliveryOrderDetail(SalesOrderDetail salesOrderDetail, IDeliveryOrderDetailService _deliveryOrderDetailService);
        SalesOrderDetail VHasConfirmationDate(SalesOrderDetail salesOrderDetail);
        SalesOrderDetail VCreateObject(SalesOrderDetail salesOrderDetail, ISalesOrderDetailService _salesOrderDetailService, ISalesOrderService _salesOrderService, IItemService _itemService);
        SalesOrderDetail VUpdateObject(SalesOrderDetail salesOrderDetail, ISalesOrderDetailService _salesOrderDetailService, ISalesOrderService _salesOrderService, IItemService _itemService);
        SalesOrderDetail VDeleteObject(SalesOrderDetail salesOrderDetail);
        SalesOrderDetail VConfirmObject(SalesOrderDetail salesOrderDetail);
        SalesOrderDetail VUnconfirmObject(SalesOrderDetail salesOrderDetail, IDeliveryOrderDetailService _deliveryOrderDetailService, IItemService _itemService);
        bool ValidCreateObject(SalesOrderDetail salesOrderDetail, ISalesOrderDetailService _salesOrderDetailService, ISalesOrderService _salesOrderService, IItemService _itemService);
        bool ValidUpdateObject(SalesOrderDetail salesOrderDetail, ISalesOrderDetailService _salesOrderDetailService, ISalesOrderService _salesOrderService, IItemService _itemService);
        bool ValidDeleteObject(SalesOrderDetail salesOrderDetail);
        bool ValidConfirmObject(SalesOrderDetail salesOrderDetail);
        bool ValidUnconfirmObject(SalesOrderDetail salesOrderDetail, IDeliveryOrderDetailService _deliveryOrderDetailService, IItemService _itemService);
        bool isValid(SalesOrderDetail salesOrderDetail);
        string PrintError(SalesOrderDetail salesOrderDetail);
    }
}
