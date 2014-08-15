using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;
using Core.Interface.Validation;

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
            else if (_contactGroupService.IsNameDuplicated(contactGroup))
            {
                contactGroup.Errors.Add("Name", "Tidak boleh ada duplikasi");
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
            VCreateObject(contactGroup, _contactGroupService);
            return contactGroup;
        }

        public ContactGroup VDeleteObject(ContactGroup contactGroup)
        {
            
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

        public bool ValidDeleteObject(ContactGroup contactGroup)
        {
            contactGroup.Errors.Clear();
            VDeleteObject(contactGroup);
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
