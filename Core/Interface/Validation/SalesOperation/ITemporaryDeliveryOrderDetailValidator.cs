using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface ITemporaryDeliveryOrderDetailValidator
    {
        TemporaryDeliveryOrderDetail VHasTemporaryDeliveryOrder(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail, ITemporaryDeliveryOrderService _temporaryDeliveryOrderService);
        TemporaryDeliveryOrderDetail VTemporaryDeliveryOrderHasNotBeenConfirmed(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail,
                                                                                ITemporaryDeliveryOrderService _temporaryDeliveryOrderService);
        TemporaryDeliveryOrderDetail VHasItem(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail, IItemService _itemService);
        TemporaryDeliveryOrderDetail VNonNegativeQuantity(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail);
        TemporaryDeliveryOrderDetail VHasVirtualOrSalesOrderDetail(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail, ITemporaryDeliveryOrderService _temporaryDeliveryOrderService,
                                                                   IVirtualOrderDetailService _virtualOrderDetailService, ISalesOrderDetailService _salesOrderDetailService);
        TemporaryDeliveryOrderDetail VUniqueVirtualOrSalesOrderDetail(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail,
                                                                      ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService,
                                                                      ITemporaryDeliveryOrderService _temporaryDeliveryOrderService, IVirtualOrderDetailService _virtualOrderDetailService,
                                                                      ISalesOrderDetailService _salesOrderDetailService, IItemService _itemService);
        TemporaryDeliveryOrderDetail VVirtualOrSalesOrderDetailHasBeenConfirmed(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail,
                                                                                ITemporaryDeliveryOrderService _temporaryDeliveryOrderService,
                                                                                IVirtualOrderDetailService _virtualOrderDetailService, ISalesOrderDetailService _salesOrderDetailService);
        TemporaryDeliveryOrderDetail VQuantityOfTemporaryDeliveryOrderDetailsIsLessThanOrEqualVirtualOrSalesOrderDetail(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail,
                                     ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService, ITemporaryDeliveryOrderService _temporaryDeliveryOrderService,
                                     IVirtualOrderDetailService _virtualOrderDetailService, ISalesOrderDetailService _salesOrderDetailService, bool CaseCreate);
        TemporaryDeliveryOrderDetail VHasTheSameParentOrder(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail, ITemporaryDeliveryOrderService _temporaryDeliveryOrderService,
                                                            IDeliveryOrderService _deliveryOrderService, IVirtualOrderDetailService _virtualOrderDetailService,
                                                            ISalesOrderDetailService _salesOrderDetailService);
        TemporaryDeliveryOrderDetail VHasItemQuantity(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail, ITemporaryDeliveryOrderService _temporaryDeliveryOrderService,
                                                      IItemService _itemService, IWarehouseItemService _warehouseItemService);
        TemporaryDeliveryOrderDetail VHasBeenConfirmed(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail);
        TemporaryDeliveryOrderDetail VHasNotBeenConfirmed(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail);
        TemporaryDeliveryOrderDetail VHasConfirmationDate(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail);
        TemporaryDeliveryOrderDetail VCreateObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService,
                                                   ITemporaryDeliveryOrderService _temporaryDeliveryOrderService, IVirtualOrderDetailService _virtualOrderDetailService,
                                                   ISalesOrderDetailService _salesOrderDetailService, IDeliveryOrderService _deliveryOrderService, IItemService _itemService);
        TemporaryDeliveryOrderDetail VUpdateObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService,
                                                   ITemporaryDeliveryOrderService _temporaryDeliveryOrderService, IVirtualOrderDetailService _virtualOrderDetailService,
                                                   ISalesOrderDetailService _salesOrderDetailService, IDeliveryOrderService _deliveryOrderService, IItemService _itemService);
        TemporaryDeliveryOrderDetail VDeleteObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail);
        TemporaryDeliveryOrderDetail VConfirmObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail, ITemporaryDeliveryOrderService _temporaryDeliveryOrderService,
                                                    ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService, IVirtualOrderDetailService _virtualOrderDetailService,
                                                    ISalesOrderDetailService _salesOrderDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        TemporaryDeliveryOrderDetail VUnconfirmObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail);
        bool ValidCreateObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService,
                               ITemporaryDeliveryOrderService _temporaryDeliveryOrderService, IVirtualOrderDetailService _virtualOrderDetailService,
                               ISalesOrderDetailService _salesOrderDetailService, IDeliveryOrderService _deliveryOrderService, IItemService _itemService);
        bool ValidUpdateObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService,
                               ITemporaryDeliveryOrderService _temporaryDeliveryOrderService, IVirtualOrderDetailService _virtualOrderDetailService,
                               ISalesOrderDetailService _salesOrderDetailService, IDeliveryOrderService _deliveryOrderService, IItemService _itemService);
        bool ValidDeleteObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail);
        bool ValidConfirmObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail, ITemporaryDeliveryOrderService _temporaryDeliveryOrderService,
                                ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService, IVirtualOrderDetailService _virtualOrderDetailService,
                                ISalesOrderDetailService _salesOrderDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        bool ValidUnconfirmObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail);
        bool isValid(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail);
        string PrintError(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail);
    }
}
