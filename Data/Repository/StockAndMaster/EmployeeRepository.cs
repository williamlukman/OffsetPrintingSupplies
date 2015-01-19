using Core.DomainModel;
using Core.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Context;
using Data.Repository;
using System.Data;

namespace Data.Repository
{
    public class EmployeeRepository : EfRepository<Employee>, IEmployeeRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public EmployeeRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<Employee> GetQueryable()
        {
            return FindAll();
        }

        public IList<Employee> GetAll()
        {
            return FindAll().ToList();
        }

        public Employee GetObjectById(int Id)
        {
            Employee employee = Find(x => x.Id == Id && !x.IsDeleted);
            if (employee != null) { employee.Errors = new Dictionary<string, string>(); }
            return employee;
        }

        public Employee GetObjectByName(string Name)
        {
            Employee employee = Find(x => x.Name == Name && !x.IsDeleted);
            if (employee != null) { employee.Errors = new Dictionary<string, string>(); }
            return employee;
        }

        public Employee CreateObject(Employee employee)
        {
            employee.IsDeleted = false;
            employee.CreatedAt = DateTime.Now;
            return Create(employee);
        }

        public Employee UpdateObject(Employee employee)
        {
            employee.UpdatedAt = DateTime.Now;
            Update(employee);
            return employee;
        }

        public Employee SoftDeleteObject(Employee employee)
        {
            employee.IsDeleted = true;
            employee.DeletedAt = DateTime.Now;
            Update(employee);
            return employee;
        }

        public bool DeleteObject(int Id)
        {
            Employee employee = Find(x => x.Id == Id);
            return (Delete(employee) == 1) ? true : false;
        }
    }
}