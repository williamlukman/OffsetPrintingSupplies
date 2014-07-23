using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
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
                rollerWarehouseMutationDetail.Errors.Add("CoreIdentificationDetailId", "Tidak terasosiasi dengan core identification detail");
            }
            return rollerWarehouseMutationDetail;
        }

        public RollerWarehouseMutationDetail VHasRollerWarehouseMutation(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService)
        {
            RollerWarehouseMutation rollerWarehouseMutation = _rollerWarehouseMutationService.GetObjectById(rollerWarehouseMutationDetail.RollerWarehouseMutationId);
            if (rollerWarehouseMutation == null)
            {
                rollerWarehouseMutationDetail.Errors.Add("RollerWarehouseMutationId", "Tidak terasosiasi dengan Stock Adjustment");
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

        public RollerWarehouseMutationDetail VUniqueItem(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService, IItemService _itemService)
        {
            IList<RollerWarehouseMutationDetail> details = _rollerWarehouseMutationDetailService.GetObjectsByRollerWarehouseMutationId(rollerWarehouseMutationDetail.RollerWarehouseMutationId);
            foreach (var detail in details)
            {
                if (detail.ItemId == rollerWarehouseMutationDetail.ItemId && detail.Id != rollerWarehouseMutationDetail.Id)
                {
                     rollerWarehouseMutationDetail.Errors.Add("ItemId", "Tidak boleh ada duplikasi item dalam 1 Stock Adjustment");
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

        public RollerWarehouseMutationDetail VRollerWarehouseMutationHasNotBeenCompleted(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService)
        {
            RollerWarehouseMutation rollerWarehouseMutation = _rollerWarehouseMutationService.GetObjectById(rollerWarehouseMutationDetail.RollerWarehouseMutationId);
            if (rollerWarehouseMutation.IsCompleted)
            {
                rollerWarehouseMutationDetail.Errors.Add("Generic", "RollerWarehouseMutation tidak boleh sudah complete");
            }
            return rollerWarehouseMutationDetail;
        }

        public RollerWarehouseMutationDetail VHasNotBeenFinished(RollerWarehouseMutationDetail rollerWarehouseMutationDetail)
        {
            if (rollerWarehouseMutationDetail.IsFinished)
            {
                rollerWarehouseMutationDetail.Errors.Add("IsFinished", "Tidak boleh sudah selesai.");
            }
            return rollerWarehouseMutationDetail;
        }

        public RollerWarehouseMutationDetail VHasBeenFinished(RollerWarehouseMutationDetail rollerWarehouseMutationDetail)
        {
            if (!rollerWarehouseMutationDetail.IsFinished)
            {
                rollerWarehouseMutationDetail.Errors.Add("IsFinished", "Harus sudah selesai.");
            }
            return rollerWarehouseMutationDetail;
        }

        public RollerWarehouseMutationDetail VNonNegativeStockQuantity(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService,
                                                                      IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService, bool CaseFinish)
        {
            int Quantity = CaseFinish ? 1 : -1;
            RollerWarehouseMutation rollerWarehouseMutation = _rollerWarehouseMutationService.GetObjectById(rollerWarehouseMutationDetail.RollerWarehouseMutationId);
            WarehouseItem warehouseItemFrom = _warehouseItemService.FindOrCreateObject(rollerWarehouseMutation.WarehouseFromId, rollerWarehouseMutationDetail.ItemId);
            if (warehouseItemFrom.Quantity + Quantity < 0)
            {
                rollerWarehouseMutationDetail.Errors.Add("Quantity", "Stock barang tidak boleh menjadi kurang dari 0");
                return rollerWarehouseMutationDetail;
            }
            return rollerWarehouseMutationDetail;
        }

        public RollerWarehouseMutationDetail VCreateObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                                          IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            VHasCoreIdentificationDetail(rollerWarehouseMutationDetail, _coreIdentificationDetailService);
            if (!isValid(rollerWarehouseMutationDetail)) { return rollerWarehouseMutationDetail; }
            VHasRollerWarehouseMutation(rollerWarehouseMutationDetail, _rollerWarehouseMutationService);
            if (!isValid(rollerWarehouseMutationDetail)) { return rollerWarehouseMutationDetail; }
            VHasWarehouseItemFrom(rollerWarehouseMutationDetail, _rollerWarehouseMutationService, _warehouseItemService);
            if (!isValid(rollerWarehouseMutationDetail)) { return rollerWarehouseMutationDetail; }
            VHasWarehouseItemTo(rollerWarehouseMutationDetail, _rollerWarehouseMutationService, _warehouseItemService);
            if (!isValid(rollerWarehouseMutationDetail)) { return rollerWarehouseMutationDetail; }
            VUniqueItem(rollerWarehouseMutationDetail, _rollerWarehouseMutationDetailService, _itemService);
            return rollerWarehouseMutationDetail;
        }

        public RollerWarehouseMutationDetail VUpdateObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                                          IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            VHasCoreIdentificationDetail(rollerWarehouseMutationDetail, _coreIdentificationDetailService);
            if (!isValid(rollerWarehouseMutationDetail)) { return rollerWarehouseMutationDetail; }
            VHasNotBeenFinished(rollerWarehouseMutationDetail);
            if (!isValid(rollerWarehouseMutationDetail)) { return rollerWarehouseMutationDetail; }
            VCreateObject(rollerWarehouseMutationDetail, _rollerWarehouseMutationService, _coreIdentificationDetailService, _rollerWarehouseMutationDetailService, _itemService, _warehouseItemService);
            return rollerWarehouseMutationDetail;
        }

        public RollerWarehouseMutationDetail VDeleteObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail)
        {
            VHasNotBeenFinished(rollerWarehouseMutationDetail);
            return rollerWarehouseMutationDetail;
        }

        public RollerWarehouseMutationDetail VFinishObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService,
                                                    IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            VRollerWarehouseMutationHasBeenConfirmed(rollerWarehouseMutationDetail, _rollerWarehouseMutationService);
            if (!isValid(rollerWarehouseMutationDetail)) { return rollerWarehouseMutationDetail; }
            VNonNegativeStockQuantity(rollerWarehouseMutationDetail, _rollerWarehouseMutationService, _itemService, _barringService, _warehouseItemService, true);
            return rollerWarehouseMutationDetail;
        }

        public RollerWarehouseMutationDetail VUnfinishObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService,
                                                      IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            VRollerWarehouseMutationHasBeenConfirmed(rollerWarehouseMutationDetail, _rollerWarehouseMutationService);
            if (!isValid(rollerWarehouseMutationDetail)) { return rollerWarehouseMutationDetail; }
            VRollerWarehouseMutationHasNotBeenCompleted(rollerWarehouseMutationDetail, _rollerWarehouseMutationService);
            if (!isValid(rollerWarehouseMutationDetail)) { return rollerWarehouseMutationDetail; }
            VHasBeenFinished(rollerWarehouseMutationDetail);
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

        public bool ValidFinishObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService,
                                       IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            rollerWarehouseMutationDetail.Errors.Clear();
            VFinishObject(rollerWarehouseMutationDetail, _rollerWarehouseMutationService, _itemService, _barringService, _warehouseItemService);
            return isValid(rollerWarehouseMutationDetail);
        }

        public bool ValidUnfinishObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService,
                                        IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            rollerWarehouseMutationDetail.Errors.Clear();
            VUnfinishObject(rollerWarehouseMutationDetail, _rollerWarehouseMutationService, _itemService, _barringService, _warehouseItemService);
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