using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repository
{
    public class TemporaryDeliveryOrderDetailRepository : EfRepository<TemporaryDeliveryOrderDetail>, ITemporaryDeliveryOrderDetailRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public TemporaryDeliveryOrderDetailRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<TemporaryDeliveryOrderDetail> GetQueryable()
        {
            return FindAll();
        }

        public IList<TemporaryDeliveryOrderDetail> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<TemporaryDeliveryOrderDetail> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month && !x.IsDeleted).ToList();
        }

        public IList<TemporaryDeliveryOrderDetail> GetObjectsByTemporaryDeliveryOrderId(int temporaryDeliveryOrderId)
        {
            return FindAll(x => x.TemporaryDeliveryOrderId == temporaryDeliveryOrderId && !x.IsDeleted).ToList();
        }

        public TemporaryDeliveryOrderDetail GetObjectByCode(string orderCode)
        {
            TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail = Find(x => x.Code == orderCode && !x.IsDeleted);
            if (temporaryDeliveryOrderDetail != null) { temporaryDeliveryOrderDetail.Errors = new Dictionary<string, string>(); }
            return temporaryDeliveryOrderDetail;
        }

        public TemporaryDeliveryOrderDetail GetObjectById(int Id)
        {
            TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail = Find(x => x.Id == Id && !x.IsDeleted);
            if (temporaryDeliveryOrderDetail != null) { temporaryDeliveryOrderDetail.Errors = new Dictionary<string, string>(); }
            return temporaryDeliveryOrderDetail;
        }

        public IList<TemporaryDeliveryOrderDetail> GetObjectsBySalesOrderDetailId(int salesOrderDetailId)
        {
            return FindAll(x => x.SalesOrderDetailId == salesOrderDetailId && !x.IsDeleted).ToList();
        }

        public IList<TemporaryDeliveryOrderDetail> GetObjectsByVirtualOrderDetailId(int virtualOrderDetailId)
        {
            return FindAll(x => x.VirtualOrderDetailId == virtualOrderDetailId && !x.IsDeleted).ToList();
        }

        public TemporaryDeliveryOrderDetail CreateObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail)
        {
            string ParentCode = ""; 
            using (var db = GetContext())
            {
                ParentCode = (from obj in db.TemporaryDeliveryOrders
                              where obj.Id == temporaryDeliveryOrderDetail.TemporaryDeliveryOrderId
                              select obj.Code).FirstOrDefault();
            }
            temporaryDeliveryOrderDetail.Code = SetObjectCode(ParentCode);
            temporaryDeliveryOrderDetail.IsConfirmed = false;
            temporaryDeliveryOrderDetail.IsDeleted = false;
            temporaryDeliveryOrderDetail.IsAllCompleted = false;
            temporaryDeliveryOrderDetail.CreatedAt = DateTime.Now;
            return Create(temporaryDeliveryOrderDetail);
        }

        public TemporaryDeliveryOrderDetail UpdateObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail)
        {
            temporaryDeliveryOrderDetail.UpdatedAt = DateTime.Now;
            Update(temporaryDeliveryOrderDetail);
            return temporaryDeliveryOrderDetail;
        }

        public TemporaryDeliveryOrderDetail SoftDeleteObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail)
        {
            temporaryDeliveryOrderDetail.IsDeleted = true;
            temporaryDeliveryOrderDetail.DeletedAt = DateTime.Now;
            Update(temporaryDeliveryOrderDetail);
            return temporaryDeliveryOrderDetail;
        }

        public bool DeleteObject(int Id)
        {
            TemporaryDeliveryOrderDetail dod = Find(x => x.Id == Id);
            return (Delete(dod) == 1) ? true : false;
        }

        public TemporaryDeliveryOrderDetail ConfirmObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail)
        {
            temporaryDeliveryOrderDetail.IsConfirmed = true;
            Update(temporaryDeliveryOrderDetail);
            return temporaryDeliveryOrderDetail;
        }

        public TemporaryDeliveryOrderDetail UnconfirmObject(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail)
        {
            temporaryDeliveryOrderDetail.IsConfirmed = false;
            temporaryDeliveryOrderDetail.ConfirmationDate = null;
            UpdateObject(temporaryDeliveryOrderDetail);
            return temporaryDeliveryOrderDetail;
        }

        public string SetObjectCode(string ParentCode)
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = ParentCode + "." + totalnumberinthemonth;
            return Code;
        } 
    }
}