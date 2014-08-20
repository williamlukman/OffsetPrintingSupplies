using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface IRollerWarehouseMutationDetailValidator
    {
        RollerWarehouseMutationDetail VHasCoreIdentificationDetail(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, ICoreIdentificationDetailService _coreIdentificationDetailService);
        RollerWarehouseMutationDetail VCoreIdentificationDetailHasNotBeenDelivered(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, ICoreIdentificationDetailService _coreIdentificationDetailService);
        RollerWarehouseMutationDetail VCoreIdentificationDetailHasBeenFinished(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, ICoreIdentificationDetailService _coreIdentificationDetailService);
        RollerWarehouseMutationDetail VHasRollerWarehouseMutation(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService);
        RollerWarehouseMutationDetail VHasWarehouseItemFrom(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService, IWarehouseItemService _warehouseItemService);
        RollerWarehouseMutationDetail VHasWarehouseItemTo(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService, IWarehouseItemService _warehouseItemService);
        RollerWarehouseMutationDetail VUniqueCoreIdentificationDetail(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService);
        RollerWarehouseMutationDetail VNonNegativeStockQuantity(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService,
                                                               IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService, bool CaseConfirm);
        RollerWarehouseMutationDetail VRollerWarehouseMutationHasBeenConfirmed(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService);
        RollerWarehouseMutationDetail VRollerWarehouseMutationHasNotBeenConfirmed(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService);
        RollerWarehouseMutationDetail VHasNotBeenConfirmed(RollerWarehouseMutationDetail rollerWarehouseMutationDetail);
        RollerWarehouseMutationDetail VHasBeenConfirmed(RollerWarehouseMutationDetail rollerWarehouseMutationDetail);
        RollerWarehouseMutationDetail VCreateObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                                   IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        RollerWarehouseMutationDetail VUpdateObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                                                   IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        RollerWarehouseMutationDetail VDeleteObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail);
        RollerWarehouseMutationDetail VHasConfirmationDate(RollerWarehouseMutationDetail rollerWarehouseMutationDetail);
        RollerWarehouseMutationDetail VConfirmObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService,
                                                    IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService);
        RollerWarehouseMutationDetail VUnconfirmObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService,
                                                      IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService);
        bool ValidCreateObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                               IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        bool ValidUpdateObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService, ICoreIdentificationDetailService _coreIdentificationDetailService,
                               IRollerWarehouseMutationDetailService _rollerWarehouseMutationDetailService, IItemService _itemService, IWarehouseItemService _warehouseItemService);
        bool ValidConfirmObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService,
                                IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService);
        bool ValidUnconfirmObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail, IRollerWarehouseMutationService _rollerWarehouseMutationService,
                                IItemService _itemService, IBarringService _barringService, IWarehouseItemService _warehouseItemService);
        bool ValidDeleteObject(RollerWarehouseMutationDetail rollerWarehouseMutationDetail);
        bool isValid(RollerWarehouseMutationDetail rollerWarehouseMutationDetail);
        string PrintError(RollerWarehouseMutationDetail rollerWarehouseMutationDetail);
    }
}