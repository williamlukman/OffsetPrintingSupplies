using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface IBlendingRecipeDetailValidator
    {
        BlendingRecipeDetail VCreateObject(BlendingRecipeDetail blendingRecipeDetail, IBlendingRecipeService _blendingRecipeService, IBlendingRecipeDetailService _blendingRecipeDetailService, IItemService _itemService);
        BlendingRecipeDetail VUpdateObject(BlendingRecipeDetail blendingRecipeDetail, IBlendingRecipeService _blendingRecipeService, IBlendingRecipeDetailService _blendingRecipeDetailService, IItemService _itemService);
        BlendingRecipeDetail VDeleteObject(BlendingRecipeDetail blendingRecipeDetail);
        BlendingRecipeDetail VAdjustQuantity(BlendingRecipeDetail blendingRecipeDetail);

        bool ValidCreateObject(BlendingRecipeDetail blendingRecipeDetail, IBlendingRecipeService _blendingRecipeService, IBlendingRecipeDetailService _blendingRecipeDetailService, IItemService _itemService);
        bool ValidUpdateObject(BlendingRecipeDetail blendingRecipeDetail, IBlendingRecipeService _blendingRecipeService, IBlendingRecipeDetailService _blendingRecipeDetailService, IItemService _itemService);
        bool ValidDeleteObject(BlendingRecipeDetail blendingRecipeDetail);
        bool ValidAdjustQuantity(BlendingRecipeDetail blendingRecipeDetail);
        bool isValid(BlendingRecipeDetail blendingRecipeDetail);
        string PrintError(BlendingRecipeDetail blendingRecipeDetail);
    }
}