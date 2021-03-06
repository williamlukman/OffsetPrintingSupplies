﻿using Core.DomainModel;
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
    public class BlendingRecipeRepository : EfRepository<BlendingRecipe>, IBlendingRecipeRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public BlendingRecipeRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<BlendingRecipe> GetQueryable()
        {
            return FindAll();
        }

        public IList<BlendingRecipe> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<BlendingRecipe> GetObjectsByTargetItemId(int TargetItemId)
        {
            return FindAll(x => x.TargetItemId == TargetItemId && !x.IsDeleted).ToList();
        }

        public BlendingRecipe GetObjectById(int Id)
        {
            BlendingRecipe blendingRecipe = FindAll(x => x.Id == Id && !x.IsDeleted).FirstOrDefault();
            if (blendingRecipe != null) { blendingRecipe.Errors = new Dictionary<string, string>(); }
            return blendingRecipe;
        }

        public BlendingRecipe CreateObject(BlendingRecipe blendingRecipe)
        {
            blendingRecipe.IsDeleted = false;
            blendingRecipe.CreatedAt = DateTime.Now;
            return Create(blendingRecipe);
        }

        public BlendingRecipe UpdateObject(BlendingRecipe blendingRecipe)
        {
            blendingRecipe.UpdatedAt = DateTime.Now;
            Update(blendingRecipe);
            return blendingRecipe;
        }

        public BlendingRecipe SoftDeleteObject(BlendingRecipe blendingRecipe)
        {
            blendingRecipe.IsDeleted = true;
            blendingRecipe.DeletedAt = DateTime.Now;
            Update(blendingRecipe);
            return blendingRecipe;
        }

        public bool DeleteObject(int Id)
        {
            BlendingRecipe blendingRecipe = FindAll(x => x.Id == Id).FirstOrDefault();
            return (Delete(blendingRecipe) == 1) ? true : false;
        }

        public bool IsNameDuplicated(BlendingRecipe blendingRecipe)
        {
            IQueryable<BlendingRecipe> blendingRecipes = FindAll(x => x.Name == blendingRecipe.Name && !x.IsDeleted && x.Id != blendingRecipe.Id);
            return (blendingRecipes.Count() > 0 ? true : false);
        }

    }
}