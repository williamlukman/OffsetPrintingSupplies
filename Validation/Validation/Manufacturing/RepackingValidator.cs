using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class RepackingValidator : IRepackingValidator
    {
        public Repacking VHasUniqueCode(Repacking repacking, IRepackingService _repackingService)
        {
            if (String.IsNullOrEmpty(repacking.Code) || repacking.Code.Trim() == "")
            {
                repacking.Errors.Add("Code", "Tidak boleh kosong");
            }
            if (_repackingService.IsCodeDuplicated(repacking))
            {
                repacking.Errors.Add("Code", "Tidak boleh ada duplikasi");
            }
            return repacking;
        }

        public Repacking VHasBlendingRecipe(Repacking repacking, IBlendingRecipeService _blendingRecipeService)
        {
            BlendingRecipe blendingRecipe = _blendingRecipeService.GetObjectById(repacking.BlendingRecipeId);
            if (blendingRecipe == null)
            {
                repacking.Errors.Add("BlendingRecipeId", "Tidak valid");
            }
            return repacking;
        }

        public Repacking VHasWarehouse(Repacking repacking, IWarehouseService _warehouseService)
        {
            Warehouse warehouse = _warehouseService.GetObjectById(repacking.WarehouseId);
            if (warehouse == null)
            {
                repacking.Errors.Add("WarehouseId", "Tidak valid");
            }
            return repacking;
        }

        public Repacking VTargetQuantityIsInStock(Repacking repacking, IWarehouseItemService _warehouseItemService, IBlendingRecipeService _blendingRecipeService)
        {
            BlendingRecipe blendingRecipe = _blendingRecipeService.GetObjectById(repacking.BlendingRecipeId);
            WarehouseItem warehouseItem = _warehouseItemService.FindOrCreateObject(repacking.WarehouseId, blendingRecipe.TargetItemId);
            if (warehouseItem.Quantity < blendingRecipe.TargetQuantity)
            {
                repacking.Errors.Add("Generic", "Stock quantity untuk Target item tidak cukup");
            }
            return repacking;
        }

        public Repacking VSourceQuantityIsInStock(Repacking repacking, IBlendingRecipeDetailService _blendingRecipeDetailService, IWarehouseItemService _warehouseItemService)
        {
            IList<BlendingRecipeDetail> details = _blendingRecipeDetailService.GetObjectsByBlendingRecipeId(repacking.BlendingRecipeId);

            // itemId contains Id of the source item
            IDictionary<int, decimal> ValuePairItemIdQuantity = new Dictionary<int, decimal>();
            foreach (var detail in details)
            {
                if (ValuePairItemIdQuantity.ContainsKey(detail.ItemId))
                {
                    ValuePairItemIdQuantity[detail.ItemId] += detail.Quantity;
                }
                else
                {
                    ValuePairItemIdQuantity.Add(detail.ItemId, detail.Quantity);
                }
            }

            foreach (var ValuePair in ValuePairItemIdQuantity)
            {
                WarehouseItem warehouseItem = _warehouseItemService.FindOrCreateObject(repacking.WarehouseId, ValuePair.Key);
                if (warehouseItem.Errors.Any())
                {
                    KeyValuePair<string, string> err = warehouseItem.Errors.FirstOrDefault();
                    repacking.Errors.Add("Generic", err.Key + " " + err.Value);
                    return repacking;
                }
                else if (warehouseItem.Quantity < ValuePair.Value)
                {
                    repacking.Errors.Add("Generic", "Stock quantity untuk Source items (detail) tidak cukup");
                    return repacking;
                }
            }
            return repacking;
        }

        public Repacking VHasRepackingDate(Repacking repacking)
        {
            if (repacking.RepackingDate == null || repacking.RepackingDate.Equals(DateTime.FromBinary(0)))
            {
                repacking.Errors.Add("RepackingDate", "Tidak boleh kosong");
            }
            return repacking;
        }

        public Repacking VHasConfirmationDate(Repacking repacking)
        {
            if (repacking.ConfirmationDate == null)
            {
                repacking.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return repacking;
        }

        public Repacking VHasBeenConfirmed(Repacking repacking)
        {
            if (!repacking.IsConfirmed)
            {
                repacking.Errors.Add("Generic", "Belum dikonfirmasi");
            }
            return repacking;
        }

        public Repacking VHasNotBeenConfirmed(Repacking repacking)
        {
            if (repacking.IsConfirmed)
            {
                repacking.Errors.Add("Generic", "Sudah dikonfirmasi");
            }
            return repacking;
        }

        public Repacking VGeneralLedgerPostingHasNotBeenClosed(Repacking repacking, IClosingService _closingService)
        {
            if (_closingService.IsDateClosed(repacking.RepackingDate))
            {
                repacking.Errors.Add("Generic", "Ledger sudah tutup buku");
            }
            return repacking;
        }

        public Repacking VCreateObject(Repacking repacking, IRepackingService _repackingService, IBlendingRecipeService _blendingRecipeService, IWarehouseService _warehouseService)
        {
            VHasUniqueCode(repacking, _repackingService);
            if (!isValid(repacking)) { return repacking; }
            VHasBlendingRecipe(repacking, _blendingRecipeService);
            if (!isValid(repacking)) { return repacking; }
            VHasWarehouse(repacking, _warehouseService);
            if (!isValid(repacking)) { return repacking; }
            VHasRepackingDate(repacking);
            return repacking;
        }

        public Repacking VUpdateObject(Repacking repacking, IRepackingService _repackingService, IBlendingRecipeService _blendingRecipeService, IWarehouseService _warehouseService)
        {
            VHasNotBeenConfirmed(repacking);
            if (!isValid(repacking)) { return repacking; }
            VCreateObject(repacking, _repackingService, _blendingRecipeService, _warehouseService);
            return repacking;
        }

        public Repacking VDeleteObject(Repacking repacking)
        {
            VHasNotBeenConfirmed(repacking);
            return repacking;
        }

        public Repacking VConfirmObject(Repacking repacking, IBlendingRecipeDetailService _blendingRecipeDetailService, IWarehouseItemService _warehouseItemService, IClosingService _closingService)
        {
            VHasConfirmationDate(repacking);
            if (!isValid(repacking)) { return repacking; }
            VHasNotBeenConfirmed(repacking);
            if (!isValid(repacking)) { return repacking; }
            VSourceQuantityIsInStock(repacking, _blendingRecipeDetailService, _warehouseItemService);
            if (!isValid(repacking)) { return repacking; }
            VGeneralLedgerPostingHasNotBeenClosed(repacking, _closingService);
            return repacking;
        }

        public Repacking VUnconfirmObject(Repacking repacking, IWarehouseItemService _warehouseItemService, IBlendingRecipeService _blendingRecipeService, IClosingService _closingService)
        {
            VHasBeenConfirmed(repacking);
            if (!isValid(repacking)) { return repacking; }
            VTargetQuantityIsInStock(repacking, _warehouseItemService, _blendingRecipeService);
            if (!isValid(repacking)) { return repacking; }
            VGeneralLedgerPostingHasNotBeenClosed(repacking, _closingService);
            return repacking;
        }

        public Repacking VAdjustQuantity(Repacking repacking)
        {
            return repacking;
        }

        public bool ValidCreateObject(Repacking repacking, IRepackingService _repackingService, IBlendingRecipeService _blendingRecipeService, IWarehouseService _warehouseService)
        {
            VCreateObject(repacking, _repackingService, _blendingRecipeService, _warehouseService);
            return isValid(repacking);
        }

        public bool ValidUpdateObject(Repacking repacking, IRepackingService _repackingService, IBlendingRecipeService _blendingRecipeService, IWarehouseService _warehouseService)
        {
            repacking.Errors.Clear();
            VUpdateObject(repacking, _repackingService, _blendingRecipeService, _warehouseService);
            return isValid(repacking);
        }

        public bool ValidDeleteObject(Repacking repacking)
        {
            repacking.Errors.Clear();
            VDeleteObject(repacking);
            return isValid(repacking);
        }

        public bool ValidConfirmObject(Repacking repacking, IBlendingRecipeDetailService _blendingRecipeDetailService, IWarehouseItemService _warehouseItemService, IClosingService _closingService)
        {
            repacking.Errors.Clear();
            VConfirmObject(repacking, _blendingRecipeDetailService, _warehouseItemService, _closingService);
            return isValid(repacking);
        }

        public bool ValidUnconfirmObject(Repacking repacking, IWarehouseItemService _warehouseItemService, IBlendingRecipeService _blendingRecipeService, IClosingService _closingService)
        {
            repacking.Errors.Clear();
            VUnconfirmObject(repacking, _warehouseItemService, _blendingRecipeService, _closingService);
            return isValid(repacking);
        }

        public bool ValidAdjustQuantity(Repacking repacking)
        {
            repacking.Errors.Clear();
            VAdjustQuantity(repacking);
            return isValid(repacking);
        }
        
        public bool isValid(Repacking obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(Repacking obj)
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
