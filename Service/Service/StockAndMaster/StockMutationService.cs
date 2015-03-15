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

        public int DeleteStockMutations(IList<StockMutation> stockMutations)
        {
            int count = 0;
            foreach (var stockMutation in stockMutations)
            {
                count += _repository.Delete(stockMutation);
            }
            return count;
        }

        public StockMutation CreateStockMutationForPurchaseOrderDetail(PurchaseOrderDetail purchaseOrderDetail, Item item, decimal Quantity)
        {
            StockMutation stockMutation = new StockMutation();
            stockMutation.ItemId = item.Id;
            stockMutation.Quantity = Math.Abs(Quantity);
            stockMutation.SourceDocumentType = Constant.SourceDocumentType.PurchaseOrder;
            stockMutation.SourceDocumentId = purchaseOrderDetail.PurchaseOrderId;
            stockMutation.SourceDocumentDetailType = Constant.SourceDocumentDetailType.PurchaseOrderDetail;
            stockMutation.SourceDocumentDetailId = purchaseOrderDetail.Id;
            stockMutation.ItemCase = Constant.ItemCase.PendingReceival;
            stockMutation.Status = Quantity < 0 ? Constant.MutationStatus.Deduction : Constant.MutationStatus.Addition;
            stockMutation.MutationDate = (DateTime)purchaseOrderDetail.ConfirmationDate;
            return _repository.CreateObject(stockMutation);
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
            stockMutation.MutationDate = (DateTime) purchaseOrderDetail.ConfirmationDate;
            return _repository.CreateObject(stockMutation);
        }
        
        public IList<StockMutation> GetStockMutationForPurchaseOrder(PurchaseOrderDetail purchaseOrderDetail, Item item)
        {
            IList<StockMutation> stockMutations = _repository.GetObjectsBySourceDocumentDetailForItem(item.Id, Constant.SourceDocumentDetailType.PurchaseOrderDetail, purchaseOrderDetail.Id);
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
            stockMutationPendingReceival.MutationDate = (DateTime) purchaseReceivalDetail.ConfirmationDate;
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
            stockMutationReady.MutationDate = (DateTime) purchaseReceivalDetail.ConfirmationDate;
            stockMutationReady = _repository.CreateObject(stockMutationReady);

            result.Add(stockMutationPendingReceival);
            result.Add(stockMutationReady);
            return result;
        }

        public IList<StockMutation> GetStockMutationForPurchaseReceival(PurchaseReceivalDetail purchaseReceivalDetail, WarehouseItem warehouseItem)
        {
            IList<StockMutation> stockMutations = _repository.GetObjectsBySourceDocumentDetailForWarehouseItem(warehouseItem.Id, Constant.SourceDocumentDetailType.PurchaseReceivalDetail, purchaseReceivalDetail.Id);
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
            stockMutation.MutationDate = (DateTime) salesOrderDetail.ConfirmationDate;
            return _repository.CreateObject(stockMutation);
        }

        public IList<StockMutation> GetStockMutationForSalesOrder(SalesOrderDetail salesOrderDetail, Item item)
        {
            IList<StockMutation> stockMutations = _repository.GetObjectsBySourceDocumentDetailForItem(item.Id, Constant.SourceDocumentDetailType.SalesOrderDetail, salesOrderDetail.Id);
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
            stockMutationPendingDelivery.MutationDate = (DateTime) deliveryOrderDetail.ConfirmationDate;
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
            stockMutationReady.MutationDate = (DateTime) deliveryOrderDetail.ConfirmationDate;
            stockMutationReady = _repository.CreateObject(stockMutationReady);

            result.Add(stockMutationPendingDelivery);
            result.Add(stockMutationReady);
            return result;
        }

        public IList<StockMutation> GetStockMutationForDeliveryOrder(DeliveryOrderDetail deliveryOrderDetail, WarehouseItem warehouseItem)
        {
            IList<StockMutation> stockMutations = _repository.GetObjectsBySourceDocumentDetailForWarehouseItem(warehouseItem.Id, Constant.SourceDocumentDetailType.DeliveryOrderDetail, deliveryOrderDetail.Id);
            return stockMutations;
        }

        public StockMutation CreateStockMutationForVirtualOrder(VirtualOrderDetail virtualOrderDetail, Item item)
        {
            StockMutation stockMutation = new StockMutation();
            stockMutation.ItemId = item.Id;
            stockMutation.Quantity = virtualOrderDetail.Quantity;
            stockMutation.SourceDocumentType = Constant.SourceDocumentType.VirtualOrder;
            stockMutation.SourceDocumentId = virtualOrderDetail.VirtualOrderId;
            stockMutation.SourceDocumentDetailType = Constant.SourceDocumentDetailType.VirtualOrderDetail;
            stockMutation.SourceDocumentDetailId = virtualOrderDetail.Id;
            stockMutation.ItemCase = Constant.ItemCase.PendingDelivery;
            stockMutation.Status = Constant.MutationStatus.Addition;
            stockMutation.MutationDate = (DateTime) virtualOrderDetail.ConfirmationDate;
            return _repository.CreateObject(stockMutation);
        }

        public IList<StockMutation> GetStockMutationForVirtualOrder(VirtualOrderDetail virtualOrderDetail, Item item)
        {
            IList<StockMutation> stockMutations = _repository.GetObjectsBySourceDocumentDetailForItem(item.Id, Constant.SourceDocumentDetailType.VirtualOrderDetail, virtualOrderDetail.Id);
            return stockMutations;
        }

        public IList<StockMutation> CreateStockMutationForTemporaryDeliveryOrder(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail, WarehouseItem warehouseItem)
        {
            IList<StockMutation> result = new List<StockMutation>();

            StockMutation stockMutationVirtual = new StockMutation();
            stockMutationVirtual.ItemId = warehouseItem.ItemId;
            stockMutationVirtual.WarehouseId = warehouseItem.WarehouseId;
            stockMutationVirtual.WarehouseItemId = warehouseItem.Id;
            stockMutationVirtual.ItemId = temporaryDeliveryOrderDetail.ItemId;
            stockMutationVirtual.Quantity = temporaryDeliveryOrderDetail.Quantity;
            stockMutationVirtual.SourceDocumentType = Constant.SourceDocumentType.TemporaryDeliveryOrder;
            stockMutationVirtual.SourceDocumentId = temporaryDeliveryOrderDetail.TemporaryDeliveryOrderId;
            stockMutationVirtual.SourceDocumentDetailType = Constant.SourceDocumentDetailType.TemporaryDeliveryOrderDetail;
            stockMutationVirtual.SourceDocumentDetailId = temporaryDeliveryOrderDetail.Id;
            stockMutationVirtual.ItemCase = Constant.ItemCase.Virtual;
            stockMutationVirtual.Status = Constant.MutationStatus.Addition;
            stockMutationVirtual.MutationDate = (DateTime) temporaryDeliveryOrderDetail.ConfirmationDate;
            stockMutationVirtual = _repository.CreateObject(stockMutationVirtual);

            result.Add(stockMutationVirtual);
            return result;
        }

        public IList<StockMutation> GetStockMutationForTemporaryDeliveryOrder(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail, WarehouseItem warehouseItem)
        {
            IList<StockMutation> stockMutations = _repository.GetObjectsBySourceDocumentDetailForWarehouseItem(warehouseItem.Id, Constant.SourceDocumentDetailType.TemporaryDeliveryOrderDetail, temporaryDeliveryOrderDetail.Id);
            return stockMutations;
        }

        public IList<StockMutation> CreateStockMutationForTemporaryDeliveryOrderWaste(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail, DateTime PushDate, WarehouseItem warehouseItem)
        {
            IList<StockMutation> result = new List<StockMutation>();

            StockMutation stockMutationVirtual = new StockMutation();
            stockMutationVirtual.ItemId = warehouseItem.ItemId;
            stockMutationVirtual.WarehouseId = warehouseItem.WarehouseId;
            stockMutationVirtual.WarehouseItemId = warehouseItem.Id;
            stockMutationVirtual.ItemId = temporaryDeliveryOrderDetail.ItemId;
            stockMutationVirtual.Quantity = temporaryDeliveryOrderDetail.WasteQuantity;
            stockMutationVirtual.SourceDocumentType = Constant.SourceDocumentType.TemporaryDeliveryOrder;
            stockMutationVirtual.SourceDocumentId = temporaryDeliveryOrderDetail.TemporaryDeliveryOrderId;
            stockMutationVirtual.SourceDocumentDetailType = Constant.SourceDocumentDetailType.TemporaryDeliveryOrderDetailWaste;
            stockMutationVirtual.SourceDocumentDetailId = temporaryDeliveryOrderDetail.Id;
            stockMutationVirtual.ItemCase = Constant.ItemCase.Virtual;
            stockMutationVirtual.Status = Constant.MutationStatus.Deduction;
            stockMutationVirtual.MutationDate = PushDate;
            stockMutationVirtual = _repository.CreateObject(stockMutationVirtual);

            StockMutation stockMutationReady = new StockMutation();
            stockMutationReady.ItemId = warehouseItem.ItemId;
            stockMutationReady.WarehouseId = warehouseItem.WarehouseId;
            stockMutationReady.WarehouseItemId = warehouseItem.Id;
            stockMutationReady.ItemId = temporaryDeliveryOrderDetail.ItemId;
            stockMutationReady.Quantity = temporaryDeliveryOrderDetail.WasteQuantity;
            stockMutationReady.SourceDocumentType = Constant.SourceDocumentType.TemporaryDeliveryOrder;
            stockMutationReady.SourceDocumentId = temporaryDeliveryOrderDetail.TemporaryDeliveryOrderId;
            stockMutationReady.SourceDocumentDetailType = Constant.SourceDocumentDetailType.TemporaryDeliveryOrderDetailWaste;
            stockMutationReady.SourceDocumentDetailId = temporaryDeliveryOrderDetail.Id;
            stockMutationReady.ItemCase = Constant.ItemCase.Ready;
            stockMutationReady.Status = Constant.MutationStatus.Deduction;
            stockMutationReady.MutationDate = PushDate;
            stockMutationReady = _repository.CreateObject(stockMutationReady);

            result.Add(stockMutationVirtual);
            result.Add(stockMutationReady);
            return result;
        }

        public IList<StockMutation> GetStockMutationForTemporaryDeliveryOrderWaste(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail, WarehouseItem warehouseItem)
        {
            IList<StockMutation> stockMutations = _repository.GetObjectsBySourceDocumentDetailForWarehouseItem(warehouseItem.Id, Constant.SourceDocumentDetailType.TemporaryDeliveryOrderDetailWaste, temporaryDeliveryOrderDetail.Id);
            return stockMutations;
        }

        public IList<StockMutation> CreateStockMutationForTemporaryDeliveryOrderRestock(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail, DateTime PushDate, WarehouseItem warehouseItem)
        {
            IList<StockMutation> result = new List<StockMutation>();

            StockMutation stockMutationVirtual = new StockMutation();
            stockMutationVirtual.ItemId = warehouseItem.ItemId;
            stockMutationVirtual.WarehouseId = warehouseItem.WarehouseId;
            stockMutationVirtual.WarehouseItemId = warehouseItem.Id;
            stockMutationVirtual.ItemId = temporaryDeliveryOrderDetail.ItemId;
            stockMutationVirtual.Quantity = temporaryDeliveryOrderDetail.RestockQuantity;
            stockMutationVirtual.SourceDocumentType = Constant.SourceDocumentType.TemporaryDeliveryOrder;
            stockMutationVirtual.SourceDocumentId = temporaryDeliveryOrderDetail.TemporaryDeliveryOrderId;
            stockMutationVirtual.SourceDocumentDetailType = Constant.SourceDocumentDetailType.TemporaryDeliveryOrderDetailRestock;
            stockMutationVirtual.SourceDocumentDetailId = temporaryDeliveryOrderDetail.Id;
            stockMutationVirtual.ItemCase = Constant.ItemCase.Virtual;
            stockMutationVirtual.Status = Constant.MutationStatus.Deduction;
            stockMutationVirtual.MutationDate = PushDate;
            stockMutationVirtual = _repository.CreateObject(stockMutationVirtual);

            result.Add(stockMutationVirtual);
            return result;
        }

        public IList<StockMutation> GetStockMutationForTemporaryDeliveryOrderRestock(TemporaryDeliveryOrderDetail temporaryDeliveryOrderDetail, WarehouseItem warehouseItem)
        {
            IList<StockMutation> stockMutations = _repository.GetObjectsBySourceDocumentDetailForWarehouseItem(warehouseItem.Id, Constant.SourceDocumentDetailType.TemporaryDeliveryOrderDetailRestock, temporaryDeliveryOrderDetail.Id);
            return stockMutations;
        }

        public IList<StockMutation> CreateStockMutationForTemporaryDeliveryOrderClearanceWaste(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail, DateTime PushDate, WarehouseItem warehouseItem)
        {
            IList<StockMutation> result = new List<StockMutation>();

            StockMutation stockMutationVirtual = new StockMutation();
            stockMutationVirtual.ItemId = warehouseItem.ItemId;
            stockMutationVirtual.WarehouseId = warehouseItem.WarehouseId;
            stockMutationVirtual.WarehouseItemId = warehouseItem.Id;
            stockMutationVirtual.Quantity = temporaryDeliveryOrderClearanceDetail.Quantity;
            stockMutationVirtual.SourceDocumentType = Constant.SourceDocumentType.TemporaryDeliveryOrderClearance;
            stockMutationVirtual.SourceDocumentId = temporaryDeliveryOrderClearanceDetail.TemporaryDeliveryOrderClearanceId;
            stockMutationVirtual.SourceDocumentDetailType = Constant.SourceDocumentDetailType.TemporaryDeliveryOrderClearanceDetailWaste;
            stockMutationVirtual.SourceDocumentDetailId = temporaryDeliveryOrderClearanceDetail.Id;
            stockMutationVirtual.ItemCase = Constant.ItemCase.Virtual;
            stockMutationVirtual.Status = Constant.MutationStatus.Deduction;
            stockMutationVirtual.MutationDate = PushDate;
            stockMutationVirtual = _repository.CreateObject(stockMutationVirtual);

            StockMutation stockMutationReady = new StockMutation();
            stockMutationReady.ItemId = warehouseItem.ItemId;
            stockMutationReady.WarehouseId = warehouseItem.WarehouseId;
            stockMutationReady.WarehouseItemId = warehouseItem.Id;
            stockMutationReady.Quantity = temporaryDeliveryOrderClearanceDetail.Quantity;
            stockMutationReady.SourceDocumentType = Constant.SourceDocumentType.TemporaryDeliveryOrderClearance;
            stockMutationReady.SourceDocumentId = temporaryDeliveryOrderClearanceDetail.TemporaryDeliveryOrderClearanceId;
            stockMutationReady.SourceDocumentDetailType = Constant.SourceDocumentDetailType.TemporaryDeliveryOrderClearanceDetailWaste;
            stockMutationReady.SourceDocumentDetailId = temporaryDeliveryOrderClearanceDetail.Id;
            stockMutationReady.ItemCase = Constant.ItemCase.Ready;
            stockMutationReady.Status = Constant.MutationStatus.Deduction;
            stockMutationReady.MutationDate = PushDate;
            stockMutationReady = _repository.CreateObject(stockMutationReady);

            result.Add(stockMutationVirtual);
            result.Add(stockMutationReady);
            return result;
        }

        public IList<StockMutation> GetStockMutationForTemporaryDeliveryOrderClearanceWaste(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail, WarehouseItem warehouseItem)
        {
            IList<StockMutation> stockMutations = _repository.GetObjectsBySourceDocumentDetailForWarehouseItem(warehouseItem.Id, Constant.SourceDocumentDetailType.TemporaryDeliveryOrderClearanceDetailWaste, temporaryDeliveryOrderClearanceDetail.Id);
            return stockMutations;
        }

        public IList<StockMutation> CreateStockMutationForTemporaryDeliveryOrderClearanceReturn(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail, DateTime PushDate, WarehouseItem warehouseItem)
        {
            IList<StockMutation> result = new List<StockMutation>();

            StockMutation stockMutationVirtual = new StockMutation();
            stockMutationVirtual.ItemId = warehouseItem.ItemId;
            stockMutationVirtual.WarehouseId = warehouseItem.WarehouseId;
            stockMutationVirtual.WarehouseItemId = warehouseItem.Id;
            stockMutationVirtual.Quantity = temporaryDeliveryOrderClearanceDetail.Quantity;
            stockMutationVirtual.SourceDocumentType = Constant.SourceDocumentType.TemporaryDeliveryOrderClearance;
            stockMutationVirtual.SourceDocumentId = temporaryDeliveryOrderClearanceDetail.TemporaryDeliveryOrderClearanceId;
            stockMutationVirtual.SourceDocumentDetailType = Constant.SourceDocumentDetailType.TemporaryDeliveryOrderClearanceDetailReturn;
            stockMutationVirtual.SourceDocumentDetailId = temporaryDeliveryOrderClearanceDetail.Id;
            stockMutationVirtual.ItemCase = Constant.ItemCase.Virtual;
            stockMutationVirtual.Status = Constant.MutationStatus.Deduction;
            stockMutationVirtual.MutationDate = PushDate;
            stockMutationVirtual = _repository.CreateObject(stockMutationVirtual);

            result.Add(stockMutationVirtual);
            return result;
        }

        public IList<StockMutation> GetStockMutationForTemporaryDeliveryOrderClearanceReturn(TemporaryDeliveryOrderClearanceDetail temporaryDeliveryOrderClearanceDetail, WarehouseItem warehouseItem)
        {
            IList<StockMutation> stockMutations = _repository.GetObjectsBySourceDocumentDetailForWarehouseItem(warehouseItem.Id, Constant.SourceDocumentDetailType.TemporaryDeliveryOrderClearanceDetailReturn, temporaryDeliveryOrderClearanceDetail.Id);
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
            stockMutation.MutationDate = (DateTime) stockAdjustmentDetail.ConfirmationDate;
            return _repository.CreateObject(stockMutation);
        }

        public IList<StockMutation> GetStockMutationForStockAdjustment(StockAdjustmentDetail stockAdjustmentDetail, WarehouseItem warehouseItem)
        {
            IList<StockMutation> stockMutations = _repository.GetObjectsBySourceDocumentDetailForWarehouseItem(warehouseItem.Id, Constant.SourceDocumentDetailType.StockAdjustmentDetail, stockAdjustmentDetail.Id);
            return stockMutations;
        }

        public StockMutation CreateStockMutationForCustomerStockAdjustment(CustomerStockAdjustmentDetail customerStockAdjustmentDetail, WarehouseItem warehouseItem)
        {
            StockMutation stockMutation = new StockMutation();
            stockMutation.ItemId = warehouseItem.ItemId;
            stockMutation.WarehouseId = warehouseItem.WarehouseId;
            stockMutation.WarehouseItemId = warehouseItem.Id;
            stockMutation.Quantity = (customerStockAdjustmentDetail.Quantity >= 0) ? customerStockAdjustmentDetail.Quantity : (-1) * customerStockAdjustmentDetail.Quantity;
            stockMutation.SourceDocumentType = Constant.SourceDocumentType.CustomerStockAdjustment;
            stockMutation.SourceDocumentId = customerStockAdjustmentDetail.CustomerStockAdjustmentId;
            stockMutation.SourceDocumentDetailType = Constant.SourceDocumentDetailType.CustomerStockAdjustmentDetail;
            stockMutation.SourceDocumentDetailId = customerStockAdjustmentDetail.Id;
            stockMutation.ItemCase = Constant.ItemCase.Ready;
            stockMutation.Status = (customerStockAdjustmentDetail.Quantity >= 0) ? Constant.MutationStatus.Addition : Constant.MutationStatus.Deduction;
            stockMutation.MutationDate = (DateTime)customerStockAdjustmentDetail.ConfirmationDate;
            return _repository.CreateObject(stockMutation);
        }

        public IList<StockMutation> GetStockMutationForCustomerStockAdjustment(CustomerStockAdjustmentDetail customerStockAdjustmentDetail, WarehouseItem warehouseItem)
        {
            IList<StockMutation> stockMutations = _repository.GetObjectsBySourceDocumentDetailForWarehouseItem(warehouseItem.Id, Constant.SourceDocumentDetailType.CustomerStockAdjustmentDetail, customerStockAdjustmentDetail.Id);
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
            stockMutation.MutationDate = (DateTime)coreIdentificationDetail.ConfirmationDate;
            return _repository.CreateObject(stockMutation);
        }

        public IList<StockMutation> GetStockMutationForCoreIdentification(CoreIdentificationDetail coreIdentificationDetail, WarehouseItem warehouseItem)
        {
            IList<StockMutation> stockMutations = _repository.GetObjectsBySourceDocumentDetailForWarehouseItem(warehouseItem.Id, Constant.SourceDocumentDetailType.CoreIdentificationDetail, coreIdentificationDetail.Id);
            return stockMutations;
        }

        public StockMutation CreateStockMutationForRecoveryOrder(RecoveryOrderDetail recoveryOrderDetail, DateTime FinishedOrRejectedDate, WarehouseItem warehouseItem, bool CaseAddition)
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
            stockMutation.MutationDate = FinishedOrRejectedDate;
            return _repository.CreateObject(stockMutation);
        }

        public StockMutation CreateStockMutationForRecoveryOrderCompound(RecoveryOrderDetail recoveryOrderDetail, DateTime FinishedOrRejectedDate, WarehouseItem warehouseItem, bool CaseAddition)
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
            stockMutation.MutationDate = FinishedOrRejectedDate;
            return _repository.CreateObject(stockMutation);
        }

        public StockMutation CreateStockMutationForRecoveryOrderCompoundUnderLayer(RecoveryOrderDetail recoveryOrderDetail, DateTime FinishedOrRejectedDate, WarehouseItem warehouseItem, bool CaseAddition)
        {
            StockMutation stockMutation = new StockMutation();
            stockMutation.ItemId = warehouseItem.ItemId;
            stockMutation.WarehouseId = warehouseItem.WarehouseId;
            stockMutation.WarehouseItemId = warehouseItem.Id;
            stockMutation.Quantity = recoveryOrderDetail.CompoundUnderLayerUsage;
            stockMutation.SourceDocumentType = Constant.SourceDocumentType.RecoveryOrder;
            stockMutation.SourceDocumentId = recoveryOrderDetail.RecoveryOrderId;
            stockMutation.SourceDocumentDetailType = Constant.SourceDocumentDetailType.RecoveryOrderDetail;
            stockMutation.SourceDocumentDetailId = recoveryOrderDetail.Id;
            stockMutation.ItemCase = Constant.ItemCase.Ready;
            stockMutation.Status = CaseAddition ? Constant.MutationStatus.Addition : Constant.MutationStatus.Deduction;
            stockMutation.MutationDate = FinishedOrRejectedDate;
            return _repository.CreateObject(stockMutation);
        }

        public IList<StockMutation> GetStockMutationForRecoveryOrder(RecoveryOrderDetail recoveryOrderDetail, WarehouseItem warehouseItem)
        {
            IList<StockMutation> stockMutations = _repository.GetObjectsBySourceDocumentDetailForWarehouseItem(warehouseItem.Id, Constant.SourceDocumentDetailType.RecoveryOrderDetail, recoveryOrderDetail.Id);
            return stockMutations;
        }

        public StockMutation CreateStockMutationForRecoveryAccessory(RecoveryAccessoryDetail recoveryAccessoryDetail, DateTime FinishedDate, WarehouseItem warehouseItem)
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
            stockMutation.MutationDate = FinishedDate;
            return _repository.CreateObject(stockMutation);
        }

        public IList<StockMutation> GetStockMutationForRecoveryAccessory(RecoveryAccessoryDetail recoveryAccessoryDetail, WarehouseItem warehouseItem)
        {
            IList<StockMutation> stockMutations = _repository.GetObjectsBySourceDocumentDetailForWarehouseItem(warehouseItem.Id, Constant.SourceDocumentDetailType.RecoveryAccessoryDetail, recoveryAccessoryDetail.Id);
            return stockMutations;
        }

        public StockMutation CreateStockMutationForBlanketOrder(BlanketOrderDetail blanketOrderDetail, decimal Usage, DateTime FinishedOrRejectedDate, WarehouseItem warehouseItem, bool CaseAddition)
        {
            StockMutation stockMutation = new StockMutation();
            stockMutation.ItemId = warehouseItem.ItemId;
            stockMutation.WarehouseId = warehouseItem.WarehouseId;
            stockMutation.WarehouseItemId = warehouseItem.Id;
            stockMutation.Quantity = Usage;
            stockMutation.SourceDocumentType = Constant.SourceDocumentType.BlanketOrder;
            stockMutation.SourceDocumentId = blanketOrderDetail.BlanketOrderId;
            stockMutation.SourceDocumentDetailType = Constant.SourceDocumentDetailType.BlanketOrderDetail;
            stockMutation.SourceDocumentDetailId = blanketOrderDetail.Id;
            stockMutation.ItemCase = Constant.ItemCase.Ready;
            stockMutation.Status = CaseAddition ? Constant.MutationStatus.Addition : Constant.MutationStatus.Deduction;
            stockMutation.MutationDate = FinishedOrRejectedDate;
            return _repository.CreateObject(stockMutation);
        }

        public IList<StockMutation> GetStockMutationForBlanketOrder(BlanketOrderDetail blanketOrderDetail, WarehouseItem warehouseItem)
        {
            IList<StockMutation> stockMutations = _repository.GetObjectsBySourceDocumentDetailForWarehouseItem(warehouseItem.Id, Constant.SourceDocumentDetailType.BlanketOrderDetail, blanketOrderDetail.Id);
            return stockMutations;
        }

        public StockMutation CreateStockMutationForBlendingWorkOrderSource(BlendingWorkOrder blendingWorkOrder, BlendingRecipeDetail blendingRecipeDetail, WarehouseItem warehouseItem)
        {
            StockMutation stockMutation = new StockMutation();
            stockMutation.ItemId = blendingRecipeDetail.ItemId;
            stockMutation.WarehouseId = blendingWorkOrder.WarehouseId;
            stockMutation.WarehouseItemId = warehouseItem.Id;
            stockMutation.Quantity = blendingRecipeDetail.Quantity;
            stockMutation.SourceDocumentType = Constant.SourceDocumentType.BlendingWorkOrder;
            stockMutation.SourceDocumentId = blendingWorkOrder.Id;
            stockMutation.SourceDocumentDetailType = Constant.SourceDocumentDetailType.BlendingRecipeDetail;
            stockMutation.SourceDocumentDetailId = blendingRecipeDetail.Id;
            stockMutation.ItemCase = Constant.ItemCase.Ready;
            stockMutation.Status = Constant.MutationStatus.Deduction;
            stockMutation.MutationDate = blendingWorkOrder.ConfirmationDate.GetValueOrDefault();
            return _repository.CreateObject(stockMutation);
        }

        public IList<StockMutation> GetStockMutationForBlendingWorkOrderSource(BlendingRecipeDetail blendingRecipeDetail, WarehouseItem warehouseItem)
        {
            IList<StockMutation> stockMutations = _repository.GetObjectsBySourceDocumentDetailForWarehouseItem(warehouseItem.Id, Constant.SourceDocumentDetailType.BlendingRecipeDetail, blendingRecipeDetail.Id);
            return stockMutations;
        }

        public StockMutation CreateStockMutationForBlendingWorkOrderTarget(BlendingWorkOrder blendingWorkOrder, BlendingRecipe blendingRecipe, WarehouseItem warehouseItem)
        {
            StockMutation stockMutation = new StockMutation();
            stockMutation.ItemId = blendingRecipe.TargetItemId;
            stockMutation.WarehouseId = blendingWorkOrder.WarehouseId;
            stockMutation.WarehouseItemId = warehouseItem.Id;
            stockMutation.Quantity = blendingRecipe.TargetQuantity;
            stockMutation.SourceDocumentType = Constant.SourceDocumentType.BlendingWorkOrder;
            stockMutation.SourceDocumentId = blendingWorkOrder.Id;
            stockMutation.SourceDocumentDetailType = Constant.SourceDocumentDetailType.BlendingRecipe;
            stockMutation.SourceDocumentDetailId = blendingRecipe.Id;
            stockMutation.ItemCase = Constant.ItemCase.Ready;
            stockMutation.Status = Constant.MutationStatus.Addition;
            stockMutation.MutationDate = blendingWorkOrder.ConfirmationDate.GetValueOrDefault();
            return _repository.CreateObject(stockMutation);
        }

        public IList<StockMutation> GetStockMutationForBlendingWorkOrderTarget(BlendingRecipe blendingRecipe, WarehouseItem warehouseItem)
        {
            IList<StockMutation> stockMutations = _repository.GetObjectsBySourceDocumentDetailForWarehouseItem(warehouseItem.Id, Constant.SourceDocumentDetailType.BlendingRecipe, blendingRecipe.Id);
            return stockMutations;
        }

        public StockMutation CreateStockMutationForRepackingSource(Repacking repacking, BlendingRecipeDetail blendingRecipeDetail, WarehouseItem warehouseItem)
        {
            StockMutation stockMutation = new StockMutation();
            stockMutation.ItemId = blendingRecipeDetail.ItemId;
            stockMutation.WarehouseId = repacking.WarehouseId;
            stockMutation.WarehouseItemId = warehouseItem.Id;
            stockMutation.Quantity = blendingRecipeDetail.Quantity;
            stockMutation.SourceDocumentType = Constant.SourceDocumentType.Repacking;
            stockMutation.SourceDocumentId = repacking.Id;
            stockMutation.SourceDocumentDetailType = Constant.SourceDocumentDetailType.BlendingRecipeDetail;
            stockMutation.SourceDocumentDetailId = blendingRecipeDetail.Id;
            stockMutation.ItemCase = Constant.ItemCase.Ready;
            stockMutation.Status = Constant.MutationStatus.Deduction;
            stockMutation.MutationDate = repacking.ConfirmationDate.GetValueOrDefault();
            return _repository.CreateObject(stockMutation);
        }

        public IList<StockMutation> GetStockMutationForRepackingSource(BlendingRecipeDetail blendingRecipeDetail, WarehouseItem warehouseItem)
        {
            IList<StockMutation> stockMutations = _repository.GetObjectsBySourceDocumentDetailForWarehouseItem(warehouseItem.Id, Constant.SourceDocumentDetailType.BlendingRecipeDetail, blendingRecipeDetail.Id);
            return stockMutations;
        }

        public StockMutation CreateStockMutationForRepackingTarget(Repacking repacking, BlendingRecipe blendingRecipe, WarehouseItem warehouseItem)
        {
            StockMutation stockMutation = new StockMutation();
            stockMutation.ItemId = blendingRecipe.TargetItemId;
            stockMutation.WarehouseId = repacking.WarehouseId;
            stockMutation.WarehouseItemId = warehouseItem.Id;
            stockMutation.Quantity = blendingRecipe.TargetQuantity;
            stockMutation.SourceDocumentType = Constant.SourceDocumentType.Repacking;
            stockMutation.SourceDocumentId = repacking.Id;
            stockMutation.SourceDocumentDetailType = Constant.SourceDocumentDetailType.BlendingRecipe;
            stockMutation.SourceDocumentDetailId = blendingRecipe.Id;
            stockMutation.ItemCase = Constant.ItemCase.Ready;
            stockMutation.Status = Constant.MutationStatus.Addition;
            stockMutation.MutationDate = repacking.ConfirmationDate.GetValueOrDefault();
            return _repository.CreateObject(stockMutation);
        }

        public IList<StockMutation> GetStockMutationForRepackingTarget(BlendingRecipe blendingRecipe, WarehouseItem warehouseItem)
        {
            IList<StockMutation> stockMutations = _repository.GetObjectsBySourceDocumentDetailForWarehouseItem(warehouseItem.Id, Constant.SourceDocumentDetailType.BlendingRecipe, blendingRecipe.Id);
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
            stockMutationFrom.MutationDate = (DateTime) rollerWarehouseMutationDetail.ConfirmationDate;
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
            stockMutationTo.MutationDate = (DateTime)rollerWarehouseMutationDetail.ConfirmationDate;
            stockMutationTo = _repository.CreateObject(stockMutationTo);

            stockMutations.Add(stockMutationFrom);
            stockMutations.Add(stockMutationTo);
            return stockMutations;
        }

        public IList<StockMutation> GetStockMutationForRollerWarehouseMutation(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, WarehouseItem warehouseItemFrom, WarehouseItem warehouseItemTo)
        {
            IList<StockMutation> stockMutations = new List<StockMutation>();

            IList<StockMutation> stockMutationFrom = _repository.GetObjectsBySourceDocumentDetailForWarehouseItem(warehouseItemFrom.Id, Constant.SourceDocumentDetailType.RollerWarehouseMutationDetail, rollerWarehouseMutationDetail.Id);
            stockMutationFrom.ToList().ForEach(x => stockMutations.Add(x));

            IList<StockMutation> stockMutationTo = _repository.GetObjectsBySourceDocumentDetailForWarehouseItem(warehouseItemTo.Id, Constant.SourceDocumentDetailType.RollerWarehouseMutationDetail, rollerWarehouseMutationDetail.Id);
            stockMutationTo.ToList().ForEach(x => stockMutations.Add(x));

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
            stockMutationFrom.MutationDate = (DateTime)WarehouseMutationDetail.ConfirmationDate;
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
            stockMutationTo.MutationDate = (DateTime)WarehouseMutationDetail.ConfirmationDate;
            stockMutationTo = _repository.CreateObject(stockMutationTo);

            stockMutations.Add(stockMutationFrom);
            stockMutations.Add(stockMutationTo);
            return stockMutations;
        }

        public IList<StockMutation> GetStockMutationForWarehouseMutation(WarehouseMutationDetail WarehouseMutationDetail, WarehouseItem warehouseItemFrom, WarehouseItem warehouseItemTo)
        {
            IList<StockMutation> stockMutations = new List<StockMutation>();

            IList<StockMutation> stockMutationFrom = _repository.GetObjectsBySourceDocumentDetailForWarehouseItem(warehouseItemFrom.Id, Constant.SourceDocumentDetailType.WarehouseMutationDetail, WarehouseMutationDetail.Id);
            stockMutationFrom.ToList().ForEach(x => stockMutations.Add(x));

            IList<StockMutation> stockMutationTo = _repository.GetObjectsBySourceDocumentDetailForWarehouseItem(warehouseItemTo.Id, Constant.SourceDocumentDetailType.WarehouseMutationDetail, WarehouseMutationDetail.Id);
            stockMutationTo.ToList().ForEach(x => stockMutations.Add(x));

            return stockMutations;
        }

        public void StockMutateObject(StockMutation stockMutation, IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService)
        {
            // decimal stockAdjustmentDetailPrice = (stockMutation.Status == Constant.MutationStatus.Addition) ? stockAdjustmentDetail.Price : ((-1) * stockAdjustmentDetail.Price);
            // item.AvgCost = _blanketService.CalculateAvgCost(item, stockAdjustmentDetail.Quantity, stockAdjustmentDetailPrice);
            // blanket.AvgCost = _blanketService.CalculateAvgCost(blanket, stockAdjustmentDetail.Quantity, stockAdjustmentDetailPrice);

            decimal Quantity = (stockMutation.Status == Constant.MutationStatus.Addition) ? stockMutation.Quantity : (-1) * stockMutation.Quantity;
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
                { _itemService.AdjustPendingDelivery(item, (int) Quantity); }
                else if (stockMutation.ItemCase == Constant.ItemCase.PendingReceival)
                { _itemService.AdjustPendingReceival(item, (int) Quantity); }
                else if (stockMutation.ItemCase == Constant.ItemCase.Virtual)
                { _itemService.AdjustVirtual(item, (int) Quantity); }
            }
            else
            {
                // blanketService in action
                if (stockMutation.ItemCase == Constant.ItemCase.Ready)
                { _blanketService.AdjustQuantity(blanket, Quantity); }
                else if (stockMutation.ItemCase == Constant.ItemCase.PendingDelivery)
                { _blanketService.AdjustPendingDelivery(blanket, (int) Quantity); }
                else if (stockMutation.ItemCase == Constant.ItemCase.PendingReceival)
                { _blanketService.AdjustPendingReceival(blanket, (int) Quantity); }
            }
        }

        public void ReverseStockMutateObject(StockMutation stockMutation, IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService)
        {
            // decimal stockAdjustmentDetailPrice = (stockMutation.Status == Constant.MutationStatus.Addition) ? stockAdjustmentDetail.Price : ((-1) * stockAdjustmentDetail.Price);

            decimal Quantity = (stockMutation.Status == Constant.MutationStatus.Deduction) ? stockMutation.Quantity : (-1) * stockMutation.Quantity;
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
                { _itemService.AdjustPendingDelivery(item, (int) Quantity); }
                else if (stockMutation.ItemCase == Constant.ItemCase.PendingReceival)
                { _itemService.AdjustPendingReceival(item, (int) Quantity); }
                else if (stockMutation.ItemCase == Constant.ItemCase.Virtual)
                { _itemService.AdjustVirtual(item, (int) Quantity); }
            }
            else
            {
                // blanketService in action
                if (stockMutation.ItemCase == Constant.ItemCase.Ready)
                { _blanketService.AdjustQuantity(blanket, Quantity); }
                else if (stockMutation.ItemCase == Constant.ItemCase.PendingDelivery)
                { _blanketService.AdjustPendingDelivery(blanket, (int) Quantity); }
                else if (stockMutation.ItemCase == Constant.ItemCase.PendingReceival)
                { _blanketService.AdjustPendingReceival(blanket, (int) Quantity); }
            }

            _repository.Delete(stockMutation);
        }
    }
}
