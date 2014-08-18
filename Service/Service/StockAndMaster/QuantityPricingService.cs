using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;

namespace Service.Service
{
    public class QuantityPricingService : IQuantityPricingService
    {
        private IQuantityPricingRepository _repository;
        private IQuantityPricingValidator _validator;
        public QuantityPricingService(IQuantityPricingRepository _quantityPricingRepository, IQuantityPricingValidator _quantityPricingValidator)
        {
            _repository = _quantityPricingRepository;
            _validator = _quantityPricingValidator;
        }

        public IQuantityPricingValidator GetValidator()
        {
            return _validator;
        }

        public IList<QuantityPricing> GetAll()
        {
            return _repository.GetAll();
        }

        public QuantityPricing GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public QuantityPricing CreateObject(QuantityPricing quantityPricing, IItemTypeService _itemTypeService)
        {
            quantityPricing.Errors = new Dictionary<String, String>();
            if (_validator.ValidCreateObject(quantityPricing, this, _itemTypeService))
            {
                quantityPricing = _repository.CreateObject(quantityPricing);
            }
            return quantityPricing;
        }

        public QuantityPricing UpdateObject(QuantityPricing quantityPricing, int oldItemTypeId, IItemTypeService _itemTypeService)
        {
            if (_validator.ValidUpdateObject(quantityPricing, oldItemTypeId, this, _itemTypeService))
            {
                quantityPricing = _repository.UpdateObject(quantityPricing);
            }
            return quantityPricing;
        }

        public QuantityPricing SoftDeleteObject(QuantityPricing quantityPricing)
        {
            return (quantityPricing = _validator.ValidDeleteObject(quantityPricing) ?
                    _repository.SoftDeleteObject(quantityPricing) : quantityPricing);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsQuantityPricingDuplicated(QuantityPricing quantityPricing)
        {
            IQueryable<QuantityPricing> quantityPricings = _repository.FindAll(x => x.ItemTypeId == quantityPricing.ItemTypeId && x.MinQuantity == quantityPricing.MinQuantity && x.MaxQuantity == quantityPricing.MaxQuantity && !x.IsDeleted && x.Id != quantityPricing.Id);
            return (quantityPricings.Count() > 0 ? true : false);
        }
    }
}
