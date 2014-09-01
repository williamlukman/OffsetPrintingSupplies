using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Validation;
using Core.DomainModel;
using Core.Interface.Service;

namespace Validation.Validation
{
    public class WarehouseMutationDetailValidator : IWarehouseMutationDetailValidator
    {
        public WarehouseMutationDetail VHasWarehouseMutation(WarehouseMutationDetail WarehouseMutationDetail, IWarehouseMutationService _WarehouseMutationService)
        {
            WarehouseMutation WarehouseMutation = _WarehouseMutationService.GetObjectById(WarehouseMutationDetail.WarehouseMutationId);
            if (WarehouseMutation == null)
            {
                WarehouseMutationDetail.Errors.Add("WarehouseMutationId", "Tidak terasosiasi dengan Stock Adjustment");
            }
            return WarehouseMutationDetail;
        }

        public WarehouseMutationDetail VHasWarehouseItemFrom(WarehouseMutationDetail WarehouseMutationDetail, IWarehouseMutationService _WarehouseMutationService, IWarehouseItemService _warehouseItemService)
        {
            WarehouseMutation WarehouseMutation = _WarehouseMutationService.GetObjectById(WarehouseMutationDetail.WarehouseMutationId);
            WarehouseItem warehouseItemFrom = _warehouseItemService.FindOrCreateObject(WarehouseMutation.WarehouseFromId, WarehouseMutationDetail.ItemId);
            if (warehouseItemFrom == null)
            {
                WarehouseMutationDetail.Errors.Add("Generic", "Tidak terasosiasi dengan item dari warehouse yang sebelum");
            }
            return WarehouseMutationDetail;
        }

        public WarehouseMutationDetail VHasWarehouseItemTo(WarehouseMutationDetail WarehouseMutationDetail, IWarehouseMutationService _WarehouseMutationService, IWarehouseItemService _warehouseItemService)
        {
            WarehouseMutation WarehouseMutation = _WarehouseMutationService.GetObjectById(WarehouseMutationDetail.WarehouseMutationId);
            WarehouseItem warehouseItemTo = _warehouseItemService.FindOrCreateObject(WarehouseMutation.WarehouseToId, WarehouseMutationDetail.ItemId);
            if (warehouseItemTo == null)
            {
                WarehouseMutationDetail.Errors.Add("Generic", "Tidak terasosiasi dengan item dari warehouse yang dituju");
            }
            return WarehouseMutationDetail;
        }

        public WarehouseMutationDetail VUniqueItem(WarehouseMutationDetail WarehouseMutationDetail, IWarehouseMutationDetailService _WarehouseMutationDetailService, IItemService _itemService)
        {
            IList<WarehouseMutationDetail> details = _WarehouseMutationDetailService.GetObjectsByWarehouseMutationId(WarehouseMutationDetail.WarehouseMutationId);
            foreach (var detail in details)
            {
                if (detail.ItemId == WarehouseMutationDetail.ItemId && detail.Id != WarehouseMutationDetail.Id)
                {
                     WarehouseMutationDetail.Errors.Add("Generic", "Tidak boleh ada duplikasi item dalam 1 Stock Adjustment");
                }
            }
            return WarehouseMutationDetail;
        }

        public WarehouseMutationDetail VNonNegativeNorZeroQuantity(WarehouseMutationDetail WarehouseMutationDetail)
        {
            if (WarehouseMutationDetail.Quantity <= 0)
            {
                WarehouseMutationDetail.Errors.Add("Quantity", "Tidak boleh negatif atau 0");
            }
            return WarehouseMutationDetail;
        }

        public WarehouseMutationDetail VWarehouseMutationHasBeenConfirmed(WarehouseMutationDetail WarehouseMutationDetail, IWarehouseMutationService _WarehouseMutationService)
        {
            WarehouseMutation WarehouseMutation = _WarehouseMutationService.GetObjectById(WarehouseMutationDetail.WarehouseMutationId);
            if (!WarehouseMutation.IsConfirmed)
            {
                WarehouseMutationDetail.Errors.Add("Generic", "WarehouseMutation harus sudah dikonfirmasi");
            }
            return WarehouseMutationDetail;
        }

        public WarehouseMutationDetail VHasNotBeenConfirmed(WarehouseMutationDetail WarehouseMutationDetail)
        {
            if (WarehouseMutationDetail.IsConfirmed)
            {
                WarehouseMutationDetail.Errors.Add("IsConfirmed", "Tidak boleh sudah selesai.");
            }
            return WarehouseMutationDetail;
        }

        public WarehouseMutationDetail VHasBeenConfirmed(WarehouseMutationDetail WarehouseMutationDetail)
        {
            if (!WarehouseMutationDetail.IsConfirmed)
            {
                WarehouseMutationDetail.Errors.Add("IsConfirmed", "Harus sudah selesai.");
            }
            return WarehouseMutationDetail;
        }

        public WarehouseMutationDetail VNonNegativeStockQuantity(WarehouseMutationDetail WarehouseMutationDetail, IWarehouseMutationService _WarehouseMutationService,
                                                                      IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService, bool CaseConfirm)
        {
            WarehouseMutation WarehouseMutation = _WarehouseMutationService.GetObjectById(WarehouseMutationDetail.WarehouseMutationId);
            WarehouseItem warehouseItemFrom = _warehouseItemService.FindOrCreateObject(WarehouseMutation.WarehouseFromId, WarehouseMutationDetail.ItemId);
            WarehouseItem warehouseItemTo = _warehouseItemService.FindOrCreateObject(WarehouseMutation.WarehouseToId, WarehouseMutationDetail.ItemId);
            int Quantity = CaseConfirm ? warehouseItemFrom.Quantity : warehouseItemTo.Quantity;
            if ( Quantity < WarehouseMutationDetail.Quantity)
            {
                WarehouseMutationDetail.Errors.Add("Quantity", "Stock barang tidak boleh kurang dari jumlah mutasi barang");
                return WarehouseMutationDetail;
            }
            return WarehouseMutationDetail;
        }

        public WarehouseMutationDetail VCreateObject(WarehouseMutationDetail WarehouseMutationDetail, IWarehouseMutationService _WarehouseMutationService,
                                                          IWarehouseMutationDetailService _WarehouseMutationDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            VHasWarehouseMutation(WarehouseMutationDetail, _WarehouseMutationService);
            if (!isValid(WarehouseMutationDetail)) { return WarehouseMutationDetail; }
            VHasWarehouseItemFrom(WarehouseMutationDetail, _WarehouseMutationService, _warehouseItemService);
            if (!isValid(WarehouseMutationDetail)) { return WarehouseMutationDetail; }
            VHasWarehouseItemTo(WarehouseMutationDetail, _WarehouseMutationService, _warehouseItemService);
            if (!isValid(WarehouseMutationDetail)) { return WarehouseMutationDetail; }
            VNonNegativeNorZeroQuantity(WarehouseMutationDetail);
            if (!isValid(WarehouseMutationDetail)) { return WarehouseMutationDetail; }
            VUniqueItem(WarehouseMutationDetail, _WarehouseMutationDetailService, _itemService);
            return WarehouseMutationDetail;
        }

        public WarehouseMutationDetail VUpdateObject(WarehouseMutationDetail WarehouseMutationDetail, IWarehouseMutationService _WarehouseMutationService,
                                                          IWarehouseMutationDetailService _WarehouseMutationDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            VHasNotBeenConfirmed(WarehouseMutationDetail);
            if (!isValid(WarehouseMutationDetail)) { return WarehouseMutationDetail; }
            VCreateObject(WarehouseMutationDetail, _WarehouseMutationService, _WarehouseMutationDetailService, _itemService, _warehouseItemService);
            return WarehouseMutationDetail;
        }

        public WarehouseMutationDetail VDeleteObject(WarehouseMutationDetail WarehouseMutationDetail)
        {
            VHasNotBeenConfirmed(WarehouseMutationDetail);
            return WarehouseMutationDetail;
        }

        public WarehouseMutationDetail VHasConfirmationDate(WarehouseMutationDetail WarehouseMutationDetail)
        {
            if (WarehouseMutationDetail.ConfirmationDate == null)
            {
                WarehouseMutationDetail.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return WarehouseMutationDetail;
        }

        public WarehouseMutationDetail VConfirmObject(WarehouseMutationDetail WarehouseMutationDetail, IWarehouseMutationService _WarehouseMutationService,
                                                    IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService)
        {
            VHasConfirmationDate(WarehouseMutationDetail);
            if (!isValid(WarehouseMutationDetail)) { return WarehouseMutationDetail; }
            VNonNegativeStockQuantity(WarehouseMutationDetail, _WarehouseMutationService, _itemService, _blanketService, _warehouseItemService, true);
            return WarehouseMutationDetail;
        }

        public WarehouseMutationDetail VUnconfirmObject(WarehouseMutationDetail WarehouseMutationDetail, IWarehouseMutationService _WarehouseMutationService,
                                                      IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService)
        {
            VWarehouseMutationHasBeenConfirmed(WarehouseMutationDetail, _WarehouseMutationService);
            if (!isValid(WarehouseMutationDetail)) { return WarehouseMutationDetail; }
            VHasBeenConfirmed(WarehouseMutationDetail);
            if (!isValid(WarehouseMutationDetail)) { return WarehouseMutationDetail; }
            VNonNegativeStockQuantity(WarehouseMutationDetail, _WarehouseMutationService, _itemService, _blanketService, _warehouseItemService, false);
            return WarehouseMutationDetail;
        }

        public bool ValidCreateObject(WarehouseMutationDetail WarehouseMutationDetail, IWarehouseMutationService _WarehouseMutationService,
                                      IWarehouseMutationDetailService _WarehouseMutationDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            VCreateObject(WarehouseMutationDetail, _WarehouseMutationService, _WarehouseMutationDetailService, _itemService, _warehouseItemService);
            return isValid(WarehouseMutationDetail);
        }

        public bool ValidUpdateObject(WarehouseMutationDetail WarehouseMutationDetail, IWarehouseMutationService _WarehouseMutationService,
                                      IWarehouseMutationDetailService _WarehouseMutationDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            WarehouseMutationDetail.Errors.Clear();
            VUpdateObject(WarehouseMutationDetail, _WarehouseMutationService, _WarehouseMutationDetailService, _itemService, _warehouseItemService);
            return isValid(WarehouseMutationDetail);
        }

        public bool ValidDeleteObject(WarehouseMutationDetail WarehouseMutationDetail)
        {
            WarehouseMutationDetail.Errors.Clear();
            VDeleteObject(WarehouseMutationDetail);
            return isValid(WarehouseMutationDetail);
        }

        public bool ValidConfirmObject(WarehouseMutationDetail WarehouseMutationDetail, IWarehouseMutationService _WarehouseMutationService,
                                       IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService)
        {
            WarehouseMutationDetail.Errors.Clear();
            VConfirmObject(WarehouseMutationDetail, _WarehouseMutationService, _itemService, _blanketService, _warehouseItemService);
            return isValid(WarehouseMutationDetail);
        }

        public bool ValidUnconfirmObject(WarehouseMutationDetail WarehouseMutationDetail, IWarehouseMutationService _WarehouseMutationService,
                                        IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService)
        {
            WarehouseMutationDetail.Errors.Clear();
            VUnconfirmObject(WarehouseMutationDetail, _WarehouseMutationService, _itemService, _blanketService, _warehouseItemService);
            return isValid(WarehouseMutationDetail);
        }

        public bool isValid(WarehouseMutationDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(WarehouseMutationDetail obj)
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