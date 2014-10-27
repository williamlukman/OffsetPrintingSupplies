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
        public Contact VHasContactGroup(Contact contact, IContactGroupService _contactGroupService)
        {
            ContactGroup contactGroup = _contactGroupService.GetObjectById(contact.ContactGroupId);
            if (contactGroup == null)
            {
                contact.Errors.Add("ContactGroupId", "Tidak memiliki asosiasi dengan Contact Group");
            }
            return contact;
        }

        public Contact VHasUniqueName(Contact contact, IContactService _contactService)
        {
            if (String.IsNullOrEmpty(contact.Name) || contact.Name.Trim() == "")
            {
                contact.Errors.Add("Name", "Tidak boleh kosong");
            }
            else if (_contactService.IsNameDuplicated(contact))
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

        public Contact VHasBlanket(Contact contact, IBlanketService _blanketService)
        {
            IList<Blanket> blankets = _blanketService.GetObjectsByContactId(contact.Id);
            if (blankets.Any())
            {
                contact.Errors.Add("Generic", "Contact masih memiliki asosiasi dengan Blanket");
            }
            return contact;
        }

        public Contact VHasPurchaseOrder(Contact contact, IPurchaseOrderService _purchaseOrderService)
        {
            IList<PurchaseOrder> purchaseOrders = _purchaseOrderService.GetObjectsByContactId(contact.Id);
            if (purchaseOrders.Any())
            {
                contact.Errors.Add("Generic", "Contact masih memiliki asosiasi dengan purchase order");
            }
            return contact;
        }

        public Contact VHasSalesOrder(Contact contact, ISalesOrderService _salesOrderService)
        {
            IList<SalesOrder> salesOrders = _salesOrderService.GetObjectsByContactId(contact.Id);
            if (salesOrders.Any())
            {
                contact.Errors.Add("Generic", "Contact masih memiliki asosiasi dengan sales order");
            }
            return contact;
        }


        public Contact VHasSalesQuotation(Contact contact, ISalesQuotationService _salesQuotationService)
        {
            IList<SalesQuotation> salesQuotations = _salesQuotationService.GetObjectsByContactId(contact.Id);
            if (salesQuotations.Any())
            {
                contact.Errors.Add("Generic", "Contact masih memiliki asosiasi dengan sales quotation");
            }
            return contact;
        }

        public Contact VHasVirtualOrder(Contact contact, IVirtualOrderService _virtualOrderService)
        {
            IList<VirtualOrder> virtualOrders = _virtualOrderService.GetObjectsByContactId(contact.Id);
            if (virtualOrders.Any())
            {
                contact.Errors.Add("Generic", "Contact masih memiliki asosiasi dengan virtual order");
            }
            return contact;
        }

        public Contact VCreateObject(Contact contact, IContactService _contactService, IContactGroupService _contactGroupService)
        {
            VHasContactGroup(contact, _contactGroupService);
            if (!isValid(contact)) { return contact; }
            VHasUniqueName(contact, _contactService);
            if (!isValid(contact)) { return contact; }
            VHasAddress(contact);
            if (!isValid(contact)) { return contact; }
            VHasContactNo(contact);
            if (!isValid(contact)) { return contact; }
            //VHasPIC(contact);
            //if (!isValid(contact)) { return contact; }
            //VHasPICContactNo(contact);
            //if (!isValid(contact)) { return contact; }
            //VHasEmail(contact);
            return contact;
        }

        public Contact VUpdateObject(Contact contact, IContactService _contactService, IContactGroupService _contactGroupService)
        {
            VCreateObject(contact, _contactService, _contactGroupService);
            return contact;
        }

        public Contact VDeleteObject(Contact contact, ICoreIdentificationService _coreIdentificationService, IBlanketService _blanketService,
                                     IPurchaseOrderService _purchaseOrderService, ISalesOrderService _salesOrderService,
                                     ISalesQuotationService _salesQuotationService, IVirtualOrderService _virtualOrderService)
        {
            VHasCoreIdentification(contact, _coreIdentificationService);
            if (!isValid(contact)) { return contact; }
            VHasBlanket(contact, _blanketService);
            if (!isValid(contact)) { return contact; }
            VHasSalesOrder(contact, _salesOrderService);
            if (!isValid(contact)) { return contact; }
            VHasSalesQuotation(contact, _salesQuotationService);
            if (!isValid(contact)) { return contact; }
            VHasPurchaseOrder(contact, _purchaseOrderService);
            if (!isValid(contact)) { return contact; }
            VHasVirtualOrder(contact, _virtualOrderService);
            return contact;
        }

        public bool ValidCreateObject(Contact contact, IContactService _contactService, IContactGroupService _contactGroupService)
        {
            VCreateObject(contact, _contactService, _contactGroupService);
            return isValid(contact);
        }

        public bool ValidUpdateObject(Contact contact, IContactService _contactService, IContactGroupService _contactGroupService)
        {
            contact.Errors.Clear();
            VUpdateObject(contact, _contactService, _contactGroupService);
            return isValid(contact);
        }

        public bool ValidDeleteObject(Contact contact, ICoreIdentificationService _coreIdentificationService, IBlanketService _blanketService,
                                      IPurchaseOrderService _purchaseOrderService, ISalesOrderService _salesOrderService,
                                      ISalesQuotationService _salesQuotationService, IVirtualOrderService _virtualOrderService)
        {
            contact.Errors.Clear();
            VDeleteObject(contact, _coreIdentificationService, _blanketService, _purchaseOrderService, _salesOrderService,
                          _salesQuotationService, _virtualOrderService);
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
