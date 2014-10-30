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
        TemporaryDeliveryOrderDetail VHasBeenReconciled(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail);
        TemporaryDeliveryOrderDetail VHasNotBeenReconciled(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail);
        TemporaryDeliveryOrderDetail VQuantityEqualsWasteAndRestock(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail);
        TemporaryDeliveryOrderDetail VQuantityLessThanOrEqualWasteAndRestock(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail);
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
        TemporaryDeliveryOrderDetail VProcessObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail);
        TemporaryDeliveryOrderDetail VReconcileObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail, IClosingService _closingService);
        TemporaryDeliveryOrderDetail VUnreconcileObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail, IClosingService _closingService);
        TemporaryDeliveryOrderDetail VCompleteObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail);
        TemporaryDeliveryOrderDetail VUndoCompleteObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail);

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
        bool ValidProcessObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail);
        bool ValidReconcileObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail, IClosingService _closingService);
        bool ValidUnreconcileObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail, IClosingService _closingService);
        bool ValidCompleteObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail);
        bool ValidUndoCompleteObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail);
        bool isValid(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail);
        string PrintError(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail);
    }
}
