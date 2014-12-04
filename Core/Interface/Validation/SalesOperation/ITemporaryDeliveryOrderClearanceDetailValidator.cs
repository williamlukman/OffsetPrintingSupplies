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
        TemporaryDeliveryOrderClearanceDetail VCreateObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail, ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService,
                                                          ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService);
        TemporaryDeliveryOrderClearanceDetail VUpdateObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail, ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService,
                                                          ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService);
        TemporaryDeliveryOrderClearanceDetail VDeleteObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail);
        TemporaryDeliveryOrderClearanceDetail VConfirmObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail, ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService,
                                                    ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        TemporaryDeliveryOrderClearanceDetail VUnconfirmObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail);
        TemporaryDeliveryOrderClearanceDetail VProcessObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail);

        bool ValidCreateObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail, ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService,
                                      ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService);
        bool ValidUpdateObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail, ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService,
                                      ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService);
        bool ValidDeleteObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail);
        bool ValidConfirmObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail, ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService,
                                       ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService, ITemporaryDeliveryOrderDetailService _temporaryDeliveryOrderDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        bool ValidUnconfirmObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail);
        bool ValidProcessObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail);
        bool isValid(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail);
        string PrintError(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail);
    }
}
