using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class BlendingWorkOrderValidator : IBlendingWorkOrderValidator
    {
        public BlendingWorkOrder VHasUniqueCode(BlendingWorkOrder blendingWorkOrder, IBlendingWorkOrderService _blendingWorkOrderService)
        {
            if (String.IsNullOrEmpty(blendingWorkOrder.Code) || blendingWorkOrder.Code.Trim() == "")
            {
                blendingWorkOrder.Errors.Add("Code", "Tidak boleh kosong");
            }
            if (_blendingWorkOrderService.IsCodeDuplicated(blendingWorkOrder))
            {
                blendingWorkOrder.Errors.Add("Code", "Tidak boleh ada duplikasi");
            }
            return blendingWorkOrder;
        }

        public BlendingWorkOrder VHasBlendingRecipe(BlendingWorkOrder blendingWorkOrder, IBlendingRecipeService _blendingRecipeService)
        {
            BlendingRecipe blendingRecipe = _blendingRecipeService.GetObjectById(blendingWorkOrder.BlendingRecipeId);
            if (blendingRecipe == null)
            {
                blendingWorkOrder.Errors.Add("BlendingRecipeId", "Tidak valid");
            }
            return blendingWorkOrder;
        }

        public BlendingWorkOrder VHasWarehouse(BlendingWorkOrder blendingWorkOrder, IWarehouseService _warehouseService)
        {
            Warehouse warehouse = _warehouseService.GetObjectById(blendingWorkOrder.WarehouseId);
            if (warehouse == null)
            {
                blendingWorkOrder.Errors.Add("WarehouseId", "Tidak valid");
            }
            return blendingWorkOrder;
        }

        public BlendingWorkOrder VTargetQuantityIsInStock(BlendingWorkOrder blendingWorkOrder, IWarehouseItemService _warehouseItemService)
        {
            WarehouseItem warehouseItem = _warehouseItemService.FindOrCreateObject(blendingWorkOrder.WarehouseId, blendingWorkOrder.BlendingRecipe.TargetItemId);
            if (warehouseItem.Quantity < blendingWorkOrder.BlendingRecipe.TargetQuantity)
            {
                blendingWorkOrder.Errors.Add("Generic", "Stock quantity untuk Target item tidak cukup");
            }
            return blendingWorkOrder;
        }

        public BlendingWorkOrder VSourceQuantityIsInStock(BlendingWorkOrder blendingWorkOrder, IBlendingRecipeDetailService _blendingRecipeDetailService, IWarehouseItemService _warehouseItemService)
        {
            IList<BlendingRecipeDetail> details = _blendingRecipeDetailService.GetObjectsByBlendingRecipeId(blendingWorkOrder.BlendingRecipeId);

            // itemId contains Id of the source item
            IDictionary<int, int> ValuePairItemIdQuantity = new Dictionary<int, int>();
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
                WarehouseItem warehouseItem = _warehouseItemService.FindOrCreateObject(blendingWorkOrder.WarehouseId, ValuePair.Key);
                if (warehouseItem.Errors.Any())
                {
                    KeyValuePair<string, string> err = warehouseItem.Errors.FirstOrDefault();
                    blendingWorkOrder.Errors.Add("Generic", err.Key + " " + err.Value);
                    return blendingWorkOrder;
                }
                else if (warehouseItem.Quantity < ValuePair.Value)
                {
                    blendingWorkOrder.Errors.Add("Generic", "Stock quantity untuk Source items (detail) tidak cukup");
                    return blendingWorkOrder;
                }
            }
            return blendingWorkOrder;
        }

        public BlendingWorkOrder VHasBlendingDate(BlendingWorkOrder blendingWorkOrder)
        {
            if (blendingWorkOrder.BlendingDate == null || blendingWorkOrder.BlendingDate.Equals(DateTime.FromBinary(0)))
            {
                blendingWorkOrder.Errors.Add("BlendingDate", "Tidak boleh kosong");
            }
            return blendingWorkOrder;
        }

        public BlendingWorkOrder VHasConfirmationDate(BlendingWorkOrder blendingWorkOrder)
        {
            if (blendingWorkOrder.ConfirmationDate == null)
            {
                blendingWorkOrder.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return blendingWorkOrder;
        }

        public BlendingWorkOrder VHasBeenConfirmed(BlendingWorkOrder blendingWorkOrder)
        {
            if (!blendingWorkOrder.IsConfirmed)
            {
                blendingWorkOrder.Errors.Add("Generic", "Belum dikonfirmasi");
            }
            return blendingWorkOrder;
        }

        public BlendingWorkOrder VHasNotBeenConfirmed(BlendingWorkOrder blendingWorkOrder)
        {
            if (blendingWorkOrder.IsConfirmed)
            {
                blendingWorkOrder.Errors.Add("Generic", "Sudah dikonfirmasi");
            }
            return blendingWorkOrder;
        }

        public BlendingWorkOrder VGeneralLedgerPostingHasNotBeenClosed(BlendingWorkOrder blendingWorkOrder, IClosingService _closingService, int CaseConfirmUnconfirm)
        {
            switch (CaseConfirmUnconfirm)
            {
                case (1): // Confirm
                    {
                        if (_closingService.IsDateClosed(blendingWorkOrder.ConfirmationDate.GetValueOrDefault()))
                        {
                            blendingWorkOrder.Errors.Add("Generic", "Ledger sudah tutup buku");
                        }
                        break;
                    }
                case (2): // Unconfirm
                    {
                        if (_closingService.IsDateClosed(DateTime.Now))
                        {
                            blendingWorkOrder.Errors.Add("Generic", "Ledger sudah tutup buku");
                        }
                        break;
                    }
            }
            return blendingWorkOrder;
        }

        public BlendingWorkOrder VCreateObject(BlendingWorkOrder blendingWorkOrder, IBlendingWorkOrderService _blendingWorkOrderService, IBlendingRecipeService _blendingRecipeService, IWarehouseService _warehouseService)
        {
            VHasUniqueCode(blendingWorkOrder, _blendingWorkOrderService);
            if (!isValid(blendingWorkOrder)) { return blendingWorkOrder; }
            VHasBlendingRecipe(blendingWorkOrder, _blendingRecipeService);
            if (!isValid(blendingWorkOrder)) { return blendingWorkOrder; }
            VHasWarehouse(blendingWorkOrder, _warehouseService);
            if (!isValid(blendingWorkOrder)) { return blendingWorkOrder; }
            VHasBlendingDate(blendingWorkOrder);
            return blendingWorkOrder;
        }

        public BlendingWorkOrder VUpdateObject(BlendingWorkOrder blendingWorkOrder, IBlendingWorkOrderService _blendingWorkOrderService, IBlendingRecipeService _blendingRecipeService, IWarehouseService _warehouseService)
        {
            VHasNotBeenConfirmed(blendingWorkOrder);
            if (!isValid(blendingWorkOrder)) { return blendingWorkOrder; }
            VCreateObject(blendingWorkOrder, _blendingWorkOrderService, _blendingRecipeService, _warehouseService);
            return blendingWorkOrder;
        }

        public BlendingWorkOrder VDeleteObject(BlendingWorkOrder blendingWorkOrder)
        {
            VHasNotBeenConfirmed(blendingWorkOrder);
            return blendingWorkOrder;
        }

        public BlendingWorkOrder VConfirmObject(BlendingWorkOrder blendingWorkOrder, IBlendingRecipeDetailService _blendingRecipeDetailService, IWarehouseItemService _warehouseItemService, IClosingService _closingService)
        {
            VHasConfirmationDate(blendingWorkOrder);
            if (!isValid(blendingWorkOrder)) { return blendingWorkOrder; }
            VHasNotBeenConfirmed(blendingWorkOrder);
            if (!isValid(blendingWorkOrder)) { return blendingWorkOrder; }
            VSourceQuantityIsInStock(blendingWorkOrder, _blendingRecipeDetailService, _warehouseItemService);
            if (!isValid(blendingWorkOrder)) { return blendingWorkOrder; }
            VGeneralLedgerPostingHasNotBeenClosed(blendingWorkOrder, _closingService, 1);
            return blendingWorkOrder;
        }

        public BlendingWorkOrder VUnconfirmObject(BlendingWorkOrder blendingWorkOrder, IWarehouseItemService _warehouseItemService, IClosingService _closingService)
        {
            VHasBeenConfirmed(blendingWorkOrder);
            if (!isValid(blendingWorkOrder)) { return blendingWorkOrder; }
            VTargetQuantityIsInStock(blendingWorkOrder, _warehouseItemService);
            if (!isValid(blendingWorkOrder)) { return blendingWorkOrder; }
            VGeneralLedgerPostingHasNotBeenClosed(blendingWorkOrder, _closingService, 2);
            return blendingWorkOrder;
        }

        public BlendingWorkOrder VAdjustQuantity(BlendingWorkOrder blendingWorkOrder)
        {
            return blendingWorkOrder;
        }

        public bool ValidCreateObject(BlendingWorkOrder blendingWorkOrder, IBlendingWorkOrderService _blendingWorkOrderService, IBlendingRecipeService _blendingRecipeService, IWarehouseService _warehouseService)
        {
            VCreateObject(blendingWorkOrder, _blendingWorkOrderService, _blendingRecipeService, _warehouseService);
            return isValid(blendingWorkOrder);
        }

        public bool ValidUpdateObject(BlendingWorkOrder blendingWorkOrder, IBlendingWorkOrderService _blendingWorkOrderService, IBlendingRecipeService _blendingRecipeService, IWarehouseService _warehouseService)
        {
            blendingWorkOrder.Errors.Clear();
            VUpdateObject(blendingWorkOrder, _blendingWorkOrderService, _blendingRecipeService, _warehouseService);
            return isValid(blendingWorkOrder);
        }

        public bool ValidDeleteObject(BlendingWorkOrder blendingWorkOrder)
        {
            blendingWorkOrder.Errors.Clear();
            VDeleteObject(blendingWorkOrder);
            return isValid(blendingWorkOrder);
        }

        public bool ValidConfirmObject(BlendingWorkOrder blendingWorkOrder, IBlendingRecipeDetailService _blendingRecipeDetailService, IWarehouseItemService _warehouseItemService, IClosingService _closingService)
        {
            blendingWorkOrder.Errors.Clear();
            VConfirmObject(blendingWorkOrder, _blendingRecipeDetailService, _warehouseItemService, _closingService);
            return isValid(blendingWorkOrder);
        }

        public bool ValidUnconfirmObject(BlendingWorkOrder blendingWorkOrder, IWarehouseItemService _warehouseItemService, IClosingService _closingService)
        {
            blendingWorkOrder.Errors.Clear();
            VUnconfirmObject(blendingWorkOrder, _warehouseItemService, _closingService);
            return isValid(blendingWorkOrder);
        }

        public bool ValidAdjustQuantity(BlendingWorkOrder blendingWorkOrder)
        {
            blendingWorkOrder.Errors.Clear();
            VAdjustQuantity(blendingWorkOrder);
            return isValid(blendingWorkOrder);
        }
        
        public bool isValid(BlendingWorkOrder obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(BlendingWorkOrder obj)
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
