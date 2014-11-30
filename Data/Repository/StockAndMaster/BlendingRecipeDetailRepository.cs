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
            return (from x in Context.Items.OfType<BlendingRecipeDetail>() where !x.IsDeleted select x).ToList();
        }

        public IList<BlendingRecipeDetail> GetObjectsByBlendingRecipeId(int BlendingRecipeId)
        {
            return (from x in Context.Items.OfType<BlendingRecipeDetail>() where x.BlendingRecipeId == BlendingRecipeId && !x.IsDeleted select x).ToList();
        }

        public BlendingRecipeDetail GetObjectById(int Id)
        {
            BlendingRecipeDetail blendingRecipeDetail = (from x in Context.Items.OfType<BlendingRecipeDetail>() where x.Id == Id && !x.IsDeleted select x).FirstOrDefault();
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
            BlendingRecipeDetail blendingRecipeDetail = (from x in Context.Items.OfType<BlendingRecipeDetail>() where x.Id == Id select x).FirstOrDefault();
            return (Delete(blendingRecipeDetail) == 1) ? true : false;
        }

    }
}