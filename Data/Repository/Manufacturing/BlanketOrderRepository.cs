using Core.DomainModel;
using Core.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Context;
using Data.Repository;
using System.Data;

namespace Data.Repository
{
    public class BlanketOrderRepository : EfRepository<BlanketOrder>, IBlanketOrderRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public BlanketOrderRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<BlanketOrder> GetQueryable()
        {
            return FindAll();
        }

        public IList<BlanketOrder> GetAll()
        {
            return FindAll().ToList();
        }

        public IList<BlanketOrder> GetAllObjectsByContactId(int ContactId)
        {
            return FindAll(x => x.ContactId == ContactId && !x.IsDeleted).ToList();
        }

        public IList<BlanketOrder> GetAllObjectsByWarehouseId(int WarehouseId)
        {
            return FindAll(x => x.WarehouseId == WarehouseId && !x.IsDeleted).ToList();
        }

        public BlanketOrder GetObjectById(int Id)
        {
            BlanketOrder blanketOrder = Find(x => x.Id == Id && !x.IsDeleted);
            if (blanketOrder != null) { blanketOrder.Errors = new Dictionary<string, string>(); }
            return blanketOrder;
        }

        public BlanketOrder CreateObject(BlanketOrder blanketOrder)
        {
            blanketOrder.QuantityFinal = 0;
            blanketOrder.QuantityRejected = 0;
            blanketOrder.IsConfirmed = false;
            blanketOrder.IsCompleted = false;
            blanketOrder.IsDeleted = false;
            blanketOrder.CreatedAt = DateTime.Now;
            return Create(blanketOrder);
        }

        public BlanketOrder UpdateObject(BlanketOrder blanketOrder)
        {
            blanketOrder.UpdatedAt = DateTime.Now;
            Update(blanketOrder);
            return blanketOrder;
        }

        public BlanketOrder SoftDeleteObject(BlanketOrder blanketOrder)
        {
            blanketOrder.IsDeleted = true;
            blanketOrder.DeletedAt = DateTime.Now;
            Update(blanketOrder);
            return blanketOrder;
        }

        public BlanketOrder ConfirmObject(BlanketOrder blanketOrder)
        {
            blanketOrder.IsConfirmed = true;
            Update(blanketOrder);
            return blanketOrder;
        }

        public BlanketOrder UnconfirmObject(BlanketOrder blanketOrder)
        {
            blanketOrder.IsConfirmed = false;
            blanketOrder.ConfirmationDate = null;
            UpdateObject(blanketOrder);
            return blanketOrder;
        }

        public BlanketOrder CompleteObject(BlanketOrder blanketOrder)
        {
            blanketOrder.IsCompleted = true;
            blanketOrder.UpdatedAt = DateTime.Now;
            Update(blanketOrder);
            return blanketOrder;
        }

        public BlanketOrder UndoCompleteObject(BlanketOrder blanketOrder)
        {
            blanketOrder.IsCompleted = false;
            blanketOrder.UpdatedAt = DateTime.Now;
            Update(blanketOrder);
            return blanketOrder;
        }

        public BlanketOrder AdjustQuantity(BlanketOrder blanketOrder)
        {
            return UpdateObject(blanketOrder);
        }

        public bool DeleteObject(int Id)
        {
            BlanketOrder blanketOrder = Find(x => x.Id == Id);
            return (Delete(blanketOrder) == 1) ? true : false;
        }

        public bool IsCodeDuplicated(BlanketOrder blanketOrder)
        {
            IQueryable<BlanketOrder> orders = FindAll(x => x.Code == blanketOrder.Code && !x.IsDeleted && x.Id != blanketOrder.Id);
            return (orders.Count() > 0 ? true : false);
        }

    }
}