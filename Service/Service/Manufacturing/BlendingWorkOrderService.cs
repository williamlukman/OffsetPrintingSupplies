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
    public class BlendingWorkOrderService : IBlendingWorkOrderService
    {
        private IBlendingWorkOrderRepository _repository;
        private IBlendingWorkOrderValidator _validator;
        public BlendingWorkOrderService(IBlendingWorkOrderRepository _blendingWorkOrderRepository, IBlendingWorkOrderValidator _blendingWorkOrderValidator)
        {
            _repository = _blendingWorkOrderRepository;
            _validator = _blendingWorkOrderValidator;
        }

        public IBlendingWorkOrderValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<BlendingWorkOrder> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<BlendingWorkOrder> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<BlendingWorkOrder> GetObjectsByBlendingRecipeId(int BlendingRecipeId)
        {
            return _repository.GetObjectsByBlendingRecipeId(BlendingRecipeId);
        }

        public BlendingWorkOrder GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public BlendingWorkOrder CreateObject(BlendingWorkOrder blendingWorkOrder, IBlendingRecipeService _blendingRecipeService, IWarehouseService _warehouseService)
        {
            blendingWorkOrder.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(blendingWorkOrder, this, _blendingRecipeService, _warehouseService) ? _repository.CreateObject(blendingWorkOrder) : blendingWorkOrder);
        }

        public BlendingWorkOrder UpdateObject(BlendingWorkOrder blendingWorkOrder, IBlendingRecipeService _blendingRecipeService, IWarehouseService _warehouseService)
        {
            return (blendingWorkOrder = _validator.ValidUpdateObject(blendingWorkOrder, this, _blendingRecipeService, _warehouseService) ? _repository.UpdateObject(blendingWorkOrder) : blendingWorkOrder);
        }

        public BlendingWorkOrder SoftDeleteObject(BlendingWorkOrder blendingWorkOrder)
        {
            if (_validator.ValidDeleteObject(blendingWorkOrder))
            {
                _repository.SoftDeleteObject(blendingWorkOrder);
            }
            return blendingWorkOrder;
        }

        public BlendingWorkOrder ConfirmObject(BlendingWorkOrder blendingWorkOrder, DateTime ConfirmationDate, IBlendingRecipeService _blendingRecipeService, IBlendingRecipeDetailService _blendingRecipeDetailService,
                                               IStockMutationService _stockMutationService, IBlanketService _blanketService, IItemService _itemService, IItemTypeService _itemTypeService, 
                                               IWarehouseItemService _warehouseItemService, IGeneralLedgerJournalService _generalLedgerJournalService, IAccountService _accountService, IClosingService _closingService)
        {
            blendingWorkOrder.ConfirmationDate = ConfirmationDate;
            if (_validator.ValidConfirmObject(blendingWorkOrder, _blendingRecipeDetailService, _warehouseItemService, _closingService))
            {
                WarehouseItem warehouseItem = null;
                StockMutation stockMutation = null;
                decimal TotalCost = 0;

                // deduce source items
                var details = _blendingRecipeDetailService.GetObjectsByBlendingRecipeId(blendingWorkOrder.BlendingRecipeId);
                foreach (var detail in details)
                {
                    Item itemdet = _itemService.GetObjectById(detail.ItemId);
                    ItemType itemTypeDet = _itemTypeService.GetObjectById(itemdet.ItemTypeId);
                    TotalCost += detail.Quantity * itemdet.AvgPrice;
                    warehouseItem = _warehouseItemService.FindOrCreateObject(blendingWorkOrder.WarehouseId, detail.ItemId);
                    stockMutation = _stockMutationService.CreateStockMutationForBlendingWorkOrderSource(blendingWorkOrder, detail, warehouseItem);
                    _stockMutationService.StockMutateObject(stockMutation, _itemService, _blanketService, _warehouseItemService);
                    _generalLedgerJournalService.CreateConfirmationJournalForBlendingWorkOrderDetail(blendingWorkOrder, itemTypeDet.AccountId.GetValueOrDefault(), detail.Quantity * itemdet.AvgPrice, _accountService);
                }

                // update avg cost of target item before mutated
                BlendingRecipe blendingRecipe = _blendingRecipeService.GetObjectById(blendingWorkOrder.BlendingRecipeId);
                Item item = _itemService.GetObjectById(blendingRecipe.TargetItemId);
                ItemType itemType = _itemTypeService.GetObjectById(item.ItemTypeId);
                _itemService.CalculateAndUpdateAvgPrice(item, blendingRecipe.TargetQuantity, TotalCost / blendingRecipe.TargetQuantity);
                
                // add target item
                warehouseItem = _warehouseItemService.FindOrCreateObject(blendingWorkOrder.WarehouseId, blendingRecipe.TargetItemId);
                stockMutation = _stockMutationService.CreateStockMutationForBlendingWorkOrderTarget(blendingWorkOrder, blendingRecipe, warehouseItem);
                _stockMutationService.StockMutateObject(stockMutation, _itemService, _blanketService, _warehouseItemService);

                // post GL (credit raw, debit finishedgoods)
                _generalLedgerJournalService.CreateConfirmationJournalForBlendingWorkOrder(blendingWorkOrder, itemType.AccountId.GetValueOrDefault(), _accountService, TotalCost);
                
                _repository.ConfirmObject(blendingWorkOrder);
            }
            return blendingWorkOrder;
        }

        public BlendingWorkOrder UnconfirmObject(BlendingWorkOrder blendingWorkOrder, IBlendingRecipeService _blendingRecipeService, IBlendingRecipeDetailService _blendingRecipeDetailService,
                                                 IStockMutationService _stockMutationService, IBlanketService _blanketService, IItemService _itemService, IItemTypeService _itemTypeService, 
                                                 IWarehouseItemService _warehouseItemService, IGeneralLedgerJournalService _generalLedgerJournalService, IAccountService _accountService,
                                                 IClosingService _closingService)
        {
            if (_validator.ValidUnconfirmObject(blendingWorkOrder, _warehouseItemService, _blendingRecipeService, _closingService))
            {
                WarehouseItem warehouseItem = null;
                IList<StockMutation> stockMutations = null;
                decimal TotalCost = 0;

                // add source items
                var details = _blendingRecipeDetailService.GetObjectsByBlendingRecipeId(blendingWorkOrder.BlendingRecipeId);
                foreach (var detail in details)
                {
                    Item itemdet = _itemService.GetObjectById(detail.ItemId);
                    ItemType itemTypeDet = _itemTypeService.GetObjectById(itemdet.ItemTypeId);
                    TotalCost += detail.Quantity * itemdet.AvgPrice;
                    warehouseItem = _warehouseItemService.FindOrCreateObject(blendingWorkOrder.WarehouseId, detail.ItemId);
                    stockMutations = _stockMutationService.GetObjectsBySourceDocumentDetailForWarehouseItem(warehouseItem.Id, Constant.SourceDocumentDetailType.BlendingRecipeDetail, detail.Id);
                    foreach (var x in stockMutations)
                    {
                        _stockMutationService.ReverseStockMutateObject(x, _itemService, _blanketService, _warehouseItemService);
                    }
                    _generalLedgerJournalService.CreateUnconfirmationJournalForBlendingWorkOrderDetail(blendingWorkOrder, itemTypeDet.AccountId.GetValueOrDefault(), detail.Quantity * itemdet.AvgPrice, _accountService);
                }

                // update avg cost of target item before mutated
                BlendingRecipe blendingRecipe = _blendingRecipeService.GetObjectById(blendingWorkOrder.BlendingRecipeId);
                Item item = _itemService.GetObjectById(blendingRecipe.TargetItemId);
                ItemType itemType = _itemTypeService.GetObjectById(item.ItemTypeId);
                _itemService.CalculateAndUpdateAvgPrice(item, (-1)*blendingRecipe.TargetQuantity, TotalCost / blendingRecipe.TargetQuantity);

                // deduce target item
                warehouseItem = _warehouseItemService.FindOrCreateObject(blendingWorkOrder.WarehouseId, blendingRecipe.TargetItemId);
                stockMutations = _stockMutationService.GetObjectsBySourceDocumentDetailForWarehouseItem(warehouseItem.Id, Constant.SourceDocumentDetailType.BlendingRecipe, blendingRecipe.Id);
                foreach (var x in stockMutations)
                {
                    _stockMutationService.ReverseStockMutateObject(x, _itemService, _blanketService, _warehouseItemService);
                }

                // post GL (debit raw, credit finishedgoods)
                _generalLedgerJournalService.CreateUnconfirmationJournalForBlendingWorkOrder(blendingWorkOrder, itemType.AccountId.GetValueOrDefault(), _accountService, TotalCost);
                
                _repository.UnconfirmObject(blendingWorkOrder);
            }
            return blendingWorkOrder;
        }

        public BlendingWorkOrder AdjustQuantity(BlendingWorkOrder blendingWorkOrder)
        {
            return (blendingWorkOrder = _validator.ValidAdjustQuantity(blendingWorkOrder) ? _repository.AdjustQuantity(blendingWorkOrder) : blendingWorkOrder);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsCodeDuplicated(BlendingWorkOrder blendingWorkOrder)
        {
            return _repository.IsCodeDuplicated(blendingWorkOrder);
        }
    }
}