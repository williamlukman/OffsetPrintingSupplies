using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Service
{
    public class BlendingRecipeService : IBlendingRecipeService
    {
        private IBlendingRecipeRepository _repository;
        private IBlendingRecipeValidator _validator;
        public BlendingRecipeService(IBlendingRecipeRepository _blendingRecipeRepository, IBlendingRecipeValidator _blendingRecipeValidator)
        {
            _repository = _blendingRecipeRepository;
            _validator = _blendingRecipeValidator;
        }

        public IBlendingRecipeValidator GetValidator()
        {
            return _validator;
        }

        public IBlendingRecipeRepository GetRepository()
        {
            return _repository;
        }

        public IQueryable<BlendingRecipe> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<BlendingRecipe> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<BlendingRecipe> GetObjectsByTargetItemId(int TargetItemId)
        {
            return _repository.GetObjectsByTargetItemId(TargetItemId);
        }

        public BlendingRecipe GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public BlendingRecipe CreateObject(BlendingRecipe blendingRecipe, IItemService _itemService, IItemTypeService _itemTypeService)
        {
            blendingRecipe.Errors = new Dictionary<String, String>();
            if (_validator.ValidCreateObject(blendingRecipe, this, _itemService, _itemTypeService))
            {
                blendingRecipe = _repository.CreateObject(blendingRecipe);
            }
            return blendingRecipe;
        }

        public BlendingRecipe UpdateObject(BlendingRecipe blendingRecipe, IItemService _itemService, IItemTypeService _itemTypeService)
        {
            if (_validator.ValidUpdateObject(blendingRecipe, this, _itemService, _itemTypeService))
            {
                _repository.UpdateObject(blendingRecipe);
            }
            return blendingRecipe;
        }

        public BlendingRecipe SoftDeleteObject(BlendingRecipe blendingRecipe, IBlendingRecipeDetailService _blendingRecipeDetailService, IBlendingWorkOrderService _blendingWorkOrderService)
        {
            if (_validator.ValidDeleteObject(blendingRecipe, _blendingRecipeDetailService, _blendingWorkOrderService))
            {
                _repository.SoftDeleteObject(blendingRecipe);
            }
            return blendingRecipe;
        }

        public BlendingRecipe AdjustQuantity(BlendingRecipe blendingRecipe, int quantity)
        {
            blendingRecipe.TargetQuantity += quantity;
            return (blendingRecipe = _validator.ValidAdjustQuantity(blendingRecipe) ? _repository.UpdateObject(blendingRecipe) : blendingRecipe);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsNameDuplicated(BlendingRecipe blendingRecipe)
        {
            return _repository.IsNameDuplicated(blendingRecipe);
        }

    }
}