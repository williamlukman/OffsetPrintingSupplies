using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;

namespace Data.Repository
{
    public class QuantityPricingRepository : EfRepository<QuantityPricing>, IQuantityPricingRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public QuantityPricingRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IList<QuantityPricing> GetAll()
        {
            return FindAll().ToList();
        }

        public IList<QuantityPricing> GetObjectsByItemTypeId(int ItemTypeId)
        {
            return FindAll(x => x.ItemTypeId == ItemTypeId && !x.IsDeleted).ToList();
        }

        public QuantityPricing GetObjectById(int Id)
        {
            QuantityPricing quantityPricing = Find(x => x.Id == Id && !x.IsDeleted);
            if (quantityPricing != null) { quantityPricing.Errors = new Dictionary<string, string>(); }
            return quantityPricing;
        }

        public QuantityPricing GetObjectByItemTypeIdAndQuantity(int ItemTypeId, int Quantity)
        {
            QuantityPricing quantityPricing = Find(x => x.ItemTypeId == ItemTypeId && Quantity >= x.MinQuantity && (x.IsInfiniteMaxQuantity || Quantity <= x.MinQuantity) && !x.IsDeleted);
            if (quantityPricing != null) { quantityPricing.Errors = new Dictionary<string, string>(); }
            return quantityPricing;
        }

        public QuantityPricing CreateObject(QuantityPricing quantityPricing)
        {
            quantityPricing.IsDeleted = false;
            quantityPricing.CreatedAt = DateTime.Now;
            return Create(quantityPricing);
        }

        public QuantityPricing UpdateObject(QuantityPricing quantityPricing)
        {
            quantityPricing.UpdatedAt = DateTime.Now;
            Update(quantityPricing);
            return quantityPricing;
        }

        public QuantityPricing SoftDeleteObject(QuantityPricing quantityPricing)
        {
            quantityPricing.IsDeleted = true;
            quantityPricing.DeletedAt = DateTime.Now;
            Update(quantityPricing);
            return quantityPricing;
        }

        public bool DeleteObject(int Id)
        {
            QuantityPricing quantityPricing = Find(x => x.Id == Id);
            return (Delete(quantityPricing) == 1) ? true : false;
        }
    }
}
