﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Text.RegularExpressions;
using Core.Interface.Validation;
using Core.DomainModel;
using Core.Interface.Service;

namespace Validation.Validation
{
    public class StockAdjustmentDetailValidator : IStockAdjustmentDetailValidator
    {
        public StockAdjustmentDetail VHasStockAdjustment(StockAdjustmentDetail stockAdjustmentDetail, IStockAdjustmentService _stockAdjustmentService)
        {
            StockAdjustment stockAdjustment = _stockAdjustmentService.GetObjectById(stockAdjustmentDetail.StockAdjustmentId);
            if (stockAdjustment == null)
            {
                stockAdjustmentDetail.Errors.Add("StockAdjustmentId", "Tidak terasosiasi dengan Stock Adjustment");
            }
            return stockAdjustmentDetail;
        }

        public StockAdjustmentDetail VHasItem(StockAdjustmentDetail stockAdjustmentDetail, IItemService _itemService)
        {
            Item item = _itemService.GetObjectById(stockAdjustmentDetail.ItemId);
            if (item == null)
            {
                stockAdjustmentDetail.Errors.Add("ItemId", "Tidak terasosiasi dengan item");
            }
            return stockAdjustmentDetail;
        }

        public StockAdjustmentDetail VHasWarehouseItem(StockAdjustmentDetail stockAdjustmentDetail, IStockAdjustmentService _stockAdjustmentService, IWarehouseItemService _warehouseItemService)
        {
            StockAdjustment stockAdjustment = _stockAdjustmentService.GetObjectById(stockAdjustmentDetail.StockAdjustmentId);
            WarehouseItem warehouseItem = _warehouseItemService.FindOrCreateObject(stockAdjustment.WarehouseId, stockAdjustmentDetail.ItemId);
            if (warehouseItem == null)
            {
                stockAdjustmentDetail.Errors.Add("Generic", "Tidak terasosiasi dengan warehouse");
            }
            return stockAdjustmentDetail;
        }

        public StockAdjustmentDetail VNonZeroQuantity(StockAdjustmentDetail stockAdjustmentDetail)
        {
            if (stockAdjustmentDetail.Quantity == 0)
            {
                stockAdjustmentDetail.Errors.Add("Quantity", "Tidak boleh 0");
            }
            return stockAdjustmentDetail;
        }

        /*
        public StockAdjustmentDetail VNonZeroNorNegativePrice(StockAdjustmentDetail stockAdjustmentDetail)
        {
            if (stockAdjustmentDetail.Price <= 0)
            {
                stockAdjustmentDetail.Errors.Add("Price", "Harus lebih besar dari 0");
            }
            return stockAdjustmentDetail;
        }
        */

        public StockAdjustmentDetail VUniqueItem(StockAdjustmentDetail stockAdjustmentDetail, IStockAdjustmentDetailService _stockAdjustmentDetailService, IItemService _itemService)
        {
            IList<StockAdjustmentDetail> details = _stockAdjustmentDetailService.GetObjectsByStockAdjustmentId(stockAdjustmentDetail.StockAdjustmentId);
            foreach (var detail in details)
            {
                if (detail.ItemId == stockAdjustmentDetail.ItemId && detail.Id != stockAdjustmentDetail.Id)
                {
                     stockAdjustmentDetail.Errors.Add("ItemId", "Tidak boleh ada duplikasi item dalam 1 Stock Adjustment");
                }
            }
            return stockAdjustmentDetail;
        }

        public StockAdjustmentDetail VHasNotBeenFinished(StockAdjustmentDetail stockAdjustmentDetail)
        {
            if (stockAdjustmentDetail.IsFinished)
            {
                stockAdjustmentDetail.Errors.Add("Generic", "Tidak boleh sudah selesai");
            }
            return stockAdjustmentDetail;
        }

        public StockAdjustmentDetail VHasBeenFinished(StockAdjustmentDetail stockAdjustmentDetail)
        {
            if (!stockAdjustmentDetail.IsFinished)
            {
                stockAdjustmentDetail.Errors.Add("Generic", "Harus sudah selesai");
            }
            return stockAdjustmentDetail;
        }

        public StockAdjustmentDetail VStockAdjustmentHasBeenConfirmed(StockAdjustmentDetail stockAdjustmentDetail, IStockAdjustmentService _stockAdjustmentService)
        {
            StockAdjustment stockAdjustment = _stockAdjustmentService.GetObjectById(stockAdjustmentDetail.StockAdjustmentId);
            if (!stockAdjustment.IsConfirmed)
            {
                stockAdjustmentDetail.Errors.Add("Generic", "Stock adjustment belum di konfirmasi");
                return stockAdjustmentDetail;
            }
            return stockAdjustmentDetail;
        }

        public StockAdjustmentDetail VStockAdjustmentHasNotBeenCompleted(StockAdjustmentDetail stockAdjustmentDetail, IStockAdjustmentService _stockAdjustmentService)
        {
            StockAdjustment stockAdjustment = _stockAdjustmentService.GetObjectById(stockAdjustmentDetail.StockAdjustmentId);
            if (stockAdjustment.IsCompleted)
            {
                stockAdjustmentDetail.Errors.Add("Generic", "Stock adjustment sudah selesai");
                return stockAdjustmentDetail;
            }
            return stockAdjustmentDetail;
        }

        public StockAdjustmentDetail VNonNegativeStockQuantity(StockAdjustmentDetail stockAdjustmentDetail, IStockAdjustmentService _stockAdjustmentService,
                                                               IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService, bool ToConfirm)
        {
            int stockAdjustmentDetailQuantity = ToConfirm ? stockAdjustmentDetail.Quantity : ((-1) * stockAdjustmentDetail.Quantity);
            //decimal stockAdjustmentDetailPrice = ToConfirm ? stockAdjustmentDetail.Price : ((-1) * stockAdjustmentDetail.Price);
            Item item = _itemService.GetObjectById(stockAdjustmentDetail.ItemId);
            StockAdjustment stockAdjustment = _stockAdjustmentService.GetObjectById(stockAdjustmentDetail.StockAdjustmentId);
            WarehouseItem warehouseItem = _warehouseItemService.FindOrCreateObject(stockAdjustment.WarehouseId, item.Id);
            if (item.Quantity + stockAdjustmentDetailQuantity < 0)
            {
                stockAdjustmentDetail.Errors.Add("Quantity", "Stock barang tidak boleh menjadi kurang dari 0");
                return stockAdjustmentDetail;
            }
            if (warehouseItem.Quantity + stockAdjustmentDetailQuantity < 0)
            {
                stockAdjustmentDetail.Errors.Add("Quantity", "Stock di dalam warehouse tidak boleh kurang dari 0");
            }
            /*
            if (_itemService.CalculateAvgCost(item, stockAdjustmentDetail.Quantity, stockAdjustmentDetailPrice) < 0)
            {
                stockAdjustmentDetail.Errors.Add("AvgCost", "Tidak boleh kurang dari 0");
            }
            */
            return stockAdjustmentDetail;
        }

        public StockAdjustmentDetail VCreateObject(StockAdjustmentDetail stockAdjustmentDetail, IStockAdjustmentDetailService _stockAdjustmentDetailService,
                                                   IStockAdjustmentService _stockAdjustmentService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            VHasStockAdjustment(stockAdjustmentDetail, _stockAdjustmentService);
            if (!isValid(stockAdjustmentDetail)) { return stockAdjustmentDetail; }
            VHasItem(stockAdjustmentDetail, _itemService);
            if (!isValid(stockAdjustmentDetail)) { return stockAdjustmentDetail; }
            VHasWarehouseItem(stockAdjustmentDetail, _stockAdjustmentService, _warehouseItemService);
            if (!isValid(stockAdjustmentDetail)) { return stockAdjustmentDetail; }
            VNonZeroQuantity(stockAdjustmentDetail);
            // if (!isValid(stockAdjustmentDetail)) { return stockAdjustmentDetail; }
            // VNonZeroNorNegativePrice(stockAdjustmentDetail);
            if (!isValid(stockAdjustmentDetail)) { return stockAdjustmentDetail; }
            VUniqueItem(stockAdjustmentDetail, _stockAdjustmentDetailService, _itemService);
            return stockAdjustmentDetail;
        }

        public StockAdjustmentDetail VUpdateObject(StockAdjustmentDetail stockAdjustmentDetail, IStockAdjustmentDetailService _stockAdjustmentDetailService,
                                                   IStockAdjustmentService _stockAdjustmentService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            VHasNotBeenFinished(stockAdjustmentDetail);
            if (!isValid(stockAdjustmentDetail)) { return stockAdjustmentDetail; }
            VCreateObject(stockAdjustmentDetail, _stockAdjustmentDetailService, _stockAdjustmentService, _itemService, _warehouseItemService);
            return stockAdjustmentDetail;
        }

        public StockAdjustmentDetail VDeleteObject(StockAdjustmentDetail stockAdjustmentDetail)
        {
            VHasNotBeenFinished(stockAdjustmentDetail);
            return stockAdjustmentDetail;
        }

        public StockAdjustmentDetail VFinishObject(StockAdjustmentDetail stockAdjustmentDetail, IStockAdjustmentService _stockAdjustmentService,
                                                    IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            VStockAdjustmentHasBeenConfirmed(stockAdjustmentDetail, _stockAdjustmentService);
            if (!isValid(stockAdjustmentDetail)) { return stockAdjustmentDetail; }
            VNonNegativeStockQuantity(stockAdjustmentDetail, _stockAdjustmentService, _itemService, _barringService, _warehouseItemService, true);
            return stockAdjustmentDetail;
        }

        public StockAdjustmentDetail VUnfinishObject(StockAdjustmentDetail stockAdjustmentDetail, IStockAdjustmentService _stockAdjustmentService,
                                                      IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            VStockAdjustmentHasBeenConfirmed(stockAdjustmentDetail, _stockAdjustmentService);
            if (!isValid(stockAdjustmentDetail)) { return stockAdjustmentDetail; }
            VStockAdjustmentHasNotBeenCompleted(stockAdjustmentDetail, _stockAdjustmentService);
            if (!isValid(stockAdjustmentDetail)) { return stockAdjustmentDetail; }
            VHasBeenFinished(stockAdjustmentDetail);
            if (!isValid(stockAdjustmentDetail)) { return stockAdjustmentDetail; }
            VNonNegativeStockQuantity(stockAdjustmentDetail, _stockAdjustmentService, _itemService, _barringService, _warehouseItemService, false);
            return stockAdjustmentDetail;
        }

        public bool ValidCreateObject(StockAdjustmentDetail stockAdjustmentDetail, IStockAdjustmentDetailService _stockAdjustmentDetailService,
                                      IStockAdjustmentService _stockAdjustmentService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            VCreateObject(stockAdjustmentDetail, _stockAdjustmentDetailService, _stockAdjustmentService, _itemService, _warehouseItemService);
            return isValid(stockAdjustmentDetail);
        }

        public bool ValidUpdateObject(StockAdjustmentDetail stockAdjustmentDetail, IStockAdjustmentDetailService _stockAdjustmentDetailService,
                                      IStockAdjustmentService _stockAdjustmentService, IItemService _itemService, IWarehouseItemService _warehouseItemService)
        {
            stockAdjustmentDetail.Errors.Clear();
            VUpdateObject(stockAdjustmentDetail, _stockAdjustmentDetailService, _stockAdjustmentService, _itemService, _warehouseItemService);
            return isValid(stockAdjustmentDetail);
        }

        public bool ValidDeleteObject(StockAdjustmentDetail stockAdjustmentDetail)
        {
            stockAdjustmentDetail.Errors.Clear();
            VDeleteObject(stockAdjustmentDetail);
            return isValid(stockAdjustmentDetail);
        }

        public bool ValidFinishObject(StockAdjustmentDetail stockAdjustmentDetail, IStockAdjustmentService _stockAdjustmentService,
                                       IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            stockAdjustmentDetail.Errors.Clear();
            VFinishObject(stockAdjustmentDetail, _stockAdjustmentService, _itemService, _barringService, _warehouseItemService);
            return isValid(stockAdjustmentDetail);
        }

        public bool ValidUnfinishObject(StockAdjustmentDetail stockAdjustmentDetail, IStockAdjustmentService _stockAdjustmentService,
                                         IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            stockAdjustmentDetail.Errors.Clear();
            VUnfinishObject(stockAdjustmentDetail, _stockAdjustmentService, _itemService, _barringService, _warehouseItemService);
            return isValid(stockAdjustmentDetail);
        }

        public bool isValid(StockAdjustmentDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(StockAdjustmentDetail obj)
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