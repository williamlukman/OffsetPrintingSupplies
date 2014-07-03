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

        public IList<RollerBuilder> GetObjectsByItemId(int ItemId)
        {
            return _repository.GetObjectsByItemId(ItemId);
        }

        public IList<RollerBuilder> GetObjectsByRollerTypeId(int rollerTypeId)
        {
            return _repository.GetObjectsByRollerTypeId(rollerTypeId);
        }

        public IList<RollerBuilder> GetObjectsByCoreBuilderId(int CoreBuilderId)
        {
            return _repository.GetObjectsByCoreBuilderId(CoreBuilderId);
        }

        public IList<RollerBuilder> GetObjectsByMachineId(int machineId)
        {
            return _repository.GetObjectsByMachineId(machineId);
        }

        public RollerBuilder GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public Item GetUsedRoller(int Id)
        {
            return _repository.GetUsedRoller(Id);
        }

        public Item GetNewRoller(int Id)
        {
            return _repository.GetNewRoller(Id);
        }

        public RollerBuilder GetObjectByName(string name)
        {
            return _repository.FindAll(x => x.Name == name && !x.IsDeleted).FirstOrDefault();
        }

        public RollerBuilder CreateObject(string BaseSku, string SkuNewRoller, string SkuUsedRoller, string Name, string Category,
                                          int CD, int RD, int RL, int WL, int TL,
                                          IMachineService _machineService, IItemService _itemService, IItemTypeService _itemTypeService,
                                          ICoreBuilderService _coreBuilderService, IRollerTypeService _rollerTypeService)
        {
            RollerBuilder rollerBuilder = new RollerBuilder
            {
                BaseSku = BaseSku,
                SkuNewRoller = SkuNewRoller,
                SkuUsedRoller = SkuUsedRoller,
                Name = Name,
                Category = Category,
                CD = CD,
                RD = RD,
                RL = RL,
                WL = WL,
                TL = TL
            };
            return this.CreateObject(rollerBuilder, _machineService, _itemService, _itemTypeService, _coreBuilderService, _rollerTypeService);
        }

        public RollerBuilder CreateObject(RollerBuilder rollerBuilder, IMachineService _machineService, IItemService _itemService, IItemTypeService _itemTypeService,
                                          ICoreBuilderService _coreBuilderService, IRollerTypeService _rollerTypeService)
        {
            rollerBuilder.Errors = new Dictionary<String, String>();
            Item UsedRoller = new Item()
            {
                Name = rollerBuilder.Name,
                Category = rollerBuilder.Category,
                UoM = "pcs",
                Quantity = 0,
                ItemTypeId = _itemTypeService.GetObjectByName(Core.Constants.Constant.ItemTypeCase.Roller).Id,
                Sku = rollerBuilder.SkuUsedRoller
            };

            Item NewRoller = new Item()
            {
                Name = rollerBuilder.Name,
                Category = rollerBuilder.Category,
                UoM = "pcs",
                Quantity = 0,
                ItemTypeId = _itemTypeService.GetObjectByName(Core.Constants.Constant.ItemTypeCase.Roller).Id,
                Sku = rollerBuilder.SkuNewRoller
            };

            if (_itemService.GetValidator().ValidCreateObject(UsedRoller, _itemService, _itemTypeService) &&
                _itemService.GetValidator().ValidCreateObject(NewRoller, _itemService, _itemTypeService))
            {
                rollerBuilder.UsedRollerItemId = UsedRoller.Id;
                rollerBuilder.NewRollerItemId = NewRoller.Id;

                if (_validator.ValidCreateObject(rollerBuilder, this, _machineService, _itemService, _coreBuilderService, _rollerTypeService))
                {
                    UsedRoller = _itemService.GetRepository().CreateObject(UsedRoller);
                    UsedRoller.Id = UsedRoller.Id;
                    NewRoller = _itemService.GetRepository().CreateObject(NewRoller);
                    NewRoller.Id = NewRoller.Id;
                    rollerBuilder = _repository.CreateObject(rollerBuilder);
                }
            }
            else
            {
                rollerBuilder.Errors.Add("Generic", "Item tidak dapat di register");
            }
            return rollerBuilder;
        }

        public RollerBuilder UpdateNameAndCategory(RollerBuilder rollerBuilder, IMachineService _machineService, IItemService _itemService, IItemTypeService _itemTypeService,
                                          ICoreBuilderService _coreBuilderService, IRollerTypeService _rollerTypeService)
        {
            Item UsedRoller = _itemService.GetObjectById(rollerBuilder.UsedRollerItemId);
            UsedRoller.Name = rollerBuilder.Name;
            UsedRoller.Category = rollerBuilder.Category;
            Item NewRoller = _itemService.GetObjectById(rollerBuilder.NewRollerItemId);
            NewRoller.Name = rollerBuilder.Name;
            NewRoller.Category = rollerBuilder.Category;

            if (_itemService.GetValidator().ValidUpdateObject(UsedRoller, _itemService, _itemTypeService) &&
                _itemService.GetValidator().ValidUpdateObject(NewRoller, _itemService, _itemTypeService))
            {
                if (_validator.ValidUpdateObject(rollerBuilder, this, _machineService, _itemService, _coreBuilderService, _rollerTypeService))
                {
                    _itemService.GetRepository().UpdateObject(UsedRoller);
                    _itemService.GetRepository().UpdateObject(NewRoller);
                    rollerBuilder = _repository.UpdateObject(rollerBuilder);
                }
            }
            else
            {
                rollerBuilder.Errors.Add("Generic", "Item tidak dapat di update");
            }
            return rollerBuilder;
        }

        public RollerBuilder UpdateMeasurement(RollerBuilder rollerBuilder)
        {
            return _repository.UpdateObject(rollerBuilder);
        }

        public RollerBuilder SoftDeleteObject(RollerBuilder rollerBuilder, IItemService _itemService, IRecoveryOrderDetailService _recoveryOrderDetailService, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService,
                                              ICoreBuilderService _coreBuilderService)
        {
            Item UsedRoller = _itemService.GetObjectById(rollerBuilder.UsedRollerItemId);
            Item NewRoller = _itemService.GetObjectById(rollerBuilder.NewRollerItemId);

            if (_itemService.GetValidator().ValidDeleteObject(UsedRoller, _recoveryOrderDetailService, _recoveryAccessoryDetailService, _coreBuilderService, this) &&
                _itemService.GetValidator().ValidDeleteObject(UsedRoller, _recoveryOrderDetailService, _recoveryAccessoryDetailService, _coreBuilderService, this))
            {
                if (_validator.ValidDeleteObject(rollerBuilder, _recoveryOrderDetailService))
                {
                    _itemService.GetRepository().SoftDeleteObject(UsedRoller);
                    _itemService.GetRepository().SoftDeleteObject(NewRoller);
                    _repository.SoftDeleteObject(rollerBuilder);
                }
            }
            else
            {
                rollerBuilder.Errors.Add("Generic", "Item tidak dapat di hapus");
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