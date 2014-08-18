using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IQuantityPricingValidator
    {
        QuantityPricing VSameItemTypeId(QuantityPricing quantityPricing, int oldItemTypeId);
        QuantityPricing VHasUniqueCombination(QuantityPricing quantityPricing, IQuantityPricingService _quantityPricingService);
        QuantityPricing VIsValidItemType(QuantityPricing quantityPricing, IItemTypeService _itemTypeService);
        QuantityPricing VIsValidMinQuantity(QuantityPricing quantityPricing);
        QuantityPricing VIsValidMaxQuantity(QuantityPricing quantityPricing);
        QuantityPricing VIsValidDiscount(QuantityPricing quantityPricing);

        QuantityPricing VCreateObject(QuantityPricing quantityPricing, IQuantityPricingService _quantityPricingService, IItemTypeService _itemTypeService);
        QuantityPricing VUpdateObject(QuantityPricing quantityPricing, int oldItemTypeId, IQuantityPricingService _quantityPricingService, IItemTypeService _itemTypeService);
        QuantityPricing VDeleteObject(QuantityPricing quantityPricing);
        bool ValidCreateObject(QuantityPricing quantityPricing, IQuantityPricingService _quantityPricingService, IItemTypeService _itemTypeService);
        bool ValidUpdateObject(QuantityPricing quantityPricing, int oldItemTypeId, IQuantityPricingService _quantityPricingService, IItemTypeService _itemTypeService);
        bool ValidDeleteObject(QuantityPricing quantityPricing);
        bool isValid(QuantityPricing quantityPricing);
        string PrintError(QuantityPricing quantityPricing);
    }
}
