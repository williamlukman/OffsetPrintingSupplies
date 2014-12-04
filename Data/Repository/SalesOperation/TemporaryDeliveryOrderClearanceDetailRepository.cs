using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repository
{
    public class TemporaryDeliveryOrderClearanceDetailRepository : EfRepository<TemporaryDeliveryOrderClearanceDetail>, ITemporaryDeliveryOrderClearanceDetailRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public TemporaryDeliveryOrderClearanceDetailRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<TemporaryDeliveryOrderClearanceDetail> GetQueryable()
        {
            return FindAll();
        }

        public IList<TemporaryDeliveryOrderClearanceDetail> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<TemporaryDeliveryOrderClearanceDetail> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month && !x.IsDeleted).ToList();
        }

        public IList<TemporaryDeliveryOrderClearanceDetail> GetObjectsByTemporaryDeliveryOrderClearanceId(int temporaryDeliveryOrderClearanceId)
        {
            return FindAll(x => x.TemporaryDeliveryOrderClearanceId == temporaryDeliveryOrderClearanceId && !x.IsDeleted).ToList();
        }

        public TemporaryDeliveryOrderClearanceDetail GetObjectByCode(string orderCode)
        {
            TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail = Find(x => x.Code == orderCode && !x.IsDeleted);
            if (temporaryDeliveryOrderClearanceDetail != null) { temporaryDeliveryOrderClearanceDetail.Errors = new Dictionary<string, string>(); }
            return temporaryDeliveryOrderClearanceDetail;
        }

        public TemporaryDeliveryOrderClearanceDetail GetObjectById(int Id)
        {
            TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail = Find(x => x.Id == Id && !x.IsDeleted);
            if (temporaryDeliveryOrderClearanceDetail != null) { temporaryDeliveryOrderClearanceDetail.Errors = new Dictionary<string, string>(); }
            return temporaryDeliveryOrderClearanceDetail;
        }

        public IList<TemporaryDeliveryOrderClearanceDetail> GetObjectsByTemporaryDeliveryOrderDetailId(int temporaryDeliveryOrderDetailId)
        {
            return FindAll(x => x.TemporaryDeliveryOrderDetailId == temporaryDeliveryOrderDetailId && !x.IsDeleted).ToList();
        }

        public TemporaryDeliveryOrderClearanceDetail CreateObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail)
        {
            string ParentCode = ""; 
            using (var db = GetContext())
            {
                ParentCode = (from obj in db.TemporaryDeliveryOrderClearances
                              where obj.Id == temporaryDeliveryOrderClearanceDetail.TemporaryDeliveryOrderClearanceId
                              select obj.Code).FirstOrDefault();
            }
            temporaryDeliveryOrderClearanceDetail.Code = SetObjectCode(ParentCode);
            temporaryDeliveryOrderClearanceDetail.IsConfirmed = false;
            temporaryDeliveryOrderClearanceDetail.IsDeleted = false;
            temporaryDeliveryOrderClearanceDetail.CreatedAt = DateTime.Now;
            return Create(temporaryDeliveryOrderClearanceDetail);
        }

        public TemporaryDeliveryOrderClearanceDetail UpdateObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail)
        {
            temporaryDeliveryOrderClearanceDetail.UpdatedAt = DateTime.Now;
            Update(temporaryDeliveryOrderClearanceDetail);
            return temporaryDeliveryOrderClearanceDetail;
        }

        public TemporaryDeliveryOrderClearanceDetail SoftDeleteObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail)
        {
            temporaryDeliveryOrderClearanceDetail.IsDeleted = true;
            temporaryDeliveryOrderClearanceDetail.DeletedAt = DateTime.Now;
            Update(temporaryDeliveryOrderClearanceDetail);
            return temporaryDeliveryOrderClearanceDetail;
        }

        public bool DeleteObject(int Id)
        {
            TemporaryDeliveryOrderClearanceDetail dod = Find(x => x.Id == Id);
            return (Delete(dod) == 1) ? true : false;
        }

        public TemporaryDeliveryOrderClearanceDetail ConfirmObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail)
        {
            temporaryDeliveryOrderClearanceDetail.IsConfirmed = true;
            Update(temporaryDeliveryOrderClearanceDetail);
            return temporaryDeliveryOrderClearanceDetail;
        }

        public TemporaryDeliveryOrderClearanceDetail UnconfirmObject(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail)
        {
            temporaryDeliveryOrderClearanceDetail.IsConfirmed = false;
            temporaryDeliveryOrderClearanceDetail.ConfirmationDate = null;
            UpdateObject(temporaryDeliveryOrderClearanceDetail);
            return temporaryDeliveryOrderClearanceDetail;
        }

        public string SetObjectCode(string ParentCode)
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = ParentCode + "." + totalnumberinthemonth;
            return Code;
        } 
    }
}