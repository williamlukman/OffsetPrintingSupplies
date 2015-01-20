using Core.DomainModel;
using Core.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Context;
using System.Data;

namespace Data.Repository
{
    public class SubTypeRepository : EfRepository<SubType>, ISubTypeRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public SubTypeRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<SubType> GetQueryable()
        {
            return FindAll();
        }

        public IList<SubType> GetAll()
        {
            return FindAll().ToList();
        }

        public SubType GetObjectById(int Id)
        {
            SubType subType = Find(x => x.Id == Id && !x.IsDeleted);
            if (subType != null) { subType.Errors = new Dictionary<string, string>(); }
            return subType;
        }

        public SubType GetObjectByName(string Name)
        {
            SubType subType = Find(x => x.Name == Name && !x.IsDeleted);
            if (subType != null) { subType.Errors = new Dictionary<string, string>(); }
            return subType;
        }

        public SubType CreateObject(SubType subType)
        {
            subType.IsDeleted = false;
            subType.CreatedAt = DateTime.Now;
            return Create(subType);
        }

        public SubType UpdateObject(SubType subType)
        {
            subType.UpdatedAt = DateTime.Now;
            Update(subType);
            return subType;
        }

        public SubType SoftDeleteObject(SubType subType)
        {
            subType.IsDeleted = true;
            subType.DeletedAt = DateTime.Now;
            Update(subType);
            return subType;
        }

        public bool DeleteObject(int Id)
        {
            SubType subType = Find(x => x.Id == Id);
            return (Delete(subType) == 1) ? true : false;
        }

    }
}