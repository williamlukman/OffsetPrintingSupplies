using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class DeliveryOrderDetailRepository : EfRepository<DeliveryOrderDetail>, IDeliveryOrderDetailRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public DeliveryOrderDetailRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IList<DeliveryOrderDetail> GetObjectsByDeliveryOrderId(int deliveryOrderId)
        {
            return FindAll(dod => dod.DeliveryOrderId == deliveryOrderId && !dod.IsDeleted).ToList();
        }

        public DeliveryOrderDetail GetObjectById(int Id)
        {
            return Find(dod => dod.Id == Id && !dod.IsDeleted);
        }

        public DeliveryOrderDetail GetObjectBySalesOrderDetailId(int salesOrderDetailId)
        {
            DeliveryOrderDetail detail = Find(dod => dod.SalesOrderDetailId == salesOrderDetailId && !dod.IsDeleted);
            if (detail != null) { detail.Errors = new Dictionary<string, string>(); }
            return detail;
        }

        public DeliveryOrderDetail CreateObject(DeliveryOrderDetail deliveryOrderDetail)
        {
            string ParentCode = ""; 
            using (var db = GetContext())
            {
                ParentCode = (from obj in db.DeliveryOrders
                              where obj.Id == deliveryOrderDetail.DeliveryOrderId
                              select obj.Code).FirstOrDefault();
            }
            deliveryOrderDetail.Code = SetObjectCode(ParentCode);
            deliveryOrderDetail.IsFinished = false;
            deliveryOrderDetail.IsDeleted = false;
            deliveryOrderDetail.CreatedAt = DateTime.Now;
            return Create(deliveryOrderDetail);
        }

        public DeliveryOrderDetail UpdateObject(DeliveryOrderDetail deliveryOrderDetail)
        {
            deliveryOrderDetail.UpdatedAt = DateTime.Now;
            Update(deliveryOrderDetail);
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail SoftDeleteObject(DeliveryOrderDetail deliveryOrderDetail)
        {
            deliveryOrderDetail.IsDeleted = true;
            deliveryOrderDetail.DeletedAt = DateTime.Now;
            Update(deliveryOrderDetail);
            return deliveryOrderDetail;
        }

        public bool DeleteObject(int Id)
        {
            DeliveryOrderDetail dod = Find(x => x.Id == Id);
            return (Delete(dod) == 1) ? true : false;
        }

        public DeliveryOrderDetail FinishObject(DeliveryOrderDetail deliveryOrderDetail)
        {
            deliveryOrderDetail.IsFinished = true;
            deliveryOrderDetail.FinishDate = DateTime.Now;
            Update(deliveryOrderDetail);
            return deliveryOrderDetail;
        }

        public DeliveryOrderDetail UnfinishObject(DeliveryOrderDetail deliveryOrderDetail)
        {
            deliveryOrderDetail.IsFinished = false;
            deliveryOrderDetail.FinishDate = null;
            UpdateObject(deliveryOrderDetail);
            return deliveryOrderDetail;
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