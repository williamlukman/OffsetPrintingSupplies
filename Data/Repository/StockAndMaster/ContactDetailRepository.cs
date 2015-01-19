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
    public class ContactDetailRepository : EfRepository<ContactDetail>, IContactDetailRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public ContactDetailRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<ContactDetail> GetQueryable()
        {
            return FindAll();
        }

        public IList<ContactDetail> GetAll()
        {
            return FindAll().ToList();
        }

        public ContactDetail GetObjectById(int Id)
        {
            ContactDetail contactDetail = Find(x => x.Id == Id && !x.IsDeleted);
            if (contactDetail != null) { contactDetail.Errors = new Dictionary<string, string>(); }
            return contactDetail;
        }

        public ContactDetail GetObjectByName(string Name)
        {
            ContactDetail contactDetail = Find(x => x.Name == Name && !x.IsDeleted);
            if (contactDetail != null) { contactDetail.Errors = new Dictionary<string, string>(); }
            return contactDetail;
        }

        public ContactDetail CreateObject(ContactDetail contactDetail)
        {
            contactDetail.IsDeleted = false;
            contactDetail.CreatedAt = DateTime.Now;
            return Create(contactDetail);
        }

        public ContactDetail UpdateObject(ContactDetail contactDetail)
        {
            contactDetail.UpdatedAt = DateTime.Now;
            Update(contactDetail);
            return contactDetail;
        }

        public ContactDetail SoftDeleteObject(ContactDetail contactDetail)
        {
            contactDetail.IsDeleted = true;
            contactDetail.DeletedAt = DateTime.Now;
            Update(contactDetail);
            return contactDetail;
        }

        public bool DeleteObject(int Id)
        {
            ContactDetail contactDetail = Find(x => x.Id == Id);
            return (Delete(contactDetail) == 1) ? true : false;
        }
    }
}