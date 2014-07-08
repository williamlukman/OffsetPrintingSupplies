using Core.DomainModel;
using Core.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Context;
using Data.Repository;
using System.Data;

namespace Data.Repository
{
    public class AbstractItemRepository : EfRepository<AbstractItem>, IAbstractItemRepository
    {

        private OffsetPrintingSuppliesEntities entities;
        public AbstractItemRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IList<AbstractItem> GetAll()
        {
            return (from x in Context.AbstractItems where !x.IsDeleted select x).ToList();
        }

        public IList<AbstractItem> GetObjectsByItemTypeId(int ItemTypeId)
        {
            return (from x in Context.AbstractItems where x.ItemTypeId == ItemTypeId && !x.IsDeleted select x).ToList();
        }

        public int GetQuantityById(int Id)
        {
            return (from x in Context.AbstractItems where x.Id == Id && !x.IsDeleted select x).FirstOrDefault().Quantity;
        }

        public AbstractItem GetObjectById(int Id)
        {
            AbstractItem item = (from x in Context.AbstractItems where x.Id == Id && !x.IsDeleted select x).FirstOrDefault();
            if (item != null) { item.Errors = new Dictionary<string, string>(); }
            return item;
        }

        public AbstractItem CreateObject(AbstractItem item)
        {
            item.IsDeleted = false;
            item.CreatedAt = DateTime.Now;
            return Create(item);
        }

        public AbstractItem UpdateObject(AbstractItem item)
        {
            item.UpdatedAt = DateTime.Now;
            Update(item);
            return item;
        }

        public AbstractItem SoftDeleteObject(AbstractItem item)
        {
            item.IsDeleted = true;
            item.DeletedAt = DateTime.Now;
            Update(item);
            return item;
        }

        public bool DeleteObject(int Id)
        {
            AbstractItem item = (from x in Context.AbstractItems where x.Id == Id select x).FirstOrDefault();
            return (Delete(item) == 1) ? true : false;
        }

        public AbstractItem GetObjectBySku(string Sku)
        {
            return (from x in Context.AbstractItems where x.Sku == Sku && !x.IsDeleted select x).FirstOrDefault();
        }

        public bool IsSkuDuplicated(AbstractItem item)
        {
            IQueryable<AbstractItem> items = (from x in Context.AbstractItems where x.Sku == item.Sku && !x.IsDeleted && x.Id != item.Id select x);
            return (items.Count() > 0 ? true : false);
        }
    }
}