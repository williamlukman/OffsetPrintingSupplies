﻿using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IItemService
    {
        IItemValidator GetValidator();
        IItemRepository GetRepository();
        IQueryable<Item> GetQueryable();
        IQueryable<Item> GetQueryableAccessories(IItemService _itemService, IItemTypeService _itemTypeService);
        IList<Item> GetAll();
        IList<Item> GetAllAccessories(IItemService _itemService, IItemTypeService _itemTypeService);
        IList<Item> GetObjectsByItemTypeId(int ItemTypeId);
        IList<Item> GetObjectsBySubTypeId(int SubTypeId);
        IList<Item> GetObjectsByUoMId(int UoMId);
        Item GetObjectById(int Id);
        Item GetObjectBySku(string Sku);
        Item CreateObject(Item item, IUoMService _uomService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService,
                          IPriceMutationService _priceMutationService);
        Item CreateLegacyObject(Item item, IUoMService _uomService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService,
                                IPriceMutationService _priceMutationService);
        Item UpdateObject(Item item, IUoMService _uomService, IItemTypeService _itemTypeService, IPriceMutationService _priceMutationService);
        Item UpdateLegacyObject(Item item, IUoMService _uomService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService,
                                IBlanketService _blanketService, IContactService _contactService, IMachineService _machineService,
                                IPriceMutationService _priceMutationService);
        Item SoftDeleteObject(Item item, IStockMutationService _stockMutationService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService,
                              IBlanketService _blanketService, IPurchaseOrderDetailService _purchaseOrderDetailService, IStockAdjustmentDetailService _stockAdjustmentDetailService,
                              ISalesOrderDetailService _salesOrderDetailService, IPriceMutationService _priceMutationService);
        Item SoftDeleteLegacyObject(Item item, IStockMutationService _stockMutationService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService,
                                    IBlanketService _blanketService, IPurchaseOrderDetailService _purchaseOrderDetailService,
                                    IStockAdjustmentDetailService _stockAdjustmentDetailService, ISalesOrderDetailService _salesOrderDetailService,
                                    IPriceMutationService _priceMutationService, IBlanketOrderDetailService _blanketOrderDetailService);
        Item AdjustCustomerQuantity(Item item, decimal quantity);
        //Item AdjustCustomerVirtual(Item item, int quantity);
        Item AdjustQuantity(Item item, decimal quantity);
        Item AdjustPendingReceival(Item item, decimal quantity);
        Item AdjustPendingDelivery(Item item, decimal quantity);
        Item AdjustVirtual(Item item, decimal quantity);
        decimal CalculateAvgPrice(Item item, decimal addedQuantity, decimal addedAvgPrice);
        decimal CalculateAndUpdateAvgPrice(Item item, decimal addedQuantity, decimal addedAvgPrice);
        decimal CalculateCustomerAvgPrice(Item item, decimal addedQuantity, decimal addedAvgPrice);
        decimal CalculateAndUpdateCustomerAvgPrice(Item item, decimal addedQuantity, decimal addedAvgPrice);
        bool DeleteObject(int Id);
        bool IsSkuDuplicated(Item item);
    }
}