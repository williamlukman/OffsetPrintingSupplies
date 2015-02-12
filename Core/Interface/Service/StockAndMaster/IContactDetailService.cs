using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IContactDetailService
    {
        IContactDetailValidator GetValidator();
        IQueryable<ContactDetail> GetQueryable();
        IList<ContactDetail> GetAll();
        ContactDetail GetObjectById(int Id);
        ContactDetail GetObjectByName(string Name);
        ContactDetail CreateObject(ContactDetail contactDetail, IContactService _contactService);
        ContactDetail UpdateObject(ContactDetail contactDetail, IContactService _contactService);
        ContactDetail SoftDeleteObject(ContactDetail contactDetail);
        bool DeleteObject(int Id);
        bool IsNameDuplicated(ContactDetail contactDetail);
    }
}