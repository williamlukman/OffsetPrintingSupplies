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
    public class WarehouseMutationOrderDetailRepository : EfRepository<WarehouseMutationOrderDetail>, IWarehouseMutationOrderDetailRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public WarehouseMutationOrderDetailRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IList<WarehouseMutationOrderDetail> GetAll()
        {
            return FindAll().ToList();
        }

        public IList<WarehouseMutationOrderDetail> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month && !x.IsDeleted).ToList();
        }

        public IList<WarehouseMutationOrderDetail> GetObjectsByWarehouseMutationOrderId(int warehouseMutationOrderId)
        {
            return FindAll(x => x.WarehouseMutationOrderId == warehouseMutationOrderId && !x.IsDeleted).ToList();
        }

        public WarehouseMutationOrderDetail GetObjectById(int Id)
        {
            WarehouseMutationOrderDetail warehouseMutationOrderDetail = Find(x => x.Id == Id && !x.IsDeleted);
            if (warehouseMutationOrderDetail != null) { warehouseMutationOrderDetail.Errors = new Dictionary<string, string>(); }
            return warehouseMutationOrderDetail;
        }

        public WarehouseMutationOrderDetail CreateObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail)
        {
            warehouseMutationOrderDetail.IsConfirmed = false;
            warehouseMutationOrderDetail.IsDeleted = false;
            warehouseMutationOrderDetail.CreatedAt = DateTime.Now;
            return Create(warehouseMutationOrderDetail);
        }

        public WarehouseMutationOrderDetail UpdateObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail)
        {
            warehouseMutationOrderDetail.UpdatedAt = DateTime.Now;
            Update(warehouseMutationOrderDetail);
            return warehouseMutationOrderDetail;
        }

        public WarehouseMutationOrderDetail SoftDeleteObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail)
        {
            warehouseMutationOrderDetail.IsDeleted = true;
            warehouseMutationOrderDetail.DeletedAt = DateTime.Now;
            Update(warehouseMutationOrderDetail);
            return warehouseMutationOrderDetail;
        }

        public bool DeleteObject(int Id)
        {
            WarehouseMutationOrderDetail warehouseMutationOrderDetail =  Find(x => x.Id == Id);
            return (Delete(warehouseMutationOrderDetail) == 1) ? true : false;
        }

        public WarehouseMutationOrderDetail ConfirmObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail)
        {
            warehouseMutationOrderDetail.IsConfirmed = true;
            Update(warehouseMutationOrderDetail);
            return warehouseMutationOrderDetail;
        }

        public WarehouseMutationOrderDetail UnconfirmObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail)
        {
            warehouseMutationOrderDetail.IsConfirmed = false;
            warehouseMutationOrderDetail.ConfirmationDate = null;
            warehouseMutationOrderDetail.UpdatedAt = DateTime.Now;
            Update(warehouseMutationOrderDetail);
            return warehouseMutationOrderDetail;
        }

    }
}