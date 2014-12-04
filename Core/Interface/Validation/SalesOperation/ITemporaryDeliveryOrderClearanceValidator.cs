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
        TemporaryDeliveryOrderClearance VCreateObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService, ITemporaryDeliveryOrderService _temporaryDeliveryOrderService);
        TemporaryDeliveryOrderClearance VUpdateObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService, ITemporaryDeliveryOrderService _temporaryDeliveryOrderService);
        TemporaryDeliveryOrderClearance VDeleteObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService);
        TemporaryDeliveryOrderClearance VConfirmObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService, IClosingService _closingService);
        TemporaryDeliveryOrderClearance VUnconfirmObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, IClosingService _closingService);
        bool ValidCreateObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService, ITemporaryDeliveryOrderService _temporaryDeliveryOrderService);
        bool ValidUpdateObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, ITemporaryDeliveryOrderClearanceService _temporaryDeliveryOrderClearanceService, ITemporaryDeliveryOrderService _temporaryDeliveryOrderService);
        bool ValidDeleteObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService);
        bool ValidConfirmObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, ITemporaryDeliveryOrderClearanceDetailService _temporaryDeliveryOrderClearanceDetailService, IClosingService _closingService);
        bool ValidUnconfirmObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance, IClosingService _closingService);
        bool isValid(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance);
        string PrintError(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance);
    }
}
