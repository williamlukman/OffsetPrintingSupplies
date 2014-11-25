using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ICustomerItemRepository : IRepository<CustomerItem>
    {
        IQueryable<CustomerItem> GetQueryable();
        IList<CustomerItem> GetAll();
        IList<CustomerItem> GetObjectsByContactId(int ContactId);
        IList<CustomerItem> GetObjectsByItemId(int itemId);
        CustomerItem GetObjectById(int Id);
        CustomerItem FindOrCreateObject(int ContactId, int ItemId);
        CustomerItem CreateObject(CustomerItem customerItem);
        CustomerItem UpdateObject(CustomerItem customerItem);
        CustomerItem SoftDeleteObject(CustomerItem customerItem);
        bool DeleteObject(int Id);
    }
}