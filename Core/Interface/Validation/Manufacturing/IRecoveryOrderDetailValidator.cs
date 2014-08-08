using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface IRecoveryOrderDetailValidator
    {
        RecoveryOrderDetail VHasRecoveryOrder(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService);
        RecoveryOrderDetail VHasCoreIdentificationDetail(RecoveryOrderDetail recoveryOrderDetail, ICoreIdentificationDetailService _coreIdentificationDetailService);
        RecoveryOrderDetail VHasRollerBuilder(RecoveryOrderDetail recoveryOrderDetail, IRollerBuilderService _rollerBuilderService);
        RecoveryOrderDetail VHasCoreTypeCase(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail VHasAcc(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail VHasRepairRequestCase(RecoveryOrderDetail recoveryOrderDetail);

        RecoveryOrderDetail VHasBeenDisassembled(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail VHasBeenStrippedAndGlued(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail VHasBeenWrapped(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail VCompoundUsageIsLargerThanZero(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail VHasBeenVulcanized(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail VHasBeenFacedOff(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail VHasBeenConventionalGrinded (RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail VHasBeenCWCGrinded(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail VHasBeenPolishedAndQC(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail VHasBeenPackaged(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail VHasBeenRejected(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail VHasBeenFinished(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail VHasFinishedDate(RecoveryOrderDetail recoveryOrderDetail);

        RecoveryOrderDetail VHasNotBeenDisassembled(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail VHasNotBeenStrippedAndGlued(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail VHasNotBeenWrapped(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail VHasNotBeenVulcanized(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail VHasNotBeenFacedOff(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail VHasNotBeenConventionalGrinded(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail VHasNotBeenCWCGrinded(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail VHasNotBeenPolishedAndQC(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail VHasNotBeenPackaged(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail VHasNotBeenRejected(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail VHasNotBeenFinished(RecoveryOrderDetail recoveryOrderDetail);

        RecoveryOrderDetail VRecoveryOrderHasNotBeenConfirmed(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService);
        RecoveryOrderDetail VRecoveryOrderHasNotBeenCompleted(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService);

        RecoveryOrderDetail VCreateObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                          IRollerBuilderService _rollerBuilderService);
        RecoveryOrderDetail VUpdateObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                          IRollerBuilderService _rollerBuilderService);
        RecoveryOrderDetail VDeleteObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService);
        RecoveryOrderDetail VFinishObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService);
        RecoveryOrderDetail VUnfinishObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService);

        // TODO: Recheck Accessory process since IsFinished is removed
        RecoveryOrderDetail VAddAccessory(RecoveryOrderDetail recoveryOrderDetail, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService);
        RecoveryOrderDetail VRemoveAccessory(RecoveryOrderDetail recoveryOrderDetail, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService);

        RecoveryOrderDetail VDisassembleObject(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail VStripAndGlueObject(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail VWrapObject(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail VVulcanizeObject(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail VFaceOffObject(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail VConventionalGrindObject(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail VCWCGrindObject(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail VPolishAndQCObject(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail VPackageObject(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail VRejectObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService);
        RecoveryOrderDetail VUndoRejectObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService);

        bool ValidCreateObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                          IRollerBuilderService _rollerBuilderService);
        bool ValidUpdateObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                          IRollerBuilderService _rollerBuilderService);
        bool ValidDeleteObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService);
        bool ValidFinishObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService);
        bool ValidUnfinishObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService);

        bool ValidAddAccessory(RecoveryOrderDetail recoveryOrderDetail, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService);
        bool ValidRemoveAccessory(RecoveryOrderDetail recoveryOrderDetail, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService);
        bool ValidDisassembleObject(RecoveryOrderDetail recoveryOrderDetail);
        bool ValidStripAndGlueObject(RecoveryOrderDetail recoveryOrderDetail);
        bool ValidWrapObject(RecoveryOrderDetail recoveryOrderDetail);
        bool ValidVulcanizeObject(RecoveryOrderDetail recoveryOrderDetail);
        bool ValidFaceOffObject(RecoveryOrderDetail recoveryOrderDetail);
        bool ValidConventionalGrindObject(RecoveryOrderDetail recoveryOrderDetail);
        bool ValidCWCGrindObject(RecoveryOrderDetail recoveryOrderDetail);
        bool ValidPolishAndQCObject(RecoveryOrderDetail recoveryOrderDetail);
        bool ValidPackageObject(RecoveryOrderDetail recoveryOrderDetail);
        bool ValidRejectObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService);
        bool ValidUndoRejectObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService);

        bool isValid(RecoveryOrderDetail recoveryOrderDetail);
        string PrintError(RecoveryOrderDetail recoveryOrderDetail);
    }
}