using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IDeliveryOrderValidator
    {
        DeliveryOrder VHasWarehouse(DeliveryOrder deliveryOrder, IWarehouseService _warehouseService);
        DeliveryOrder VHasSalesOrder(DeliveryOrder deliveryOrder, ISalesOrderService _salesOrderService);
        DeliveryOrder VHasDeliveryDate(DeliveryOrder deliveryOrder);
        DeliveryOrder VHasBeenConfirmed(DeliveryOrder deliveryOrder);
        DeliveryOrder VHasNotBeenConfirmed(DeliveryOrder deliveryOrder);
        DeliveryOrder VHasDeliveryOrderDetails(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService);
        DeliveryOrder VHasNoDeliveryOrderDetail(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService);
        DeliveryOrder VSalesOrderHasBeenConfirmed(DeliveryOrder deliveryOrder, ISalesOrderService _salesOrderService);
        DeliveryOrder VHasConfirmationDate(DeliveryOrder deliveryOrder);
        DeliveryOrder VHasNoSalesInvoice(DeliveryOrder deliveryOrder, ISalesInvoiceService _salesInvoiceService);
        DeliveryOrder VCreateObject(DeliveryOrder deliveryOrder, ISalesOrderService _salesOrderService, IWarehouseService _warehouseService);
        DeliveryOrder VUpdateObject(DeliveryOrder deliveryOrder, ISalesOrderService _salesOrderService, IWarehouseService _warehouseService);
        DeliveryOrder VDeleteObject(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService);
        DeliveryOrder VConfirmObject(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService);
        DeliveryOrder VUnconfirmObject(DeliveryOrder deliveryOrder, ISalesInvoiceService _salesInvoiceService);
        bool ValidCreateObject(DeliveryOrder deliveryOrder, ISalesOrderService _salesOrderService, IWarehouseService _warehouseService);
        bool ValidUpdateObject(DeliveryOrder deliveryOrder, ISalesOrderService _salesOrderService, IWarehouseService _warehouseService);
        bool ValidDeleteObject(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService);
        bool ValidConfirmObject(DeliveryOrder deliveryOrder, IDeliveryOrderDetailService _deliveryOrderDetailService);
        bool ValidUnconfirmObject(DeliveryOrder deliveryOrder, ISalesInvoiceService _salesInvoiceService);
        bool isValid(DeliveryOrder deliveryOrder);
        string PrintError(DeliveryOrder deliveryOrder);
    }
}
