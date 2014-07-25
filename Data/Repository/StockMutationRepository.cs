using Core.DomainModel;
using Core.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Context;
using System.Data;

namespace Data.Repository
{
    public class StockMutationRepository : EfRepository<StockMutation>, IStockMutationRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public StockMutationRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IList<StockMutation> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<StockMutation> GetObjectsByItemId(int itemId)
        {
            return FindAll(x => x.ItemId == itemId && !x.IsDeleted).ToList();
        }

        public IList<StockMutation> GetObjectsByWarehouseId(int warehouseId)
        {
            return FindAll(x => x.WarehouseId == warehouseId && !x.IsDeleted).ToList();
        }

        public IList<StockMutation> GetObjectsByWarehouseItemId(int warehouseItemId)
        {
            return FindAll(x => x.WarehouseItemId == warehouseItemId && !x.IsDeleted).ToList();
        }

        public StockMutation GetObjectById(int Id)
        {
            StockMutation stockMutation = Find(x => x.Id == Id && !x.IsDeleted);
            if (stockMutation != null) { stockMutation.Errors = new Dictionary<string, string>(); }
            return stockMutation;
        }

        public IList<StockMutation> GetObjectsBySourceDocumentDetailForWarehouseItem(int warehouseItemId, string SourceDocumentDetailType, int SourceDocumentDetailId)
        {
            return FindAll(x => x.WarehouseItemId == warehouseItemId && x.SourceDocumentDetailType == SourceDocumentDetailType
                                && x.SourceDocumentDetailId == SourceDocumentDetailId && !x.IsDeleted).ToList();
        }

        public IList<StockMutation> GetObjectsBySourceDocumentDetailForItem(int itemId, string SourceDocumentDetailType, int SourceDocumentDetailId)
        {
            return FindAll(x => x.ItemId == itemId && x.SourceDocumentDetailType == SourceDocumentDetailType
                                && x.SourceDocumentDetailId == SourceDocumentDetailId && !x.IsDeleted).ToList();
        }

        public StockMutation CreateObject(StockMutation stockMutation)
        {
            stockMutation.IsDeleted = false;
            stockMutation.CreatedAt = DateTime.Now;
            return Create(stockMutation);
        }

        public StockMutation UpdateObject(StockMutation stockMutation)
        {
            stockMutation.UpdatedAt = DateTime.Now;
            Update(stockMutation);
            return stockMutation;
        }

        public StockMutation SoftDeleteObject(StockMutation stockMutation)
        {
            stockMutation.IsDeleted = true;
            stockMutation.DeletedAt = DateTime.Now;
            Update(stockMutation);
            return stockMutation;
        }

        public bool DeleteObject(int Id)
        {
            StockMutation stockMutation = Find(x => x.Id == Id);
            return (Delete(stockMutation) == 1) ? true : false;
        }

    }
}