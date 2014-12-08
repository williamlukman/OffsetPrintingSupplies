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
         int DeleteStockMutations(IList<StockMutation> stockMutations); 
         IList<StockMutation> DeleteStockMutationForPurchaseOrder(PurchaseOrderDetail purchaseOrderDetail, Item item);
         IList<StockMutation> CreateStockMutationForPurchaseReceival(PurchaseReceivalDetail purchaseReceivalDetail, WarehouseItem warehouseItem);
         IList<StockMutation> DeleteStockMutationForPurchaseReceival(PurchaseReceivalDetail purchaseReceivalDetail, WarehouseItem warehouseItem);
         StockMutation CreateStockMutationForSalesOrder(SalesOrderDetail salesOrderDetail, Item item);
         IList<StockMutation> DeleteStockMutationForSalesOrder(SalesOrderDetail salesOrderDetail, Item item);
         StockMutation CreateStockMutationForVirtualOrder(VirtualOrderDetail virtualOrderDetail, Item item);
         IList<StockMutation> DeleteStockMutationForVirtualOrder(VirtualOrderDetail virtualOrderDetail, Item item);
         IList<StockMutation> CreateStockMutationForDeliveryOrder(DeliveryOrderDetail deliveryOrderDetail, WarehouseItem warehouseItem);
         IList<StockMutation> DeleteStockMutationForDeliveryOrder(DeliveryOrderDetail deliveryOrderDetail, WarehouseItem warehouseItem);
         IList<StockMutation> CreateStockMutationForTemporaryDeliveryOrder(TemporaryDeliveryOrderDetail deliveryOrderDetail, WarehouseItem warehouseItem);
         IList<StockMutation> DeleteStockMutationForTemporaryDeliveryOrder(TemporaryDeliveryOrderDetail deliveryOrderDetail, WarehouseItem warehouseItem);
         IList<StockMutation> CreateStockMutationForTemporaryDeliveryOrderWaste(TemporaryDeliveryOrderDetail deliveryOrderDetail, DateTime PushDate, WarehouseItem warehouseItem);
         IList<StockMutation> DeleteStockMutationForTemporaryDeliveryOrderWaste(TemporaryDeliveryOrderDetail deliveryOrderDetail, WarehouseItem warehouseItem);
         IList<StockMutation> CreateStockMutationForTemporaryDeliveryOrderRestock(TemporaryDeliveryOrderDetail deliveryOrderDetail, DateTime PushDate, WarehouseItem warehouseItem);
         IList<StockMutation> DeleteStockMutationForTemporaryDeliveryOrderRestock(TemporaryDeliveryOrderDetail deliveryOrderDetail, WarehouseItem warehouseItem);
         IList<StockMutation> CreateStockMutationForTemporaryDeliveryOrderClearanceWaste(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail, DateTime PushDate, WarehouseItem warehouseItem);
         IList<StockMutation> DeleteStockMutationForTemporaryDeliveryOrderClearanceWaste(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail, WarehouseItem warehouseItem);
         IList<StockMutation> CreateStockMutationForTemporaryDeliveryOrderClearanceReturn(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail, DateTime PushDate, WarehouseItem warehouseItem);
         IList<StockMutation> DeleteStockMutationForTemporaryDeliveryOrderClearanceReturn(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail, WarehouseItem warehouseItem);
         StockMutation CreateStockMutationForStockAdjustment(StockAdjustmentDetail stockAdjustmentDetail, WarehouseItem warehouseItem);
         IList<StockMutation> DeleteStockMutationForStockAdjustment(StockAdjustmentDetail stockAdjustmentDetail, WarehouseItem warehouseItem);
         StockMutation CreateStockMutationForCustomerStockAdjustment(CustomerStockAdjustmentDetail customerStockAdjustmentDetail, WarehouseItem warehouseItem);
         IList<StockMutation> DeleteStockMutationForCustomerStockAdjustment(CustomerStockAdjustmentDetail customerStockAdjustmentDetail, WarehouseItem warehouseItem);
         StockMutation CreateStockMutationForCoreIdentification(CoreIdentificationDetail coreIdentificationDetail, WarehouseItem warehouseItem);
         IList<StockMutation> DeleteStockMutationForCoreIdentification(CoreIdentificationDetail coreIdentificationDetail, WarehouseItem warehouseItem);
         StockMutation CreateStockMutationForRecoveryOrder(RecoveryOrderDetail recoveryOrderDetail, DateTime FinishedOrRejectedDate, WarehouseItem warehouseItem, bool CaseAddition);
         StockMutation CreateStockMutationForRecoveryOrderCompound(RecoveryOrderDetail recoveryOrderDetail, DateTime FinishedOrRejectedDate, WarehouseItem warehouseItem, bool CaseAddition);
         IList<StockMutation> DeleteStockMutationForRecoveryOrder(RecoveryOrderDetail recoveryOrderDetail, WarehouseItem warehouseItem);
         StockMutation CreateStockMutationForRecoveryAccessory(RecoveryAccessoryDetail recoveryAccessoryDetail, DateTime FinishedDate, WarehouseItem warehouseItem);
         IList<StockMutation> DeleteStockMutationForRecoveryAccessory(RecoveryAccessoryDetail recoveryAccessoryDetail, WarehouseItem warehouseItem);
         StockMutation CreateStockMutationForBlanketOrder(BlanketOrderDetail blanketOrderDetail, DateTime FinishedOrRejectedDate, WarehouseItem warehouseItem, bool CaseAddition);
         IList<StockMutation> DeleteStockMutationForBlanketOrder(BlanketOrderDetail blanketOrderDetail, WarehouseItem warehouseItem);
         StockMutation CreateStockMutationForBlendingWorkOrderSource(BlendingWorkOrder blendingWorkOrder, BlendingRecipeDetail blendingRecipeDetail, WarehouseItem warehouseItem);
         IList<StockMutation> DeleteStockMutationForBlendingWorkOrderSource(BlendingRecipeDetail blendingRecipeDetail, WarehouseItem warehouseItem);
         StockMutation CreateStockMutationForBlendingWorkOrderTarget(BlendingWorkOrder blendingWorkOrder, BlendingRecipe blendingRecipe, WarehouseItem warehouseItem);
         IList<StockMutation> DeleteStockMutationForBlendingWorkOrderTarget(BlendingRecipe blendingRecipe, WarehouseItem warehouseItem);
         IList<StockMutation> CreateStockMutationForRollerWarehouseMutation(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, WarehouseItem warehouseItemFrom, WarehouseItem warehouseItemTo);
         IList<StockMutation> DeleteStockMutationForRollerWarehouseMutation(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, WarehouseItem warehouseItemFrom, WarehouseItem warehouseItemTo);
         IList<StockMutation> CreateStockMutationForWarehouseMutation(WarehouseMutationDetail WarehouseMutationDetail, WarehouseItem warehouseItemFrom, WarehouseItem warehouseItemTo);
         IList<StockMutation> DeleteStockMutationForWarehouseMutation(WarehouseMutationDetail WarehouseMutationDetail, WarehouseItem warehouseItemFrom, WarehouseItem warehouseItemTo);
         void StockMutateObject(StockMutation stockMutation, IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService);
         void ReverseStockMutateObject(StockMutation stockMutation, IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService);
    }
}
