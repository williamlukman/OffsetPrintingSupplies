using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class ContactValidator : IContactValidator
    {
        public Contact VHasUniqueName(Contact contact, IContactService _contactService)
        {
            if (String.IsNullOrEmpty(contact.Name) || contact.Name.Trim() == "")
            {
                contact.Errors.Add("Name", "Tidak boleh kosong");
            }
            if (_contactService.IsNameDuplicated(contact))
            {
                contact.Errors.Add("Name", "Tidak boleh ada duplikasi");
            }
            return contact;
        }

        public Contact VHasAddress(Contact contact)
        {
            if (String.IsNullOrEmpty(contact.Address) || contact.Address.Trim() == "")
            {
                contact.Errors.Add("Address", "Tidak boleh kosong");
            }
            return contact;
        }

        public Contact VHasContactNo(Contact contact)
        {
            if (String.IsNullOrEmpty(contact.ContactNo) || contact.ContactNo.Trim() == "")
            {
                contact.Errors.Add("ContactNo", "Tidak boleh kosong");
            }
            return contact;
        }

        public Contact VHasPIC(Contact contact)
        {
            if (String.IsNullOrEmpty(contact.PIC) || contact.PIC.Trim() == "")
            {
                contact.Errors.Add("PIC", "Tidak boleh kosong");
            }
            return contact;
        }

        public Contact VHasPICContactNo(Contact contact)
        {
            if (String.IsNullOrEmpty(contact.PICContactNo) || contact.PICContactNo.Trim() == "")
            {
                contact.Errors.Add("PICContactNo", "Tidak boleh kosong");
            }
            return contact;
        }

        public Contact VHasEmail(Contact contact)
        {
            if (String.IsNullOrEmpty(contact.Email) || contact.Email.Trim() == "")
            {
                contact.Errors.Add("Email", "Tidak boleh kosong");
            }
            return contact;
        }

        public Contact VHasCoreIdentification(Contact contact, ICoreIdentificationService _coreIdentificationService)
        {
            IList<CoreIdentification> coreIdentifications = _coreIdentificationService.GetAllObjectsByContactId(contact.Id).ToList();
            if (coreIdentifications.Any())
            {
                contact.Errors.Add("Generic", "Contact masih memiliki Core Identification");
            }
            return contact;
        }

        public Contact VHasBarring(Contact contact, IBarringService _barringService)
        {
            IList<Barring> barrings = _barringService.GetObjectsByContactId(contact.Id);
            if (barrings.Any())
            {
                contact.Errors.Add("Generic", "Contact masih memiliki asosiasi dengan Barring");
            }
            return contact;
        }

        public Contact VCreateObject(Contact contact, IContactService _contactService)
        {
            VHasUniqueName(contact, _contactService);
            if (!isValid(contact)) { return contact; }
            VHasAddress(contact);
            if (!isValid(contact)) { return contact; }
            VHasContactNo(contact);
            if (!isValid(contact)) { return contact; }
            VHasPIC(contact);
            if (!isValid(contact)) { return contact; }
            VHasPICContactNo(contact);
            if (!isValid(contact)) { return contact; }
            VHasEmail(contact);
            return contact;
        }

        public Contact VUpdateObject(Contact contact, IContactService _contactService)
        {
            VCreateObject(contact, _contactService);
            return contact;
        }

        public Contact VDeleteObject(Contact contact, ICoreIdentificationService _coreIdentificationService, IBarringService _barringService)
        {
            VHasCoreIdentification(contact, _coreIdentificationService);
            if (!isValid(contact)) { return contact; }
            VHasBarring(contact, _barringService);
            return contact;
        }

        public bool ValidCreateObject(Contact contact, IContactService _contactService)
        {
            VCreateObject(contact, _contactService);
            return isValid(contact);
        }

        public bool ValidUpdateObject(Contact contact, IContactService _contactService)
        {
            contact.Errors.Clear();
            VUpdateObject(contact, _contactService);
            return isValid(contact);
        }

        public bool ValidDeleteObject(Contact contact, ICoreIdentificationService _coreIdentificationService, IBarringService _barringService)
        {
            contact.Errors.Clear();
            VDeleteObject(contact, _coreIdentificationService, _barringService);
            return isValid(contact);
        }

        public bool isValid(Contact obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(Contact obj)
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
