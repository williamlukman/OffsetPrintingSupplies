using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IBlendingRecipeDetailService
    {
        IBlendingRecipeDetailValidator GetValidator();
        IBlendingRecipeDetailRepository GetRepository();
        IQueryable<BlendingRecipeDetail> GetQueryable();
        IList<BlendingRecipeDetail> GetAll();
        IList<BlendingRecipeDetail> GetObjectsByBlendingRecipeId(int BlendingRecipeId);
        BlendingRecipeDetail GetObjectById(int Id);
        BlendingRecipeDetail CreateObject(BlendingRecipeDetail blendingRecipeDetail, IBlendingRecipeService _blendingRecipeService, IItemService _itemService);
        BlendingRecipeDetail UpdateObject(BlendingRecipeDetail blendingRecipeDetail, IBlendingRecipeService _blendingRecipeService, IItemService _itemService);
        BlendingRecipeDetail SoftDeleteObject(BlendingRecipeDetail blendingRecipeDetail);
        BlendingRecipeDetail AdjustQuantity(BlendingRecipeDetail blendingRecipeDetail, int quantity);
        bool DeleteObject(int Id);
    }
}