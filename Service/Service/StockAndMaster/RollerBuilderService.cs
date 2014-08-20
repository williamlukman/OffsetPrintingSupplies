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
    public class RollerBuilderService : IRollerBuilderService
    {
        private IRollerBuilderRepository _repository;
        private IRollerBuilderValidator _validator;
        public RollerBuilderService(IRollerBuilderRepository _rollerBuilderRepository, IRollerBuilderValidator _rollerBuilderValidator)
        {
            _repository = _rollerBuilderRepository;
            _validator = _rollerBuilderValidator;
        }

        public IRollerBuilderValidator GetValidator()
        {
            return _validator;
        }

        public IList<RollerBuilder> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<RollerBuilder> GetObjectsByCompoundId(int compoundId)
        {
            return _repository.GetObjectsByCompoundId(compoundId);
        }

        public IList<RollerBuilder> GetObjectsByCoreBuilderId(int CoreBuilderId)
        {
            return _repository.GetObjectsByCoreBuilderId(CoreBuilderId);
        }

        public IList<RollerBuilder> GetObjectsByItemId(int ItemId)
        {
            return _repository.GetObjectsByItemId(ItemId);
        }

        public IList<RollerBuilder> GetObjectsByMachineId(int machineId)
        {
            return _repository.GetObjectsByMachineId(machineId);
        }

        public IList<RollerBuilder> GetObjectsByRollerTypeId(int rollerTypeId)
        {
            return _repository.GetObjectsByRollerTypeId(rollerTypeId);
        }

        public RollerBuilder GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public Item GetRollerUsedCore(int Id)
        {
            return _repository.GetRollerUsedCore(Id);
        }

        public Item GetRollerNewCore(int Id)
        {
            return _repository.GetRollerNewCore(Id);
        }

        public RollerBuilder GetObjectByName(string name)
        {
            return _repository.FindAll(x => x.Name == name && !x.IsDeleted).FirstOrDefault();
        }

        public RollerBuilder CreateObject(string BaseSku, string SkuRollerNewCore, string SkuRollerUsedCore, string Name, string Category,
                                          int CD, int RD, int RL, int WL, int TL,
                                          IMachineService _machineService, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService,
                                          ICoreBuilderService _coreBuilderService, IRollerTypeService _rollerTypeService,
                                          IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService,
                                          IPriceMutationService _priceMutationService, IContactGroupService _contactGroupService)
        {
            RollerBuilder rollerBuilder = new RollerBuilder
            {
                BaseSku = BaseSku,
                SkuRollerNewCore = SkuRollerNewCore,
                SkuRollerUsedCore = SkuRollerUsedCore,
                Name = Name,
                Category = Category,
                CD = CD,
                RD = RD,
                RL = RL,
                WL = WL,
                TL = TL
            };
            return this.CreateObject(rollerBuilder, _machineService, _uomService, _itemService, _itemTypeService, _coreBuilderService, _rollerTypeService,
                                     _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);
        }

        public RollerBuilder CreateObject(RollerBuilder rollerBuilder, IMachineService _machineService, IUoMService _uomService,
                                          IItemService _itemService, IItemTypeService _itemTypeService, ICoreBuilderService _coreBuilderService,
                                          IRollerTypeService _rollerTypeService, IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService,
                                          IPriceMutationService _priceMutationService, IContactGroupService _contactGroupService)
        {
            rollerBuilder.Errors = new Dictionary<String, String>();
            ItemType typeRoller = _itemTypeService.GetObjectByName(Core.Constants.Constant.ItemTypeCase.Roller);
            if (typeRoller == null)
            {
                rollerBuilder.Errors.Add("Generic", "ItemType [Roller] Legacy harus dibuat terlebih dahulu");
            }

            Item RollerUsedCore = new Item()
            {
                Name = rollerBuilder.Name,
                Category = rollerBuilder.Category,
                UoMId = rollerBuilder.UoMId,
                Quantity = 0,
                ItemTypeId = _itemTypeService.GetObjectByName(Core.Constants.Constant.ItemTypeCase.Roller).Id,
                Sku = rollerBuilder.SkuRollerUsedCore
            };
            RollerUsedCore.Errors = new Dictionary<string, string>();

            Item RollerNewCore = new Item()
            {
                Name = rollerBuilder.Name,
                Category = rollerBuilder.Category,
                UoMId = rollerBuilder.UoMId,
                Quantity = 0,
                ItemTypeId = _itemTypeService.GetObjectByName(Core.Constants.Constant.ItemTypeCase.Roller).Id,
                Sku = rollerBuilder.SkuRollerNewCore
            };
            RollerNewCore.Errors = new Dictionary<string, string>();

            if (_itemService.GetValidator().ValidCreateLegacyObject(RollerUsedCore, _uomService, _itemService, _itemTypeService) &&
                _itemService.GetValidator().ValidCreateLegacyObject(RollerNewCore, _uomService, _itemService, _itemTypeService))
            {
                if (_validator.ValidCreateObject(rollerBuilder, this, _machineService, _uomService, _itemService, _coreBuilderService, _rollerTypeService))
                {
                    RollerUsedCore = _itemService.CreateLegacyObject(RollerUsedCore, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);
                    RollerUsedCore.Id = RollerUsedCore.Id;
                    RollerNewCore = _itemService.CreateLegacyObject(RollerNewCore, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _priceMutationService, _contactGroupService);
                    RollerNewCore.Id = RollerNewCore.Id;
                    rollerBuilder.RollerUsedCoreItemId = RollerUsedCore.Id;
                    rollerBuilder.RollerNewCoreItemId = RollerNewCore.Id;
                    rollerBuilder = _repository.CreateObject(rollerBuilder);
                }
            }
            else
            {
                if (RollerUsedCore.Errors.Count() > 0) { rollerBuilder.Errors.Add("Generic", RollerUsedCore.Errors.Keys.First() + ": " + RollerUsedCore.Errors.Values.First()); }
                else if (RollerNewCore.Errors.Count() > 0) { rollerBuilder.Errors.Add("Generic", RollerNewCore.Errors.Keys.First() + ": " + RollerNewCore.Errors.Values.First()); }
                else { rollerBuilder.Errors.Add("Generic", "Item tidak dapat di update"); }
            }
            return rollerBuilder;
        }

        public RollerBuilder UpdateObject(RollerBuilder rollerBuilder, IMachineService _machineService, IUoMService _uomService,
                                          IItemService _itemService, IItemTypeService _itemTypeService, ICoreBuilderService _coreBuilderService,
                                          IRollerTypeService _rollerTypeService, IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService,
                                          IBarringService _barringService, IContactService _contactService, IPriceMutationService _priceMutationService,
                                          IContactGroupService _contactGroupService)
        {
            Item RollerUsedCore = _itemService.GetObjectById(rollerBuilder.RollerUsedCoreItemId);
            RollerUsedCore.Name = rollerBuilder.Name;
            RollerUsedCore.Category = rollerBuilder.Category;
            Item RollerNewCore = _itemService.GetObjectById(rollerBuilder.RollerNewCoreItemId);
            RollerNewCore.Name = rollerBuilder.Name;
            RollerNewCore.Category = rollerBuilder.Category;

            if (_itemService.GetValidator().ValidUpdateLegacyObject(RollerUsedCore, _uomService, _itemService, _itemTypeService) &&
                _itemService.GetValidator().ValidUpdateLegacyObject(RollerNewCore, _uomService, _itemService, _itemTypeService))
            {
                if (_validator.ValidUpdateObject(rollerBuilder, this, _machineService, _uomService, _itemService, _coreBuilderService, _rollerTypeService))
                {
                    _itemService.UpdateLegacyObject(RollerUsedCore, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _barringService,
                                                    _contactService, _machineService, _priceMutationService, _contactGroupService);
                    _itemService.UpdateLegacyObject(RollerNewCore, _uomService, _itemTypeService, _warehouseItemService, _warehouseService, _barringService,
                                                    _contactService, _machineService, _priceMutationService, _contactGroupService);
                    rollerBuilder = _repository.UpdateObject(rollerBuilder);
                }
            }
            else
            {
                if (RollerUsedCore.Errors.Count() > 0) { rollerBuilder.Errors.Add(RollerUsedCore.Errors.First().Key, RollerUsedCore.Errors.First().Value); }
                else if (RollerNewCore.Errors.Count() > 0) { rollerBuilder.Errors.Add(RollerNewCore.Errors.First().Key, RollerNewCore.Errors.First().Value); }
                else { rollerBuilder.Errors.Add("Generic", "Item tidak dapat di update"); }
            }
            return rollerBuilder;
        }

        public RollerBuilder SoftDeleteObject(RollerBuilder rollerBuilder, IItemService _itemService, IBarringService _barringService, IPriceMutationService _priceMutationService,
                                              IRecoveryOrderDetailService _recoveryOrderDetailService, ICoreBuilderService _coreBuilderService, IWarehouseItemService _warehouseItemService,
                                              IStockMutationService _stockMutationService, IItemTypeService _itemTypeService, IPurchaseOrderDetailService _purchaseOrderDetailService, 
                                              IStockAdjustmentDetailService _stockAdjustmentDetailService, ISalesOrderDetailService _salesOrderDetailService)
        {
            Item RollerUsedCore = _itemService.GetObjectById(rollerBuilder.RollerUsedCoreItemId);
            Item RollerNewCore = _itemService.GetObjectById(rollerBuilder.RollerNewCoreItemId);

            if (_itemService.GetValidator().ValidDeleteLegacyObject(RollerUsedCore, _stockMutationService, _itemTypeService, _warehouseItemService, _purchaseOrderDetailService, _stockAdjustmentDetailService, _salesOrderDetailService) &&
                _itemService.GetValidator().ValidDeleteLegacyObject(RollerUsedCore, _stockMutationService, _itemTypeService, _warehouseItemService, _purchaseOrderDetailService, _stockAdjustmentDetailService, _salesOrderDetailService))
            {
                if (_validator.ValidDeleteObject(rollerBuilder, _recoveryOrderDetailService))
                {
                    _itemService.SoftDeleteLegacyObject(RollerUsedCore, _stockMutationService, _itemTypeService, _warehouseItemService, _barringService, _purchaseOrderDetailService,
                                                        _stockAdjustmentDetailService, _salesOrderDetailService, _priceMutationService);
                    _itemService.SoftDeleteLegacyObject(RollerNewCore, _stockMutationService, _itemTypeService, _warehouseItemService, _barringService, _purchaseOrderDetailService,
                                                        _stockAdjustmentDetailService, _salesOrderDetailService, _priceMutationService);
                    _repository.SoftDeleteObject(rollerBuilder);
                }
            }
            else
            {
                if (RollerUsedCore.Errors.Count() > 0) { rollerBuilder.Errors.Add(RollerUsedCore.Errors.First().Key, RollerUsedCore.Errors.First().Value); }
                else if (RollerNewCore.Errors.Count() > 0) { rollerBuilder.Errors.Add(RollerNewCore.Errors.First().Key, RollerNewCore.Errors.First().Value); }
                else { rollerBuilder.Errors.Add("Generic", "Item tidak dapat di hapus"); }
            }
            return rollerBuilder;
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsBaseSkuDuplicated(RollerBuilder rollerBuilder)
        {
            IQueryable<RollerBuilder> builders = _repository.FindAll(x => x.BaseSku == rollerBuilder.BaseSku && !x.IsDeleted && x.Id != rollerBuilder.Id);
            return (builders.Count() > 0 ? true : false);
        }
    }
}