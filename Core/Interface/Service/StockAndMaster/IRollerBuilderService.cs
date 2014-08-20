using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IRollerBuilderService
    {
        IRollerBuilderValidator GetValidator();
        IQueryable<RollerBuilder> GetQueryable();
        IList<RollerBuilder> GetAll();
        IList<RollerBuilder> GetObjectsByCompoundId(int compoundId);
        IList<RollerBuilder> GetObjectsByCoreBuilderId(int coreBuilderId);
        IList<RollerBuilder> GetObjectsByItemId(int ItemId);
        IList<RollerBuilder> GetObjectsByMachineId(int machineId);
        IList<RollerBuilder> GetObjectsByRollerTypeId(int rollerTypeId);
        RollerBuilder GetObjectById(int Id);
        Item GetRollerUsedCore(int id);
        Item GetRollerNewCore(int id);
        RollerBuilder CreateObject(string BaseSku, string SkuRollerNewCore, string SkuRollerUsedCore, string Name, string Category,
                                   int CD, int RD, int RL, int WL, int TL,
                                   IMachineService _machineService, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService,
                                   ICoreBuilderService _coreBuilderService, IRollerTypeService _rollerTypeService,
                                   IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService,
                                   IPriceMutationService _priceMutationService, IContactGroupService _contactGroupService);
        RollerBuilder CreateObject(RollerBuilder rollerBuilder, IMachineService _machineService, IUoMService _uomService, IItemService _itemService,
                                   IItemTypeService _itemTypeService, ICoreBuilderService _coreBuilderService, IRollerTypeService _rollerTypeService,
                                   IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService,
                                   IPriceMutationService _priceMutationService, IContactGroupService _contactGroupService);
        RollerBuilder UpdateObject(RollerBuilder rollerBuilder, IMachineService _machineService, IUoMService _uomService,
                                          IItemService _itemService, IItemTypeService _itemTypeService, ICoreBuilderService _coreBuilderService,
                                          IRollerTypeService _rollerTypeService, IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService,
                                          IBarringService _barringService, IContactService _contactService, IPriceMutationService _priceMutationService,
                                          IContactGroupService _contactGroupService);
        RollerBuilder SoftDeleteObject(RollerBuilder rollerBuilder, IItemService _itemService, IBarringService _barringService, IPriceMutationService _priceMutationService,
                                       IRecoveryOrderDetailService _recoveryOrderDetailService, ICoreBuilderService _coreBuilderService, IWarehouseItemService _warehouseItemService,
                                       IStockMutationService _stockMutationService, IItemTypeService _itemTypeService, IPurchaseOrderDetailService _purchaseOrderDetailService,
                                       IStockAdjustmentDetailService _stockAdjustmentDetailService, ISalesOrderDetailService _salesOrderDetailService);
        bool DeleteObject(int Id);
        bool IsBaseSkuDuplicated(RollerBuilder rollerBuilder);
    }
}