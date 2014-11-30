using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IBlendingRecipeRepository : IRepository<BlendingRecipe>
    {
        IQueryable<BlendingRecipe> GetQueryable();
        IList<BlendingRecipe> GetAll();
        IList<BlendingRecipe> GetObjectsByTargetItemId(int TargetItemId);
        BlendingRecipe GetObjectById(int Id);
        BlendingRecipe CreateObject(BlendingRecipe blendingRecipe);
        BlendingRecipe UpdateObject(BlendingRecipe blendingRecipe);
        BlendingRecipe SoftDeleteObject(BlendingRecipe blendingRecipe);
        bool DeleteObject(int Id);
        bool IsNameDuplicated(BlendingRecipe blendingRecipe);
    }
}