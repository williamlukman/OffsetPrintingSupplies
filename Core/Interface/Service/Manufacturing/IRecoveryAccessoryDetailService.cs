using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IRecoveryAccessoryDetailService
    {
        IRecoveryAccessoryDetailValidator GetValidator();
        IRecoveryAccessoryDetailRepository GetRepository();
        IQueryable<RecoveryAccessoryDetail> GetQueryable();
        IList<RecoveryAccessoryDetail> GetAll();
        IList<RecoveryAccessoryDetail> GetObjectsByRecoveryOrderDetailId(int recoveryOrderDetailId);
        IList<RecoveryAccessoryDetail> GetObjectsByItemId(int ItemId);
        RecoveryAccessoryDetail GetObjectById(int Id);
        RecoveryAccessoryDetail CreateObject(RecoveryAccessoryDetail recoveryAccessoryDetail, IRecoveryOrderService _recoveryOrderService, IRecoveryOrderDetailService _recoveryOrderDetailService,
                                             IItemService _itemService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService);
        RecoveryAccessoryDetail CreateObject(int RecoveryOrderDetailId, int ItemId, int Quantity, IRecoveryOrderService _recoveryOrderService, IRecoveryOrderDetailService _recoveryOrderDetailService,
                                             IItemService _itemService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService);
        RecoveryAccessoryDetail UpdateObject(RecoveryAccessoryDetail recoveryAccessoryDetail, IRecoveryOrderService _recoveryOrderService, IRecoveryOrderDetailService _recoveryOrderDetailService,
                                             IItemService _itemService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService);
        RecoveryAccessoryDetail SoftDeleteObject(RecoveryAccessoryDetail recoveryAccessoryDetail, IRecoveryOrderDetailService _recoveryOrderDetailService);
        bool DeleteObject(int Id);
    }
}