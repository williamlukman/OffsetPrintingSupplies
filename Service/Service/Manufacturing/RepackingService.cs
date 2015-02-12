using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Core.Constants;

namespace Service.Service
{
    public class RepackingService : IRepackingService
    {
        private IRepackingRepository _repository;
        private IRepackingValidator _validator;
        public RepackingService(IRepackingRepository _repackingRepository, IRepackingValidator _repackingValidator)
        {
            _repository = _repackingRepository;
            _validator = _repackingValidator;
        }

        public IRepackingValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<Repacking> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<Repacking> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<Repacking> GetObjectsByBlendingRecipeId(int BlendingRecipeId)
        {
            return _repository.GetObjectsByBlendingRecipeId(BlendingRecipeId);
        }

        public Repacking GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public Repacking CreateObject(Repacking repacking, IBlendingRecipeService _blendingRecipeService, IWarehouseService _warehouseService)
        {
            repacking.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(repacking, this, _blendingRecipeService, _warehouseService) ? _repository.CreateObject(repacking) : repacking);
        }

        public Repacking UpdateObject(Repacking repacking, IBlendingRecipeService _blendingRecipeService, IWarehouseService _warehouseService)
        {
            return (repacking = _validator.ValidUpdateObject(repacking, this, _blendingRecipeService, _warehouseService) ? _repository.UpdateObject(repacking) : repacking);
        }

        public Repacking SoftDeleteObject(Repacking repacking)
        {
            if (_validator.ValidDeleteObject(repacking))
            {
                _repository.SoftDeleteObject(repacking);
            }
            return repacking;
        }

        public Repacking ConfirmObject(Repacking repacking, DateTime ConfirmationDate, IBlendingRecipeService _blendingRecipeService, IBlendingRecipeDetailService _blendingRecipeDetailService,
                                               IStockMutationService _stockMutationService, IBlanketService _blanketService, IItemService _itemService, IItemTypeService _itemTypeService, 
                                               IWarehouseItemService _warehouseItemService, IGeneralLedgerJournalService _generalLedgerJournalService, IAccountService _accountService, IClosingService _closingService)
        {
            repacking.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(repacking, _blendingRecipeDetailService, _warehouseItemService, _closingService))
            {
                WarehouseItem warehouseItem = null;
                StockMutation stockMutation = null;
                decimal TotalCost = 0;

                // deduce source items
                var details = _blendingRecipeDetailService.GetObjectsByBlendingRecipeId(repacking.BlendingRecipeId);
                foreach (var detail in details)
                {
                    Item itemdet = _itemService.GetObjectById(detail.ItemId);
                    ItemType itemTypeDet = _itemTypeService.GetObjectById(itemdet.ItemTypeId);
                    TotalCost += detail.Quantity * itemdet.AvgPrice;
                    warehouseItem = _warehouseItemService.FindOrCreateObject(repacking.WarehouseId, detail.ItemId);
                    stockMutation = _stockMutationService.CreateStockMutationForRepackingSource(repacking, detail, warehouseItem);
                    _stockMutationService.StockMutateObject(stockMutation, _itemService, _blanketService, _warehouseItemService);
                    _generalLedgerJournalService.CreateConfirmationJournalForRepackingDetail(repacking, itemTypeDet.AccountId.GetValueOrDefault(), detail.Quantity * itemdet.AvgPrice, _accountService);
                }

                // update avg cost of target item before mutated
                BlendingRecipe blendingRecipe = _blendingRecipeService.GetObjectById(repacking.BlendingRecipeId);
                Item item = _itemService.GetObjectById(blendingRecipe.TargetItemId);
                ItemType itemType = _itemTypeService.GetObjectById(item.ItemTypeId);
                _itemService.CalculateAndUpdateAvgPrice(item, blendingRecipe.TargetQuantity, TotalCost / blendingRecipe.TargetQuantity);
                
                // add target item
                warehouseItem = _warehouseItemService.FindOrCreateObject(repacking.WarehouseId, blendingRecipe.TargetItemId);
                stockMutation = _stockMutationService.CreateStockMutationForRepackingTarget(repacking, blendingRecipe, warehouseItem);
                _stockMutationService.StockMutateObject(stockMutation, _itemService, _blanketService, _warehouseItemService);

                // post GL (credit raw, debit finishedgoods)
                _generalLedgerJournalService.CreateConfirmationJournalForRepacking(repacking, itemType.AccountId.GetValueOrDefault(), _accountService, TotalCost);
                
                _repository.ConfirmObject(repacking);
            }
            return repacking;
        }

        public Repacking UnconfirmObject(Repacking repacking, IBlendingRecipeService _blendingRecipeService, IBlendingRecipeDetailService _blendingRecipeDetailService,
                                                 IStockMutationService _stockMutationService, IBlanketService _blanketService, IItemService _itemService, IItemTypeService _itemTypeService, 
                                                 IWarehouseItemService _warehouseItemService, IGeneralLedgerJournalService _generalLedgerJournalService, IAccountService _accountService,
                                                 IClosingService _closingService)
        {
            if (_validator.ValidUnconfirmObject(repacking, _warehouseItemService, _blendingRecipeService, _closingService))
            {
                WarehouseItem warehouseItem = null;
                IList<StockMutation> stockMutations = null;
                decimal TotalCost = 0;

                // add source items
                var details = _blendingRecipeDetailService.GetObjectsByBlendingRecipeId(repacking.BlendingRecipeId);
                foreach (var detail in details)
                {
                    Item itemdet = _itemService.GetObjectById(detail.ItemId);
                    ItemType itemTypeDet = _itemTypeService.GetObjectById(itemdet.ItemTypeId);
                    TotalCost += detail.Quantity * itemdet.AvgPrice;
                    warehouseItem = _warehouseItemService.FindOrCreateObject(repacking.WarehouseId, detail.ItemId);
                    stockMutations = _stockMutationService.GetObjectsBySourceDocumentDetailForWarehouseItem(warehouseItem.Id, Constant.SourceDocumentDetailType.BlendingRecipeDetail, detail.Id);
                    foreach (var x in stockMutations)
                    {
                        _stockMutationService.ReverseStockMutateObject(x, _itemService, _blanketService, _warehouseItemService);
                    }
                    _generalLedgerJournalService.CreateUnconfirmationJournalForRepackingDetail(repacking, itemTypeDet.AccountId.GetValueOrDefault(), detail.Quantity * itemdet.AvgPrice, _accountService);
                }

                // update avg cost of target item before mutated
                BlendingRecipe blendingRecipe = _blendingRecipeService.GetObjectById(repacking.BlendingRecipeId);
                Item item = _itemService.GetObjectById(blendingRecipe.TargetItemId);
                ItemType itemType = _itemTypeService.GetObjectById(item.ItemTypeId);
                _itemService.CalculateAndUpdateAvgPrice(item, (-1)*blendingRecipe.TargetQuantity, TotalCost / blendingRecipe.TargetQuantity);

                // deduce target item
                warehouseItem = _warehouseItemService.FindOrCreateObject(repacking.WarehouseId, blendingRecipe.TargetItemId);
                stockMutations = _stockMutationService.GetObjectsBySourceDocumentDetailForWarehouseItem(warehouseItem.Id, Constant.SourceDocumentDetailType.BlendingRecipe, blendingRecipe.Id);
                foreach (var x in stockMutations)
                {
                    _stockMutationService.ReverseStockMutateObject(x, _itemService, _blanketService, _warehouseItemService);
                }

                // post GL (debit raw, credit finishedgoods)
                _generalLedgerJournalService.CreateUnconfirmationJournalForRepacking(repacking, itemType.AccountId.GetValueOrDefault(), _accountService, TotalCost);
                
                _repository.UnconfirmObject(repacking);
            }
            return repacking;
        }

        public Repacking AdjustQuantity(Repacking repacking)
        {
            return (repacking = _validator.ValidAdjustQuantity(repacking) ? _repository.AdjustQuantity(repacking) : repacking);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsCodeDuplicated(Repacking repacking)
        {
            return _repository.IsCodeDuplicated(repacking);
        }
    }
}