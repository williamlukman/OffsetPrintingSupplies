using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class CoreIdentificationDetailValidator : ICoreIdentificationDetailValidator
    {
        public CoreIdentificationDetail VHasCoreIdentification(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService)
        {
            CoreIdentification coreIdentification = _coreIdentificationService.GetObjectById(coreIdentificationDetail.CoreIdentificationId);
            if (coreIdentification == null)
            {
                coreIdentificationDetail.Errors.Add("Generic", "CoreIdentificationDetail harus memiliki Core Identification");
            }
            return coreIdentificationDetail;
        }

        public CoreIdentificationDetail VDetailsDoNotExceedCoreIdentificationQuantity(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService,
                                                                                      ICoreIdentificationDetailService _coreIdentificationDetailService, bool CaseCreate)
        {
            CoreIdentification coreIdentification = _coreIdentificationService.GetObjectById(coreIdentificationDetail.CoreIdentificationId);
            IList<CoreIdentificationDetail> details = _coreIdentificationDetailService.GetObjectsByCoreIdentificationId(coreIdentificationDetail.CoreIdentificationId);
            if (coreIdentification.Quantity < details.Count())
            {
                coreIdentificationDetail.Errors.Add("Generic", "Jumlah detail harus " + coreIdentification.Quantity);
            }
            else if (CaseCreate && coreIdentification.Quantity == details.Count())
            {
                coreIdentificationDetail.Errors.Add("Generic", "Jumlah detail harus " + coreIdentification.Quantity);
            }
            return coreIdentificationDetail;
        }

        public CoreIdentificationDetail VHasUniqueDetailId(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationDetailService _coreIdentificationDetailService)
        {
            //IList<CoreIdentificationDetail> details = _coreIdentificationDetailService.GetObjectsByCoreIdentificationId(coreIdentificationDetail.CoreIdentificationId);
            //foreach (var detail in details)
            //{
            //    if (detail.DetailId == coreIdentificationDetail.DetailId && detail.Id != coreIdentificationDetail.Id)
            //    {
            //        coreIdentificationDetail.Errors.Add("DetailId", "Tidak boleh di duplikasi");
            //        return coreIdentificationDetail;
            //    }
            //}
            IList<CoreIdentificationDetail> details = _coreIdentificationDetailService.GetObjectsByCoreIdentificationId(coreIdentificationDetail.CoreIdentificationId);
            foreach (var detail in details)
            {
                if (detail.DetailId == coreIdentificationDetail.DetailId && detail.Id != coreIdentificationDetail.Id)
                {
                    coreIdentificationDetail.Errors.Add("Generic", "Tidak boleh di duplikasi");
                    return coreIdentificationDetail;
                }
            }
            return coreIdentificationDetail;
        }

        public CoreIdentificationDetail VHasMaterialCase(CoreIdentificationDetail coreIdentificationDetail)
        {
            if (coreIdentificationDetail.MaterialCase != Core.Constants.Constant.MaterialCase.New &&
                coreIdentificationDetail.MaterialCase != Core.Constants.Constant.MaterialCase.Used)
            {
                coreIdentificationDetail.Errors.Add("Generic", "Hanya boleh 1. New atau 2. Used");
            }
            return coreIdentificationDetail;
        }

        public CoreIdentificationDetail VHasCoreBuilder(CoreIdentificationDetail coreIdentificationDetail, ICoreBuilderService _coreBuilderService)
        {
            CoreBuilder coreBuilder = _coreBuilderService.GetObjectById(coreIdentificationDetail.CoreBuilderId);
            if (coreBuilder == null)
            {
                coreIdentificationDetail.Errors.Add("Generic", "Tidak terasosiasi dengan coreBuilder");
            }
            return coreIdentificationDetail;
        }

        public CoreIdentificationDetail VHasRollerType(CoreIdentificationDetail coreIdentificationDetail, IRollerTypeService _rollerTypeService)
        {
            RollerType rollerType = _rollerTypeService.GetObjectById(coreIdentificationDetail.RollerTypeId);
            if (rollerType == null)
            {
                coreIdentificationDetail.Errors.Add("Generic", "Tidak terasosiasi dengan rollerType");
            }
            return coreIdentificationDetail;
        }

        public CoreIdentificationDetail VHasMachine(CoreIdentificationDetail coreIdentificationDetail, IMachineService _machineService)
        {
            Machine machine = _machineService.GetObjectById(coreIdentificationDetail.MachineId);
            if (machine == null)
            {
                coreIdentificationDetail.Errors.Add("Generic", "Tidak terasosiasi dengan machine");
            }
            return coreIdentificationDetail;
        }

        public CoreIdentificationDetail VHasRepairRequestCase(CoreIdentificationDetail coreIdentificationDetail)
        {
            if (coreIdentificationDetail.RepairRequestCase != Core.Constants.Constant.RepairRequestCase.BearingSeat &&
                coreIdentificationDetail.RepairRequestCase != Core.Constants.Constant.RepairRequestCase.CentreDrill &&
                coreIdentificationDetail.RepairRequestCase != Core.Constants.Constant.RepairRequestCase.None)
            {
                coreIdentificationDetail.Errors.Add("Generic", "Hanya dapat diisi dengan 1 untuk Bearing Seat atau 2 untuk CentreDrill atau 3 None");
            }
            return coreIdentificationDetail;
        }

        public CoreIdentificationDetail VHasMeasurement(CoreIdentificationDetail coreIdentificationDetail)
        {
            if (coreIdentificationDetail.CD <= 0) { coreIdentificationDetail.Errors.Add("CD", "Tidak boleh 0 atau negatif"); return coreIdentificationDetail; }
            if (coreIdentificationDetail.RD <= 0) { coreIdentificationDetail.Errors.Add("RD", "Tidak boleh 0 atau negatif"); return coreIdentificationDetail; }
            if (coreIdentificationDetail.RL <= 0) { coreIdentificationDetail.Errors.Add("RL", "Tidak boleh 0 atau negatif"); return coreIdentificationDetail; }
            if (coreIdentificationDetail.WL <= 0) { coreIdentificationDetail.Errors.Add("WL", "Tidak boleh 0 atau negatif"); return coreIdentificationDetail; }
            if (coreIdentificationDetail.TL <= 0) { coreIdentificationDetail.Errors.Add("TL", "Tidak boleh 0 atau negatif"); return coreIdentificationDetail; }
            return coreIdentificationDetail;
        }

        public CoreIdentificationDetail VIsInRecoveryOrderDetails(CoreIdentificationDetail coreIdentificationDetail, IRecoveryOrderDetailService _recoveryOrderDetailService)
        {
            IList<RecoveryOrderDetail> details = _recoveryOrderDetailService.GetObjectsByCoreIdentificationDetailId(coreIdentificationDetail.Id);
            if (details.Any())
            {
                coreIdentificationDetail.Errors.Add("Generic", "Tidak boleh memiliki asosiasi Recovery Order Detail");
            }
            return coreIdentificationDetail;
        }

        public CoreIdentificationDetail VCoreIdentificationHasBeenConfirmed(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService)
        {
            CoreIdentification coreIdentification = _coreIdentificationService.GetObjectById(coreIdentificationDetail.CoreIdentificationId);
            if (!coreIdentification.IsConfirmed)
            {
                coreIdentificationDetail.Errors.Add("Generic", "CoreIdentification belum dikonfirmasi");
            }
            return coreIdentificationDetail;
        }

        public CoreIdentificationDetail VCoreIdentificationHasNotBeenConfirmed(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService)
        {
            CoreIdentification coreIdentification = _coreIdentificationService.GetObjectById(coreIdentificationDetail.CoreIdentificationId);
            if (coreIdentification.IsConfirmed)
            {
                coreIdentificationDetail.Errors.Add("Generic", "CoreIdentification sudah dikonfirmasi");
            }
            return coreIdentificationDetail;
        }

        public CoreIdentificationDetail VCoreIdentificationHasNotBeenCompleted(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService)
        {
            CoreIdentification coreIdentification = _coreIdentificationService.GetObjectById(coreIdentificationDetail.CoreIdentificationId);
            if (coreIdentification.IsCompleted)
            {
                coreIdentificationDetail.Errors.Add("Generic", "CoreIdentification sudah selesai");
            }
            return coreIdentificationDetail;
        }

        public CoreIdentificationDetail VHasBeenJobScheduled(CoreIdentificationDetail coreIdentificationDetail)
        {
            if (!coreIdentificationDetail.IsJobScheduled)
            {
                coreIdentificationDetail.Errors.Add("Generic", "Job masih belum di schedule");
            }
            return coreIdentificationDetail;
        }

        public CoreIdentificationDetail VHasNotBeenJobScheduled(CoreIdentificationDetail coreIdentificationDetail)
        {
            if (coreIdentificationDetail.IsJobScheduled)
            {
                coreIdentificationDetail.Errors.Add("Generic", "Job sudah terschedule");
            }
            return coreIdentificationDetail;
        }

        public CoreIdentificationDetail VHasBeenDelivered(CoreIdentificationDetail coreIdentificationDetail)
        {
            if (!coreIdentificationDetail.IsDelivered)
            {
                coreIdentificationDetail.Errors.Add("Generic", "Item belum dikirim");
            }
            return coreIdentificationDetail;
        }

        public CoreIdentificationDetail VHasNotBeenDelivered(CoreIdentificationDetail coreIdentificationDetail)
        {
            if (coreIdentificationDetail.IsDelivered)
            {
                coreIdentificationDetail.Errors.Add("Generic", "Roller sudah dikirim");
            }
            return coreIdentificationDetail;
        }

        public CoreIdentificationDetail VHasBeenRollerBuilt(CoreIdentificationDetail coreIdentificationDetail)
        {
            if (!coreIdentificationDetail.IsRollerBuilt)
            {
                coreIdentificationDetail.Errors.Add("Generic", "Roller is not built yet");
            }
            return coreIdentificationDetail;
        }

        public CoreIdentificationDetail VHasNotBeenConfirmed(CoreIdentificationDetail coreIdentificationDetail)
        {
            if (coreIdentificationDetail.IsConfirmed)
            {
                coreIdentificationDetail.Errors.Add("Generic", "CoreIdentificationDetail sudah selesai");
            }
            return coreIdentificationDetail;
        }

        public CoreIdentificationDetail VHasConfirmationDate(CoreIdentificationDetail coreIdentificationDetail)
        {
            if (coreIdentificationDetail.ConfirmationDate == null)
            {
                coreIdentificationDetail.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return coreIdentificationDetail;
        }

        public CoreIdentificationDetail VQuantityIsInStockForInHouse(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService, ICoreBuilderService _coreBuilderService, IWarehouseItemService _warehouseItemService)
        {
            CoreIdentification coreIdentification = _coreIdentificationService.GetObjectById(coreIdentificationDetail.CoreIdentificationId);
            if (coreIdentification.IsInHouse)
            {
                int MaterialCase = coreIdentificationDetail.MaterialCase;
                Item item = (MaterialCase == Core.Constants.Constant.MaterialCase.New ?
                                _coreBuilderService.GetNewCore(coreIdentificationDetail.CoreBuilderId) :
                                _coreBuilderService.GetUsedCore(coreIdentificationDetail.CoreBuilderId));
                WarehouseItem warehouseItem = _warehouseItemService.FindOrCreateObject(coreIdentification.WarehouseId, item.Id);
                if (warehouseItem.Quantity < 1)
                {
                    coreIdentificationDetail.Errors.Add("Generic", "Stock barang tidak boleh kurang dari stock yang mau dimutasikan");
                }
            }
            return coreIdentificationDetail;
        }

        public CoreIdentificationDetail VQuantityIsInStockForContact(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService, ICoreBuilderService _coreBuilderService, IWarehouseItemService _warehouseItemService)
        {
            CoreIdentification coreIdentification = _coreIdentificationService.GetObjectById(coreIdentificationDetail.CoreIdentificationId);
            if (coreIdentification.ContactId != null)
            {
                int MaterialCase = coreIdentificationDetail.MaterialCase;
                Item item = (MaterialCase == Core.Constants.Constant.MaterialCase.New ?
                                _coreBuilderService.GetNewCore(coreIdentificationDetail.CoreBuilderId) :
                                _coreBuilderService.GetUsedCore(coreIdentificationDetail.CoreBuilderId));
                WarehouseItem warehouseItem = _warehouseItemService.FindOrCreateObject(coreIdentification.WarehouseId, item.Id);
                if (warehouseItem.Quantity < 1)
                {
                    coreIdentificationDetail.Errors.Add("Generic", "Stock barang tidak boleh kurang dari stock yang mau dimutasikan");
                }
            }
            return coreIdentificationDetail;
        }

        public CoreIdentificationDetail VCustomerQuantityIsInStockForUnconfirm(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService, ICoreBuilderService _coreBuilderService, ICustomerItemService _customerItemService, IWarehouseItemService _warehouseItemService)
        {
            CoreIdentification coreIdentification = _coreIdentificationService.GetObjectById(coreIdentificationDetail.CoreIdentificationId);
            if (!coreIdentification.IsInHouse)
            {
                int MaterialCase = coreIdentificationDetail.MaterialCase;
                Item item = (MaterialCase == Core.Constants.Constant.MaterialCase.New ?
                                _coreBuilderService.GetNewCore(coreIdentificationDetail.CoreBuilderId) :
                                _coreBuilderService.GetUsedCore(coreIdentificationDetail.CoreBuilderId));
                WarehouseItem warehouseItem = _warehouseItemService.FindOrCreateObject(coreIdentification.WarehouseId, item.Id);
                CustomerItem customerItem = _customerItemService.FindOrCreateObject(coreIdentification.ContactId.GetValueOrDefault(), warehouseItem.Id);
                if (customerItem.Quantity < 1)
                {
                    coreIdentificationDetail.Errors.Add("Generic", "Stock barang customer tidak boleh kurang dari stock yang mau dimutasikan");
                }
            }
            return coreIdentificationDetail;
        }

        public CoreIdentificationDetail VCreateObject(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService,
                                                      ICoreIdentificationDetailService _coreIdentificationDetailService, ICoreBuilderService _coreBuilderService,
                                                      IRollerTypeService _rollerTypeService, IMachineService _machineService, IWarehouseItemService _warehouseItemService)
        {
            VHasCoreIdentification(coreIdentificationDetail, _coreIdentificationService);
            if (!isValid(coreIdentificationDetail)) { return coreIdentificationDetail; }
            VHasUniqueDetailId(coreIdentificationDetail, _coreIdentificationDetailService);
            if (!isValid(coreIdentificationDetail)) { return coreIdentificationDetail; }
            VHasMaterialCase(coreIdentificationDetail);
            if (!isValid(coreIdentificationDetail)) { return coreIdentificationDetail; }
            VHasCoreBuilder(coreIdentificationDetail, _coreBuilderService);
            if (!isValid(coreIdentificationDetail)) { return coreIdentificationDetail; }
            VHasMachine(coreIdentificationDetail, _machineService);
            if (!isValid(coreIdentificationDetail)) { return coreIdentificationDetail; }
            VHasMeasurement(coreIdentificationDetail);
            if (!isValid(coreIdentificationDetail)) { return coreIdentificationDetail; }
            VHasRollerType(coreIdentificationDetail, _rollerTypeService);
            if (!isValid(coreIdentificationDetail)) { return coreIdentificationDetail; }
            VQuantityIsInStockForInHouse(coreIdentificationDetail, _coreIdentificationService, _coreBuilderService, _warehouseItemService);
            if (!isValid(coreIdentificationDetail)) { return coreIdentificationDetail; }
            VHasRepairRequestCase(coreIdentificationDetail);
            if (!isValid(coreIdentificationDetail)) { return coreIdentificationDetail; }
            // specific for create, with CaseCreate = true
            VDetailsDoNotExceedCoreIdentificationQuantity(coreIdentificationDetail, _coreIdentificationService, _coreIdentificationDetailService, true);
            return coreIdentificationDetail;
        }

        public CoreIdentificationDetail VUpdateObject(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService,
                                                      ICoreIdentificationDetailService _coreIdentificationDetailService, ICoreBuilderService _coreBuilderService,
                                                      IRollerTypeService _rollerTypeService, IMachineService _machineService, IWarehouseItemService _warehouseItemService)
        {
            VHasCoreIdentification(coreIdentificationDetail, _coreIdentificationService);
            if (!isValid(coreIdentificationDetail)) { return coreIdentificationDetail; }
            VHasUniqueDetailId(coreIdentificationDetail, _coreIdentificationDetailService);
            if (!isValid(coreIdentificationDetail)) { return coreIdentificationDetail; }
            VHasMaterialCase(coreIdentificationDetail);
            if (!isValid(coreIdentificationDetail)) { return coreIdentificationDetail; }
            VHasCoreBuilder(coreIdentificationDetail, _coreBuilderService);
            if (!isValid(coreIdentificationDetail)) { return coreIdentificationDetail; }
            VHasMachine(coreIdentificationDetail, _machineService);
            if (!isValid(coreIdentificationDetail)) { return coreIdentificationDetail; }
            VHasMeasurement(coreIdentificationDetail);
            if (!isValid(coreIdentificationDetail)) { return coreIdentificationDetail; }
            VHasRollerType(coreIdentificationDetail, _rollerTypeService);
            if (!isValid(coreIdentificationDetail)) { return coreIdentificationDetail; }
            VQuantityIsInStockForInHouse(coreIdentificationDetail, _coreIdentificationService, _coreBuilderService, _warehouseItemService);
            if (!isValid(coreIdentificationDetail)) { return coreIdentificationDetail; }
            VHasRepairRequestCase(coreIdentificationDetail);
            if (!isValid(coreIdentificationDetail)) { return coreIdentificationDetail; }
            // specific for update, with CaseCreate = false
            VDetailsDoNotExceedCoreIdentificationQuantity(coreIdentificationDetail, _coreIdentificationService, _coreIdentificationDetailService, false);
            if (!isValid(coreIdentificationDetail)) { return coreIdentificationDetail; }
            VHasNotBeenConfirmed(coreIdentificationDetail);
            return coreIdentificationDetail;
        }

        public CoreIdentificationDetail VConfirmObject(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService,
                                                       ICoreIdentificationDetailService _coreIdentificationDetailService, ICoreBuilderService _coreBuilderService,
                                                       IWarehouseItemService _warehouseItemService)
        {
            VQuantityIsInStockForInHouse(coreIdentificationDetail, _coreIdentificationService, _coreBuilderService, _warehouseItemService);
            if (!isValid(coreIdentificationDetail)) { return coreIdentificationDetail; }
            VHasConfirmationDate(coreIdentificationDetail);
            return coreIdentificationDetail;
        }

        public CoreIdentificationDetail VUnconfirmObject(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService,
                                                          ICoreIdentificationDetailService _coreIdentificationDetailService, ICoreBuilderService _coreBuilderService, IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService)
        {
            VCoreIdentificationHasBeenConfirmed(coreIdentificationDetail, _coreIdentificationService);
            if (!isValid(coreIdentificationDetail)) { return coreIdentificationDetail; }
            VHasNotBeenJobScheduled(coreIdentificationDetail);
            if (!isValid(coreIdentificationDetail)) { return coreIdentificationDetail; }
            VQuantityIsInStockForContact(coreIdentificationDetail, _coreIdentificationService, _coreBuilderService, _warehouseItemService);
            if (!isValid(coreIdentificationDetail)) { return coreIdentificationDetail; }
            VCustomerQuantityIsInStockForUnconfirm(coreIdentificationDetail, _coreIdentificationService, _coreBuilderService, _customerItemService, _warehouseItemService);
            return coreIdentificationDetail;
        }

        public CoreIdentificationDetail VSetJobScheduled(CoreIdentificationDetail coreIdentificationDetail, IRecoveryOrderService _recoveryOrderService,
                                                         IRecoveryOrderDetailService _recoveryOrderDetailService)
        {
            IList<RecoveryOrderDetail> details = _recoveryOrderDetailService.GetObjectsByCoreIdentificationDetailId(coreIdentificationDetail.Id);
            if (!details.Any())
            {
                coreIdentificationDetail.Errors.Add("Generic", "Tidak ada job yang ter schedule");
                return coreIdentificationDetail;
            }
            foreach (var detail in details)
            {
                RecoveryOrder recoveryOrder = _recoveryOrderService.GetObjectById(detail.RecoveryOrderId);
                if (!recoveryOrder.IsConfirmed)
                {
                    coreIdentificationDetail.Errors.Add("Generic", "Job masih belum terkonfirmasi");
                    return coreIdentificationDetail;
                }
            }
            return coreIdentificationDetail;
        }

        public CoreIdentificationDetail VUnsetJobScheduled(CoreIdentificationDetail coreIdentificationDetail, IRecoveryOrderService _recoveryOrderService,
                                                         IRecoveryOrderDetailService _recoveryOrderDetailService)
        {
            IList<RecoveryOrderDetail> details = _recoveryOrderDetailService.GetObjectsByCoreIdentificationDetailId(coreIdentificationDetail.Id);
            if (!details.Any())
            {
                return coreIdentificationDetail;
            }
            bool IsFinished = false;
            foreach (var detail in details)
            {
                if (detail.IsFinished || detail.IsRejected)
                {
                    IsFinished = true;
                    break;
                }
            }
            if (!IsFinished)
            {
                coreIdentificationDetail.Errors.Add("Generic", "Belum bisa di unset Job Schedule");
            }

            return coreIdentificationDetail;
        }

        public CoreIdentificationDetail VDeliverObject(CoreIdentificationDetail coreIdentificationDetail, IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService)
        {
            return coreIdentificationDetail;
        }

        public CoreIdentificationDetail VUndoDeliverObject(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService, IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService)
        {
            //VCoreIdentificationHasNotBeenCompleted(coreIdentificationDetail, _coreIdentificationService);
            return coreIdentificationDetail;
        }

        public CoreIdentificationDetail VDeleteObject(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService,
                                                      IRecoveryOrderDetailService _recoveryOrderDetailService, IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService)
        {
            VCoreIdentificationHasNotBeenConfirmed(coreIdentificationDetail, _coreIdentificationService);
            if (!isValid(coreIdentificationDetail)) { return coreIdentificationDetail; }
            VIsInRecoveryOrderDetails(coreIdentificationDetail, _recoveryOrderDetailService);
            return coreIdentificationDetail;
        }

        public bool ValidCreateObject(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService,
                                                      ICoreIdentificationDetailService _coreIdentificationDetailService, ICoreBuilderService _coreBuilderService,
                                                      IRollerTypeService _rollerTypeService, IMachineService _machineService, IWarehouseItemService _warehouseItemService)
        {
            VCreateObject(coreIdentificationDetail, _coreIdentificationService, _coreIdentificationDetailService, _coreBuilderService,
                          _rollerTypeService, _machineService, _warehouseItemService);
            return isValid(coreIdentificationDetail);
        }

        public bool ValidUpdateObject(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService,
                                      ICoreIdentificationDetailService _coreIdentificationDetailService, ICoreBuilderService _coreBuilderService,
                                      IRollerTypeService _rollerTypeService, IMachineService _machineService, IWarehouseItemService _warehouseItemService)
        {
            coreIdentificationDetail.Errors.Clear();
            VUpdateObject(coreIdentificationDetail, _coreIdentificationService, _coreIdentificationDetailService, _coreBuilderService,
                          _rollerTypeService, _machineService, _warehouseItemService);
            return isValid(coreIdentificationDetail);
        }

        public bool ValidConfirmObject(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService,
                               ICoreIdentificationDetailService _coreIdentificationDetailService, ICoreBuilderService _coreBuilderService, IWarehouseItemService _warehouseItemService)
        {
            coreIdentificationDetail.Errors.Clear();
            VConfirmObject(coreIdentificationDetail, _coreIdentificationService, _coreIdentificationDetailService, _coreBuilderService, _warehouseItemService);
            return isValid(coreIdentificationDetail);
        }

        public bool ValidUnconfirmObject(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService,
                               ICoreIdentificationDetailService _coreIdentificationDetailService, ICoreBuilderService _coreBuilderService, IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService)
        {
            coreIdentificationDetail.Errors.Clear();
            VUnconfirmObject(coreIdentificationDetail, _coreIdentificationService, _coreIdentificationDetailService, _coreBuilderService, _warehouseItemService, _customerItemService);
            return isValid(coreIdentificationDetail);
        }

        public bool ValidSetJobScheduled(CoreIdentificationDetail coreIdentificationDetail, IRecoveryOrderService _recoveryOrderService,
                                         IRecoveryOrderDetailService _recoveryOrderDetailService)
        {
            coreIdentificationDetail.Errors.Clear();
            VSetJobScheduled(coreIdentificationDetail, _recoveryOrderService, _recoveryOrderDetailService);
            return isValid(coreIdentificationDetail);
        }

        public bool ValidUnsetJobScheduled(CoreIdentificationDetail coreIdentificationDetail, IRecoveryOrderService _recoveryOrderService,
                                           IRecoveryOrderDetailService _recoveryOrderDetailService)
        {
            coreIdentificationDetail.Errors.Clear();
            VUnsetJobScheduled(coreIdentificationDetail, _recoveryOrderService, _recoveryOrderDetailService);
            return isValid(coreIdentificationDetail);
        }

        public bool ValidDeliverObject(CoreIdentificationDetail coreIdentificationDetail, IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService)
        {
            coreIdentificationDetail.Errors.Clear();
            VDeliverObject(coreIdentificationDetail, _rollerWarehouseMutationDetailService);
            return isValid(coreIdentificationDetail);
        }

        public bool ValidUndoDeliverObject(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService, IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService)
        {
            coreIdentificationDetail.Errors.Clear();
            VUndoDeliverObject(coreIdentificationDetail, _coreIdentificationService, _rollerWarehouseMutationDetailService);
            return isValid(coreIdentificationDetail);
        }
        public bool ValidDeleteObject(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService,
                                      IRecoveryOrderDetailService _recoveryOrderDetailService, IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService)
        {
            coreIdentificationDetail.Errors.Clear();
            VDeleteObject(coreIdentificationDetail, _coreIdentificationService, _recoveryOrderDetailService, _rollerWarehouseMutationDetailService);
            return isValid(coreIdentificationDetail);
        }

        public bool isValid(CoreIdentificationDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(CoreIdentificationDetail obj)
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
