using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface IContactGroupValidator
    {
        ContactGroup VHasUniqueName(ContactGroup contactGroup, IContactGroupService _contactGroupService);
        ContactGroup VHasContact(ContactGroup contactGroup, IContactService _contactService);
        ContactGroup VCreateObject(ContactGroup contactGroup, IContactGroupService _contactGroupService);
        ContactGroup VUpdateObject(ContactGroup contactGroup, IContactGroupService _contactGroupService);
        ContactGroup VDeleteObject(ContactGroup contactGroup, IContactService _contactService);
        bool ValidCreateObject(ContactGroup contactGroup, IContactGroupService _contactGroupService);
        bool ValidUpdateObject(ContactGroup contactGroup, IContactGroupService _contactGroupService);
        bool ValidDeleteObject(ContactGroup contactGroup, IContactService _contactService);
        bool isValid(ContactGroup contactGroup);
        string PrintError(ContactGroup contactGroup);
    }
}