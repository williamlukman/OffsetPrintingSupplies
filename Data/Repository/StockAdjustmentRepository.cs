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
    public class StockAdjustmentRepository : EfRepository<StockAdjustment>, IStockAdjustmentRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public StockAdjustmentRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IList<StockAdjustment> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public StockAdjustment GetObjectById(int Id)
        {
            StockAdjustment stockAdjustment = Find(x => x.Id == Id && !x.IsDeleted);
            if (stockAdjustment != null) { stockAdjustment.Errors = new Dictionary<string, string>(); }
            return stockAdjustment;
        }

        public StockAdjustment CreateObject(StockAdjustment stockAdjustment)
        {
            stockAdjustment.Code = SetObjectCode();
            stockAdjustment.IsDeleted = false;
            stockAdjustment.IsConfirmed = false;
            stockAdjustment.IsCompleted = false;
            stockAdjustment.CreatedAt = DateTime.Now;
            return Create(stockAdjustment);
        }

        public StockAdjustment UpdateObject(StockAdjustment stockAdjustment)
        {
            stockAdjustment.UpdatedAt = DateTime.Now;
            Update(stockAdjustment);
            return stockAdjustment;
        }

        public StockAdjustment SoftDeleteObject(StockAdjustment stockAdjustment)
        {
            stockAdjustment.IsDeleted = true;
            stockAdjustment.DeletedAt = DateTime.Now;
            Update(stockAdjustment);
            return stockAdjustment;
        }

        public StockAdjustment ConfirmObject(StockAdjustment stockAdjustment)
        {
            stockAdjustment.IsConfirmed = true;
            stockAdjustment.ConfirmationDate = DateTime.Now;
            Update(stockAdjustment);
            return stockAdjustment;
        }

        public StockAdjustment UnconfirmObject(StockAdjustment stockAdjustment)
        {
            stockAdjustment.IsConfirmed = false;
            stockAdjustment.ConfirmationDate = null;
            stockAdjustment.UpdatedAt = DateTime.Now;
            Update(stockAdjustment);
            return stockAdjustment;
        }

        public StockAdjustment CompleteObject(StockAdjustment stockAdjustment)
        {
            stockAdjustment.IsCompleted = true;
            stockAdjustment.UpdatedAt = DateTime.Now;
            Update(stockAdjustment);
            return stockAdjustment;
        }

        public bool DeleteObject(int Id)
        {
            StockAdjustment stockAdjustment = Find(x => x.Id == Id);
            return (Delete(stockAdjustment) == 1) ? true : false;
        }

        public string SetObjectCode()
        {
            // Code: #{year}/#{total_number
            int totalobject = FindAll().Count() + 1;
            string Code = "#" + DateTime.Now.Year.ToString() + "/#" + totalobject;
            return Code;
        }
    }
}