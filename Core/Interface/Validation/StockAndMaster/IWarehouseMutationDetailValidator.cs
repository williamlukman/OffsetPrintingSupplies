﻿using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface IWarehouseMutationDetailValidator
    {
        WarehouseMutationDetail VHasWarehouseMutation(WarehouseMutationDetail warehouseMutationDetail, IWarehouseMutationService _warehouseMutationService);
        WarehouseMutationDetail VHasWarehouseItemFrom(WarehouseMutationDetail warehouseMutationDetail, IWarehouseMutationService _warehouseMutationService, IWarehouseItemService _warehouseItemService);
        WarehouseMutationDetail VHasWarehouseItemTo(WarehouseMutationDetail warehouseMutationDetail, IWarehouseMutationService _warehouseMutationService, IWarehouseItemService _warehouseItemService);
        WarehouseMutationDetail VUniqueItem(WarehouseMutationDetail warehouseMutationDetail, IWarehouseMutationDetailService _warehouseMutationDetailService, IItemService _itemService);
        WarehouseMutationDetail VNonNegativeStockQuantity(WarehouseMutationDetail warehouseMutationDetail, IWarehouseMutationService _warehouseMutationService,
                                                               IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService, bool CaseConfirmOrConfirm);
        WarehouseMutationDetail VWarehouseMutationHasBeenConfirmed(WarehouseMutationDetail warehouseMutationDetail, IWarehouseMutationService _warehouseMutationService);
        WarehouseMutationDetail VHasNotBeenConfirmed(WarehouseMutationDetail warehouseMutationDetail);
        WarehouseMutationDetail VHasBeenConfirmed(WarehouseMutationDetail warehouseMutationDetail);
        WarehouseMutationDetail VCreateObject(WarehouseMutationDetail warehouseMutationDetail, IWarehouseMutationService _warehouseMutationService,
                                                   IWarehouseMutationDetailService _warehouseMutationDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        WarehouseMutationDetail VUpdateObject(WarehouseMutationDetail warehouseMutationDetail, IWarehouseMutationService _warehouseMutationService,
                                                   IWarehouseMutationDetailService _warehouseMutationDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        WarehouseMutationDetail VDeleteObject(WarehouseMutationDetail warehouseMutationDetail);
        WarehouseMutationDetail VHasConfirmationDate(WarehouseMutationDetail warehouseMutationDetail);
        WarehouseMutationDetail VConfirmObject(WarehouseMutationDetail warehouseMutationDetail, IWarehouseMutationService _warehouseMutationService,
                                                    IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService);
        WarehouseMutationDetail VUnconfirmObject(WarehouseMutationDetail warehouseMutationDetail, IWarehouseMutationService _warehouseMutationService,
                                                      IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService);
        bool ValidCreateObject(WarehouseMutationDetail warehouseMutationDetail, IWarehouseMutationService _warehouseMutationService,
                               IWarehouseMutationDetailService _warehouseMutationDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        bool ValidUpdateObject(WarehouseMutationDetail warehouseMutationDetail, IWarehouseMutationService _warehouseMutationService,
                               IWarehouseMutationDetailService _warehouseMutationDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        bool ValidConfirmObject(WarehouseMutationDetail warehouseMutationDetail, IWarehouseMutationService _warehouseMutationService,
                                IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService);
        bool ValidUnconfirmObject(WarehouseMutationDetail warehouseMutationDetail, IWarehouseMutationService _warehouseMutationService,
                                IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService);
        bool ValidDeleteObject(WarehouseMutationDetail warehouseMutationDetail);
        bool isValid(WarehouseMutationDetail warehouseMutationDetail);
        string PrintError(WarehouseMutationDetail warehouseMutationDetail);
    }
}