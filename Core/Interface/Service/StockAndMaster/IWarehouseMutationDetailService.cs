﻿using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IWarehouseMutationDetailService
    {
        IWarehouseMutationDetailValidator GetValidator();
        IQueryable<WarehouseMutationDetail> GetQueryable();
        IList<WarehouseMutationDetail> GetAll();
        IList<WarehouseMutationDetail> GetObjectsByWarehouseMutationId(int warehouseMutationId);
        WarehouseMutationDetail GetObjectById(int Id);
        WarehouseMutationDetail CreateObject(WarehouseMutationDetail warehouseMutationDetail, IWarehouseMutationService _warehouseMutationService,
                                             IItemService _itemService, IWarehouseItemService _warehouseItemService, IBlanketService _blanketService);
        WarehouseMutationDetail UpdateObject(WarehouseMutationDetail warehouseMutationDetail, IWarehouseMutationService _warehouseMutationService,
                                             IItemService _itemService, IWarehouseItemService _warehouseItemService, IBlanketService _blanketService);
        WarehouseMutationDetail SoftDeleteObject(WarehouseMutationDetail warehouseMutationDetail, IWarehouseMutationService _warehouseMutationService, IWarehouseItemService _warehouseItemService);
        WarehouseMutationDetail ConfirmObject(WarehouseMutationDetail WarehouseMutationDetail, DateTime ConfirmationDate, IWarehouseMutationService _warehouseMutationService,
                                                   IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService, IStockMutationService _stockMutationService);
        WarehouseMutationDetail UnconfirmObject(WarehouseMutationDetail WarehouseMutationDetail, IWarehouseMutationService _warehouseMutationService,
                                                     IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService, IStockMutationService _stockMutationService);
        bool DeleteObject(int Id);
    }
}