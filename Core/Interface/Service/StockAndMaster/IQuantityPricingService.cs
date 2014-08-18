using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Validation;

namespace Core.Interface.Service
{
    public interface IQuantityPricingService
    {
        IQuantityPricingValidator GetValidator();
        IList<QuantityPricing> GetAll();
        QuantityPricing GetObjectById(int Id);
        QuantityPricing CreateObject(QuantityPricing quantityPricing, IItemTypeService _itemTypeService);
        QuantityPricing UpdateObject(QuantityPricing quantityPricing, int oldItemTypeId, IItemTypeService _itemTypeService);
        QuantityPricing SoftDeleteObject(QuantityPricing quantityPricing);
        bool DeleteObject(int Id);
        bool IsQuantityPricingDuplicated(QuantityPricing quantityPricing);
    }
}
