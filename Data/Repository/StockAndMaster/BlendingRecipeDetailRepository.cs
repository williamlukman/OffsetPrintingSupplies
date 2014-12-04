using Core.DomainModel;
using Core.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Context;
using Data.Repository;
using System.Data;

namespace Data.Repository
{
    public class BlendingRecipeDetailRepository : EfRepository<BlendingRecipeDetail>, IBlendingRecipeDetailRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public BlendingRecipeDetailRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<BlendingRecipeDetail> GetQueryable()
        {
            return FindAll();
        }

        public IList<BlendingRecipeDetail> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<BlendingRecipeDetail> GetObjectsByBlendingRecipeId(int BlendingRecipeId)
        {
            return FindAll(x => x.BlendingRecipeId == BlendingRecipeId && !x.IsDeleted).ToList();
        }

        public BlendingRecipeDetail GetObjectById(int Id)
        {
            BlendingRecipeDetail blendingRecipeDetail = FindAll(x => x.Id == Id && !x.IsDeleted).FirstOrDefault();
            if (blendingRecipeDetail != null) { blendingRecipeDetail.Errors = new Dictionary<string, string>(); }
            return blendingRecipeDetail;
        }

        public BlendingRecipeDetail CreateObject(BlendingRecipeDetail blendingRecipeDetail)
        {
            blendingRecipeDetail.IsDeleted = false;
            blendingRecipeDetail.CreatedAt = DateTime.Now;
            return Create(blendingRecipeDetail);
        }

        public BlendingRecipeDetail UpdateObject(BlendingRecipeDetail blendingRecipeDetail)
        {
            blendingRecipeDetail.UpdatedAt = DateTime.Now;
            Update(blendingRecipeDetail);
            return blendingRecipeDetail;
        }

        public BlendingRecipeDetail SoftDeleteObject(BlendingRecipeDetail blendingRecipeDetail)
        {
            blendingRecipeDetail.IsDeleted = true;
            blendingRecipeDetail.DeletedAt = DateTime.Now;
            Update(blendingRecipeDetail);
            return blendingRecipeDetail;
        }

        public bool DeleteObject(int Id)
        {
            BlendingRecipeDetail blendingRecipeDetail = FindAll(x => x.Id == Id).FirstOrDefault();
            return (Delete(blendingRecipeDetail) == 1) ? true : false;
        }

    }
}