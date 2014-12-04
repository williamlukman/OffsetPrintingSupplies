using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IBlendingRecipeService
    {
        IBlendingRecipeValidator GetValidator();
        IBlendingRecipeRepository GetRepository();
        IQueryable<BlendingRecipe> GetQueryable();
        IList<BlendingRecipe> GetAll();
        IList<BlendingRecipe> GetObjectsByTargetItemId(int TargetItemId);
        BlendingRecipe GetObjectById(int Id);
        BlendingRecipe CreateObject(BlendingRecipe blendingRecipe, IItemService _itemService, IItemTypeService _itemTypeService);
        BlendingRecipe UpdateObject(BlendingRecipe blendingRecipe, IItemService _itemService, IItemTypeService _itemTypeService);
        BlendingRecipe SoftDeleteObject(BlendingRecipe blendingRecipe, IBlendingRecipeDetailService _blendingRecipeDetailService, IBlendingWorkOrderService _blendingWorkOrderService);
        BlendingRecipe AdjustQuantity(BlendingRecipe blendingRecipe, int quantity);
        bool DeleteObject(int Id);
        bool IsNameDuplicated(BlendingRecipe blendingRecipe);
    }
}