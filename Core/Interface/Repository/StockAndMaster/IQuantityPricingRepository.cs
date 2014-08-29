using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;

namespace Core.Interface.Repository
{
    public interface IQuantityPricingRepository : IRepository<QuantityPricing>
    {
        IQueryable<QuantityPricing> GetQueryable();
        IList<QuantityPricing> GetAll();
        IQueryable<QuantityPricing> GetQueryableObjectsByItemTypeId(int ItemTypeId);
        IList<QuantityPricing> GetObjectsByItemTypeId(int ItemTypeId);
        QuantityPricing GetObjectById(int Id);
        QuantityPricing GetObjectByItemTypeIdAndQuantity(int ItemTypeId, int Quantity);
        QuantityPricing CreateObject(QuantityPricing quantityPricing);
        QuantityPricing UpdateObject(QuantityPricing quantityPricing);
        QuantityPricing SoftDeleteObject(QuantityPricing quantityPricing);
        bool DeleteObject(int Id);
    }
}
