using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface IRepackingValidator
    {
        Repacking VCreateObject(Repacking repacking, IRepackingService _repackingService, IBlendingRecipeService _blendingRecipeService, IWarehouseService _warehouseService);
        Repacking VUpdateObject(Repacking repacking, IRepackingService _repackingService, IBlendingRecipeService _blendingRecipeService, IWarehouseService _warehouseService);
        Repacking VDeleteObject(Repacking repacking);
        Repacking VConfirmObject(Repacking repacking, IBlendingRecipeDetailService _blendingRecipeDetailService, IWarehouseItemService _warehouseItemService, IClosingService _closingService);
        Repacking VUnconfirmObject(Repacking repacking, IWarehouseItemService _warehouseItemService, IBlendingRecipeService _blendingRecipeService, IClosingService _closingService);
        Repacking VAdjustQuantity(Repacking repacking);

        bool ValidCreateObject(Repacking repacking, IRepackingService _repackingService, IBlendingRecipeService _blendingRecipeService, IWarehouseService _warehouseService);
        bool ValidUpdateObject(Repacking repacking, IRepackingService _repackingService, IBlendingRecipeService _blendingRecipeService, IWarehouseService _warehouseService);
        bool ValidDeleteObject(Repacking repacking);
        bool ValidConfirmObject(Repacking repacking, IBlendingRecipeDetailService _blendingRecipeDetailService, IWarehouseItemService _warehouseItemService, IClosingService _closingService);
        bool ValidUnconfirmObject(Repacking repacking, IWarehouseItemService _warehouseItemService, IBlendingRecipeService _blendingRecipeService, IClosingService _closingService);
        bool ValidAdjustQuantity(Repacking repacking);
        bool isValid(Repacking repacking);
        string PrintError(Repacking repacking);
    }
}