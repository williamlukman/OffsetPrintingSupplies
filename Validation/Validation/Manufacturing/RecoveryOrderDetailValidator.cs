using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class RecoveryOrderDetailValidator : IRecoveryOrderDetailValidator
    {
        public RecoveryOrderDetail VHasRecoveryOrder(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService)
        {
            RecoveryOrder recoveryOrder = _recoveryOrderService.GetObjectById(recoveryOrderDetail.RecoveryOrderId);
            if (recoveryOrder == null)
            {
                recoveryOrderDetail.Errors.Add("RecoveryOrderId", "Tidak terasosiasi dengan Recovery Order");
            }
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail VHasCoreIdentificationDetail(RecoveryOrderDetail recoveryOrderDetail, ICoreIdentificationDetailService _coreIdentificationDetailService)
        {
            CoreIdentificationDetail coreIdentificationDetail = _coreIdentificationDetailService.GetObjectById(recoveryOrderDetail.CoreIdentificationDetailId);
            if (coreIdentificationDetail == null)
            {
                recoveryOrderDetail.Errors.Add("CoreIdentificationDetailId", "Tidak terasosiasi dengan Core Identification Detail");
            }
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail VHasRollerBuilder(RecoveryOrderDetail recoveryOrderDetail, IRollerBuilderService _rollerBuilderService)
        {
            RollerBuilder rollerBuilder = _rollerBuilderService.GetObjectById(recoveryOrderDetail.RollerBuilderId);
            if (rollerBuilder == null)
            {
                recoveryOrderDetail.Errors.Add("RollerBuilderId", "Tidak terasosiasi dengan Roller Builder");
            }
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail VHasCoreTypeCase(RecoveryOrderDetail recoveryOrderDetail)
        {
            if (recoveryOrderDetail.CoreTypeCase != Core.Constants.Constant.CoreTypeCase.R &&
                recoveryOrderDetail.CoreTypeCase != Core.Constants.Constant.CoreTypeCase.Z)
            {
                recoveryOrderDetail.Errors.Add("CoreTypeCase", "Hanya dapat diisi dengan R atau Z");
            }
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail VHasAcc(RecoveryOrderDetail recoveryOrderDetail)
        {
            if (String.IsNullOrEmpty(recoveryOrderDetail.Acc) || recoveryOrderDetail.Acc.Trim() == "")
            {
                recoveryOrderDetail.Errors.Add("Acc", "Tidak boleh kosong");
            }
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail VHasRepairRequestCase(RecoveryOrderDetail recoveryOrderDetail)
        {
            if (recoveryOrderDetail.RepairRequestCase != Core.Constants.Constant.RepairRequestCase.BearingSeat &&
                recoveryOrderDetail.RepairRequestCase != Core.Constants.Constant.RepairRequestCase.CentreDrill)
            {
                recoveryOrderDetail.Errors.Add("RepairRequestCase", "Hanya dapat diisi dengan 1 untuk Bearing Seat atau 2 untuk CentreDrill");
            }
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail VHasBeenDisassembled(RecoveryOrderDetail recoveryOrderDetail)
        {
            if (!recoveryOrderDetail.IsDisassembled)
            {
                recoveryOrderDetail.Errors.Add("Generic", "Belum di disassembly");
            }
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail VHasBeenStrippedAndGlued(RecoveryOrderDetail recoveryOrderDetail)
        {
            if (!recoveryOrderDetail.IsStrippedAndGlued)
            {
                recoveryOrderDetail.Errors.Add("Generic", "Belum di strip dan glue");
            }
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail VHasBeenWrapped(RecoveryOrderDetail recoveryOrderDetail)
        {
            if (!recoveryOrderDetail.IsWrapped)
            {
                recoveryOrderDetail.Errors.Add("Generic", "Belum di wrap");
            }
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail VHasBeenVulcanized(RecoveryOrderDetail recoveryOrderDetail)
        {
            if (!recoveryOrderDetail.IsVulcanized)
            {
                recoveryOrderDetail.Errors.Add("Generic", "Belum di vulkanize");
            }
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail VHasBeenFacedOff(RecoveryOrderDetail recoveryOrderDetail)
        {
            if (!recoveryOrderDetail.IsFacedOff)
            {
                recoveryOrderDetail.Errors.Add("Generic", "Belum di face off");
            }
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail VHasBeenConventionalGrinded(RecoveryOrderDetail recoveryOrderDetail)
        {
            if (!recoveryOrderDetail.IsConventionalGrinded)
            {
                recoveryOrderDetail.Errors.Add("Generic", "Belum di conventional grind");
            }
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail VHasBeenCWCGrinded(RecoveryOrderDetail recoveryOrderDetail)
        {
            if (!recoveryOrderDetail.IsCWCGrinded)
            {
                recoveryOrderDetail.Errors.Add("Generic", "Belum di CWC grind");
            }
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail VHasBeenPolishedAndQC(RecoveryOrderDetail recoveryOrderDetail)
        {
            if (!recoveryOrderDetail.IsPolishedAndQC)
            {
                recoveryOrderDetail.Errors.Add("Generic", "Belum di polish dan QC");
            }
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail VHasBeenPackaged(RecoveryOrderDetail recoveryOrderDetail)
        {
            if (!recoveryOrderDetail.IsPackaged)
            {
                recoveryOrderDetail.Errors.Add("Generic", "Belum di package");
            }
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail VHasBeenFinished(RecoveryOrderDetail recoveryOrderDetail)
        {
            if (!recoveryOrderDetail.IsFinished)
            {
                recoveryOrderDetail.Errors.Add("Generic", "Belum selesai");
            }
            return recoveryOrderDetail;
        }
        
        public RecoveryOrderDetail VHasBeenRejected(RecoveryOrderDetail recoveryOrderDetail)
        {
            if (!recoveryOrderDetail.IsRejected)
            {
                recoveryOrderDetail.Errors.Add("Generic", "Belum di reject");
            }
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail VHasNotBeenDisassembled(RecoveryOrderDetail recoveryOrderDetail)
        {
            if (recoveryOrderDetail.IsDisassembled)
            {
                recoveryOrderDetail.Errors.Add("Generic", "Sudah di disassembly");
            }
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail VHasNotBeenStrippedAndGlued(RecoveryOrderDetail recoveryOrderDetail)
        {
            if (recoveryOrderDetail.IsStrippedAndGlued)
            {
                recoveryOrderDetail.Errors.Add("Generic", "Sudah di strip dan glue");
            }
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail VHasNotBeenWrapped(RecoveryOrderDetail recoveryOrderDetail)
        {
            if (recoveryOrderDetail.IsWrapped)
            {
                recoveryOrderDetail.Errors.Add("Generic", "Sudah di wrap");
            }
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail VCompoundUsageIsLargerThanZero(RecoveryOrderDetail recoveryOrderDetail)
        {
            if (recoveryOrderDetail.CompoundUsage <= 0)
            {
                recoveryOrderDetail.Errors.Add("CompoundUsage", "Tidak boleh nol atau negatif");
            }
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail VHasNotBeenVulcanized(RecoveryOrderDetail recoveryOrderDetail)
        {
            if (recoveryOrderDetail.IsVulcanized)
            {
                recoveryOrderDetail.Errors.Add("Generic", "Sudah di vulcanize");
            }
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail VHasNotBeenFacedOff(RecoveryOrderDetail recoveryOrderDetail)
        {
            if (recoveryOrderDetail.IsFacedOff)
            {
                recoveryOrderDetail.Errors.Add("Generic", "Sudah di face off");
            }
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail VHasNotBeenConventionalGrinded(RecoveryOrderDetail recoveryOrderDetail)
        {
            if (recoveryOrderDetail.IsConventionalGrinded)
            {
                recoveryOrderDetail.Errors.Add("Generic", "Sudah di conventional grind");
            }
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail VHasNotBeenCWCGrinded(RecoveryOrderDetail recoveryOrderDetail)
        {
            if (recoveryOrderDetail.IsCWCGrinded)
            {
                recoveryOrderDetail.Errors.Add("Generic", "Sudah di CWC grind");
            }
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail VHasNotBeenPolishedAndQC(RecoveryOrderDetail recoveryOrderDetail)
        {
            if (recoveryOrderDetail.IsPolishedAndQC)
            {
                recoveryOrderDetail.Errors.Add("Generic", "Sudah di polish dan QC");
            }
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail VHasNotBeenPackaged(RecoveryOrderDetail recoveryOrderDetail)
        {
            if (recoveryOrderDetail.IsPackaged)
            {
                recoveryOrderDetail.Errors.Add("Generic", "Sudah di package");
            }
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail VHasNotBeenFinished(RecoveryOrderDetail recoveryOrderDetail)
        {
            if (recoveryOrderDetail.IsFinished)
            {
                recoveryOrderDetail.Errors.Add("Generic", "Sudah selesai");
            }
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail VHasNotBeenRejected(RecoveryOrderDetail recoveryOrderDetail)
        {
            if (recoveryOrderDetail.IsRejected)
            {
                recoveryOrderDetail.Errors.Add("Generic", "Sudah di reject");
            }
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail VRecoveryOrderHasNotBeenConfirmed(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService)
        {
            RecoveryOrder recoveryOrder = _recoveryOrderService.GetObjectById(recoveryOrderDetail.RecoveryOrderId);
            if (recoveryOrder.IsConfirmed)
            {
                recoveryOrderDetail.Errors.Add("Generic", "Recovery Order sudah dikonfirmasi");
            }
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail VRecoveryOrderHasBeenConfirmed(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService)
        {
            RecoveryOrder recoveryOrder = _recoveryOrderService.GetObjectById(recoveryOrderDetail.RecoveryOrderId);
            if (!recoveryOrder.IsConfirmed)
            {
                recoveryOrderDetail.Errors.Add("Generic", "Recovery Order belum dikonfirmasi");
            }
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail VRecoveryOrderHasNotBeenCompleted(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService)
        {
            RecoveryOrder recoveryOrder = _recoveryOrderService.GetObjectById(recoveryOrderDetail.RecoveryOrderId);
            if (recoveryOrder.IsCompleted)
            {
                recoveryOrderDetail.Errors.Add("Generic", "Recovery order sudah complete");
            }
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail VCreateObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService,
                                                 ICoreIdentificationDetailService _coreIdentificationDetailService, IRollerBuilderService _rollerBuilderService)
        {
            VHasRecoveryOrder(recoveryOrderDetail, _recoveryOrderService);
            if (!isValid(recoveryOrderDetail)) { return recoveryOrderDetail; }
            VHasCoreIdentificationDetail(recoveryOrderDetail, _coreIdentificationDetailService);
            if (!isValid(recoveryOrderDetail)) { return recoveryOrderDetail; }
            VHasRollerBuilder(recoveryOrderDetail, _rollerBuilderService);
            if (!isValid(recoveryOrderDetail)) { return recoveryOrderDetail; }
            VHasCoreTypeCase(recoveryOrderDetail);
            if (!isValid(recoveryOrderDetail)) { return recoveryOrderDetail; }
            VHasAcc(recoveryOrderDetail);
            if (!isValid(recoveryOrderDetail)) { return recoveryOrderDetail; }
            VHasRepairRequestCase(recoveryOrderDetail);
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail VUpdateObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService,
                                                 ICoreIdentificationDetailService _coreIdentificationDetailService, IRollerBuilderService _rollerBuilderService)
        {
            VHasNotBeenDisassembled(recoveryOrderDetail);
            if (!isValid(recoveryOrderDetail)) { return recoveryOrderDetail; }
            VHasNotBeenRejected(recoveryOrderDetail);
            if (!isValid(recoveryOrderDetail)) { return recoveryOrderDetail; }
            VCreateObject(recoveryOrderDetail, _recoveryOrderService, _coreIdentificationDetailService, _rollerBuilderService);
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail VDeleteObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService,
                                                 IRecoveryAccessoryDetailService _recoveryAccessoryDetailService)
        {
            VHasNotBeenDisassembled(recoveryOrderDetail);
            if (!isValid(recoveryOrderDetail)) { return recoveryOrderDetail; }
            VHasNotBeenRejected(recoveryOrderDetail);
            if (!isValid(recoveryOrderDetail)) { return recoveryOrderDetail; }
            VRecoveryOrderHasNotBeenConfirmed(recoveryOrderDetail, _recoveryOrderService);
            if (!isValid(recoveryOrderDetail)) { return recoveryOrderDetail; }
            //VHasNoRecoveryAccessoryDetails(recoveryOrderDetail, _recoveryAccessoryDetailService);
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail VAddAccessory(RecoveryOrderDetail recoveryOrderDetail, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService)
        {
            VHasNotBeenFinished(recoveryOrderDetail);
            if (!isValid(recoveryOrderDetail)) { return recoveryOrderDetail; }
            VHasNotBeenRejected(recoveryOrderDetail);
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail VRemoveAccessory(RecoveryOrderDetail recoveryOrderDetail, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService)
        {
            // VNoAccessoriesOrAccessoriesHaveNotBeenFinished(recoveryOrderDetail, _recoveryAccessoryDetailService);
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail VDisassembleObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService)
        {
            VRecoveryOrderHasBeenConfirmed(recoveryOrderDetail, _recoveryOrderService);
            if (!isValid(recoveryOrderDetail)) { return recoveryOrderDetail; }
            VHasNotBeenDisassembled(recoveryOrderDetail);
            if (!isValid(recoveryOrderDetail)) { return recoveryOrderDetail; }
            VHasNotBeenRejected(recoveryOrderDetail);
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail VStripAndGlueObject(RecoveryOrderDetail recoveryOrderDetail)
        {
            VHasNotBeenStrippedAndGlued(recoveryOrderDetail);
            if (!isValid(recoveryOrderDetail)) { return recoveryOrderDetail; }
            VHasBeenDisassembled(recoveryOrderDetail);
            if (!isValid(recoveryOrderDetail)) { return recoveryOrderDetail; }
            VHasNotBeenRejected(recoveryOrderDetail);
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail VWrapObject(RecoveryOrderDetail recoveryOrderDetail)
        {
            VHasNotBeenWrapped(recoveryOrderDetail);
            if (!isValid(recoveryOrderDetail)) { return recoveryOrderDetail; }
            VHasBeenStrippedAndGlued(recoveryOrderDetail);
            if (!isValid(recoveryOrderDetail)) { return recoveryOrderDetail; }
            VHasNotBeenRejected(recoveryOrderDetail);
            if (!isValid(recoveryOrderDetail)) { return recoveryOrderDetail; }
            VCompoundUsageIsLargerThanZero(recoveryOrderDetail);
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail VVulcanizeObject(RecoveryOrderDetail recoveryOrderDetail)
        {
            VHasNotBeenVulcanized(recoveryOrderDetail);
            if (!isValid(recoveryOrderDetail)) { return recoveryOrderDetail; }
            VHasBeenWrapped(recoveryOrderDetail);
            if (!isValid(recoveryOrderDetail)) { return recoveryOrderDetail; }
            VHasNotBeenRejected(recoveryOrderDetail);
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail VFaceOffObject(RecoveryOrderDetail recoveryOrderDetail)
        {
            VHasNotBeenFacedOff(recoveryOrderDetail);
            if (!isValid(recoveryOrderDetail)) { return recoveryOrderDetail; }
            VHasBeenVulcanized(recoveryOrderDetail);
            if (!isValid(recoveryOrderDetail)) { return recoveryOrderDetail; }
            VHasNotBeenRejected(recoveryOrderDetail);
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail VConventionalGrindObject(RecoveryOrderDetail recoveryOrderDetail)
        {
            VHasNotBeenConventionalGrinded(recoveryOrderDetail);
            if (!isValid(recoveryOrderDetail)) { return recoveryOrderDetail; }
            VHasBeenFacedOff(recoveryOrderDetail);
            if (!isValid(recoveryOrderDetail)) { return recoveryOrderDetail; }
            VHasNotBeenRejected(recoveryOrderDetail);
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail VCWCGrindObject(RecoveryOrderDetail recoveryOrderDetail)
        {
            VHasNotBeenCWCGrinded(recoveryOrderDetail);
            if (!isValid(recoveryOrderDetail)) { return recoveryOrderDetail; }
            VHasBeenConventionalGrinded(recoveryOrderDetail);
            if (!isValid(recoveryOrderDetail)) { return recoveryOrderDetail; }
            VHasNotBeenRejected(recoveryOrderDetail);
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail VPolishAndQCObject(RecoveryOrderDetail recoveryOrderDetail)
        {
            VHasNotBeenPolishedAndQC(recoveryOrderDetail);
            if (!isValid(recoveryOrderDetail)) { return recoveryOrderDetail; }
            VHasBeenCWCGrinded(recoveryOrderDetail);
            if (!isValid(recoveryOrderDetail)) { return recoveryOrderDetail; }
            VHasNotBeenRejected(recoveryOrderDetail);
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail VPackageObject(RecoveryOrderDetail recoveryOrderDetail)
        {
            VHasNotBeenPackaged(recoveryOrderDetail);
            if (!isValid(recoveryOrderDetail)) { return recoveryOrderDetail; }
            VHasBeenPolishedAndQC(recoveryOrderDetail);
            if (!isValid(recoveryOrderDetail)) { return recoveryOrderDetail; }
            VHasNotBeenRejected(recoveryOrderDetail);
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail VHasFinishedDate(RecoveryOrderDetail recoveryOrderDetail)
        {
            if (recoveryOrderDetail.FinishedDate == null)
            {
                recoveryOrderDetail.Errors.Add("FinishedDate", "Tidak boleh kosong");
            }
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail VFinishObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService)
        {
            VHasFinishedDate(recoveryOrderDetail);
            if (!isValid(recoveryOrderDetail)) { return recoveryOrderDetail; }
            VHasNotBeenFinished(recoveryOrderDetail);
            if (!isValid(recoveryOrderDetail)) { return recoveryOrderDetail; }
            VHasBeenPackaged(recoveryOrderDetail);
            if (!isValid(recoveryOrderDetail)) { return recoveryOrderDetail; }
            VHasNotBeenRejected(recoveryOrderDetail);
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail VUnfinishObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService) 
        {
            VHasBeenFinished(recoveryOrderDetail);
            if (!isValid(recoveryOrderDetail)) { return recoveryOrderDetail; }
            VRecoveryOrderHasNotBeenCompleted(recoveryOrderDetail, _recoveryOrderService);
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail VRejectObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService)
        {
            VHasNotBeenRejected(recoveryOrderDetail);
            if (!isValid(recoveryOrderDetail)) { return recoveryOrderDetail; }
            VHasNotBeenFinished(recoveryOrderDetail);
            return recoveryOrderDetail;
        }

        public RecoveryOrderDetail VUndoRejectObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService)
        {
            VHasBeenRejected(recoveryOrderDetail);
            if (!isValid(recoveryOrderDetail)) { return recoveryOrderDetail; }
            VRecoveryOrderHasNotBeenCompleted(recoveryOrderDetail, _recoveryOrderService);
            return recoveryOrderDetail;
        }

        public bool ValidCreateObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                  IRollerBuilderService _rollerBuilderService)
        {
            VCreateObject(recoveryOrderDetail, _recoveryOrderService, _coreIdentificationDetailService, _rollerBuilderService);
            return isValid(recoveryOrderDetail);
        }

        public bool ValidUpdateObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                          IRollerBuilderService _rollerBuilderService)
        {
            recoveryOrderDetail.Errors.Clear();
            VUpdateObject(recoveryOrderDetail, _recoveryOrderService, _coreIdentificationDetailService, _rollerBuilderService);
            return isValid(recoveryOrderDetail);
        }

        public bool ValidDeleteObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService)
        {
            recoveryOrderDetail.Errors.Clear();
            VDeleteObject(recoveryOrderDetail, _recoveryOrderService, _recoveryAccessoryDetailService);
            return isValid(recoveryOrderDetail);
        }

        public bool ValidAddAccessory(RecoveryOrderDetail recoveryOrderDetail, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService)
        {
            recoveryOrderDetail.Errors.Clear();
            VAddAccessory(recoveryOrderDetail, _recoveryAccessoryDetailService);
            return isValid(recoveryOrderDetail);
        }

        public bool ValidRemoveAccessory(RecoveryOrderDetail recoveryOrderDetail, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService)
        {
            recoveryOrderDetail.Errors.Clear();
            VRemoveAccessory(recoveryOrderDetail, _recoveryAccessoryDetailService);
            return isValid(recoveryOrderDetail);
        }

        public bool ValidDisassembleObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService)
        {
            recoveryOrderDetail.Errors.Clear();
            VDisassembleObject(recoveryOrderDetail, _recoveryOrderService);
            return isValid(recoveryOrderDetail);
        }

        public bool ValidStripAndGlueObject(RecoveryOrderDetail recoveryOrderDetail)
        {
            recoveryOrderDetail.Errors.Clear();
            VStripAndGlueObject(recoveryOrderDetail);
            return isValid(recoveryOrderDetail);
        }

        public bool ValidWrapObject(RecoveryOrderDetail recoveryOrderDetail)
        {
            recoveryOrderDetail.Errors.Clear();
            VWrapObject(recoveryOrderDetail);
            return isValid(recoveryOrderDetail);
        }

        public bool ValidVulcanizeObject(RecoveryOrderDetail recoveryOrderDetail)
        {
            recoveryOrderDetail.Errors.Clear();
            VVulcanizeObject(recoveryOrderDetail);
            return isValid(recoveryOrderDetail);
        }

        public bool ValidFaceOffObject(RecoveryOrderDetail recoveryOrderDetail)
        {
            recoveryOrderDetail.Errors.Clear();
            VFaceOffObject(recoveryOrderDetail);
            return isValid(recoveryOrderDetail);
        }

        public bool ValidConventionalGrindObject(RecoveryOrderDetail recoveryOrderDetail)
        {
            recoveryOrderDetail.Errors.Clear();
            VConventionalGrindObject(recoveryOrderDetail);
            return isValid(recoveryOrderDetail);
        }

        public bool ValidCWCGrindObject(RecoveryOrderDetail recoveryOrderDetail)
        {
            recoveryOrderDetail.Errors.Clear();
            VCWCGrindObject(recoveryOrderDetail);
            return isValid(recoveryOrderDetail);
        }

        public bool ValidPolishAndQCObject(RecoveryOrderDetail recoveryOrderDetail)
        {
            recoveryOrderDetail.Errors.Clear();
            VPolishAndQCObject(recoveryOrderDetail);
            return isValid(recoveryOrderDetail);
        }
        
        public bool ValidPackageObject(RecoveryOrderDetail recoveryOrderDetail)
        {
            recoveryOrderDetail.Errors.Clear();
            VPackageObject(recoveryOrderDetail);
            return isValid(recoveryOrderDetail);
        }

        public bool ValidFinishObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService)
        {
            recoveryOrderDetail.Errors.Clear();
            VFinishObject(recoveryOrderDetail, _recoveryOrderService, _recoveryAccessoryDetailService);
            return isValid(recoveryOrderDetail);
        }

        public bool ValidUnfinishObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService)
        {
            recoveryOrderDetail.Errors.Clear();
            VUnfinishObject(recoveryOrderDetail, _recoveryOrderService, _recoveryAccessoryDetailService);
            return isValid(recoveryOrderDetail);
        }

        public bool ValidRejectObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService)
        {
            recoveryOrderDetail.Errors.Clear();
            VRejectObject(recoveryOrderDetail, _recoveryOrderService);
            return isValid(recoveryOrderDetail);
        }

        public bool ValidUndoRejectObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService)
        {
            recoveryOrderDetail.Errors.Clear();
            VUndoRejectObject(recoveryOrderDetail, _recoveryOrderService);
            return isValid(recoveryOrderDetail);
        }

        public bool isValid(RecoveryOrderDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(RecoveryOrderDetail obj)
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
