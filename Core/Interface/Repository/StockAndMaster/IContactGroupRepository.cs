using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IContactGroupRepository : IRepository<ContactGroup>
    {
        IQueryable<ContactGroup> GetQueryable();
        IList<ContactGroup> GetAll();
        ContactGroup GetObjectById(int Id);
        ContactGroup GetObjectByName(string Name);
        ContactGroup CreateObject(ContactGroup contactGroup);
        ContactGroup UpdateObject(ContactGroup contactGroup);
        ContactGroup SoftDeleteObject(ContactGroup contactGroup);
        bool DeleteObject(int Id);
    }
}