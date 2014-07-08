using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface IBarringOrderDetailValidator
    {
        BarringOrderDetail VHasBarringOrder(BarringOrderDetail barringOrderDetail, IBarringOrderService _barringOrderService);
        BarringOrderDetail VHasBarring(BarringOrderDetail barringOrderDetail, IBarringService _barringService);
        BarringOrderDetail VHasLeftBar(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail VHasRightBar(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail VHasBeenCut(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail VHasBeenSideSealed(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail VHasBeenBarPrepared(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail VHasBeenAdhesiveTapeApplied(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail VHasBeenBarMounted(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail VHasBeenBarHeatPressed(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail VHasBeenBarPullOffTested(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail VHasBeenQCAndMarked(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail VHasBeenPackaged(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail VHasBeenRejected(BarringOrderDetail barringOrderDetail);

        BarringOrderDetail VHasNoLeftBar(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail VHasNoRightBar(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail VHasNotBeenCut(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail VHasNotBeenSideSealed(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail VHasNotBeenBarPrepared(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail VHasNotBeenAdhesiveTapeApplied(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail VHasNotBeenBarMounted(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail VHasNotBeenBarHeatPressed(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail VHasNotBeenBarPullOffTested(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail VHasNotBeenQCAndMarked(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail VHasNotBeenPackaged(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail VHasNotBeenRejected(BarringOrderDetail barringOrderDetail);

        BarringOrderDetail VBarringOrderHasNotBeenConfirmed(BarringOrderDetail barringOrderDetail, IBarringOrderService _barringOrderService);
        BarringOrderDetail VBarringOrderHasNotBeenFinished(BarringOrderDetail barringOrderDetail, IBarringOrderService _barringOrderService);

        BarringOrderDetail VCreateObject(BarringOrderDetail barringOrderDetail, IBarringOrderService _barringOrderService, IBarringService _barringService);
        BarringOrderDetail VUpdateObject(BarringOrderDetail barringOrderDetail, IBarringOrderService _barringOrderService, IBarringService _barringService);
        BarringOrderDetail VDeleteObject(BarringOrderDetail barringOrderDetail, IBarringOrderService _barringOrderService);

        BarringOrderDetail VAddLeftBar(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail VRemoveLeftBar(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail VAddRightBar(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail VRemoveRightBar(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail VCutObject(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail VSideSealObject(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail VPrepareObject(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail VApplyTapeAdhesiveToObject(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail VMountObject(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail VHeatPressObject(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail VPullOffTestObject(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail VQCAndMarkObject(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail VPackageObject(BarringOrderDetail barringOrderDetail);
        BarringOrderDetail VRejectObject(BarringOrderDetail barringOrderDetail, IBarringOrderService _barringOrderService);
        BarringOrderDetail VUndoRejectObject(BarringOrderDetail barringOrderDetail, IBarringOrderService _barringOrderService);

        bool ValidCreateObject(BarringOrderDetail barringOrderDetail, IBarringOrderService _barringOrderService, IBarringService _barringService);
        bool ValidUpdateObject(BarringOrderDetail barringOrderDetail, IBarringOrderService _barringOrderService, IBarringService _barringService);
        bool ValidDeleteObject(BarringOrderDetail barringOrderDetail, IBarringOrderService _barringOrderService);

        bool ValidAddLeftBar(BarringOrderDetail barringOrderDetail);
        bool ValidRemoveLeftBar(BarringOrderDetail barringOrderDetail);
        bool ValidAddRightBar(BarringOrderDetail barringOrderDetail);
        bool ValidRemoveRightBar(BarringOrderDetail barringOrderDetail);
        bool ValidCutObject(BarringOrderDetail barringOrderDetail);
        bool ValidSideSealObject(BarringOrderDetail barringOrderDetail);
        bool ValidPrepareObject(BarringOrderDetail barringOrderDetail);
        bool ValidApplyTapeAdhesiveToObject(BarringOrderDetail barringOrderDetail);
        bool ValidMountObject(BarringOrderDetail barringOrderDetail);
        bool ValidHeatPressObject(BarringOrderDetail barringOrderDetail);
        bool ValidPullOffTestObject(BarringOrderDetail barringOrderDetail);
        bool ValidQCAndMarkObject(BarringOrderDetail barringOrderDetail);
        bool ValidPackageObject(BarringOrderDetail barringOrderDetail);
        bool ValidRejectObject(BarringOrderDetail barringOrderDetail, IBarringOrderService _barringOrderService);
        bool ValidUndoRejectObject(BarringOrderDetail barringOrderDetail, IBarringOrderService _barringOrderService);

        bool isValid(BarringOrderDetail barringOrderDetail);
        string PrintError(BarringOrderDetail barringOrderDetail);
    }
}