using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface IRecoveryOrderDetailValidator
    {
        RecoveryOrderDetail VHasRecoveryOrder(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService);
        RecoveryOrderDetail VHasCoreIdentificationDetail(RecoveryOrderDetail recoveryOrderDetail, ICoreIdentificationDetailService _coreIdentificationDetailService);
        RecoveryOrderDetail VHasRollerBuilder(RecoveryOrderDetail recoveryOrderDetail, ICoreBuilderService _rollerBuilderService);
        RecoveryOrderDetail VHasCoreTypeCase(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail VHasAcc(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail VHasRequestTypeCase(RecoveryOrderDetail recoveryOrderDetail);

        RecoveryOrderDetail VIsNotDissembled(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail VIsStrippedAndGlued(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail VIsWrapped(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail VIsVulcanized(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail VIsFacedOff(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail VIsConventionalGrinded (RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail VIsCWCGrinded(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail VIsPolishedAndQC(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail VIsPackaged(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail VIsRejected(RecoveryOrderDetail recoveryOrderDetail);

        RecoveryOrderDetail VIsDisassembled(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail VIsNotPackaged(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail VIsNotRejected(RecoveryOrderDetail recoveryOrderDetail);

        RecoveryOrderDetail VHasAccessories(RecoveryOrderDetail recoveryOrderDetail, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService);
        RecoveryOrderDetail VHasConfirmedAccessories(RecoveryOrderDetail recoveryOrderDetail, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService);

        RecoveryOrderDetail VCreateObject();
        RecoveryOrderDetail VUpdateObject();
        RecoveryOrderDetail VDeleteObject();
        RecoveryOrderDetail VAssembleObject();
        RecoveryOrderDetail VStripAndGlueObject();
        RecoveryOrderDetail VWrapObject();
        RecoveryOrderDetail VVulcanizeObject();
        RecoveryOrderDetail VFaceOffObject();
        RecoveryOrderDetail VConventionalGrindObject();
        RecoveryOrderDetail VCWCGrindObject();
        RecoveryOrderDetail VPolishAndQCObject();
        RecoveryOrderDetail VPackageObject();
        RecoveryOrderDetail VRejectObject();

        bool isValid(RecoveryOrderDetail recoveryOrderDetail);
        string PrintError(RecoveryOrderDetail recoveryOrderDetail);
    }
}