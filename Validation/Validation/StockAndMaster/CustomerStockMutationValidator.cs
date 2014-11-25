using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Validation;
using Core.DomainModel;
using Core.Interface.Service;
using Core.Constants;

namespace Validation.Validation
{
    public class CustomerStockMutationValidator : ICustomerStockMutationValidator
    {
        public CustomerStockMutation VHasItem(CustomerStockMutation customerStockMutation, IItemService _itemService)
        {
            Item item = _itemService.GetObjectById(customerStockMutation.ItemId);
            if (item == null)
            {
                customerStockMutation.Errors.Add("Generic", "Tidak terasosiasi dengan barang");
            }
            return customerStockMutation;
        }

        //public CustomerStockMutation VHasContact(CustomerStockMutation customerStockMutation, IContactService _contactService)
        //{
        //    if (customerStockMutation.ContactId != null)
        //    {
        //        Contact contact = _contactService.GetObjectById((int)customerStockMutation.ContactId);
        //        if (contact == null)
        //        {
        //            customerStockMutation.Errors.Add("Generic", "Tidak terasosiasi dengan Contact");
        //        }
        //    }
        //    return customerStockMutation;
        //}

        public CustomerStockMutation VHasCustomerItem(CustomerStockMutation customerStockMutation, ICustomerItemService _customerItemService)
        {
            if (customerStockMutation.CustomerItemId != null)
            {
                CustomerItem customerItem = _customerItemService.GetObjectById((int)customerStockMutation.CustomerItemId);
                if (customerItem == null)
                {
                    customerStockMutation.Errors.Add("Generic", "Tidak terasosiasi dengan item di stockMutation");
                }
            }
            return customerStockMutation;
        }

        public CustomerStockMutation VItemCase(CustomerStockMutation customerStockMutation)
        {
            if (!customerStockMutation.ItemCase.Equals(Constant.ItemCase.Ready) &&
                !customerStockMutation.ItemCase.Equals(Constant.ItemCase.PendingDelivery) &&
                !customerStockMutation.ItemCase.Equals(Constant.ItemCase.PendingReceival))
            {
                customerStockMutation.Errors.Add("Generic", "Harus merupakan bagian dari Constant.ItemCase");
            }
            return customerStockMutation;
        }

        public CustomerStockMutation VStatus(CustomerStockMutation customerStockMutation)
        {
            if (!customerStockMutation.Status.Equals(Constant.MutationStatus.Addition) &&
                !customerStockMutation.Status.Equals(Constant.MutationStatus.Deduction))
            {
                customerStockMutation.Errors.Add("Generic", "Harus merupakan bagian dari Constant.MutationStatus");
            }
            return customerStockMutation;
        }

        public CustomerStockMutation VSourceDocumentType(CustomerStockMutation customerStockMutation)
        {
            if (!customerStockMutation.SourceDocumentType.Equals(Constant.SourceDocumentType.PurchaseOrder) &&
                !customerStockMutation.SourceDocumentType.Equals(Constant.SourceDocumentType.PurchaseReceival) &&
                !customerStockMutation.SourceDocumentType.Equals(Constant.SourceDocumentType.SalesOrder) &&
                !customerStockMutation.SourceDocumentType.Equals(Constant.SourceDocumentType.DeliveryOrder) &&
                !customerStockMutation.SourceDocumentType.Equals(Constant.SourceDocumentType.RecoveryOrder) &&
                !customerStockMutation.SourceDocumentType.Equals(Constant.SourceDocumentType.RecoveryOrderDetail) &&
                !customerStockMutation.SourceDocumentType.Equals(Constant.SourceDocumentType.RetailSalesInvoice))
            {
                customerStockMutation.Errors.Add("Generic", "Harus merupakan bagian dari Constant.SourceDocumentType");
            }
            return customerStockMutation;
        }

        public CustomerStockMutation VSourceDocumentDetailType(CustomerStockMutation customerStockMutation)
        {
            if (!customerStockMutation.SourceDocumentDetailType.Equals(Constant.SourceDocumentDetailType.PurchaseOrderDetail) &&
                !customerStockMutation.SourceDocumentDetailType.Equals(Constant.SourceDocumentDetailType.PurchaseReceivalDetail) &&
                !customerStockMutation.SourceDocumentDetailType.Equals(Constant.SourceDocumentDetailType.SalesOrderDetail) &&
                !customerStockMutation.SourceDocumentDetailType.Equals(Constant.SourceDocumentDetailType.DeliveryOrderDetail) &&
                !customerStockMutation.SourceDocumentDetailType.Equals(Constant.SourceDocumentDetailType.RecoveryOrderDetail) &&
                !customerStockMutation.SourceDocumentDetailType.Equals(Constant.SourceDocumentDetailType.RecoveryAccessoryDetail) &&
                !customerStockMutation.SourceDocumentDetailType.Equals(Constant.SourceDocumentDetailType.RetailSalesInvoiceDetail))
            {
                customerStockMutation.Errors.Add("Generic", "Harus merupakan bagian dari Constant.SourceDocumentDetailType");
            }
            return customerStockMutation;
        }

        public CustomerStockMutation VNonNegativeNorZeroQuantity(CustomerStockMutation customerStockMutation)
        {
            if (customerStockMutation.Quantity <= 0)
            {
                customerStockMutation.Errors.Add("Generic", "Tidak boleh negatif atau 0");
            }
            return customerStockMutation;
        }

        public CustomerStockMutation VCreateObject(CustomerStockMutation customerStockMutation, ICustomerItemService _customerItemService, IItemService _itemService)
        {
            VHasItem(customerStockMutation, _itemService);
            //if (!isValid(customerStockMutation)) { return customerStockMutation; }
            //VHasContact(customerStockMutation, _contactService);
            if (!isValid(customerStockMutation)) { return customerStockMutation; }
            VHasCustomerItem(customerStockMutation, _customerItemService);
            if (!isValid(customerStockMutation)) { return customerStockMutation; }
            VItemCase(customerStockMutation);
            if (!isValid(customerStockMutation)) { return customerStockMutation; }
            VStatus(customerStockMutation);
            if (!isValid(customerStockMutation)) { return customerStockMutation; }
            VSourceDocumentType(customerStockMutation);
            if (!isValid(customerStockMutation)) { return customerStockMutation; }
            VSourceDocumentDetailType(customerStockMutation);
            if (!isValid(customerStockMutation)) { return customerStockMutation; }
            VNonNegativeNorZeroQuantity(customerStockMutation);
            return customerStockMutation;
        }

        public CustomerStockMutation VUpdateObject(CustomerStockMutation customerStockMutation, ICustomerItemService _customerItemService, IItemService _itemService)
        {
            VCreateObject(customerStockMutation, _customerItemService, _itemService);
            return customerStockMutation;
        }

        public CustomerStockMutation VDeleteObject(CustomerStockMutation customerStockMutation, ICustomerItemService _customerItemService)
        {
            return customerStockMutation;
        }

        public bool ValidCreateObject(CustomerStockMutation customerStockMutation, ICustomerItemService _customerItemService, IItemService _itemService)
        {
            VCreateObject(customerStockMutation, _customerItemService, _itemService);
            return isValid(customerStockMutation);
        }

        public bool ValidUpdateObject(CustomerStockMutation customerStockMutation, ICustomerItemService _customerItemService, IItemService _itemService)
        {
            customerStockMutation.Errors.Clear();
            VUpdateObject(customerStockMutation, _customerItemService, _itemService);
            return isValid(customerStockMutation);
        }

        public bool ValidDeleteObject(CustomerStockMutation customerStockMutation, ICustomerItemService _customerItemService)
        {
            customerStockMutation.Errors.Clear();
            VDeleteObject(customerStockMutation, _customerItemService);
            return isValid(customerStockMutation);
        }

        public bool isValid(CustomerStockMutation obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(CustomerStockMutation obj)
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
