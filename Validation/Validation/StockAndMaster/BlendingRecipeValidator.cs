using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Constants;

namespace Validation.Validation
{
    public class BlendingRecipeValidator : IBlendingRecipeValidator
    {
        public BlendingRecipe VHasTargetItemAndIsLegacy(BlendingRecipe blendingRecipe, IItemService _itemService)
        {
            Item item = _itemService.GetObjectById(blendingRecipe.TargetItemId);
            if (item == null)
            {
                blendingRecipe.Errors.Add("TargetItemId", "Tidak boleh tidak ada");
            }
            else if (item.ItemType.Name != Constant.ItemTypeCase.Chemical)
            {
                blendingRecipe.Errors.Add("TargetItemId", "Item Type Harus berupa Chemical");
            }
            return blendingRecipe;
        }

        public BlendingRecipe VHasUniqueName(BlendingRecipe blendingRecipe, IBlendingRecipeService _blendingRecipeService)
        {
            if (String.IsNullOrEmpty(blendingRecipe.Name) || blendingRecipe.Name.Trim() == "")
            {
                blendingRecipe.Errors.Add("Name", "Tidak boleh kosong");
            }
            else if (_blendingRecipeService.IsNameDuplicated(blendingRecipe))
            {
                blendingRecipe.Errors.Add("Name", "Tidak boleh diduplikasi");
            }
            return blendingRecipe;
        }

        public BlendingRecipe VNonZeroNonNegativeQuantity(BlendingRecipe blendingRecipe)
        {
            if (blendingRecipe.TargetQuantity <= 0)
            {
                blendingRecipe.Errors.Add("TargetQuantity", "Tidak harus lebih besar dari 0");
            }
            return blendingRecipe;
        }

        public BlendingRecipe VHasNoBlendingRecipeDetails(BlendingRecipe blendingRecipe, IBlendingRecipeDetailService _blendingRecipeDetailService)
        {
            IList<BlendingRecipeDetail> blendingRecipeDetails = _blendingRecipeDetailService.GetObjectsByBlendingRecipeId(blendingRecipe.Id);
            if (blendingRecipeDetails.Any())
            {
                blendingRecipe.Errors.Add("Generic", "Tidak boleh terasosiasi dengan Blending Recipe Detail");
            }
            return blendingRecipe;
        }

        public BlendingRecipe VHasNoBlendingWorkOrders(BlendingRecipe blendingRecipe, IBlendingWorkOrderService _blendingWorkOrderService)
        {
            IList<BlendingWorkOrder> blendingWorkOrders = _blendingWorkOrderService.GetObjectsByBlendingRecipeId(blendingRecipe.Id);
            if (blendingWorkOrders.Any())
            {
                blendingRecipe.Errors.Add("Generic", "Tidak boleh terasosiasi dengan Blending Work Order");
            }
            return blendingRecipe;
        }

        public BlendingRecipe VCreateObject(BlendingRecipe blendingRecipe, IBlendingRecipeService _blendingRecipeService, IItemService _itemService)
        {
            VHasTargetItemAndIsLegacy(blendingRecipe, _itemService);
            if (!isValid(blendingRecipe)) { return blendingRecipe; }
            VHasUniqueName(blendingRecipe, _blendingRecipeService);
            if (!isValid(blendingRecipe)) { return blendingRecipe; }
            VNonZeroNonNegativeQuantity(blendingRecipe);
            if (!isValid(blendingRecipe)) { return blendingRecipe; }
            return blendingRecipe;
        }

        public BlendingRecipe VUpdateObject(BlendingRecipe blendingRecipe, IBlendingRecipeService _blendingRecipeService, IItemService _itemService)
        {
            return VCreateObject(blendingRecipe, _blendingRecipeService, _itemService);
        }

        public BlendingRecipe VDeleteObject(BlendingRecipe blendingRecipe, IBlendingRecipeDetailService _blendingRecipeDetailService, IBlendingWorkOrderService _blendingWorkOrderService)
        {
            VHasNoBlendingWorkOrders(blendingRecipe, _blendingWorkOrderService);
            if (!isValid(blendingRecipe)) { return blendingRecipe; }
            VHasNoBlendingRecipeDetails(blendingRecipe, _blendingRecipeDetailService);
            return blendingRecipe;
        }

        public BlendingRecipe VAdjustQuantity(BlendingRecipe blendingRecipe)
        {
            VNonZeroNonNegativeQuantity(blendingRecipe);
            return blendingRecipe;
        }

        public bool ValidCreateObject(BlendingRecipe blendingRecipe, IBlendingRecipeService _blendingRecipeService, IItemService _itemService)
        {
            VCreateObject(blendingRecipe, _blendingRecipeService, _itemService);
            return isValid(blendingRecipe);
        }

        public bool ValidUpdateObject(BlendingRecipe blendingRecipe, IBlendingRecipeService _blendingRecipeService, IItemService _itemService)
        {
            blendingRecipe.Errors.Clear();
            VUpdateObject(blendingRecipe, _blendingRecipeService, _itemService);
            return isValid(blendingRecipe);
        }

        public bool ValidDeleteObject(BlendingRecipe blendingRecipe, IBlendingRecipeDetailService _blendingRecipeDetailService, IBlendingWorkOrderService _blendingWorkOrderService)
        {
            blendingRecipe.Errors.Clear();
            VDeleteObject(blendingRecipe, _blendingRecipeDetailService, _blendingWorkOrderService);
            return isValid(blendingRecipe);
        }

        public bool ValidAdjustQuantity(BlendingRecipe blendingRecipe)
        {
            blendingRecipe.Errors.Clear();
            VAdjustQuantity(blendingRecipe);
            return isValid(blendingRecipe);
        }

        public bool isValid(BlendingRecipe obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(BlendingRecipe obj)
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
