using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IBarringOrderService
    {
        IBarringOrderValidator GetValidator();
        IList<BarringOrder> GetAll();
        IList<BarringOrder> GetAllObjectsByContactId(int ContactId);
        IList<BarringOrder> GetAllObjectsByWarehouseId(int WarehouseId);
        BarringOrder GetObjectById(int Id);
        BarringOrder CreateObject(BarringOrder barringOrder);
        BarringOrder UpdateObject(BarringOrder barringOrder, IBarringOrderDetailService _barringOrderDetailService);
        BarringOrder SoftDeleteObject(BarringOrder barringOrder, IBarringOrderDetailService _barringOrderDetailService);
        BarringOrder ConfirmObject(BarringOrder barringOrder, IBarringOrderDetailService _barringOrderDetailService, IBarringService _barringService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        BarringOrder UnconfirmObject(BarringOrder barringOrder, IBarringOrderDetailService _barringOrderDetailService, IBarringService _barringService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        BarringOrder CompleteObject(BarringOrder barringOrder, IBarringOrderDetailService _barringOrderDetailService, IBarringService _barringService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        BarringOrder AdjustQuantity(BarringOrder barringOrder);
        bool DeleteObject(int Id);
        bool IsCodeDuplicated(BarringOrder barringOrder);
    }
}