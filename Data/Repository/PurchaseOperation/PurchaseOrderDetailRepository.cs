using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repository
{
    public class PurchaseOrderDetailRepository : EfRepository<PurchaseOrderDetail>, IPurchaseOrderDetailRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public PurchaseOrderDetailRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IList<PurchaseOrderDetail> GetObjectsByPurchaseOrderId(int purchaseOrderId)
        {
            return FindAll(pod => pod.PurchaseOrderId == purchaseOrderId && !pod.IsDeleted).ToList();
        }

        public PurchaseOrderDetail GetObjectById(int Id)
        {
            PurchaseOrderDetail detail = Find(pod => pod.Id == Id);
            if (detail != null) { detail.Errors = new Dictionary<string, string>(); }
            return detail;
        }

        public PurchaseOrderDetail CreateObject(PurchaseOrderDetail purchaseOrderDetail)
        {
            string ParentCode = "";
            using (var db = GetContext())
            {
                ParentCode = (from obj in db.PurchaseOrders
                              where obj.Id == purchaseOrderDetail.PurchaseOrderId
                              select obj.Code).FirstOrDefault();
            }
            purchaseOrderDetail.Code = SetObjectCode(ParentCode);
            purchaseOrderDetail.IsFinished = false;
            purchaseOrderDetail.IsDeleted = false;
            purchaseOrderDetail.CreatedAt = DateTime.Now;
            return Create(purchaseOrderDetail);
        }

        public PurchaseOrderDetail UpdateObject(PurchaseOrderDetail purchaseOrderDetail)
        {
            purchaseOrderDetail.UpdatedAt = DateTime.Now;
            Update(purchaseOrderDetail);
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail SoftDeleteObject(PurchaseOrderDetail purchaseOrderDetail)
        {
            purchaseOrderDetail.IsDeleted = true;
            purchaseOrderDetail.DeletedAt = DateTime.Now;
            Update(purchaseOrderDetail);
            return purchaseOrderDetail;
        }

        public bool DeleteObject(int Id)
        {
            PurchaseOrderDetail pod = Find(x => x.Id == Id);
            return (Delete(pod) == 1) ? true : false;
        }

        public PurchaseOrderDetail FinishObject(PurchaseOrderDetail purchaseOrderDetail)
        {
            purchaseOrderDetail.IsFinished = true;
            purchaseOrderDetail.FinishDate = DateTime.Now;
            Update(purchaseOrderDetail);
            return purchaseOrderDetail;
        }

        public PurchaseOrderDetail UnfinishObject(PurchaseOrderDetail purchaseOrderDetail)
        {
            purchaseOrderDetail.IsFinished = false;
            purchaseOrderDetail.FinishDate = null;
            UpdateObject(purchaseOrderDetail);
            return purchaseOrderDetail;
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