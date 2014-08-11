using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class RecoveryOrderValidator : IRecoveryOrderValidator
    {
        public RecoveryOrder VHasUniqueCode(RecoveryOrder recoveryOrder, IRecoveryOrderService _recoveryOrderService)
        {
            if (String.IsNullOrEmpty(recoveryOrder.Code) || recoveryOrder.Code.Trim() == "")
            {
                recoveryOrder.Errors.Add("Code", "Tidak boleh kosong");
            }
            if (_recoveryOrderService.IsCodeDuplicated(recoveryOrder))
            {
                recoveryOrder.Errors.Add("Code", "Tidak boleh ada duplikasi");
            }
            return recoveryOrder;
        }

        public RecoveryOrder VHasCoreIdentificationAndConfirmed(RecoveryOrder recoveryOrder, ICoreIdentificationService _coreIdentificationService)
        {
            CoreIdentification coreIdentification = _coreIdentificationService.GetObjectById(recoveryOrder.CoreIdentificationId);
            if (coreIdentification == null)
            {
                recoveryOrder.Errors.Add("CoreIdentificationId", "Tidak terasosiasi dengan core identification");
            }
            else if (!coreIdentification.IsConfirmed)
            {
                recoveryOrder.Errors.Add("CoreIdentifcationId", "Belum dikonfirmasi");
            }
            return recoveryOrder;
        }

        public RecoveryOrder VHasRecoveryOrderDetails(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService)
        {
            IList<RecoveryOrderDetail> details = _recoveryOrderDetailService.GetObjectsByRecoveryOrderId(recoveryOrder.Id);
            if (!details.Any())
            {
                recoveryOrder.Errors.Add("Generic", "Harus membuat recovery order detail dahulu");
            }
            return recoveryOrder;
        }

        public RecoveryOrder VHasQuantityReceived(RecoveryOrder recoveryOrder)
        {
            if (recoveryOrder.QuantityReceived <= 0)
            {
                recoveryOrder.Errors.Add("QuantityReceived", "Harus lebih dari 0");
            }
            return recoveryOrder;
        }

        public RecoveryOrder VQuantityFinalAndRejectedIsLessThanOrEqualQuantityReceived(RecoveryOrder recoveryOrder)
        {
            if (recoveryOrder.QuantityFinal + recoveryOrder.QuantityRejected > recoveryOrder.QuantityReceived)
            {
                recoveryOrder.Errors.Add("Generic", "jumlah sudah melebihi jumalah yang diterima diawal");
            }
            return recoveryOrder;
        }

        public RecoveryOrder VQuantityReceivedLessThanCoreIdentification(RecoveryOrder recoveryOrder, ICoreIdentificationService _coreIdentificationService)
        {
            CoreIdentification coreIdentification = _coreIdentificationService.GetObjectById(recoveryOrder.CoreIdentificationId);
            if (coreIdentification.Quantity < recoveryOrder.QuantityReceived)
            {
                recoveryOrder.Errors.Add("QuantityReceived", "Harus sama atau lebih sedikit dari Core Identification");
            }
            return recoveryOrder;
        }

        public RecoveryOrder VQuantityReceivedEqualDetails(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService)
        {
            IList<RecoveryOrderDetail> details = _recoveryOrderDetailService.GetObjectsByRecoveryOrderId(recoveryOrder.Id);
            if (recoveryOrder.QuantityReceived != details.Count())
            {
                recoveryOrder.Errors.Add("QuantityReceived", "Jumlah quantity received dan jumlah recovery order detail tidak sama");
            }
            return recoveryOrder;
        }

        public RecoveryOrder VQuantityIsInStock(RecoveryOrder recoveryOrder, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                                IRecoveryOrderDetailService _recoveryOrderDetailService, ICoreBuilderService _coreBuilderService, IItemService _itemService,
                                                IWarehouseItemService _warehouseItemService)
        {
            IList<RecoveryOrderDetail> details = _recoveryOrderDetailService.GetObjectsByRecoveryOrderId(recoveryOrder.Id);
            IDictionary<int, int> ValuePairItemIdQuantity = new Dictionary<int, int>();
            foreach (var detail in details)
            {
                CoreIdentificationDetail coreIdentificationDetail = _coreIdentificationDetailService.GetObjectById(detail.CoreIdentificationDetailId);
                CoreBuilder coreBuilder = _coreBuilderService.GetObjectById(coreIdentificationDetail.CoreBuilderId);
                Item item = (coreIdentificationDetail.MaterialCase == Core.Constants.Constant.MaterialCase.New) ?
                            _coreBuilderService.GetNewCore(coreBuilder.Id) : _coreBuilderService.GetUsedCore(coreBuilder.Id);
                WarehouseItem warehouseItem = _warehouseItemService.FindOrCreateObject(recoveryOrder.WarehouseId, item.Id);
                if (ValuePairItemIdQuantity.ContainsKey(warehouseItem.Id))
                {
                    ValuePairItemIdQuantity[warehouseItem.Id] += 1;
                }
                else
                {
                    ValuePairItemIdQuantity.Add(warehouseItem.Id, 1);
                }
            }

            foreach (var ValuePair in ValuePairItemIdQuantity)
            {
                WarehouseItem warehouseItem = _warehouseItemService.GetObjectById(ValuePair.Key);
                if (warehouseItem.Quantity < ValuePair.Value)
                {
                    recoveryOrder.Errors.Add("Generic", "Stock quantity core item tidak boleh kurang dari jumlah di dalam recovery order");
                    return recoveryOrder;
                }
            }
            return recoveryOrder;
        }
        
        public RecoveryOrder VHasBeenConfirmed(RecoveryOrder recoveryOrder)
        {
            if (!recoveryOrder.IsConfirmed)
            {
                recoveryOrder.Errors.Add("Generic", "Belum dikonfirmasi");
            }
            return recoveryOrder;
        }

        public RecoveryOrder VHasNotBeenConfirmed(RecoveryOrder recoveryOrder)
        {
            if (recoveryOrder.IsConfirmed)
            {
                recoveryOrder.Errors.Add("Generic", "Sudah dikonfirmasi");
            }
            return recoveryOrder;
        }

        public RecoveryOrder VHasBeenCompleted(RecoveryOrder recoveryOrder)
        {
            if (!recoveryOrder.IsCompleted)
            {
                recoveryOrder.Errors.Add("Generic", "Belum complete");
            }
            return recoveryOrder;
        }

        public RecoveryOrder VHasNotBeenCompleted(RecoveryOrder recoveryOrder)
        {
            if (recoveryOrder.IsCompleted)
            {
                recoveryOrder.Errors.Add("Generic", "Sudah complete");
            }
            return recoveryOrder;
        }

        public RecoveryOrder VAllDetailsHaveBeenFinishedOrRejected(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService)
        {
            IList<RecoveryOrderDetail> details = _recoveryOrderDetailService.GetObjectsByRecoveryOrderId(recoveryOrder.Id);
            foreach (var detail in details)
            {
                if (!detail.IsFinished && !detail.IsRejected)
                {
                    recoveryOrder.Errors.Add("Generic", "Semua recovery order detail harus telah selesai atau di reject");
                    return recoveryOrder;
                }
            }
            return recoveryOrder;
        }

        public RecoveryOrder VAllDetailsHaveBeenDisassembledOrRejected(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService)
        {
            IList<RecoveryOrderDetail> details = _recoveryOrderDetailService.GetObjectsByRecoveryOrderId(recoveryOrder.Id);
            foreach (var detail in details)
            {
                if (!detail.IsDisassembled && !detail.IsRejected)
                {
                    recoveryOrder.Errors.Add("Generic", "Semua recovery order detail harus telah di disassemble atau di reject");
                    return recoveryOrder;
                }
            }
            return recoveryOrder;
        }

        public RecoveryOrder VAllDetailsHaveNotBeenDisassembledNorRejected(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService)
        {
            IList<RecoveryOrderDetail> details = _recoveryOrderDetailService.GetObjectsByRecoveryOrderId(recoveryOrder.Id);
            foreach (var detail in details)
            {
                if (detail.IsDisassembled || detail.IsRejected)
                {
                    recoveryOrder.Errors.Add("Generic", "Semua recovery order detail harus belum di disassemble atau di reject");
                    return recoveryOrder;
                }
            }
            return recoveryOrder;
        }

        public RecoveryOrder VCreateObject(RecoveryOrder recoveryOrder, ICoreIdentificationService _coreIdentificationService, IRecoveryOrderService _recoveryOrderService)
        {
            VHasCoreIdentificationAndConfirmed(recoveryOrder, _coreIdentificationService);
            if (!isValid(recoveryOrder)) { return recoveryOrder; }
            VHasUniqueCode(recoveryOrder, _recoveryOrderService);
            if (!isValid(recoveryOrder)) { return recoveryOrder; }
            VQuantityReceivedLessThanCoreIdentification(recoveryOrder, _coreIdentificationService);
            if (!isValid(recoveryOrder)) { return recoveryOrder; }
            VHasQuantityReceived(recoveryOrder);
            return recoveryOrder;
        }

        public RecoveryOrder VUpdateObject(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService, ICoreIdentificationService _coreIdentificationService, IRecoveryOrderService _recoveryOrderService)
        {
            VHasNotBeenConfirmed(recoveryOrder);
            if (!isValid(recoveryOrder)) { return recoveryOrder; }
            VCreateObject(recoveryOrder, _coreIdentificationService, _recoveryOrderService);
            return recoveryOrder;
        }

        public RecoveryOrder VDeleteObject(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService)
        {
            VHasNotBeenConfirmed(recoveryOrder);
            if (!isValid(recoveryOrder)) { return recoveryOrder; }
            // TODO
            // VHaveNoRecoveryOrderDetails(recoveryOrder, _recoveryOrderDetailService);
            return recoveryOrder;
        }

        public RecoveryOrder VHasConfirmationDate(RecoveryOrder obj)
        {
            if (obj.ConfirmationDate == null)
            {
                obj.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return obj;
        }

        public RecoveryOrder VConfirmObject(RecoveryOrder recoveryOrder, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                            IRecoveryOrderDetailService _recoveryOrderDetailService, ICoreBuilderService _coreBuilderService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            VHasConfirmationDate(recoveryOrder);
            if (!isValid(recoveryOrder)) { return recoveryOrder; }
            VHasNotBeenConfirmed(recoveryOrder);
            if (!isValid(recoveryOrder)) { return recoveryOrder; }
            VHasRecoveryOrderDetails(recoveryOrder, _recoveryOrderDetailService);
            if (!isValid(recoveryOrder)) { return recoveryOrder; }
            VQuantityReceivedEqualDetails(recoveryOrder, _recoveryOrderDetailService);
            if (!isValid(recoveryOrder)) { return recoveryOrder; }
            VQuantityIsInStock(recoveryOrder, _coreIdentificationDetailService, _recoveryOrderDetailService, _coreBuilderService, _itemService, _warehouseItemService);
            return recoveryOrder;
        }

        public RecoveryOrder VUnconfirmObject(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService)
        {
            VHasBeenConfirmed(recoveryOrder);
            if (!isValid(recoveryOrder)) { return recoveryOrder; }
            VHasNotBeenCompleted(recoveryOrder);
            return recoveryOrder;
        }

        public RecoveryOrder VAdjustQuantity(RecoveryOrder recoveryOrder)
        {
            VQuantityFinalAndRejectedIsLessThanOrEqualQuantityReceived(recoveryOrder);
            return recoveryOrder;
        }

        public RecoveryOrder VCompleteObject(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService)
        {
            VHasBeenConfirmed(recoveryOrder);
            if (!isValid(recoveryOrder)) { return recoveryOrder; }
            VHasNotBeenCompleted(recoveryOrder);
            if (!isValid(recoveryOrder)) { return recoveryOrder; }
            VAllDetailsHaveBeenFinishedOrRejected(recoveryOrder, _recoveryOrderDetailService);
            return recoveryOrder;
        }

        public bool ValidCreateObject(RecoveryOrder recoveryOrder, ICoreIdentificationService _coreIdentificationService, IRecoveryOrderService _recoveryOrderService)
        {
            VCreateObject(recoveryOrder, _coreIdentificationService, _recoveryOrderService);
            return isValid(recoveryOrder);
        }

        public bool ValidUpdateObject(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService, ICoreIdentificationService _coreIdentificationService, IRecoveryOrderService _recoveryOrderService)
        {
            recoveryOrder.Errors.Clear();
            VUpdateObject(recoveryOrder, _recoveryOrderDetailService, _coreIdentificationService, _recoveryOrderService);
            return isValid(recoveryOrder);
        }

        public bool ValidDeleteObject(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService)
        {
            recoveryOrder.Errors.Clear();
            VDeleteObject(recoveryOrder, _recoveryOrderDetailService, _recoveryAccessoryDetailService);
            return isValid(recoveryOrder);
        }

        public bool ValidConfirmObject(RecoveryOrder recoveryOrder, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                       IRecoveryOrderDetailService _recoveryOrderDetailService, ICoreBuilderService _coreBuilderService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            recoveryOrder.Errors.Clear();
            VConfirmObject(recoveryOrder, _coreIdentificationDetailService, _recoveryOrderDetailService, _coreBuilderService, _itemService, _warehouseItemService);
            return isValid(recoveryOrder);
        }

        public bool ValidUnconfirmObject(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService)
        {
            recoveryOrder.Errors.Clear();
            VUnconfirmObject(recoveryOrder, _recoveryOrderDetailService, _recoveryAccessoryDetailService);
            return isValid(recoveryOrder);
        }

        public bool ValidCompleteObject(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService)
        {
            recoveryOrder.Errors.Clear();
            VCompleteObject(recoveryOrder, _recoveryOrderDetailService, _recoveryAccessoryDetailService);
            return isValid(recoveryOrder);
        }

        public bool ValidAdjustQuantity(RecoveryOrder recoveryOrder)
        {
            recoveryOrder.Errors.Clear();
            VAdjustQuantity(recoveryOrder);
            return isValid(recoveryOrder);
        }

        public bool isValid(RecoveryOrder obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(RecoveryOrder obj)
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
