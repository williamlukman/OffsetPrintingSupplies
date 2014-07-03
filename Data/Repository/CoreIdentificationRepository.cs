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
    public class CoreIdentificationRepository : EfRepository<CoreIdentification>, ICoreIdentificationRepository
    {

        private OffsetPrintingSuppliesEntities entities;
        public CoreIdentificationRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IList<CoreIdentification> GetAll()
        {
            return FindAll().ToList();
        }

        public IList<CoreIdentification> GetAllObjectsInHouse()
        {
            return FindAll(x => x.IsInHouse && !x.IsDeleted).ToList();
        }

        public IList<CoreIdentification> GetAllObjectsByCustomerId(int CustomerId)
        {
            return FindAll(x => x.CustomerId == CustomerId && !x.IsDeleted).ToList();
        }

        public CoreIdentification GetObjectById(int Id)
        {
            CoreIdentification coreIdentification = Find(x => x.Id == Id && !x.IsDeleted);
            if (coreIdentification != null) { coreIdentification.Errors = new Dictionary<string, string>(); }
            return coreIdentification;
        }

        public CoreIdentification CreateObject(CoreIdentification coreIdentification)
        {
            coreIdentification.Errors = new Dictionary<string, string>();
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
            coreIdentification.ConfirmationDate = DateTime.Now;
            Update(coreIdentification);
            return coreIdentification;
        }

        public CoreIdentification UnconfirmObject(CoreIdentification coreIdentification)
        {
            coreIdentification.IsConfirmed = false;
            coreIdentification.ConfirmationDate = null;
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