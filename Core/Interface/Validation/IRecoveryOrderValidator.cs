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
        RecoveryOrder VQuantityReceivedLessThanCoreIdentification(RecoveryOrder recoveryOrder, ICoreIdentificationService _coreIdentificationService);
        RecoveryOrder VQuantityReceivedEqualDetails(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService);
        RecoveryOrder VQuantityIsInStock(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService, ICoreBuilderService _coreBuilderService);
        RecoveryOrder VHasBeenConfirmed(RecoveryOrder recoveryOrder);
        RecoveryOrder VHasNotBeenConfirmed(RecoveryOrder recoveryOrder);
        RecoveryOrder VHasBeenFinished(RecoveryOrder recoveryOrder);
        RecoveryOrder VHasNotBeenFinished(RecoveryOrder recoveryOrder);
        RecoveryOrder VAllAccessoriesHaveNotBeenConfirmed(RecoveryOrder recoveryOrder, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService);
        RecoveryOrder VAllAccessoriesHaveBeenConfirmed(RecoveryOrder recoveryOrder, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService);
        RecoveryOrder VAllDetailsHaveBeenPackagedOrRejected(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService);
        RecoveryOrder VAllDetailsHaveBeenAssembledOrRejected(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService);
        RecoveryOrder VAllDetailsHaveNotBeenAssembledNorRejected(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService);
        
        RecoveryOrder VCreateObject(RecoveryOrder recoveryOrder, ICoreIdentificationService _coreIdentificationService, IRecoveryOrderService _recoveryOrderService);
        RecoveryOrder VUpdateObject(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService, ICoreIdentificationService _coreIdentificationService, IRecoveryOrderService _recoveryOrderService);
        RecoveryOrder VDeleteObject(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService);
        RecoveryOrder VConfirmObject(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService, ICoreBuilderService _coreBuilderService);
        RecoveryOrder VUnconfirmObject(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService);
        RecoveryOrder VFinishObject(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService);
        RecoveryOrder VUnfinishObject(RecoveryOrder recoveryOrder);

        bool ValidCreateObject(RecoveryOrder recoveryOrder, ICoreIdentificationService _coreIdentificationService, IRecoveryOrderService _recoveryOrderService);
        bool ValidUpdateObject(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService, ICoreIdentificationService _coreIdentificationService, IRecoveryOrderService _recoveryOrderService);
        bool ValidDeleteObject(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService);
        bool ValidConfirmObject(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService, ICoreBuilderService _coreBuilderService);
        bool ValidUnconfirmObject(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService);
        bool ValidFinishObject(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService);
        bool ValidUnfinishObject(RecoveryOrder recoveryOrder);
        bool isValid(RecoveryOrder recoveryOrder);
        string PrintError(RecoveryOrder recoveryOrder);
    }
}