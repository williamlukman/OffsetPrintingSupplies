using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface ICustomerValidator
    {
        Customer VHasUniqueName(Customer customer, ICustomerService _customerService);
        Customer VHasAddress(Customer customer);
        Customer VHasCustomerNo(Customer customer);
        Customer VHasPIC(Customer customer);
        Customer VHasPICCustomerNo(Customer customer);
        Customer VHasEmail(Customer customer);
        Customer VHasCoreIdentification(Customer customer, ICoreIdentificationService _coreIdentificationService);
        Customer VHasBarring(Customer customer, IBarringService _barringService);
        Customer VCreateObject(Customer customer, ICustomerService _customerService);
        Customer VUpdateObject(Customer customer, ICustomerService _customerService);
        Customer VDeleteObject(Customer customer, ICoreIdentificationService _coreIdentificationService, IBarringService _barringService);
        bool ValidCreateObject(Customer customer, ICustomerService _customerService);
        bool ValidUpdateObject(Customer customer, ICustomerService _customerService);
        bool ValidDeleteObject(Customer customer, ICoreIdentificationService _coreIdentificationService, IBarringService _barringService);
        bool isValid(Customer customer);
        string PrintError(Customer customer);
    }
}