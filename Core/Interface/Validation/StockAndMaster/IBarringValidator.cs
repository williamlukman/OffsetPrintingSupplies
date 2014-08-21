using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface IBarringValidator
    {
        Barring VHasItemTypeAndIsLegacy(Barring barring, IItemTypeService _itemTypeService);
        Barring VHasUniqueSku(Barring barring, IBarringService _barringService);
        Barring VHasName(Barring barring);
        Barring VHasCategory(Barring barring);
        Barring VHasUoM(Barring barring, IUoMService _uomService);
        Barring VWarehouseQuantityMustBeZero(Barring barring, IWarehouseItemService _warehouseItemService);
        Barring VNonNegativeQuantity(Barring barring);

        Barring VHasBlanket(Barring barring, IItemService _itemService);
        Barring VHasContact(Barring barring, IContactService _contactService);
        Barring VHasMachine(Barring barring, IMachineService _machineService);
        Barring VHasMeasurement(Barring barring);
        Barring VIfIsBarRequiredThenHasAtLeastOneBar(Barring barring);
        Barring VIfIsBarNotRequiredThenHasNoBars(Barring barring);
        Barring VIfHasLeftBarThenLeftBarIsValid(Barring barring, IBarringService _barringService);
        Barring VIfHasRightBarThenRightBarIsValid(Barring barring, IBarringService _barringService);

        Barring VHasNoStockMutations(Barring barring, IStockMutationService _stockMutationService);
        Barring VHasNoPurchaseOrderDetails(Barring barring, IPurchaseOrderDetailService _purchaseOrderDetailService);
        Barring VHasNoStockAdjustmentDetails(Barring barring, IStockAdjustmentDetailService _stockAdjustmentDetailService);
        Barring VHasNoSalesOrderDetails(Barring barring, ISalesOrderDetailService _salesOrderDetailService);

        Barring VCreateObject(Barring barring, IBarringService _barringService, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService,
                              IContactService _contactService, IMachineService _machineService);
        Barring VUpdateObject(Barring barring, IBarringService _barringService, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService,
                              IContactService _contactService, IMachineService _machineService);
        Barring VDeleteObject(Barring barring, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService, IPurchaseOrderDetailService _purchaseOrderDetailService,
                              IStockAdjustmentDetailService _stockAdjustmentDetailService, ISalesOrderDetailService _salesOrderDetailService,
                              IStockMutationService _stockMutationService, IBarringOrderDetailService _barringOrderDetailService);
        Barring VAdjustQuantity(Barring barring);
        Barring VAdjustPendingDelivery(Barring barring);
        Barring VAdjustPendingReceival(Barring barring);

        bool ValidCreateObject(Barring barring, IBarringService _barringService, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService,
                               IContactService _contactService, IMachineService _machineService);
        bool ValidUpdateObject(Barring barring, IBarringService _barringService, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService,
                               IContactService _contactService, IMachineService _machineService);
        bool ValidDeleteObject(Barring barring, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService, IPurchaseOrderDetailService _purchaseOrderDetailService,
                               IStockAdjustmentDetailService _stockAdjustmentDetailService, ISalesOrderDetailService _salesOrderDetailService,
                               IStockMutationService _stockMutationService, IBarringOrderDetailService _barringOrderDetailService);
        bool ValidAdjustQuantity(Barring barring);
        bool ValidAdjustPendingDelivery(Barring barring);
        bool ValidAdjustPendingReceival(Barring barring);
        bool isValid(Barring barring);
        string PrintError(Barring barring);
    }
}