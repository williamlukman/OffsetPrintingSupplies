using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IWarehouseMutationOrderDetailService
    {
        IQueryable<WarehouseMutationOrderDetail> GetQueryable();
        IWarehouseMutationOrderDetailValidator GetValidator();
        IList<WarehouseMutationOrderDetail> GetAll();
        IQueryable<WarehouseMutationOrderDetail> GetQueryableObjectsByWarehouseMutationOrderId(int warehouseMutationOrderId);
        IList<WarehouseMutationOrderDetail> GetObjectsByWarehouseMutationOrderId(int warehouseMutationOrderId);
        WarehouseMutationOrderDetail GetObjectById(int Id);
        WarehouseMutationOrderDetail CreateObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail, IWarehouseMutationOrderService _warehouseMutationOrderService,
                                                  IItemService _itemService, IWarehouseItemService _warehouseItemService);
        WarehouseMutationOrderDetail CreateObject(int warehouseMutationOrderId, int itemId, int quantity, IWarehouseMutationOrderService _warehouseMutationOrderService,
                                                  IItemService _itemService, IWarehouseItemService _warehouseItemService);
        WarehouseMutationOrderDetail UpdateObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail, IWarehouseMutationOrderService _warehouseMutationOrderService,
                                                  IItemService _itemService, IWarehouseItemService _warehouseItemService);
        WarehouseMutationOrderDetail SoftDeleteObject(WarehouseMutationOrderDetail warehouseMutationOrderDetail, IWarehouseMutationOrderService _warehouseMutationOrderService, IWarehouseItemService _warehouseItemService);
        WarehouseMutationOrderDetail ConfirmObject(WarehouseMutationOrderDetail WarehouseMutationOrderDetail, DateTime ConfirmationDate, IWarehouseMutationOrderService _warehouseMutationOrderService,
                                                   IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService, IStockMutationService _stockMutationService);
        WarehouseMutationOrderDetail UnconfirmObject(WarehouseMutationOrderDetail WarehouseMutationOrderDetail, IWarehouseMutationOrderService _warehouseMutationOrderService,
                                                     IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService, IStockMutationService _stockMutationService);
        bool DeleteObject(int Id);
    }
}