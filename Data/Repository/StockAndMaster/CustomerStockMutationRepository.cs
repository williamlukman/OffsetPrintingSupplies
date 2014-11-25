using Core.DomainModel;
using Core.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Context;
using System.Data;
using System.Linq.Dynamic;
using System.Data.Entity;

namespace Data.Repository
{
    public class CustomerStockMutationRepository : EfRepository<CustomerStockMutation>, ICustomerStockMutationRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public CustomerStockMutationRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<CustomerStockMutation> GetQueryable()
        {
            return FindAll();
        }

        public IList<CustomerStockMutation> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<CustomerStockMutation> GetObjectsByContactId(int contactId)
        {
            return FindAll(x => x.ContactId.Value == contactId && !x.IsDeleted).ToList();
        }

        public IList<CustomerStockMutation> GetObjectsByCustomerItemId(int customerItemId)
        {
            return FindAll(x => x.CustomerItemId == customerItemId && !x.IsDeleted).ToList();
        }

        public CustomerStockMutation GetObjectById(int Id)
        {
            CustomerStockMutation customerStockMutation = FindAll(x => x.Id == Id && !x.IsDeleted).Include(x => x.CustomerItem).Include(x => x.Item).Include(x => x.Contact).FirstOrDefault();
            if (customerStockMutation != null) { customerStockMutation.Errors = new Dictionary<string, string>(); }
            return customerStockMutation;
        }

        public IList<CustomerStockMutation> GetObjectsBySourceDocumentDetailForCustomerItem(int customerItemId, string SourceDocumentDetailType, int SourceDocumentDetailId)
        {
            return FindAll(x => x.CustomerItemId == customerItemId && x.SourceDocumentDetailType == SourceDocumentDetailType
                                && x.SourceDocumentDetailId == SourceDocumentDetailId && !x.IsDeleted).ToList();
        }

        //public IList<CustomerStockMutation> GetObjectsBySourceDocumentDetailForItem(int itemId, string SourceDocumentDetailType, int SourceDocumentDetailId)
        //{
        //    return FindAll(x => x.ItemId == itemId && x.SourceDocumentDetailType == SourceDocumentDetailType
        //                        && x.SourceDocumentDetailId == SourceDocumentDetailId && !x.IsDeleted).ToList();
        //}

        public CustomerStockMutation CreateObject(CustomerStockMutation customerStockMutation)
        {
            customerStockMutation.IsDeleted = false;
            customerStockMutation.CreatedAt = DateTime.Now;
            return Create(customerStockMutation);
        }

        public CustomerStockMutation UpdateObject(CustomerStockMutation customerStockMutation)
        {
            customerStockMutation.UpdatedAt = DateTime.Now;
            Update(customerStockMutation);
            return customerStockMutation;
        }

        public CustomerStockMutation SoftDeleteObject(CustomerStockMutation customerStockMutation)
        {
            customerStockMutation.IsDeleted = true;
            customerStockMutation.DeletedAt = DateTime.Now;
            Update(customerStockMutation);
            return customerStockMutation;
        }

        public bool DeleteObject(int Id)
        {
            CustomerStockMutation customerStockMutation = Find(x => x.Id == Id);
            return (Delete(customerStockMutation) == 1) ? true : false;
        }

    }
}