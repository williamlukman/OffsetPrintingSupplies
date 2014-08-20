﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Validation;

namespace Core.Interface.Service
{
    public interface IContactGroupService
    {
        IContactGroupValidator GetValidator();
        IQueryable<ContactGroup> GetQueryable();
        IList<ContactGroup> GetAll();
        ContactGroup GetObjectById(int Id);
        ContactGroup GetObjectByIsLegacy(bool IsLegacy);
        ContactGroup GetObjectByName(string Name);
        ContactGroup CreateObject(ContactGroup contactGroup);
        ContactGroup CreateObject(string Name, string Description);
        ContactGroup CreateObject(string Name, string Description, bool IsLegacy);
        ContactGroup UpdateObject(ContactGroup contactGroup);
        ContactGroup SoftDeleteObject(ContactGroup contactGroup);
        bool DeleteObject(int Id);
        bool IsNameDuplicated(ContactGroup contactGroup);
    }
}
