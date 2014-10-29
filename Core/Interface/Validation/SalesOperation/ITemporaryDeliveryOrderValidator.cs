using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface ITemporaryDeliveryOrderValidator
    {
        TemporaryDeliveryOrder VHasUniqueNomorSurat(TemporaryDeliveryOrder temporaryDeliveryOrder, ITemporaryDeliveryOrderService _temporaryDeliveryOrderService);
        TemporaryDeliveryOrder VHasWarehouse(TemporaryDeliveryOrder temporaryDeliveryOrder, IWarehouseService _warehouseService);
        TemporaryDeliveryOrder VHasVirtualOrderOrDeliveryOrder(TemporaryDeliveryOrder temporaryDeliveryOrder, IVirtualOrderService _virtualOrderService, IDeliveryOrderService _deliveryOrderService);
        TemporaryDeliveryOrder VHasDeliveryDate(TemporaryDeliveryOrder temporaryDeliveryOrder);
        TemporaryDeliveryOrder VHasBeenConfirmed(TemporaryDeliveryOrder temporaryDeliveryOrder);
        TemporaryDeliveryOrder VHasNotBeenConfirmed(TemporaryDeliveryOrder temporaryDeliveryOrder);
        TemporaryDeliveryOrder VHasTemporaryDeliveryOrderDetails(TemporaryDeliveryOrder temporaryDeliveryOrder, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService);
        TemporaryDeliveryOrder VHasNoTemporaryDeliveryOrderDetail(TemporaryDeliveryOrder temporaryDeliveryOrder, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService);
        TemporaryDeliveryOrder VVirtualOrderHasBeenConfirmed(TemporaryDeliveryOrder temporaryDeliveryOrder, IVirtualOrderService _virtualOrderService);
        TemporaryDeliveryOrder VDeliveryOrderHasNotBeenConfirmed(TemporaryDeliveryOrder temporaryDeliveryOrder, IDeliveryOrderService _virtualOrderService);
        TemporaryDeliveryOrder VHasConfirmationDate(TemporaryDeliveryOrder temporaryDeliveryOrder);
        TemporaryDeliveryOrder VGeneralLedgerPostingHasNotBeenClosed(TemporaryDeliveryOrder temporaryDeliveryOrder, IClosingService _closingService, DateTime PushDate);
        TemporaryDeliveryOrder VAllQuantitiesEqualWasteAndRestock(TemporaryDeliveryOrder temporaryDeliveryOrder, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService);
        TemporaryDeliveryOrder VDeliveryOrderHasNotBeenConfirmedForPartDeliveryOrder(TemporaryDeliveryOrder temporaryDeliveryOrder, IDeliveryOrderService _deliveryOrderService);
        TemporaryDeliveryOrder VCreateObject(TemporaryDeliveryOrder temporaryDeliveryOrder, ITemporaryDeliveryOrderService _temporaryDeliveryOrderService, IVirtualOrderService _virtualOrderService, IDeliveryOrderService _deliveryOrderService, IWarehouseService _warehouseService);
        TemporaryDeliveryOrder VUpdateObject(TemporaryDeliveryOrder temporaryDeliveryOrder, ITemporaryDeliveryOrderService _temporaryDeliveryOrderService, IVirtualOrderService _virtualOrderService, IDeliveryOrderService _deliveryOrderService, IWarehouseService _warehouseService);
        TemporaryDeliveryOrder VDeleteObject(TemporaryDeliveryOrder temporaryDeliveryOrder, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService);
        TemporaryDeliveryOrder VConfirmObject(TemporaryDeliveryOrder temporaryDeliveryOrder, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService);
        TemporaryDeliveryOrder VUnconfirmObject(TemporaryDeliveryOrder temporaryDeliveryOrder);
        TemporaryDeliveryOrder VPushObject(TemporaryDeliveryOrder temporaryDeliveryOrder, DateTime PushDate, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService, IClosingService _closingService,
                                           IDeliveryOrderService _deliveryOrderService);
        bool ValidCreateObject(TemporaryDeliveryOrder temporaryDeliveryOrder, ITemporaryDeliveryOrderService _temporaryDeliveryOrderService, IVirtualOrderService _virtualOrderService, IDeliveryOrderService _deliveryOrderService, IWarehouseService _warehouseService);
        bool ValidUpdateObject(TemporaryDeliveryOrder temporaryDeliveryOrder, ITemporaryDeliveryOrderService _temporaryDeliveryOrderService, IVirtualOrderService _virtualOrderService, IDeliveryOrderService _deliveryOrderService, IWarehouseService _warehouseService);
        bool ValidDeleteObject(TemporaryDeliveryOrder temporaryDeliveryOrder, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService);
        bool ValidConfirmObject(TemporaryDeliveryOrder temporaryDeliveryOrder, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService);
        bool ValidUnconfirmObject(TemporaryDeliveryOrder temporaryDeliveryOrder);
        bool ValidPushObject(TemporaryDeliveryOrder temporaryDeliveryOrder, DateTime PushDate, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService, IClosingService _closingService,
                             IDeliveryOrderService _deliveryOrderService);
        bool isValid(TemporaryDeliveryOrder temporaryDeliveryOrder);
        string PrintError(TemporaryDeliveryOrder temporaryDeliveryOrder);
    }
}
