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
    public class ContactRepository : EfRepository<Contact>, IContactRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public ContactRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<Contact> GetQueryable()
        {
            return FindAll();
        }

        public IList<Contact> GetAll()
        {
            return FindAll().ToList();
        }

        public Contact GetObjectById(int Id)
        {
            Contact contact = Find(x => x.Id == Id && !x.IsDeleted);
            if (contact != null) { contact.Errors = new Dictionary<string, string>(); }
            return contact;
        }

        public Contact GetObjectByName(string Name)
        {
            Contact contact = Find(x => x.Name == Name && !x.IsDeleted);
            if (contact != null) { contact.Errors = new Dictionary<string, string>(); }
            return contact;
        }

        public Contact CreateObject(Contact contact)
        {
            contact.IsDeleted = false;
            contact.CreatedAt = DateTime.Now;
            return Create(contact);
        }

        public Contact UpdateObject(Contact contact)
        {
            contact.UpdatedAt = DateTime.Now;
            Update(contact);
            return contact;
        }

        public Contact SoftDeleteObject(Contact contact)
        {
            contact.IsDeleted = true;
            contact.DeletedAt = DateTime.Now;
            Update(contact);
            return contact;
        }

        public bool DeleteObject(int Id)
        {
            Contact contact = Find(x => x.Id == Id);
            return (Delete(contact) == 1) ? true : false;
        }
    }
}