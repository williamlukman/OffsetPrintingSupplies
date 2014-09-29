using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IStockMutationService
    {
         IStockMutationValidator GetValidator();
         IQueryable<StockMutation> GetQueryable();
         IList<StockMutation> GetAll();
         IList<StockMutation> GetObjectsByItemId(int itemId);
         IList<StockMutation> GetObjectsByWarehouseId(int warehouseId);
         IList<StockMutation> GetObjectsByWarehouseItemId(int WarehouseId);
         StockMutation GetObjectById(int Id);
         IList<StockMutation> GetObjectsBySourceDocumentDetailForWarehouseItem(int warehouseItemId, string SourceDocumentDetailType, int SourceDocumentDetailId);
         IList<StockMutation> GetObjectsBySourceDocumentDetailForItem(int ItemId, string SourceDocumentDetailType, int SourceDocumentDetailId);
         StockMutation CreateObject(StockMutation stockMutation, IWarehouseService _warehouseService, IWarehouseItemService _warehouseItemService, IItemService _itemService, IBlanketService _blanketService);
         StockMutation UpdateObject(StockMutation stockMutation, IWarehouseService _warehouseService, IWarehouseItemService _warehouseItemService, IItemService _itemService, IBlanketService _blanketService);
         StockMutation SoftDeleteObject(StockMutation stockMutation, IWarehouseService _warehouseService, IWarehouseItemService _warehouseItemService, IItemService _itemService, IBlanketService _blanketService);
         bool DeleteObject(int Id);
         StockMutation CreateStockMutationForPurchaseOrder(PurchaseOrderDetail purchaseOrderDetail, Item item);
         IList<StockMutation> SoftDeleteStockMutationForPurchaseOrder(PurchaseOrderDetail purchaseOrderDetail, Item item);
         IList<StockMutation> CreateStockMutationForPurchaseReceival(PurchaseReceivalDetail purchaseReceivalDetail, WarehouseItem warehouseItem);
         IList<StockMutation> SoftDeleteStockMutationForPurchaseReceival(PurchaseReceivalDetail purchaseReceivalDetail, WarehouseItem warehouseItem);
         StockMutation CreateStockMutationForSalesOrder(SalesOrderDetail salesOrderDetail, Item item);
         IList<StockMutation> SoftDeleteStockMutationForSalesOrder(SalesOrderDetail salesOrderDetail, Item item);
         StockMutation CreateStockMutationForVirtualOrder(VirtualOrderDetail virtualOrderDetail, Item item);
         IList<StockMutation> SoftDeleteStockMutationForVirtualOrder(VirtualOrderDetail virtualOrderDetail, Item item);
         IList<StockMutation> CreateStockMutationForDeliveryOrder(DeliveryOrderDetail deliveryOrderDetail, WarehouseItem warehouseItem);
         IList<StockMutation> SoftDeleteStockMutationForDeliveryOrder(DeliveryOrderDetail deliveryOrderDetail, WarehouseItem warehouseItem);
         IList<StockMutation> CreateStockMutationForTemporaryDeliveryOrder(TemporaryDeliveryOrderDetail deliveryOrderDetail, WarehouseItem warehouseItem);
         IList<StockMutation> SoftDeleteStockMutationForTemporaryDeliveryOrder(TemporaryDeliveryOrderDetail deliveryOrderDetail, WarehouseItem warehouseItem);
         StockMutation CreateStockMutationForStockAdjustment(StockAdjustmentDetail stockAdjustmentDetail, WarehouseItem warehouseItem);
         IList<StockMutation> SoftDeleteStockMutationForStockAdjustment(StockAdjustmentDetail stockAdjustmentDetail, WarehouseItem warehouseItem);
         StockMutation CreateStockMutationForCoreIdentification(CoreIdentificationDetail coreIdentificationDetail, WarehouseItem warehouseItem);
         IList<StockMutation> SoftDeleteStockMutationForCoreIdentification(CoreIdentificationDetail coreIdentificationDetail, WarehouseItem warehouseItem);
         StockMutation CreateStockMutationForRecoveryOrder(RecoveryOrderDetail recoveryOrderDetail, WarehouseItem warehouseItem, bool CaseAddition);
         StockMutation CreateStockMutationForRecoveryOrderCompound(RecoveryOrderDetail recoveryOrderDetail, WarehouseItem warehouseItem, bool CaseAddition);
         IList<StockMutation> SoftDeleteStockMutationForRecoveryOrder(RecoveryOrderDetail recoveryOrderDetail, WarehouseItem warehouseItem);
         StockMutation CreateStockMutationForRecoveryAccessory(RecoveryAccessoryDetail recoveryAccessoryDetail, WarehouseItem warehouseItem);
         IList<StockMutation> SoftDeleteStockMutationForRecoveryAccessory(RecoveryAccessoryDetail recoveryAccessoryDetail, WarehouseItem warehouseItem);
         StockMutation CreateStockMutationForBlanketOrder(BlanketOrderDetail blanketOrderDetail, WarehouseItem warehouseItem, bool CaseAddition);
         IList<StockMutation> SoftDeleteStockMutationForBlanketOrder(BlanketOrderDetail blanketOrderDetail, WarehouseItem warehouseItem);
         IList<StockMutation> CreateStockMutationForRollerWarehouseMutation(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, WarehouseItem warehouseItemFrom, WarehouseItem warehouseItemTo);
         IList<StockMutation> SoftDeleteStockMutationForRollerWarehouseMutation(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, WarehouseItem warehouseItemFrom, WarehouseItem warehouseItemTo);
         IList<StockMutation> CreateStockMutationForWarehouseMutation(WarehouseMutationDetail WarehouseMutationDetail, WarehouseItem warehouseItemFrom, WarehouseItem warehouseItemTo);
         IList<StockMutation> SoftDeleteStockMutationForWarehouseMutation(WarehouseMutationDetail WarehouseMutationDetail, WarehouseItem warehouseItemFrom, WarehouseItem warehouseItemTo);
         void StockMutateObject(StockMutation stockMutation, IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService);
         void ReverseStockMutateObject(StockMutation stockMutation, IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService);
    }
}
