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
    public class BlendingWorkOrderRepository : EfRepository<BlendingWorkOrder>, IBlendingWorkOrderRepository
    {
        private OffsetPrintingSuppliesEntities entities;
        public BlendingWorkOrderRepository()
        {
            entities = new OffsetPrintingSuppliesEntities();
        }

        public IQueryable<BlendingWorkOrder> GetQueryable()
        {
            return FindAll();
        }

        public IList<BlendingWorkOrder> GetAll()
        {
            return FindAll().ToList();
        }

        public IList<BlendingWorkOrder> GetObjectsByBlendingRecipeId(int BlendingRecipeId)
        {
            return FindAll(x => x.BlendingRecipeId == BlendingRecipeId && !x.IsDeleted).ToList();
        }

        public BlendingWorkOrder GetObjectById(int Id)
        {
            BlendingWorkOrder blendingWorkOrder = Find(x => x.Id == Id && !x.IsDeleted);
            if (blendingWorkOrder != null) { blendingWorkOrder.Errors = new Dictionary<string, string>(); }
            return blendingWorkOrder;
        }

        public BlendingWorkOrder CreateObject(BlendingWorkOrder blendingWorkOrder)
        {
            blendingWorkOrder.IsConfirmed = false;
            blendingWorkOrder.IsDeleted = false;
            blendingWorkOrder.CreatedAt = DateTime.Now;
            return Create(blendingWorkOrder);
        }

        public BlendingWorkOrder UpdateObject(BlendingWorkOrder blendingWorkOrder)
        {
            blendingWorkOrder.UpdatedAt = DateTime.Now;
            Update(blendingWorkOrder);
            return blendingWorkOrder;
        }

        public BlendingWorkOrder SoftDeleteObject(BlendingWorkOrder blendingWorkOrder)
        {
            blendingWorkOrder.IsDeleted = true;
            blendingWorkOrder.DeletedAt = DateTime.Now;
            Update(blendingWorkOrder);
            return blendingWorkOrder;
        }

        public BlendingWorkOrder ConfirmObject(BlendingWorkOrder blendingWorkOrder)
        {
            blendingWorkOrder.IsConfirmed = true;
            Update(blendingWorkOrder);
            return blendingWorkOrder;
        }

        public BlendingWorkOrder UnconfirmObject(BlendingWorkOrder blendingWorkOrder)
        {
            blendingWorkOrder.IsConfirmed = false;
            blendingWorkOrder.ConfirmationDate = null;
            UpdateObject(blendingWorkOrder);
            return blendingWorkOrder;
        }

        public BlendingWorkOrder AdjustQuantity(BlendingWorkOrder blendingWorkOrder)
        {
            return UpdateObject(blendingWorkOrder);
        }

        public bool DeleteObject(int Id)
        {
            BlendingWorkOrder blendingWorkOrder = Find(x => x.Id == Id);
            return (Delete(blendingWorkOrder) == 1) ? true : false;
        }

        public bool IsCodeDuplicated(BlendingWorkOrder blendingWorkOrder)
        {
            IQueryable<BlendingWorkOrder> orders = FindAll(x => x.Code == blendingWorkOrder.Code && !x.IsDeleted && x.Id != blendingWorkOrder.Id);
            return (orders.Count() > 0 ? true : false);
        }

    }
}