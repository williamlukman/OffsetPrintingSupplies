using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class BlanketOrderDetailValidator : IBlanketOrderDetailValidator
    {
        public BlanketOrderDetail VHasBlanketOrder(BlanketOrderDetail blanketOrderDetail, IBlanketOrderService _blanketOrderService)
        {
            BlanketOrder blanketOrder = _blanketOrderService.GetObjectById(blanketOrderDetail.BlanketOrderId);
            if (blanketOrder == null)
            {
                blanketOrderDetail.Errors.Add("BlanketOrderId", "Tidak terasosiasi dengan Blanket Order");
            }
            return blanketOrderDetail;
        }

        public BlanketOrderDetail VHasBlanket(BlanketOrderDetail blanketOrderDetail, IBlanketService _blanketService)
        {
            Blanket blanket = _blanketService.GetObjectById(blanketOrderDetail.BlanketId);
            if (blanket == null)
            {
                blanketOrderDetail.Errors.Add("BlanketId", "Tidak terasosiasi dengan Blanket");
            }
            return blanketOrderDetail;
        }

        public BlanketOrderDetail VHasBeenCut(BlanketOrderDetail blanketOrderDetail)
        {
            if (!blanketOrderDetail.IsCut)
            {
                blanketOrderDetail.Errors.Add("Generic", "Belum di cut");
            }
            return blanketOrderDetail;
        }

        public BlanketOrderDetail VHasBeenSideSealed(BlanketOrderDetail blanketOrderDetail)
        {
            if (!blanketOrderDetail.IsSideSealed)
            {
                blanketOrderDetail.Errors.Add("Generic", "Belum di side seal");
            }
            return blanketOrderDetail;
        }

        public BlanketOrderDetail VHasBeenBarPrepared(BlanketOrderDetail blanketOrderDetail)
        {
            if (!blanketOrderDetail.IsBarPrepared)
            {
                blanketOrderDetail.Errors.Add("Generic", "Bar belum di prepare");
            }
            return blanketOrderDetail;
        }

        public BlanketOrderDetail VHasBeenAdhesiveTapeApplied(BlanketOrderDetail blanketOrderDetail)
        {
            if (!blanketOrderDetail.IsAdhesiveTapeApplied)
            {
                blanketOrderDetail.Errors.Add("Generic", "Belum di adhesive tape");
            }
            return blanketOrderDetail;
        }

        public BlanketOrderDetail VHasBeenBarMounted(BlanketOrderDetail blanketOrderDetail)
        {
            if (!blanketOrderDetail.IsBarMounted)
            {
                blanketOrderDetail.Errors.Add("Generic", "Bar belum di mount");
            }
            return blanketOrderDetail;
        }

        public BlanketOrderDetail VHasBeenBarHeatPressed(BlanketOrderDetail blanketOrderDetail)
        {
            if (!blanketOrderDetail.IsBarHeatPressed)
            {
                blanketOrderDetail.Errors.Add("Generic", "Bar belum di heat press");
            }
            return blanketOrderDetail;
        }

        public BlanketOrderDetail VHasBeenBarPullOffTested(BlanketOrderDetail blanketOrderDetail)
        {
            if (!blanketOrderDetail.IsBarPullOffTested)
            {
                blanketOrderDetail.Errors.Add("Generic", "Bar belum di pull-off test");
            }
            return blanketOrderDetail;
        }

        public BlanketOrderDetail VHasBeenQCAndMarked(BlanketOrderDetail blanketOrderDetail)
        {
            if (!blanketOrderDetail.IsQCAndMarked)
            {
                blanketOrderDetail.Errors.Add("Generic", "Belum di QC dan mark");
            }
            return blanketOrderDetail;
        }

        public BlanketOrderDetail VHasBeenPackaged(BlanketOrderDetail blanketOrderDetail)
        {
            if (!blanketOrderDetail.IsPackaged)
            {
                blanketOrderDetail.Errors.Add("Generic", "Belum di package");
            }
            return blanketOrderDetail;
        }

        public BlanketOrderDetail VHasBeenFinished(BlanketOrderDetail blanketOrderDetail)
        {
            if (!blanketOrderDetail.IsFinished)
            {
                blanketOrderDetail.Errors.Add("Generic", "Belum selesai");
            }
            return blanketOrderDetail;
        }

        public BlanketOrderDetail VHasFinishedDate(BlanketOrderDetail blanketOrderDetail)
        {
            if (blanketOrderDetail.FinishedDate == null)
            {
                blanketOrderDetail.Errors.Add("FinishedDate", "Tidak boleh kosong");
            }
            return blanketOrderDetail;
        }

        public BlanketOrderDetail VHasBeenRejected(BlanketOrderDetail blanketOrderDetail)
        {
            if (!blanketOrderDetail.IsRejected)
            {
                blanketOrderDetail.Errors.Add("Generic", "Belum di reject");
            }
            return blanketOrderDetail;
        }

        public BlanketOrderDetail VHasRejectedDate(BlanketOrderDetail blanketOrderDetail)
        {
            if (blanketOrderDetail.RejectedDate == null)
            {
                blanketOrderDetail.Errors.Add("RejectedDate", "Tidak boleh kosong");
            }
            return blanketOrderDetail;
        }

        public BlanketOrderDetail VBlanketOrderHasBeenConfirmed(BlanketOrderDetail blanketOrderDetail, IBlanketOrderService _blanketOrderService)
        {
            BlanketOrder blanketOrder = _blanketOrderService.GetObjectById(blanketOrderDetail.BlanketOrderId);
            if (!blanketOrder.IsConfirmed)
            {
                blanketOrderDetail.Errors.Add("Generic", "Blanket Order belum dikonfirmasi");
            }
            return blanketOrderDetail;
        }

        public BlanketOrderDetail VHasNotBeenCut(BlanketOrderDetail blanketOrderDetail)
        {
            if (blanketOrderDetail.IsCut)
            {
                blanketOrderDetail.Errors.Add("Generic", "Sudah di cut");
            }
            return blanketOrderDetail;
        }

        public BlanketOrderDetail VHasNotBeenSideSealed(BlanketOrderDetail blanketOrderDetail)
        {
            if (blanketOrderDetail.IsSideSealed)
            {
                blanketOrderDetail.Errors.Add("Generic", "Sudah di side seal");
            }
            return blanketOrderDetail;
        }

        public BlanketOrderDetail VHasNotBeenBarPrepared(BlanketOrderDetail blanketOrderDetail)
        {
            if (blanketOrderDetail.IsBarPrepared)
            {
                blanketOrderDetail.Errors.Add("Generic", "Bar belum di prepare");
            }
            return blanketOrderDetail;
        }

        public BlanketOrderDetail VHasNotBeenAdhesiveTapeApplied(BlanketOrderDetail blanketOrderDetail)
        {
            if (blanketOrderDetail.IsAdhesiveTapeApplied)
            {
                blanketOrderDetail.Errors.Add("Generic", "Sudah di adhesive tape");
            }
            return blanketOrderDetail;
        }

        public BlanketOrderDetail VHasNotBeenBarMounted(BlanketOrderDetail blanketOrderDetail)
        {
            if (blanketOrderDetail.IsBarMounted)
            {
                blanketOrderDetail.Errors.Add("Generic", "Bar belum di mount");
            }
            return blanketOrderDetail;
        }

        public BlanketOrderDetail VHasNotBeenBarHeatPressed(BlanketOrderDetail blanketOrderDetail)
        {
            if (blanketOrderDetail.IsBarHeatPressed)
            {
                blanketOrderDetail.Errors.Add("Generic", "Bar sudah di heat press");
            }
            return blanketOrderDetail;
        }

        public BlanketOrderDetail VHasNotBeenBarPullOffTested(BlanketOrderDetail blanketOrderDetail)
        {
            if (blanketOrderDetail.IsBarPullOffTested)
            {
                blanketOrderDetail.Errors.Add("Generic", "Bar sudah di pull-off test");
            }
            return blanketOrderDetail;
        }

        public BlanketOrderDetail VHasNotBeenQCAndMarked(BlanketOrderDetail blanketOrderDetail)
        {
            if (blanketOrderDetail.IsQCAndMarked)
            {
                blanketOrderDetail.Errors.Add("Generic", "Sudah di QC dan mark");
            }
            return blanketOrderDetail;
        }

        public BlanketOrderDetail VHasNotBeenPackaged(BlanketOrderDetail blanketOrderDetail)
        {
            if (blanketOrderDetail.IsPackaged)
            {
                blanketOrderDetail.Errors.Add("Generic", "Sudah di package");
            }
            return blanketOrderDetail;
        }

        public BlanketOrderDetail VHasNotBeenFinished(BlanketOrderDetail blanketOrderDetail)
        {
            if (blanketOrderDetail.IsFinished)
            {
                blanketOrderDetail.Errors.Add("Generic", "Sudah selesai");
            }
            return blanketOrderDetail;
        }

        public BlanketOrderDetail VHasNotBeenRejected(BlanketOrderDetail blanketOrderDetail)
        {
            if (blanketOrderDetail.IsRejected)
            {
                blanketOrderDetail.Errors.Add("Generic", "Sudah di reject");
            }
            return blanketOrderDetail;
        }

        public BlanketOrderDetail VBlanketOrderHasNotBeenConfirmed(BlanketOrderDetail blanketOrderDetail, IBlanketOrderService _blanketOrderService)
        {
            BlanketOrder blanketOrder = _blanketOrderService.GetObjectById(blanketOrderDetail.BlanketOrderId);
            if (blanketOrder.IsConfirmed)
            {
                blanketOrderDetail.Errors.Add("Generic", "Blanket Order sudah dikonfirmasi");
            }
            return blanketOrderDetail;
        }

        public BlanketOrderDetail VBlanketOrderHasNotBeenCompleted(BlanketOrderDetail blanketOrderDetail, IBlanketOrderService _blanketOrderService)
        {
            BlanketOrder blanketOrder = _blanketOrderService.GetObjectById(blanketOrderDetail.BlanketOrderId);
            if (blanketOrder.IsCompleted)
            {
                blanketOrderDetail.Errors.Add("Generic", "Blanket order sudah complete");
            }
            return blanketOrderDetail;
        }

        public BlanketOrderDetail VCreateObject(BlanketOrderDetail blanketOrderDetail, IBlanketOrderService _blanketOrderService, IBlanketService _blanketService)
        {
            VHasBlanketOrder(blanketOrderDetail, _blanketOrderService);
            if (!isValid(blanketOrderDetail)) { return blanketOrderDetail; }
            VHasBlanket(blanketOrderDetail, _blanketService);
            if (!isValid(blanketOrderDetail)) { return blanketOrderDetail; }
            VBlanketOrderHasNotBeenConfirmed(blanketOrderDetail, _blanketOrderService);
            return blanketOrderDetail;
        }

        public BlanketOrderDetail VUpdateObject(BlanketOrderDetail blanketOrderDetail, IBlanketOrderService _blanketOrderService, IBlanketService _blanketService)
        {
            VHasNotBeenCut(blanketOrderDetail);
            if (!isValid(blanketOrderDetail)) { return blanketOrderDetail; }
            VHasNotBeenRejected(blanketOrderDetail);
            if (!isValid(blanketOrderDetail)) { return blanketOrderDetail; }
            VCreateObject(blanketOrderDetail, _blanketOrderService, _blanketService);
            return blanketOrderDetail;
        }

        public BlanketOrderDetail VDeleteObject(BlanketOrderDetail blanketOrderDetail, IBlanketOrderService _blanketOrderService)
        {
            VHasNotBeenCut(blanketOrderDetail);
            if (!isValid(blanketOrderDetail)) { return blanketOrderDetail; }
            VHasNotBeenRejected(blanketOrderDetail);
            if (!isValid(blanketOrderDetail)) { return blanketOrderDetail; }
            VBlanketOrderHasNotBeenConfirmed(blanketOrderDetail, _blanketOrderService);
            return blanketOrderDetail;
        }

        public BlanketOrderDetail VCutObject(BlanketOrderDetail blanketOrderDetail, IBlanketOrderService _blanketOrderService)
        {
            VHasNotBeenCut(blanketOrderDetail);
            if (!isValid(blanketOrderDetail)) { return blanketOrderDetail; }
            VHasNotBeenRejected(blanketOrderDetail);
            if (!isValid(blanketOrderDetail)) { return blanketOrderDetail; }
            VBlanketOrderHasBeenConfirmed(blanketOrderDetail, _blanketOrderService);
            return blanketOrderDetail;
        }

        public BlanketOrderDetail VSideSealObject(BlanketOrderDetail blanketOrderDetail)
        {
            VHasNotBeenSideSealed(blanketOrderDetail);
            if (!isValid(blanketOrderDetail)) { return blanketOrderDetail; }
            VHasBeenCut(blanketOrderDetail);
            if (!isValid(blanketOrderDetail)) { return blanketOrderDetail; }
            VHasNotBeenRejected(blanketOrderDetail);
            return blanketOrderDetail;
        }

        public BlanketOrderDetail VPrepareObject(BlanketOrderDetail blanketOrderDetail)
        {
            VHasNotBeenBarPrepared(blanketOrderDetail);
            if (!isValid(blanketOrderDetail)) { return blanketOrderDetail; }
            VHasBeenSideSealed(blanketOrderDetail);
            if (!isValid(blanketOrderDetail)) { return blanketOrderDetail; }
            VHasNotBeenRejected(blanketOrderDetail);
            return blanketOrderDetail;
        }

        public BlanketOrderDetail VApplyTapeAdhesiveToObject(BlanketOrderDetail blanketOrderDetail)
        {
            VHasNotBeenAdhesiveTapeApplied(blanketOrderDetail);
            if (!isValid(blanketOrderDetail)) { return blanketOrderDetail; }
            VHasBeenBarPrepared(blanketOrderDetail);
            if (!isValid(blanketOrderDetail)) { return blanketOrderDetail; }
            VHasNotBeenRejected(blanketOrderDetail);
            return blanketOrderDetail;
        }

        public BlanketOrderDetail VMountObject(BlanketOrderDetail blanketOrderDetail)
        {
            VHasNotBeenBarMounted(blanketOrderDetail);
            if (!isValid(blanketOrderDetail)) { return blanketOrderDetail; }
            VHasBeenAdhesiveTapeApplied(blanketOrderDetail);
            if (!isValid(blanketOrderDetail)) { return blanketOrderDetail; }
            VHasNotBeenRejected(blanketOrderDetail);
            return blanketOrderDetail;
        }

        public BlanketOrderDetail VHeatPressObject(BlanketOrderDetail blanketOrderDetail)
        {
            VHasNotBeenBarHeatPressed(blanketOrderDetail);
            if (!isValid(blanketOrderDetail)) { return blanketOrderDetail; }
            VHasBeenBarMounted(blanketOrderDetail);
            if (!isValid(blanketOrderDetail)) { return blanketOrderDetail; }
            VHasNotBeenRejected(blanketOrderDetail);
            return blanketOrderDetail;
        }

        public BlanketOrderDetail VPullOffTestObject(BlanketOrderDetail blanketOrderDetail)
        {
            VHasNotBeenBarPullOffTested(blanketOrderDetail);
            if (!isValid(blanketOrderDetail)) { return blanketOrderDetail; }
            VHasBeenBarHeatPressed(blanketOrderDetail);
            if (!isValid(blanketOrderDetail)) { return blanketOrderDetail; }
            VHasNotBeenRejected(blanketOrderDetail);
            return blanketOrderDetail;
        }

        public BlanketOrderDetail VQCAndMarkObject(BlanketOrderDetail blanketOrderDetail)
        {
            VHasNotBeenQCAndMarked(blanketOrderDetail);
            if (!isValid(blanketOrderDetail)) { return blanketOrderDetail; }
            VHasBeenBarPullOffTested(blanketOrderDetail);
            if (!isValid(blanketOrderDetail)) { return blanketOrderDetail; }
            VHasNotBeenRejected(blanketOrderDetail);
            return blanketOrderDetail;
        }

        public BlanketOrderDetail VPackageObject(BlanketOrderDetail blanketOrderDetail)
        {
            VHasNotBeenPackaged(blanketOrderDetail);
            if (!isValid(blanketOrderDetail)) { return blanketOrderDetail; }
            VHasBeenQCAndMarked(blanketOrderDetail);
            if (!isValid(blanketOrderDetail)) { return blanketOrderDetail; }
            VHasNotBeenRejected(blanketOrderDetail);
            return blanketOrderDetail;
        }

        public BlanketOrderDetail VFinishObject(BlanketOrderDetail blanketOrderDetail, IBlanketOrderService _blanketOrderService)
        {
            VHasFinishedDate(blanketOrderDetail);
            if (!isValid(blanketOrderDetail)) { return blanketOrderDetail; }
            VBlanketOrderHasBeenConfirmed(blanketOrderDetail, _blanketOrderService);
            if (!isValid(blanketOrderDetail)) { return blanketOrderDetail; }
            VHasNotBeenFinished(blanketOrderDetail);
            if (!isValid(blanketOrderDetail)) { return blanketOrderDetail; }
            VHasBeenPackaged(blanketOrderDetail);
            if (!isValid(blanketOrderDetail)) { return blanketOrderDetail; }
            VHasNotBeenRejected(blanketOrderDetail);
            return blanketOrderDetail;
        }

        public BlanketOrderDetail VUnfinishObject(BlanketOrderDetail blanketOrderDetail, IBlanketOrderService _blanketOrderService)
        {
            VHasBeenFinished(blanketOrderDetail);
            if (!isValid(blanketOrderDetail)) { return blanketOrderDetail; }
            VBlanketOrderHasNotBeenCompleted(blanketOrderDetail, _blanketOrderService);
            return blanketOrderDetail;
        }

        public BlanketOrderDetail VRejectObject(BlanketOrderDetail blanketOrderDetail, IBlanketOrderService _blanketOrderService)
        {
            VHasRejectedDate(blanketOrderDetail);
            if (!isValid(blanketOrderDetail)) { return blanketOrderDetail; }
            VHasNotBeenRejected(blanketOrderDetail);
            if (!isValid(blanketOrderDetail)) { return blanketOrderDetail; }
            VHasNotBeenFinished(blanketOrderDetail);
            if (!isValid(blanketOrderDetail)) { return blanketOrderDetail; }
            VBlanketOrderHasNotBeenCompleted(blanketOrderDetail, _blanketOrderService);
            return blanketOrderDetail;
        }

        public BlanketOrderDetail VUndoRejectObject(BlanketOrderDetail blanketOrderDetail, IBlanketOrderService _blanketOrderService)
        {
            VHasBeenRejected(blanketOrderDetail);
            if (!isValid(blanketOrderDetail)) { return blanketOrderDetail; }
            VBlanketOrderHasNotBeenCompleted(blanketOrderDetail, _blanketOrderService);
            return blanketOrderDetail;
        }

        public bool ValidCreateObject(BlanketOrderDetail blanketOrderDetail, IBlanketOrderService _blanketOrderService, IBlanketService _blanketService)
        {
            VCreateObject(blanketOrderDetail, _blanketOrderService, _blanketService);
            return isValid(blanketOrderDetail);
        }

        public bool ValidUpdateObject(BlanketOrderDetail blanketOrderDetail, IBlanketOrderService _blanketOrderService, IBlanketService _blanketService)
        {
            blanketOrderDetail.Errors.Clear();
            VUpdateObject(blanketOrderDetail, _blanketOrderService, _blanketService);
            return isValid(blanketOrderDetail);
        }

        public bool ValidDeleteObject(BlanketOrderDetail blanketOrderDetail, IBlanketOrderService _blanketOrderService)
        {
            blanketOrderDetail.Errors.Clear();
            VDeleteObject(blanketOrderDetail, _blanketOrderService);
            return isValid(blanketOrderDetail);
        }

        public bool ValidCutObject(BlanketOrderDetail blanketOrderDetail, IBlanketOrderService _blanketOrderService)
        {
            blanketOrderDetail.Errors.Clear();
            VCutObject(blanketOrderDetail, _blanketOrderService);
            return isValid(blanketOrderDetail);
        }

        public bool ValidSideSealObject(BlanketOrderDetail blanketOrderDetail)
        {
            blanketOrderDetail.Errors.Clear();
            VSideSealObject(blanketOrderDetail);
            return isValid(blanketOrderDetail);
        }

        public bool ValidPrepareObject(BlanketOrderDetail blanketOrderDetail)
        {
            blanketOrderDetail.Errors.Clear();
            VPrepareObject(blanketOrderDetail);
            return isValid(blanketOrderDetail);
        }

        public bool ValidApplyTapeAdhesiveToObject(BlanketOrderDetail blanketOrderDetail)
        {
            blanketOrderDetail.Errors.Clear();
            VApplyTapeAdhesiveToObject(blanketOrderDetail);
            return isValid(blanketOrderDetail);
        }

        public bool ValidMountObject(BlanketOrderDetail blanketOrderDetail)
        {
            blanketOrderDetail.Errors.Clear();
            VMountObject(blanketOrderDetail);
            return isValid(blanketOrderDetail);
        }

        public bool ValidHeatPressObject(BlanketOrderDetail blanketOrderDetail)
        {
            blanketOrderDetail.Errors.Clear();
            VHeatPressObject(blanketOrderDetail);
            return isValid(blanketOrderDetail);
        }

        public bool ValidPullOffTestObject(BlanketOrderDetail blanketOrderDetail)
        {
            blanketOrderDetail.Errors.Clear();
            VPullOffTestObject(blanketOrderDetail);
            return isValid(blanketOrderDetail);
        }

        public bool ValidQCAndMarkObject(BlanketOrderDetail blanketOrderDetail)
        {
            blanketOrderDetail.Errors.Clear();
            VQCAndMarkObject(blanketOrderDetail);
            return isValid(blanketOrderDetail);
        }
        
        public bool ValidPackageObject(BlanketOrderDetail blanketOrderDetail)
        {
            blanketOrderDetail.Errors.Clear();
            VPackageObject(blanketOrderDetail);
            return isValid(blanketOrderDetail);
        }

        public bool ValidFinishObject(BlanketOrderDetail blanketOrderDetail, IBlanketOrderService _blanketOrderService)
        {
            blanketOrderDetail.Errors.Clear();
            VFinishObject(blanketOrderDetail, _blanketOrderService);
            return isValid(blanketOrderDetail);
        }

        public bool ValidUnfinishObject(BlanketOrderDetail blanketOrderDetail, IBlanketOrderService _blanketOrderService)
        {
            blanketOrderDetail.Errors.Clear();
            VUnfinishObject(blanketOrderDetail, _blanketOrderService);
            return isValid(blanketOrderDetail);
        }

        public bool ValidRejectObject(BlanketOrderDetail blanketOrderDetail, IBlanketOrderService _blanketOrderService)
        {
            blanketOrderDetail.Errors.Clear();
            VRejectObject(blanketOrderDetail, _blanketOrderService);
            return isValid(blanketOrderDetail);
        }

        public bool ValidUndoRejectObject(BlanketOrderDetail blanketOrderDetail, IBlanketOrderService _blanketOrderService)
        {
            blanketOrderDetail.Errors.Clear();
            VUndoRejectObject(blanketOrderDetail, _blanketOrderService);
            return isValid(blanketOrderDetail);
        }

        public bool isValid(BlanketOrderDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(BlanketOrderDetail obj)
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
