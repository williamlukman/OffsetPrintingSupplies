using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;
using Core.Interface.Validation;

namespace Validation.Validation
{
    public class QuantityPricingValidator : IQuantityPricingValidator
    {
        public QuantityPricing VSameItemTypeId(QuantityPricing quantityPricing, int oldItemTypeId)
        {
            if (quantityPricing.ItemTypeId != oldItemTypeId)
            {
                quantityPricing.Errors.Add("ItemTypeId", "Tidak boleh beda");
            }
            return quantityPricing;
        }

        public QuantityPricing VHasUniqueCombination(QuantityPricing quantityPricing, IQuantityPricingService _quantityPricingService)
        {
            if (_quantityPricingService.IsQuantityPricingDuplicated(quantityPricing))
            {
                quantityPricing.Errors.Add("Generic", "Kombinasi ItemTypeId, MinQuantity dan MaxQuantity Tidak boleh ada duplikasi");
            }
            return quantityPricing;
        }

        public QuantityPricing VIsValidItemType(QuantityPricing quantityPricing, IItemTypeService _itemTypeService)
        {
            ItemType itemType = _itemTypeService.GetObjectById(quantityPricing.ItemTypeId);
            if (itemType == null)
            {
                quantityPricing.Errors.Add("ItemTypeId", "Tidak valid");
            }
            return quantityPricing;
        }

        public QuantityPricing VIsValidMinQuantity(QuantityPricing quantityPricing)
        {
            if (quantityPricing.IsMinimumQuantity && quantityPricing.MinQuantity <= 1)
            {
                quantityPricing.Errors.Add("MinQuantity", "Harus lebih besar dari 1");
            }
            return quantityPricing;
        }

        public QuantityPricing VIsValidMaxQuantity(QuantityPricing quantityPricing)
        {
            if (!quantityPricing.IsInfiniteMaxQuantity && quantityPricing.MaxQuantity < quantityPricing.MinQuantity)
            {
                quantityPricing.Errors.Add("MaxQuantity", "Harus lebih besar atau sama dengan MinQuantity");
            }
            return quantityPricing;
        }

        public QuantityPricing VIsValidDiscount(QuantityPricing quantityPricing)
        {
            if (quantityPricing.Discount < 0 || quantityPricing.Discount > 100)
            {
                quantityPricing.Errors.Add("Discount", "Harus diantara 0 sampai 100");
            }
            return quantityPricing;
        }

        public QuantityPricing VCreateObject(QuantityPricing quantityPricing, IQuantityPricingService _quantityPricingService, IItemTypeService _itemTypeService)
        {
            VIsValidMinQuantity(quantityPricing);
            if (!isValid(quantityPricing)) { return quantityPricing; }
            VIsValidMaxQuantity(quantityPricing);
            if (!isValid(quantityPricing)) { return quantityPricing; }
            VIsValidDiscount(quantityPricing);
            if (!isValid(quantityPricing)) { return quantityPricing; }
            VHasUniqueCombination(quantityPricing, _quantityPricingService);
            if (!isValid(quantityPricing)) { return quantityPricing; }
            VIsValidItemType(quantityPricing, _itemTypeService);
            return quantityPricing;
        }

        public QuantityPricing VUpdateObject(QuantityPricing quantityPricing, int oldItemTypeId, IQuantityPricingService _quantityPricingService, IItemTypeService _itemTypeService)
        {
            VSameItemTypeId(quantityPricing, oldItemTypeId);
            if (!isValid(quantityPricing)) { return quantityPricing; }
            return VCreateObject(quantityPricing, _quantityPricingService, _itemTypeService);
        }

        public QuantityPricing VDeleteObject(QuantityPricing quantityPricing)
        {
            
            return quantityPricing;
        }

        public bool ValidCreateObject(QuantityPricing quantityPricing, IQuantityPricingService _quantityPricingService, IItemTypeService _itemTypeService)
        {
            VCreateObject(quantityPricing, _quantityPricingService, _itemTypeService);
            return isValid(quantityPricing);
        }

        public bool ValidUpdateObject(QuantityPricing quantityPricing, int oldItemTypeId, IQuantityPricingService _quantityPricingService, IItemTypeService _itemTypeService)
        {
            quantityPricing.Errors.Clear();
            VUpdateObject(quantityPricing, oldItemTypeId, _quantityPricingService, _itemTypeService);
            return isValid(quantityPricing);
        }

        public bool ValidDeleteObject(QuantityPricing quantityPricing)
        {
            quantityPricing.Errors.Clear();
            VDeleteObject(quantityPricing);
            return isValid(quantityPricing);
        }

        public bool isValid(QuantityPricing obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(QuantityPricing obj)
        {
            string erroroutput = "";
            KeyValuePair<string, string> first = obj.Errors.ElementAt(0);
            erroroutput += first.Key + "," + first.Value;
            foreach (KeyValuePair<string, string> pair in obj.Errors.Skip(1))
            {
                erroroutput += Environment.NewLine;
                erroroutput += pair.Key + "," + pair.Value;
            }
            return erroroutput;
        }

    }
}
