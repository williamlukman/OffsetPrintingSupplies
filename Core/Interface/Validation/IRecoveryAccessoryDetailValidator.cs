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
        RecoveryAccessoryDetail VIsAccessory(RecoveryAccessoryDetail recoveryAccessoryDetail, IItemService _itemService);
        RecoveryAccessoryDetail VNonZeroQuantity(RecoveryAccessoryDetail recoveryAccessoryDetail);
        RecoveryAccessoryDetail VQuantityInStock(RecoveryAccessoryDetail recoveryAccessoryDetail, IItemService _itemService);
        RecoveryAccessoryDetail VAdjustQuantity(RecoveryAccessoryDetail recoveryAccessoryDetail, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService, IItemService _itemService);
        RecoveryAccessoryDetail VIsConfirmed(RecoveryAccessoryDetail recoveryAccessoryDetail);
        RecoveryAccessoryDetail VIsNotConfirmed(RecoveryAccessoryDetail recoveryAccessoryDetail);

        RecoveryAccessoryDetail VCreateObject(RecoveryAccessoryDetail recoveryAccessoryDetail, IRecoveryOrderDetailService _recoveryOrderDetailService, IItemService _itemService);
        RecoveryAccessoryDetail VUpdateObject(RecoveryAccessoryDetail recoveryAccessoryDetail, IRecoveryOrderDetailService _recoveryOrderDetailService, IItemService _itemService);
        RecoveryAccessoryDetail VDeleteObject(RecoveryAccessoryDetail recoveryAccessoryDetail);
        RecoveryAccessoryDetail VConfirmObject(RecoveryAccessoryDetail recoveryAccessoryDetail, IItemService _itemService);
        RecoveryAccessoryDetail VUnconfirmObject(RecoveryAccessoryDetail recoveryAccessoryDetail, IItemService _itemService);
        RecoveryAccessoryDetail VAdjustQuantity(RecoveryAccessoryDetail recoveryAccessoryDetail);

        bool ValidCreateObject(RecoveryAccessoryDetail recoveryAccessoryDetail, IRecoveryOrderDetailService _recoveryOrderDetailService, IItemService _itemService);
        bool ValidUpdateObject(RecoveryAccessoryDetail recoveryAccessoryDetail, IRecoveryOrderDetailService _recoveryOrderDetailService, IItemService _itemService);
        bool ValidDeleteObject(RecoveryAccessoryDetail recoveryAccessoryDetail);
        bool ValidConfirmObject(RecoveryAccessoryDetail recoveryAccessoryDetail, IItemService _itemService);
        bool ValidUnconfirmObject(RecoveryAccessoryDetail recoveryAccessoryDetail, IItemService _itemService);
        bool ValidAdjustQuantity(RecoveryAccessoryDetail recoveryAccessoryDetail, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService, IItemService _itemService);
        bool isValid(RecoveryAccessoryDetail recoveryAccessoryDetail);
        string PrintError(RecoveryAccessoryDetail recoveryAccessoryDetail);
    }
}