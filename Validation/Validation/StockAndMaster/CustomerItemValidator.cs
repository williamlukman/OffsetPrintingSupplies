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
        public CustomerItem VHasWarehouseItem(CustomerItem customerItem, IWarehouseItemService _warehouseItemService)
        {
            WarehouseItem warehouseItem = _warehouseItemService.GetObjectById(customerItem.WarehouseItemId.GetValueOrDefault());
            if (warehouseItem == null)
            {
                customerItem.Errors.Add("Generic", "Tidak terasosiasi dengan WarehouseItem");
            }
            return customerItem;
        }

        public CustomerItem VHasContact(CustomerItem customerItem, IContactService _contactService)
        {
            Contact contact = _contactService.GetObjectById(customerItem.ContactId);
            if (contact == null)
            {
                customerItem.Errors.Add("Generic", "Tidak terasosiasi dengan Contact");
            }
            return customerItem;
        }

        public CustomerItem VNonNegativeQuantity(CustomerItem customerItem)
        {
            if (customerItem.Quantity < 0)
            {
                customerItem.Errors.Add("Generic", "Quantity Tidak boleh negatif");
            }
            else if (customerItem.Virtual < 0)
            {
                customerItem.Errors.Add("Generic", "Virtual quantity Tidak boleh negatif");
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

        public CustomerItem VCreateObject(CustomerItem customerItem, IContactService _contactService, IWarehouseItemService _warehouseItemService)
        {
            VHasWarehouseItem(customerItem, _warehouseItemService);
            if (!isValid(customerItem)) { return customerItem; }
            VHasContact(customerItem, _contactService);
            if (!isValid(customerItem)) { return customerItem; }
            VNonNegativeQuantity(customerItem);
            return customerItem;
        }

        public CustomerItem VUpdateObject(CustomerItem customerItem, IContactService _contactService, IWarehouseItemService _warehouseItemService)
        {
            return VCreateObject(customerItem, _contactService, _warehouseItemService);
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

        public bool ValidCreateObject(CustomerItem customerItem, IContactService _contactService, IWarehouseItemService _warehouseItemService)
        {
            VCreateObject(customerItem, _contactService, _warehouseItemService);
            return isValid(customerItem);
        }

        public bool ValidUpdateObject(CustomerItem customerItem, IContactService _contactService, IWarehouseItemService _warehouseItemService)
        {
            customerItem.Errors.Clear();
            VUpdateObject(customerItem, _contactService, _warehouseItemService);
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
