using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class CustomerItemValidator : ICustomerItemValidator
    {
        public CustomerItem VHasItem(CustomerItem customerItem, IItemService _itemService)
        {
            Item item = _itemService.GetObjectById(customerItem.ItemId);
            if (item == null)
            {
                customerItem.Errors.Add("ItemId", "Tidak terasosiasi dengan item");
            }
            return customerItem;
        }

        public CustomerItem VHasContact(CustomerItem customerItem, IContactService _contactService)
        {
            Contact contact = _contactService.GetObjectById(customerItem.ContactId);
            if (contact == null)
            {
                customerItem.Errors.Add("ContactId", "Tidak terasosiasi dengan contact");
            }
            return customerItem;
        }

        public CustomerItem VNonNegativeQuantity(CustomerItem customerItem)
        {
            if (customerItem.Quantity < 0)
            {
                customerItem.Errors.Add("Quantity", "Tidak boleh negatif");
            }
            //else if (customerItem.PendingDelivery < 0)
            //{
            //    customerItem.Errors.Add("Generic", "Pending delivery tidak boleh negatif");
            //}
            //else if (customerItem.PendingReceival < 0)
            //{
            //    customerItem.Errors.Add("Generic", "Pending receival tidak boleh negatif");
            //}
            return customerItem;
        }

        public CustomerItem VQuantityMustBeZero(CustomerItem customerItem)
        {
            if (customerItem.Quantity != 0)
            {
                customerItem.Errors.Add("Generic", "Quantity di setiap contact harus 0");
            }
            return customerItem;
        }

        public CustomerItem VCreateObject(CustomerItem customerItem, IContactService _contactService, IItemService _itemService)
        {
            // Item Validation
            VHasItem(customerItem, _itemService);
            if (!isValid(customerItem)) { return customerItem; }
            VHasContact(customerItem, _contactService);
            if (!isValid(customerItem)) { return customerItem; }
            VNonNegativeQuantity(customerItem);
            return customerItem;
        }

        public CustomerItem VUpdateObject(CustomerItem customerItem, IContactService _contactService, IItemService _itemService)
        {
            return VCreateObject(customerItem, _contactService, _itemService);
        }

        public CustomerItem VDeleteObject(CustomerItem customerItem)
        {
            VQuantityMustBeZero(customerItem);
            return customerItem;
        }

        public CustomerItem VAdjustQuantity(CustomerItem customerItem)
        {
            VNonNegativeQuantity(customerItem);
            return customerItem;
        }

        //public CustomerItem VAdjustPendingReceival(CustomerItem customerItem)
        //{
        //    VNonNegativeQuantity(customerItem);
        //    return customerItem;
        //}

        //public CustomerItem VAdjustPendingDelivery(CustomerItem customerItem)
        //{
        //    VNonNegativeQuantity(customerItem);
        //    return customerItem;
        //}

        public bool ValidCreateObject(CustomerItem customerItem, IContactService _contactService, IItemService _itemService)
        {
            VCreateObject(customerItem, _contactService, _itemService);
            return isValid(customerItem);
        }

        public bool ValidUpdateObject(CustomerItem customerItem, IContactService _contactService, IItemService _itemService)
        {
            customerItem.Errors.Clear();
            VUpdateObject(customerItem, _contactService, _itemService);
            return isValid(customerItem);
        }

        public bool ValidDeleteObject(CustomerItem customerItem)
        {
            customerItem.Errors.Clear();
            VDeleteObject(customerItem);
            return isValid(customerItem);
        }

        public bool ValidAdjustQuantity(CustomerItem customerItem)
        {
            customerItem.Errors.Clear();
            VAdjustQuantity(customerItem);
            return isValid(customerItem);
        }

        //public bool ValidAdjustPendingDelivery(CustomerItem customerItem)
        //{
        //    customerItem.Errors.Clear();
        //    VAdjustPendingDelivery(customerItem);
        //    return isValid(customerItem);
        //}

        //public bool ValidAdjustPendingReceival(CustomerItem customerItem)
        //{
        //    customerItem.Errors.Clear();
        //    VAdjustPendingReceival(customerItem);
        //    return isValid(customerItem);
        //}

        public bool isValid(CustomerItem obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(CustomerItem obj)
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
