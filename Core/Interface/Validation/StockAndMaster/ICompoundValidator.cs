using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface ICompoundValidator
    {
        Compound VHasExpiryDate(Compound compound);
        Compound VHasItemTypeAndIsLegacy(Compound compound, IItemTypeService _itemTypeService);
        Compound VHasUniqueSku(Compound compound, ICompoundService _compoundService);
        Compound VHasName(Compound compound);
        Compound VHasUoM(Compound compound, IUoMService _uomService);
        Compound VWarehouseQuantityMustBeZero(Compound compound, IWarehouseItemService _warehouseItemService);
        Compound VNonNegativeQuantity(Compound compound);

        Compound VHasNoStockMutations(Compound compound, IStockMutationService _stockMutationService);
        Compound VHasNoPurchaseOrderDetails(Compound compound, IPurchaseOrderDetailService _purchaseOrderDetailService);
        Compound VHasNoStockAdjustmentDetails(Compound compound, IStockAdjustmentDetailService _stockAdjustmentDetailService);
        Compound VHasNoSalesOrderDetails(Compound compound, ISalesOrderDetailService _salesOrderDetailService);

        Compound VCreateObject(Compound compound, ICompoundService _compoundService, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService);
        Compound VUpdateObject(Compound compound, ICompoundService _compoundService, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService);
        Compound VDeleteObject(Compound compound, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService, IPurchaseOrderDetailService _purchaseOrderDetailService,
                               IStockAdjustmentDetailService _stockAdjustmentDetailService, ISalesOrderDetailService _salesOrderDetailService, IStockMutationService _stockMutationService);
        Compound VAdjustQuantity(Compound compound);
        Compound VAdjustPendingDelivery(Compound compound);
        Compound VAdjustPendingReceival(Compound compound);

        bool ValidCreateObject(Compound compound, ICompoundService _compoundService, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService);
        bool ValidUpdateObject(Compound compound, ICompoundService _compoundService, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService);
        bool ValidDeleteObject(Compound compound, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService, IPurchaseOrderDetailService _purchaseOrderDetailService,
                               IStockAdjustmentDetailService _stockAdjustmentDetailService, ISalesOrderDetailService _salesOrderDetailService, IStockMutationService _stockMutationService);
        bool ValidAdjustQuantity(Compound compound);
        bool ValidAdjustPendingDelivery(Compound compound);
        bool ValidAdjustPendingReceival(Compound compound);
        bool isValid(Compound compound);
        string PrintError(Compound compound);
    }
}