﻿using Core.DomainModel;
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
    public class BarringOrderRepository : EfRepository<BarringOrder>, IBarringOrderRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public BarringOrderRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<BarringOrder> GetQueryable()
        {
            return FindAll();
        }

        public IList<BarringOrder> GetAll()
        {
            return FindAll().ToList();
        }

        public IList<BarringOrder> GetAllObjectsByContactId(int ContactId)
        {
            return FindAll(x => x.ContactId == ContactId && !x.IsDeleted).ToList();
        }

        public IList<BarringOrder> GetAllObjectsByWarehouseId(int WarehouseId)
        {
            return FindAll(x => x.WarehouseId == WarehouseId && !x.IsDeleted).ToList();
        }

        public BarringOrder GetObjectById(int Id)
        {
            BarringOrder barringOrder = Find(x => x.Id == Id && !x.IsDeleted);
            if (barringOrder != null) { barringOrder.Errors = new Dictionary<string, string>(); }
            return barringOrder;
        }

        public BarringOrder CreateObject(BarringOrder barringOrder)
        {
            barringOrder.QuantityFinal = 0;
            barringOrder.QuantityRejected = 0;
            barringOrder.IsConfirmed = false;
            barringOrder.IsCompleted = false;
            barringOrder.IsDeleted = false;
            barringOrder.CreatedAt = DateTime.Now;
            return Create(barringOrder);
        }

        public BarringOrder UpdateObject(BarringOrder barringOrder)
        {
            barringOrder.UpdatedAt = DateTime.Now;
            Update(barringOrder);
            return barringOrder;
        }

        public BarringOrder SoftDeleteObject(BarringOrder barringOrder)
        {
            barringOrder.IsDeleted = true;
            barringOrder.DeletedAt = DateTime.Now;
            Update(barringOrder);
            return barringOrder;
        }

        public BarringOrder ConfirmObject(BarringOrder barringOrder)
        {
            barringOrder.IsConfirmed = true;
            Update(barringOrder);
            return barringOrder;
        }

        public BarringOrder UnconfirmObject(BarringOrder barringOrder)
        {
            barringOrder.IsConfirmed = false;
            barringOrder.ConfirmationDate = null;
            UpdateObject(barringOrder);
            return barringOrder;
        }

        public BarringOrder CompleteObject(BarringOrder barringOrder)
        {
            barringOrder.IsCompleted = true;
            barringOrder.UpdatedAt = DateTime.Now;
            Update(barringOrder);
            return barringOrder;
        }

        public BarringOrder AdjustQuantity(BarringOrder barringOrder)
        {
            return UpdateObject(barringOrder);
        }

        public bool DeleteObject(int Id)
        {
            BarringOrder barringOrder = Find(x => x.Id == Id);
            return (Delete(barringOrder) == 1) ? true : false;
        }

        public bool IsCodeDuplicated(BarringOrder barringOrder)
        {
            IQueryable<BarringOrder> orders = FindAll(x => x.Code == barringOrder.Code && !x.IsDeleted && x.Id != barringOrder.Id);
            return (orders.Count() > 0 ? true : false);
        }

    }
}