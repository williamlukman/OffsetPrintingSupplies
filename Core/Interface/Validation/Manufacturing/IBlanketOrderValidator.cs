using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface IBlanketOrderValidator
    {
        BlanketOrder VHasUniqueCode(BlanketOrder blanketOrder, IBlanketOrderService _blanketOrderService);
        BlanketOrder VHasBlanketOrderDetails(BlanketOrder blanketOrder, IBlanketOrderDetailService _blanketOrderDetailService);
        BlanketOrder VHasQuantityReceived(BlanketOrder blanketOrder);
        BlanketOrder VQuantityFinalAndRejectedIsLessThanOrEqualQuantityReceived(BlanketOrder blanketOrder);
        BlanketOrder VQuantityReceivedEqualDetails(BlanketOrder blanketOrder, IBlanketOrderDetailService _blanketOrderDetailService);
        BlanketOrder VQuantityIsInStock(BlanketOrder blanketOrder, IBlanketOrderDetailService _blanketOrderDetailService,
                                        IBlanketService _blanketService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        BlanketOrder VHasBeenConfirmed(BlanketOrder blanketOrder);
        BlanketOrder VHasNotBeenConfirmed(BlanketOrder blanketOrder);
        BlanketOrder VHasBeenCompleted(BlanketOrder blanketOrder);
        BlanketOrder VHasNotBeenCompleted(BlanketOrder blanketOrder);
        BlanketOrder VAllDetailsHaveBeenFinishedOrRejected(BlanketOrder blanketOrder, IBlanketOrderDetailService _blanketOrderDetailService);
        BlanketOrder VAllDetailsHaveBeenCutOrRejected(BlanketOrder blanketOrder, IBlanketOrderDetailService _blanketOrderDetailService);
        BlanketOrder VAllDetailsHaveNotBeenFinishedNorRejected(BlanketOrder blanketOrder, IBlanketOrderDetailService _blanketOrderDetailService);
        BlanketOrder VCreateObject(BlanketOrder blanketOrder, IBlanketOrderService _blanketOrderService);
        BlanketOrder VUpdateObject(BlanketOrder blanketOrder, IBlanketOrderDetailService _blanketOrderDetailService, IBlanketOrderService _blanketOrderService);
        BlanketOrder VDeleteObject(BlanketOrder blanketOrder, IBlanketOrderDetailService _blanketOrderDetailService);
        BlanketOrder VHasConfirmationDate(BlanketOrder blanketOrder);
        BlanketOrder VConfirmObject(BlanketOrder blanketOrder, IBlanketOrderDetailService _blanketOrderDetailService, IBlanketService _blanketService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        BlanketOrder VUnconfirmObject(BlanketOrder blanketOrder, IBlanketOrderDetailService _blanketOrderDetailService);
        BlanketOrder VCompleteObject(BlanketOrder blanketOrder, IBlanketOrderDetailService _blanketOrderDetailService);
        BlanketOrder VUndoCompleteObject(BlanketOrder blanketOrder, IBlanketOrderDetailService _blanketOrderDetailService);
        BlanketOrder VAdjustQuantity(BlanketOrder blanketOrder);

        bool ValidCreateObject(BlanketOrder blanketOrder, IBlanketOrderService _blanketOrderService);
        bool ValidUpdateObject(BlanketOrder blanketOrder, IBlanketOrderDetailService _blanketOrderDetailService, IBlanketOrderService _blanketOrderService);
        bool ValidDeleteObject(BlanketOrder blanketOrder, IBlanketOrderDetailService _blanketOrderDetailService);
        bool ValidConfirmObject(BlanketOrder blanketOrder, IBlanketOrderDetailService _blanketOrderDetailService, IBlanketService _blanketService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        bool ValidUnconfirmObject(BlanketOrder blanketOrder, IBlanketOrderDetailService _blanketOrderDetailService);
        bool ValidCompleteObject(BlanketOrder blanketOrder, IBlanketOrderDetailService _blanketOrderDetailService);
        bool ValidUndoCompleteObject(BlanketOrder blanketOrder, IBlanketOrderDetailService _blanketOrderDetailService);
        bool ValidAdjustQuantity(BlanketOrder blanketOrder);
        bool isValid(BlanketOrder blanketOrder);
        string PrintError(BlanketOrder blanketOrder);
    }
}