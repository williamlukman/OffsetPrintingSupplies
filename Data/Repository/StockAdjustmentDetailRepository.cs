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
    public class StockAdjustmentDetailRepository : EfRepository<StockAdjustmentDetail>, IStockAdjustmentDetailRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public StockAdjustmentDetailRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IList<StockAdjustmentDetail> GetObjectsByStockAdjustmentId(int stockAdjustmentId)
        {
            return FindAll(x => x.StockAdjustmentId == stockAdjustmentId && !x.IsDeleted).ToList();
        }

        public StockAdjustmentDetail GetObjectById(int Id)
        {
            StockAdjustmentDetail detail = Find(x => x.Id == Id && !x.IsDeleted);
            detail.Errors = new Dictionary<string, string>();
            return detail;
        }

        public StockAdjustmentDetail CreateObject(StockAdjustmentDetail stockAdjustmentDetail)
        {
            string ParentCode = "";
            using (var db = GetContext())
            {
                ParentCode = (from obj in db.StockAdjustments
                              where obj.Id == stockAdjustmentDetail.StockAdjustmentId
                              select obj.Code).FirstOrDefault();
            }
            stockAdjustmentDetail.Code = SetObjectCode(ParentCode);
            stockAdjustmentDetail.IsConfirmed = false;
            stockAdjustmentDetail.IsDeleted = false;
            stockAdjustmentDetail.CreatedAt = DateTime.Now;
            return Create(stockAdjustmentDetail);
        }

        public StockAdjustmentDetail UpdateObject(StockAdjustmentDetail stockAdjustmentDetail)
        {
            stockAdjustmentDetail.ModifiedAt = DateTime.Now;
            Update(stockAdjustmentDetail);
            return stockAdjustmentDetail;
        }

        public StockAdjustmentDetail SoftDeleteObject(StockAdjustmentDetail stockAdjustmentDetail)
        {
            stockAdjustmentDetail.IsDeleted = true;
            stockAdjustmentDetail.DeletedAt = DateTime.Now;
            Update(stockAdjustmentDetail);
            return stockAdjustmentDetail;
        }

        public bool DeleteObject(int Id)
        {
            StockAdjustmentDetail stockAdjustmentDetail = Find(x => x.Id == Id);
            return (Delete(stockAdjustmentDetail) == 1) ? true : false;
        }

        public StockAdjustmentDetail ConfirmObject(StockAdjustmentDetail stockAdjustmentDetail)
        {
            stockAdjustmentDetail.IsConfirmed = true;
            Update(stockAdjustmentDetail);
            return stockAdjustmentDetail;
        }

        public StockAdjustmentDetail UnconfirmObject(StockAdjustmentDetail stockAdjustmentDetail)
        {
            stockAdjustmentDetail.IsConfirmed = false;
            Update(stockAdjustmentDetail);
            return stockAdjustmentDetail;
        }

        public string SetObjectCode(string ParentCode)
        {
            // Code: #{parent_object.code}/#{total_number_objects}
            int totalobject = FindAll().Count() + 1;
            string Code = ParentCode + "/#" + totalobject;
            return Code;
        } 

    }
}