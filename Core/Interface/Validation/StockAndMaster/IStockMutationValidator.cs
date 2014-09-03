﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IStockMutationValidator
    {
        StockMutation VHasItem(StockMutation stockMutation, IItemService _itemService);
        StockMutation VHasWarehouse(StockMutation stockMutation, IWarehouseService _warehouseService);
        StockMutation VHasWarehouseItem(StockMutation stockMutation, IWarehouseItemService _warehouseItemService);
        StockMutation VItemCase(StockMutation stockMutation);
        StockMutation VStatus(StockMutation stockMutation);
        StockMutation VSourceDocumentType(StockMutation stockMutation);
        StockMutation VSourceDocumentDetailType(StockMutation stockMutation);
        StockMutation VNonNegativeNorZeroQuantity(StockMutation stockMutation);
        StockMutation VCreateObject(StockMutation stockMutation, IWarehouseService _warehouseService, IWarehouseItemService _warehouseItemService, IItemService _itemService);
        StockMutation VUpdateObject(StockMutation stockMutation, IWarehouseService _warehouseService, IWarehouseItemService _warehouseItemService, IItemService _itemService);
        StockMutation VDeleteObject(StockMutation stockMutation, IWarehouseService _warehouseService, IWarehouseItemService _warehouseItemService);
        bool ValidCreateObject(StockMutation stockMutation, IWarehouseService _warehouseService, IWarehouseItemService _warehouseItemService, IItemService _itemService);
        bool ValidUpdateObject(StockMutation stockMutation, IWarehouseService _warehouseService, IWarehouseItemService _warehouseItemService, IItemService _itemService);
        bool ValidDeleteObject(StockMutation stockMutation, IWarehouseService _warehouseService, IWarehouseItemService _warehouseItemService);
        bool isValid(StockMutation stockMutation);
        string PrintError(StockMutation stockMutation);
    }
}
