using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repository
{
    public class VirtualOrderDetailRepository : EfRepository<VirtualOrderDetail>, IVirtualOrderDetailRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public VirtualOrderDetailRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<VirtualOrderDetail> GetQueryable()
        {
            return FindAll();
        }

        public IList<VirtualOrderDetail> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<VirtualOrderDetail> GetAllByMonthCreated()
        {
            return FindAll(x => x.CreatedAt.Month == DateTime.Today.Month && !x.IsDeleted).ToList();
        }

        public IList<VirtualOrderDetail> GetObjectsByVirtualOrderId(int virtualOrderId)
        {
            return FindAll(x => x.VirtualOrderId == virtualOrderId && !x.IsDeleted).ToList();
        }

        public IList<VirtualOrderDetail> GetObjectsByItemId(int itemId)
        {
            return FindAll(x => x.ItemId == itemId && !x.IsDeleted).ToList();
        }

        public VirtualOrderDetail GetObjectById(int Id)
        {
            VirtualOrderDetail detail = Find(vod => vod.Id == Id);
            if (detail != null) { detail.Errors = new Dictionary<string, string>(); }
            return detail;
        }

        public VirtualOrderDetail CreateObject(VirtualOrderDetail virtualOrderDetail)
        {
            string ParentCode = "";
            using (var db = GetContext())
            {
                ParentCode = (from obj in db.VirtualOrders
                              where obj.Id == virtualOrderDetail.VirtualOrderId
                              select obj.Code).FirstOrDefault();
            }
            virtualOrderDetail.Code = SetObjectCode(ParentCode);
            virtualOrderDetail.IsConfirmed = false;
            virtualOrderDetail.IsDeleted = false;
            virtualOrderDetail.CreatedAt = DateTime.Now;
            virtualOrderDetail.PendingDeliveryQuantity = virtualOrderDetail.Quantity;
            return Create(virtualOrderDetail);
        }

        public VirtualOrderDetail UpdateObject(VirtualOrderDetail virtualOrderDetail)
        {
            virtualOrderDetail.UpdatedAt = DateTime.Now;
            Update(virtualOrderDetail);
            return virtualOrderDetail;
        }

        public VirtualOrderDetail SoftDeleteObject(VirtualOrderDetail virtualOrderDetail)
        {
            virtualOrderDetail.IsDeleted = true;
            virtualOrderDetail.DeletedAt = DateTime.Now;
            Update(virtualOrderDetail);
            return virtualOrderDetail;
        }

        public bool DeleteObject(int Id)
        {
            VirtualOrderDetail vod = Find(x => x.Id == Id);
            return (Delete(vod) == 1) ? true : false;
        }

        public VirtualOrderDetail ConfirmObject(VirtualOrderDetail virtualOrderDetail)
        {
            virtualOrderDetail.IsConfirmed = true;
            Update(virtualOrderDetail);
            return virtualOrderDetail;
        }

        public VirtualOrderDetail UnconfirmObject(VirtualOrderDetail virtualOrderDetail)
        {
            virtualOrderDetail.IsConfirmed = false;
            virtualOrderDetail.ConfirmationDate = null;
            UpdateObject(virtualOrderDetail);
            return virtualOrderDetail;
        }

        public string SetObjectCode(string ParentCode)
        {
            int totalnumberinthemonth = GetAllByMonthCreated().Count() + 1;
            string Code = ParentCode + "." + totalnumberinthemonth;
            return Code;
        } 

    }
}