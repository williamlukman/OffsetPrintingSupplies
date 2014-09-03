using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IContactRepository : IRepository<Contact>
    {
        IQueryable<Contact> GetQueryable();
        IList<Contact> GetAll();
        Contact GetObjectById(int Id);
        Contact GetObjectByName(string Name);
        Contact CreateObject(Contact contact);
        Contact UpdateObject(Contact contact);
        Contact SoftDeleteObject(Contact contact);
        bool DeleteObject(int Id);
    }
}