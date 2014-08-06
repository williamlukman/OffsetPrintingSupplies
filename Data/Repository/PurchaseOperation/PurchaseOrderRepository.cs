using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repository
{
    public class PurchaseOrderRepository : EfRepository<PurchaseOrder>, IPurchaseOrderRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public PurchaseOrderRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IList<PurchaseOrder> GetAll()
        {
            return FindAll(po => !po.IsDeleted).ToList();
        }

        public PurchaseOrder GetObjectById(int Id)
        {
            PurchaseOrder purchaseOrder = Find(po => po.Id == Id && !po.IsDeleted);
            if (purchaseOrder != null) { purchaseOrder.Errors = new Dictionary<string, string>(); }
            return purchaseOrder;
        }

        public IList<PurchaseOrder> GetObjectsByCustomerId(int customerId)
        {
            return FindAll(po => po.CustomerId == customerId && !po.IsDeleted).ToList();
        }

        public PurchaseOrder CreateObject(PurchaseOrder purchaseOrder)
        {
            purchaseOrder.Code = SetObjectCode();
            purchaseOrder.IsDeleted = false;
            purchaseOrder.IsCompleted = false;
            purchaseOrder.IsConfirmed = false;
            purchaseOrder.CreatedAt = DateTime.Now;
            return Create(purchaseOrder);
        }

        public PurchaseOrder UpdateObject(PurchaseOrder purchaseOrder)
        {
            purchaseOrder.UpdatedAt = DateTime.Now;
            Update(purchaseOrder);
            return purchaseOrder;
        }

        public PurchaseOrder SoftDeleteObject(PurchaseOrder purchaseOrder)
        {
            purchaseOrder.IsDeleted = true;
            purchaseOrder.DeletedAt = DateTime.Now;
            Update(purchaseOrder);
            return purchaseOrder;
        }

        public bool DeleteObject(int Id)
        {
            PurchaseOrder po = Find(x => x.Id == Id);
            return (Delete(po) == 1) ? true : false;
        }

        public PurchaseOrder ConfirmObject(PurchaseOrder purchaseOrder)
        {
            purchaseOrder.IsConfirmed = true;
            purchaseOrder.ConfirmationDate = DateTime.Now;
            Update(purchaseOrder);
            return purchaseOrder;
        }

        public PurchaseOrder UnconfirmObject(PurchaseOrder purchaseOrder)
        {
            purchaseOrder.IsConfirmed = false;
            UpdateObject(purchaseOrder);
            return purchaseOrder;
        }

        public PurchaseOrder CompleteObject(PurchaseOrder purchaseOrder)
        {
            purchaseOrder.IsCompleted = true;
            UpdateObject(purchaseOrder);
            return purchaseOrder;
        }

        public string SetObjectCode()
        {
            // Code: #{year}/#{total_number
            int totalobject = FindAll().Count() + 1;
            string Code = "#" + DateTime.Now.Year.ToString() + "/#" + totalobject;
            return Code;
        }
    }
}