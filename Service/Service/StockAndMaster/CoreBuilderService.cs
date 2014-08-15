using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Service
{
    public class CoreBuilderService : ICoreBuilderService
    {
        private ICoreBuilderRepository _repository;
        private ICoreBuilderValidator _validator;
        public CoreBuilderService(ICoreBuilderRepository _coreBuilderRepository, ICoreBuilderValidator _coreBuilderValidator)
        {
            _repository = _coreBuilderRepository;
            _validator = _coreBuilderValidator;
        }

        public ICoreBuilderValidator GetValidator()
        {
            return _validator;
        }

        public IList<CoreBuilder> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<CoreBuilder> GetObjectsByItemId(int ItemId)
        {
            return _repository.GetObjectsByItemId(ItemId);
        }

        public CoreBuilder GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public Item GetUsedCore(int Id)
        {
            return _repository.GetUsedCore(Id);
        }

        public Item GetNewCore(int Id)
        {
            return _repository.GetNewCore(Id);
        }

        public CoreBuilder CreateObject(string BaseSku, string SkuNewCore, string SkuUsedCore, string Name, string Category, int UoMId, IUoMService _uomService,
                                        IItemService _itemService, IItemTypeService _itemTypeService,
                                        IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService,
                                        IPriceMutationService _priceMutationService, IContactGroupService _contactGroupService)
        {
            CoreBuilder coreBuilder = new CoreBuilder
            {
                BaseSku = BaseSku,
                SkuNewCore = SkuNewCore,
                SkuUsedCore = SkuUsedCore,
                Name = Name,
                Category = Category,
                UoMId = UoMId
            };
            return this.CreateObject(coreBuilder, _uomService, _itemService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);
        }

        public CoreBuilder CreateObject(CoreBuilder coreBuilder, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService,
                                        IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService, 
                                        IPriceMutationService _priceMutationService, IContactGroupService _contactGroupService)
        {
            coreBuilder.Errors = new Dictionary<String, String>();

            ItemType typeCore = _itemTypeService.GetObjectByName(Core.Constants.Constant.ItemTypeCase.Core);
            if (typeCore == null)
            {
                coreBuilder.Errors.Add("Generic", "ItemType [Core] Legacy harus dibuat terlebih dahulu");
            }

            Item UsedCore = new Item()
            {
                Name = coreBuilder.Name,
                Category = coreBuilder.Category,
                UoMId = coreBuilder.UoMId,
                Quantity = 0,
                ItemTypeId = typeCore.Id,
                Sku = coreBuilder.SkuUsedCore
            };
            UsedCore.Errors = new Dictionary<string, string>();

            Item NewCore = new Item()
            {
                Name = coreBuilder.Name,
                Category = coreBuilder.Category,
                UoMId = coreBuilder.UoMId,
                Quantity = 0,
                ItemTypeId = typeCore.Id,
                Sku = coreBuilder.SkuNewCore
            };
            NewCore.Errors = new Dictionary<string, string>();

            if (_itemService.GetValidator().ValidCreateLegacyObject(UsedCore, _uomService, _itemService, _itemTypeService) &&
                _itemService.GetValidator().ValidCreateLegacyObject(NewCore, _uomService, _itemService, _itemTypeService))
            {
                if (_validator.ValidCreateObject(coreBuilder, this, _uomService, _itemService))
                {
                    UsedCore = _itemService.CreateLegacyObject(UsedCore, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);
                    UsedCore.Id = UsedCore.Id;
                    NewCore = _itemService.CreateLegacyObject(NewCore, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);
                    NewCore.Id = NewCore.Id;
                    coreBuilder.UsedCoreItemId = UsedCore.Id;
                    coreBuilder.NewCoreItemId = NewCore.Id;
                    coreBuilder = _repository.CreateObject(coreBuilder);
                }
            }
            else
            {
                if (UsedCore.Errors.Count() > 0) { coreBuilder.Errors.Add(UsedCore.Errors.First().Key, UsedCore.Errors.First().Value); }
                else if (NewCore.Errors.Count() > 0) { coreBuilder.Errors.Add(NewCore.Errors.First().Key, NewCore.Errors.First().Value); }
                else { coreBuilder.Errors.Add("Generic", "Item tidak dapat di register"); }
            }
            return coreBuilder;
        }

        public CoreBuilder UpdateObject(CoreBuilder coreBuilder, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService,
                                        IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService, IBarringService _barringService,
                                        IContactService _contactService, IMachineService _machineService, IPriceMutationService _priceMutationService,
                                        IContactGroupService _contactGroupService)
        {
            Item UsedCore = _itemService.GetObjectById(coreBuilder.UsedCoreItemId);
            UsedCore.Name = coreBuilder.Name;
            UsedCore.Category = coreBuilder.Category;
            Item NewCore = _itemService.GetObjectById(coreBuilder.NewCoreItemId);
            NewCore.Name = coreBuilder.Name;
            NewCore.Category = coreBuilder.Category;

            if (_itemService.GetValidator().ValidUpdateLegacyObject(UsedCore, _uomService, _itemService, _itemTypeService) &&
                _itemService.GetValidator().ValidUpdateLegacyObject(NewCore, _uomService, _itemService, _itemTypeService))
            {
                if (_validator.ValidUpdateObject(coreBuilder, this, _uomService, _itemService))
                {
                    _itemService.UpdateLegacyObject(UsedCore, _uomService, _itemTypeService, _warehouseItemService, _warehouseService,
                                                    _barringService, _contactService, _machineService, _priceMutationService, _contactGroupService);
                    _itemService.UpdateLegacyObject(NewCore, _uomService, _itemTypeService, _warehouseItemService, _warehouseService,
                                                    _barringService, _contactService, _machineService, _priceMutationService, _contactGroupService);
                    coreBuilder = _repository.UpdateObject(coreBuilder);
                }
            }
            else
            {
                if (UsedCore.Errors.Count() > 0) { coreBuilder.Errors.Add(UsedCore.Errors.First().Key, UsedCore.Errors.First().Value); }
                else if (NewCore.Errors.Count() > 0) { coreBuilder.Errors.Add(NewCore.Errors.First().Key, NewCore.Errors.First().Value); }
                else { coreBuilder.Errors.Add("Generic", "Item tidak dapat di update"); }
            }
            return coreBuilder;
        }

        public CoreBuilder SoftDeleteObject(CoreBuilder coreBuilder, IItemService _itemService, IRollerBuilderService _rollerBuilderService,
                                            ICoreIdentificationDetailService _coreIdentificationDetailService, IRecoveryOrderDetailService _recoveryOrderDetailService,
                                            IRecoveryAccessoryDetailService _recoveryAccessoryDetailService, IWarehouseItemService _warehouseItemService,
                                            IStockMutationService _stockMutationService, IItemTypeService _itemTypeService, IBarringService _barringService,
                                            IPurchaseOrderDetailService _purchaseOrderDetailService, IStockAdjustmentDetailService _stockAdjustmentDetailService,
                                            ISalesOrderDetailService _salesOrderDetailService, IPriceMutationService _priceMutationService)
        {
            Item UsedCore = _itemService.GetObjectById(coreBuilder.UsedCoreItemId);
            Item NewCore = _itemService.GetObjectById(coreBuilder.NewCoreItemId);

            if (_itemService.GetValidator().ValidDeleteLegacyObject(UsedCore, _stockMutationService, _itemTypeService, _warehouseItemService, _purchaseOrderDetailService, _stockAdjustmentDetailService, _salesOrderDetailService) &&
                _itemService.GetValidator().ValidDeleteLegacyObject(UsedCore, _stockMutationService, _itemTypeService, _warehouseItemService, _purchaseOrderDetailService, _stockAdjustmentDetailService, _salesOrderDetailService))
            {
                if (_validator.ValidDeleteObject(coreBuilder, _coreIdentificationDetailService, _rollerBuilderService))
                {
                    _itemService.SoftDeleteLegacyObject(UsedCore, _stockMutationService, _itemTypeService, _warehouseItemService, _barringService, _purchaseOrderDetailService,
                                                        _stockAdjustmentDetailService, _salesOrderDetailService, _priceMutationService);
                    _itemService.SoftDeleteLegacyObject(NewCore, _stockMutationService, _itemTypeService, _warehouseItemService, _barringService, _purchaseOrderDetailService,
                                                        _stockAdjustmentDetailService, _salesOrderDetailService, _priceMutationService);
                    _repository.SoftDeleteObject(coreBuilder);
                }
            }
            else
            {
                if (UsedCore.Errors.Count() > 0) { coreBuilder.Errors.Add(UsedCore.Errors.First().Key, UsedCore.Errors.First().Value); }
                else if (NewCore.Errors.Count() > 0) { coreBuilder.Errors.Add(NewCore.Errors.First().Key, NewCore.Errors.First().Value); }
                else { coreBuilder.Errors.Add("Generic", "Item tidak dapat di hapus"); }
            }
            return coreBuilder;
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsBaseSkuDuplicated(CoreBuilder coreBuilder)
        {
            IQueryable<CoreBuilder> coreBuilders = _repository.FindAll(x => x.BaseSku == coreBuilder.BaseSku && !x.IsDeleted && x.Id != coreBuilder.Id);
            return (coreBuilders.Count() > 0 ? true : false);
        }
    }
}