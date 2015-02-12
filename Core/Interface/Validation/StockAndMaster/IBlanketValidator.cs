using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface IBlanketValidator
    {
        Blanket VHasItemTypeAndIsLegacy(Blanket blanket, IItemTypeService _itemTypeService);
        Blanket VHasUniqueSku(Blanket blanket, IBlanketService _blanketService);
        Blanket VHasName(Blanket blanket);
        Blanket VHasUoM(Blanket blanket, IUoMService _uomService);
        Blanket VHasApplicationCase(Blanket blanket);
        Blanket VWarehouseQuantityMustBeZero(Blanket blanket, IWarehouseItemService _warehouseItemService);
        Blanket VNonNegativeQuantity(Blanket blanket);

        Blanket VHasRollBlanket(Blanket blanket, IItemService _itemService);
        Blanket VHasContact(Blanket blanket, IContactService _contactService);
        Blanket VHasMachine(Blanket blanket, IMachineService _machineService);
        Blanket VHasMeasurement(Blanket blanket);
        Blanket VIfIsBarRequiredThenHasAtLeastOneBar(Blanket blanket);
        Blanket VIfIsBarNotRequiredThenHasNoBars(Blanket blanket);
        Blanket VIfHasLeftBarThenLeftBarIsValid(Blanket blanket, IBlanketService _blanketService);
        Blanket VIfHasRightBarThenRightBarIsValid(Blanket blanket, IBlanketService _blanketService);

        Blanket VHasNoStockMutations(Blanket blanket, IStockMutationService _stockMutationService);
        Blanket VHasNoPurchaseOrderDetails(Blanket blanket, IPurchaseOrderDetailService _purchaseOrderDetailService);
        Blanket VHasNoStockAdjustmentDetails(Blanket blanket, IStockAdjustmentDetailService _stockAdjustmentDetailService);
        Blanket VHasNoSalesOrderDetails(Blanket blanket, ISalesOrderDetailService _salesOrderDetailService);

        Blanket VCreateObject(Blanket blanket, IBlanketService _blanketService, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService,
                              IContactService _contactService, IMachineService _machineService);
        Blanket VUpdateObject(Blanket blanket, IBlanketService _blanketService, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService,
                              IContactService _contactService, IMachineService _machineService);
        Blanket VDeleteObject(Blanket blanket, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService, IPurchaseOrderDetailService _purchaseOrderDetailService,
                              IStockAdjustmentDetailService _stockAdjustmentDetailService, ISalesOrderDetailService _salesOrderDetailService,
                              IStockMutationService _stockMutationService, IBlanketOrderDetailService _blanketOrderDetailService);
        Blanket VAdjustQuantity(Blanket blanket);
        Blanket VAdjustPendingDelivery(Blanket blanket);
        Blanket VAdjustPendingReceival(Blanket blanket);

        bool ValidCreateObject(Blanket blanket, IBlanketService _blanketService, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService,
                               IContactService _contactService, IMachineService _machineService);
        bool ValidUpdateObject(Blanket blanket, IBlanketService _blanketService, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService,
                               IContactService _contactService, IMachineService _machineService);
        bool ValidDeleteObject(Blanket blanket, IItemTypeService _itemTypeService, IWarehouseItemService _warehouseItemService, IPurchaseOrderDetailService _purchaseOrderDetailService,
                               IStockAdjustmentDetailService _stockAdjustmentDetailService, ISalesOrderDetailService _salesOrderDetailService,
                               IStockMutationService _stockMutationService, IBlanketOrderDetailService _blanketOrderDetailService);
        bool ValidAdjustQuantity(Blanket blanket);
        bool ValidAdjustPendingDelivery(Blanket blanket);
        bool ValidAdjustPendingReceival(Blanket blanket);
        bool isValid(Blanket blanket);
        string PrintError(Blanket blanket);
    }
}