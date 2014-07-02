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
        RecoveryOrderDetail VHasRollerBuilder(RecoveryOrderDetail recoveryOrderDetail, IRollerBuilderService _rollerBuilderService);
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
        RecoveryOrderDetail VIsConfirmedRecoveryOrder(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService);
        RecoveryOrderDetail VRecoveryOrderConfirmDetail(RecoveryOrderDetail recoveryOrderDetail, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService);

        RecoveryOrderDetail VCreateObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                          IRollerBuilderService _rollerBuilderService);
        RecoveryOrderDetail VUpdateObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                          IRollerBuilderService _rollerBuilderService);
        RecoveryOrderDetail VDeleteObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService, IRecoveryAccessoryDetailService _recoveryAccessoryAccessoryDetailService);

        RecoveryOrderDetail VAssembleObject(RecoveryOrderDetail recoveryOrderDetail);
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
        bool ValidDeleteObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService, IRecoveryAccessoryDetailService _recoveryAccessoryAccessoryDetailService);

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