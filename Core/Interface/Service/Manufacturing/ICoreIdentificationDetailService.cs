using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ICoreIdentificationDetailService
    {
        ICoreIdentificationDetailValidator GetValidator();
        ICoreIdentificationDetailRepository GetRepository();
        IQueryable<CoreIdentificationDetail> GetQueryable();
        IList<CoreIdentificationDetail> GetAll();
        IList<CoreIdentificationDetail> GetObjectsByCoreIdentificationId(int CoreIdentificationId);
        IList<CoreIdentificationDetail> GetObjectsByCoreBuilderId(int CoreBuilderId);
        IList<CoreIdentificationDetail> GetObjectsByRollerTypeId(int rollerTypeId);
        IList<CoreIdentificationDetail> GetObjectsByMachineId(int machineId);
        CoreIdentificationDetail GetObjectById(int Id);
        CoreIdentificationDetail GetObjectByDetailId(int CoreIdentificationId, int DetailId);
        Item GetCore(CoreIdentificationDetail coreIdentificationDetail, ICoreBuilderService _coreBuilderService);
        CoreIdentificationDetail CreateObject(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService,
                                              ICoreBuilderService _coreBuilderService, IRollerTypeService _rollerTypeService, IMachineService _machineService,
                                              IWarehouseItemService _warehouseItemService);
        CoreIdentificationDetail UpdateObject(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService,
                                              ICoreBuilderService _coreBuilderService, IRollerTypeService _rollerTypeService, IMachineService _machineService,
                                              IWarehouseItemService _warehouseItemService);
        CoreIdentificationDetail SoftDeleteObject(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService,
                                                  IRecoveryOrderDetailService _recoveryOrderDetailService, IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService);
        CoreIdentificationDetail SetJobScheduled(CoreIdentificationDetail coreIdentificationDetail, IRecoveryOrderService _recoveryOrderService, IRecoveryOrderDetailService _recoveryOrderDetailService);
        CoreIdentificationDetail UnsetJobScheduled(CoreIdentificationDetail coreIdentificationDetail, IRecoveryOrderService _recoveryOrderService, IRecoveryOrderDetailService _recoveryOrderDetailService);
        CoreIdentificationDetail ConfirmObject(CoreIdentificationDetail coreIdentificationDetail, DateTime FinishedDate, ICoreIdentificationService _coreIdentificationService,
                                              ICoreBuilderService _coreBuilderService, IStockMutationService _stockMutationService,
                                              IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService);
        CoreIdentificationDetail UnconfirmObject(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService,
                                                ICoreBuilderService _coreBuilderService, IStockMutationService _stockMutationService,
                                                IItemService _itemService, IBlanketService _blanketService, IWarehouseItemService _warehouseItemService);
        CoreIdentificationDetail DeliverObject(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService, IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService);
        CoreIdentificationDetail UndoDeliverObject(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService, IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService);
        CoreIdentificationDetail BuildRoller(CoreIdentificationDetail coreIdentificationDetail);
        bool DeleteObject(int Id);
    }
}