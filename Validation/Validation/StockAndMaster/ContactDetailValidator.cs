using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Constants;

namespace Validation.Validation
{
    public class ContactDetailValidator : IContactDetailValidator
    {
        public ContactDetail VHasUniqueName(ContactDetail contactDetail, IContactDetailService _contactDetailService)
        {
            if (String.IsNullOrEmpty(contactDetail.Name) || contactDetail.Name.Trim() == "")
            {
                contactDetail.Errors.Add("Name", "Tidak boleh kosong");
            }
            else if (_contactDetailService.IsNameDuplicated(contactDetail))
            {
                contactDetail.Errors.Add("Name", "Tidak boleh ada duplikasi");
            }
            return contactDetail;
        }

        public ContactDetail VHasContact(ContactDetail contactDetail, IContactService _contactService)
        {
            Contact contact = _contactService.GetObjectById(contactDetail.ContactId);
            if (contact == null)
            {
                contactDetail.Errors.Add("Generic", "ContactDetail harus terasosiasi dengan contact");
            }
            return contactDetail;
        }

        public ContactDetail VCreateObject(ContactDetail contactDetail, IContactDetailService _contactDetailService, IContactService _contactService)
        {
            VHasUniqueName(contactDetail, _contactDetailService);
            if (!isValid(contactDetail)) { return contactDetail; }
            VHasContact(contactDetail, _contactService);
            return contactDetail;
        }

        public ContactDetail VUpdateObject(ContactDetail contactDetail, IContactDetailService _contactDetailService, IContactService _contactService)
        {
            VCreateObject(contactDetail, _contactDetailService, _contactService);
            return contactDetail;
        }

        public ContactDetail VDeleteObject(ContactDetail contactDetail)
        {
            return contactDetail;
        }

        public bool ValidCreateObject(ContactDetail contactDetail, IContactDetailService _contactDetailService, IContactService _contactService)
        {
            VCreateObject(contactDetail, _contactDetailService, _contactService);
            return isValid(contactDetail);
        }

        public bool ValidUpdateObject(ContactDetail contactDetail, IContactDetailService _contactDetailService, IContactService _contactService)
        {
            contactDetail.Errors.Clear();
            VUpdateObject(contactDetail, _contactDetailService, _contactService);
            return isValid(contactDetail);
        }

        public bool ValidDeleteObject(ContactDetail contactDetail)
        {
            contactDetail.Errors.Clear();
            VDeleteObject(contactDetail);
            return isValid(contactDetail);
        }

        public bool isValid(ContactDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(ContactDetail obj)
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
