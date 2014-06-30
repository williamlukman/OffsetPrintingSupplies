using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface ICustomerValidator
    {
        Customer VUniqueName(Customer customer, ICustomerService _customerService);
        Customer VHasAddress(Customer customer);
        Customer VHasContactNo(Customer customer);
        Customer VHasPIC(Customer customer);
        Customer VHasPICContactNo(Customer customer);
        Customer VHasEmail(Customer customer);
        Customer VCreateObject(Customer customer, ICustomerService _customerService);
        Customer VUpdateObject(Customer customer, ICustomerService _customerService);
        Customer VDeleteObject(Customer customer, IRecoveryOrderService _recoveryOrderService);
        bool ValidCreateObject(Customer customer, ICustomerService _customerService);
        bool ValidUpdateObject(Customer customer, ICustomerService _customerService);
        bool ValidDeleteObject(Customer customer, IRecoveryOrderService _recoveryOrderService);
        bool isValid(Customer customer);
        string PrintError(Customer customer);
    }
}