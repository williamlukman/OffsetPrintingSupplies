using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ICoreBuilderService
    {
        ICoreBuilderValidator GetValidator();
        IQueryable<CoreBuilder> GetQueryable();
        IList<CoreBuilder> GetAll();
        IList<CoreBuilder> GetObjectsByItemId(int ItemId);
        CoreBuilder GetObjectById(int Id);
        Item GetUsedCore(int id);
        Item GetNewCore(int id);
        CoreBuilder CreateObject(CoreBuilder coreBuilder, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService,
                                 IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService,
                                 IPriceMutationService _priceMutationService, IMachineService _machineService);
        CoreBuilder CreateObject(string BaseSku, string SkuNewCore, string SkuUsedCore, string Name, string Description, int UoMId, int MachineId,
                                 IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService,
                                 IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService,
                                 IPriceMutationService _priceMutationService, IMachineService _machineService);
        CoreBuilder UpdateObject(CoreBuilder coreBuilder, IUoMService _uomService, IItemService _itemService, IItemTypeService _itemTypeService,
                                 IWarehouseItemService _warehouseItemService, IWarehouseService _warehouseService, IBlanketService _blanketService,
                                 IContactService _contactService, IMachineService _machineService, IPriceMutationService _priceMutationService);
        CoreBuilder SoftDeleteObject(CoreBuilder coreBuilder, IItemService _itemService, IRollerBuilderService _rollerBuilderService,
                                     ICoreIdentificationDetailService _coreIdentificationDetailService, IRecoveryOrderDetailService _recoveryOrderDetailService,
                                     IRecoveryAccessoryDetailService _recoveryAccessoryDetailService, IWarehouseItemService _warehouseItemService,
                                     IStockMutationService _stockMutationService, IItemTypeService _itemTypeService, IBlanketService _blanketService,
                                     IPurchaseOrderDetailService _purchaseOrderDetailService, IStockAdjustmentDetailService _stockAdjustmentDetailService,
                                     ISalesOrderDetailService _salesOrderDetailService, IPriceMutationService _priceMutationService, IBlanketOrderDetailService _blanketOrderDetailService);
        bool DeleteObject(int Id);
        bool IsBaseSkuDuplicated(CoreBuilder coreBuilder);
    }
}