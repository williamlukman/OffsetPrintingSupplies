using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface IBlanketOrderDetailValidator
    {
        BlanketOrderDetail VHasBlanketOrder(BlanketOrderDetail blanketOrderDetail, IBlanketOrderService _blanketOrderService);
        BlanketOrderDetail VHasBlanket(BlanketOrderDetail blanketOrderDetail, IBlanketService _blanketService);
        BlanketOrderDetail VHasBeenCut(BlanketOrderDetail blanketOrderDetail);
        BlanketOrderDetail VHasBeenSideSealed(BlanketOrderDetail blanketOrderDetail);
        BlanketOrderDetail VHasBeenBarPrepared(BlanketOrderDetail blanketOrderDetail);
        BlanketOrderDetail VHasRollBlanketAmount(BlanketOrderDetail blanketOrderDetail);

        BlanketOrderDetail VHasBeenAdhesiveTapeApplied(BlanketOrderDetail blanketOrderDetail);
        BlanketOrderDetail VHasBeenBarMounted(BlanketOrderDetail blanketOrderDetail);
        BlanketOrderDetail VHasBeenBarHeatPressed(BlanketOrderDetail blanketOrderDetail);
        BlanketOrderDetail VHasBeenBarPullOffTested(BlanketOrderDetail blanketOrderDetail);
        BlanketOrderDetail VHasBeenQCAndMarked(BlanketOrderDetail blanketOrderDetail);
        BlanketOrderDetail VHasBeenPackaged(BlanketOrderDetail blanketOrderDetail);
        BlanketOrderDetail VHasBeenRejected(BlanketOrderDetail blanketOrderDetail);
        BlanketOrderDetail VHasRejectedDate(BlanketOrderDetail blanketOrderDetail);
        BlanketOrderDetail VHasBeenFinished(BlanketOrderDetail blanketOrderDetail);
        BlanketOrderDetail VHasFinishedDate(BlanketOrderDetail blanketOrderDetail);

        BlanketOrderDetail VHasNotBeenCut(BlanketOrderDetail blanketOrderDetail);
        BlanketOrderDetail VHasNotBeenSideSealed(BlanketOrderDetail blanketOrderDetail);
        BlanketOrderDetail VHasNotBeenBarPrepared(BlanketOrderDetail blanketOrderDetail);
        BlanketOrderDetail VHasNotBeenAdhesiveTapeApplied(BlanketOrderDetail blanketOrderDetail);
        BlanketOrderDetail VHasNotBeenBarMounted(BlanketOrderDetail blanketOrderDetail);
        BlanketOrderDetail VHasNotBeenBarHeatPressed(BlanketOrderDetail blanketOrderDetail);
        BlanketOrderDetail VHasNotBeenBarPullOffTested(BlanketOrderDetail blanketOrderDetail);
        BlanketOrderDetail VHasNotBeenQCAndMarked(BlanketOrderDetail blanketOrderDetail);
        BlanketOrderDetail VHasNotBeenPackaged(BlanketOrderDetail blanketOrderDetail);
        BlanketOrderDetail VHasNotBeenRejected(BlanketOrderDetail blanketOrderDetail);
        BlanketOrderDetail VHasNotBeenFinished(BlanketOrderDetail blanketOrderDetail);
        BlanketOrderDetail VBlanketOrderHasNotBeenConfirmed(BlanketOrderDetail blanketOrderDetail, IBlanketOrderService _blanketOrderService);
        BlanketOrderDetail VBlanketOrderHasNotBeenCompleted(BlanketOrderDetail blanketOrderDetail, IBlanketOrderService _blanketOrderService);

        BlanketOrderDetail VCreateObject(BlanketOrderDetail blanketOrderDetail, IBlanketOrderService _blanketOrderService, IBlanketService _blanketService);
        BlanketOrderDetail VUpdateObject(BlanketOrderDetail blanketOrderDetail, IBlanketOrderService _blanketOrderService, IBlanketService _blanketService);
        BlanketOrderDetail VDeleteObject(BlanketOrderDetail blanketOrderDetail, IBlanketOrderService _blanketOrderService);
        BlanketOrderDetail VFinishObject(BlanketOrderDetail blanketOrderDetail, IBlanketOrderService _blanketOrderService);
        BlanketOrderDetail VUnfinishObject(BlanketOrderDetail blanketOrderDetail, IBlanketOrderService _blanketOrderService);

        BlanketOrderDetail VCutObject(BlanketOrderDetail blanketOrderDetail, IBlanketOrderService _blanketOrderService);
        BlanketOrderDetail VSideSealObject(BlanketOrderDetail blanketOrderDetail);
        BlanketOrderDetail VPrepareObject(BlanketOrderDetail blanketOrderDetail, IBlanketService _blanketService);
        BlanketOrderDetail VApplyTapeAdhesiveToObject(BlanketOrderDetail blanketOrderDetail, IBlanketService _blanketService);
        BlanketOrderDetail VMountObject(BlanketOrderDetail blanketOrderDetail, IBlanketService _blanketService);
        BlanketOrderDetail VHeatPressObject(BlanketOrderDetail blanketOrderDetail, IBlanketService _blanketService);
        BlanketOrderDetail VPullOffTestObject(BlanketOrderDetail blanketOrderDetail, IBlanketService _blanketService);
        BlanketOrderDetail VQCAndMarkObject(BlanketOrderDetail blanketOrderDetail, IBlanketService _blanketService);
        BlanketOrderDetail VPackageObject(BlanketOrderDetail blanketOrderDetail);
        BlanketOrderDetail VRejectObject(BlanketOrderDetail blanketOrderDetail, IBlanketOrderService _blanketOrderService);
        BlanketOrderDetail VUndoRejectObject(BlanketOrderDetail blanketOrderDetail, IBlanketOrderService _blanketOrderService);

        bool ValidCreateObject(BlanketOrderDetail blanketOrderDetail, IBlanketOrderService _blanketOrderService, IBlanketService _blanketService);
        bool ValidUpdateObject(BlanketOrderDetail blanketOrderDetail, IBlanketOrderService _blanketOrderService, IBlanketService _blanketService);
        bool ValidDeleteObject(BlanketOrderDetail blanketOrderDetail, IBlanketOrderService _blanketOrderService);
        bool ValidFinishObject(BlanketOrderDetail blanketOrderDetail, IBlanketOrderService _blanketOrderService);
        bool ValidUnfinishObject(BlanketOrderDetail blanketOrderDetail, IBlanketOrderService _blanketOrderService);

        bool ValidCutObject(BlanketOrderDetail blanketOrderDetail, IBlanketOrderService _blanketOrderSerivce);
        bool ValidSideSealObject(BlanketOrderDetail blanketOrderDetail);
        bool ValidPrepareObject(BlanketOrderDetail blanketOrderDetail, IBlanketService _blanketService);
        bool ValidApplyTapeAdhesiveToObject(BlanketOrderDetail blanketOrderDetail, IBlanketService _blanketService);
        bool ValidMountObject(BlanketOrderDetail blanketOrderDetail, IBlanketService _blanketService);
        bool ValidHeatPressObject(BlanketOrderDetail blanketOrderDetail, IBlanketService _blanketService);
        bool ValidPullOffTestObject(BlanketOrderDetail blanketOrderDetail, IBlanketService _blanketService);
        bool ValidQCAndMarkObject(BlanketOrderDetail blanketOrderDetail, IBlanketService _blanketService);
        bool ValidPackageObject(BlanketOrderDetail blanketOrderDetail);
        bool ValidRejectObject(BlanketOrderDetail blanketOrderDetail, IBlanketOrderService _blanketOrderService);
        bool ValidUndoRejectObject(BlanketOrderDetail blanketOrderDetail, IBlanketOrderService _blanketOrderService);

        bool isValid(BlanketOrderDetail blanketOrderDetail);
        string PrintError(BlanketOrderDetail blanketOrderDetail);
    }
}