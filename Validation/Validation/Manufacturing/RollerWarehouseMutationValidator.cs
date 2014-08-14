using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Validation;
using Core.DomainModel;
using Core.Interface.Service;

namespace Validation.Validation
{
    public class RollerWarehouseMutationValidator : IRollerWarehouseMutationValidator
    {
        public RollerWarehouseMutation VHasCoreIdentification(RollerWarehouseMutation rollerWarehouseMutation, ICoreIdentificationService _coreIdentificationService)
        {
            CoreIdentification coreIdentification = _coreIdentificationService.GetObjectById(rollerWarehouseMutation.CoreIdentificationId);
            if (coreIdentification == null)
            {
                rollerWarehouseMutation.Errors.Add("CoreIdentificationId", "Tidak terasosiasi dengan core identification");
            }
            return rollerWarehouseMutation;
        }

        public RollerWarehouseMutation VHasDifferentWarehouse(RollerWarehouseMutation rollerWarehouseMutation)
        {
            if (rollerWarehouseMutation.WarehouseFromId == rollerWarehouseMutation.WarehouseToId)
            {
                rollerWarehouseMutation.Errors.Add("Generic", "Warehouse sebelum dan sesudah tidak boleh sama");
            }
            return rollerWarehouseMutation;
        }

        public RollerWarehouseMutation VHasWarehouseFrom(RollerWarehouseMutation rollerWarehouseMutation, IWarehouseService _warehouseService)
        {
            Warehouse warehouseFrom = _warehouseService.GetObjectById(rollerWarehouseMutation.WarehouseFromId);
            if (warehouseFrom == null)
            {
                rollerWarehouseMutation.Errors.Add("WarehouseFromId", "Tidak terasosiasi dengan warehouse");
            }
            return rollerWarehouseMutation;
        }

        public RollerWarehouseMutation VHasWarehouseTo(RollerWarehouseMutation rollerWarehouseMutation, IWarehouseService _warehouseService)
        {
            Warehouse warehouseTo = _warehouseService.GetObjectById(rollerWarehouseMutation.WarehouseToId);
            if (warehouseTo == null)
            {
                rollerWarehouseMutation.Errors.Add("WarehouseToId", "Tidak terasosiasi dengan warehouse");
            }
            return rollerWarehouseMutation;
        }

        public RollerWarehouseMutation VHasMutationDate(RollerWarehouseMutation rollerWarehouseMutation)
        {
            if (rollerWarehouseMutation.MutationDate == null)
            {
                rollerWarehouseMutation.Errors.Add("MutationDate", "Tidak boleh kosong");
            }
            return rollerWarehouseMutation;
        }

        public RollerWarehouseMutation VHasRollerWarehouseMutationDetails(RollerWarehouseMutation rollerWarehouseMutation, IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService)
        {
            IList<RollerWarehouseMutationDetail> details = _rollerWarehouseMutationDetailService.GetObjectsByRollerWarehouseMutationId(rollerWarehouseMutation.Id);
            if (!details.Any())
            {
                rollerWarehouseMutation.Errors.Add("Generic", "Details tidak boleh tidak ada");
            }
            return rollerWarehouseMutation;
        }

        public RollerWarehouseMutation VHasNotBeenConfirmed(RollerWarehouseMutation rollerWarehouseMutation)
        {
            if (rollerWarehouseMutation.IsConfirmed)
            {
                rollerWarehouseMutation.Errors.Add("Generic", "Tidak boleh sudah dikonfirmasi");
            }
            return rollerWarehouseMutation;
        }

        public RollerWarehouseMutation VHasBeenConfirmed(RollerWarehouseMutation rollerWarehouseMutation)
        {
            if (!rollerWarehouseMutation.IsConfirmed)
            {
                rollerWarehouseMutation.Errors.Add("Generic", "Harus sudah dikonfirmasi");
            }
            return rollerWarehouseMutation;
        }

        public RollerWarehouseMutation VDetailsAreVerifiedConfirmable(RollerWarehouseMutation rollerWarehouseMutation, IRollerWarehouseMutationService _rollerWarehouseMutationService, IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService,
                                                                     IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            IList<RollerWarehouseMutationDetail> details = _rollerWarehouseMutationDetailService.GetObjectsByRollerWarehouseMutationId(rollerWarehouseMutation.Id);
            IDictionary<int, int> ValuePairWarehouseItemIdQuantity = new Dictionary<int, int>();
            foreach (var detail in details)
            {
                WarehouseItem warehouseItemFrom = _warehouseItemService.FindOrCreateObject(rollerWarehouseMutation.WarehouseFromId, detail.ItemId);
                if (ValuePairWarehouseItemIdQuantity.ContainsKey(warehouseItemFrom.Id))
                {
                    ValuePairWarehouseItemIdQuantity[warehouseItemFrom.Id] += 1;
                }
                else
                {
                    ValuePairWarehouseItemIdQuantity.Add(warehouseItemFrom.Id, 1);
                }
            }

            foreach(var ValuePair in ValuePairWarehouseItemIdQuantity)
            {
                WarehouseItem warehouseItemFrom = _warehouseItemService.GetObjectById(ValuePair.Key);
                if (ValuePair.Value > warehouseItemFrom.Quantity)
                {
                    rollerWarehouseMutation.Errors.Add("Generic", "Stock barang tidak boleh kurang dari stock yang mau dimutasikan");
                    return rollerWarehouseMutation;
                }
            }
            return rollerWarehouseMutation;
        }

        public RollerWarehouseMutation VCreateObject(RollerWarehouseMutation rollerWarehouseMutation, IWarehouseService _warehouseService, ICoreIdentificationService _coreIdentificationService)
        {
            VHasCoreIdentification(rollerWarehouseMutation, _coreIdentificationService);
            if (!isValid(rollerWarehouseMutation)) { return rollerWarehouseMutation; }
            VHasDifferentWarehouse(rollerWarehouseMutation);
            if (!isValid(rollerWarehouseMutation)) { return rollerWarehouseMutation; }
            VHasWarehouseFrom(rollerWarehouseMutation, _warehouseService);
            if (!isValid(rollerWarehouseMutation)) { return rollerWarehouseMutation; }
            VHasWarehouseTo(rollerWarehouseMutation, _warehouseService);
            return rollerWarehouseMutation;
        }

        public RollerWarehouseMutation VUpdateObject(RollerWarehouseMutation rollerWarehouseMutation, IWarehouseService _warehouseService, ICoreIdentificationService _coreIdentificationService)
        {
            VHasNotBeenConfirmed(rollerWarehouseMutation);
            if (!isValid(rollerWarehouseMutation)) { return rollerWarehouseMutation; }
            VCreateObject(rollerWarehouseMutation, _warehouseService, _coreIdentificationService);
            return rollerWarehouseMutation;
        }

        public RollerWarehouseMutation VDeleteObject(RollerWarehouseMutation rollerWarehouseMutation)
        {
            VHasNotBeenConfirmed(rollerWarehouseMutation);
            return rollerWarehouseMutation;
        }

        public RollerWarehouseMutation VHasConfirmationDate(RollerWarehouseMutation obj)
        {
            if (obj.ConfirmationDate == null)
            {
                obj.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return obj;
        }

        public RollerWarehouseMutation VConfirmObject(RollerWarehouseMutation rollerWarehouseMutation, IRollerWarehouseMutationService _rollerWarehouseMutationService, IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService,
                                              IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            VHasConfirmationDate(rollerWarehouseMutation);
            if (!isValid(rollerWarehouseMutation)) { return rollerWarehouseMutation; }
            VHasNotBeenConfirmed(rollerWarehouseMutation);
            if (!isValid(rollerWarehouseMutation)) { return rollerWarehouseMutation; }
            VHasRollerWarehouseMutationDetails(rollerWarehouseMutation, _rollerWarehouseMutationDetailService);
            if (!isValid(rollerWarehouseMutation)) { return rollerWarehouseMutation; }
            VDetailsAreVerifiedConfirmable(rollerWarehouseMutation, _rollerWarehouseMutationService, _rollerWarehouseMutationDetailService, _itemService, _barringService, _warehouseItemService);
            return rollerWarehouseMutation;
        }

        public RollerWarehouseMutation VUnconfirmObject(RollerWarehouseMutation rollerWarehouseMutation, IRollerWarehouseMutationService _rollerWarehouseMutationService, IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService,
                                                IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            VHasBeenConfirmed(rollerWarehouseMutation);
            return rollerWarehouseMutation;
        }

        public bool ValidCreateObject(RollerWarehouseMutation rollerWarehouseMutation, IWarehouseService _warehouseService, ICoreIdentificationService _coreIdentificationService)
        {
            VCreateObject(rollerWarehouseMutation, _warehouseService, _coreIdentificationService);
            return isValid(rollerWarehouseMutation);
        }

        public bool ValidUpdateObject(RollerWarehouseMutation rollerWarehouseMutation, IWarehouseService _warehouseService, ICoreIdentificationService _coreIdentificationService)
        {
            rollerWarehouseMutation.Errors.Clear();
            VUpdateObject(rollerWarehouseMutation, _warehouseService, _coreIdentificationService);
            return isValid(rollerWarehouseMutation);
        }

        public bool ValidDeleteObject(RollerWarehouseMutation rollerWarehouseMutation)
        {
            rollerWarehouseMutation.Errors.Clear();
            VDeleteObject(rollerWarehouseMutation);
            return isValid(rollerWarehouseMutation);
        }

        public bool ValidConfirmObject(RollerWarehouseMutation rollerWarehouseMutation, IRollerWarehouseMutationService _rollerWarehouseMutationService, IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService,
                                       IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            rollerWarehouseMutation.Errors.Clear();
            VConfirmObject(rollerWarehouseMutation, _rollerWarehouseMutationService, _rollerWarehouseMutationDetailService, _itemService, _barringService, _warehouseItemService);
            return isValid(rollerWarehouseMutation);
        }

        public bool ValidUnconfirmObject(RollerWarehouseMutation rollerWarehouseMutation, IRollerWarehouseMutationService _rollerWarehouseMutationService, IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService,
                                         IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            rollerWarehouseMutation.Errors.Clear();
            VUnconfirmObject(rollerWarehouseMutation, _rollerWarehouseMutationService, _rollerWarehouseMutationDetailService, _itemService, _barringService, _warehouseItemService);
            return isValid(rollerWarehouseMutation);
        }

        public bool isValid(RollerWarehouseMutation obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(RollerWarehouseMutation obj)
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