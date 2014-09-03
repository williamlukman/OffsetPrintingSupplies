using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Validation;

namespace Service.Service
{
    public class StockMutationService : IStockMutationService
    {
        private IStockMutationRepository _repository;
        private IStockMutationValidator _validator;

        public StockMutationService(IStockMutationRepository _stockMutationRepository, IStockMutationValidator _stockMutationValidator)
        {
            _repository = _stockMutationRepository;
            _validator = _stockMutationValidator;
        }

        public IStockMutationValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<StockMutation> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<StockMutation> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<StockMutation> GetObjectsByItemId(int itemId)
        {
            return _repository.GetObjectsByItemId(itemId);
        }

        public IList<StockMutation> GetObjectsByWarehouseId(int warehouseId)
        {
            return _repository.GetObjectsByWarehouseId(warehouseId);
        }

        public IList<StockMutation> GetObjectsByWarehouseItemId(int warehouseItemId)
        {
            return _repository.GetObjectsByWarehouseItemId(warehouseItemId);
        }

        public StockMutation GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public IList<StockMutation> GetObjectsBySourceDocumentDetailForWarehouseItem(int warehouseItemId, string SourceDocumentDetailType, int SourceDocumentDetailId)
        {
            return _repository.GetObjectsBySourceDocumentDetailForWarehouseItem(warehouseItemId, SourceDocumentDetailType, SourceDocumentDetailId);
        }

        public IList<StockMutation> GetObjectsBySourceDocumentDetailForItem(int itemId, string SourceDocumentDetailType, int SourceDocumentDetailId)
        {
            return _repository.GetObjectsBySourceDocumentDetailForItem(itemId, SourceDocumentDetailType, SourceDocumentDetailId);
        }

        public StockMutation CreateObject(StockMutation stockMutation, IWarehouseService _warehouseService, IWarehouseItemService _warehouseItemService, IItemService _itemService, IBlanketService _blanketService)
        {
            stockMutation.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(stockMutation, _warehouseService, _warehouseItemService, _itemService) ? _repository.CreateObject(stockMutation) : stockMutation);
        }

        public StockMutation UpdateObject(StockMutation stockMutation, IWarehouseService _warehouseService, IWarehouseItemService _warehouseItemService, IItemService _itemService, IBlanketService _blanketService)
        {
            return (_validator.ValidUpdateObject(stockMutation, _warehouseService, _warehouseItemService, _itemService) ? _repository.UpdateObject(stockMutation) : stockMutation);
        }

        public StockMutation SoftDeleteObject(StockMutation stockMutation, IWarehouseService _warehouseService, IWarehouseItemService _warehouseItemService, IItemService _itemService, IBlanketService _blanketService)
        {
            return (_validator.ValidDeleteObject(stockMutation, _warehouseService, _warehouseItemService) ? _repository.SoftDeleteObject(stockMutation) : stockMutation);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public StockMutation CreateStockMutationForPurchaseOrder(PurchaseOrderDetail purchaseOrderDetail, Item item)
        {
            StockMutation stockMutation = new StockMutation();
            stockMutation.ItemId = item.Id;
            stockMutation.Quantity = purchaseOrderDetail.Quantity;
            stockMutation.SourceDocumentType = Constant.SourceDocumentType.PurchaseOrder;
            stockMutation.SourceDocumentId = purchaseOrderDetail.PurchaseOrderId;
            stockMutation.SourceDocumentDetailType = Constant.SourceDocumentDetailType.PurchaseOrderDetail;
            stockMutation.SourceDocumentDetailId = purchaseOrderDetail.Id;
            stockMutation.ItemCase = Constant.ItemCase.PendingReceival;
            stockMutation.Status = Constant.MutationStatus.Addition;
            return _repository.CreateObject(stockMutation);
        }
        
        public IList<StockMutation> SoftDeleteStockMutationForPurchaseOrder(PurchaseOrderDetail purchaseOrderDetail, Item item)
        {
            IList<StockMutation> stockMutations = _repository.GetObjectsBySourceDocumentDetailForItem(item.Id, Constant.SourceDocumentDetailType.PurchaseOrderDetail, purchaseOrderDetail.Id);
            foreach (var stockMutation in stockMutations)
            {
                _repository.Delete(stockMutation);
            }
            return stockMutations;
        }

        public IList<StockMutation> CreateStockMutationForPurchaseReceival(PurchaseReceivalDetail purchaseReceivalDetail, WarehouseItem warehouseItem)
        {
            IList<StockMutation> result = new List<StockMutation>();
            
            StockMutation stockMutationPendingReceival = new StockMutation();
            stockMutationPendingReceival.ItemId = warehouseItem.ItemId;
            stockMutationPendingReceival.WarehouseId = warehouseItem.WarehouseId;
            stockMutationPendingReceival.WarehouseItemId = warehouseItem.Id;
            stockMutationPendingReceival.Quantity = purchaseReceivalDetail.Quantity;
            stockMutationPendingReceival.SourceDocumentType = Constant.SourceDocumentType.PurchaseReceival;
            stockMutationPendingReceival.SourceDocumentId = purchaseReceivalDetail.PurchaseReceivalId;
            stockMutationPendingReceival.SourceDocumentDetailType = Constant.SourceDocumentDetailType.PurchaseReceivalDetail;
            stockMutationPendingReceival.SourceDocumentDetailId = purchaseReceivalDetail.Id;
            stockMutationPendingReceival.ItemCase = Constant.ItemCase.PendingReceival;
            stockMutationPendingReceival.Status = Constant.MutationStatus.Deduction;
            stockMutationPendingReceival = _repository.CreateObject(stockMutationPendingReceival);

            StockMutation stockMutationReady = new StockMutation();
            stockMutationReady.ItemId = warehouseItem.ItemId;
            stockMutationReady.WarehouseId = warehouseItem.WarehouseId;
            stockMutationReady.WarehouseItemId = warehouseItem.Id;
            stockMutationReady.Quantity = purchaseReceivalDetail.Quantity;
            stockMutationReady.SourceDocumentType = Constant.SourceDocumentType.PurchaseReceival;
            stockMutationReady.SourceDocumentId = purchaseReceivalDetail.PurchaseReceivalId;
            stockMutationReady.SourceDocumentDetailType = Constant.SourceDocumentDetailType.PurchaseReceivalDetail;
            stockMutationReady.SourceDocumentDetailId = purchaseReceivalDetail.Id;
            stockMutationReady.ItemCase = Constant.ItemCase.Ready;
            stockMutationReady.Status = Constant.MutationStatus.Addition;
            stockMutationReady = _repository.CreateObject(stockMutationReady);

            result.Add(stockMutationPendingReceival);
            result.Add(stockMutationReady);
            return result;
        }

        public IList<StockMutation> SoftDeleteStockMutationForPurchaseReceival(PurchaseReceivalDetail purchaseReceivalDetail, WarehouseItem warehouseItem)
        {
            IList<StockMutation> stockMutations = _repository.GetObjectsBySourceDocumentDetailForWarehouseItem(warehouseItem.Id, Constant.SourceDocumentDetailType.PurchaseReceivalDetail, purchaseReceivalDetail.Id);
            foreach (var stockMutation in stockMutations)
            {
                _repository.Delete(stockMutation);
            }
            return stockMutations;
        }

        public StockMutation CreateStockMutationForSalesOrder(SalesOrderDetail salesOrderDetail, Item item)
        {
            StockMutation stockMutation = new StockMutation();
            stockMutation.ItemId = item.Id;
            stockMutation.Quantity = salesOrderDetail.Quantity;
            stockMutation.SourceDocumentType = Constant.SourceDocumentType.SalesOrder;
            stockMutation.SourceDocumentId = salesOrderDetail.SalesOrderId;
            stockMutation.SourceDocumentDetailType = Constant.SourceDocumentDetailType.SalesOrderDetail;
            stockMutation.SourceDocumentDetailId = salesOrderDetail.Id;
            stockMutation.ItemCase = Constant.ItemCase.PendingDelivery;
            stockMutation.Status = Constant.MutationStatus.Addition;
            return _repository.CreateObject(stockMutation);
        }

        public IList<StockMutation> SoftDeleteStockMutationForSalesOrder(SalesOrderDetail salesOrderDetail, Item item)
        {
            IList<StockMutation> stockMutations = _repository.GetObjectsBySourceDocumentDetailForItem(item.Id, Constant.SourceDocumentDetailType.SalesOrderDetail, salesOrderDetail.Id);
            foreach (var stockMutation in stockMutations)
            {
                _repository.Delete(stockMutation);
            }
            return stockMutations;
        }

        public IList<StockMutation> CreateStockMutationForDeliveryOrder(DeliveryOrderDetail deliveryOrderDetail, WarehouseItem warehouseItem)
        {
            IList<StockMutation> result = new List<StockMutation>();

            StockMutation stockMutationPendingDelivery = new StockMutation();
            stockMutationPendingDelivery.ItemId = warehouseItem.ItemId;
            stockMutationPendingDelivery.WarehouseId = warehouseItem.WarehouseId;
            stockMutationPendingDelivery.WarehouseItemId = warehouseItem.Id;
            stockMutationPendingDelivery.ItemId = deliveryOrderDetail.ItemId;
            stockMutationPendingDelivery.Quantity = deliveryOrderDetail.Quantity;
            stockMutationPendingDelivery.SourceDocumentType = Constant.SourceDocumentType.DeliveryOrder;
            stockMutationPendingDelivery.SourceDocumentId = deliveryOrderDetail.DeliveryOrderId;
            stockMutationPendingDelivery.SourceDocumentDetailType = Constant.SourceDocumentDetailType.DeliveryOrderDetail;
            stockMutationPendingDelivery.SourceDocumentDetailId = deliveryOrderDetail.Id;
            stockMutationPendingDelivery.ItemCase = Constant.ItemCase.PendingDelivery;
            stockMutationPendingDelivery.Status = Constant.MutationStatus.Deduction;
            stockMutationPendingDelivery = _repository.CreateObject(stockMutationPendingDelivery);

            StockMutation stockMutationReady = new StockMutation();
            stockMutationReady.ItemId = warehouseItem.ItemId;
            stockMutationReady.WarehouseId = warehouseItem.WarehouseId;
            stockMutationReady.WarehouseItemId = warehouseItem.Id;
            stockMutationReady.ItemId = deliveryOrderDetail.ItemId;
            stockMutationReady.Quantity = deliveryOrderDetail.Quantity;
            stockMutationReady.SourceDocumentType = Constant.SourceDocumentType.DeliveryOrder;
            stockMutationReady.SourceDocumentId = deliveryOrderDetail.DeliveryOrderId;
            stockMutationReady.SourceDocumentDetailType = Constant.SourceDocumentDetailType.DeliveryOrderDetail;
            stockMutationReady.SourceDocumentDetailId = deliveryOrderDetail.Id;
            stockMutationReady.ItemCase = Constant.ItemCase.Ready;
            stockMutationReady.Status = Constant.MutationStatus.Deduction;
            stockMutationReady = _repository.CreateObject(stockMutationReady);

            result.Add(stockMutationPendingDelivery);
            result.Add(stockMutationReady);
            return result;
        }

        public IList<StockMutation> SoftDeleteStockMutationForDeliveryOrder(DeliveryOrderDetail deliveryOrderDetail, WarehouseItem warehouseItem)
        {
            IList<StockMutation> stockMutations = _repository.GetObjectsBySourceDocumentDetailForWarehouseItem(warehouseItem.Id, Constant.SourceDocumentDetailType.DeliveryOrderDetail, deliveryOrderDetail.Id);
            foreach (var stockMutation in stockMutations)
            {
                _repository.Delete(stockMutation);
            }
            return stockMutations;
        }

        public StockMutation CreateStockMutationForStockAdjustment(StockAdjustmentDetail stockAdjustmentDetail, WarehouseItem warehouseItem)
        {
            StockMutation stockMutation = new StockMutation();
            stockMutation.ItemId = warehouseItem.ItemId;
            stockMutation.WarehouseId = warehouseItem.WarehouseId;
            stockMutation.WarehouseItemId = warehouseItem.Id;
            stockMutation.Quantity = (stockAdjustmentDetail.Quantity >= 0) ? stockAdjustmentDetail.Quantity : (-1) * stockAdjustmentDetail.Quantity;
            stockMutation.SourceDocumentType = Constant.SourceDocumentType.StockAdjustment;
            stockMutation.SourceDocumentId = stockAdjustmentDetail.StockAdjustmentId;
            stockMutation.SourceDocumentDetailType = Constant.SourceDocumentDetailType.StockAdjustmentDetail;
            stockMutation.SourceDocumentDetailId = stockAdjustmentDetail.Id;
            stockMutation.ItemCase = Constant.ItemCase.Ready;
            stockMutation.Status = (stockAdjustmentDetail.Quantity >= 0) ? Constant.MutationStatus.Addition : Constant.MutationStatus.Deduction;
            return _repository.CreateObject(stockMutation);
        }

        public IList<StockMutation> SoftDeleteStockMutationForStockAdjustment(StockAdjustmentDetail stockAdjustmentDetail, WarehouseItem warehouseItem)
        {
            IList<StockMutation> stockMutations = _repository.GetObjectsBySourceDocumentDetailForWarehouseItem(warehouseItem.Id, Constant.SourceDocumentDetailType.StockAdjustmentDetail, stockAdjustmentDetail.Id);
            foreach (var stockMutation in stockMutations)
            {
                _repository.Delete(stockMutation);
            }
            return stockMutations;
        }

        public StockMutation CreateStockMutationForCoreIdentification(CoreIdentificationDetail coreIdentificationDetail, WarehouseItem warehouseItem)
        {
            StockMutation stockMutation = new StockMutation();
            stockMutation.ItemId = warehouseItem.ItemId;
            stockMutation.WarehouseId = warehouseItem.WarehouseId;
            stockMutation.WarehouseItemId = warehouseItem.Id;
            stockMutation.Quantity = 1;
            stockMutation.SourceDocumentType = Constant.SourceDocumentType.CoreIdentification;
            stockMutation.SourceDocumentId = coreIdentificationDetail.CoreIdentificationId;
            stockMutation.SourceDocumentDetailType = Constant.SourceDocumentDetailType.CoreIdentificationDetail;
            stockMutation.SourceDocumentDetailId = coreIdentificationDetail.Id;
            stockMutation.ItemCase = Constant.ItemCase.Ready;
            stockMutation.Status = Constant.MutationStatus.Addition;
            return _repository.CreateObject(stockMutation);
        }

        public IList<StockMutation> SoftDeleteStockMutationForCoreIdentification(CoreIdentificationDetail coreIdentificationDetail, WarehouseItem warehouseItem)
        {
            IList<StockMutation> stockMutations = _repository.GetObjectsBySourceDocumentDetailForWarehouseItem(warehouseItem.Id, Constant.SourceDocumentDetailType.CoreIdentificationDetail, coreIdentificationDetail.Id);
            foreach (var stockMutation in stockMutations)
            {
                _repository.Delete(stockMutation);
            }
            return stockMutations;
        }

        public StockMutation CreateStockMutationForRecoveryOrder(RecoveryOrderDetail recoveryOrderDetail, WarehouseItem warehouseItem, bool CaseAddition)
        {
            StockMutation stockMutation = new StockMutation();
            stockMutation.ItemId = warehouseItem.ItemId;
            stockMutation.WarehouseId = warehouseItem.WarehouseId;
            stockMutation.WarehouseItemId = warehouseItem.Id;
            stockMutation.Quantity = 1;
            stockMutation.SourceDocumentType = Constant.SourceDocumentType.RecoveryOrder;
            stockMutation.SourceDocumentId = recoveryOrderDetail.RecoveryOrderId;
            stockMutation.SourceDocumentDetailType = Constant.SourceDocumentDetailType.RecoveryOrderDetail;
            stockMutation.SourceDocumentDetailId = recoveryOrderDetail.Id;
            stockMutation.ItemCase = Constant.ItemCase.Ready;
            stockMutation.Status = CaseAddition ? Constant.MutationStatus.Addition : Constant.MutationStatus.Deduction;
            return _repository.CreateObject(stockMutation);
        }

        public StockMutation CreateStockMutationForRecoveryOrderCompound(RecoveryOrderDetail recoveryOrderDetail, WarehouseItem warehouseItem, bool CaseAddition)
        {
            StockMutation stockMutation = new StockMutation();
            stockMutation.ItemId = warehouseItem.ItemId;
            stockMutation.WarehouseId = warehouseItem.WarehouseId;
            stockMutation.WarehouseItemId = warehouseItem.Id;
            stockMutation.Quantity = recoveryOrderDetail.CompoundUsage;
            stockMutation.SourceDocumentType = Constant.SourceDocumentType.RecoveryOrder;
            stockMutation.SourceDocumentId = recoveryOrderDetail.RecoveryOrderId;
            stockMutation.SourceDocumentDetailType = Constant.SourceDocumentDetailType.RecoveryOrderDetail;
            stockMutation.SourceDocumentDetailId = recoveryOrderDetail.Id;
            stockMutation.ItemCase = Constant.ItemCase.Ready;
            stockMutation.Status = CaseAddition ? Constant.MutationStatus.Addition : Constant.MutationStatus.Deduction;
            return _repository.CreateObject(stockMutation);
        }

        public IList<StockMutation> SoftDeleteStockMutationForRecoveryOrder(RecoveryOrderDetail recoveryOrderDetail, WarehouseItem warehouseItem)
        {
            IList<StockMutation> stockMutations = _repository.GetObjectsBySourceDocumentDetailForWarehouseItem(warehouseItem.Id, Constant.SourceDocumentDetailType.RecoveryOrderDetail, recoveryOrderDetail.Id);
            foreach (var stockMutation in stockMutations)
            {
                _repository.Delete(stockMutation);
            }
            return stockMutations;
        }

        public StockMutation CreateStockMutationForRecoveryAccessory(RecoveryAccessoryDetail recoveryAccessoryDetail, WarehouseItem warehouseItem)
        {
            StockMutation stockMutation = new StockMutation();
            stockMutation.ItemId = warehouseItem.ItemId;
            stockMutation.WarehouseId = warehouseItem.WarehouseId;
            stockMutation.WarehouseItemId = warehouseItem.Id;
            stockMutation.Quantity = recoveryAccessoryDetail.Quantity;
            stockMutation.SourceDocumentType = Constant.SourceDocumentType.RecoveryOrderDetail;
            stockMutation.SourceDocumentId = recoveryAccessoryDetail.RecoveryOrderDetailId;
            stockMutation.SourceDocumentDetailType = Constant.SourceDocumentDetailType.RecoveryAccessoryDetail;
            stockMutation.SourceDocumentDetailId = recoveryAccessoryDetail.Id;
            stockMutation.ItemCase = Constant.ItemCase.Ready;
            stockMutation.Status = Constant.MutationStatus.Deduction;
            return _repository.CreateObject(stockMutation);
        }

        public IList<StockMutation> SoftDeleteStockMutationForRecoveryAccessory(RecoveryAccessoryDetail recoveryAccessoryDetail, WarehouseItem warehouseItem)
        {
            IList<StockMutation> stockMutations = _repository.GetObjectsBySourceDocumentDetailForWarehouseItem(warehouseItem.Id, Constant.SourceDocumentDetailType.RecoveryAccessoryDetail, recoveryAccessoryDetail.Id);
            foreach (var stockMutation in stockMutations)
            {
                _repository.Delete(stockMutation);
            }
            return stockMutations;
        }

        public StockMutation CreateStockMutationForBlanketOrder(BlanketOrderDetail blanketOrderDetail, WarehouseItem warehouseItem, bool CaseAddition)
        {
            StockMutation stockMutation = new StockMutation();
            stockMutation.ItemId = warehouseItem.ItemId;
            stockMutation.WarehouseId = warehouseItem.WarehouseId;
            stockMutation.WarehouseItemId = warehouseItem.Id;
            stockMutation.Quantity = 1;
            stockMutation.SourceDocumentType = Constant.SourceDocumentType.BlanketOrder;
            stockMutation.SourceDocumentId = blanketOrderDetail.BlanketOrderId;
            stockMutation.SourceDocumentDetailType = Constant.SourceDocumentDetailType.BlanketOrderDetail;
            stockMutation.SourceDocumentDetailId = blanketOrderDetail.Id;
            stockMutation.ItemCase = Constant.ItemCase.Ready;
            stockMutation.Status = CaseAddition ? Constant.MutationStatus.Addition : Constant.MutationStatus.Deduction;
            return _repository.CreateObject(stockMutation);
        }

        public IList<StockMutation> SoftDeleteStockMutationForBlanketOrder(BlanketOrderDetail blanketOrderDetail, WarehouseItem warehouseItem)
        {
            IList<StockMutation> stockMutations = _repository.GetObjectsBySourceDocumentDetailForWarehouseItem(warehouseItem.Id, Constant.SourceDocumentDetailType.BlanketOrderDetail, blanketOrderDetail.Id);
            foreach (var stockMutation in stockMutations)
            {
                _repository.Delete(stockMutation);
            }
            return stockMutations;
        }

        public IList<StockMutation> CreateStockMutationForRollerWarehouseMutation(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, WarehouseItem warehouseItemFrom, WarehouseItem warehouseItemTo)
        {
            IList<StockMutation> stockMutations = new List<StockMutation>();

            StockMutation stockMutationFrom = new StockMutation();
            stockMutationFrom.ItemId = warehouseItemFrom.ItemId;
            stockMutationFrom.WarehouseId = warehouseItemFrom.WarehouseId;
            stockMutationFrom.WarehouseItemId = warehouseItemFrom.Id;
            stockMutationFrom.Quantity = 1;
            stockMutationFrom.SourceDocumentType = Constant.SourceDocumentType.RollerWarehouseMutation;
            stockMutationFrom.SourceDocumentId = rollerWarehouseMutationDetail.RollerWarehouseMutationId;
            stockMutationFrom.SourceDocumentDetailType = Constant.SourceDocumentDetailType.RollerWarehouseMutationDetail;
            stockMutationFrom.SourceDocumentDetailId = rollerWarehouseMutationDetail.Id;
            stockMutationFrom.ItemCase = Constant.ItemCase.Ready;
            stockMutationFrom.Status = Constant.MutationStatus.Deduction;
            stockMutationFrom = _repository.CreateObject(stockMutationFrom);

            StockMutation stockMutationTo = new StockMutation();
            stockMutationTo.ItemId = warehouseItemTo.ItemId;
            stockMutationTo.WarehouseId = warehouseItemTo.WarehouseId;
            stockMutationTo.WarehouseItemId = warehouseItemTo.Id;
            stockMutationTo.Quantity = 1;
            stockMutationTo.SourceDocumentType = Constant.SourceDocumentType.RollerWarehouseMutation;
            stockMutationTo.SourceDocumentId = rollerWarehouseMutationDetail.RollerWarehouseMutationId;
            stockMutationTo.SourceDocumentDetailType = Constant.SourceDocumentDetailType.RollerWarehouseMutationDetail;
            stockMutationTo.SourceDocumentDetailId = rollerWarehouseMutationDetail.Id;
            stockMutationTo.ItemCase = Constant.ItemCase.Ready;
            stockMutationTo.Status = Constant.MutationStatus.Addition;
            stockMutationTo = _repository.CreateObject(stockMutationTo);

            stockMutations.Add(stockMutationFrom);
            stockMutations.Add(stockMutationTo);
            return stockMutations;
        }

        public IList<StockMutation> SoftDeleteStockMutationForRollerWarehouseMutation(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, WarehouseItem warehouseItemFrom, WarehouseItem warehouseItemTo)
        {
            IList<StockMutation> stockMutations = new List<StockMutation>();

            IList<StockMutation> stockMutationFrom = _repository.GetObjectsBySourceDocumentDetailForWarehouseItem(warehouseItemFrom.Id, Constant.SourceDocumentDetailType.RollerWarehouseMutationDetail, rollerWarehouseMutationDetail.Id);
            stockMutationFrom.ToList().ForEach(x => stockMutations.Add(x));
            foreach (var stockMutation in stockMutationFrom)
            {
                _repository.Delete(stockMutation);
            }

            IList<StockMutation> stockMutationTo = _repository.GetObjectsBySourceDocumentDetailForWarehouseItem(warehouseItemTo.Id, Constant.SourceDocumentDetailType.RollerWarehouseMutationDetail, rollerWarehouseMutationDetail.Id);
            stockMutationTo.ToList().ForEach(x => stockMutations.Add(x));
            foreach (var stockMutation in stockMutationTo)
            {
                _repository.Delete(stockMutation);
            }
            return stockMutations;
        }

        public IList<StockMutation> CreateStockMutationForWarehouseMutation(WarehouseMutationDetail WarehouseMutationDetail, WarehouseItem warehouseItemFrom, WarehouseItem warehouseItemTo)
        {
            IList<StockMutation> stockMutations = new List<StockMutation>();

            StockMutation stockMutationFrom = new StockMutation();
            stockMutationFrom.ItemId = warehouseItemFrom.ItemId;
            stockMutationFrom.WarehouseId = warehouseItemFrom.WarehouseId;
            stockMutationFrom.WarehouseItemId = warehouseItemFrom.Id;
            stockMutationFrom.Quantity = WarehouseMutationDetail.Quantity;
            stockMutationFrom.SourceDocumentType = Constant.SourceDocumentType.WarehouseMutation;
            stockMutationFrom.SourceDocumentId = WarehouseMutationDetail.WarehouseMutationId;
            stockMutationFrom.SourceDocumentDetailType = Constant.SourceDocumentDetailType.WarehouseMutationDetail;
            stockMutationFrom.SourceDocumentDetailId = WarehouseMutationDetail.Id;
            stockMutationFrom.ItemCase = Constant.ItemCase.Ready;
            stockMutationFrom.Status = Constant.MutationStatus.Deduction;
            stockMutationFrom = _repository.CreateObject(stockMutationFrom);

            StockMutation stockMutationTo = new StockMutation();
            stockMutationTo.ItemId = warehouseItemTo.ItemId;
            stockMutationTo.WarehouseId = warehouseItemTo.WarehouseId;
            stockMutationTo.WarehouseItemId = warehouseItemTo.Id;
            stockMutationTo.Quantity = WarehouseMutationDetail.Quantity;
            stockMutationTo.SourceDocumentType = Constant.SourceDocumentType.WarehouseMutation;
            stockMutationTo.SourceDocumentId = WarehouseMutationDetail.WarehouseMutationId;
            stockMutationTo.SourceDocumentDetailType = Constant.SourceDocumentDetailType.WarehouseMutationDetail;
            stockMutationTo.SourceDocumentDetailId = WarehouseMutationDetail.Id;
            stockMutationTo.ItemCase = Constant.ItemCase.Ready;
            stockMutationTo.Status = Constant.MutationStatus.Addition;
            stockMutationTo = _repository.CreateObject(stockMutationTo);

            stockMutations.Add(stockMutationFrom);
            stockMutations.Add(stockMutationTo);
            return stockMutations;
        }

        public IList<StockMutation> SoftDeleteStockMutationForWarehouseMutation(WarehouseMutationDetail WarehouseMutationDetail, WarehouseItem warehouseItemFrom, WarehouseItem warehouseItemTo)
        {
            IList<StockMutation> stockMutations = new List<StockMutation>();

            IList<StockMutation> stockMutationFrom = _repository.GetObjectsBySourceDocumentDetailForWarehouseItem(warehouseItemFrom.Id, Constant.SourceDocumentDetailType.WarehouseMutationDetail, WarehouseMutationDetail.Id);
            stockMutationFrom.ToList().ForEach(x => stockMutations.Add(x));
            foreach (var stockMutation in stockMutationFrom)
            {
                _repository.Delete(stockMutation);
            }

            IList<StockMutation> stockMutationTo = _repository.GetObjectsBySourceDocumentDetailForWarehouseItem(warehouseItemTo.Id, Constant.SourceDocumentDetailType.WarehouseMutationDetail, WarehouseMutationDetail.Id);
            stockMutationTo.ToList().ForEach(x => stockMutations.Add(x));
            foreach (var stockMutation in stockMutationTo)
            {
                _repository.Delete(stockMutation);
            }
            return stockMutations;
        }

        public void StockMutateObject(StockMutation stockMutation, IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService)
        {
            // decimal stockAdjustmentDetailPrice = (stockMutation.Status == Constant.MutationStatus.Addition) ? stockAdjustmentDetail.Price : ((-1) * stockAdjustmentDetail.Price);
            // item.AvgCost = _blanketService.CalculateAvgCost(item, stockAdjustmentDetail.Quantity, stockAdjustmentDetailPrice);
            // blanket.AvgCost = _blanketService.CalculateAvgCost(blanket, stockAdjustmentDetail.Quantity, stockAdjustmentDetailPrice);

            int Quantity = (stockMutation.Status == Constant.MutationStatus.Addition) ? stockMutation.Quantity : (-1) * stockMutation.Quantity;
            WarehouseItem warehouseItem = stockMutation.WarehouseItemId == null ? null : _warehouseItemService.GetObjectById((int) stockMutation.WarehouseItemId);
            Item item = _itemService.GetObjectById(stockMutation.ItemId);
            Blanket blanket = _blanketService.GetObjectById(stockMutation.ItemId);

            if (warehouseItem != null)
            {
                if (stockMutation.ItemCase == Constant.ItemCase.Ready)
                { _warehouseItemService.AdjustQuantity(warehouseItem, Quantity); }
            }

            if (blanket == null)
            {
                // itemService in action
                if (stockMutation.ItemCase == Constant.ItemCase.Ready)
                { _itemService.AdjustQuantity(item, Quantity); }
                else if (stockMutation.ItemCase == Constant.ItemCase.PendingDelivery)
                { _itemService.AdjustPendingDelivery(item, Quantity); }
                else if (stockMutation.ItemCase == Constant.ItemCase.PendingReceival)
                { _itemService.AdjustPendingReceival(item, Quantity); }
            }
            else
            {
                // blanketService in action
                if (stockMutation.ItemCase == Constant.ItemCase.Ready)
                { _blanketService.AdjustQuantity(blanket, Quantity); }
                else if (stockMutation.ItemCase == Constant.ItemCase.PendingDelivery)
                { _blanketService.AdjustPendingDelivery(blanket, Quantity); }
                else if (stockMutation.ItemCase == Constant.ItemCase.PendingReceival)
                { _blanketService.AdjustPendingReceival(blanket, Quantity); }
            }
        }

        public void ReverseStockMutateObject(StockMutation stockMutation, IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService)
        {
            // decimal stockAdjustmentDetailPrice = (stockMutation.Status == Constant.MutationStatus.Addition) ? stockAdjustmentDetail.Price : ((-1) * stockAdjustmentDetail.Price);

            int Quantity = (stockMutation.Status == Constant.MutationStatus.Deduction) ? stockMutation.Quantity : (-1) * stockMutation.Quantity;
            WarehouseItem warehouseItem = stockMutation.WarehouseItemId == null ? null : _warehouseItemService.GetObjectById((int)stockMutation.WarehouseItemId);
            Item item = _itemService.GetObjectById(stockMutation.ItemId);
            Blanket blanket = _blanketService.GetObjectById(stockMutation.ItemId);
            if (warehouseItem != null) {
                if (stockMutation.ItemCase == Constant.ItemCase.Ready)
                { _warehouseItemService.AdjustQuantity(warehouseItem, Quantity); }
            }

            if (blanket == null)
            {
                // itemService in action
                if (stockMutation.ItemCase == Constant.ItemCase.Ready)
                { _itemService.AdjustQuantity(item, Quantity); }
                else if (stockMutation.ItemCase == Constant.ItemCase.PendingDelivery)
                { _itemService.AdjustPendingDelivery(item, Quantity); }
                else if (stockMutation.ItemCase == Constant.ItemCase.PendingReceival)
                { _itemService.AdjustPendingReceival(item, Quantity); }
            }
            else
            {
                // blanketService in action
                if (stockMutation.ItemCase == Constant.ItemCase.Ready)
                { _blanketService.AdjustQuantity(blanket, Quantity); }
                else if (stockMutation.ItemCase == Constant.ItemCase.PendingDelivery)
                { _blanketService.AdjustPendingDelivery(blanket, Quantity); }
                else if (stockMutation.ItemCase == Constant.ItemCase.PendingReceival)
                { _blanketService.AdjustPendingReceival(blanket, Quantity); }
            }
        }
    }
}
