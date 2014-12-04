using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repository
{
    public class TemporaryDeliveryOrderClearanceRepository : EfRepository<TemporaryDeliveryOrderClearance>, ITemporaryDeliveryOrderClearanceRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public TemporaryDeliveryOrderClearanceRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<TemporaryDeliveryOrderClearance> GetQueryable()
        {
            return FindAll();
        }

        public IList<TemporaryDeliveryOrderClearance> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<TemporaryDeliveryOrderClearance> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month && !x.IsDeleted).ToList();
        }

        public TemporaryDeliveryOrderClearance GetObjectById(int Id)
        {
            TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance = Find(x => x.Id == Id && !x.IsDeleted);
            if (temporaryDeliveryOrderClearance != null) { temporaryDeliveryOrderClearance.Errors = new Dictionary<string, string>(); }
            return temporaryDeliveryOrderClearance;
        }

        public IList<TemporaryDeliveryOrderClearance> GetObjectsByTemporaryDeliveryOrderId(int temporaryDeliveryOrderId)
        {
            return FindAll(x => x.TemporaryDeliveryOrderId == temporaryDeliveryOrderId && !x.IsDeleted).ToList();
        }

        public IList<TemporaryDeliveryOrderClearance> GetConfirmedObjects()
        {
            return FindAll(x => x.IsConfirmed && !x.IsDeleted).ToList();
        }

        public TemporaryDeliveryOrderClearance CreateObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance)
        {
            temporaryDeliveryOrderClearance.Code = SetObjectCode();
            temporaryDeliveryOrderClearance.IsDeleted = false;
            temporaryDeliveryOrderClearance.IsConfirmed = false;
            temporaryDeliveryOrderClearance.CreatedAt = DateTime.Now;
            return Create(temporaryDeliveryOrderClearance);
        }

        public TemporaryDeliveryOrderClearance UpdateObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance)
        {
            temporaryDeliveryOrderClearance.UpdatedAt = DateTime.Now;
            Update(temporaryDeliveryOrderClearance);
            return temporaryDeliveryOrderClearance;
        }

        public TemporaryDeliveryOrderClearance SoftDeleteObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance)
        {
            temporaryDeliveryOrderClearance.IsDeleted = true;
            temporaryDeliveryOrderClearance.DeletedAt = DateTime.Now;
            Update(temporaryDeliveryOrderClearance);
            return temporaryDeliveryOrderClearance;
        }

        public bool DeleteObject(int Id)
        {
            TemporaryDeliveryOrderClearance d = Find(x => x.Id == Id);
            return (Delete(d) == 1) ? true : false;
        }

        public TemporaryDeliveryOrderClearance ConfirmObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance)
        {
            temporaryDeliveryOrderClearance.IsConfirmed = true;
            Update(temporaryDeliveryOrderClearance);
            return temporaryDeliveryOrderClearance;
        }

        public TemporaryDeliveryOrderClearance UnconfirmObject(TemporaryDeliveryOrderClearance temporaryDeliveryOrderClearance)
        {
            temporaryDeliveryOrderClearance.IsConfirmed = false;
            temporaryDeliveryOrderClearance.ConfirmationDate = null;
            UpdateObject(temporaryDeliveryOrderClearance);
            return temporaryDeliveryOrderClearance;
        }

        public string SetObjectCode()
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = DateTime.Today.Year.ToString() + "." + DateTime.Today.Month.ToString() + "." + totalnumberinthemonth;
            return Code;
        }
    }
}