using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Core.Interface.Validation;
using Core.DomainModel;
using Core.Interface.Service;

namespace Validation.Validation
{
    public class StockAdjustmentValidator : IStockAdjustmentValidator
    {

        public StockAdjustment VHasWarehouse(StockAdjustment stockAdjustment, IWarehouseService _warehouseService)
        {
            Warehouse warehouse = _warehouseService.GetObjectById(stockAdjustment.WarehouseId);
            if (warehouse == null)
            {
                stockAdjustment.Errors.Add("WarehouseId", "Tidak terasosiasi dengan warehouse");
            }
            return stockAdjustment;
        }

        public StockAdjustment VAdjustmentDate(StockAdjustment stockAdjustment)
        {
            /* adjustment is never null
            if (stockAdjustment.AdjustmentDate == null)
            {
                stockAdjustment.Errors.Add("AdjustmentDate", "Tidak boleh tidak ada");
            }
            */
            return stockAdjustment;
        }

        public StockAdjustment VHasNotBeenConfirmed(StockAdjustment stockAdjustment)
        {
            if (stockAdjustment.IsConfirmed)
            {
                stockAdjustment.Errors.Add("IsConfirmed", "Tidak boleh sudah dikonfirmasi");
            }
            return stockAdjustment;
        }

        public StockAdjustment VHasBeenConfirmed(StockAdjustment stockAdjustment)
        {
            if (!stockAdjustment.IsConfirmed)
            {
                stockAdjustment.Errors.Add("IsConfirmed", "Harus sudah dikonfirmasi");
            }
            return stockAdjustment;
        }

        public StockAdjustment VHasStockAdjustmentDetails(StockAdjustment stockAdjustment, IStockAdjustmentDetailService _stockAdjustmentDetailService)
        {
            IList<StockAdjustmentDetail> details = _stockAdjustmentDetailService.GetObjectsByStockAdjustmentId(stockAdjustment.Id);
            if (!details.Any())
            {
                stockAdjustment.Errors.Add("Generic", "Details tidak boleh tidak ada");
            }
            return stockAdjustment;
        }

        public StockAdjustment VDetailsAreVerifiedConfirmable(StockAdjustment stockAdjustment, IStockAdjustmentService _stockAdjustmentService, IStockAdjustmentDetailService _stockAdjustmentDetailService,
                                                              IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            IList<StockAdjustmentDetail> details = _stockAdjustmentDetailService.GetObjectsByStockAdjustmentId(stockAdjustment.Id);
            foreach (var detail in details)
            {
                if (!_stockAdjustmentDetailService.GetValidator().ValidConfirmObject(detail, _stockAdjustmentService, _itemService, _barringService, _warehouseItemService))
                {
                    stockAdjustment.Errors.Add("Generic", "Details tidak dapat dikonfirmasi");
                }
            }
            return stockAdjustment;
        }

        public StockAdjustment VDetailsAreVerifiedUnconfirmable(StockAdjustment stockAdjustment, IStockAdjustmentService _stockAdjustmentService, IStockAdjustmentDetailService _stockAdjustmentDetailService,
                                                                IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            IList<StockAdjustmentDetail> details = _stockAdjustmentDetailService.GetObjectsByStockAdjustmentId(stockAdjustment.Id);
            foreach (var detail in details)
            {
                if (!_stockAdjustmentDetailService.GetValidator().ValidUnconfirmObject(detail, _stockAdjustmentService, _itemService, _barringService, _warehouseItemService))
                {
                    foreach (var error in detail.Errors)
                    {
                        stockAdjustment.Errors.Add(error.Key, error.Value);
                    }
                    if (!isValid(stockAdjustment)) { return stockAdjustment; }
                }
            }
            return stockAdjustment;
        }

        public StockAdjustment VCreateObject(StockAdjustment stockAdjustment, IWarehouseService _warehouseService)
        {
            VAdjustmentDate(stockAdjustment);
            if (!isValid(stockAdjustment)) { return stockAdjustment; }
            VHasWarehouse(stockAdjustment, _warehouseService);
            return stockAdjustment;
        }

        public StockAdjustment VUpdateObject(StockAdjustment stockAdjustment, IWarehouseService _warehouseService)
        {
            VHasNotBeenConfirmed(stockAdjustment);
            if (!isValid(stockAdjustment)) { return stockAdjustment; }
            VCreateObject(stockAdjustment, _warehouseService);
            return stockAdjustment;
        }

        public StockAdjustment VDeleteObject(StockAdjustment stockAdjustment)
        {
            VHasNotBeenConfirmed(stockAdjustment);
            return stockAdjustment;
        }

        public StockAdjustment VConfirmObject(StockAdjustment stockAdjustment, IStockAdjustmentService _stockAdjustmentService, IStockAdjustmentDetailService _stockAdjustmentDetailService,
                                              IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            VHasNotBeenConfirmed(stockAdjustment);
            if (!isValid(stockAdjustment)) { return stockAdjustment; }
            VHasStockAdjustmentDetails(stockAdjustment, _stockAdjustmentDetailService);
            if (!isValid(stockAdjustment)) { return stockAdjustment; }
            VDetailsAreVerifiedConfirmable(stockAdjustment, _stockAdjustmentService, _stockAdjustmentDetailService, _itemService, _barringService, _warehouseItemService);
            return stockAdjustment;
        }

        public StockAdjustment VUnconfirmObject(StockAdjustment stockAdjustment, IStockAdjustmentService _stockAdjustmentService, IStockAdjustmentDetailService _stockAdjustmentDetailService,
                                                IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            VHasBeenConfirmed(stockAdjustment);
            if (!isValid(stockAdjustment)) { return stockAdjustment; }
            VDetailsAreVerifiedUnconfirmable(stockAdjustment, _stockAdjustmentService, _stockAdjustmentDetailService, _itemService, _barringService, _warehouseItemService);
            return stockAdjustment;
        }

        public bool ValidCreateObject(StockAdjustment stockAdjustment, IWarehouseService _warehouseService)
        {
            VCreateObject(stockAdjustment, _warehouseService);
            return isValid(stockAdjustment);
        }

        public bool ValidUpdateObject(StockAdjustment stockAdjustment, IWarehouseService _warehouseService)
        {
            stockAdjustment.Errors.Clear();
            VUpdateObject(stockAdjustment, _warehouseService);
            return isValid(stockAdjustment);
        }

        public bool ValidDeleteObject(StockAdjustment stockAdjustment)
        {
            stockAdjustment.Errors.Clear();
            VDeleteObject(stockAdjustment);
            return isValid(stockAdjustment);
        }

        public bool ValidConfirmObject(StockAdjustment stockAdjustment, IStockAdjustmentService _stockAdjustmentService, IStockAdjustmentDetailService _stockAdjustmentDetailService,
                                       IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            stockAdjustment.Errors.Clear();
            VConfirmObject(stockAdjustment, _stockAdjustmentService, _stockAdjustmentDetailService, _itemService, _barringService, _warehouseItemService);
            return isValid(stockAdjustment);
        }

        public bool ValidUnconfirmObject(StockAdjustment stockAdjustment, IStockAdjustmentService _stockAdjustmentService, IStockAdjustmentDetailService _stockAdjustmentDetailService,
                                         IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService)
        {
            stockAdjustment.Errors.Clear();
            VUnconfirmObject(stockAdjustment, _stockAdjustmentService, _stockAdjustmentDetailService, _itemService, _barringService, _warehouseItemService);
            return isValid(stockAdjustment);
        }

        public bool isValid(StockAdjustment obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(StockAdjustment obj)
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