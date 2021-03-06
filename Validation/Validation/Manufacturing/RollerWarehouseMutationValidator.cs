﻿using System;
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
        public RollerWarehouseMutation VHasRecoveryOrder(RollerWarehouseMutation rollerWarehouseMutation, IRecoveryOrderService _recoveryOrderService)
        {
            RecoveryOrder recoveryOrder = _recoveryOrderService.GetObjectById(rollerWarehouseMutation.RecoveryOrderId);
            if (recoveryOrder == null)
            {
                rollerWarehouseMutation.Errors.Add("RecoveryOrderId", "Tidak terasosiasi dengan recovery order");
            }
            return rollerWarehouseMutation;
        }

        public RollerWarehouseMutation VRecoveryOrderHasBeenCompleted(RollerWarehouseMutation rollerWarehouseMutation, IRecoveryOrderService _recoveryOrderService)
        {
            RecoveryOrder recoveryOrder = _recoveryOrderService.GetObjectById(rollerWarehouseMutation.RecoveryOrderId);
            if (!recoveryOrder.IsCompleted)
            {
                rollerWarehouseMutation.Errors.Add("Generic", "Recovery Order belum selesai");
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

        public RollerWarehouseMutation VHasRollerWarehouseMutationDetails(RollerWarehouseMutation rollerWarehouseMutation,
                                                                          IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService)
        {
            IList<RollerWarehouseMutationDetail> details = _rollerWarehouseMutationDetailService.GetObjectsByRollerWarehouseMutationId(rollerWarehouseMutation.Id);
            if (!details.Any())
            {
                rollerWarehouseMutation.Errors.Add("Generic", "Details tidak boleh tidak ada");
            }
            return rollerWarehouseMutation;
        }

        public RollerWarehouseMutation VQuantityIsEqualTheNumberOfDetails(RollerWarehouseMutation rollerWarehouseMutation,
                                                                          IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService)
        {
            IList<RollerWarehouseMutationDetail> details = _rollerWarehouseMutationDetailService.GetObjectsByRollerWarehouseMutationId(rollerWarehouseMutation.Id);
            if (rollerWarehouseMutation.Quantity != details.Count())
            {
                rollerWarehouseMutation.Errors.Add("Generic", "jumlah Quantity " + rollerWarehouseMutation.Quantity + " harus sama dengan jumlah detail " + details.Count());
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

        public RollerWarehouseMutation VDetailsAreVerifiedConfirmable(RollerWarehouseMutation rollerWarehouseMutation, IRollerWarehouseMutationService _rollerWarehouseMutationService,
                                                                      IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService, IItemService _itemService, 
                                                                      IBlanketService _blanketService, IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService,
                                                                      IRecoveryOrderService _recoveryOrderService, ICoreIdentificationService _coreIdentificationService)
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
                RecoveryOrder recoveryOrder = _recoveryOrderService.GetObjectById(rollerWarehouseMutation.RecoveryOrderId);
                CoreIdentification coreIdentification = _coreIdentificationService.GetObjectById(recoveryOrder.CoreIdentificationId);
                if (coreIdentification.IsInHouse)
                {
                    if (ValuePair.Value > warehouseItemFrom.Quantity)
                    {
                        rollerWarehouseMutation.Errors.Add("Generic", "Stock barang tidak boleh kurang dari stock yang mau dimutasikan");
                        return rollerWarehouseMutation;
                    }
                }
                else
                {
                    CustomerItem customerItemFrom = _customerItemService.FindOrCreateObject(coreIdentification.ContactId.GetValueOrDefault(), warehouseItemFrom.Id);
                    if (ValuePair.Value > customerItemFrom.Quantity)
                    {
                        rollerWarehouseMutation.Errors.Add("Generic", "Stock barang Customer tidak boleh kurang dari stock yang mau dimutasikan");
                        return rollerWarehouseMutation;
                    }
                }
            }
            return rollerWarehouseMutation;
        }

        public RollerWarehouseMutation VAllDetailsAreUnconfirmable(RollerWarehouseMutation rollerWarehouseMutation, IRecoveryOrderService _recoveryOrderService, ICoreIdentificationService _coreIdentificationService,
                                                                   IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService, IRollerWarehouseMutationService _rollerWarehouseMutationService,
                                                                   IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService)
        {
            IList<RollerWarehouseMutationDetail> details = _rollerWarehouseMutationDetailService.GetObjectsByRollerWarehouseMutationId(rollerWarehouseMutation.Id);
            foreach (var detail in details)
            {
                detail.Errors = new Dictionary<string, string>();
                if (!_rollerWarehouseMutationDetailService.GetValidator().ValidUnconfirmObject(detail, _recoveryOrderService, _coreIdentificationService, _rollerWarehouseMutationService, _itemService, 
                                                                                               _blanketService, _warehouseItemService, _customerItemService))
                {
                    rollerWarehouseMutation.Errors.Add("Generic", detail.Errors.FirstOrDefault().Key + " " + detail.Errors.FirstOrDefault().Value);
                    return rollerWarehouseMutation;
                }
            }
            return rollerWarehouseMutation;
        }

        public RollerWarehouseMutation VCreateObject(RollerWarehouseMutation rollerWarehouseMutation, IWarehouseService _warehouseService, IRecoveryOrderService _recoveryOrderService)
        {
            VHasRecoveryOrder(rollerWarehouseMutation, _recoveryOrderService);
            if (!isValid(rollerWarehouseMutation)) { return rollerWarehouseMutation; }
            VRecoveryOrderHasBeenCompleted(rollerWarehouseMutation, _recoveryOrderService);
            if (!isValid(rollerWarehouseMutation)) { return rollerWarehouseMutation; }
            VHasWarehouseFrom(rollerWarehouseMutation, _warehouseService);
            if (!isValid(rollerWarehouseMutation)) { return rollerWarehouseMutation; }
            VHasWarehouseTo(rollerWarehouseMutation, _warehouseService);
            if (!isValid(rollerWarehouseMutation)) { return rollerWarehouseMutation; }
            VHasDifferentWarehouse(rollerWarehouseMutation);
            return rollerWarehouseMutation;
        }

        public RollerWarehouseMutation VUpdateObject(RollerWarehouseMutation rollerWarehouseMutation, IWarehouseService _warehouseService, IRecoveryOrderService _recoveryOrderService)
        {
            VHasNotBeenConfirmed(rollerWarehouseMutation);
            if (!isValid(rollerWarehouseMutation)) { return rollerWarehouseMutation; }
            VCreateObject(rollerWarehouseMutation, _warehouseService, _recoveryOrderService);
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

        public RollerWarehouseMutation VConfirmObject(RollerWarehouseMutation rollerWarehouseMutation, IRollerWarehouseMutationService _rollerWarehouseMutationService,
                                                      IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService, IItemService _itemService, 
                                                      IBlanketService _blanketService, IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService,
                                                      IRecoveryOrderService _recoveryOrderService, ICoreIdentificationService _coreIdentificationService)
        {
            VHasConfirmationDate(rollerWarehouseMutation);
            if (!isValid(rollerWarehouseMutation)) { return rollerWarehouseMutation; }
            VHasNotBeenConfirmed(rollerWarehouseMutation);
            if (!isValid(rollerWarehouseMutation)) { return rollerWarehouseMutation; }
            VHasRollerWarehouseMutationDetails(rollerWarehouseMutation, _rollerWarehouseMutationDetailService);
            if (!isValid(rollerWarehouseMutation)) { return rollerWarehouseMutation; }
            VQuantityIsEqualTheNumberOfDetails(rollerWarehouseMutation, _rollerWarehouseMutationDetailService);
            if (!isValid(rollerWarehouseMutation)) { return rollerWarehouseMutation; }
            VDetailsAreVerifiedConfirmable(rollerWarehouseMutation, _rollerWarehouseMutationService, _rollerWarehouseMutationDetailService, _itemService,
                                           _blanketService, _warehouseItemService, _customerItemService, _recoveryOrderService, _coreIdentificationService);
            return rollerWarehouseMutation;
        }

        public RollerWarehouseMutation VUnconfirmObject(RollerWarehouseMutation rollerWarehouseMutation, IRecoveryOrderService _recoveryOrderService, ICoreIdentificationService _coreIdentificationService,
                                                        IRollerWarehouseMutationService _rollerWarehouseMutationService, IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService,
                                                        IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService)
        {
            VHasBeenConfirmed(rollerWarehouseMutation);
            if (!isValid(rollerWarehouseMutation)) { return rollerWarehouseMutation; }
            VAllDetailsAreUnconfirmable(rollerWarehouseMutation, _recoveryOrderService, _coreIdentificationService, _rollerWarehouseMutationDetailService, _rollerWarehouseMutationService, 
                                        _itemService, _blanketService, _warehouseItemService, _customerItemService);
            return rollerWarehouseMutation;
        }

        public bool ValidCreateObject(RollerWarehouseMutation rollerWarehouseMutation, IWarehouseService _warehouseService, IRecoveryOrderService _recoveryOrderService)
        {
            VCreateObject(rollerWarehouseMutation, _warehouseService, _recoveryOrderService);
            return isValid(rollerWarehouseMutation);
        }

        public bool ValidUpdateObject(RollerWarehouseMutation rollerWarehouseMutation, IWarehouseService _warehouseService, IRecoveryOrderService _recoveryOrderService)
        {
            rollerWarehouseMutation.Errors.Clear();
            VUpdateObject(rollerWarehouseMutation, _warehouseService, _recoveryOrderService);
            return isValid(rollerWarehouseMutation);
        }

        public bool ValidDeleteObject(RollerWarehouseMutation rollerWarehouseMutation)
        {
            rollerWarehouseMutation.Errors.Clear();
            VDeleteObject(rollerWarehouseMutation);
            return isValid(rollerWarehouseMutation);
        }

        public bool ValidConfirmObject(RollerWarehouseMutation rollerWarehouseMutation, IRollerWarehouseMutationService _rollerWarehouseMutationService,
                                       IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService, IItemService _itemService, 
                                       IBlanketService _blanketService, IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService,
                                       IRecoveryOrderService _recoveryOrderService, ICoreIdentificationService _coreIdentificationService)
        {
            rollerWarehouseMutation.Errors.Clear();
            VConfirmObject(rollerWarehouseMutation, _rollerWarehouseMutationService, _rollerWarehouseMutationDetailService, _itemService, _blanketService,
                           _warehouseItemService, _customerItemService, _recoveryOrderService, _coreIdentificationService);
            return isValid(rollerWarehouseMutation);
        }

        public bool ValidUnconfirmObject(RollerWarehouseMutation rollerWarehouseMutation, IRecoveryOrderService _recoveryOrderService, ICoreIdentificationService _coreIdentificationService,
                                         IRollerWarehouseMutationService _rollerWarehouseMutationService, IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService, 
                                         IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService)
        {
            rollerWarehouseMutation.Errors.Clear();
            VUnconfirmObject(rollerWarehouseMutation, _recoveryOrderService, _coreIdentificationService, _rollerWarehouseMutationService, _rollerWarehouseMutationDetailService,
                             _itemService, _blanketService, _warehouseItemService, _customerItemService);
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