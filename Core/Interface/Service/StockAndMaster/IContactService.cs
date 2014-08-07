using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IContactService
    {
        IContactValidator GetValidator();
        IList<Contact> GetAll();
        Contact GetObjectById(int Id);
        Contact GetObjectByName(string Name);
        Contact CreateObject(Contact contact);
        Contact CreateObject(string Name, string Address, string ContactNo, string PIC, string PICContactNo, string Email);
        Contact UpdateObject(Contact contact);
        Contact SoftDeleteObject(Contact contact, ICoreIdentificationService _coreIdentificationService, IBarringService _barringService);
        bool DeleteObject(int Id);
        bool IsNameDuplicated(Contact contact);
    }
}