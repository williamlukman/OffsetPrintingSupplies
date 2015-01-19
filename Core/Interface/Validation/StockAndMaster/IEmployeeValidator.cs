using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface IEmployeeValidator
    {
        Employee VHasUniqueName(Employee employee, IEmployeeService _employeeService);
        Employee VHasSalesOrder(Employee employee, ISalesOrderService _salesOrderService);
        Employee VCreateObject(Employee employee, IEmployeeService _employeeService);
        Employee VUpdateObject(Employee employee, IEmployeeService _employeeService);
        Employee VDeleteObject(Employee employee, ISalesOrderService _salesOrderService);
        bool ValidCreateObject(Employee employee, IEmployeeService _employeeService);
        bool ValidUpdateObject(Employee employee, IEmployeeService _employeeService);
        bool ValidDeleteObject(Employee employee, ISalesOrderService _salesOrderService);
        bool isValid(Employee employee);
        string PrintError(Employee employee);
    }
}