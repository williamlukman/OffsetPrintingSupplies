using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IItemService
    {
        IItemValidator GetValidator();
        IItemRepository GetRepository();
        IList<Item> GetAll();
        IList<Item> GetAllAccessories(IItemService _itemService, IItemTypeService _itemTypeService);
        IList<Item> GetObjectsByItemTypeId(int ItemTypeId);
        IList<Item> GetObjectsByUoMId(int UoMId);
        Item GetObjectById(int Id);
        Item GetObjectBySku(string Sku);
        Item CreateObject(Item item, IUoMService _uomService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService,
                          IPriceMutationService _priceMutationService, IContactGroupService _contactGroupService);
        Item CreateLegacyObject(Item item, IUoMService _uomService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService,
                                IPriceMutationService _priceMutationService, IContactGroupService _contactGroupService);
        Item UpdateObject(Item item, IUoMService _uomService, IItemTypeService _itemTypeService, IPriceMutationService _priceMutationService, IContactGroupService _contactGroupService);
        Item UpdateLegacyObject(Item item, IUoMService _uomService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService,
                                       IBarringService _barringService, IContactService _contactService, IMachineService _machineService,
                                       IPriceMutationService _priceMutationService, IContactGroupService _contactGroupService);
        Item SoftDeleteObject(Item item, IStockMutationService _stockMutationService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService, IBarringService _barringService, IPurchaseOrderDetailService _purchaseOrderDetailService, IStockAdjustmentDetailService _stockAdjustmentDetailService, ISalesOrderDetailService _salesOrderDetailService, IPriceMutationService _priceMutationService);
        Item SoftDeleteLegacyObject(Item item, IStockMutationService _stockMutationService, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService, IBarringService _barringService, IPurchaseOrderDetailService _purchaseOrderDetailService, IStockAdjustmentDetailService _stockAdjustmentDetailService, ISalesOrderDetailService _salesOrderDetailService, IPriceMutationService _priceMutationService);
        Item AdjustQuantity(Item item, int quantity);
        Item AdjustPendingReceival(Item item, int quantity);
        Item AdjustPendingDelivery(Item item, int quantity);
        decimal CalculateAvgPrice(Item item, int addedQuantity, decimal addedAvgCost);
        bool DeleteObject(int Id);
        bool IsSkuDuplicated(Item item);
    }
}