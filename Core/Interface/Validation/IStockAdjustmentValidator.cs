using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DomainModel;
using Core.Interface.Service;

namespace Core.Interface.Validation
{
    public interface IStockAdjustmentValidator
    {
        StockAdjustment VHasWarehouse(StockAdjustment stockAdjustment, IWarehouseService _warehouseService);
        StockAdjustment VAdjustmentDate(StockAdjustment stockAdjustment);
        StockAdjustment VHasBeenConfirmed(StockAdjustment stockAdjustment);
        StockAdjustment VHasNotBeenConfirmed(StockAdjustment stockAdjustment);
        StockAdjustment VHasStockAdjustmentDetails(StockAdjustment stockAdjustment, IStockAdjustmentDetailService _stockAdjustmentDetailService);
        StockAdjustment VDetailsAreVerifiedConfirmable(StockAdjustment stockAdjustment, IStockAdjustmentService _stockAdjustmentService, IStockAdjustmentDetailService _stockAdjustmentDetailService,
                                                       IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService);
        StockAdjustment VAllDetailsHaveBeenFinished(StockAdjustment stockAdjustment, IStockAdjustmentDetailService _stockAdjustmentDetailService);
        StockAdjustment VAllDetailsHaveNotBeenFinished(StockAdjustment stockAdjustment, IStockAdjustmentDetailService _stockAdjustmentDetailService);
        StockAdjustment VCreateObject(StockAdjustment stockAdjustment, IWarehouseService _warehouseService);
        StockAdjustment VUpdateObject(StockAdjustment stockAdjustment, IWarehouseService _warehouseService);
        StockAdjustment VDeleteObject(StockAdjustment stockAdjustment);
        StockAdjustment VConfirmObject(StockAdjustment stockAdjustment, IStockAdjustmentService _stockAdjustmentService, IStockAdjustmentDetailService _stockAdjustmentDetailService,
                                       IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService);
        StockAdjustment VUnconfirmObject(StockAdjustment stockAdjustment, IStockAdjustmentService _stockAdjustmentService, IStockAdjustmentDetailService _stockAdjustmentDetailService,
                                       IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService);
        StockAdjustment VCompleteObject(StockAdjustment stockAdjustment, IStockAdjustmentDetailService _stockAdjustmentDetailService);
        bool ValidCreateObject(StockAdjustment stockAdjustment, IWarehouseService _warehouseService);
        bool ValidUpdateObject(StockAdjustment stockAdjustment, IWarehouseService _warehouseService);
        bool ValidDeleteObject(StockAdjustment stockAdjustment);
        bool ValidConfirmObject(StockAdjustment stockAdjustment, IStockAdjustmentService _stockAdjustmentService, IStockAdjustmentDetailService _stockAdjustmentDetailService,
                                IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService);
        bool ValidUnconfirmObject(StockAdjustment stockAdjustment, IStockAdjustmentService _stockAdjustmentService, IStockAdjustmentDetailService _stockAdjustmentDetailService,
                                IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService);
        bool ValidCompleteObject(StockAdjustment stockAdjustment, IStockAdjustmentDetailService _stockAdjustmentDetailService);
        bool isValid(StockAdjustment stockAdjustment);
        string PrintError(StockAdjustment stockAdjustment);
    }
}
