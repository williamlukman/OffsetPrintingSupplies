using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Service
{
    public interface IStockMutationService
    {
         IStockMutationValidator GetValidator();
         IList<StockMutation> GetAll();
         IList<StockMutation> GetObjectsByWarehouseId(int warehouseId);
         IList<StockMutation> GetObjectsByWarehouseItemId(int WarehouseId);
         StockMutation GetObjectById(int Id);
         IList<StockMutation> GetObjectsBySourceDocumentDetail(int warehouseItemId, string SourceDocumentDetailType, int SourceDocumentDetailId);
         StockMutation CreateObject(StockMutation stockMutation, IWarehouseService _warehouseService, IWarehouseItemService _warehouseItemService);
         StockMutation UpdateObject(StockMutation stockMutation, IWarehouseService _warehouseService, IWarehouseItemService _warehouseItemService);
         StockMutation SoftDeleteObject(StockMutation stockMutation, IWarehouseService _warehouseService, IWarehouseItemService _warehouseItemService);
         bool DeleteObject(int Id);
         //StockMutation CreateStockMutationForPurchaseOrder(PurchaseOrderDetail purchaseOrderDetail, WarehouseItem warehouseItem);
         //IList<StockMutation> SoftDeleteStockMutationForPurchaseOrder(PurchaseOrderDetail purchaseOrderDetail, WarehouseItem warehouseItem);
         //IList<StockMutation> CreateStockMutationForPurchaseReceival(PurchaseReceivalDetail purchaseReceivalDetail, WarehouseItem warehouseItem);
         //IList<StockMutation> SoftDeleteStockMutationForPurchaseReceival(PurchaseReceivalDetail purchaseReceivalDetail, WarehouseItem warehouseItem);
         //StockMutation CreateStockMutationForSalesOrder(SalesOrderDetail salesOrderDetail, WarehouseItem warehouseItem);
         //IList<StockMutation> SoftDeleteStockMutationForSalesOrder(SalesOrderDetail salesOrderDetail, WarehouseItem warehouseItem);
         //IList<StockMutation> CreateStockMutationForDeliveryOrder(DeliveryOrderDetail deliveryOrderDetail, WarehouseItem warehouseItem);
         //IList<StockMutation> SoftDeleteStockMutationForDeliveryOrder(DeliveryOrderDetail deliveryOrderDetail, WarehouseItem warehouseItem);
         StockMutation CreateStockMutationForStockAdjustment(StockAdjustmentDetail stockAdjustmentDetail, WarehouseItem warehouseItem);
         IList<StockMutation> SoftDeleteStockMutationForStockAdjustment(StockAdjustmentDetail stockAdjustmentDetail, WarehouseItem warehouseItem);
         StockMutation CreateStockMutationForCoreIdentification(CoreIdentificationDetail coreIdentificationDetail, WarehouseItem warehouseItem);
         IList<StockMutation> SoftDeleteStockMutationForCoreIdentification(CoreIdentificationDetail coreIdentificationDetail, WarehouseItem warehouseItem);
         StockMutation CreateStockMutationForRecoveryOrder(RecoveryOrderDetail recoveryOrderDetail, WarehouseItem warehouseItem, bool CaseAddition);
         IList<StockMutation> SoftDeleteStockMutationForRecoveryOrder(RecoveryOrderDetail recoveryOrderDetail, WarehouseItem warehouseItem);
         StockMutation CreateStockMutationForRecoveryAccessory(RecoveryAccessoryDetail recoveryAccessoryDetail, WarehouseItem warehouseItem);
         IList<StockMutation> SoftDeleteStockMutationForRecoveryAccessory(RecoveryAccessoryDetail recoveryAccessoryDetail, WarehouseItem warehouseItem);
         StockMutation CreateStockMutationForBarringOrder(BarringOrderDetail barringOrderDetail, WarehouseItem warehouseItem, bool CaseAddition);
         IList<StockMutation> SoftDeleteStockMutationForBarringOrder(BarringOrderDetail barringOrderDetail, WarehouseItem warehouseItem);
         IList<StockMutation> CreateStockMutationForRollerWarehouseMutation(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, WarehouseItem warehouseItemFrom, WarehouseItem warehouseItemTo);
         IList<StockMutation> SoftDeleteStockMutationForRollerWarehouseMutation(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, WarehouseItem warehouseItemFrom, WarehouseItem warehouseItemTo);
         IList<StockMutation> CreateStockMutationForWarehouseMutationOrder(WarehouseMutationOrderDetail warehouseMutationOrderDetail, WarehouseItem warehouseItemFrom, WarehouseItem warehouseItemTo);
         IList<StockMutation> SoftDeleteStockMutationForWarehouseMutationOrder(WarehouseMutationOrderDetail warehouseMutationOrderDetail, WarehouseItem warehouseItemFrom, WarehouseItem warehouseItemTo);
    }
}
