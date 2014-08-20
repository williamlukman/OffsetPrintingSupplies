using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IStockMutationRepository : IRepository<StockMutation>
    {
        IQueryable<StockMutation> GetQueryable();
        IList<StockMutation> GetAll();
        IList<StockMutation> GetObjectsByItemId(int itemId);
        IList<StockMutation> GetObjectsByWarehouseId(int warehouseId);
        IList<StockMutation> GetObjectsByWarehouseItemId(int warehouseItemId);
        StockMutation GetObjectById(int Id);
        IList<StockMutation> GetObjectsBySourceDocumentDetailForWarehouseItem(int warehouseItemId, string SourceDocumentDetailType, int SourceDocumentDetailId);
        IList<StockMutation> GetObjectsBySourceDocumentDetailForItem(int itemId, string SourceDocumentDetailType, int SourceDocumentDetailId);
        StockMutation CreateObject(StockMutation stockMutation);
        StockMutation UpdateObject(StockMutation stockMutation);
        StockMutation SoftDeleteObject(StockMutation stockMutation);
        bool DeleteObject(int Id);
    }
}