using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repository
{
    public class TemporaryDeliveryOrderRepository : EfRepository<TemporaryDeliveryOrder>, ITemporaryDeliveryOrderRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public TemporaryDeliveryOrderRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<TemporaryDeliveryOrder> GetQueryable()
        {
            return FindAll();
        }

        public IList<TemporaryDeliveryOrder> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<TemporaryDeliveryOrder> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month && !x.IsDeleted).ToList();
        }

        public TemporaryDeliveryOrder GetObjectById(int Id)
        {
            TemporaryDeliveryOrder temporaryDeliveryOrder = Find(x => x.Id == Id && !x.IsDeleted);
            if (temporaryDeliveryOrder != null) { temporaryDeliveryOrder.Errors = new Dictionary<string, string>(); }
            return temporaryDeliveryOrder;
        }

        public IList<TemporaryDeliveryOrder> GetObjectsByVirtualOrderId(int virtualOrderId)
        {
            return FindAll(x => x.VirtualOrderId == virtualOrderId && !x.IsDeleted).ToList();
        }

        public IList<TemporaryDeliveryOrder> GetObjectsByDeliveryOrderId(int deliveryOrderId)
        {
            return FindAll(x => x.DeliveryOrderId == deliveryOrderId && !x.IsDeleted).ToList();
        }

        public IList<TemporaryDeliveryOrder> GetConfirmedObjects()
        {
            return FindAll(x => x.IsConfirmed && !x.IsDeleted).ToList();
        }

        public TemporaryDeliveryOrder CreateObject(TemporaryDeliveryOrder temporaryDeliveryOrder)
        {
            temporaryDeliveryOrder.Code = SetObjectCode();
            temporaryDeliveryOrder.IsDeleted = false;
            temporaryDeliveryOrder.IsConfirmed = false;
            temporaryDeliveryOrder.CreatedAt = DateTime.Now;
            return Create(temporaryDeliveryOrder);
        }

        public TemporaryDeliveryOrder UpdateObject(TemporaryDeliveryOrder temporaryDeliveryOrder)
        {
            temporaryDeliveryOrder.UpdatedAt = DateTime.Now;
            Update(temporaryDeliveryOrder);
            return temporaryDeliveryOrder;
        }

        public TemporaryDeliveryOrder SoftDeleteObject(TemporaryDeliveryOrder temporaryDeliveryOrder)
        {
            temporaryDeliveryOrder.IsDeleted = true;
            temporaryDeliveryOrder.DeletedAt = DateTime.Now;
            Update(temporaryDeliveryOrder);
            return temporaryDeliveryOrder;
        }

        public bool DeleteObject(int Id)
        {
            TemporaryDeliveryOrder d = Find(x => x.Id == Id);
            return (Delete(d) == 1) ? true : false;
        }

        public TemporaryDeliveryOrder ConfirmObject(TemporaryDeliveryOrder temporaryDeliveryOrder)
        {
            temporaryDeliveryOrder.IsConfirmed = true;
            Update(temporaryDeliveryOrder);
            return temporaryDeliveryOrder;
        }

        public TemporaryDeliveryOrder UnconfirmObject(TemporaryDeliveryOrder temporaryDeliveryOrder)
        {
            temporaryDeliveryOrder.IsConfirmed = false;
            temporaryDeliveryOrder.ConfirmationDate = null;
            UpdateObject(temporaryDeliveryOrder);
            return temporaryDeliveryOrder;
        }

        public TemporaryDeliveryOrder SetReconcileComplete(TemporaryDeliveryOrder temporaryDeliveryOrder)
        {
            temporaryDeliveryOrder.IsReconcileCompleted = true;
            UpdateObject(temporaryDeliveryOrder);
            return temporaryDeliveryOrder;
        }

        public TemporaryDeliveryOrder UnsetReconcileComplete(TemporaryDeliveryOrder temporaryDeliveryOrder)
        {
            temporaryDeliveryOrder.IsReconcileCompleted = false;
            UpdateObject(temporaryDeliveryOrder);
            return temporaryDeliveryOrder;
        }

        public string SetObjectCode()
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = DateTime.Today.Year.ToString() + "." + DateTime.Today.Month.ToString() + "." + totalnumberinthemonth;
            return Code;
        }
    }
}