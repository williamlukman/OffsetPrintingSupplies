using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Validation;
using Core.DomainModel;
using Core.Interface.Service;
using Core.Constants;

namespace Validation.Validation
{
    public class StockMutationValidator : IStockMutationValidator
    {
        public StockMutation VHasWarehouse(StockMutation stockMutation, IWarehouseService _warehouseService)
        {
            Warehouse warehouse = _warehouseService.GetObjectById(stockMutation.WarehouseId);
            if (warehouse == null)
            {
                stockMutation.Errors.Add("WarehouseId", "Tidak terasosiasi dengan warehouse");
            }
            return stockMutation;
        }

        public StockMutation VHasWarehouseItem(StockMutation stockMutation, IWarehouseItemService _warehouseItemService)
        {
            WarehouseItem warehouseItem = _warehouseItemService.GetObjectById(stockMutation.WarehouseItemId);
            if (warehouseItem == null)
            {
                stockMutation.Errors.Add("WarehouseItemId", "Tidak terasosiasi dengan item di warehouse");
            }
            return stockMutation;
        }

        public StockMutation VItemCase(StockMutation stockMutation)
        {
            if (!stockMutation.ItemCase.Equals (Constant.ItemCase.Ready) &&
                !stockMutation.ItemCase.Equals (Constant.ItemCase.PendingDelivery) &&
                !stockMutation.ItemCase.Equals (Constant.ItemCase.PendingReceival))
            {
                stockMutation.Errors.Add("ItemCase", "Harus merupakan bagian dari Constant.ItemCase");
            }
            return stockMutation;
        }

        public StockMutation VStatus(StockMutation stockMutation)
        {
            if (!stockMutation.Status.Equals(Constant.MutationStatus.Addition) &&
                !stockMutation.Status.Equals(Constant.MutationStatus.Deduction))
            {
                stockMutation.Errors.Add("Status", "Harus merupakan bagian dari Constant.MutationStatus");
            }
            return stockMutation;
        }

        public StockMutation VSourceDocumentType(StockMutation stockMutation)
        {
            if (!stockMutation.SourceDocumentType.Equals(Constant.SourceDocumentType.PurchaseOrder) &&
                !stockMutation.SourceDocumentType.Equals(Constant.SourceDocumentType.PurchaseReceival) &&
                !stockMutation.SourceDocumentType.Equals(Constant.SourceDocumentType.SalesOrder) &&
                !stockMutation.SourceDocumentType.Equals(Constant.SourceDocumentType.DeliveryOrder) &&
                !stockMutation.SourceDocumentType.Equals(Constant.SourceDocumentType.RecoveryOrder) &&
                !stockMutation.SourceDocumentType.Equals(Constant.SourceDocumentType.RecoveryOrderDetail))
            {
                stockMutation.Errors.Add("SourceDocumentType", "Harus merupakan bagian dari Constant.SourceDocumentType");
            }
            return stockMutation;
        }

        public StockMutation VSourceDocumentDetailType(StockMutation stockMutation)
        {
            if (!stockMutation.SourceDocumentDetailType.Equals(Constant.SourceDocumentDetailType.PurchaseOrderDetail) &&
                !stockMutation.SourceDocumentDetailType.Equals(Constant.SourceDocumentDetailType.PurchaseReceivalDetail) &&
                !stockMutation.SourceDocumentDetailType.Equals(Constant.SourceDocumentDetailType.SalesOrderDetail) &&
                !stockMutation.SourceDocumentDetailType.Equals(Constant.SourceDocumentDetailType.DeliveryOrderDetail) &&
                !stockMutation.SourceDocumentDetailType.Equals(Constant.SourceDocumentDetailType.RecoveryOrderDetail) &&
                !stockMutation.SourceDocumentDetailType.Equals(Constant.SourceDocumentDetailType.RecoveryAccessoryDetail))
            {
                stockMutation.Errors.Add("SourceDocumentDetailType", "Harus merupakan bagian dari Constant.SourceDocumentDetailType");
            }
            return stockMutation;
        }

        public StockMutation VNonNegativeNorZeroQuantity(StockMutation stockMutation)
        {
            if (stockMutation.Quantity <= 0)
            {
                stockMutation.Errors.Add("Quantity", "Tidak boleh negatif atau 0");
            }
            return stockMutation;
        }

        public StockMutation VCreateObject(StockMutation stockMutation, IWarehouseService _warehouseService, IWarehouseItemService _warehouseItemService)
        {
            VHasWarehouse(stockMutation, _warehouseService);
            if (!isValid(stockMutation)) { return stockMutation; }
            VHasWarehouseItem(stockMutation, _warehouseItemService);
            if (!isValid(stockMutation)) { return stockMutation; }
            VItemCase(stockMutation);
            if (!isValid(stockMutation)) { return stockMutation; }
            VStatus(stockMutation);
            if (!isValid(stockMutation)) { return stockMutation; }
            VSourceDocumentType(stockMutation);
            if (!isValid(stockMutation)) { return stockMutation; }
            VSourceDocumentDetailType(stockMutation);
            if (!isValid(stockMutation)) { return stockMutation; }
            VNonNegativeNorZeroQuantity(stockMutation);
            return stockMutation;
        }

        public StockMutation VUpdateObject(StockMutation stockMutation, IWarehouseService _warehouseService, IWarehouseItemService _warehouseItemService)
        {
            VCreateObject(stockMutation, _warehouseService, _warehouseItemService);
            return stockMutation;
        }

        public StockMutation VDeleteObject(StockMutation stockMutation, IWarehouseService _warehouseService, IWarehouseItemService _warehouseItemService)
        {
            return stockMutation;
        }

        public bool ValidCreateObject(StockMutation stockMutation, IWarehouseService _warehouseService, IWarehouseItemService _warehouseItemService)
        {
            VCreateObject(stockMutation, _warehouseService, _warehouseItemService);
            return isValid(stockMutation);
        }

        public bool ValidUpdateObject(StockMutation stockMutation, IWarehouseService _warehouseService, IWarehouseItemService _warehouseItemService)
        {
            stockMutation.Errors.Clear();
            VUpdateObject(stockMutation, _warehouseService, _warehouseItemService);
            return isValid(stockMutation);
        }

        public bool ValidDeleteObject(StockMutation stockMutation, IWarehouseService _warehouseService, IWarehouseItemService _warehouseItemService)
        {
            stockMutation.Errors.Clear();
            VDeleteObject(stockMutation, _warehouseService, _warehouseItemService);
            return isValid(stockMutation);
        }

        public bool isValid(StockMutation obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(StockMutation obj)
        {
            string erroroutput = "";
            KeyValuePair<string, string> first = obj.Errors.ElementAt(0);
            erroroutput += first.Key + "," + first.Value;
            foreach (KeyValuePair<string, string> pair in obj.Errors.Skip(1))
            {
                erroroutput += Environment.NewLine;
                erroroutput += pair.Key + "," + pair.Value;
            }
            return erroroutput;
        }

    }
}
