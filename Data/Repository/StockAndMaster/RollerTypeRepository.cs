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
    public class RollerTypeRepository : EfRepository<RollerType>, IRollerTypeRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public RollerTypeRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<RollerType> GetQueryable()
        {
            return FindAll();
        }

        public IList<RollerType> GetAll()
        {
            return FindAll().ToList();
        }

        public RollerType GetObjectById(int Id)
        {
            RollerType rollerType = Find(x => x.Id == Id && !x.IsDeleted);
            if (rollerType != null) { rollerType.Errors = new Dictionary<string, string>(); }
            return rollerType;
        }

        public RollerType CreateObject(RollerType rollerType)
        {
            rollerType.IsDeleted = false;
            rollerType.CreatedAt = DateTime.Now;
            return Create(rollerType);
        }

        public RollerType UpdateObject(RollerType rollerType)
        {
            rollerType.UpdatedAt = DateTime.Now;
            Update(rollerType);
            return rollerType;
        }

        public RollerType SoftDeleteObject(RollerType rollerType)
        {
            rollerType.IsDeleted = true;
            rollerType.DeletedAt = DateTime.Now;
            Update(rollerType);
            return rollerType;
        }

        public bool DeleteObject(int Id)
        {
            RollerType rollerType = Find(x => x.Id == Id);
            return (Delete(rollerType) == 1) ? true : false;
        }
    }
}