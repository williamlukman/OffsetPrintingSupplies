using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Validation;
using Core.DomainModel;
using Core.Interface.Service;

namespace Validation.Validation
{
    public class WarehouseMutationValidator : IWarehouseMutationValidator
    {

        public WarehouseMutation VHasDifferentWarehouse(WarehouseMutation WarehouseMutation)
        {
            if (WarehouseMutation.WarehouseFromId == WarehouseMutation.WarehouseToId)
            {
                WarehouseMutation.Errors.Add("Generic", "Warehouse sebelum dan sesudah tidak boleh sama");
            }
            return WarehouseMutation;
        }

        public WarehouseMutation VHasWarehouseFrom(WarehouseMutation WarehouseMutation, IWarehouseService _warehouseService)
        {
            Warehouse warehouseFrom = _warehouseService.GetObjectById(WarehouseMutation.WarehouseFromId);
            if (warehouseFrom == null)
            {
                WarehouseMutation.Errors.Add("WarehouseFromId", "Tidak terasosiasi dengan warehouse");
            }
            return WarehouseMutation;
        }

        public WarehouseMutation VHasWarehouseTo(WarehouseMutation WarehouseMutation, IWarehouseService _warehouseService)
        {
            Warehouse warehouseTo = _warehouseService.GetObjectById(WarehouseMutation.WarehouseToId);
            if (warehouseTo == null)
            {
                WarehouseMutation.Errors.Add("WarehouseToId", "Tidak terasosiasi dengan warehouse");
            }
            return WarehouseMutation;
        }

        public WarehouseMutation VHasWarehouseMutationDetails(WarehouseMutation WarehouseMutation, IWarehouseMutationDetailService _WarehouseMutationDetailService)
        {
            IList<WarehouseMutationDetail> details = _WarehouseMutationDetailService.GetObjectsByWarehouseMutationId(WarehouseMutation.Id);
            if (!details.Any())
            {
                WarehouseMutation.Errors.Add("Generic", "Details tidak boleh tidak ada");
            }
            return WarehouseMutation;
        }

        public WarehouseMutation VHasMutationDate(WarehouseMutation WarehouseMutation)
        {
            if (WarehouseMutation.MutationDate == null)
            {
                WarehouseMutation.Errors.Add("MutationDate", "Tidak boleh kosong");
            }
            return WarehouseMutation;
        }

        public WarehouseMutation VHasNotBeenConfirmed(WarehouseMutation WarehouseMutation)
        {
            if (WarehouseMutation.IsConfirmed)
            {
                WarehouseMutation.Errors.Add("Generic", "Tidak boleh sudah dikonfirmasi");
            }
            return WarehouseMutation;
        }

        public WarehouseMutation VHasBeenConfirmed(WarehouseMutation WarehouseMutation)
        {
            if (!WarehouseMutation.IsConfirmed)
            {
                WarehouseMutation.Errors.Add("Generic", "Harus sudah dikonfirmasi");
            }
            return WarehouseMutation;
        }

        public WarehouseMutation VDetailsAreVerifiedConfirmable(WarehouseMutation WarehouseMutation, IWarehouseMutationService _WarehouseMutationService, IWarehouseMutationDetailService _WarehouseMutationDetailService,
                                                                     IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            IList<WarehouseMutationDetail> details = _WarehouseMutationDetailService.GetObjectsByWarehouseMutationId(WarehouseMutation.Id);
            foreach (var detail in details)
            {
                WarehouseItem warehouseItemFrom = _warehouseItemService.FindOrCreateObject(WarehouseMutation.WarehouseFromId, detail.ItemId);
                if (warehouseItemFrom.Quantity < detail.Quantity)
                {
                    WarehouseMutation.Errors.Add("Generic", "Stock barang tidak boleh kurang dari stock yang akan dimutasikan");
                    return WarehouseMutation;
                }
            }
            return WarehouseMutation;
        }

        public WarehouseMutation VCreateObject(WarehouseMutation WarehouseMutation, IWarehouseService _warehouseService)
        {
            VHasDifferentWarehouse(WarehouseMutation);
            if (!isValid(WarehouseMutation)) { return WarehouseMutation; }
            VHasWarehouseFrom(WarehouseMutation, _warehouseService);
            if (!isValid(WarehouseMutation)) { return WarehouseMutation; }
            VHasWarehouseTo(WarehouseMutation, _warehouseService);
            if (!isValid(WarehouseMutation)) { return WarehouseMutation; }
            VHasMutationDate(WarehouseMutation);
            return WarehouseMutation;
        }

        public WarehouseMutation VUpdateObject(WarehouseMutation WarehouseMutation, IWarehouseService _warehouseService)
        {
            VHasNotBeenConfirmed(WarehouseMutation);
            if (!isValid(WarehouseMutation)) { return WarehouseMutation; }
            VCreateObject(WarehouseMutation, _warehouseService);
            return WarehouseMutation;
        }

        public WarehouseMutation VDeleteObject(WarehouseMutation WarehouseMutation)
        {
            VHasNotBeenConfirmed(WarehouseMutation);
            return WarehouseMutation;
        }

        public WarehouseMutation VHasConfirmationDate(WarehouseMutation obj)
        {
            if (obj.ConfirmationDate == null)
            {
                obj.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return obj;
        }

        public WarehouseMutation VConfirmObject(WarehouseMutation WarehouseMutation, IWarehouseMutationService _WarehouseMutationService, IWarehouseMutationDetailService _WarehouseMutationDetailService,
                                                     IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            VHasConfirmationDate(WarehouseMutation);
            if (!isValid(WarehouseMutation)) { return WarehouseMutation; }
            VHasNotBeenConfirmed(WarehouseMutation);
            if (!isValid(WarehouseMutation)) { return WarehouseMutation; }
            VHasWarehouseMutationDetails(WarehouseMutation, _WarehouseMutationDetailService);
            if (!isValid(WarehouseMutation)) { return WarehouseMutation; }
            VDetailsAreVerifiedConfirmable(WarehouseMutation, _WarehouseMutationService, _WarehouseMutationDetailService, _itemService, _barringService, _warehouseItemService);
            return WarehouseMutation;
        }

        public WarehouseMutation VUnconfirmObject(WarehouseMutation WarehouseMutation, IWarehouseMutationService _WarehouseMutationService, IWarehouseMutationDetailService _WarehouseMutationDetailService,
                                                       IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            VHasBeenConfirmed(WarehouseMutation);
            return WarehouseMutation;
        }

        public bool ValidCreateObject(WarehouseMutation WarehouseMutation, IWarehouseService _warehouseService)
        {
            VCreateObject(WarehouseMutation, _warehouseService);
            return isValid(WarehouseMutation);
        }

        public bool ValidUpdateObject(WarehouseMutation WarehouseMutation, IWarehouseService _warehouseService)
        {
            WarehouseMutation.Errors.Clear();
            VUpdateObject(WarehouseMutation, _warehouseService);
            return isValid(WarehouseMutation);
        }

        public bool ValidDeleteObject(WarehouseMutation WarehouseMutation)
        {
            WarehouseMutation.Errors.Clear();
            VDeleteObject(WarehouseMutation);
            return isValid(WarehouseMutation);
        }

        public bool ValidConfirmObject(WarehouseMutation WarehouseMutation, IWarehouseMutationService _WarehouseMutationService, IWarehouseMutationDetailService _WarehouseMutationDetailService,
                                       IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            WarehouseMutation.Errors.Clear();
            VConfirmObject(WarehouseMutation, _WarehouseMutationService, _WarehouseMutationDetailService, _itemService, _barringService, _warehouseItemService);
            return isValid(WarehouseMutation);
        }

        public bool ValidUnconfirmObject(WarehouseMutation WarehouseMutation, IWarehouseMutationService _WarehouseMutationService, IWarehouseMutationDetailService _WarehouseMutationDetailService,
                                         IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            WarehouseMutation.Errors.Clear();
            VUnconfirmObject(WarehouseMutation, _WarehouseMutationService, _WarehouseMutationDetailService, _itemService, _barringService, _warehouseItemService);
            return isValid(WarehouseMutation);
        }

        public bool isValid(WarehouseMutation obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(WarehouseMutation obj)
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