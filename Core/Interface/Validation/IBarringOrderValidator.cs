using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Validation
{
    public interface IBarringOrderValidator
    {
        BarringOrder VHasUniqueCode(BarringOrder barringOrder, IBarringOrderService _barringOrderService);
        BarringOrder VHasBarringOrderDetails(BarringOrder barringOrder, IBarringOrderDetailService _barringOrderDetailService);
        BarringOrder VHasQuantityOrdered(BarringOrder barringOrder);
        BarringOrder VQuantityOrderedEqualDetails(BarringOrder barringOrder, IBarringOrderDetailService _barringOrderDetailService);
        BarringOrder VQuantityIsInStock(BarringOrder barringOrder, IBarringOrderDetailService _barringOrderDetailService, IBarringService _barringService, IItemService _itemService);
        BarringOrder VHasBeenConfirmed(BarringOrder barringOrder);
        BarringOrder VHasNotBeenConfirmed(BarringOrder barringOrder);
        BarringOrder VHasBeenFinished(BarringOrder barringOrder);
        BarringOrder VHasNotBeenFinished(BarringOrder barringOrder);
        BarringOrder VAllDetailsHaveBeenPackagedOrRejected(BarringOrder barringOrder, IBarringOrderDetailService _barringOrderDetailService);
        BarringOrder VAllDetailsHaveBeenCutOrRejected(BarringOrder barringOrder, IBarringOrderDetailService _barringOrderDetailService);
        BarringOrder VAllDetailsHaveNotBeenCutNorRejected(BarringOrder barringOrder, IBarringOrderDetailService _barringOrderDetailService);
        BarringOrder VAllDetailsHaveLeftAndRightBar(BarringOrder barringOrder, IBarringOrderDetailService _barringOrderDetailService, IItemService _itemService);
        BarringOrder VCreateObject(BarringOrder barringOrder, IBarringOrderService _barringOrderService);
        BarringOrder VUpdateObject(BarringOrder barringOrder, IBarringOrderDetailService _barringOrderDetailService, IBarringOrderService _barringOrderService);
        BarringOrder VDeleteObject(BarringOrder barringOrder, IBarringOrderDetailService _barringOrderDetailService);
        BarringOrder VConfirmObject(BarringOrder barringOrder, IBarringOrderDetailService _barringOrderDetailService, IBarringService _barringService, IItemService _itemService);
        BarringOrder VUnconfirmObject(BarringOrder barringOrder, IBarringOrderDetailService _barringOrderDetailService);
        BarringOrder VFinishObject(BarringOrder barringOrder, IBarringOrderDetailService _barringOrderDetailService, IItemService _itemService);
        BarringOrder VUnfinishObject(BarringOrder barringOrder);

        bool ValidCreateObject(BarringOrder barringOrder, IBarringOrderService _barringOrderService);
        bool ValidUpdateObject(BarringOrder barringOrder, IBarringOrderDetailService _barringOrderDetailService, IBarringOrderService _barringOrderService);
        bool ValidDeleteObject(BarringOrder barringOrder, IBarringOrderDetailService _barringOrderDetailService);
        bool ValidConfirmObject(BarringOrder barringOrder, IBarringOrderDetailService _barringOrderDetailService, IBarringService _barringService, IItemService _itemService);
        bool ValidUnconfirmObject(BarringOrder barringOrder, IBarringOrderDetailService _barringOrderDetailService);
        bool ValidFinishObject(BarringOrder barringOrder, IBarringOrderDetailService _barringOrderDetailService, IItemService _itemService);
        bool ValidUnfinishObject(BarringOrder barringOrder);
        bool isValid(BarringOrder barringOrder);
        string PrintError(BarringOrder barringOrder);
    }
}