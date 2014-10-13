using Core.DomainModel;
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
    public class CompoundRepository : EfRepository<Compound>, ICompoundRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public CompoundRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<Compound> GetQueryable()
        {
            return FindAll();
        }

        public IList<Compound> GetAll()
        {
            return (from x in Context.Items.OfType<Compound>() where !x.IsDeleted select x).ToList();
        }

        public IList<Compound> GetObjectsByItemTypeId(int ItemTypeId)
        {
            return (from x in Context.Items.OfType<Compound>() where x.ItemTypeId == ItemTypeId && !x.IsDeleted select x).ToList();
        }

        public IList<Compound> GetObjectsByUoMId(int UoMId)
        {
            return (from x in Context.Items.OfType<Compound>() where x.UoMId == UoMId && !x.IsDeleted select x).ToList();
        }

        public Compound GetObjectById(int Id)
        {
            Compound compound = (from x in Context.Items.OfType<Compound>() where x.Id == Id && !x.IsDeleted select x).FirstOrDefault();
            if (compound != null) { compound.Errors = new Dictionary<string, string>(); }
            return compound;
        }

        public Compound CreateObject(Compound compound)
        {
            compound.Quantity = 0;
            compound.IsDeleted = false;
            compound.CreatedAt = DateTime.Now;
            return Create(compound);
        }

        public Compound UpdateObject(Compound compound)
        {
            compound.UpdatedAt = DateTime.Now;
            Update(compound);
            return compound;
        }

        public Compound SoftDeleteObject(Compound compound)
        {
            compound.IsDeleted = true;
            compound.DeletedAt = DateTime.Now;
            Update(compound);
            return compound;
        }

        public bool DeleteObject(int Id)
        {
            Compound compound = (from x in Context.Items.OfType<Compound>() where x.Id == Id select x).FirstOrDefault();
            return (Delete(compound) == 1) ? true : false;
        }

        public Compound GetObjectBySku(string Sku)
        {
            return (from x in Context.Items.OfType<Compound>() where x.Sku == Sku && !x.IsDeleted select x).FirstOrDefault();
        }

        public bool IsSkuDuplicated(Compound compound)
        {
            IQueryable<Compound> items = (from x in Context.Items.OfType<Compound>() where x.Sku == compound.Sku && !x.IsDeleted && x.Id != compound.Id select x);
            return (items.Count() > 0 ? true : false);
        }
    }
}