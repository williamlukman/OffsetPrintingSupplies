using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IBlendingWorkOrderService
    {
        IBlendingWorkOrderValidator GetValidator();
        IQueryable<BlendingWorkOrder> GetQueryable();
        IList<BlendingWorkOrder> GetAll();
        IList<BlendingWorkOrder> GetObjectsByBlendingRecipeId(int BlendingRecipeId);
        BlendingWorkOrder GetObjectById(int Id);
        BlendingWorkOrder CreateObject(BlendingWorkOrder blendingWorkOrder, IBlendingRecipeService _blendingRecipeService, IWarehouseService _warehouseService);
        BlendingWorkOrder UpdateObject(BlendingWorkOrder blendingWorkOrder, IBlendingRecipeService _blendingRecipeService, IWarehouseService _warehouseService);
        BlendingWorkOrder SoftDeleteObject(BlendingWorkOrder blendingWorkOrder);
        BlendingWorkOrder ConfirmObject(BlendingWorkOrder blendingWorkOrder, DateTime ConfirmationDate, IBlendingRecipeService _blendingRecipeService, IBlendingRecipeDetailService _blendingRecipeDetailService,
                                        IStockMutationService _stockMutationService, IBlanketService _blanketService, IItemService _itemService, IItemTypeService _itemTypeService,
                                        IWarehouseItemService _warehouseItemService, IGeneralLedgerJournalService _generalLedgerJournalService, IAccountService _accountService, 
                                        IClosingService _closingService);
        BlendingWorkOrder UnconfirmObject(BlendingWorkOrder blendingWorkOrder, IBlendingRecipeService _blendingRecipeService, IBlendingRecipeDetailService _blendingRecipeDetailService,
                                          IStockMutationService _stockMutationService, IBlanketService _blanketService, IItemService _itemService, IItemTypeService _itemTypeService, 
                                          IWarehouseItemService _warehouseItemService, IGeneralLedgerJournalService _generalLedgerJournalService, IAccountService _accountService, 
                                          IClosingService _closingService);
        BlendingWorkOrder AdjustQuantity(BlendingWorkOrder blendingWorkOrder);
        bool DeleteObject(int Id);
        bool IsCodeDuplicated(BlendingWorkOrder blendingWorkOrder);
    }
}