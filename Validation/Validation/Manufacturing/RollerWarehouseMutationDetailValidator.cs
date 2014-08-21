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
        public RollerWarehouseMutationDetail VHasCoreIdentificationDetail(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, ICoreIdentificationDetailService _coreIdentificationDetailService)
        {
            CoreIdentificationDetail coreIdentificationDetail = _coreIdentificationDetailService.GetObjectById(rollerWarehouseMutationDetail.CoreIdentificationDetailId);
            if (coreIdentificationDetail == null)
            {
                rollerWarehouseMutationDetail.Errors.Add("Generic", "Core identification detail tidak terasosiasi dengan core identification detail");
            }
            return rollerWarehouseMutationDetail;
        }

        public RollerWarehouseMutationDetail VCoreIdentificationDetailHasNotBeenDelivered(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, ICoreIdentificationDetailService _coreIdentificationDetailService)
        {
            CoreIdentificationDetail coreIdentificationDetail = _coreIdentificationDetailService.GetObjectById(rollerWarehouseMutationDetail.CoreIdentificationDetailId);
            if (coreIdentificationDetail.IsDelivered)
            {
                rollerWarehouseMutationDetail.Errors.Add("Generic", "Core Identification Detail Sudah terkirim");
            }
            return rollerWarehouseMutationDetail;
        }

        public RollerWarehouseMutationDetail VCoreIdentificationDetailHasBeenConfirmed(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, ICoreIdentificationDetailService _coreIdentificationDetailService)
        {
            CoreIdentificationDetail coreIdentificationDetail = _coreIdentificationDetailService.GetObjectById(rollerWarehouseMutationDetail.CoreIdentificationDetailId);
            if (!coreIdentificationDetail.IsConfirmed)
            {
                rollerWarehouseMutationDetail.Errors.Add("Generic", "Core Identification Detail belum dikonfirmasi");
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

        public RollerWarehouseMutationDetail VUniqueCoreIdentificationDetail(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService)
        {
            IList<RollerWarehouseMutationDetail> details = _rollerWarehouseMutationDetailService.GetObjectsByRollerWarehouseMutationId(rollerWarehouseMutationDetail.RollerWarehouseMutationId);
            foreach (var detail in details)
            {
                if (detail.CoreIdentificationDetailId == rollerWarehouseMutationDetail.CoreIdentificationDetailId && detail.Id != rollerWarehouseMutationDetail.Id)
                {
                     rollerWarehouseMutationDetail.Errors.Add("Generic", "Tidak boleh ada duplikasi core identification detail dalam 1 Roller Warehouse Mutation");
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

        public RollerWarehouseMutationDetail VRollerWarehouseMutationHasNotBeenCompleted(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService)
        {
            RollerWarehouseMutation rollerWarehouseMutation = _rollerWarehouseMutationService.GetObjectById(rollerWarehouseMutationDetail.RollerWarehouseMutationId);
            if (rollerWarehouseMutation.IsCompleted)
            {
                rollerWarehouseMutationDetail.Errors.Add("Generic", "RollerWarehouseMutation tidak boleh sudah complete");
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

        public RollerWarehouseMutationDetail VNonNegativeStockQuantity(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService,
                                                                      IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService, bool CaseConfirm)
        {
            int Quantity = CaseConfirm ? 1 : -1;
            RollerWarehouseMutation rollerWarehouseMutation = _rollerWarehouseMutationService.GetObjectById(rollerWarehouseMutationDetail.RollerWarehouseMutationId);
            WarehouseItem warehouseItemFrom = _warehouseItemService.FindOrCreateObject(rollerWarehouseMutation.WarehouseFromId, rollerWarehouseMutationDetail.ItemId);
            Item item = _itemService.GetObjectById(warehouseItemFrom.ItemId);
            if (warehouseItemFrom.Quantity + Quantity < 0)
            {
                rollerWarehouseMutationDetail.Errors.Add("Generic", "Stock barang " + item.Name + "di warehouse terpilih tinggal " + warehouseItemFrom.Quantity);
                return rollerWarehouseMutationDetail;
            }
            return rollerWarehouseMutationDetail;
        }

        public RollerWarehouseMutationDetail VCreateObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                                          IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            VHasCoreIdentificationDetail(rollerWarehouseMutationDetail, _coreIdentificationDetailService);
            if (!isValid(rollerWarehouseMutationDetail)) { return rollerWarehouseMutationDetail; }
            VCoreIdentificationDetailHasNotBeenDelivered(rollerWarehouseMutationDetail, _coreIdentificationDetailService);
            if (!isValid(rollerWarehouseMutationDetail)) { return rollerWarehouseMutationDetail; }
            VCoreIdentificationDetailHasBeenConfirmed(rollerWarehouseMutationDetail, _coreIdentificationDetailService);
            if (!isValid(rollerWarehouseMutationDetail)) { return rollerWarehouseMutationDetail; }
            VHasRollerWarehouseMutation(rollerWarehouseMutationDetail, _rollerWarehouseMutationService);
            if (!isValid(rollerWarehouseMutationDetail)) { return rollerWarehouseMutationDetail; }
            VHasWarehouseItemFrom(rollerWarehouseMutationDetail, _rollerWarehouseMutationService, _warehouseItemService);
            if (!isValid(rollerWarehouseMutationDetail)) { return rollerWarehouseMutationDetail; }
            VHasWarehouseItemTo(rollerWarehouseMutationDetail, _rollerWarehouseMutationService, _warehouseItemService);
            if (!isValid(rollerWarehouseMutationDetail)) { return rollerWarehouseMutationDetail; }
            VUniqueCoreIdentificationDetail(rollerWarehouseMutationDetail, _rollerWarehouseMutationDetailService);
            if (!isValid(rollerWarehouseMutationDetail)) { return rollerWarehouseMutationDetail; }
            VRollerWarehouseMutationHasNotBeenConfirmed(rollerWarehouseMutationDetail, _rollerWarehouseMutationService);
            return rollerWarehouseMutationDetail;
        }

        public RollerWarehouseMutationDetail VUpdateObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                                          IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            VHasCoreIdentificationDetail(rollerWarehouseMutationDetail, _coreIdentificationDetailService);
            if (!isValid(rollerWarehouseMutationDetail)) { return rollerWarehouseMutationDetail; }
            VHasNotBeenConfirmed(rollerWarehouseMutationDetail);
            if (!isValid(rollerWarehouseMutationDetail)) { return rollerWarehouseMutationDetail; }
            VCreateObject(rollerWarehouseMutationDetail, _rollerWarehouseMutationService, _coreIdentificationDetailService, _rollerWarehouseMutationDetailService, _itemService, _warehouseItemService);
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

        public RollerWarehouseMutationDetail VConfirmObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService,
                                                            IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            VHasConfirmationDate(rollerWarehouseMutationDetail);
            if (!isValid(rollerWarehouseMutationDetail)) { return rollerWarehouseMutationDetail; }
            VNonNegativeStockQuantity(rollerWarehouseMutationDetail, _rollerWarehouseMutationService, _itemService, _barringService, _warehouseItemService, true);
            return rollerWarehouseMutationDetail;
        }

        public RollerWarehouseMutationDetail VUnconfirmObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService,
                                                      IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            VRollerWarehouseMutationHasNotBeenCompleted(rollerWarehouseMutationDetail, _rollerWarehouseMutationService);
            if (!isValid(rollerWarehouseMutationDetail)) { return rollerWarehouseMutationDetail; }
            VHasBeenConfirmed(rollerWarehouseMutationDetail);
            if (!isValid(rollerWarehouseMutationDetail)) { return rollerWarehouseMutationDetail; }
            VNonNegativeStockQuantity(rollerWarehouseMutationDetail, _rollerWarehouseMutationService, _itemService, _barringService, _warehouseItemService, false);
            return rollerWarehouseMutationDetail;
        }

        public bool ValidCreateObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                      IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            VCreateObject(rollerWarehouseMutationDetail, _rollerWarehouseMutationService, _coreIdentificationDetailService, _rollerWarehouseMutationDetailService, _itemService, _warehouseItemService);
            return isValid(rollerWarehouseMutationDetail);
        }

        public bool ValidUpdateObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                      IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            rollerWarehouseMutationDetail.Errors.Clear();
            VUpdateObject(rollerWarehouseMutationDetail, _rollerWarehouseMutationService, _coreIdentificationDetailService, _rollerWarehouseMutationDetailService, _itemService, _warehouseItemService);
            return isValid(rollerWarehouseMutationDetail);
        }

        public bool ValidDeleteObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail)
        {
            rollerWarehouseMutationDetail.Errors.Clear();
            VDeleteObject(rollerWarehouseMutationDetail);
            return isValid(rollerWarehouseMutationDetail);
        }

        public bool ValidConfirmObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService,
                                       IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            rollerWarehouseMutationDetail.Errors.Clear();
            VConfirmObject(rollerWarehouseMutationDetail, _rollerWarehouseMutationService, _itemService, _barringService, _warehouseItemService);
            return isValid(rollerWarehouseMutationDetail);
        }

        public bool ValidUnconfirmObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService,
                                        IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            rollerWarehouseMutationDetail.Errors.Clear();
            VUnconfirmObject(rollerWarehouseMutationDetail, _rollerWarehouseMutationService, _itemService, _barringService, _warehouseItemService);
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