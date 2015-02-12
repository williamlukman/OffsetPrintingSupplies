using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Validation;
using Core.DomainModel;
using Core.Interface.Service;

namespace Validation.Validation
{
    public class RollerWarehouseMutationDetailValidator : IRollerWarehouseMutationDetailValidator
    {
        public RollerWarehouseMutationDetail VHasRecoveryOrderDetail(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRecoveryOrderDetailService _recoveryOrderDetailService)
        {
            RecoveryOrderDetail recoveryOrderDetail = _recoveryOrderDetailService.GetObjectById(rollerWarehouseMutationDetail.RecoveryOrderDetailId);
            if (recoveryOrderDetail == null)
            {
                rollerWarehouseMutationDetail.Errors.Add("Generic", "Recovery work order tidak terasosiasi dengan roller identification detail");
            }
            return rollerWarehouseMutationDetail;
        }

        public RollerWarehouseMutationDetail VCoreIdentificationDetailHasNotBeenDelivered(RollerWarehouseMutationDetail rollerWarehouseMutationDetail,
                                             IRecoveryOrderDetailService _recoveryOrderDetailService, ICoreIdentificationDetailService _coreIdentificationDetailService)
        {
            RecoveryOrderDetail recoveryOrderDetail = _recoveryOrderDetailService.GetObjectById(rollerWarehouseMutationDetail.RecoveryOrderDetailId);
            CoreIdentificationDetail coreIdentificationDetail = _coreIdentificationDetailService.GetObjectById(recoveryOrderDetail.CoreIdentificationDetailId);
            if (coreIdentificationDetail.IsDelivered)
            {
                rollerWarehouseMutationDetail.Errors.Add("Generic", "Roller sudah terkirim");
            }
            return rollerWarehouseMutationDetail;
        }

        public RollerWarehouseMutationDetail VRecoveryOrderDetailHasBeenFinished(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRecoveryOrderDetailService _recoveryOrderDetailService)
        {
            RecoveryOrderDetail recoveryOrderDetail = _recoveryOrderDetailService.GetObjectById(rollerWarehouseMutationDetail.RecoveryOrderDetailId);
            if (!recoveryOrderDetail.IsFinished)
            {
                rollerWarehouseMutationDetail.Errors.Add("Generic", "Roller work order detail belum selesai");
            }
            return rollerWarehouseMutationDetail;
        }

        public RollerWarehouseMutationDetail VHasRollerWarehouseMutation(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService)
        {
            RollerWarehouseMutation rollerWarehouseMutation = _rollerWarehouseMutationService.GetObjectById(rollerWarehouseMutationDetail.RollerWarehouseMutationId);
            if (rollerWarehouseMutation == null)
            {
                rollerWarehouseMutationDetail.Errors.Add("RollerWarehouseMutationId", "Tidak terasosiasi dengan Roller Warehouse Mutation");
            }
            return rollerWarehouseMutationDetail;
        }

        public RollerWarehouseMutationDetail VHasWarehouseItemFrom(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService, IWarehouseItemService _warehouseItemService)
        {
            RollerWarehouseMutation rollerWarehouseMutation = _rollerWarehouseMutationService.GetObjectById(rollerWarehouseMutationDetail.RollerWarehouseMutationId);
            WarehouseItem warehouseItemFrom = _warehouseItemService.FindOrCreateObject(rollerWarehouseMutation.WarehouseFromId, rollerWarehouseMutationDetail.ItemId);
            if (warehouseItemFrom == null)
            {
                rollerWarehouseMutationDetail.Errors.Add("Generic", "Tidak terasosiasi dengan item dari warehouse yang sebelum");
            }
            return rollerWarehouseMutationDetail;
        }

        public RollerWarehouseMutationDetail VHasWarehouseItemTo(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService, IWarehouseItemService _warehouseItemService)
        {
            RollerWarehouseMutation rollerWarehouseMutation = _rollerWarehouseMutationService.GetObjectById(rollerWarehouseMutationDetail.RollerWarehouseMutationId);
            WarehouseItem warehouseItemTo = _warehouseItemService.FindOrCreateObject(rollerWarehouseMutation.WarehouseToId, rollerWarehouseMutationDetail.ItemId);
            if (warehouseItemTo == null)
            {
                rollerWarehouseMutationDetail.Errors.Add("Generic", "Tidak terasosiasi dengan item dari warehouse yang dituju");
            }
            return rollerWarehouseMutationDetail;
        }

        public RollerWarehouseMutationDetail VUniqueRecoveryOrderDetail(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService)
        {
            IList<RollerWarehouseMutationDetail> details = _rollerWarehouseMutationDetailService.GetObjectsByRollerWarehouseMutationId(rollerWarehouseMutationDetail.RollerWarehouseMutationId);
            foreach (var detail in details)
            {
                if (detail.RecoveryOrderDetailId == rollerWarehouseMutationDetail.RecoveryOrderDetailId && detail.Id != rollerWarehouseMutationDetail.Id)
                {
                     rollerWarehouseMutationDetail.Errors.Add("Generic", "Tidak boleh ada duplikasi roller work order detail dalam 1 Roller Warehouse Mutation");
                }
            }
            return rollerWarehouseMutationDetail;
        }

        public RollerWarehouseMutationDetail VRollerWarehouseMutationHasBeenConfirmed(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService)
        {
            RollerWarehouseMutation rollerWarehouseMutation = _rollerWarehouseMutationService.GetObjectById(rollerWarehouseMutationDetail.RollerWarehouseMutationId);
            if (!rollerWarehouseMutation.IsConfirmed)
            {
                rollerWarehouseMutationDetail.Errors.Add("Generic", "RollerWarehouseMutation harus sudah dikonfirmasi");
            }
            return rollerWarehouseMutationDetail;
        }

        public RollerWarehouseMutationDetail VRollerWarehouseMutationHasNotBeenConfirmed(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService)
        {
            RollerWarehouseMutation rollerWarehouseMutation = _rollerWarehouseMutationService.GetObjectById(rollerWarehouseMutationDetail.RollerWarehouseMutationId);
            if (rollerWarehouseMutation.IsConfirmed)
            {
                rollerWarehouseMutationDetail.Errors.Add("Generic", "RollerWarehouseMutation sudah dikonfirmasi");
            }
            return rollerWarehouseMutationDetail;
        }

        public RollerWarehouseMutationDetail VHasNotBeenConfirmed(RollerWarehouseMutationDetail rollerWarehouseMutationDetail)
        {
            if (rollerWarehouseMutationDetail.IsConfirmed)
            {
                rollerWarehouseMutationDetail.Errors.Add("Generic", "Tidak boleh sudah dikonfirmasi.");
            }
            return rollerWarehouseMutationDetail;
        }

        public RollerWarehouseMutationDetail VHasBeenConfirmed(RollerWarehouseMutationDetail rollerWarehouseMutationDetail)
        {
            if (!rollerWarehouseMutationDetail.IsConfirmed)
            {
                rollerWarehouseMutationDetail.Errors.Add("Generic", "Harus sudah dikonfirmasi.");
            }
            return rollerWarehouseMutationDetail;
        }

        public RollerWarehouseMutationDetail VNonNegativeStockQuantity(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRecoveryOrderService _recoveryOrderService, ICoreIdentificationService _coreIdentificationService,
                                                                       IRollerWarehouseMutationService _rollerWarehouseMutationService, IItemService _itemService, IBlanketService _blanketService,
                                                                       IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService, bool CaseConfirm)
        {
            int Quantity = CaseConfirm ? 1 : -1;
            RollerWarehouseMutation rollerWarehouseMutation = _rollerWarehouseMutationService.GetObjectById(rollerWarehouseMutationDetail.RollerWarehouseMutationId);
            WarehouseItem warehouseItem = null;
            if (CaseConfirm)
            {
                warehouseItem = _warehouseItemService.FindOrCreateObject(rollerWarehouseMutation.WarehouseFromId, rollerWarehouseMutationDetail.ItemId);
            }
            else
            {
                warehouseItem = _warehouseItemService.FindOrCreateObject(rollerWarehouseMutation.WarehouseToId, rollerWarehouseMutationDetail.ItemId);
            }
            Item item = _itemService.GetObjectById(warehouseItem.ItemId);
            RecoveryOrder recoveryOrder = _recoveryOrderService.GetObjectById(rollerWarehouseMutation.RecoveryOrderId);
            CoreIdentification coreIdentification = _coreIdentificationService.GetObjectById(recoveryOrder.CoreIdentificationId);
            if (coreIdentification.IsInHouse)
            {
                if (warehouseItem.Quantity + Quantity < 0)
                {
                    rollerWarehouseMutationDetail.Errors.Add("Generic", "Stock barang " + item.Name + " di warehouse " + warehouseItem.Warehouse.Name + " tinggal " + warehouseItem.Quantity);
                    return rollerWarehouseMutationDetail;
                }
            }
            else
            {
                CustomerItem customerItemFrom = _customerItemService.FindOrCreateObject(coreIdentification.ContactId.GetValueOrDefault(), warehouseItem.Id);
                if (customerItemFrom.Quantity + Quantity < 0)
                {
                    rollerWarehouseMutationDetail.Errors.Add("Generic", "Stock barang Customer " + item.Name + " di warehouse " + warehouseItem.Warehouse.Name + " tinggal " + customerItemFrom.Quantity);
                    return rollerWarehouseMutationDetail;
                }
            }
            return rollerWarehouseMutationDetail;
        }

        public RollerWarehouseMutationDetail VCreateObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService,
                                                           IRecoveryOrderDetailService _recoveryOrderDetailService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                                           IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            VHasRecoveryOrderDetail(rollerWarehouseMutationDetail, _recoveryOrderDetailService);
            if (!isValid(rollerWarehouseMutationDetail)) { return rollerWarehouseMutationDetail; }
            VCoreIdentificationDetailHasNotBeenDelivered(rollerWarehouseMutationDetail, _recoveryOrderDetailService, _coreIdentificationDetailService);
            if (!isValid(rollerWarehouseMutationDetail)) { return rollerWarehouseMutationDetail; }
            VRecoveryOrderDetailHasBeenFinished(rollerWarehouseMutationDetail, _recoveryOrderDetailService);
            if (!isValid(rollerWarehouseMutationDetail)) { return rollerWarehouseMutationDetail; }
            VHasRollerWarehouseMutation(rollerWarehouseMutationDetail, _rollerWarehouseMutationService);
            if (!isValid(rollerWarehouseMutationDetail)) { return rollerWarehouseMutationDetail; }
            VHasWarehouseItemFrom(rollerWarehouseMutationDetail, _rollerWarehouseMutationService, _warehouseItemService);
            if (!isValid(rollerWarehouseMutationDetail)) { return rollerWarehouseMutationDetail; }
            VHasWarehouseItemTo(rollerWarehouseMutationDetail, _rollerWarehouseMutationService, _warehouseItemService);
            if (!isValid(rollerWarehouseMutationDetail)) { return rollerWarehouseMutationDetail; }
            VUniqueRecoveryOrderDetail(rollerWarehouseMutationDetail, _rollerWarehouseMutationDetailService);
            if (!isValid(rollerWarehouseMutationDetail)) { return rollerWarehouseMutationDetail; }
            VRollerWarehouseMutationHasNotBeenConfirmed(rollerWarehouseMutationDetail, _rollerWarehouseMutationService);
            return rollerWarehouseMutationDetail;
        }

        public RollerWarehouseMutationDetail VUpdateObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService,
                                                           IRecoveryOrderDetailService _recoveryOrderDetailService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                                           IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            VHasNotBeenConfirmed(rollerWarehouseMutationDetail);
            if (!isValid(rollerWarehouseMutationDetail)) { return rollerWarehouseMutationDetail; }
            VCreateObject(rollerWarehouseMutationDetail, _rollerWarehouseMutationService, _recoveryOrderDetailService, _coreIdentificationDetailService,
                          _rollerWarehouseMutationDetailService, _itemService, _warehouseItemService);
            return rollerWarehouseMutationDetail;
        }

        public RollerWarehouseMutationDetail VDeleteObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail)
        {
            VHasNotBeenConfirmed(rollerWarehouseMutationDetail);
            return rollerWarehouseMutationDetail;
        }

        public RollerWarehouseMutationDetail VHasConfirmationDate(RollerWarehouseMutationDetail rollerWarehouseMutationDetail)
        {
            if (rollerWarehouseMutationDetail.ConfirmationDate == null)
            {
                rollerWarehouseMutationDetail.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return rollerWarehouseMutationDetail;
        }

        public RollerWarehouseMutationDetail VConfirmObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRecoveryOrderService _recoveryOrderService, ICoreIdentificationService _coreIdentificationService,
                                                            IRollerWarehouseMutationService _rollerWarehouseMutationService, IItemService _itemService, IBlanketService _blanketService,
                                                            IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService)
        {
            VHasConfirmationDate(rollerWarehouseMutationDetail);
            if (!isValid(rollerWarehouseMutationDetail)) { return rollerWarehouseMutationDetail; }
            VNonNegativeStockQuantity(rollerWarehouseMutationDetail, _recoveryOrderService, _coreIdentificationService, _rollerWarehouseMutationService,
                                      _itemService, _blanketService, _warehouseItemService, _customerItemService, true);
            return rollerWarehouseMutationDetail;
        }

        public RollerWarehouseMutationDetail VUnconfirmObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRecoveryOrderService _recoveryOrderService, ICoreIdentificationService _coreIdentificationService,
                                                              IRollerWarehouseMutationService _rollerWarehouseMutationService, IItemService _itemService, IBlanketService _blanketService,
                                                              IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService)
        {
            VHasBeenConfirmed(rollerWarehouseMutationDetail);
            if (!isValid(rollerWarehouseMutationDetail)) { return rollerWarehouseMutationDetail; }
            VNonNegativeStockQuantity(rollerWarehouseMutationDetail, _recoveryOrderService, _coreIdentificationService, _rollerWarehouseMutationService,
                                      _itemService, _blanketService, _warehouseItemService, _customerItemService, false);
            return rollerWarehouseMutationDetail;
        }

        public bool ValidCreateObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService,
                                      IRecoveryOrderDetailService _recoveryOrderDetailService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                      IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            VCreateObject(rollerWarehouseMutationDetail, _rollerWarehouseMutationService, _recoveryOrderDetailService, _coreIdentificationDetailService,
                          _rollerWarehouseMutationDetailService, _itemService, _warehouseItemService);
            return isValid(rollerWarehouseMutationDetail);
        }

        public bool ValidUpdateObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService,
                                      IRecoveryOrderDetailService _recoveryOrderDetailService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                      IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            rollerWarehouseMutationDetail.Errors.Clear();
            VUpdateObject(rollerWarehouseMutationDetail, _rollerWarehouseMutationService, _recoveryOrderDetailService, _coreIdentificationDetailService,
                          _rollerWarehouseMutationDetailService, _itemService, _warehouseItemService);
            return isValid(rollerWarehouseMutationDetail);
        }

        public bool ValidDeleteObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail)
        {
            rollerWarehouseMutationDetail.Errors.Clear();
            VDeleteObject(rollerWarehouseMutationDetail);
            return isValid(rollerWarehouseMutationDetail);
        }

        public bool ValidConfirmObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRecoveryOrderService _recoveryOrderService, ICoreIdentificationService _coreIdentificationService, 
                                       IRollerWarehouseMutationService _rollerWarehouseMutationService, IItemService _itemService, IBlanketService _blanketService,
                                       IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService)
        {
            rollerWarehouseMutationDetail.Errors.Clear();
            VConfirmObject(rollerWarehouseMutationDetail, _recoveryOrderService, _coreIdentificationService, _rollerWarehouseMutationService, _itemService, _blanketService, _warehouseItemService, _customerItemService);
            return isValid(rollerWarehouseMutationDetail);
        }

        public bool ValidUnconfirmObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRecoveryOrderService _recoveryOrderService, ICoreIdentificationService _coreIdentificationService, 
                                         IRollerWarehouseMutationService _rollerWarehouseMutationService, IItemService _itemService, IBlanketService _blanketService,
                                         IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService)
        {
            rollerWarehouseMutationDetail.Errors.Clear();
            VUnconfirmObject(rollerWarehouseMutationDetail, _recoveryOrderService, _coreIdentificationService, _rollerWarehouseMutationService,
                             _itemService, _blanketService, _warehouseItemService, _customerItemService);
            return isValid(rollerWarehouseMutationDetail);
        }

        public bool isValid(RollerWarehouseMutationDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(RollerWarehouseMutationDetail obj)
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