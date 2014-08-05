using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IDeliveryOrderDetailValidator
    {
        DeliveryOrderDetail VHasDeliveryOrder(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderService _deliveryOrderService);
        DeliveryOrderDetail VHasItem(DeliveryOrderDetail deliveryOrderDetail, IItemService _itemService);
        DeliveryOrderDetail VCustomer(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderService _deliveryOrderService, ISalesOrderService _sos, ISalesOrderDetailService _salesOrderDetailService, ICustomerService _customerService);
        DeliveryOrderDetail VQuantityCreate(DeliveryOrderDetail deliveryOrderDetail, ISalesOrderDetailService _salesOrderDetailService);
        DeliveryOrderDetail VQuantityUpdate(DeliveryOrderDetail deliveryOrderDetail, ISalesOrderDetailService _salesOrderDetailService);
        DeliveryOrderDetail VQuantityUnfinish(DeliveryOrderDetail deliveryOrderDetail, IItemService _itemService);
        DeliveryOrderDetail VHasSalesOrderDetail(DeliveryOrderDetail deliveryOrderDetail, ISalesOrderDetailService _salesOrderDetailService);
        DeliveryOrderDetail VHasItemQuantity(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderService _deliveryOrderService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        DeliveryOrderDetail VUniqueSalesOrderDetail(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderDetailService _deliveryOrderDetails, IItemService _itemService);
        DeliveryOrderDetail VHasBeenFinished(DeliveryOrderDetail deliveryOrderDetail);
        DeliveryOrderDetail VHasNotBeenFinished(DeliveryOrderDetail deliveryOrderDetail);
        DeliveryOrderDetail VDeliveryOrderHasNotBeenCompleted(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderService _deliveryOrderService);
        DeliveryOrderDetail VCreateObject(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderDetailService _deliveryOrderDetails, IDeliveryOrderService _deliveryOrderService, ISalesOrderDetailService _salesOrderDetailService, ISalesOrderService _sos, IItemService _itemService, ICustomerService _customerService);
        DeliveryOrderDetail VUpdateObject(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderDetailService _deliveryOrderDetails, IDeliveryOrderService _deliveryOrderService, ISalesOrderDetailService _salesOrderDetailService, ISalesOrderService _sos, IItemService _itemService, ICustomerService _customerService);
        DeliveryOrderDetail VDeleteObject(DeliveryOrderDetail deliveryOrderDetail);
        DeliveryOrderDetail VFinishObject(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderService _deliveryOrderService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        DeliveryOrderDetail VUnfinishObject(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderService _deliveryOrderService, IDeliveryOrderDetailService _deliveryOrderDetailService, IItemService _itemService);
        bool ValidCreateObject(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderDetailService _deliveryOrderDetails, IDeliveryOrderService _deliveryOrderService, ISalesOrderDetailService _salesOrderDetailService, ISalesOrderService _sos, IItemService _itemService, ICustomerService _customerService);
        bool ValidUpdateObject(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderDetailService _deliveryOrderDetails, IDeliveryOrderService _deliveryOrderService, ISalesOrderDetailService _salesOrderDetailService, ISalesOrderService _sos, IItemService _itemService, ICustomerService _customerService);
        bool ValidDeleteObject(DeliveryOrderDetail deliveryOrderDetail);
        bool ValidFinishObject(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderService _deliveryOrderService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        bool ValidUnfinishObject(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderService _deliveryOrderService, IDeliveryOrderDetailService _deliveryOrderDetails, IItemService _itemService);
        bool isValid(DeliveryOrderDetail deliveryOrderDetail);
        string PrintError(DeliveryOrderDetail deliveryOrderDetail);
    }
}
