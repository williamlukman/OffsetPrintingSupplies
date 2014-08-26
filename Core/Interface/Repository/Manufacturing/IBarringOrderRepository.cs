using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IBarringOrderRepository : IRepository<BarringOrder>
    {
        IQueryable<BarringOrder> GetQueryable();
        IList<BarringOrder> GetAll();
        IList<BarringOrder> GetAllObjectsByContactId(int ContactId);
        IList<BarringOrder> GetAllObjectsByWarehouseId(int WarehouseId);
        BarringOrder GetObjectById(int Id);
        BarringOrder CreateObject(BarringOrder barringOrder);
        BarringOrder UpdateObject(BarringOrder barringOrder);
        BarringOrder SoftDeleteObject(BarringOrder barringOrder);
        BarringOrder ConfirmObject(BarringOrder barringOrder);
        BarringOrder UnconfirmObject(BarringOrder barringOrder);
        BarringOrder CompleteObject(BarringOrder barringOrder);
        BarringOrder AdjustQuantity(BarringOrder barringOrder);
        bool DeleteObject(int Id);
        bool IsCodeDuplicated(BarringOrder barringOrder);
    }
}