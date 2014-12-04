using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface IBlendingRecipeValidator
    {
        BlendingRecipe VCreateObject(BlendingRecipe blendingRecipe, IBlendingRecipeService _blendingRecipeService, IItemService _itemService, IItemTypeService _itemTypeService);
        BlendingRecipe VUpdateObject(BlendingRecipe blendingRecipe, IBlendingRecipeService _blendingRecipeService, IItemService _itemService, IItemTypeService _itemTypeService);
        BlendingRecipe VDeleteObject(BlendingRecipe blendingRecipe, IBlendingRecipeDetailService _blendingRecipeDetailService, IBlendingWorkOrderService _blendingWorkOrderService);
        BlendingRecipe VAdjustQuantity(BlendingRecipe blendingRecipe);

        bool ValidCreateObject(BlendingRecipe blendingRecipe, IBlendingRecipeService _blendingRecipeService, IItemService _itemService, IItemTypeService _itemTypeService);
        bool ValidUpdateObject(BlendingRecipe blendingRecipe, IBlendingRecipeService _blendingRecipeService, IItemService _itemService, IItemTypeService _itemTypeService);
        bool ValidDeleteObject(BlendingRecipe blendingRecipe, IBlendingRecipeDetailService _blendingRecipeDetailService, IBlendingWorkOrderService _blendingWorkOrderService);
        bool ValidAdjustQuantity(BlendingRecipe blendingRecipe);
        bool isValid(BlendingRecipe blendingRecipe);
        string PrintError(BlendingRecipe blendingRecipe);
    }
}