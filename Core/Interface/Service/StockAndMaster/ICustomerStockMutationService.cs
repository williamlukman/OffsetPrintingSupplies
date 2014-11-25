using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ICustomerStockMutationService
    {
         ICustomerStockMutationValidator GetValidator();
         IQueryable<CustomerStockMutation> GetQueryable();
         IList<CustomerStockMutation> GetAll();
         IList<CustomerStockMutation> GetObjectsByContactId(int contactId);
         IList<CustomerStockMutation> GetObjectsByCustomerItemId(int customerItemId);
         CustomerStockMutation GetObjectById(int Id);
         IList<CustomerStockMutation> GetObjectsBySourceDocumentDetailForCustomerItem(int customerItemId, string SourceDocumentDetailType, int SourceDocumentDetailId);
         //IList<CustomerStockMutation> GetObjectsBySourceDocumentDetailForItem(int ItemId, string SourceDocumentDetailType, int SourceDocumentDetailId);
         CustomerStockMutation CreateObject(CustomerStockMutation customerStockMutation, ICustomerItemService _customerItemService, IItemService _itemService);
         CustomerStockMutation UpdateObject(CustomerStockMutation customerStockMutation, ICustomerItemService _customerItemService, IItemService _itemService);
         CustomerStockMutation SoftDeleteObject(CustomerStockMutation customerStockMutation, ICustomerItemService _customerItemService);
         bool DeleteObject(int Id);
         //CustomerStockMutation CreateCustomerStockMutationForPurchaseOrder(PurchaseOrderDetail purchaseOrderDetail, Item item);
         //IList<CustomerStockMutation> DeleteCustomerStockMutationForPurchaseOrder(PurchaseOrderDetail purchaseOrderDetail, Item item);
         //IList<CustomerStockMutation> CreateCustomerStockMutationForPurchaseReceival(PurchaseReceivalDetail purchaseReceivalDetail, CustomerItem customerItem);
         //IList<CustomerStockMutation> DeleteCustomerStockMutationForPurchaseReceival(PurchaseReceivalDetail purchaseReceivalDetail, CustomerItem customerItem);
         //CustomerStockMutation CreateCustomerStockMutationForSalesOrder(SalesOrderDetail salesOrderDetail, Item item);
         //IList<CustomerStockMutation> DeleteCustomerStockMutationForSalesOrder(SalesOrderDetail salesOrderDetail, Item item);
         //CustomerStockMutation CreateCustomerStockMutationForVirtualOrder(VirtualOrderDetail virtualOrderDetail, Item item);
         //IList<CustomerStockMutation> DeleteCustomerStockMutationForVirtualOrder(VirtualOrderDetail virtualOrderDetail, Item item);
         //IList<CustomerStockMutation> CreateCustomerStockMutationForDeliveryOrder(DeliveryOrderDetail deliveryOrderDetail, CustomerItem customerItem);
         //IList<CustomerStockMutation> DeleteCustomerStockMutationForDeliveryOrder(DeliveryOrderDetail deliveryOrderDetail, CustomerItem customerItem);
         //IList<CustomerStockMutation> CreateCustomerStockMutationForTemporaryDeliveryOrder(TemporaryDeliveryOrderDetail deliveryOrderDetail, CustomerItem customerItem);
         //IList<CustomerStockMutation> DeleteCustomerStockMutationForTemporaryDeliveryOrder(TemporaryDeliveryOrderDetail deliveryOrderDetail, CustomerItem customerItem);
         //IList<CustomerStockMutation> CreateCustomerStockMutationForTemporaryDeliveryOrderWaste(TemporaryDeliveryOrderDetail deliveryOrderDetail, DateTime PushDate, CustomerItem customerItem);
         //IList<CustomerStockMutation> DeleteCustomerStockMutationForTemporaryDeliveryOrderWaste(TemporaryDeliveryOrderDetail deliveryOrderDetail, CustomerItem customerItem);
         //IList<CustomerStockMutation> CreateCustomerStockMutationForTemporaryDeliveryOrderRestock(TemporaryDeliveryOrderDetail deliveryOrderDetail, DateTime PushDate, CustomerItem customerItem);
         //IList<CustomerStockMutation> DeleteCustomerStockMutationForTemporaryDeliveryOrderRestock(TemporaryDeliveryOrderDetail deliveryOrderDetail, CustomerItem customerItem);
         //CustomerStockMutation CreateCustomerStockMutationForStockAdjustment(StockAdjustmentDetail stockAdjustmentDetail, CustomerItem customerItem);
         //IList<CustomerStockMutation> DeleteCustomerStockMutationForStockAdjustment(StockAdjustmentDetail stockAdjustmentDetail, CustomerItem customerItem);
         CustomerStockMutation CreateCustomerStockMutationForCoreIdentification(CoreIdentificationDetail coreIdentificationDetail, CustomerItem customerItem);
         IList<CustomerStockMutation> DeleteCustomerStockMutationForCoreIdentification(CoreIdentificationDetail coreIdentificationDetail, CustomerItem customerItem);
         CustomerStockMutation CreateCustomerStockMutationForRecoveryOrder(RecoveryOrderDetail recoveryOrderDetail, DateTime FinishedOrRejectedDate, CustomerItem customerItem, bool CaseAddition);
         //CustomerStockMutation CreateCustomerStockMutationForRecoveryOrderCompound(RecoveryOrderDetail recoveryOrderDetail, DateTime FinishedOrRejectedDate, CustomerItem customerItem, bool CaseAddition);
         IList<CustomerStockMutation> DeleteCustomerStockMutationForRecoveryOrder(RecoveryOrderDetail recoveryOrderDetail, CustomerItem customerItem);
         //CustomerStockMutation CreateCustomerStockMutationForRecoveryAccessory(RecoveryAccessoryDetail recoveryAccessoryDetail, DateTime FinishedDate, CustomerItem customerItem);
         //IList<CustomerStockMutation> DeleteCustomerStockMutationForRecoveryAccessory(RecoveryAccessoryDetail recoveryAccessoryDetail, CustomerItem customerItem);
         //CustomerStockMutation CreateCustomerStockMutationForBlanketOrder(BlanketOrderDetail blanketOrderDetail, DateTime FinishedOrRejectedDate, CustomerItem customerItem, bool CaseAddition);
         //IList<CustomerStockMutation> DeleteCustomerStockMutationForBlanketOrder(BlanketOrderDetail blanketOrderDetail, CustomerItem customerItem);
         //IList<CustomerStockMutation> CreateCustomerStockMutationForRollerWarehouseMutation(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, CustomerItem customerItemFrom, CustomerItem customerItemTo);
         //IList<CustomerStockMutation> DeleteCustomerStockMutationForRollerWarehouseMutation(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, CustomerItem customerItemFrom, CustomerItem customerItemTo);
         //IList<CustomerStockMutation> CreateCustomerStockMutationForWarehouseMutation(WarehouseMutationDetail WarehouseMutationDetail, CustomerItem customerItemFrom, CustomerItem customerItemTo);
         //IList<CustomerStockMutation> DeleteCustomerStockMutationForWarehouseMutation(WarehouseMutationDetail WarehouseMutationDetail, CustomerItem customerItemFrom, CustomerItem customerItemTo);

         void StockMutateObject(CustomerStockMutation customerStockMutation, bool IsInHouse, IItemService _itemService, ICustomerItemService _customerItemService);
         void ReverseStockMutateObject(CustomerStockMutation customerStockMutation, bool IsInHouse, IItemService _itemService, ICustomerItemService _customerItemService);
    }
}
