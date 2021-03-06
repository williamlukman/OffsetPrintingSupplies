﻿using Core.DomainModel;
using Core.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Context;
using Data.Repository;
using System.Data;
using System.Linq.Dynamic;
using System.Data.Entity;

namespace Data.Repository
{
    public class CustomerItemRepository : EfRepository<CustomerItem>, ICustomerItemRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public CustomerItemRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<CustomerItem> GetQueryable()
        {
            return FindAll();
        }

        public IList<CustomerItem> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<CustomerItem> GetObjectsByContactId(int contactId)
        {
            return FindAll(x => x.ContactId == contactId && !x.IsDeleted).ToList();
        }

        public IList<CustomerItem> GetObjectsByWarehouseItemId(int warehouseItemId)
        {
            return FindAll(x => x.WarehouseItemId == warehouseItemId && !x.IsDeleted).ToList();
        }

        public IList<CustomerItem> GetObjectsByItemId(int itemId)
        {
            return FindAll(x => !x.IsDeleted).Include(x => x.WarehouseItem).Where(x => x.WarehouseItem.ItemId == itemId).ToList();
        }

        public IList<CustomerItem> GetObjectsByWarehouseId(int warehouseId)
        {
            return FindAll(x => !x.IsDeleted).Include(x => x.WarehouseItem).Where(x => x.WarehouseItem.WarehouseId == warehouseId).ToList();
        }

        public CustomerItem GetObjectById(int Id)
        {
            CustomerItem customerItem = FindAll(x => x.Id == Id && !x.IsDeleted).Include(x => x.Contact).Include(x => x.WarehouseItem).FirstOrDefault();
            if (customerItem != null) { customerItem.Errors = new Dictionary<string, string>(); }
            return customerItem;
        }

        public CustomerItem FindOrCreateObject(int ContactId, int WarehouseItemId)
        {
            CustomerItem customerItem = FindAll(x => x.ContactId == ContactId && x.WarehouseItemId == WarehouseItemId && !x.IsDeleted).Include(x => x.WarehouseItem).Include(x => x.Contact).FirstOrDefault();
            if (customerItem == null)
            {
                customerItem = new CustomerItem()
                {
                    ContactId = ContactId,
                    WarehouseItemId = WarehouseItemId,
                    Quantity = 0
                };
                customerItem = CreateObject(customerItem);
                
            }
            customerItem.Errors = new Dictionary<string, string>();
            return customerItem;
        }

        public CustomerItem CreateObject(CustomerItem customerItem)
        {
            customerItem.Quantity = 0;
            customerItem.IsDeleted = false;
            customerItem.CreatedAt = DateTime.Now;
            return Create(customerItem);
        }

        public CustomerItem UpdateObject(CustomerItem customerItem)
        {
            customerItem.UpdatedAt = DateTime.Now;
            Update(customerItem);
            return customerItem;
        }

        public CustomerItem SoftDeleteObject(CustomerItem customerItem)
        {
            customerItem.IsDeleted = true;
            customerItem.DeletedAt = DateTime.Now;
            Update(customerItem);
            return customerItem;
        }

        public bool DeleteObject(int Id)
        {
            CustomerItem customerItem =  Find(x => x.Id == Id);
            return (Delete(customerItem) == 1) ? true : false;
        }
    }
}