using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IWarehouseMutationService
    {
        IWarehouseMutationValidator GetValidator();
        IQueryable<WarehouseMutation> GetQueryable();
        IList<WarehouseMutation> GetAll();
        WarehouseMutation GetObjectById(int Id);
        Warehouse GetWarehouseFrom(WarehouseMutation warehouseMutation);
        Warehouse GetWarehouseTo(WarehouseMutation warehouseMutation);
        WarehouseMutation CreateObject(WarehouseMutation warehouseMutation, IWarehouseService _warehouseService);
        WarehouseMutation UpdateObject(WarehouseMutation warehouseMutation, IWarehouseService _warehouseService);
        WarehouseMutation SoftDeleteObject(WarehouseMutation warehouseMutation);
        bool DeleteObject(int Id);
        WarehouseMutation ConfirmObject(WarehouseMutation warehouseMutation, DateTime ConfirmationDate, IWarehouseMutationDetailService _warehouseMutationDetailService,
                                             IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService, IStockMutationService _stockMutationService);
        WarehouseMutation UnconfirmObject(WarehouseMutation warehouseMutation, IWarehouseMutationDetailService _warehouseMutationDetailService,
                                               IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService, IStockMutationService _stockMutationService);
    }
}