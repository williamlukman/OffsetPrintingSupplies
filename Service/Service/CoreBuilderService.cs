using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public CoreBuilder CreateObject(string BaseSku, string SkuNewCore, string SkuUsedCore, string Name, string Category, IItemService _itemService, IItemTypeService _itemTypeService)
        {
            CoreBuilder coreBuilder = new CoreBuilder
            {
                BaseSku = BaseSku,
                SkuNewCore = SkuNewCore,
                SkuUsedCore = SkuUsedCore,
                Name = Name,
                Category = Category
            };
            return this.CreateObject(coreBuilder, _itemService, _itemTypeService);
        }

        public CoreBuilder CreateObject(CoreBuilder coreBuilder, IItemService _itemService, IItemTypeService _itemTypeService)
        {
            coreBuilder.Errors = new Dictionary<String, String>();
            Item UsedCore = new Item()
            {
                Name = coreBuilder.Name,
                Category = coreBuilder.Category,
                UoM = "pcs",
                Quantity = 0,
                ItemTypeId = _itemTypeService.GetObjectByName(Core.Constants.Constant.ItemTypeCase.Core).Id,
                Sku = coreBuilder.SkuUsedCore
            };
            UsedCore.Errors = new Dictionary<string, string>();

            Item NewCore = new Item()
            {
                Name = coreBuilder.Name,
                Category = coreBuilder.Category,
                UoM = "pcs",
                Quantity = 0,
                ItemTypeId = _itemTypeService.GetObjectByName(Core.Constants.Constant.ItemTypeCase.Core).Id,
                Sku = coreBuilder.SkuNewCore
            };
            NewCore.Errors = new Dictionary<string, string>();

            if (_itemService.GetValidator().ValidCreateObject(UsedCore, _itemService, _itemTypeService) &&
                _itemService.GetValidator().ValidCreateObject(NewCore, _itemService, _itemTypeService))
            {
                if (_validator.ValidCreateObject(coreBuilder, this, _itemService))
                {
                    UsedCore = _itemService.GetRepository().CreateObject(UsedCore);
                    UsedCore.Id = UsedCore.Id;
                    NewCore = _itemService.GetRepository().CreateObject(NewCore);
                    NewCore.Id = NewCore.Id;
                    coreBuilder.UsedCoreItemId = UsedCore.Id;
                    coreBuilder.NewCoreItemId = NewCore.Id;
                    coreBuilder = _repository.CreateObject(coreBuilder);
                }
            }
            else
            {
                coreBuilder.Errors.Add("Generic", "Item tidak dapat di register");
            }
            return coreBuilder;
        }

        public CoreBuilder UpdateObject(CoreBuilder coreBuilder, IItemService _itemService, IItemTypeService _itemTypeService)
        {
            Item UsedCore = _itemService.GetObjectById(coreBuilder.UsedCoreItemId);
            UsedCore.Name = coreBuilder.Name;
            UsedCore.Category = coreBuilder.Category;
            Item NewCore = _itemService.GetObjectById(coreBuilder.NewCoreItemId);
            NewCore.Name = coreBuilder.Name;
            NewCore.Category = coreBuilder.Category;

            if (_itemService.GetValidator().ValidUpdateObject(UsedCore, _itemService, _itemTypeService) &&
                _itemService.GetValidator().ValidUpdateObject(NewCore, _itemService, _itemTypeService))
            {
                if (_validator.ValidUpdateObject(coreBuilder, this, _itemService))
                {
                    _itemService.GetRepository().UpdateObject(UsedCore);
                    _itemService.GetRepository().UpdateObject(NewCore);
                    coreBuilder = _repository.UpdateObject(coreBuilder);
                }
            }
            else
            {
                coreBuilder.Errors.Add("Generic", "Item tidak dapat di update");
            }
            return coreBuilder;
        }

        public CoreBuilder SoftDeleteObject(CoreBuilder coreBuilder, IItemService _itemService, IRollerBuilderService _rollerBuilderService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                            IRecoveryOrderDetailService _recoveryOrderDetailService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService)
        {
            Item UsedCore = _itemService.GetObjectById(coreBuilder.UsedCoreItemId);
            Item NewCore = _itemService.GetObjectById(coreBuilder.NewCoreItemId);

            if (_itemService.GetValidator().ValidDeleteObject(UsedCore, _recoveryOrderDetailService, _recoveryAccessoryDetailService, _rollerBuilderService) &&
                _itemService.GetValidator().ValidDeleteObject(UsedCore, _recoveryOrderDetailService, _recoveryAccessoryDetailService, _rollerBuilderService))
            {
                if (_validator.ValidDeleteObject(coreBuilder, _coreIdentificationDetailService, _rollerBuilderService))
                {
                    _itemService.GetRepository().SoftDeleteObject(UsedCore);
                    _itemService.GetRepository().SoftDeleteObject(NewCore);
                    _repository.SoftDeleteObject(coreBuilder);
                }
            }
            else
            {
                coreBuilder.Errors.Add("Generic", "Item tidak dapat di hapus");
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