using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface IRecoveryOrderValidator
    {
        RecoveryOrder VHasUniqueCode(RecoveryOrder recoveryOrder, IRecoveryOrderService _recoveryOrderService);
        RecoveryOrder VHasCoreIdentificationAndConfirmed(RecoveryOrder recoveryOrder, ICoreIdentificationService _coreIdentificationService);
        RecoveryOrder VHasRecoveryOrderDetails(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService);
        RecoveryOrder VHasQuantityReceived(RecoveryOrder recoveryOrder);
        RecoveryOrder VQuantityFinalAndRejectedIsLessThanOrEqualQuantityReceived(RecoveryOrder recoveryOrder);
        RecoveryOrder VQuantityReceivedLessThanCoreIdentification(RecoveryOrder recoveryOrder, ICoreIdentificationService _coreIdentificationService);
        RecoveryOrder VQuantityReceivedEqualDetails(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService);
        RecoveryOrder VQuantityIsInStock(RecoveryOrder recoveryOrder, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                         IRecoveryOrderDetailService _recoveryOrderDetailService, ICoreBuilderService _coreBuilderService, IItemService _itemService,
                                         IWarehouseItemService _warehouseItemService);
        RecoveryOrder VHasBeenConfirmed(RecoveryOrder recoveryOrder);
        RecoveryOrder VHasNotBeenConfirmed(RecoveryOrder recoveryOrder);
        RecoveryOrder VHasBeenCompleted(RecoveryOrder recoveryOrder);
        RecoveryOrder VHasNotBeenCompleted(RecoveryOrder recoveryOrder);
        RecoveryOrder VAllAccessoriesHaveNotBeenFinished(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService);
        RecoveryOrder VAllAccessoriesHaveBeenFinished(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService);
        RecoveryOrder VAllDetailsHaveBeenFinishedOrRejected(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService);
        RecoveryOrder VAllDetailsHaveNotBeenDisassembledNorRejected(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService);
        
        RecoveryOrder VCreateObject(RecoveryOrder recoveryOrder, ICoreIdentificationService _coreIdentificationService, IRecoveryOrderService _recoveryOrderService);
        RecoveryOrder VUpdateObject(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService, ICoreIdentificationService _coreIdentificationService, IRecoveryOrderService _recoveryOrderService);
        RecoveryOrder VDeleteObject(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService);
        RecoveryOrder VConfirmObject(RecoveryOrder recoveryOrder, ICoreIdentificationDetailService _coreIdentificationDetailService, IRecoveryOrderDetailService _recoveryOrderDetailService,
                                     ICoreBuilderService _coreBuilderService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        RecoveryOrder VUnconfirmObject(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService);
        RecoveryOrder VCompleteObject(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService);
        RecoveryOrder VAdjustQuantity(RecoveryOrder recoveryOrder);

        bool ValidCreateObject(RecoveryOrder recoveryOrder, ICoreIdentificationService _coreIdentificationService, IRecoveryOrderService _recoveryOrderService);
        bool ValidUpdateObject(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService, ICoreIdentificationService _coreIdentificationService, IRecoveryOrderService _recoveryOrderService);
        bool ValidDeleteObject(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService);
        bool ValidConfirmObject(RecoveryOrder recoveryOrder, ICoreIdentificationDetailService _coreIdentificationDetailService, IRecoveryOrderDetailService _recoveryOrderDetailService,
                                ICoreBuilderService _coreBuilderService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        bool ValidUnconfirmObject(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService);
        bool ValidCompleteObject(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService);
        bool ValidAdjustQuantity(RecoveryOrder recoveryOrder);
        bool isValid(RecoveryOrder recoveryOrder);
        string PrintError(RecoveryOrder recoveryOrder);
    }
}