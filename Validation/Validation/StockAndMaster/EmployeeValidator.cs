using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Constants;

namespace Validation.Validation
{
    public class EmployeeValidator : IEmployeeValidator
    {
        public Employee VHasUniqueName(Employee employee, IEmployeeService _employeeService)
        {
            if (String.IsNullOrEmpty(employee.Name) || employee.Name.Trim() == "")
            {
                employee.Errors.Add("Name", "Tidak boleh kosong");
            }
            else if (_employeeService.IsNameDuplicated(employee))
            {
                employee.Errors.Add("Name", "Tidak boleh ada duplikasi");
            }
            return employee;
        }

        public Employee VHasSalesOrder(Employee employee, ISalesOrderService _salesOrderService)
        {
            IList<SalesOrder> salesOrders = _salesOrderService.GetQueryable().Where(x => x.EmployeeId == employee.Id && !x.IsDeleted).ToList();
            if (salesOrders.Any())
            {
                employee.Errors.Add("Generic", "Employee masih memiliki asosiasi dengan sales order");
            }
            return employee;
        }

        public Employee VCreateObject(Employee employee, IEmployeeService _employeeService)
        {
            VHasUniqueName(employee, _employeeService);
            return employee;
        }

        public Employee VUpdateObject(Employee employee, IEmployeeService _employeeService)
        {
            VCreateObject(employee, _employeeService);
            return employee;
        }

        public Employee VDeleteObject(Employee employee, ISalesOrderService _salesOrderService)
        {
            VHasSalesOrder(employee, _salesOrderService);
            return employee;
        }

        public bool ValidCreateObject(Employee employee, IEmployeeService _employeeService)
        {
            VCreateObject(employee, _employeeService);
            return isValid(employee);
        }

        public bool ValidUpdateObject(Employee employee, IEmployeeService _employeeService)
        {
            employee.Errors.Clear();
            VUpdateObject(employee, _employeeService);
            return isValid(employee);
        }

        public bool ValidDeleteObject(Employee employee, ISalesOrderService _salesOrderService)
        {
            employee.Errors.Clear();
            VDeleteObject(employee, _salesOrderService);
            return isValid(employee);
        }

        public bool isValid(Employee obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(Employee obj)
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
