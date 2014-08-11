using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IContactRepository : IRepository<Contact>
    {
        IList<Contact> GetAll();
        Contact GetObjectById(int Id);
        Contact CreateObject(Contact contact);
        Contact UpdateObject(Contact contact);
        Contact SoftDeleteObject(Contact contact);
        bool DeleteObject(int Id);
    }
}