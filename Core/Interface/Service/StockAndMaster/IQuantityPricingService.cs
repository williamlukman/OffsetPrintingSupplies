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
        IQueryable<QuantityPricing> GetQueryable();
        IQuantityPricingValidator GetValidator();
        IList<QuantityPricing> GetAll();
        IQueryable<QuantityPricing> GetQueryableObjectsByItemTypeId(int ItemTypeId);
        IList<QuantityPricing> GetObjectsByItemTypeId(int ItemTypeId);
        QuantityPricing GetObjectById(int Id);
        QuantityPricing GetObjectByItemTypeIdAndQuantity(int ItemTypeId, int Quantity);
        QuantityPricing CreateObject(QuantityPricing quantityPricing, IItemTypeService _itemTypeService);
        QuantityPricing UpdateObject(QuantityPricing quantityPricing, int oldItemTypeId, IItemTypeService _itemTypeService);
        QuantityPricing SoftDeleteObject(QuantityPricing quantityPricing);
        bool DeleteObject(int Id);
        bool IsQuantityPricingDuplicated(QuantityPricing quantityPricing);
    }
}
