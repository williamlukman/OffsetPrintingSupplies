using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IBlendingWorkOrderRepository : IRepository<BlendingWorkOrder>
    {
        IQueryable<BlendingWorkOrder> GetQueryable();
        IList<BlendingWorkOrder> GetAll();
        IList<BlendingWorkOrder> GetObjectsByBlendingRecipeId(int BlendingRecipeId);
        BlendingWorkOrder GetObjectById(int Id);
        BlendingWorkOrder CreateObject(BlendingWorkOrder blendingWorkOrder);
        BlendingWorkOrder UpdateObject(BlendingWorkOrder blendingWorkOrder);
        BlendingWorkOrder SoftDeleteObject(BlendingWorkOrder blendingWorkOrder);
        BlendingWorkOrder ConfirmObject(BlendingWorkOrder blendingWorkOrder);
        BlendingWorkOrder UnconfirmObject(BlendingWorkOrder blendingWorkOrder);
        BlendingWorkOrder AdjustQuantity(BlendingWorkOrder blendingWorkOrder);
        bool DeleteObject(int Id);
        bool IsCodeDuplicated(BlendingWorkOrder blendingWorkOrder);
    }
}