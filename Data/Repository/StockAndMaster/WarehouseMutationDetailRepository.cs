using Core.DomainModel;
using Core.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Context;
using Data.Repository;
using System.Data;

namespace Data.Repository
{
    public class WarehouseMutationDetailRepository : EfRepository<WarehouseMutationDetail>, IWarehouseMutationDetailRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public WarehouseMutationDetailRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<WarehouseMutationDetail> GetQueryable()
        {
            return FindAll();
        }

        public IList<WarehouseMutationDetail> GetAll()
        {
            return FindAll().ToList();
        }

        public IList<WarehouseMutationDetail> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month && !x.IsDeleted).ToList();
        }

        public IList<WarehouseMutationDetail> GetObjectsByWarehouseMutationId(int warehouseMutationId)
        {
            return FindAll(x => x.WarehouseMutationId == warehouseMutationId && !x.IsDeleted).ToList();
        }

        public WarehouseMutationDetail GetObjectById(int Id)
        {
            WarehouseMutationDetail warehouseMutationDetail = Find(x => x.Id == Id && !x.IsDeleted);
            if (warehouseMutationDetail != null) { warehouseMutationDetail.Errors = new Dictionary<string, string>(); }
            return warehouseMutationDetail;
        }

        public WarehouseMutationDetail CreateObject(WarehouseMutationDetail warehouseMutationDetail)
        {
            string ParentCode = "";
            using (var db = GetContext())
            {
                ParentCode = (from obj in db.WarehouseMutations
                              where obj.Id == warehouseMutationDetail.WarehouseMutationId
                              select obj.Code).FirstOrDefault();
            }
            warehouseMutationDetail.Code = SetObjectCode(ParentCode);
            warehouseMutationDetail.IsConfirmed = false;
            warehouseMutationDetail.IsDeleted = false;
            warehouseMutationDetail.CreatedAt = DateTime.Now;
            return Create(warehouseMutationDetail);
        }

        public WarehouseMutationDetail UpdateObject(WarehouseMutationDetail warehouseMutationDetail)
        {
            warehouseMutationDetail.UpdatedAt = DateTime.Now;
            Update(warehouseMutationDetail);
            return warehouseMutationDetail;
        }

        public WarehouseMutationDetail SoftDeleteObject(WarehouseMutationDetail warehouseMutationDetail)
        {
            warehouseMutationDetail.IsDeleted = true;
            warehouseMutationDetail.DeletedAt = DateTime.Now;
            Update(warehouseMutationDetail);
            return warehouseMutationDetail;
        }

        public bool DeleteObject(int Id)
        {
            WarehouseMutationDetail warehouseMutationDetail =  Find(x => x.Id == Id);
            return (Delete(warehouseMutationDetail) == 1) ? true : false;
        }

        public WarehouseMutationDetail ConfirmObject(WarehouseMutationDetail warehouseMutationDetail)
        {
            warehouseMutationDetail.IsConfirmed = true;
            Update(warehouseMutationDetail);
            return warehouseMutationDetail;
        }

        public WarehouseMutationDetail UnconfirmObject(WarehouseMutationDetail warehouseMutationDetail)
        {
            warehouseMutationDetail.IsConfirmed = false;
            warehouseMutationDetail.ConfirmationDate = null;
            warehouseMutationDetail.UpdatedAt = DateTime.Now;
            Update(warehouseMutationDetail);
            return warehouseMutationDetail;
        }

        public string SetObjectCode(string ParentCode)
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = ParentCode + totalnumberinthemonth;
            return Code;
        } 
    }
}