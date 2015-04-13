using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Validation;
using Core.DomainModel;
using Core.Interface.Service;

namespace Validation.Validation
{
    public class BlanketWarehouseMutationValidator : IBlanketWarehouseMutationValidator
    {
        public BlanketWarehouseMutation VHasBlanketOrder(BlanketWarehouseMutation blanketWarehouseMutation, IBlanketOrderService _blanketOrderService)
        {
            BlanketOrder blanketOrder = _blanketOrderService.GetObjectById(blanketWarehouseMutation.BlanketOrderId);
            if (blanketOrder == null)
            {
                blanketWarehouseMutation.Errors.Add("BlanketOrderId", "Tidak terasosiasi dengan blanket order");
            }
            return blanketWarehouseMutation;
        }

        public BlanketWarehouseMutation VBlanketOrderHasBeenCompleted(BlanketWarehouseMutation blanketWarehouseMutation, IBlanketOrderService _blanketOrderService)
        {
            BlanketOrder blanketOrder = _blanketOrderService.GetObjectById(blanketWarehouseMutation.BlanketOrderId);
            if (!blanketOrder.IsCompleted)
            {
                blanketWarehouseMutation.Errors.Add("Generic", "Blanket Order belum selesai");
            }
            return blanketWarehouseMutation;
        }

        public BlanketWarehouseMutation VHasDifferentWarehouse(BlanketWarehouseMutation blanketWarehouseMutation)
        {
            if (blanketWarehouseMutation.WarehouseFromId == blanketWarehouseMutation.WarehouseToId)
            {
                blanketWarehouseMutation.Errors.Add("Generic", "Warehouse sebelum dan sesudah tidak boleh sama");
            }
            return blanketWarehouseMutation;
        }

        public BlanketWarehouseMutation VHasWarehouseFrom(BlanketWarehouseMutation blanketWarehouseMutation, IWarehouseService _warehouseService)
        {
            Warehouse warehouseFrom = _warehouseService.GetObjectById(blanketWarehouseMutation.WarehouseFromId);
            if (warehouseFrom == null)
            {
                blanketWarehouseMutation.Errors.Add("WarehouseFromId", "Tidak terasosiasi dengan warehouse");
            }
            return blanketWarehouseMutation;
        }

        public BlanketWarehouseMutation VHasWarehouseTo(BlanketWarehouseMutation blanketWarehouseMutation, IWarehouseService _warehouseService)
        {
            Warehouse warehouseTo = _warehouseService.GetObjectById(blanketWarehouseMutation.WarehouseToId);
            if (warehouseTo == null)
            {
                blanketWarehouseMutation.Errors.Add("WarehouseToId", "Tidak terasosiasi dengan warehouse");
            }
            return blanketWarehouseMutation;
        }

        public BlanketWarehouseMutation VHasMutationDate(BlanketWarehouseMutation blanketWarehouseMutation)
        {
            if (blanketWarehouseMutation.MutationDate == null)
            {
                blanketWarehouseMutation.Errors.Add("MutationDate", "Tidak boleh kosong");
            }
            return blanketWarehouseMutation;
        }

        public BlanketWarehouseMutation VHasBlanketWarehouseMutationDetails(BlanketWarehouseMutation blanketWarehouseMutation,
                                                                          IBlanketWarehouseMutationDetailService _blanketWarehouseMutationDetailService)
        {
            IList<BlanketWarehouseMutationDetail> details = _blanketWarehouseMutationDetailService.GetObjectsByBlanketWarehouseMutationId(blanketWarehouseMutation.Id);
            if (!details.Any())
            {
                blanketWarehouseMutation.Errors.Add("Generic", "Details tidak boleh tidak ada");
            }
            return blanketWarehouseMutation;
        }

        public BlanketWarehouseMutation VQuantityIsEqualTheNumberOfDetails(BlanketWarehouseMutation blanketWarehouseMutation,
                                                                          IBlanketWarehouseMutationDetailService _blanketWarehouseMutationDetailService)
        {
            IList<BlanketWarehouseMutationDetail> details = _blanketWarehouseMutationDetailService.GetObjectsByBlanketWarehouseMutationId(blanketWarehouseMutation.Id);
            if (blanketWarehouseMutation.Quantity != details.Count())
            {
                blanketWarehouseMutation.Errors.Add("Generic", "jumlah Quantity " + blanketWarehouseMutation.Quantity + " harus sama dengan jumlah detail " + details.Count());
            }
            return blanketWarehouseMutation;
        }
        
        public BlanketWarehouseMutation VHasNotBeenConfirmed(BlanketWarehouseMutation blanketWarehouseMutation)
        {
            if (blanketWarehouseMutation.IsConfirmed)
            {
                blanketWarehouseMutation.Errors.Add("Generic", "Tidak boleh sudah dikonfirmasi");
            }
            return blanketWarehouseMutation;
        }

        public BlanketWarehouseMutation VHasBeenConfirmed(BlanketWarehouseMutation blanketWarehouseMutation)
        {
            if (!blanketWarehouseMutation.IsConfirmed)
            {
                blanketWarehouseMutation.Errors.Add("Generic", "Harus sudah dikonfirmasi");
            }
            return blanketWarehouseMutation;
        }

        public BlanketWarehouseMutation VDetailsAreVerifiedConfirmable(BlanketWarehouseMutation blanketWarehouseMutation, IBlanketWarehouseMutationService _blanketWarehouseMutationService,
                                                                      IBlanketWarehouseMutationDetailService _blanketWarehouseMutationDetailService, IItemService _itemService, 
                                                                      IBlanketService _blanketService, IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService,
                                                                      IBlanketOrderService _blanketOrderService, ICoreIdentificationService _coreIdentificationService)
        {
            IList<BlanketWarehouseMutationDetail> details = _blanketWarehouseMutationDetailService.GetObjectsByBlanketWarehouseMutationId(blanketWarehouseMutation.Id);
            IDictionary<int, int> ValuePairWarehouseItemIdQuantity = new Dictionary<int, int>();
            foreach (var detail in details)
            {
                WarehouseItem warehouseItemFrom = _warehouseItemService.FindOrCreateObject(blanketWarehouseMutation.WarehouseFromId, detail.ItemId);
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
                BlanketOrder blanketOrder = _blanketOrderService.GetObjectById(blanketWarehouseMutation.BlanketOrderId);
                //CoreIdentification coreIdentification = _coreIdentificationService.GetObjectById(blanketOrder.CoreIdentificationId);
                //if (coreIdentification.IsInHouse)
                {
                    if (ValuePair.Value > warehouseItemFrom.Quantity)
                    {
                        blanketWarehouseMutation.Errors.Add("Generic", "Stock barang tidak boleh kurang dari stock yang mau dimutasikan");
                        return blanketWarehouseMutation;
                    }
                }
                //else
                //{
                //    CustomerItem customerItemFrom = _customerItemService.FindOrCreateObject(coreIdentification.ContactId.GetValueOrDefault(), warehouseItemFrom.Id);
                //    if (ValuePair.Value > customerItemFrom.Quantity)
                //    {
                //        blanketWarehouseMutation.Errors.Add("Generic", "Stock barang Customer tidak boleh kurang dari stock yang mau dimutasikan");
                //        return blanketWarehouseMutation;
                //    }
                //}
            }
            return blanketWarehouseMutation;
        }

        public BlanketWarehouseMutation VAllDetailsAreUnconfirmable(BlanketWarehouseMutation blanketWarehouseMutation, IBlanketOrderService _blanketOrderService, ICoreIdentificationService _coreIdentificationService,
                                                                   IBlanketWarehouseMutationDetailService _blanketWarehouseMutationDetailService, IBlanketWarehouseMutationService _blanketWarehouseMutationService,
                                                                   IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService)
        {
            IList<BlanketWarehouseMutationDetail> details = _blanketWarehouseMutationDetailService.GetObjectsByBlanketWarehouseMutationId(blanketWarehouseMutation.Id);
            foreach (var detail in details)
            {
                detail.Errors = new Dictionary<string, string>();
                if (!_blanketWarehouseMutationDetailService.GetValidator().ValidUnconfirmObject(detail, _blanketOrderService, _coreIdentificationService, _blanketWarehouseMutationService, _itemService, 
                                                                                               _blanketService, _warehouseItemService, _customerItemService))
                {
                    blanketWarehouseMutation.Errors.Add("Generic", detail.Errors.FirstOrDefault().Key + " " + detail.Errors.FirstOrDefault().Value);
                    return blanketWarehouseMutation;
                }
            }
            return blanketWarehouseMutation;
        }

        public BlanketWarehouseMutation VCreateObject(BlanketWarehouseMutation blanketWarehouseMutation, IWarehouseService _warehouseService, IBlanketOrderService _blanketOrderService)
        {
            VHasBlanketOrder(blanketWarehouseMutation, _blanketOrderService);
            if (!isValid(blanketWarehouseMutation)) { return blanketWarehouseMutation; }
            VBlanketOrderHasBeenCompleted(blanketWarehouseMutation, _blanketOrderService);
            if (!isValid(blanketWarehouseMutation)) { return blanketWarehouseMutation; }
            VHasWarehouseFrom(blanketWarehouseMutation, _warehouseService);
            if (!isValid(blanketWarehouseMutation)) { return blanketWarehouseMutation; }
            VHasWarehouseTo(blanketWarehouseMutation, _warehouseService);
            if (!isValid(blanketWarehouseMutation)) { return blanketWarehouseMutation; }
            VHasDifferentWarehouse(blanketWarehouseMutation);
            return blanketWarehouseMutation;
        }

        public BlanketWarehouseMutation VUpdateObject(BlanketWarehouseMutation blanketWarehouseMutation, IWarehouseService _warehouseService, IBlanketOrderService _blanketOrderService)
        {
            VHasNotBeenConfirmed(blanketWarehouseMutation);
            if (!isValid(blanketWarehouseMutation)) { return blanketWarehouseMutation; }
            VCreateObject(blanketWarehouseMutation, _warehouseService, _blanketOrderService);
            return blanketWarehouseMutation;
        }

        public BlanketWarehouseMutation VDeleteObject(BlanketWarehouseMutation blanketWarehouseMutation)
        {
            VHasNotBeenConfirmed(blanketWarehouseMutation);
            return blanketWarehouseMutation;
        }

        public BlanketWarehouseMutation VHasConfirmationDate(BlanketWarehouseMutation obj)
        {
            if (obj.ConfirmationDate == null)
            {
                obj.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return obj;
        }

        public BlanketWarehouseMutation VConfirmObject(BlanketWarehouseMutation blanketWarehouseMutation, IBlanketWarehouseMutationService _blanketWarehouseMutationService,
                                                      IBlanketWarehouseMutationDetailService _blanketWarehouseMutationDetailService, IItemService _itemService, 
                                                      IBlanketService _blanketService, IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService,
                                                      IBlanketOrderService _blanketOrderService, ICoreIdentificationService _coreIdentificationService)
        {
            VHasConfirmationDate(blanketWarehouseMutation);
            if (!isValid(blanketWarehouseMutation)) { return blanketWarehouseMutation; }
            VHasNotBeenConfirmed(blanketWarehouseMutation);
            if (!isValid(blanketWarehouseMutation)) { return blanketWarehouseMutation; }
            VHasBlanketWarehouseMutationDetails(blanketWarehouseMutation, _blanketWarehouseMutationDetailService);
            if (!isValid(blanketWarehouseMutation)) { return blanketWarehouseMutation; }
            VQuantityIsEqualTheNumberOfDetails(blanketWarehouseMutation, _blanketWarehouseMutationDetailService);
            if (!isValid(blanketWarehouseMutation)) { return blanketWarehouseMutation; }
            VDetailsAreVerifiedConfirmable(blanketWarehouseMutation, _blanketWarehouseMutationService, _blanketWarehouseMutationDetailService, _itemService,
                                           _blanketService, _warehouseItemService, _customerItemService, _blanketOrderService, _coreIdentificationService);
            return blanketWarehouseMutation;
        }

        public BlanketWarehouseMutation VUnconfirmObject(BlanketWarehouseMutation blanketWarehouseMutation, IBlanketOrderService _blanketOrderService, ICoreIdentificationService _coreIdentificationService,
                                                        IBlanketWarehouseMutationService _blanketWarehouseMutationService, IBlanketWarehouseMutationDetailService _blanketWarehouseMutationDetailService,
                                                        IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService)
        {
            VHasBeenConfirmed(blanketWarehouseMutation);
            if (!isValid(blanketWarehouseMutation)) { return blanketWarehouseMutation; }
            VAllDetailsAreUnconfirmable(blanketWarehouseMutation, _blanketOrderService, _coreIdentificationService, _blanketWarehouseMutationDetailService, _blanketWarehouseMutationService, 
                                        _itemService, _blanketService, _warehouseItemService, _customerItemService);
            return blanketWarehouseMutation;
        }

        public bool ValidCreateObject(BlanketWarehouseMutation blanketWarehouseMutation, IWarehouseService _warehouseService, IBlanketOrderService _blanketOrderService)
        {
            VCreateObject(blanketWarehouseMutation, _warehouseService, _blanketOrderService);
            return isValid(blanketWarehouseMutation);
        }

        public bool ValidUpdateObject(BlanketWarehouseMutation blanketWarehouseMutation, IWarehouseService _warehouseService, IBlanketOrderService _blanketOrderService)
        {
            blanketWarehouseMutation.Errors.Clear();
            VUpdateObject(blanketWarehouseMutation, _warehouseService, _blanketOrderService);
            return isValid(blanketWarehouseMutation);
        }

        public bool ValidDeleteObject(BlanketWarehouseMutation blanketWarehouseMutation)
        {
            blanketWarehouseMutation.Errors.Clear();
            VDeleteObject(blanketWarehouseMutation);
            return isValid(blanketWarehouseMutation);
        }

        public bool ValidConfirmObject(BlanketWarehouseMutation blanketWarehouseMutation, IBlanketWarehouseMutationService _blanketWarehouseMutationService,
                                       IBlanketWarehouseMutationDetailService _blanketWarehouseMutationDetailService, IItemService _itemService, 
                                       IBlanketService _blanketService, IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService,
                                       IBlanketOrderService _blanketOrderService, ICoreIdentificationService _coreIdentificationService)
        {
            blanketWarehouseMutation.Errors.Clear();
            VConfirmObject(blanketWarehouseMutation, _blanketWarehouseMutationService, _blanketWarehouseMutationDetailService, _itemService, _blanketService,
                           _warehouseItemService, _customerItemService, _blanketOrderService, _coreIdentificationService);
            return isValid(blanketWarehouseMutation);
        }

        public bool ValidUnconfirmObject(BlanketWarehouseMutation blanketWarehouseMutation, IBlanketOrderService _blanketOrderService, ICoreIdentificationService _coreIdentificationService,
                                         IBlanketWarehouseMutationService _blanketWarehouseMutationService, IBlanketWarehouseMutationDetailService _blanketWarehouseMutationDetailService, 
                                         IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService, ICustomerItemService _customerItemService)
        {
            blanketWarehouseMutation.Errors.Clear();
            VUnconfirmObject(blanketWarehouseMutation, _blanketOrderService, _coreIdentificationService, _blanketWarehouseMutationService, _blanketWarehouseMutationDetailService,
                             _itemService, _blanketService, _warehouseItemService, _customerItemService);
            return isValid(blanketWarehouseMutation);
        }

        public bool isValid(BlanketWarehouseMutation obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(BlanketWarehouseMutation obj)
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