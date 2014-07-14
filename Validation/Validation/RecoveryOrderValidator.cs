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
                WarehouseItem warehouseItem = _warehouseItemService.GetObjectByWarehouseAndItem(recoveryOrder.WarehouseId, item.Id);
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

        public RecoveryOrder VHasBeenFinished(RecoveryOrder recoveryOrder)
        {
            if (!recoveryOrder.IsFinished)
            {
                recoveryOrder.Errors.Add("Generic", "Belum di finish");
            }
            return recoveryOrder;
        }

        public RecoveryOrder VHasNotBeenFinished(RecoveryOrder recoveryOrder)
        {
            if (recoveryOrder.IsFinished)
            {
                recoveryOrder.Errors.Add("Generic", "Sudah di finish");
            }
            return recoveryOrder;
        }

        public RecoveryOrder VAllAccessoriesHaveNotBeenConfirmed(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService)
        {
            IList<RecoveryOrderDetail> details = _recoveryOrderDetailService.GetObjectsByRecoveryOrderId(recoveryOrder.Id);
            IList<RecoveryAccessoryDetail> accessories = new List<RecoveryAccessoryDetail>();
            foreach (var detail in details)
            {
                // populate accessories
                _recoveryAccessoryDetailService.GetObjectsByRecoveryOrderDetailId(detail.Id).ToList().ForEach(x => accessories.Add(x));
            }
            foreach (var accessory in accessories)
            {
                if (accessory.IsConfirmed)
                {
                    recoveryOrder.Errors.Add("Generic", "Semua accessories tidak boleh telah dikonfirmasi");
                    return recoveryOrder;
                }
            }
            return recoveryOrder;
        }

        public RecoveryOrder VAllAccessoriesHaveBeenConfirmed(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService)
        {
            IList<RecoveryOrderDetail> details = _recoveryOrderDetailService.GetObjectsByRecoveryOrderId(recoveryOrder.Id);
            IList<RecoveryAccessoryDetail> accessories = new List<RecoveryAccessoryDetail>();
            foreach (var detail in details)
            {
                // populate accessories
                _recoveryAccessoryDetailService.GetObjectsByRecoveryOrderDetailId(detail.Id).ToList().ForEach(x => accessories.Add(x));
            }
            foreach (var accessory in accessories)
            {
                if (!accessory.IsConfirmed)
                {
                    recoveryOrder.Errors.Add("Generic", "Semua accessories harus telah dikonfirmasi");
                    return recoveryOrder;
                }
            }
            return recoveryOrder;
        }

        public RecoveryOrder VAllDetailsHaveBeenPackagedOrRejected(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService)
        {
            IList<RecoveryOrderDetail> details = _recoveryOrderDetailService.GetObjectsByRecoveryOrderId(recoveryOrder.Id);
            foreach (var detail in details)
            {
                if (!detail.IsPackaged && !detail.IsRejected)
                {
                    recoveryOrder.Errors.Add("Generic", "Semua recovery order detail harus telah di package atau di reject");
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
            VAllDetailsHaveNotBeenDisassembledNorRejected(recoveryOrder, _recoveryOrderDetailService);
            if (!isValid(recoveryOrder)) { return recoveryOrder; }
            VAllAccessoriesHaveNotBeenConfirmed(recoveryOrder, _recoveryOrderDetailService, _recoveryAccessoryDetailService);
            return recoveryOrder;
        }

        public RecoveryOrder VConfirmObject(RecoveryOrder recoveryOrder, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                            IRecoveryOrderDetailService _recoveryOrderDetailService, ICoreBuilderService _coreBuilderService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
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
            VHasNotBeenFinished(recoveryOrder);
            if (!isValid(recoveryOrder)) { return recoveryOrder; }
            VAllDetailsHaveNotBeenDisassembledNorRejected(recoveryOrder, _recoveryOrderDetailService);
            if (!isValid(recoveryOrder)) { return recoveryOrder; }
            VAllAccessoriesHaveNotBeenConfirmed(recoveryOrder, _recoveryOrderDetailService, _recoveryAccessoryDetailService);
            return recoveryOrder;
        }

        public RecoveryOrder VFinishObject(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService)
        {
            VHasBeenConfirmed(recoveryOrder);
            if (!isValid(recoveryOrder)) { return recoveryOrder; }
            VHasNotBeenFinished(recoveryOrder);
            if (!isValid(recoveryOrder)) { return recoveryOrder; }
            VAllDetailsHaveBeenPackagedOrRejected(recoveryOrder, _recoveryOrderDetailService);
            if (!isValid(recoveryOrder)) { return recoveryOrder; }
            VAllAccessoriesHaveBeenConfirmed(recoveryOrder, _recoveryOrderDetailService, _recoveryAccessoryDetailService);
            return recoveryOrder;
        }

        public RecoveryOrder VUnfinishObject(RecoveryOrder recoveryOrder)
        {
            VHasBeenFinished(recoveryOrder);
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

        public bool ValidFinishObject(RecoveryOrder recoveryOrder, IRecoveryOrderDetailService _recoveryOrderDetailService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService)
        {
            recoveryOrder.Errors.Clear();
            VFinishObject(recoveryOrder, _recoveryOrderDetailService, _recoveryAccessoryDetailService);
            return isValid(recoveryOrder);
        }

        public bool ValidUnfinishObject(RecoveryOrder recoveryOrder)
        {
            recoveryOrder.Errors.Clear();
            VUnfinishObject(recoveryOrder);
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
