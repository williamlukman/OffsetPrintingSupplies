﻿using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Service
{
    public class EmployeeService : IEmployeeService
    {
        private IEmployeeRepository _repository;
        private IEmployeeValidator _validator;
        public EmployeeService(IEmployeeRepository _employeeRepository, IEmployeeValidator _employeeValidator)
        {
            _repository = _employeeRepository;
            _validator = _employeeValidator;
        }

        public IEmployeeValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<Employee> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<Employee> GetAll()
        {
            return _repository.GetAll();
        }

        public Employee GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public Employee GetObjectByName(string name)
        {
            return _repository.GetObjectByName(name);
        }

        public Employee CreateObject(Employee employee)
        {
            employee.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(employee, this) ? _repository.CreateObject(employee) : employee);
        }

        public Employee UpdateObject(Employee employee)
        {
            return (employee = _validator.ValidUpdateObject(employee, this) ? _repository.UpdateObject(employee) : employee);
        }

        public Employee SoftDeleteObject(Employee employee, ISalesOrderService _salesOrderService)
        {
            return (employee = _validator.ValidDeleteObject(employee, _salesOrderService) ?
                    _repository.SoftDeleteObject(employee) : employee);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsNameDuplicated(Employee employee)
        {
            IQueryable<Employee> employees = _repository.FindAll(x => x.Name == employee.Name && !x.IsDeleted && x.Id != employee.Id);
            return (employees.Count() > 0 ? true : false);
        }
    }
}