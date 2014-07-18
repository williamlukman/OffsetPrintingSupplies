using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface.Service
{
    public interface IRecoveryOrderDetailService
    {
        IRecoveryOrderDetailValidator GetValidator();
        IRecoveryOrderDetailRepository GetRepository();
        IList<RecoveryOrderDetail> GetAll();
        IList<RecoveryOrderDetail> GetObjectsByRecoveryOrderId(int recoveryOrderId);
        IList<RecoveryOrderDetail> GetObjectsByCoreIdentificationDetailId(int coreIdentificationDetailId);
        IList<RecoveryOrderDetail> GetObjectsByRollerBuilderId(int rollerBuilderId);
        RecoveryOrderDetail GetObjectById(int Id);
        Item GetCore(RecoveryOrderDetail recoveryOrderDetail, ICoreIdentificationDetailService _coreIdentificationDetailService,
                     ICoreBuilderService _coreBuilderService, IItemService _itemService);
        Item GetRoller(RecoveryOrderDetail recoveryOrderDetail, ICoreIdentificationDetailService _coreIdentificationDetailService,
                       IRollerBuilderService _rollerBuilderService, IItemService _itemService);
        RecoveryOrderDetail CreateObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService,
                                                ICoreIdentificationDetailService _coreIdentificationDetailService, IRollerBuilderService _rollerBuilderService);
        RecoveryOrderDetail CreateObject(int CoreIdentificationDetailId, int RollerBuilderId, string CoreTypeCase, string Acc, int RepairRequestCase,
                                         IRecoveryOrderService _recoveryOrderService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                         IRollerBuilderService _rollerBuilderService);
        RecoveryOrderDetail UpdateObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService,
                                                ICoreIdentificationDetailService _coreIdentificationDetailService, IRollerBuilderService _rollerBuilderService);
        RecoveryOrderDetail SoftDeleteObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService,
                                             IRecoveryAccessoryDetailService _recoveryAccessoryDetailService);

        RecoveryOrderDetail AddAccessory(RecoveryOrderDetail recoveryOrderDetail, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService);
        RecoveryOrderDetail RemoveAccessory(RecoveryOrderDetail recoveryOrderDetail, IRecoveryAccessoryDetailService _recoveryAccessoryDetailService);
        RecoveryOrderDetail DisassembleObject(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail StripAndGlueObject(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail WrapObject(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail VulcanizeObject(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail FaceOffObject(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail ConventionalGrindObject(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail CWCGrindObject(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail PolishAndQCObject(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail PackageObject(RecoveryOrderDetail recoveryOrderDetail);
        RecoveryOrderDetail RejectObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService);
        RecoveryOrderDetail UndoRejectObject(RecoveryOrderDetail recoveryOrderDetail, IRecoveryOrderService _recoveryOrderService);
        RecoveryOrderDetail FinishObject(RecoveryOrder recoveryOrderDetail, ICoreIdentificationDetailService _coreIdentificationDetailService, IRecoveryOrderService _recoveryOrderService,
                                         IRecoveryAccessoryDetailService _recoveryAccessoryDetailService, ICoreBuilderService _coreBuilderService, IRollerBuilderService _rollerBuilderService,
                                         IItemService _itemService, IWarehouseItemService _warehouseItemService, IBarringService _barringService, IStockMutationService _stockMutationService);
        RecoveryOrderDetail UnfinishObject(RecoveryOrder recoveryOrderDetail, ICoreIdentificationDetailService _coreIdentificationDetailService, IRecoveryOrderService _recoveryOrderService,
                                           IRecoveryAccessoryDetailService _recoveryAccessoryDetailService, ICoreBuilderService _coreBuilderService, IRollerBuilderService _rollerBuilderService,
                                           IItemService _itemService, IWarehouseItemService _warehouseItemService, IBarringService _barringService, IStockMutationService _stockMutationService);
        bool DeleteObject(int Id);
    }
}