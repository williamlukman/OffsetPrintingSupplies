﻿using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Validation.Validation
{
    public class CustomerValidator : ICustomerValidator
    {

        public Customer VHasUniqueName(Customer customer, ICustomerService _customerService)
        {
            if (String.IsNullOrEmpty(customer.Name) || customer.Name.Trim() == "")
            {
                customer.Errors.Add("Name", "Tidak boleh kosong");
            }
            if (_customerService.IsNameDuplicated(customer))
            {
                customer.Errors.Add("Name", "Tidak boleh ada duplikasi");
            }
            return customer;
        }

        public Customer VHasAddress(Customer customer)
        {
            if (String.IsNullOrEmpty(customer.Address) || customer.Address.Trim() == "")
            {
                customer.Errors.Add("Address", "Tidak boleh kosong");
            }
            return customer;
        }

        public Customer VHasContactNo(Customer customer)
        {
            if (String.IsNullOrEmpty(customer.ContactNo) || customer.ContactNo.Trim() == "")
            {
                customer.Errors.Add("ContactNo", "Tidak boleh kosong");
            }
            return customer;
        }

        public Customer VHasPIC(Customer customer)
        {
            if (String.IsNullOrEmpty(customer.PIC) || customer.PIC.Trim() == "")
            {
                customer.Errors.Add("PIC", "Tidak boleh kosong");
            }
            return customer;
        }

        public Customer VHasPICContactNo(Customer customer)
        {
            if (String.IsNullOrEmpty(customer.PICContactNo) || customer.PICContactNo.Trim() == "")
            {
                customer.Errors.Add("PICContactNo", "Tidak boleh kosong");
            }
            return customer;
        }

        public Customer VHasEmail(Customer customer)
        {
            if (String.IsNullOrEmpty(customer.Email) || customer.Email.Trim() == "")
            {
                customer.Errors.Add("Email", "Tidak boleh kosong");
            }
            return customer;
        }

        public Customer VHasCoreIdentification(Customer customer, ICoreIdentificationService _coreIdentificationService)
        {
            IList<CoreIdentification> coreIdentifications = _coreIdentificationService.GetAllObjectsByCustomerId(customer.Id).ToList();
            if (coreIdentifications.Any())
            {
                customer.Errors.Add("Generic", "Customer masih memiliki Core Identification");
            }
            return customer;
        }

        public Customer VCreateObject(Customer customer, ICustomerService _customerService)
        {
            VHasUniqueName(customer, _customerService);
            if (!isValid(customer)) { return customer; }
            VHasAddress(customer);
            if (!isValid(customer)) { return customer; }
            VHasContactNo(customer);
            if (!isValid(customer)) { return customer; }
            VHasPIC(customer);
            if (!isValid(customer)) { return customer; }
            VHasPICContactNo(customer);
            if (!isValid(customer)) { return customer; }
            VHasEmail(customer);
            return customer;
        }

        public Customer VUpdateObject(Customer customer, ICustomerService _customerService)
        {
            VCreateObject(customer, _customerService);
            return customer;
        }

        public Customer VDeleteObject(Customer customer, ICoreIdentificationService _coreIdentificationService)
        {
            VHasCoreIdentification(customer, _coreIdentificationService);
            return customer;
        }

        public bool ValidCreateObject(Customer customer, ICustomerService _customerService)
        {
            VCreateObject(customer, _customerService);
            return isValid(customer);
        }

        public bool ValidUpdateObject(Customer customer, ICustomerService _customerService)
        {
            customer.Errors.Clear();
            VUpdateObject(customer, _customerService);
            return isValid(customer);
        }

        public bool ValidDeleteObject(Customer customer, ICoreIdentificationService _coreIdentificationService)
        {
            customer.Errors.Clear();
            VDeleteObject(customer, _coreIdentificationService);
            return isValid(customer);
        }

        public bool isValid(Customer obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(Customer obj)
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