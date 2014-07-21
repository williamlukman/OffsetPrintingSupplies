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
    public interface IStockAdjustmentDetailService
    {
        IStockAdjustmentDetailValidator GetValidator();
        IList<StockAdjustmentDetail> GetObjectsByStockAdjustmentId(int stockAdjustmentId);
        StockAdjustmentDetail GetObjectById(int Id);
        StockAdjustmentDetail CreateObject(StockAdjustmentDetail stockAdjustmentDetail, IStockAdjustmentService _stockAdjustmentService,
                                           IItemService _itemService, IWarehouseItemService _warehouseItemService);
        StockAdjustmentDetail CreateObject(int stockAdjustmentId, int itemId, int quantity, IStockAdjustmentService _stockAdjustmentService,
                                           IItemService _itemService, IWarehouseItemService _warehouseItemService);
        StockAdjustmentDetail UpdateObject(StockAdjustmentDetail stockAdjustmentDetail, IStockAdjustmentService _stockAdjustmentService,
                                           IItemService _itemService, IWarehouseItemService _warehouseItemService);
        StockAdjustmentDetail SoftDeleteObject(StockAdjustmentDetail stockAdjustmentDetail);
        bool DeleteObject(int Id);
        StockAdjustmentDetail FinishObject(StockAdjustmentDetail stockAdjustmentDetail, IStockAdjustmentService _stockAdjustmentService, IStockMutationService _stockMutationService,
                                            IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService);
        StockAdjustmentDetail UnfinishObject(StockAdjustmentDetail stockAdjustmentDetail, IStockAdjustmentService _stockAdjustmentService, IStockMutationService _stockMutationService,
                                            IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService);
        void AdjustStock(StockAdjustment stockAdjustment, StockAdjustmentDetail detail, IStockMutationService _stockMutationService,
                         IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService, bool CaseFinish);
    }
}