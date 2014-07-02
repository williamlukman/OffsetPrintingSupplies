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
        RecoveryAccessoryDetail VAdjustQuantity(RecoveryAccessoryDetail recoveryAccessoryDetail, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService, IItemService _itemService);
        RecoveryAccessoryDetail VIsConfirmed(RecoveryAccessoryDetail recoveryAccessoryDetail);
        RecoveryAccessoryDetail VIsNotConfirmed(RecoveryAccessoryDetail recoveryAccessoryDetail);

        RecoveryAccessoryDetail VCreateObject(RecoveryAccessoryDetail recoveryAccessoryDetail, IRecoveryOrderDetailService _recoveryOrderDetailService, IItemService _itemService, IItemTypeService _itemTypeService);
        RecoveryAccessoryDetail VUpdateObject(RecoveryAccessoryDetail recoveryAccessoryDetail, IRecoveryOrderDetailService _recoveryOrderDetailService, IItemService _itemService, IItemTypeService _itemTypeService);
        RecoveryAccessoryDetail VDeleteObject(RecoveryAccessoryDetail recoveryAccessoryDetail);
        RecoveryAccessoryDetail VConfirmObject(RecoveryAccessoryDetail recoveryAccessoryDetail, IItemService _itemService);
        RecoveryAccessoryDetail VUnconfirmObject(RecoveryAccessoryDetail recoveryAccessoryDetail, IItemService _itemService);

        bool ValidCreateObject(RecoveryAccessoryDetail recoveryAccessoryDetail, IRecoveryOrderDetailService _recoveryOrderDetailService, IItemService _itemService, IItemTypeService _itemTypeService);
        bool ValidUpdateObject(RecoveryAccessoryDetail recoveryAccessoryDetail, IRecoveryOrderDetailService _recoveryOrderDetailService, IItemService _itemService, IItemTypeService _itemTypeService);
        bool ValidDeleteObject(RecoveryAccessoryDetail recoveryAccessoryDetail);
        bool ValidConfirmObject(RecoveryAccessoryDetail recoveryAccessoryDetail, IRecoveryOrderService _recoveryOrderService, IRecoveryOrderDetailService _recoveryOrderDetailService, IItemService _itemService);
        bool ValidUnconfirmObject(RecoveryAccessoryDetail recoveryAccessoryDetail, IRecoveryOrderService _recoveryOrderService, IRecoveryOrderDetailService _recoveryOrderDetailService, IItemService _itemService);
        bool isValid(RecoveryAccessoryDetail recoveryAccessoryDetail);
        string PrintError(RecoveryAccessoryDetail recoveryAccessoryDetail);
    }
}