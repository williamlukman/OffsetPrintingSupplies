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
    public class CustomerStockAdjustmentDetailRepository : EfRepository<CustomerStockAdjustmentDetail>, ICustomerStockAdjustmentDetailRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public CustomerStockAdjustmentDetailRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<CustomerStockAdjustmentDetail> GetQueryable()
        {
            return FindAll();
        }

        public IList<CustomerStockAdjustmentDetail> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<CustomerStockAdjustmentDetail> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month && !x.IsDeleted).ToList();
        }

        public IList<CustomerStockAdjustmentDetail> GetObjectsByCustomerStockAdjustmentId(int customerStockAdjustmentId)
        {
            return FindAll(x => x.CustomerStockAdjustmentId == customerStockAdjustmentId && !x.IsDeleted).ToList();
        }

        public IList<CustomerStockAdjustmentDetail> GetObjectsByItemId(int itemId)
        {
            return FindAll(x => x.ItemId == itemId && !x.IsDeleted).ToList();
        }

        public CustomerStockAdjustmentDetail GetObjectById(int Id)
        {
            CustomerStockAdjustmentDetail detail = FindAll(x => x.Id == Id && !x.IsDeleted).FirstOrDefault();
            detail.Errors = new Dictionary<string, string>();
            return detail;
        }

        public CustomerStockAdjustmentDetail CreateObject(CustomerStockAdjustmentDetail customerStockAdjustmentDetail)
        {
            string ParentCode = "";
            using (var db = GetContext())
            {
                ParentCode = (from obj in db.CustomerStockAdjustments
                              where obj.Id == customerStockAdjustmentDetail.CustomerStockAdjustmentId
                              select obj.Code).FirstOrDefault();
            }
            customerStockAdjustmentDetail.Code = SetObjectCode(ParentCode);
            customerStockAdjustmentDetail.IsConfirmed = false;
            customerStockAdjustmentDetail.IsDeleted = false;
            customerStockAdjustmentDetail.CreatedAt = DateTime.Now;
            return Create(customerStockAdjustmentDetail);
        }

        public CustomerStockAdjustmentDetail UpdateObject(CustomerStockAdjustmentDetail customerStockAdjustmentDetail)
        {
            customerStockAdjustmentDetail.UpdatedAt = DateTime.Now;
            Update(customerStockAdjustmentDetail);
            return customerStockAdjustmentDetail;
        }

        public CustomerStockAdjustmentDetail SoftDeleteObject(CustomerStockAdjustmentDetail customerStockAdjustmentDetail)
        {
            customerStockAdjustmentDetail.IsDeleted = true;
            customerStockAdjustmentDetail.DeletedAt = DateTime.Now;
            Update(customerStockAdjustmentDetail);
            return customerStockAdjustmentDetail;
        }

        public bool DeleteObject(int Id)
        {
            CustomerStockAdjustmentDetail customerStockAdjustmentDetail = Find(x => x.Id == Id);
            return (Delete(customerStockAdjustmentDetail) == 1) ? true : false;
        }

        public CustomerStockAdjustmentDetail ConfirmObject(CustomerStockAdjustmentDetail customerStockAdjustmentDetail)
        {
            customerStockAdjustmentDetail.IsConfirmed = true;
            Update(customerStockAdjustmentDetail);
            return customerStockAdjustmentDetail;
        }

        public CustomerStockAdjustmentDetail UnconfirmObject(CustomerStockAdjustmentDetail customerStockAdjustmentDetail)
        {
            customerStockAdjustmentDetail.IsConfirmed = false;
            customerStockAdjustmentDetail.ConfirmationDate = null;
            customerStockAdjustmentDetail.UpdatedAt = DateTime.Now;
            Update(customerStockAdjustmentDetail);
            return customerStockAdjustmentDetail;
        }

        public string SetObjectCode(string ParentCode)
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = ParentCode + "." + totalnumberinthemonth;
            return Code;
        } 

    }
}