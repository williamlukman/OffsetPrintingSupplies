using Core.DomainModel;
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
            return FindAll().ToList();
        }

        public IList<CustomerItem> GetObjectsByContactId(int contactId)
        {
            return FindAll(x => x.ContactId == contactId && !x.IsDeleted).ToList();
        }

        public IList<CustomerItem> GetObjectsByItemId(int itemId)
        {
            return FindAll(x => x.ItemId == itemId).ToList();
        }

        public CustomerItem GetObjectById(int Id)
        {
            CustomerItem customerItem = FindAll(x => x.Id == Id && !x.IsDeleted).Include(x => x.Contact).Include(x => x.Item).FirstOrDefault();
            if (customerItem != null) { customerItem.Errors = new Dictionary<string, string>(); }
            return customerItem;
        }

        public CustomerItem FindOrCreateObject(int ContactId, int ItemId)
        {
            CustomerItem customerItem = Find(x => x.ContactId == ContactId && x.ItemId == ItemId && !x.IsDeleted);
            if (customerItem == null)
            {
                customerItem = new CustomerItem()
                {
                    ContactId = ContactId,
                    ItemId = ItemId,
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