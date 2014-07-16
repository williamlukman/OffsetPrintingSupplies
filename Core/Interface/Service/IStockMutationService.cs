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
         IList<StockMutation> GetObjectsByWarehouseItemId(int WarehouseId);
         StockMutation GetObjectById(int Id);
         IList<StockMutation> GetObjectsBySourceDocumentDetail(int warehouseItemId, string SourceDocumentDetailType, int SourceDocumentDetailId);
         StockMutation CreateObject(StockMutation stockMutation);
         StockMutation UpdateObject(StockMutation stockMutation);
         StockMutation SoftDeleteObject(StockMutation stockMutation);
         bool DeleteObject(int Id);
         //StockMutation CreateStockMutationForPurchaseOrder(PurchaseOrderDetail purchaseOrderDetail, WarehouseItem warehouseItem);
         //IList<StockMutation> CreateStockMutationForPurchaseReceival(PurchaseReceivalDetail purchaseReceivalDetail, WarehouseItem warehouseItem);
         //StockMutation CreateStockMutationForSalesOrder(SalesOrderDetail salesOrderDetail, WarehouseItem warehouseItem);
         //IList<StockMutation> CreateStockMutationForDeliveryOrder(DeliveryOrderDetail deliveryOrderDetail, WarehouseItem warehouseItem);
         StockMutation CreateStockMutationForStockAdjustment(StockAdjustmentDetail stockAdjustmentDetail, WarehouseItem warehouseItem);
         StockMutation CreateStockMutationForRecoveryOrder(RecoveryOrder recoveryOrder, WarehouseItem warehouseItem);
         //IList<StockMutation> SoftDeleteStockMutationForPurchaseOrder(PurchaseOrderDetail purchaseOrderDetail, WarehouseItem warehouseItem);
         //IList<StockMutation> SoftDeleteStockMutationForPurchaseReceival(PurchaseReceivalDetail purchaseReceivalDetail, WarehouseItem warehouseItem);
         //IList<StockMutation> SoftDeleteStockMutationForSalesOrder(SalesOrderDetail salesOrderDetail, WarehouseItem warehouseItem);
         //IList<StockMutation> SoftDeleteStockMutationForDeliveryOrder(DeliveryOrderDetail deliveryOrderDetail, WarehouseItem warehouseItem);
         IList<StockMutation> SoftDeleteStockMutationForStockAdjustment(StockAdjustmentDetail stockAdjustmentDetail, WarehouseItem warehouseItem);
         IList<StockMutation> SoftDeleteStockMutationForRecoveryOrder(RecoveryOrder recoveryOrder, WarehouseItem warehouseItem);
    }
}
