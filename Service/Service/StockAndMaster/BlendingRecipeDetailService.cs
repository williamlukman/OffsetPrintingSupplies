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
    public class BlendingRecipeDetailService : IBlendingRecipeDetailService
    {
        private IBlendingRecipeDetailRepository _repository;
        private IBlendingRecipeDetailValidator _validator;
        public BlendingRecipeDetailService(IBlendingRecipeDetailRepository _blendingRecipeDetailRepository, IBlendingRecipeDetailValidator _blendingRecipeDetailValidator)
        {
            _repository = _blendingRecipeDetailRepository;
            _validator = _blendingRecipeDetailValidator;
        }

        public IBlendingRecipeDetailValidator GetValidator()
        {
            return _validator;
        }

        public IBlendingRecipeDetailRepository GetRepository()
        {
            return _repository;
        }

        public IQueryable<BlendingRecipeDetail> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<BlendingRecipeDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<BlendingRecipeDetail> GetObjectsByBlendingRecipeId(int BlendingRecipeId)
        {
            return _repository.GetObjectsByBlendingRecipeId(BlendingRecipeId);
        }

        public BlendingRecipeDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public BlendingRecipeDetail CreateObject(BlendingRecipeDetail blendingRecipeDetail, IBlendingRecipeService _blendingRecipeService, IItemService _itemService)
        {
            blendingRecipeDetail.Errors = new Dictionary<String, String>();
            if (_validator.ValidCreateObject(blendingRecipeDetail, _blendingRecipeService, this, _itemService))
            {
                blendingRecipeDetail = _repository.CreateObject(blendingRecipeDetail);
            }
            return blendingRecipeDetail;
        }

        public BlendingRecipeDetail UpdateObject(BlendingRecipeDetail blendingRecipeDetail, IBlendingRecipeService _blendingRecipeService, IItemService _itemService)
        {
            if (_validator.ValidUpdateObject(blendingRecipeDetail, _blendingRecipeService, this, _itemService))
            {
                _repository.UpdateObject(blendingRecipeDetail);
            }
            return blendingRecipeDetail;
        }

        public BlendingRecipeDetail SoftDeleteObject(BlendingRecipeDetail blendingRecipeDetail)
        {
            if (_validator.ValidDeleteObject(blendingRecipeDetail))
            {
                _repository.SoftDeleteObject(blendingRecipeDetail);
            }
            return blendingRecipeDetail;
        }

        public BlendingRecipeDetail AdjustQuantity(BlendingRecipeDetail blendingRecipeDetail, int quantity)
        {
            blendingRecipeDetail.Quantity += quantity;
            return (blendingRecipeDetail = _validator.ValidAdjustQuantity(blendingRecipeDetail) ? _repository.UpdateObject(blendingRecipeDetail) : blendingRecipeDetail);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

    }
}