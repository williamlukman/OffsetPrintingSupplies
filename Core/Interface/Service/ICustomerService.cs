using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Service
{
    public interface ICustomerService
    {
        ICustomerValidator GetValidator();
        IList<Customer> GetAll();
        Customer GetObjectById(int Id);
        Customer GetObjectByName(string Name);
        Customer CreateObject(Customer customer);
        Customer CreateObject(string Name, string Address, string ContactNo, string PIC, string PICContactNo, string Email);
        Customer UpdateObject(Customer customer);
        Customer SoftDeleteObject(Customer customer, ICoreIdentificationService _coreIdentificationService, IBarringService _barringService);
        bool DeleteObject(int Id);
        bool IsNameDuplicated(Customer customer);
    }
}