using Core.DomainModel;
using Core.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Context;
using System.Data;

namespace Data.Repository
{
    public class CustomerStockAdjustmentRepository : EfRepository<CustomerStockAdjustment>, ICustomerStockAdjustmentRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public CustomerStockAdjustmentRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<CustomerStockAdjustment> GetQueryable()
        {
            return FindAll();
        }

        public IList<CustomerStockAdjustment> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<CustomerStockAdjustment> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month && !x.IsDeleted).ToList();
        }

        public CustomerStockAdjustment GetObjectById(int Id)
        {
            CustomerStockAdjustment customerStockAdjustment = FindAll(x => x.Id == Id && !x.IsDeleted).FirstOrDefault();
            if (customerStockAdjustment != null) { customerStockAdjustment.Errors = new Dictionary<string, string>(); }
            return customerStockAdjustment;
        }

        public CustomerStockAdjustment CreateObject(CustomerStockAdjustment customerStockAdjustment)
        {
            customerStockAdjustment.Code = SetObjectCode();
            customerStockAdjustment.IsDeleted = false;
            customerStockAdjustment.IsConfirmed = false;
            customerStockAdjustment.CreatedAt = DateTime.Now;
            return Create(customerStockAdjustment);
        }

        public CustomerStockAdjustment UpdateObject(CustomerStockAdjustment customerStockAdjustment)
        {
            customerStockAdjustment.UpdatedAt = DateTime.Now;
            Update(customerStockAdjustment);
            return customerStockAdjustment;
        }

        public CustomerStockAdjustment SoftDeleteObject(CustomerStockAdjustment customerStockAdjustment)
        {
            customerStockAdjustment.IsDeleted = true;
            customerStockAdjustment.DeletedAt = DateTime.Now;
            Update(customerStockAdjustment);
            return customerStockAdjustment;
        }

        public CustomerStockAdjustment ConfirmObject(CustomerStockAdjustment customerStockAdjustment)
        {
            customerStockAdjustment.IsConfirmed = true;
            Update(customerStockAdjustment);
            return customerStockAdjustment;
        }

        public CustomerStockAdjustment UnconfirmObject(CustomerStockAdjustment customerStockAdjustment)
        {
            customerStockAdjustment.IsConfirmed = false;
            customerStockAdjustment.ConfirmationDate = null;
            customerStockAdjustment.UpdatedAt = DateTime.Now;
            Update(customerStockAdjustment);
            return customerStockAdjustment;
        }

        public bool DeleteObject(int Id)
        {
            CustomerStockAdjustment customerStockAdjustment = Find(x => x.Id == Id);
            return (Delete(customerStockAdjustment) == 1) ? true : false;
        }

        public string SetObjectCode()
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = DateTime.Today.Year.ToString() + "." + DateTime.Today.Month.ToString() + "." + totalnumberinthemonth;
            return Code;
        }
    }
}