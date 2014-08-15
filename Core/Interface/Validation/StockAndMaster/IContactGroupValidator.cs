using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IContactGroupValidator
    {
        ContactGroup VHasUniqueName(ContactGroup contactGroup, IContactGroupService _contactGroupService);        
        ContactGroup VCreateObject(ContactGroup contactGroup, IContactGroupService _contactGroupService);
        ContactGroup VUpdateObject(ContactGroup contactGroup, IContactGroupService _contactGroupService);
        ContactGroup VDeleteObject(ContactGroup contactGroup);
        bool ValidCreateObject(ContactGroup contactGroup, IContactGroupService _contactGroupService);
        bool ValidUpdateObject(ContactGroup contactGroup, IContactGroupService _contactGroupService);
        bool ValidDeleteObject(ContactGroup contactGroup);
        bool isValid(ContactGroup contactGroup);
        string PrintError(ContactGroup contactGroup);
    }
}
