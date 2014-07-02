using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface IRecoveryAccessoryDetailValidator
    {
        RecoveryAccessoryDetail VHasRecoveryOrderDetail(RecoveryAccessoryDetail recoveryAccessoryDetail, IRecoveryOrderDetailService _recoveryOrderDetailService);
        RecoveryAccessoryDetail VIsAccessory(RecoveryAccessoryDetail recoveryAccessoryDetail, IItemService _itemService, IItemTypeService _itemTypeService);
        RecoveryAccessoryDetail VNonZeroQuantity(RecoveryAccessoryDetail recoveryAccessoryDetail);
        RecoveryAccessoryDetail VQuantityInStock(RecoveryAccessoryDetail recoveryAccessoryDetail, IItemService _itemService);
        RecoveryAccessoryDetail VHasBeenConfirmed(RecoveryAccessoryDetail recoveryAccessoryDetail);
        RecoveryAccessoryDetail VHasNotBeenConfirmed(RecoveryAccessoryDetail recoveryAccessoryDetail);
        RecoveryAccessoryDetail VRecoveryOrderHasNotBeenFinished(RecoveryAccessoryDetail recoveryAccessoryDetail, IRecoveryOrderService _recoveryOrderService);
        RecoveryAccessoryDetail VRecoveryOrderHasBeenFinished(RecoveryAccessoryDetail recoveryAccessoryDetail, IRecoveryOrderService _recoveryOrderService);
        RecoveryAccessoryDetail VCreateObject(RecoveryAccessoryDetail recoveryAccessoryDetail, IRecoveryOrderDetailService _recoveryOrderDetailService, IItemService _itemService, IItemTypeService _itemTypeService);
        RecoveryAccessoryDetail VUpdateObject(RecoveryAccessoryDetail recoveryAccessoryDetail, IRecoveryOrderDetailService _recoveryOrderDetailService, IItemService _itemService, IItemTypeService _itemTypeService);
        RecoveryAccessoryDetail VDeleteObject(RecoveryAccessoryDetail recoveryAccessoryDetail);
        RecoveryAccessoryDetail VConfirmObject(RecoveryAccessoryDetail recoveryAccessoryDetail, IRecoveryOrderService _recoveryOrderService, IItemService _itemService);
        RecoveryAccessoryDetail VUnconfirmObject(RecoveryAccessoryDetail recoveryAccessoryDetail, IRecoveryOrderService _recoveryOrderService);

        bool ValidCreateObject(RecoveryAccessoryDetail recoveryAccessoryDetail, IRecoveryOrderDetailService _recoveryOrderDetailService, IItemService _itemService, IItemTypeService _itemTypeService);
        bool ValidUpdateObject(RecoveryAccessoryDetail recoveryAccessoryDetail, IRecoveryOrderDetailService _recoveryOrderDetailService, IItemService _itemService, IItemTypeService _itemTypeService);
        bool ValidDeleteObject(RecoveryAccessoryDetail recoveryAccessoryDetail);
        bool ValidConfirmObject(RecoveryAccessoryDetail recoveryAccessoryDetail, IRecoveryOrderService _recoveryOrderService, IRecoveryOrderDetailService _recoveryOrderDetailService, IItemService _itemService);
        bool ValidUnconfirmObject(RecoveryAccessoryDetail recoveryAccessoryDetail, IRecoveryOrderService _recoveryOrderService, IRecoveryOrderDetailService _recoveryOrderDetailService);
        bool isValid(RecoveryAccessoryDetail recoveryAccessoryDetail);
        string PrintError(RecoveryAccessoryDetail recoveryAccessoryDetail);
    }
}