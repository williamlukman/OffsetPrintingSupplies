using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface IBlendingWorkOrderValidator
    {
        BlendingWorkOrder VCreateObject(BlendingWorkOrder blendingWorkOrder, IBlendingWorkOrderService _blendingWorkOrderService, IBlendingRecipeService _blendingRecipeService, IWarehouseService _warehouseService);
        BlendingWorkOrder VUpdateObject(BlendingWorkOrder blendingWorkOrder, IBlendingWorkOrderService _blendingWorkOrderService, IBlendingRecipeService _blendingRecipeService, IWarehouseService _warehouseService);
        BlendingWorkOrder VDeleteObject(BlendingWorkOrder blendingWorkOrder);
        BlendingWorkOrder VConfirmObject(BlendingWorkOrder blendingWorkOrder, IBlendingRecipeDetailService _blendingRecipeDetailService, IWarehouseItemService _warehouseItemService, IClosingService _closingService);
        BlendingWorkOrder VUnconfirmObject(BlendingWorkOrder blendingWorkOrder, IWarehouseItemService _warehouseItemService, IBlendingRecipeService _blendingRecipeService, IClosingService _closingService);
        BlendingWorkOrder VAdjustQuantity(BlendingWorkOrder blendingWorkOrder);

        bool ValidCreateObject(BlendingWorkOrder blendingWorkOrder, IBlendingWorkOrderService _blendingWorkOrderService, IBlendingRecipeService _blendingRecipeService, IWarehouseService _warehouseService);
        bool ValidUpdateObject(BlendingWorkOrder blendingWorkOrder, IBlendingWorkOrderService _blendingWorkOrderService, IBlendingRecipeService _blendingRecipeService, IWarehouseService _warehouseService);
        bool ValidDeleteObject(BlendingWorkOrder blendingWorkOrder);
        bool ValidConfirmObject(BlendingWorkOrder blendingWorkOrder, IBlendingRecipeDetailService _blendingRecipeDetailService, IWarehouseItemService _warehouseItemService, IClosingService _closingService);
        bool ValidUnconfirmObject(BlendingWorkOrder blendingWorkOrder, IWarehouseItemService _warehouseItemService, IBlendingRecipeService _blendingRecipeService, IClosingService _closingService);
        bool ValidAdjustQuantity(BlendingWorkOrder blendingWorkOrder);
        bool isValid(BlendingWorkOrder blendingWorkOrder);
        string PrintError(BlendingWorkOrder blendingWorkOrder);
    }
}