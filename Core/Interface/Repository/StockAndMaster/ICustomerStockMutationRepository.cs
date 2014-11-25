using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ICustomerStockMutationRepository : IRepository<CustomerStockMutation>
    {
        IQueryable<CustomerStockMutation> GetQueryable();
        IList<CustomerStockMutation> GetAll();
        IList<CustomerStockMutation> GetObjectsByCustomerItemId(int customerItemId);
        IList<CustomerStockMutation> GetObjectsByContactId(int contactId);
        CustomerStockMutation GetObjectById(int Id);
        IList<CustomerStockMutation> GetObjectsBySourceDocumentDetailForCustomerItem(int customerItemId, string SourceDocumentDetailType, int SourceDocumentDetailId);
        //IList<CustomerStockMutation> GetObjectsBySourceDocumentDetailForItem(int itemId, string SourceDocumentDetailType, int SourceDocumentDetailId);
        CustomerStockMutation CreateObject(CustomerStockMutation customerStockMutation);
        CustomerStockMutation UpdateObject(CustomerStockMutation customerStockMutation);
        CustomerStockMutation SoftDeleteObject(CustomerStockMutation customerStockMutation);
        bool DeleteObject(int Id);
    }
}