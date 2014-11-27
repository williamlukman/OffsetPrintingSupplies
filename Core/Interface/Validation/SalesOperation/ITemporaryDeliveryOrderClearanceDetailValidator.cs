using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface ITemporaryDeliveryOrderClearanceDetailValidator
    {
        TemporaryDeliveryOrderClearanceDetail VHasTemporaryDeliveryOrderClearance(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail, ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService);
        TemporaryDeliveryOrderClearanceDetail VTemporaryDeliveryOrderClearanceHasNotBeenConfirmed(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail,
                                                                                ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService);
        TemporaryDeliveryOrderClearanceDetail VNonNegativeQuantity(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail);
        TemporaryDeliveryOrderClearanceDetail VHasTemporaryDeliveryOrderDetail(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail, ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService,
                                                                   IVirtualOrderDetailService _virtualOrderDetailService, ISalesOrderDetailService _salesOrderDetailService);
        TemporaryDeliveryOrderClearanceDetail VUniqueTemporaryDeliveryOrderDetail(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail,
                                                                      ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService,
                                                                      ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService, IVirtualOrderDetailService _virtualOrderDetailService,
                                                                      ISalesOrderDetailService _salesOrderDetailService, IItemService _itemService);
        TemporaryDeliveryOrderClearanceDetail VTemporaryDeliveryOrderDetailHasBeenConfirmed(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail,
                                                                                ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService,
                                                                                IVirtualOrderDetailService _virtualOrderDetailService, ISalesOrderDetailService _salesOrderDetailService);
        TemporaryDeliveryOrderClearanceDetail VQuantityOfTemporaryDeliveryOrderClearanceDetailsIsLessThanOrEqualVirtualOrSalesOrderDetail(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail,
                                     ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService, ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService,
                                     IVirtualOrderDetailService _virtualOrderDetailService, ISalesOrderDetailService _salesOrderDetailService, bool CaseCreate);
        TemporaryDeliveryOrderClearanceDetail VHasTheSameParentOrder(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail, ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService,
                                                            IDeliveryOrderService _deliveryOrderService, IVirtualOrderDetailService _virtualOrderDetailService,
                                                            ISalesOrderDetailService _salesOrderDetailService);
        TemporaryDeliveryOrderClearanceDetail VHasItemQuantity(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail, ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService,
                                                      IItemService _itemService, IWarehouseItemService _warehouseItemService);
        TemporaryDeliveryOrderClearanceDetail VHasBeenConfirmed(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail);
        TemporaryDeliveryOrderClearanceDetail VHasNotBeenConfirmed(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail);
        TemporaryDeliveryOrderClearanceDetail VHasConfirmationDate(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail);
        TemporaryDeliveryOrderClearanceDetail VHasBeenReconciled(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail);
        TemporaryDeliveryOrderClearanceDetail VHasNotBeenReconciled(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail);
        TemporaryDeliveryOrderClearanceDetail VQuantityEqualsWasteAndReturn(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail);
        TemporaryDeliveryOrderClearanceDetail VQuantityLessThanOrEqualWasteAndReturn(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail);
        TemporaryDeliveryOrderClearanceDetail VCreateObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail, ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService,
                                                   ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService, IVirtualOrderDetailService _virtualOrderDetailService,
                                                   ISalesOrderDetailService _salesOrderDetailService, IDeliveryOrderService _deliveryOrderService, IItemService _itemService);
        TemporaryDeliveryOrderClearanceDetail VUpdateObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail, ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService,
                                                   ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService, IVirtualOrderDetailService _virtualOrderDetailService,
                                                   ISalesOrderDetailService _salesOrderDetailService, IDeliveryOrderService _deliveryOrderService, IItemService _itemService);
        TemporaryDeliveryOrderClearanceDetail VDeleteObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail);
        TemporaryDeliveryOrderClearanceDetail VConfirmObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail, ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService,
                                                    ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService, IVirtualOrderDetailService _virtualOrderDetailService,
                                                    ISalesOrderDetailService _salesOrderDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        TemporaryDeliveryOrderClearanceDetail VUnconfirmObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail);
        TemporaryDeliveryOrderClearanceDetail VProcessObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail);

        bool ValidCreateObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail, ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService,
                               ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService, IVirtualOrderDetailService _virtualOrderDetailService,
                               ISalesOrderDetailService _salesOrderDetailService, IDeliveryOrderService _deliveryOrderService, IItemService _itemService);
        bool ValidUpdateObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail, ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService,
                               ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService, IVirtualOrderDetailService _virtualOrderDetailService,
                               ISalesOrderDetailService _salesOrderDetailService, IDeliveryOrderService _deliveryOrderService, IItemService _itemService);
        bool ValidDeleteObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail);
        bool ValidConfirmObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail, ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService,
                                ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService, IVirtualOrderDetailService _virtualOrderDetailService,
                                ISalesOrderDetailService _salesOrderDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        bool ValidUnconfirmObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail);
        bool ValidProcessObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail);
        bool isValid(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail);
        string PrintError(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail);
    }
}
