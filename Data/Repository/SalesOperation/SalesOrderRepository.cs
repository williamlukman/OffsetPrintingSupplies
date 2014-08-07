using Core.DomainModel;
using Core.Interface.Repository;
using Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Repository
{
    public class SalesOrderRepository : EfRepository<SalesOrder>, ISalesOrderRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public SalesOrderRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IList<SalesOrder> GetAll()
        {
            return FindAll(so => !so.IsDeleted).ToList();
        }

        public SalesOrder GetObjectById(int Id)
        {
            SalesOrder salesOrder = Find(so => so.Id == Id && !so.IsDeleted);
            if (salesOrder != null) { salesOrder.Errors = new Dictionary<string, string>(); }
            return salesOrder;
        }

        public IList<SalesOrder> GetObjectsByContactId(int contactId)
        {
            return FindAll(so => so.ContactId == contactId && !so.IsDeleted).ToList();
        }

        public SalesOrder CreateObject(SalesOrder salesOrder)
        {
            salesOrder.Code = SetObjectCode();
            salesOrder.IsDeleted = false;
            salesOrder.IsCompleted = false;
            salesOrder.IsConfirmed = false;
            salesOrder.CreatedAt = DateTime.Now;
            return Create(salesOrder);
        }

        public SalesOrder UpdateObject(SalesOrder salesOrder)
        {
            salesOrder.UpdatedAt = DateTime.Now;
            Update(salesOrder);
            return salesOrder;
        }

        public SalesOrder SoftDeleteObject(SalesOrder salesOrder)
        {
            salesOrder.IsDeleted = true;
            salesOrder.DeletedAt = DateTime.Now;
            Update(salesOrder);
            return salesOrder;
        }

        public bool DeleteObject(int Id)
        {
            SalesOrder so = Find(x => x.Id == Id);
            return (Delete(so) == 1) ? true : false;
        }

        public SalesOrder ConfirmObject(SalesOrder salesOrder)
        {
            salesOrder.IsConfirmed = true;
            salesOrder.ConfirmationDate = DateTime.Now;
            Update(salesOrder);
            return salesOrder;
        }

        public SalesOrder UnconfirmObject(SalesOrder salesOrder)
        {
            salesOrder.IsConfirmed = false;
            salesOrder.ConfirmationDate = null;
            UpdateObject(salesOrder);
            return salesOrder;
        }

        public SalesOrder CompleteObject(SalesOrder salesOrder)
        {
            salesOrder.IsCompleted = true;
            UpdateObject(salesOrder);
            return salesOrder;
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