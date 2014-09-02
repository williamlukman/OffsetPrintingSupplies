using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface ICoreIdentificationDetailValidator
    {
        CoreIdentificationDetail VHasCoreIdentification(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService);
        CoreIdentificationDetail VDetailsDoNotExceedCoreIdentificationQuantity(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService,
                                                                               ICoreIdentificationDetailService _coreIdentificationDetailService, bool CaseCreate);
        CoreIdentificationDetail VHasUniqueDetailId(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationDetailService _coreIdentificationDetailService);
        CoreIdentificationDetail VHasMaterialCase(CoreIdentificationDetail coreIdentificationDetail);
        CoreIdentificationDetail VHasCoreBuilder(CoreIdentificationDetail coreIdentificationDetail, ICoreBuilderService _coreBuilderService);
        CoreIdentificationDetail VHasRollerType(CoreIdentificationDetail coreIdentificationDetail, IRollerTypeService _rollerTypeService);
        CoreIdentificationDetail VHasMachine(CoreIdentificationDetail coreIdentificationDetail, IMachineService _machineService);
        CoreIdentificationDetail VHasRepairRequestCase(CoreIdentificationDetail coreIdentificationDetail);
        CoreIdentificationDetail VHasMeasurement(CoreIdentificationDetail coreIdentificationDetail);
        CoreIdentificationDetail VIsInRecoveryOrderDetails(CoreIdentificationDetail coreIdentificationDetail, IRecoveryOrderDetailService _recoveryOrderDetailService);
        CoreIdentificationDetail VCoreIdentificationHasBeenConfirmed(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService);
        CoreIdentificationDetail VCoreIdentificationHasNotBeenConfirmed(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService);
        CoreIdentificationDetail VCoreIdentificationHasNotBeenCompleted(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService);
        CoreIdentificationDetail VHasBeenJobScheduled(CoreIdentificationDetail coreIdentificationDetail);
        CoreIdentificationDetail VHasNotBeenJobScheduled(CoreIdentificationDetail coreIdentificationDetail);
        CoreIdentificationDetail VHasBeenDelivered(CoreIdentificationDetail coreIdentificationDetail);
        CoreIdentificationDetail VHasNotBeenDelivered(CoreIdentificationDetail coreIdentificationDetail);
        CoreIdentificationDetail VHasBeenRollerBuilt(CoreIdentificationDetail coreIdentificationDetail);
        CoreIdentificationDetail VHasNotBeenConfirmed(CoreIdentificationDetail coreIdentificationDetail);
        CoreIdentificationDetail VHasConfirmationDate(CoreIdentificationDetail coreIdentificationDetail);
        CoreIdentificationDetail VQuantityIsInStockForInHouse(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService,
                                                              ICoreBuilderService _coreBuilderService, IWarehouseItemService _warehouseItemService);
        CoreIdentificationDetail VQuantityIsInStockForContact(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService,
                                                              ICoreBuilderService _coreBuilderService, IWarehouseItemService _warehouseItemService);
        CoreIdentificationDetail VCreateObject(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService,
                                               ICoreIdentificationDetailService _coreIdentificationDetailService, ICoreBuilderService _coreBuilderService,
                                                IRollerTypeService _rollerTypeService, IMachineService _machineService, IWarehouseItemService _warehouseItemService);
        CoreIdentificationDetail VUpdateObject(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService,
                                               ICoreIdentificationDetailService _coreIdentificationDetailService, ICoreBuilderService _coreBuilderService,
                                                IRollerTypeService _rollerTypeService, IMachineService _machineService, IWarehouseItemService _warehouseItemService);
        CoreIdentificationDetail VDeleteObject(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService,
                                               IRecoveryOrderDetailService _recoveryOrderDetailService, IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService);
        CoreIdentificationDetail VConfirmObject(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService,
                                               ICoreIdentificationDetailService _coreIdentificationDetailService, ICoreBuilderService _coreBuilderService, IWarehouseItemService _warehouseItemService);
        CoreIdentificationDetail VUnconfirmObject(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService,
                                                 ICoreIdentificationDetailService _coreIdentificationDetailService, ICoreBuilderService _coreBuilderService, IWarehouseItemService _warehouseItemService);
        CoreIdentificationDetail VSetJobScheduled(CoreIdentificationDetail coreIdentificationDetail, IRecoveryOrderService _recoveryOrderService, IRecoveryOrderDetailService _recoveryOrderDetailService);
        CoreIdentificationDetail VUnsetJobScheduled(CoreIdentificationDetail coreIdentificationDetail, IRecoveryOrderService _recoveryOrderService, IRecoveryOrderDetailService _recoveryOrderDetailService);
        CoreIdentificationDetail VDeliverObject(CoreIdentificationDetail coreIdentificationDetail, IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService);
        CoreIdentificationDetail VUndoDeliverObject(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService, IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService);

        bool ValidCreateObject(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService,
                               ICoreIdentificationDetailService _coreIdentificationDetailService, ICoreBuilderService _coreBuilderService,
                               IRollerTypeService _rollerTypeService, IMachineService _machineService, IWarehouseItemService _warehouseItemService);
        bool ValidUpdateObject(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService,
                               ICoreIdentificationDetailService _coreIdentificationDetailService, ICoreBuilderService _coreBuilderService,
                               IRollerTypeService _rollerTypeService, IMachineService _machineService, IWarehouseItemService _warehouseItemService);
        bool ValidDeleteObject(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService,
                               IRecoveryOrderDetailService _recoveryOrderDetailService, IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService);
        bool ValidConfirmObject(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService,
                               ICoreIdentificationDetailService _coreIdentificationDetailService, ICoreBuilderService _coreBuilderService, IWarehouseItemService _warehouseItemService);
        bool ValidUnconfirmObject(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService,
                                 ICoreIdentificationDetailService _coreIdentificationDetailService, ICoreBuilderService _coreBuilderService, IWarehouseItemService _warehouseItemService);
        bool ValidSetJobScheduled(CoreIdentificationDetail coreIdentificationDetail, IRecoveryOrderService _recoveryOrderService, IRecoveryOrderDetailService _recoveryOrderDetailService);
        bool ValidUnsetJobScheduled(CoreIdentificationDetail coreIdentificationDetail, IRecoveryOrderService _recoveryOrderService, IRecoveryOrderDetailService _recoveryOrderDetailService);
        bool ValidDeliverObject(CoreIdentificationDetail coreIdentificationDetail, IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService);
        bool ValidUndoDeliverObject(CoreIdentificationDetail coreIdentificationDetail, ICoreIdentificationService _coreIdentificationService, IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService);
        bool isValid(CoreIdentificationDetail coreIdentificationDetail);
        string PrintError(CoreIdentificationDetail coreIdentificationDetail);
    }
}