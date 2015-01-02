using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IRepackingService
    {
        IRepackingValidator GetValidator();
        IQueryable<Repacking> GetQueryable();
        IList<Repacking> GetAll();
        IList<Repacking> GetObjectsByBlendingRecipeId(int BlendingRecipeId);
        Repacking GetObjectById(int Id);
        Repacking CreateObject(Repacking repacking, IBlendingRecipeService _blendingRecipeService, IWarehouseService _warehouseService);
        Repacking UpdateObject(Repacking repacking, IBlendingRecipeService _blendingRecipeService, IWarehouseService _warehouseService);
        Repacking SoftDeleteObject(Repacking repacking);
        Repacking ConfirmObject(Repacking repacking, DateTime ConfirmationDate, IBlendingRecipeService _blendingRecipeService, IBlendingRecipeDetailService _blendingRecipeDetailService,
                                IStockMutationService _stockMutationService, IBlanketService _blanketService, IItemService _itemService, IItemTypeService _itemTypeService,
                                IWarehouseItemService _warehouseItemService, IGeneralLedgerJournalService _generalLedgerJournalService, IAccountService _accountService, 
                                IClosingService _closingService);
        Repacking UnconfirmObject(Repacking repacking, IBlendingRecipeService _blendingRecipeService, IBlendingRecipeDetailService _blendingRecipeDetailService,
                                  IStockMutationService _stockMutationService, IBlanketService _blanketService, IItemService _itemService, IItemTypeService _itemTypeService, 
                                  IWarehouseItemService _warehouseItemService, IGeneralLedgerJournalService _generalLedgerJournalService, IAccountService _accountService, 
                                  IClosingService _closingService);
        Repacking AdjustQuantity(Repacking repacking);
        bool DeleteObject(int Id);
        bool IsCodeDuplicated(Repacking repacking);
    }
}