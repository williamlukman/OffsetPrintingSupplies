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
        RecoveryAccessoryDetail VQuantityInStock(RecoveryAccessoryDetail recoveryAccessoryDetail, IRecoveryOrderService _recoveryOrderService,
                                                 IRecoveryOrderDetailService _recoveryOrderDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        RecoveryAccessoryDetail VHasBeenFinished(RecoveryAccessoryDetail recoveryAccessoryDetail);
        RecoveryAccessoryDetail VHasNotBeenFinished(RecoveryAccessoryDetail recoveryAccessoryDetail);
        RecoveryAccessoryDetail VRecoveryOrderHasNotBeenFinished(RecoveryAccessoryDetail recoveryAccessoryDetail, IRecoveryOrderService _recoveryOrderService, 
                                                                 IRecoveryOrderDetailService _recoveryOrderDetailService);
        RecoveryAccessoryDetail VRecoveryOrderHasBeenFinished(RecoveryAccessoryDetail recoveryAccessoryDetail, IRecoveryOrderService _recoveryOrderService, 
                                                              IRecoveryOrderDetailService _recoveryOrderDetailService);
        RecoveryAccessoryDetail VCreateObject(RecoveryAccessoryDetail recoveryAccessoryDetail, IRecoveryOrderDetailService _recoveryOrderDetailService,
                                              IItemService _itemService, IItemTypeService _itemTypeService);
        RecoveryAccessoryDetail VUpdateObject(RecoveryAccessoryDetail recoveryAccessoryDetail, IRecoveryOrderDetailService _recoveryOrderDetailService,
                                              IItemService _itemService, IItemTypeService _itemTypeService);
        RecoveryAccessoryDetail VDeleteObject(RecoveryAccessoryDetail recoveryAccessoryDetail);
        RecoveryAccessoryDetail VFinishObject(RecoveryAccessoryDetail recoveryAccessoryDetail);
        RecoveryAccessoryDetail VUnfinishObject(RecoveryAccessoryDetail recoveryAccessoryDetail);
        bool ValidCreateObject(RecoveryAccessoryDetail recoveryAccessoryDetail, IRecoveryOrderDetailService _recoveryOrderDetailService, IItemService _itemService, IItemTypeService _itemTypeService);
        bool ValidUpdateObject(RecoveryAccessoryDetail recoveryAccessoryDetail, IRecoveryOrderDetailService _recoveryOrderDetailService, IItemService _itemService, IItemTypeService _itemTypeService);
        bool ValidDeleteObject(RecoveryAccessoryDetail recoveryAccessoryDetail);
        bool ValidFinishObject(RecoveryAccessoryDetail recoveryAccessoryDetail);
        bool ValidUnfinishObject(RecoveryAccessoryDetail recoveryAccessoryDetail);
        bool isValid(RecoveryAccessoryDetail recoveryAccessoryDetail);
        string PrintError(RecoveryAccessoryDetail recoveryAccessoryDetail);
    }
}