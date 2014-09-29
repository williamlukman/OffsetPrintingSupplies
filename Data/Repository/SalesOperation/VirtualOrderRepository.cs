using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repository
{
    public class VirtualOrderRepository : EfRepository<VirtualOrder>, IVirtualOrderRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public VirtualOrderRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<VirtualOrder> GetQueryable()
        {
            return FindAll();
        }

        public IList<VirtualOrder> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<VirtualOrder> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month && !x.IsDeleted).ToList();
        }

        public VirtualOrder GetObjectById(int Id)
        {
            VirtualOrder virtualOrder = Find(vo => vo.Id == Id && !vo.IsDeleted);
            if (virtualOrder != null) { virtualOrder.Errors = new Dictionary<string, string>(); }
            return virtualOrder;
        }

        public IList<VirtualOrder> GetObjectsByContactId(int contactId)
        {
            return FindAll(vo => vo.ContactId == contactId && !vo.IsDeleted).ToList();
        }

        public IList<VirtualOrder> GetConfirmedObjects()
        {
            return FindAll(x => x.IsConfirmed && !x.IsDeleted).ToList();
        }

        public VirtualOrder CreateObject(VirtualOrder virtualOrder)
        {
            virtualOrder.Code = SetObjectCode();
            virtualOrder.IsDeleted = false;
            virtualOrder.IsConfirmed = false;
            virtualOrder.CreatedAt = DateTime.Now;
            return Create(virtualOrder);
        }

        public VirtualOrder UpdateObject(VirtualOrder virtualOrder)
        {
            virtualOrder.UpdatedAt = DateTime.Now;
            Update(virtualOrder);
            return virtualOrder;
        }

        public VirtualOrder SoftDeleteObject(VirtualOrder virtualOrder)
        {
            virtualOrder.IsDeleted = true;
            virtualOrder.DeletedAt = DateTime.Now;
            Update(virtualOrder);
            return virtualOrder;
        }

        public bool DeleteObject(int Id)
        {
            VirtualOrder vo = Find(x => x.Id == Id);
            return (Delete(vo) == 1) ? true : false;
        }

        public VirtualOrder ConfirmObject(VirtualOrder virtualOrder)
        {
            virtualOrder.IsConfirmed = true;
            Update(virtualOrder);
            return virtualOrder;
        }

        public VirtualOrder UnconfirmObject(VirtualOrder virtualOrder)
        {
            virtualOrder.IsConfirmed = false;
            virtualOrder.ConfirmationDate = null;
            UpdateObject(virtualOrder);
            return virtualOrder;
        }

        public VirtualOrder SetDeliveryComplete(VirtualOrder virtualOrder)
        {
            virtualOrder.IsDeliveryCompleted = true;
            UpdateObject(virtualOrder);
            return virtualOrder;
        }

        public VirtualOrder UnsetDeliveryComplete(VirtualOrder virtualOrder)
        {
            virtualOrder.IsDeliveryCompleted = false;
            UpdateObject(virtualOrder);
            return virtualOrder;
        }

        public string SetObjectCode()
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = DateTime.Today.Year.ToString() + "." + DateTime.Today.Month.ToString() + "." + totalnumberinthemonth;
            return Code;
        }
    }
}