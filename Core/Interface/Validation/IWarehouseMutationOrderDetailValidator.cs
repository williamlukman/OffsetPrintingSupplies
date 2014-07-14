using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface IWarehouseMutationOrderDetailValidator
    {
        WarehouseMutationOrderDetail VHasMutationOrder(WarehouseMutationOrderDetail warehouseMutationOrderDetail, IWarehouseMutationOrderService _warehouseMutationOrderService);
        WarehouseMutationOrderDetail VHasUniqueAbstractItemId(WarehouseMutationOrderDetail warehouseMutationOrderDetail, IWarehouseMutationOrderDetailService _warehouseMutationOrderDetailService, IItemService _itemService);
        WarehouseMutationOrderDetail VIsUnconfirmedWarehouseMutationOrder(WarehouseMutationOrderDetail warehouseMutationOrderDetail, IWarehouseMutationOrderService _warehouseMutationOrderService);
        WarehouseMutationOrderDetail VCreateObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail, IWarehouseService _warehouseService);
        WarehouseMutationOrderDetail VUpdateObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail, IWarehouseService _warehouseService);
        WarehouseMutationOrderDetail VDeleteObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail, IWarehouseItemService _warehouseItemService, IWarehouseMutationOrderService _warehouseMutationOrderService);
        bool ValidCreateObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail, IWarehouseMutationOrderService _warehouseMutationOrderService, IWarehouseMutationOrderDetailService _warehouseMutationOrderDetailService, IItemService _itemService);
        bool ValidUpdateObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail, IWarehouseMutationOrderService _warehouseMutationOrderService, IWarehouseMutationOrderDetailService _warehouseMutationOrderDetailService, IItemService _itemService);
        bool ValidDeleteObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail, IWarehouseItemService _warehouseItemService, IWarehouseMutationOrderService _warehouseMutationOrderService);
        bool isValid(WarehouseMutationOrderDetail warehouseMutationOrderDetail);
        string PrintError(WarehouseMutationOrderDetail warehouseMutationOrderDetail);
    }
}