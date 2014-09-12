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
    public class ValidCombRepository : EfRepository<ValidComb>, IValidCombRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public ValidCombRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<ValidComb> GetQueryable()
        {
            return FindAll(/*x => !x.IsDeleted*/);
        }

        public IList<ValidComb> GetAll()
        {
            return FindAll(/*x => !x.IsDeleted*/).ToList();
        }

        public ValidComb GetObjectById(int Id)
        {
            ValidComb validComb = Find(x => x.Id == Id /*&& !x.IsDeleted*/);
            if (validComb != null) { validComb.Errors = new Dictionary<string, string>(); }
            return validComb;
        }

        public ValidComb CreateObject(ValidComb validComb)
        {
            //validComb.IsDeleted = false;
            validComb.CreatedAt = DateTime.Now;
            return Create(validComb);
        }

        /*public ValidComb SoftDeleteObject(ValidComb validComb)
        {
            validComb.IsDeleted = true;
            validComb.DeletedAt = DateTime.Now;
            Update(validComb);
            return validComb;
        }*/

        public bool DeleteObject(int Id)
        {
            ValidComb validComb = Find(x => x.Id == Id);
            return (Delete(validComb) == 1) ? true : false;
        }

    }
}