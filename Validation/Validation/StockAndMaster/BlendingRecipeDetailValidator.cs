using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class BlendingRecipeDetailValidator : IBlendingRecipeDetailValidator
    {
        public BlendingRecipeDetail VHasUniqueItem(BlendingRecipeDetail blendingRecipeDetail, IItemService _itemService, IBlendingRecipeDetailService _blendingRecipeDetailService)
        {
            Item item = _itemService.GetObjectById(blendingRecipeDetail.ItemId);
            if (item == null)
            {
                blendingRecipeDetail.Errors.Add("ItemId", "Tidak boleh tidak ada");
            } 
            else
            {
                var list = _blendingRecipeDetailService.GetObjectsByBlendingRecipeId(blendingRecipeDetail.BlendingRecipeId).Where(x => x.ItemId == blendingRecipeDetail.ItemId && x.Id != blendingRecipeDetail.Id);
                if (list.Any())
                {
                    blendingRecipeDetail.Errors.Add("ItemId", "Tidak boleh ada item yang sama di dalam satu Blending Recipe");
                }
            }
            return blendingRecipeDetail;
        }

        public BlendingRecipeDetail VHasBlendingRecipe(BlendingRecipeDetail blendingRecipeDetail, IBlendingRecipeService _blendingRecipeService)
        {
            BlendingRecipe blendingRecipe = _blendingRecipeService.GetObjectById(blendingRecipeDetail.BlendingRecipeId);
            if (blendingRecipe == null)
            {
                blendingRecipeDetail.Errors.Add("BlendingRecipeId", "Tidak boleh tidak ada");
            }
            return blendingRecipeDetail;
        }

        public BlendingRecipeDetail VNonNegativeNonZeroQuantity(BlendingRecipeDetail blendingRecipeDetail)
        {
            if (blendingRecipeDetail.Quantity <= 0)
            {
                blendingRecipeDetail.Errors.Add("Quantity", "Harus lebih besar dari 0");
            }
            return blendingRecipeDetail;
        }

        public BlendingRecipeDetail VCreateObject(BlendingRecipeDetail blendingRecipeDetail, IBlendingRecipeService _blendingRecipeService, IBlendingRecipeDetailService _blendingRecipeDetailService, IItemService _itemService)
        {
            VHasBlendingRecipe(blendingRecipeDetail, _blendingRecipeService);
            if (!isValid(blendingRecipeDetail)) { return blendingRecipeDetail; }
            VHasUniqueItem(blendingRecipeDetail, _itemService, _blendingRecipeDetailService);
            if (!isValid(blendingRecipeDetail)) { return blendingRecipeDetail; }
            VNonNegativeNonZeroQuantity(blendingRecipeDetail);
            if (!isValid(blendingRecipeDetail)) { return blendingRecipeDetail; }
            return blendingRecipeDetail;
        }

        public BlendingRecipeDetail VUpdateObject(BlendingRecipeDetail blendingRecipeDetail, IBlendingRecipeService _blendingRecipeService, IBlendingRecipeDetailService _blendingRecipeDetailService, IItemService _itemService)
        {
            return VCreateObject(blendingRecipeDetail, _blendingRecipeService, _blendingRecipeDetailService, _itemService);
        }

        public BlendingRecipeDetail VDeleteObject(BlendingRecipeDetail blendingRecipeDetail)
        {
            return blendingRecipeDetail;
        }

        public BlendingRecipeDetail VAdjustQuantity(BlendingRecipeDetail blendingRecipeDetail)
        {
            VNonNegativeNonZeroQuantity(blendingRecipeDetail);
            return blendingRecipeDetail;
        }

        public bool ValidCreateObject(BlendingRecipeDetail blendingRecipeDetail, IBlendingRecipeService _blendingRecipeService, IBlendingRecipeDetailService _blendingRecipeDetailService, IItemService _itemService)
        {
            VCreateObject(blendingRecipeDetail, _blendingRecipeService, _blendingRecipeDetailService, _itemService);
            return isValid(blendingRecipeDetail);
        }

        public bool ValidUpdateObject(BlendingRecipeDetail blendingRecipeDetail, IBlendingRecipeService _blendingRecipeService, IBlendingRecipeDetailService _blendingRecipeDetailService, IItemService _itemService)
        {
            blendingRecipeDetail.Errors.Clear();
            VUpdateObject(blendingRecipeDetail, _blendingRecipeService, _blendingRecipeDetailService, _itemService);
            return isValid(blendingRecipeDetail);
        }

        public bool ValidDeleteObject(BlendingRecipeDetail blendingRecipeDetail)
        {
            blendingRecipeDetail.Errors.Clear();
            VDeleteObject(blendingRecipeDetail);
            return isValid(blendingRecipeDetail);
        }

        public bool ValidAdjustQuantity(BlendingRecipeDetail blendingRecipeDetail)
        {
            blendingRecipeDetail.Errors.Clear();
            VAdjustQuantity(blendingRecipeDetail);
            return isValid(blendingRecipeDetail);
        }

        public bool isValid(BlendingRecipeDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(BlendingRecipeDetail obj)
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
