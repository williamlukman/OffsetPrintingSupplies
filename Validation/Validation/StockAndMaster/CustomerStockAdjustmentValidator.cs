using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Validation;
using Core.DomainModel;
using Core.Interface.Service;

namespace Validation.Validation
{
    public class CustomerStockAdjustmentValidator : ICustomerStockAdjustmentValidator
    {

        public CustomerStockAdjustment VHasWarehouse(CustomerStockAdjustment customerStockAdjustment, IWarehouseService _warehouseService)
        {
            Warehouse warehouse = _warehouseService.GetObjectById(customerStockAdjustment.WarehouseId);
            if (warehouse == null)
            {
                customerStockAdjustment.Errors.Add("WarehouseId", "Tidak terasosiasi dengan warehouse");
            }
            return customerStockAdjustment;
        }

        public CustomerStockAdjustment VHasContact(CustomerStockAdjustment customerStockAdjustment, IContactService _contactService)
        {
            Contact contact = _contactService.GetObjectById(customerStockAdjustment.ContactId);
            if (contact == null)
            {
                customerStockAdjustment.Errors.Add("ContactId", "Tidak terasosiasi dengan contact");
            }
            return customerStockAdjustment;
        }

        public CustomerStockAdjustment VAdjustmentDate(CustomerStockAdjustment customerStockAdjustment)
        {

            if (customerStockAdjustment.AdjustmentDate == null)
            {
                customerStockAdjustment.Errors.Add("AdjustmentDate", "Tidak boleh tidak ada");
            }

            return customerStockAdjustment;
        }

        public CustomerStockAdjustment VHasNotBeenConfirmed(CustomerStockAdjustment customerStockAdjustment)
        {
            if (customerStockAdjustment.IsConfirmed)
            {
                customerStockAdjustment.Errors.Add("Generic", "Tidak boleh sudah dikonfirmasi");
            }
            return customerStockAdjustment;
        }

        public CustomerStockAdjustment VHasBeenConfirmed(CustomerStockAdjustment customerStockAdjustment)
        {
            if (!customerStockAdjustment.IsConfirmed)
            {
                customerStockAdjustment.Errors.Add("Generic", "Harus sudah dikonfirmasi");
            }
            return customerStockAdjustment;
        }

        public CustomerStockAdjustment VHasCustomerStockAdjustmentDetails(CustomerStockAdjustment customerStockAdjustment, ICustomerStockAdjustmentDetailService _customerStockAdjustmentDetailService)
        {
            IList<CustomerStockAdjustmentDetail> details = _customerStockAdjustmentDetailService.GetObjectsByCustomerStockAdjustmentId(customerStockAdjustment.Id);
            if (!details.Any())
            {
                customerStockAdjustment.Errors.Add("Generic", "Details tidak boleh tidak ada");
            }
            return customerStockAdjustment;
        }

        public CustomerStockAdjustment VDetailsAreVerifiedConfirmable(CustomerStockAdjustment customerStockAdjustment, ICustomerStockAdjustmentService _customerStockAdjustmentService, ICustomerStockAdjustmentDetailService _customerStockAdjustmentDetailService,
                                                              IItemService _itemService, ICustomerItemService _customerItemService, IWarehouseItemService _warehouseItemService)
        {
            IList<CustomerStockAdjustmentDetail> details = _customerStockAdjustmentDetailService.GetObjectsByCustomerStockAdjustmentId(customerStockAdjustment.Id);
            foreach (var detail in details)
            {
                detail.Errors = new Dictionary<string, string>();
                detail.ConfirmationDate = customerStockAdjustment.ConfirmationDate;
                _customerStockAdjustmentDetailService.GetValidator().ValidConfirmObject(detail, _customerStockAdjustmentService, _itemService, _customerItemService, _warehouseItemService);
                if (detail.Errors.Any())
                {
                    var error = detail.Errors.FirstOrDefault();
                    customerStockAdjustment.Errors.Add("Generic", error.Key + " " + error.Value);
                    return customerStockAdjustment;
                }
            }
            return customerStockAdjustment;
        }

        public CustomerStockAdjustment VGeneralLedgerPostingHasNotBeenClosed(CustomerStockAdjustment customerStockAdjustment, IClosingService _closingService)
        {
            if (_closingService.IsDateClosed(customerStockAdjustment.AdjustmentDate))
            {
                customerStockAdjustment.Errors.Add("Generic", "Ledger sudah tutup buku");
            }
            return customerStockAdjustment;
        }

        public CustomerStockAdjustment VCreateObject(CustomerStockAdjustment customerStockAdjustment, IWarehouseService _warehouseService, IContactService _contactService)
        {
            VAdjustmentDate(customerStockAdjustment);
            if (!isValid(customerStockAdjustment)) { return customerStockAdjustment; }
            VHasWarehouse(customerStockAdjustment, _warehouseService);
            if (!isValid(customerStockAdjustment)) { return customerStockAdjustment; }
            VHasContact(customerStockAdjustment, _contactService);
            return customerStockAdjustment;
        }

        public CustomerStockAdjustment VUpdateObject(CustomerStockAdjustment customerStockAdjustment, IWarehouseService _warehouseService, IContactService _contactService)
        {
            VHasNotBeenConfirmed(customerStockAdjustment);
            if (!isValid(customerStockAdjustment)) { return customerStockAdjustment; }
            VCreateObject(customerStockAdjustment, _warehouseService, _contactService);
            return customerStockAdjustment;
        }

        public CustomerStockAdjustment VDeleteObject(CustomerStockAdjustment customerStockAdjustment)
        {
            VHasNotBeenConfirmed(customerStockAdjustment);
            return customerStockAdjustment;
        }

        public CustomerStockAdjustment VHasConfirmationDate(CustomerStockAdjustment obj)
        {
            if (obj.ConfirmationDate == null)
            {
                obj.Errors.Add("ConfirmationDate", "Tidak boleh kosong");
            }
            return obj;
        }

        public CustomerStockAdjustment VConfirmObject(CustomerStockAdjustment customerStockAdjustment, ICustomerStockAdjustmentService _customerStockAdjustmentService, ICustomerStockAdjustmentDetailService _customerStockAdjustmentDetailService,
                                              IItemService _itemService, ICustomerItemService _customerItemService, IWarehouseItemService _warehouseItemService, IClosingService _closingService)
        {
            VHasConfirmationDate(customerStockAdjustment);
            if (!isValid(customerStockAdjustment)) { return customerStockAdjustment; }
            VHasNotBeenConfirmed(customerStockAdjustment);
            if (!isValid(customerStockAdjustment)) { return customerStockAdjustment; }
            VHasCustomerStockAdjustmentDetails(customerStockAdjustment, _customerStockAdjustmentDetailService);
            if (!isValid(customerStockAdjustment)) { return customerStockAdjustment; }
            VGeneralLedgerPostingHasNotBeenClosed(customerStockAdjustment, _closingService);
            if (!isValid(customerStockAdjustment)) { return customerStockAdjustment; }
            VDetailsAreVerifiedConfirmable(customerStockAdjustment, _customerStockAdjustmentService, _customerStockAdjustmentDetailService, _itemService, _customerItemService, _warehouseItemService);
            return customerStockAdjustment;
        }

        public CustomerStockAdjustment VUnconfirmObject(CustomerStockAdjustment customerStockAdjustment, ICustomerStockAdjustmentService _customerStockAdjustmentService, ICustomerStockAdjustmentDetailService _customerStockAdjustmentDetailService,
                                                IItemService _itemService, ICustomerItemService _customerItemService, IWarehouseItemService _warehouseItemService, IClosingService _closingService)
        {
            VHasBeenConfirmed(customerStockAdjustment);
            if (!isValid(customerStockAdjustment)) { return customerStockAdjustment; }
            VGeneralLedgerPostingHasNotBeenClosed(customerStockAdjustment, _closingService);
            return customerStockAdjustment;
        }

        public bool ValidCreateObject(CustomerStockAdjustment customerStockAdjustment, IWarehouseService _warehouseService, IContactService _contactService)
        {
            VCreateObject(customerStockAdjustment, _warehouseService, _contactService);
            return isValid(customerStockAdjustment);
        }

        public bool ValidUpdateObject(CustomerStockAdjustment customerStockAdjustment, IWarehouseService _warehouseService, IContactService _contactService)
        {
            customerStockAdjustment.Errors.Clear();
            VUpdateObject(customerStockAdjustment, _warehouseService, _contactService);
            return isValid(customerStockAdjustment);
        }

        public bool ValidDeleteObject(CustomerStockAdjustment customerStockAdjustment)
        {
            customerStockAdjustment.Errors.Clear();
            VDeleteObject(customerStockAdjustment);
            return isValid(customerStockAdjustment);
        }

        public bool ValidConfirmObject(CustomerStockAdjustment customerStockAdjustment, ICustomerStockAdjustmentService _customerStockAdjustmentService, ICustomerStockAdjustmentDetailService _customerStockAdjustmentDetailService,
                                       IItemService _itemService, ICustomerItemService _customerItemService, IWarehouseItemService _warehouseItemService, IClosingService _closingService)
        {
            customerStockAdjustment.Errors.Clear();
            VConfirmObject(customerStockAdjustment, _customerStockAdjustmentService, _customerStockAdjustmentDetailService, _itemService, _customerItemService, _warehouseItemService, _closingService);
            return isValid(customerStockAdjustment);
        }

        public bool ValidUnconfirmObject(CustomerStockAdjustment customerStockAdjustment, ICustomerStockAdjustmentService _customerStockAdjustmentService, ICustomerStockAdjustmentDetailService _customerStockAdjustmentDetailService,
                                         IItemService _itemService, ICustomerItemService _customerItemService, IWarehouseItemService _warehouseItemService, IClosingService _closingService)
        {
            customerStockAdjustment.Errors.Clear();
            VUnconfirmObject(customerStockAdjustment, _customerStockAdjustmentService, _customerStockAdjustmentDetailService, _itemService, _customerItemService, _warehouseItemService, _closingService);
            return isValid(customerStockAdjustment);
        }

        public bool isValid(CustomerStockAdjustment obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(CustomerStockAdjustment obj)
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