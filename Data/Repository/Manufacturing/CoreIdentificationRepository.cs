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
    public class CoreIdentificationRepository : EfRepository<CoreIdentification>, ICoreIdentificationRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public CoreIdentificationRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<CoreIdentification> GetQueryable()
        {
            return FindAll();
        }

        public IList<CoreIdentification> GetAll()
        {
            return FindAll().ToList();
        }

        public IList<CoreIdentification> GetAllObjectsInHouse()
        {
            return FindAll(x => x.IsInHouse && !x.IsDeleted).ToList();
        }

        public IList<CoreIdentification> GetAllObjectsByContactId(int ContactId)
        {
            return FindAll(x => x.ContactId == ContactId && !x.IsDeleted).ToList();
        }

        public IList<CoreIdentification> GetAllObjectsByWarehouseId(int WarehouseId)
        {
            return FindAll(x => x.WarehouseId == WarehouseId && !x.IsDeleted).ToList();
        }

        public IList<CoreIdentification> GetConfirmedObjects()
        {
            return FindAll(x => x.IsConfirmed && !x.IsDeleted).ToList();
        }

        public IList<CoreIdentification> GetConfirmedNotCompletedObjects()
        {
            return FindAll(x => x.IsConfirmed && !x.IsCompleted && !x.IsDeleted).ToList();
        }
        
        public CoreIdentification GetObjectById(int Id)
        {
            CoreIdentification coreIdentification = Find(x => x.Id == Id && !x.IsDeleted);
            if (coreIdentification != null) { coreIdentification.Errors = new Dictionary<string, string>(); }
            return coreIdentification;
        }

        public CoreIdentification CreateObject(CoreIdentification coreIdentification)
        {
            coreIdentification.IsCompleted = false;
            coreIdentification.IsConfirmed = false;
            coreIdentification.IsDeleted = false;
            coreIdentification.CreatedAt = DateTime.Now;
            return Create(coreIdentification);
        }

        public CoreIdentification UpdateObject(CoreIdentification coreIdentification)
        {
            coreIdentification.UpdatedAt = DateTime.Now;
            Update(coreIdentification);
            return coreIdentification;
        }

        public CoreIdentification SoftDeleteObject(CoreIdentification coreIdentification)
        {
            coreIdentification.IsDeleted = true;
            coreIdentification.DeletedAt = DateTime.Now;
            Update(coreIdentification);
            return coreIdentification;
        }

        public CoreIdentification ConfirmObject(CoreIdentification coreIdentification)
        {
            coreIdentification.IsConfirmed = true;
            Update(coreIdentification);
            return coreIdentification;
        }

        public CoreIdentification UnconfirmObject(CoreIdentification coreIdentification)
        {
            coreIdentification.IsConfirmed = false;
            coreIdentification.ConfirmationDate = null;
            coreIdentification.UpdatedAt = DateTime.Now;
            Update(coreIdentification);
            return coreIdentification;
        }

        public CoreIdentification CompleteObject(CoreIdentification coreIdentification)
        {
            coreIdentification.IsCompleted = true;
            coreIdentification.UpdatedAt = DateTime.Now;
            Update(coreIdentification);
            return coreIdentification;
        }

        public bool DeleteObject(int Id)
        {
            CoreIdentification coreIdentification = Find(x => x.Id == Id);
            return (Delete(coreIdentification) == 1) ? true : false;
        }
    }
}