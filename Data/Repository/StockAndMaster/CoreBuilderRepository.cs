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
    public class CoreBuilderRepository : EfRepository<CoreBuilder>, ICoreBuilderRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public CoreBuilderRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<CoreBuilder> GetQueryable()
        {
            return FindAll();
        }

        public IList<CoreBuilder> GetAll()
        {
            return FindAll().ToList();
        }

        public IList<CoreBuilder> GetObjectsByItemId(int ItemId)
        {
            return FindAll(x => !x.IsDeleted && (x.UsedCoreItemId == ItemId || x.NewCoreItemId == ItemId)).ToList();
        }

        public Item GetUsedCore(int id)
        {
            using (var db = GetContext())
            {
                CoreBuilder coreBuilder = GetObjectById(id);
                Item item =
                    (from obj in db.Items
                     where obj.Id == coreBuilder.UsedCoreItemId && !obj.IsDeleted
                     select obj).First();
                if (item != null) { item.Errors = new Dictionary<string, string>(); }
                return item;
            }
        }

        public Item GetNewCore(int id)
        {
            using (var db = GetContext())
            {
                CoreBuilder coreBuilder = GetObjectById(id);
                Item item = (from obj in db.Items
                             where obj.Id == coreBuilder.NewCoreItemId && !obj.IsDeleted
                             select obj).FirstOrDefault();
                if (item != null) { item.Errors = new Dictionary<string, string>(); }
                return item;
            }
        }

        public CoreBuilder GetObjectById(int Id)
        {
            CoreBuilder coreBuilder = Find(x => x.Id == Id && !x.IsDeleted);
            if (coreBuilder != null) { coreBuilder.Errors = new Dictionary<string, string>(); }
            return coreBuilder;
        }

        public CoreBuilder CreateObject(CoreBuilder coreBuilder)
        {
            coreBuilder.IsDeleted = false;
            coreBuilder.CreatedAt = DateTime.Now;
            return Create(coreBuilder);
        }

        public CoreBuilder UpdateObject(CoreBuilder coreBuilder)
        {
            coreBuilder.UpdatedAt = DateTime.Now;
            Update(coreBuilder);
            return coreBuilder;
        }

        public CoreBuilder SoftDeleteObject(CoreBuilder coreBuilder)
        {
            coreBuilder.IsDeleted = true;
            coreBuilder.DeletedAt = DateTime.Now;
            Update(coreBuilder);
            return coreBuilder;
        }

        public bool DeleteObject(int Id)
        {
            CoreBuilder coreBuilder = Find(x => x.Id == Id);
            return (Delete(coreBuilder) == 1) ? true : false;
        }
    }
}