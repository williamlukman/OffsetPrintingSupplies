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
        SalesOrderDetail VHasItem(SalesOrderDetail salesOrderDetail, IItemService _itemService);
        SalesOrderDetail VQuantity(SalesOrderDetail salesOrderDetail);
        SalesOrderDetail VPrice(SalesOrderDetail salesOrderDetail);
        SalesOrderDetail VUniqueSalesOrderDetail(SalesOrderDetail salesOrderDetail, ISalesOrderDetailService _salesOrderDetailService, IItemService _itemService);
        SalesOrderDetail VHasBeenFinished(SalesOrderDetail salesOrderDetail);
        SalesOrderDetail VHasNotBeenFinished(SalesOrderDetail salesOrderDetail);
        SalesOrderDetail VSalesOrderHasNotBeenCompleted(SalesOrderDetail salesOrderDetail, ISalesOrderService _salesOrderService);
        SalesOrderDetail VHasItemPendingDelivery(SalesOrderDetail salesOrderDetail, IItemService _itemService);
        SalesOrderDetail VHasNoDeliveryOrderDetail(SalesOrderDetail salesOrderDetail, IDeliveryOrderDetailService _deliveryOrderDetailService);
        SalesOrderDetail VDeliveryOrderDetailHasBeenFinished(SalesOrderDetail salesOrderDetail, IDeliveryOrderDetailService _deliveryOrderDetailService);
        SalesOrderDetail VCreateObject(SalesOrderDetail salesOrderDetail, ISalesOrderDetailService _salesOrderDetailService, ISalesOrderService _salesOrderService, IItemService _itemService);
        SalesOrderDetail VUpdateObject(SalesOrderDetail salesOrderDetail, ISalesOrderDetailService _salesOrderDetailService, ISalesOrderService _salesOrderService, IItemService _itemService);
        SalesOrderDetail VDeleteObject(SalesOrderDetail salesOrderDetail);
        SalesOrderDetail VFinishObject(SalesOrderDetail salesOrderDetail);
        SalesOrderDetail VUnfinishObject(SalesOrderDetail salesOrderDetail, ISalesOrderService _salesOrderService, ISalesOrderDetailService _salesOrderDetailService, IDeliveryOrderDetailService _deliveryOrderDetailService, IItemService _itemService);
        bool ValidCreateObject(SalesOrderDetail salesOrderDetail, ISalesOrderDetailService _salesOrderDetailService, ISalesOrderService _salesOrderService, IItemService _itemService);
        bool ValidUpdateObject(SalesOrderDetail salesOrderDetail, ISalesOrderDetailService _salesOrderDetailService, ISalesOrderService _salesOrderService, IItemService _itemService);
        bool ValidDeleteObject(SalesOrderDetail salesOrderDetail);
        bool ValidFinishObject(SalesOrderDetail salesOrderDetail);
        bool ValidUnfinishObject(SalesOrderDetail salesOrderDetail, ISalesOrderService _salesOrderService, ISalesOrderDetailService _salesOrderDetailService, IDeliveryOrderDetailService _deliveryOrderDetailService, IItemService _itemService);
        bool isValid(SalesOrderDetail salesOrderDetail);
        string PrintError(SalesOrderDetail salesOrderDetail);
    }
}
