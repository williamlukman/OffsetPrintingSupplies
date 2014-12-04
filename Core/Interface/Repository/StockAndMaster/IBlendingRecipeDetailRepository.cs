using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IBlendingRecipeDetailRepository : IRepository<BlendingRecipeDetail>
    {
        IQueryable<BlendingRecipeDetail> GetQueryable();
        IList<BlendingRecipeDetail> GetAll();
        IList<BlendingRecipeDetail> GetObjectsByBlendingRecipeId(int BlendingRecipeId);
        BlendingRecipeDetail GetObjectById(int Id);
        BlendingRecipeDetail CreateObject(BlendingRecipeDetail blendingRecipeDetail);
        BlendingRecipeDetail UpdateObject(BlendingRecipeDetail blendingRecipeDetail);
        BlendingRecipeDetail SoftDeleteObject(BlendingRecipeDetail blendingRecipeDetail);
        bool DeleteObject(int Id);
    }
}