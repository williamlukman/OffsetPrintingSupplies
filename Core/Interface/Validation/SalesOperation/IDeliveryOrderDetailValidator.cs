using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IDeliveryOrderDetailValidator
    {
        DeliveryOrderDetail VHasDeliveryOrder(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderService _deliveryOrderService);
        DeliveryOrderDetail VDeliveryOrderHasNotBeenConfirmed(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderService _deliveryOrderService);
        DeliveryOrderDetail VHasItem(DeliveryOrderDetail deliveryOrderDetail, IItemService _itemService);
        DeliveryOrderDetail VNonNegativeQuantity(DeliveryOrderDetail deliveryOrderDetail);
        DeliveryOrderDetail VHasSalesOrderDetail(DeliveryOrderDetail deliveryOrderDetail, ISalesOrderDetailService _salesOrderDetailService);
        DeliveryOrderDetail VUniqueSalesOrderDetail(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderDetailService _deliveryOrderDetails, IItemService _itemService);
        DeliveryOrderDetail VSalesOrderDetailHasBeenConfirmed(DeliveryOrderDetail deliveryOrderDetail, ISalesOrderDetailService _salesOrderDetailService);
        DeliveryOrderDetail VQuantityOfDeliveryOrderDetailsIsLessThanOrEqualSalesOrderDetail(DeliveryOrderDetail deliveryOrderDetail,
                                     IDeliveryOrderDetailService _deliveryOrderDetailService, ISalesOrderDetailService _salesOrderDetailService, bool CaseCreate);
        DeliveryOrderDetail VDeliveryOrderAndSalesOrderDetailHaveTheSameSalesOrder(DeliveryOrderDetail deliveryOrderDetail,
                                     IDeliveryOrderService _deliveryOrderService, ISalesOrderDetailService _salesOrderDetailService);
        DeliveryOrderDetail VHasItemQuantity(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderService _deliveryOrderService, IItemService _itemService,
                                             IWarehouseItemService _warehouseItemService);
        DeliveryOrderDetail VHasBeenConfirmed(DeliveryOrderDetail deliveryOrderDetail);
        DeliveryOrderDetail VHasNotBeenConfirmed(DeliveryOrderDetail deliveryOrderDetail);
        DeliveryOrderDetail VHasConfirmationDate(DeliveryOrderDetail deliveryOrderDetail);
        DeliveryOrderDetail VHasNoSalesInvoiceDetail(DeliveryOrderDetail deliveryOrderDetail, ISalesInvoiceDetailService _salesInvoiceDetailService);
        DeliveryOrderDetail VCreateObject(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderDetailService _deliveryOrderDetailService,
                                          IDeliveryOrderService _deliveryOrderService, ISalesOrderDetailService _salesOrderDetailService, IItemService _itemService);
        DeliveryOrderDetail VUpdateObject(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderDetailService _deliveryOrderDetailService,
                                          IDeliveryOrderService _deliveryOrderService, ISalesOrderDetailService _salesOrderDetailService, IItemService _itemService);
        DeliveryOrderDetail VDeleteObject(DeliveryOrderDetail deliveryOrderDetail);
        DeliveryOrderDetail VConfirmObject(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderService _deliveryOrderService, IDeliveryOrderDetailService _deliveryOrderDetailService,
                                           ISalesOrderDetailService _salesOrderDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        DeliveryOrderDetail VUnconfirmObject(DeliveryOrderDetail deliveryOrderDetail, ISalesInvoiceDetailService _salesInvoiceDetailService);
        bool ValidCreateObject(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderDetailService _deliveryOrderDetailService,
                               IDeliveryOrderService _deliveryOrderService, ISalesOrderDetailService _salesOrderDetailService, IItemService _itemService);
        bool ValidUpdateObject(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderDetailService _deliveryOrderDetailService,
                               IDeliveryOrderService _deliveryOrderService, ISalesOrderDetailService _salesOrderDetailService, IItemService _itemService);
        bool ValidDeleteObject(DeliveryOrderDetail deliveryOrderDetail);
        bool ValidConfirmObject(DeliveryOrderDetail deliveryOrderDetail, IDeliveryOrderService _deliveryOrderService, IDeliveryOrderDetailService _deliveryOrderDetailService,
                                ISalesOrderDetailService _salesOrderDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        bool ValidUnconfirmObject(DeliveryOrderDetail deliveryOrderDetail, ISalesInvoiceDetailService _salesInvoiceDetailService);
        bool isValid(DeliveryOrderDetail deliveryOrderDetail);
        string PrintError(DeliveryOrderDetail deliveryOrderDetail);
    }
}
