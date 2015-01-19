using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class ContactGroupValidator : IContactGroupValidator
    {
        public ContactGroup VHasUniqueName(ContactGroup contactGroup, IContactGroupService _contactGroupService)
        {
            if (String.IsNullOrEmpty(contactGroup.Name) || contactGroup.Name.Trim() == "")
            {
                contactGroup.Errors.Add("Name", "Tidak boleh kosong");
            }
            if (_contactGroupService.IsNameDuplicated(contactGroup))
            {
                contactGroup.Errors.Add("Name", "Tidak boleh diduplikasi");
            }
            return contactGroup;
        }

        public ContactGroup VHasContact(ContactGroup contactGroup, IContactService _contactService)
        {
            IList<Contact> contacts = _contactService.GetQueryable().Where(x => x.ContactGroupId == contactGroup.Id && !x.IsDeleted).ToList();
            if (!contacts.Any())
            {
                contactGroup.Errors.Add("Generic", "Masih terasosiasi dengan Contact");
            }
            return contactGroup;
        }

        public ContactGroup VCreateObject(ContactGroup contactGroup, IContactGroupService _contactGroupService)
        {
            VHasUniqueName(contactGroup, _contactGroupService);
            return contactGroup;
        }

        public ContactGroup VUpdateObject(ContactGroup contactGroup, IContactGroupService _contactGroupService)
        {
            VHasUniqueName(contactGroup, _contactGroupService);
            return contactGroup;
        }

        public ContactGroup VDeleteObject(ContactGroup contactGroup, IContactService _contactService)
        {
            VHasContact(contactGroup, _contactService);
            return contactGroup;
        }

        public bool ValidCreateObject(ContactGroup contactGroup, IContactGroupService _contactGroupService)
        {
            VCreateObject(contactGroup, _contactGroupService);
            return isValid(contactGroup);
        }

        public bool ValidUpdateObject(ContactGroup contactGroup, IContactGroupService _contactGroupService)
        {
            contactGroup.Errors.Clear();
            VUpdateObject(contactGroup, _contactGroupService);
            return isValid(contactGroup);
        }

        public bool ValidDeleteObject(ContactGroup contactGroup, IContactService _contactService)
        {
            contactGroup.Errors.Clear();
            VDeleteObject(contactGroup, _contactService);
            return isValid(contactGroup);
        }

        public bool isValid(ContactGroup obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(ContactGroup obj)
        {
            string erroroutput = "";
            KeyValuePair<string, string> first = obj.Errors.ElementAt(0);
            erroroutput += first.Key + "," + first.Value;
            foreach (KeyValuePair<string, string> pair in obj.Errors.Skip(1))
            {
                erroroutput += Environment.NewLine;
                erroroutput += pair.Key + "," + pair.Value;
            }
            return erroroutput;
        }

    }
}
