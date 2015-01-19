using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IContactDetailRepository : IRepository<ContactDetail>
    {
        IQueryable<ContactDetail> GetQueryable();
        IList<ContactDetail> GetAll();
        ContactDetail GetObjectById(int Id);
        ContactDetail GetObjectByName(string Name);
        ContactDetail CreateObject(ContactDetail contactDetail);
        ContactDetail UpdateObject(ContactDetail contactDetail);
        ContactDetail SoftDeleteObject(ContactDetail contactDetail);
        bool DeleteObject(int Id);
    }
}