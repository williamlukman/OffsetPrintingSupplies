using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IBlanketOrderRepository : IRepository<BlanketOrder>
    {
        IQueryable<BlanketOrder> GetQueryable();
        IList<BlanketOrder> GetAll();
        IList<BlanketOrder> GetAllObjectsByContactId(int ContactId);
        IList<BlanketOrder> GetAllObjectsByWarehouseId(int WarehouseId);
        BlanketOrder GetObjectById(int Id);
        BlanketOrder CreateObject(BlanketOrder blanketOrder);
        BlanketOrder UpdateObject(BlanketOrder blanketOrder);
        BlanketOrder SoftDeleteObject(BlanketOrder blanketOrder);
        BlanketOrder ConfirmObject(BlanketOrder blanketOrder);
        BlanketOrder UnconfirmObject(BlanketOrder blanketOrder);
        BlanketOrder CompleteObject(BlanketOrder blanketOrder);
        BlanketOrder UndoCompleteObject(BlanketOrder blanketOrder);
        BlanketOrder AdjustQuantity(BlanketOrder blanketOrder);
        bool DeleteObject(int Id);
        bool IsCodeDuplicated(BlanketOrder blanketOrder);
    }
}