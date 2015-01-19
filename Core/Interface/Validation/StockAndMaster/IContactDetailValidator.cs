using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface IContactDetailValidator
    {
        ContactDetail VHasUniqueName(ContactDetail contactDetail, IContactDetailService _contactDetailService);
        ContactDetail VHasContact(ContactDetail contactDetail, IContactService _contactService);
        ContactDetail VCreateObject(ContactDetail contactDetail, IContactDetailService _contactDetailService, IContactService _contactService);
        ContactDetail VUpdateObject(ContactDetail contactDetail, IContactDetailService _contactDetailService, IContactService _contactService);
        ContactDetail VDeleteObject(ContactDetail contactDetail);
        bool ValidCreateObject(ContactDetail contactDetail, IContactDetailService _contactDetailService, IContactService _contactService);
        bool ValidUpdateObject(ContactDetail contactDetail, IContactDetailService _contactDetailService, IContactService _contactService);
        bool ValidDeleteObject(ContactDetail contactDetail);
        bool isValid(ContactDetail contactDetail);
        string PrintError(ContactDetail contactDetail);
    }
}