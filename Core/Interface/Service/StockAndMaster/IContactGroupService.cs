using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IContactGroupService
    {
        IContactGroupValidator GetValidator();
        IQueryable<ContactGroup> GetQueryable();
        IList<ContactGroup> GetAll();
        ContactGroup GetObjectById(int Id);
        ContactGroup GetObjectByName(string Name);
        ContactGroup CreateObject(ContactGroup contactGroup);
        ContactGroup UpdateObject(ContactGroup contactGroup);
        ContactGroup SoftDeleteObject(ContactGroup contactGroup, IContactService _contactService);
        bool DeleteObject(int Id);
        bool IsNameDuplicated(ContactGroup contactGroup);
    }
}