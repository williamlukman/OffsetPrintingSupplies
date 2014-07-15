using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Validation.Validation
{
    public class BarringOrderDetailValidator : IBarringOrderDetailValidator
    {
        public BarringOrderDetail VHasBarringOrder(BarringOrderDetail barringOrderDetail, IBarringOrderService _barringOrderService)
        {
            BarringOrder barringOrder = _barringOrderService.GetObjectById(barringOrderDetail.BarringOrderId);
            if (barringOrder == null)
            {
                barringOrderDetail.Errors.Add("BarringOrderId", "Tidak terasosiasi dengan Barring Order");
            }
            return barringOrderDetail;
        }

        public BarringOrderDetail VHasBarring(BarringOrderDetail barringOrderDetail, IBarringService _barringService)
        {
            Barring barring = _barringService.GetObjectById(barringOrderDetail.BarringId);
            if (barring == null)
            {
                barringOrderDetail.Errors.Add("BarringId", "Tidak terasosiasi dengan Barring");
            }
            return barringOrderDetail;
        }

        public BarringOrderDetail VIsBarRequiredMustBeFalse(BarringOrderDetail barringOrderDetail)
        {
            if (barringOrderDetail.IsBarRequired == true)
            {
                barringOrderDetail.Errors.Add("Generic", "Tidak bisa menambahkan bar untuk Barring Order yang tidak memerlukan bar");
            }
            return barringOrderDetail;
        }

        public BarringOrderDetail VHasLeftBar(BarringOrderDetail barringOrderDetail)
        {
            if (!barringOrderDetail.HasLeftBar)
            {
                barringOrderDetail.Errors.Add("Generic", "LeftBar tidak ada");
            }
            return barringOrderDetail;
        }

        public BarringOrderDetail VHasRightBar(BarringOrderDetail barringOrderDetail)
        {
            if (!barringOrderDetail.HasRightBar)
            {
                barringOrderDetail.Errors.Add("Generic", "RightBar tidak ada");
            }
            return barringOrderDetail;
        }

        public BarringOrderDetail VHasBeenCut(BarringOrderDetail barringOrderDetail)
        {
            if (!barringOrderDetail.IsCut)
            {
                barringOrderDetail.Errors.Add("Generic", "Belum di cut");
            }
            return barringOrderDetail;
        }

        public BarringOrderDetail VHasBeenSideSealed(BarringOrderDetail barringOrderDetail)
        {
            if (!barringOrderDetail.IsSideSealed)
            {
                barringOrderDetail.Errors.Add("Generic", "Belum di side seal");
            }
            return barringOrderDetail;
        }

        public BarringOrderDetail VHasBeenBarPrepared(BarringOrderDetail barringOrderDetail)
        {
            if (!barringOrderDetail.IsBarPrepared)
            {
                barringOrderDetail.Errors.Add("Generic", "Bar belum di prepare");
            }
            return barringOrderDetail;
        }

        public BarringOrderDetail VHasBeenAdhesiveTapeApplied(BarringOrderDetail barringOrderDetail)
        {
            if (!barringOrderDetail.IsAdhesiveTapeApplied)
            {
                barringOrderDetail.Errors.Add("Generic", "Belum di adhesive tape");
            }
            return barringOrderDetail;
        }

        public BarringOrderDetail VHasBeenBarMounted(BarringOrderDetail barringOrderDetail)
        {
            if (!barringOrderDetail.IsBarMounted)
            {
                barringOrderDetail.Errors.Add("Generic", "Bar belum di mount");
            }
            return barringOrderDetail;
        }

        public BarringOrderDetail VHasBeenBarHeatPressed(BarringOrderDetail barringOrderDetail)
        {
            if (!barringOrderDetail.IsBarHeatPressed)
            {
                barringOrderDetail.Errors.Add("Generic", "Bar belum di heat press");
            }
            return barringOrderDetail;
        }

        public BarringOrderDetail VHasBeenBarPullOffTested(BarringOrderDetail barringOrderDetail)
        {
            if (!barringOrderDetail.IsBarPullOffTested)
            {
                barringOrderDetail.Errors.Add("Generic", "Bar belum di pull-off test");
            }
            return barringOrderDetail;
        }

        public BarringOrderDetail VHasBeenQCAndMarked(BarringOrderDetail barringOrderDetail)
        {
            if (!barringOrderDetail.IsQCAndMarked)
            {
                barringOrderDetail.Errors.Add("Generic", "Belum di QC dan mark");
            }
            return barringOrderDetail;
        }

        public BarringOrderDetail VHasBeenPackaged(BarringOrderDetail barringOrderDetail)
        {
            if (!barringOrderDetail.IsPackaged)
            {
                barringOrderDetail.Errors.Add("Generic", "Belum di package");
            }
            return barringOrderDetail;
        }

        public BarringOrderDetail VHasBeenRejected(BarringOrderDetail barringOrderDetail)
        {
            if (!barringOrderDetail.IsRejected)
            {
                barringOrderDetail.Errors.Add("Generic", "Belum di reject");
            }
            return barringOrderDetail;
        }

        public BarringOrderDetail VHasNoLeftBar(BarringOrderDetail barringOrderDetail)
        {
            if (barringOrderDetail.HasLeftBar)
            {
                barringOrderDetail.Errors.Add("Generic", "LeftBar tidak boleh ada");
            }
            return barringOrderDetail;
        }

        public BarringOrderDetail VHasNoRightBar(BarringOrderDetail barringOrderDetail)
        {
            if (barringOrderDetail.HasRightBar)
            {
                barringOrderDetail.Errors.Add("Generic", "RightBar tidak boleh ada");
            }
            return barringOrderDetail;
        }

        public BarringOrderDetail VHasNotBeenCut(BarringOrderDetail barringOrderDetail)
        {
            if (barringOrderDetail.IsCut)
            {
                barringOrderDetail.Errors.Add("Generic", "Sudah di cut");
            }
            return barringOrderDetail;
        }

        public BarringOrderDetail VHasNotBeenSideSealed(BarringOrderDetail barringOrderDetail)
        {
            if (barringOrderDetail.IsSideSealed)
            {
                barringOrderDetail.Errors.Add("Generic", "Sudah di side seal");
            }
            return barringOrderDetail;
        }

        public BarringOrderDetail VHasNotBeenBarPrepared(BarringOrderDetail barringOrderDetail)
        {
            if (barringOrderDetail.IsBarPrepared)
            {
                barringOrderDetail.Errors.Add("Generic", "Bar belum di prepare");
            }
            return barringOrderDetail;
        }

        public BarringOrderDetail VHasNotBeenAdhesiveTapeApplied(BarringOrderDetail barringOrderDetail)
        {
            if (barringOrderDetail.IsAdhesiveTapeApplied)
            {
                barringOrderDetail.Errors.Add("Generic", "Sudah di adhesive tape");
            }
            return barringOrderDetail;
        }

        public BarringOrderDetail VHasNotBeenBarMounted(BarringOrderDetail barringOrderDetail)
        {
            if (barringOrderDetail.IsBarMounted)
            {
                barringOrderDetail.Errors.Add("Generic", "Bar belum di mount");
            }
            return barringOrderDetail;
        }

        public BarringOrderDetail VHasNotBeenBarHeatPressed(BarringOrderDetail barringOrderDetail)
        {
            if (barringOrderDetail.IsBarHeatPressed)
            {
                barringOrderDetail.Errors.Add("Generic", "Bar sudah di heat press");
            }
            return barringOrderDetail;
        }

        public BarringOrderDetail VHasNotBeenBarPullOffTested(BarringOrderDetail barringOrderDetail)
        {
            if (barringOrderDetail.IsBarPullOffTested)
            {
                barringOrderDetail.Errors.Add("Generic", "Bar sudah di pull-off test");
            }
            return barringOrderDetail;
        }

        public BarringOrderDetail VHasNotBeenQCAndMarked(BarringOrderDetail barringOrderDetail)
        {
            if (barringOrderDetail.IsQCAndMarked)
            {
                barringOrderDetail.Errors.Add("Generic", "Sudah di QC dan mark");
            }
            return barringOrderDetail;
        }

        public BarringOrderDetail VHasNotBeenPackaged(BarringOrderDetail barringOrderDetail)
        {
            if (barringOrderDetail.IsPackaged)
            {
                barringOrderDetail.Errors.Add("Generic", "Sudah di package");
            }
            return barringOrderDetail;
        }

        public BarringOrderDetail VHasNotBeenRejected(BarringOrderDetail barringOrderDetail)
        {
            if (barringOrderDetail.IsRejected)
            {
                barringOrderDetail.Errors.Add("Generic", "Sudah di reject");
            }
            return barringOrderDetail;
        }

        public BarringOrderDetail VBarringOrderHasNotBeenConfirmed(BarringOrderDetail barringOrderDetail, IBarringOrderService _barringOrderService)
        {
            BarringOrder barringOrder = _barringOrderService.GetObjectById(barringOrderDetail.BarringOrderId);
            if (barringOrder.IsConfirmed)
            {
                barringOrderDetail.Errors.Add("Generic", "Barring Order sudah dikonfirmasi");
            }
            return barringOrderDetail;
        }

        public BarringOrderDetail VBarringOrderHasNotBeenFinished(BarringOrderDetail barringOrderDetail, IBarringOrderService _barringOrderService)
        {
            BarringOrder barringOrder = _barringOrderService.GetObjectById(barringOrderDetail.BarringOrderId);
            if (barringOrder.IsFinished)
            {
                barringOrderDetail.Errors.Add("Generic", "Barring order sudah di finish");
            }
            return barringOrderDetail;
        }

        public BarringOrderDetail VCreateObject(BarringOrderDetail barringOrderDetail, IBarringOrderService _barringOrderService, IBarringService _barringService)
        {
            VHasBarringOrder(barringOrderDetail, _barringOrderService);
            if (!isValid(barringOrderDetail)) { return barringOrderDetail; }
            VHasBarring(barringOrderDetail, _barringService);
            return barringOrderDetail;
        }

        public BarringOrderDetail VUpdateObject(BarringOrderDetail barringOrderDetail, IBarringOrderService _barringOrderService, IBarringService _barringService)
        {
            VHasNotBeenCut(barringOrderDetail);
            if (!isValid(barringOrderDetail)) { return barringOrderDetail; }
            VHasNotBeenRejected(barringOrderDetail);
            if (!isValid(barringOrderDetail)) { return barringOrderDetail; }
            VCreateObject(barringOrderDetail, _barringOrderService, _barringService);
            return barringOrderDetail;
        }

        public BarringOrderDetail VDeleteObject(BarringOrderDetail barringOrderDetail, IBarringOrderService _barringOrderService)
        {
            VHasNotBeenCut(barringOrderDetail);
            if (!isValid(barringOrderDetail)) { return barringOrderDetail; }
            VHasNotBeenRejected(barringOrderDetail);
            if (!isValid(barringOrderDetail)) { return barringOrderDetail; }
            VBarringOrderHasNotBeenConfirmed(barringOrderDetail, _barringOrderService);
            return barringOrderDetail;
        }

        public BarringOrderDetail VAddLeftBar(BarringOrderDetail barringOrderDetail)
        {
            VIsBarRequiredMustBeFalse(barringOrderDetail);
            if (!isValid(barringOrderDetail)) { return barringOrderDetail; }
            VHasNoLeftBar(barringOrderDetail);
            return barringOrderDetail;
        }

        public BarringOrderDetail VRemoveLeftBar(BarringOrderDetail barringOrderDetail)
        {
            VHasLeftBar(barringOrderDetail);
            return barringOrderDetail;
        }

        public BarringOrderDetail VAddRightBar(BarringOrderDetail barringOrderDetail)
        {
            VIsBarRequiredMustBeFalse(barringOrderDetail);
            if (!isValid(barringOrderDetail)) { return barringOrderDetail; }
            VHasNoRightBar(barringOrderDetail);
            return barringOrderDetail;
        }

        public BarringOrderDetail VRemoveRightBar(BarringOrderDetail barringOrderDetail)
        {
            VHasRightBar(barringOrderDetail);
            return barringOrderDetail;
        }

        public BarringOrderDetail VCutObject(BarringOrderDetail barringOrderDetail)
        {
            VHasNotBeenCut(barringOrderDetail);
            if (!isValid(barringOrderDetail)) { return barringOrderDetail; }
            VHasNotBeenRejected(barringOrderDetail);
            return barringOrderDetail;
        }

        public BarringOrderDetail VSideSealObject(BarringOrderDetail barringOrderDetail)
        {
            VHasNotBeenSideSealed(barringOrderDetail);
            if (!isValid(barringOrderDetail)) { return barringOrderDetail; }
            VHasBeenCut(barringOrderDetail);
            if (!isValid(barringOrderDetail)) { return barringOrderDetail; }
            VHasNotBeenRejected(barringOrderDetail);
            return barringOrderDetail;
        }

        public BarringOrderDetail VPrepareObject(BarringOrderDetail barringOrderDetail)
        {
            VHasNotBeenBarPrepared(barringOrderDetail);
            if (!isValid(barringOrderDetail)) { return barringOrderDetail; }
            VHasBeenSideSealed(barringOrderDetail);
            if (!isValid(barringOrderDetail)) { return barringOrderDetail; }
            VHasNotBeenRejected(barringOrderDetail);
            return barringOrderDetail;
        }

        public BarringOrderDetail VApplyTapeAdhesiveToObject(BarringOrderDetail barringOrderDetail)
        {
            VHasNotBeenAdhesiveTapeApplied(barringOrderDetail);
            if (!isValid(barringOrderDetail)) { return barringOrderDetail; }
            VHasBeenBarPrepared(barringOrderDetail);
            if (!isValid(barringOrderDetail)) { return barringOrderDetail; }
            VHasNotBeenRejected(barringOrderDetail);
            return barringOrderDetail;
        }

        public BarringOrderDetail VMountObject(BarringOrderDetail barringOrderDetail)
        {
            VHasNotBeenBarMounted(barringOrderDetail);
            if (!isValid(barringOrderDetail)) { return barringOrderDetail; }
            VHasBeenAdhesiveTapeApplied(barringOrderDetail);
            if (!isValid(barringOrderDetail)) { return barringOrderDetail; }
            VHasNotBeenRejected(barringOrderDetail);
            return barringOrderDetail;
        }

        public BarringOrderDetail VHeatPressObject(BarringOrderDetail barringOrderDetail)
        {
            VHasNotBeenBarHeatPressed(barringOrderDetail);
            if (!isValid(barringOrderDetail)) { return barringOrderDetail; }
            VHasBeenBarMounted(barringOrderDetail);
            if (!isValid(barringOrderDetail)) { return barringOrderDetail; }
            VHasNotBeenRejected(barringOrderDetail);
            return barringOrderDetail;
        }

        public BarringOrderDetail VPullOffTestObject(BarringOrderDetail barringOrderDetail)
        {
            VHasNotBeenBarPullOffTested(barringOrderDetail);
            if (!isValid(barringOrderDetail)) { return barringOrderDetail; }
            VHasBeenBarHeatPressed(barringOrderDetail);
            if (!isValid(barringOrderDetail)) { return barringOrderDetail; }
            VHasNotBeenRejected(barringOrderDetail);
            return barringOrderDetail;
        }

        public BarringOrderDetail VQCAndMarkObject(BarringOrderDetail barringOrderDetail)
        {
            VHasNotBeenQCAndMarked(barringOrderDetail);
            if (!isValid(barringOrderDetail)) { return barringOrderDetail; }
            VHasBeenBarPullOffTested(barringOrderDetail);
            if (!isValid(barringOrderDetail)) { return barringOrderDetail; }
            VHasNotBeenRejected(barringOrderDetail);
            return barringOrderDetail;
        }

        public BarringOrderDetail VPackageObject(BarringOrderDetail barringOrderDetail)
        {
            VHasNotBeenPackaged(barringOrderDetail);
            if (!isValid(barringOrderDetail)) { return barringOrderDetail; }
            VHasBeenQCAndMarked(barringOrderDetail);
            if (!isValid(barringOrderDetail)) { return barringOrderDetail; }
            VHasNotBeenRejected(barringOrderDetail);
            return barringOrderDetail;
        }

        public BarringOrderDetail VRejectObject(BarringOrderDetail barringOrderDetail, IBarringOrderService _barringOrderService)
        {
            VHasNotBeenRejected(barringOrderDetail);
            if (!isValid(barringOrderDetail)) { return barringOrderDetail; }
            VBarringOrderHasNotBeenFinished(barringOrderDetail, _barringOrderService);
            return barringOrderDetail;
        }

        public BarringOrderDetail VUndoRejectObject(BarringOrderDetail barringOrderDetail, IBarringOrderService _barringOrderService)
        {
            VHasBeenRejected(barringOrderDetail);
            if (!isValid(barringOrderDetail)) { return barringOrderDetail; }
            VBarringOrderHasNotBeenFinished(barringOrderDetail, _barringOrderService);
            return barringOrderDetail;
        }

        public bool ValidCreateObject(BarringOrderDetail barringOrderDetail, IBarringOrderService _barringOrderService, IBarringService _barringService)
        {
            VCreateObject(barringOrderDetail, _barringOrderService, _barringService);
            return isValid(barringOrderDetail);
        }

        public bool ValidUpdateObject(BarringOrderDetail barringOrderDetail, IBarringOrderService _barringOrderService, IBarringService _barringService)
        {
            barringOrderDetail.Errors.Clear();
            VUpdateObject(barringOrderDetail, _barringOrderService, _barringService);
            return isValid(barringOrderDetail);
        }

        public bool ValidDeleteObject(BarringOrderDetail barringOrderDetail, IBarringOrderService _barringOrderService)
        {
            barringOrderDetail.Errors.Clear();
            VDeleteObject(barringOrderDetail, _barringOrderService);
            return isValid(barringOrderDetail);
        }

        public bool ValidAddLeftBar(BarringOrderDetail barringOrderDetail)
        {
            barringOrderDetail.Errors.Clear();
            VAddLeftBar(barringOrderDetail);
            return isValid(barringOrderDetail);
        }

        public bool ValidRemoveLeftBar(BarringOrderDetail barringOrderDetail)
        {
            barringOrderDetail.Errors.Clear();
            VRemoveLeftBar(barringOrderDetail);
            return isValid(barringOrderDetail);
        }

        public bool ValidAddRightBar(BarringOrderDetail barringOrderDetail)
        {
            barringOrderDetail.Errors.Clear();
            VAddRightBar(barringOrderDetail);
            return isValid(barringOrderDetail);
        }

        public bool ValidRemoveRightBar(BarringOrderDetail barringOrderDetail)
        {
            barringOrderDetail.Errors.Clear();
            VRemoveRightBar(barringOrderDetail);
            return isValid(barringOrderDetail);
        }

        public bool ValidCutObject(BarringOrderDetail barringOrderDetail)
        {
            barringOrderDetail.Errors.Clear();
            VCutObject(barringOrderDetail);
            return isValid(barringOrderDetail);
        }

        public bool ValidSideSealObject(BarringOrderDetail barringOrderDetail)
        {
            barringOrderDetail.Errors.Clear();
            VSideSealObject(barringOrderDetail);
            return isValid(barringOrderDetail);
        }

        public bool ValidPrepareObject(BarringOrderDetail barringOrderDetail)
        {
            barringOrderDetail.Errors.Clear();
            VPrepareObject(barringOrderDetail);
            return isValid(barringOrderDetail);
        }

        public bool ValidApplyTapeAdhesiveToObject(BarringOrderDetail barringOrderDetail)
        {
            barringOrderDetail.Errors.Clear();
            VApplyTapeAdhesiveToObject(barringOrderDetail);
            return isValid(barringOrderDetail);
        }

        public bool ValidMountObject(BarringOrderDetail barringOrderDetail)
        {
            barringOrderDetail.Errors.Clear();
            VMountObject(barringOrderDetail);
            return isValid(barringOrderDetail);
        }

        public bool ValidHeatPressObject(BarringOrderDetail barringOrderDetail)
        {
            barringOrderDetail.Errors.Clear();
            VHeatPressObject(barringOrderDetail);
            return isValid(barringOrderDetail);
        }

        public bool ValidPullOffTestObject(BarringOrderDetail barringOrderDetail)
        {
            barringOrderDetail.Errors.Clear();
            VPullOffTestObject(barringOrderDetail);
            return isValid(barringOrderDetail);
        }

        public bool ValidQCAndMarkObject(BarringOrderDetail barringOrderDetail)
        {
            barringOrderDetail.Errors.Clear();
            VQCAndMarkObject(barringOrderDetail);
            return isValid(barringOrderDetail);
        }
        
        public bool ValidPackageObject(BarringOrderDetail barringOrderDetail)
        {
            barringOrderDetail.Errors.Clear();
            VPackageObject(barringOrderDetail);
            return isValid(barringOrderDetail);
        }

        public bool ValidRejectObject(BarringOrderDetail barringOrderDetail, IBarringOrderService _barringOrderService)
        {
            barringOrderDetail.Errors.Clear();
            VRejectObject(barringOrderDetail, _barringOrderService);
            return isValid(barringOrderDetail);
        }

        public bool ValidUndoRejectObject(BarringOrderDetail barringOrderDetail, IBarringOrderService _barringOrderService)
        {
            barringOrderDetail.Errors.Clear();
            VUndoRejectObject(barringOrderDetail, _barringOrderService);
            return isValid(barringOrderDetail);
        }

        public bool isValid(BarringOrderDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(BarringOrderDetail obj)
        {
            string erroroutput = "";
            KeyValuePair<string, string> first = obj.Errors.ElementAt(0);
            erroroutput += first.Key + "," + first.Value;
            foreach (KeyValuePair<string, string> pair in obj.Errors.Skip(1))
            {
                erroroutput += Environment.NewLine;
                erroroutput += pair.Key + "," + pair.Value;
            }
            return erroroutput;
        }
    }
}
