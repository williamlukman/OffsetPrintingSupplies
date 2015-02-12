using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IBlanketOrderService
    {
        IBlanketOrderValidator GetValidator();
        IQueryable<BlanketOrder> GetQueryable();
        IList<BlanketOrder> GetAll();
        IList<BlanketOrder> GetAllObjectsByContactId(int ContactId);
        IList<BlanketOrder> GetAllObjectsByWarehouseId(int WarehouseId);
        BlanketOrder GetObjectById(int Id);
        BlanketOrder CreateObject(BlanketOrder blanketOrder);
        BlanketOrder UpdateObject(BlanketOrder blanketOrder, IBlanketOrderDetailService _blanketOrderDetailService);
        BlanketOrder UpdateAfterConfirmObject(BlanketOrder blanketOrder, IBlanketOrderDetailService _blanketOrderDetailService);
        BlanketOrder SoftDeleteObject(BlanketOrder blanketOrder, IBlanketOrderDetailService _blanketOrderDetailService);
        BlanketOrder ConfirmObject(BlanketOrder blanketOrder, DateTime ConfirmationDate, IBlanketOrderDetailService _blanketOrderDetailService, IBlanketService _blanketService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        BlanketOrder UnconfirmObject(BlanketOrder blanketOrder, IBlanketOrderDetailService _blanketOrderDetailService, IBlanketService _blanketService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        BlanketOrder CompleteObject(BlanketOrder blanketOrder, IBlanketOrderDetailService _blanketOrderDetailService, IBlanketService _blanketService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        BlanketOrder UndoCompleteObject(BlanketOrder blanketOrder, IBlanketOrderDetailService _blanketOrderDetailService, IBlanketService _blanketService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        BlanketOrder AdjustQuantity(BlanketOrder blanketOrder);
        bool DeleteObject(int Id);
        bool IsCodeDuplicated(BlanketOrder blanketOrder);
    }
}