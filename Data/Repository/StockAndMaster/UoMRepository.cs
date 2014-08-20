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
    public class UoMRepository : EfRepository<UoM>, IUoMRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public UoMRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<UoM> GetQueryable()
        {
            return FindAll();
        }

        public IList<UoM> GetAll()
        {
            return FindAll().ToList();
        }

        public UoM GetObjectById(int Id)
        {
            UoM unitOfMeasurement = Find(x => x.Id == Id && !x.IsDeleted);
            if (unitOfMeasurement != null) { unitOfMeasurement.Errors = new Dictionary<string, string>(); }
            return unitOfMeasurement;
        }

        public UoM CreateObject(UoM unitOfMeasurement)
        {
            unitOfMeasurement.IsDeleted = false;
            unitOfMeasurement.CreatedAt = DateTime.Now;
            return Create(unitOfMeasurement);
        }

        public UoM UpdateObject(UoM unitOfMeasurement)
        {
            unitOfMeasurement.UpdatedAt = DateTime.Now;
            Update(unitOfMeasurement);
            return unitOfMeasurement;
        }

        public UoM SoftDeleteObject(UoM unitOfMeasurement)
        {
            unitOfMeasurement.IsDeleted = true;
            unitOfMeasurement.DeletedAt = DateTime.Now;
            Update(unitOfMeasurement);
            return unitOfMeasurement;
        }

        public bool DeleteObject(int Id)
        {
            UoM unitOfMeasurement = Find(x => x.Id == Id);
            return (Delete(unitOfMeasurement) == 1) ? true : false;
        }

    }
}