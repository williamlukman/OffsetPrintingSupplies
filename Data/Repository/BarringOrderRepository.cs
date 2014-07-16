﻿using Core.DomainModel;
using Core.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public IList<BarringOrder> GetAll()
        {
            return FindAll().ToList();
        }

        public IList<BarringOrder> GetAllObjectsByCustomerId(int CustomerId)
        {
            return FindAll(x => x.CustomerId == CustomerId && !x.IsDeleted).ToList();
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
            barringOrder.IsFinished = false;
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
            barringOrder.ConfirmationDate = DateTime.Now;
            Update(barringOrder);
            return barringOrder;
        }

        public BarringOrder UnconfirmObject(BarringOrder barringOrder)
        {
            barringOrder.IsConfirmed = false;
            barringOrder.ConfirmationDate = null;
            Update(barringOrder);
            return barringOrder;
        }

        public BarringOrder FinishObject(BarringOrder barringOrder)
        {
            barringOrder.IsCompleted = true;
            barringOrder.IsFinished = true;
            barringOrder.FinishDate = DateTime.Now;
            Update(barringOrder);
            return barringOrder;
        }

        public BarringOrder UnfinishObject(BarringOrder barringOrder)
        {
            barringOrder.IsCompleted = false;
            barringOrder.IsFinished = false;
            Update(barringOrder);
            return barringOrder;
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