using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface ITemporaryDeliveryOrderClearanceValidator
    {
        TemporaryDeliveryOrderClearance VHasClearanceDate(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance);
        TemporaryDeliveryOrderClearance VHasBeenConfirmed(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance);
        TemporaryDeliveryOrderClearance VHasNotBeenConfirmed(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance);
        TemporaryDeliveryOrderClearance VHasTemporaryDeliveryOrderClearanceDetails(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService);
        TemporaryDeliveryOrderClearance VHasNoTemporaryDeliveryOrderClearanceDetail(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService);
        TemporaryDeliveryOrderClearance VTemporaryDeliveryOrderHasNotBeenConfirmed(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, IDeliveryOrderService _virtualOrderService);
        TemporaryDeliveryOrderClearance VHasConfirmationDate(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance);
        TemporaryDeliveryOrderClearance VHasNotBeenReconciled(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance);
        TemporaryDeliveryOrderClearance VGeneralLedgerPostingHasNotBeenClosed(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, IClosingService _closingService, DateTime PushDate);
        TemporaryDeliveryOrderClearance VAllQuantitiesEqualWasteAndRestock(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService);
        TemporaryDeliveryOrderClearance VTemporaryDeliveryOrderHasNotBeenConfirmedForPartDeliveryOrder(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, IDeliveryOrderService _deliveryOrderService);
        TemporaryDeliveryOrderClearance VCreateObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService, IVirtualOrderService _virtualOrderService, IDeliveryOrderService _deliveryOrderService, IWarehouseService _warehouseService);
        TemporaryDeliveryOrderClearance VUpdateObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService, IVirtualOrderService _virtualOrderService, IDeliveryOrderService _deliveryOrderService, IWarehouseService _warehouseService);
        TemporaryDeliveryOrderClearance VDeleteObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService);
        TemporaryDeliveryOrderClearance VConfirmObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService);
        TemporaryDeliveryOrderClearance VUnconfirmObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance);
        TemporaryDeliveryOrderClearance VPushObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, DateTime PushDate, ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService, IClosingService _closingService,
                                           IDeliveryOrderService _deliveryOrderService);
        bool ValidCreateObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService, IVirtualOrderService _virtualOrderService, IDeliveryOrderService _deliveryOrderService, IWarehouseService _warehouseService);
        bool ValidUpdateObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService, IVirtualOrderService _virtualOrderService, IDeliveryOrderService _deliveryOrderService, IWarehouseService _warehouseService);
        bool ValidDeleteObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService);
        bool ValidConfirmObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService);
        bool ValidUnconfirmObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance);
        bool ValidPushObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, DateTime PushDate, ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService, IClosingService _closingService,
                             IDeliveryOrderService _deliveryOrderService);
        bool isValid(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance);
        string PrintError(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance);
    }
}
