using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;

namespace Core.Interface.Repository
{
    public interface IContactGroupRepository : IRepository<ContactGroup>
    {
        IList<ContactGroup> GetAll();
        ContactGroup GetObjectById(int Id);
        ContactGroup GetObjectByIsLegacy(bool IsLegacy);
        ContactGroup GetObjectByName(string Name);
        ContactGroup CreateObject(ContactGroup contactGroup);
        ContactGroup UpdateObject(ContactGroup contactGroup);
        ContactGroup SoftDeleteObject(ContactGroup contactGroup);
        bool DeleteObject(int Id);
    }
}
